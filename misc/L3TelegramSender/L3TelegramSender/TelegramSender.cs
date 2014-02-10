using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using ProconTel.CommunicationCenter.Kernel;
using ProconTel.Logging;

namespace L3TelegramSender
{
  /// <summary>
  /// Represents plugin's strategy that is responsible for providing content for other communication partner.
  /// </summary>
  [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
  public class TelegramSender : ProviderStrategyBase<Endpoint>, ITelegramSender
  {
    private const string XsbtDirectory = @"C:\Users\wm\Documents\Developer Studio\slpps\deploy";
    private const string DBconnectionString = "Data Source=PMCDB; User Id=PMC; Password=PMC";

    /// <summary>
    /// Stores key mapping for telegrams where the key of the dictionary is a telegram name and a value is a list of field-key mapping.
    /// </summary>
    private readonly Dictionary<string, List<TelegramKeyMapping>> _keyMappings = new Dictionary<string, List<TelegramKeyMapping>>();

    public const string SqlSingleTableHandlerInsert =
     @"INSERT INTO TCP_TEL_HIST ({7}ID, DATA, ROUTE, ID_CHAN, ID_ORIG, HANDLING_TIME, PARTNER_ADDRESS) VALUES ({8}{0}, {1}, {2}, {3}, {4}, {5}, {6}) returning ID_SEQ into :id";

    private const string UpdateArchiveFlagCmd = "UPDATE TCP_TEL_HIST t SET t.archive = :flag WHERE id_seq = :idseq";

    public override IEnumerable<ContentDetails> ProvidingContentDetails
    {
      get
      {
        var xsbtfiles = Directory.GetFiles(XsbtDirectory, "*.xsbt.xml");
        return xsbtfiles
          .Select(f =>
                    {
                      var d = XDocument.Load(f);
                      return GetTelegramDetailsFromXsbt(d);
                    })
          .Where(i => !String.IsNullOrWhiteSpace(i.Id))
          .ToArray();
      }
    }

    /// <summary>
    /// Initializes this instance. Implement here initialization steps for this strategy. For details, see Remarks.
    /// </summary>
    /// <remarks>
    /// This method is invoked automatically after <see cref="M:ProconTel.CommunicationCenter.Kernel.ChannelEndpointBase.Initialize"/> method.
    /// </remarks>
    /// <exception cref="T:ProconTel.CommunicationCenter.Kernel.EndpointInitializationException">Thrown when an error occurs during endpoint initialization.</exception>
    public override void Initialize()
    {
      ReadKeyMappingFiles();
    }

    /// <summary>
    /// Terminates this instance. Implement here termination steps for this strategy. For details, see Remarks.
    /// </summary>
    /// <remarks>
    /// This method is invoked automatically before <see cref="M:ProconTel.CommunicationCenter.Kernel.ChannelEndpointBase.Terminate"/> method.
    /// </remarks>
    public override void Terminate()
    {
      Logger.Debug(this, "Terminated.");
    }
    
    protected override void OnContentAcknowledged(ContentAcknowledgment acknowledgment)
    {      
      if (acknowledgment.Result == ContentProcessingResult.Processed)
      {
        var idseq = acknowledgment.Metadata["ID_SEQ"];
        Logger.Information(this,
                           String.Format("The content has been acknowledged: ID_SEQ: {0}", idseq));
        var archivingSucceeded = SetTelegramArchiveFlag(Convert.ToInt32(idseq), true);
        if (archivingSucceeded)
          Logger.Debug(string.Format("Archive flag of telegram with ID_SEQ: {0} successfully set",idseq));
        else
          Logger.Warning(string.Format("Archive flag of telegram with ID_SEQ: {0} was not changed", idseq));
      }
      else
      {
        Logger.Warning(this, String.Format("The content was acknowledged as {0}. Message: \"{1}\"", acknowledgment.Result, acknowledgment.Message));
      }
    }

    public void SendTelegram(string xml)
    {
      try
      {
        var d = XDocument.Parse(xml);
        string telegramId = d.Element("Telegram").Attribute("ID").Value;
        string telegramName = d.Element("Telegram").Attribute("Name").Value;

        var seqId = StoreXml(d.ToString());

        BroadcastContent(telegramId, xml, new XmlProtocol(), new Dictionary<string, object>
                                                               {
                                                                 {"ID_SEQ", seqId},
                                                                 {"TELEGRAM_NAME", telegramName},
                                                                 {"TELEGRAM_ID", telegramId}
                                                               });
        Logger.Information(this, String.Format("The telegram {0} ({1}) has been stored in the database with ID_SEQ value set to {2} and broadcast to the channel.", telegramName, telegramId, seqId));
      }
      catch (Exception e)
      {
        Logger.Error(this, e);
      }
    }

    private ContentDetails GetTelegramDetailsFromXsbt(XDocument d)
    {
      var telegramElement = d.Element("Telegram");
      return new ContentDetails()
               {
                 Id = telegramElement.Attribute("ID").Value,
                 ProviderId = Endpoint.Id,
                 ProviderName = Endpoint.Name,
                 Caption =
                   String.Format("{0}: {1}", telegramElement.Attribute("name").Value,
                                 telegramElement.Attribute("ID").Value)
               };
    }

    private void GetTelegramDetails(XDocument d, out string id, out string name)
    {
      var telegramElement = d.Element("Telegram");
      id = telegramElement.Attribute("ID").Value;
      name = telegramElement.Attribute("Name").Value;
    }


    protected virtual int StoreXml(string xml)
    {
      var handlingTime = DateTime.Now;
      var partnerAddress = GetClientAdress();

      string telegramName;
      string telegramId;
      var doc = XDocument.Parse(xml);
      GetTelegramDetails(doc, out telegramId, out telegramName);

      List<OracleParameter> insertSqlParameters;
      {
        string columnNames;
        OracleParameter[] columnParameters;
        GenerateKeyColumns(telegramName, doc, out columnNames, out columnParameters);

        insertSqlParameters = new List<OracleParameter>();
        //The order in which parameters are added seems to matter!
        insertSqlParameters.AddRange(columnParameters);
        insertSqlParameters.Add(new OracleParameter("telegram_id", OracleDbType.Int32) { Value = telegramId });
        insertSqlParameters.Add(new OracleParameter("xml", OracleDbType.Clob) { Value = xml });
        insertSqlParameters.Add(new OracleParameter("direction", OracleDbType.Int32) { Value = 1 });
        insertSqlParameters.Add(new OracleParameter("channel_name", OracleDbType.Varchar2) { Value = this.Endpoint.Channel.Name });
        insertSqlParameters.Add(new OracleParameter("provider_id", OracleDbType.Varchar2) { Value = partnerAddress });
        insertSqlParameters.Add(new OracleParameter("handling_time", OracleDbType.TimeStamp) { Value = handlingTime });
        insertSqlParameters.Add(new OracleParameter("partner_address", OracleDbType.Varchar2) { Value = partnerAddress });
        insertSqlParameters.Add(new OracleParameter("id", OracleDbType.Int32, System.Data.ParameterDirection.Output));

        var parametersString = String.Join(",", columnParameters.Select(p => ":" + p.ParameterName).ToArray());

        if (!String.IsNullOrWhiteSpace(columnNames))
          columnNames += ",";
        if (!String.IsNullOrWhiteSpace(parametersString))
          parametersString += ",";

        string insertClause = String.Format(
          SqlSingleTableHandlerInsert,
          ":telegram_id",
          ":xml",
          ":direction",
          ":channel_name",
          ":provider_id",
          ":handling_time",
          ":partner_address",
          columnNames,
          parametersString);

        using (var cn = new OracleConnection(DBconnectionString))
        {
          cn.Open();
          using (OracleCommand command = new OracleCommand(insertClause, cn))
          {
            command.BindByName = true;
            command.Parameters.AddRange(insertSqlParameters.ToArray());
            command.ExecuteNonQuery();
            int id = ((OracleDecimal)command.Parameters["id"].Value).ToInt32();
            return id;
          }
        }
      }
    }

    private string GetClientAdress()
    {
      OperationContext context = OperationContext.Current;
      MessageProperties messageProperties = context.IncomingMessageProperties;
      var endpointProperty =
          (RemoteEndpointMessageProperty)messageProperties[RemoteEndpointMessageProperty.Name];

      return string.Format("{0}:{1}", endpointProperty.Address, endpointProperty.Port);
    }

    private void GenerateKeyColumns(string telegramName, XDocument telegram, out string columnNames, out OracleParameter[] columnParameters)
    {
      columnNames = null;
      var columnParameterList = new List<OracleParameter>();
      if (_keyMappings.ContainsKey(telegramName))
      {
        var mappings = _keyMappings[telegramName];

        for (int index = 0; index < mappings.Count; index++)
        {
          var m = mappings[index];
          if (columnNames != null)
            columnNames += ",";
          columnNames += m.KeyColumnName;
          var fieldElement = telegram.Element("Telegram").Element(m.FieldName);
          if (fieldElement != null)
            columnParameterList.Add(new OracleParameter("key_" + (index + 1), OracleDbType.Varchar2) { Value = fieldElement.Value });
        }
      }
      columnParameters = columnParameterList.ToArray();
    }



    /// <summary>
    /// Reads key mapping files and ads them to the endpoint dictionary.
    /// </summary>
    private void ReadKeyMappingFiles()
    {
      Logger.Information(this, "Reading key mapping files.");
      _keyMappings.Clear();
      string[] mappingFiles = Directory.GetFiles(XsbtDirectory, "*.kmap.xml");
      foreach (var fileName in mappingFiles)
      {
        var d = XDocument.Load(fileName);
        var telegramElement = d.Element("Telegram");
        if (telegramElement != null)
        {
          var telegramAttribute = telegramElement.Attribute("name");
          if (telegramAttribute != null)
          {
            string telegramName = telegramAttribute.Value;
            var fieldMappings = new List<TelegramKeyMapping>();
            foreach (var fieldElement in telegramElement.Elements("element"))
            {
              var fieldNameAttribute = fieldElement.Attribute("name");
              var keyColumnAttribute = fieldElement.Attribute("keyMapping");
              if (fieldNameAttribute != null && keyColumnAttribute != null)
              {
                fieldMappings.Add(new TelegramKeyMapping
                {
                  FieldName = fieldNameAttribute.Value,
                  KeyColumnName = keyColumnAttribute.Value
                });
                Logger.Information(this,
                                   String.Format(
                                     "Telegram key mapping found - Telegram: {0}, Field Name: {1}, Key Column: {2}.",
                                     telegramName, fieldNameAttribute.Value, keyColumnAttribute.Value));
              }
              else
                Logger.Warning(this, String.Format("The file {0} does not contain a valid mapping element.", fileName));
            }
            _keyMappings.Add(telegramName, fieldMappings);
          }
          else
            Logger.Warning(this, String.Format("The file {0} does not contain a valid \"Telegram\" element. The \"Name\" attribute is missing.", fileName));
        }
        else
          Logger.Warning(this, String.Format("The file {0} does not contain a \"Telegram\" element.", fileName));
      }
    }

    private static bool SetTelegramArchiveFlag(int idSeq, bool flagValue)
    {
      using (var cn = new OracleConnection(DBconnectionString))
      {
        cn.Open();
        using (OracleCommand command = new OracleCommand(UpdateArchiveFlagCmd, cn))
        {
          command.Parameters.Add("flag", flagValue ? 1 : 0);
          command.Parameters.Add("idseq", idSeq);
          return command.ExecuteNonQuery()==1;
        }
      }
    }

    private class TelegramKeyMapping
    {
      public string FieldName { get; set; }
      public string KeyColumnName { get; set; }
    }
  }


  [Serializable]
  public class MyOwnContentInfo
  {
    public string Test { get; set; }
  }
}
