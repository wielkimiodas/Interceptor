using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using SampleTOG.TelegramSenderService;


namespace SampleTOG
{
  class Program
  {
    static void Main(string[] args)
    {
      string tel1 = GetXmlFromResource("SL_PPS_REQUEST");
      using(TelegramSenderClient client = new TelegramSenderClient())
      {
        client.SendTelegram(tel1);
      }
    }

    private static string GetXmlFromResource(string name)
    {
      var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SampleTOG.Telegrams." + name + ".xml");
      var d = XDocument.Load(stream);
      return d.ToString();
    }
  }
}
