using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using ProconTel.CommunicationCenter.Kernel;
using ProconTel.Logging;
using L3TelegramSender.View;

namespace L3TelegramSender
{
  /// <summary>
  /// Represents the endpoint of this plugin.
  /// </summary>
  [Endpoint(Name = "L3 Telegram Sender", SupportedRoles = SupportedRoles.Provider)]
  public class Endpoint : ChannelEndpointBase
  {
    private ServiceHost _host;

    /// <summary>
    /// Initializes a new instance of this endpoint. This method is invoked when channel endpoint is activated. For details, see Remarks.
    /// </summary>
    /// <remarks>Override this method to perform operations for initializing.
    /// After this method Initialize() methods on all strategies are invoked.</remarks>
    /// <example>
    /// This is an example for using initialize method.
    /// <code>
    /// public void Initialize()
    /// {
    /// InitializeConnection();
    /// }
    /// </code>
    /// </example>
    /// <exception cref="T:ProconTel.CommunicationCenter.Kernel.EndpointInitializationException">Thrown when an error occurs during endpoint initialization.</exception>
    public override void Initialize()
    {
      _host = CreateHost(9005, _providerStrategy, typeof(ITelegramSender), "TelegramSender");
      foreach(var address in _host.BaseAddresses)
      Logger.Information(String.Format("The Telegram Sender Service is available on address {0}", address));
    }

    /// <summary>
    /// Terminates current instance of this endpoint. This method is invoked when channel endpoint is deactivated.
    /// Override this method to destroy your endpoint. For details, see Remarks.
    /// </summary>
    /// <remarks>
    /// After this method Terminate() methods on all strategies are invoked.</remarks>
    /// <example>
    /// This is an example for using termination method.
    /// <code>
    /// public void Terminate()
    /// {
    /// Disconnect();
    /// }
    /// </code>
    /// </example>
    /// <exception cref="T:ProconTel.CommunicationCenter.Kernel.EndpointRuntimeException">Thrown when an error occurs during endpoint lifetime.</exception>
    public override void Terminate()
    {
      _host.Close();
    }

    /// <summary>
    /// Instantiate an implementation of the <see cref="T:ProconTel.CommunicationCenter.Kernel.ProviderStrategyBase"/> class here
    /// if you want your endpoint to provide contents for content subscribing endpoints in the same channel.
    /// Return null if your endpoint does not support provider role. For details, see Remarks.
    /// </summary>
    /// <returns></returns>
    /// <remarks>
    /// This method is called everytime this endpoint is instantiated. Do not perform initialization of strategy here.
    /// </remarks>
    /// <example>
    /// 	<code>
    /// public ProviderStrategyBase InstantiateProviderStrategy()
    /// {
    /// return new ProviderStrategyImpl();
    /// }
    /// </code>
    /// </example>
    /// <value>The subscriber strategy of this endpoint.</value>
    public override ProviderStrategyBase InstantiateProviderStrategy()
    {
        return new TelegramSender();
    }

    /// <summary>
    /// Instantiate an implementation of the <see cref="T:ProconTel.CommunicationCenter.Kernel.SubscriberStrategyBase"/> class here
    /// if you want your endpoint to subscribe contents from content providing endpoints in the same channel.
    /// Return null if your endpoint does not support subscriber role. For details, see Remarks.
    /// </summary>
    /// <returns></returns>
    /// <remarks>
    /// This method is called everytime this endpoint is instantiated. Do not perform initialization of strategy here.
    /// </remarks>
    /// <example>
    /// 	<code>
    /// public SubscriberStrategyBase InstantiateSubscriberStrategy()
    /// {
    /// return new SubscriberStrategyImpl();
    /// }
    /// </code>
    /// </example>
    /// <value>The provider strategy of this endpoint.</value>
    public override SubscriberStrategyBase InstantiateSubscriberStrategy()
    {
      return null;
    }


    /// <summary>
    /// Gets value indicating whether endpoint has a configuration dialog in specified runtime context.
    /// For more information see remarks.
    /// </summary>
    /// <param name="context">The configuration dialog runtime context.</param>
    /// <returns><c>true</c> when endpoint has a configuration dialog, otherwise returns <c>false</c>.</returns>
    /// <remarks>Notice that this method is correlated with <see cref="M:ProconTel.CommunicationCenter.Kernel.ChannelEndpointBase.GetConfigurationDialog"/>.
    /// If result from <see cref="M:ProconTel.CommunicationCenter.Kernel.ChannelEndpointBase.HasConfigurationDialog"/> equals <c>false</c> then method
    /// <see cref="M:ProconTel.CommunicationCenter.Kernel.ChannelEndpointBase.GetConfigurationDialog"/> will not be called. Call for
    /// <see cref="M:ProconTel.CommunicationCenter.Kernel.ChannelEndpointBase.GetConfigurationDialog"/> will be made only when 
    /// <see cref="M:ProconTel.CommunicationCenter.Kernel.ChannelEndpointBase.HasConfigurationDialog"/> returns <c>true</c>.</remarks>
    public override bool HasConfigurationDialog(EndpointPluginRuntimeContext context)
    {
      return true; // has configuration dialog
    }

    /// <summary>
    /// Gets an instance of the <see cref="T:ProconTel.CommunicationCenter.Kernel.ConfigurationDialogBase"/> class.
    /// Return <c>null</c> if your endpoint does not provide a configuration dialog. For details, see Remarks.
    /// </summary>
    /// <param name="context">The configuration dialog runtime context.</param>
    /// <returns>instance of the <see cref="T:ProconTel.CommunicationCenter.Kernel.ConfigurationDialogBase"/> class or <c>null</c> if endpoint does not
    /// provide a configuration dialog.</returns>
    /// <remarks>
    /// <para>Override this method to return a new instance of an inherited
    /// <see cref="T:ProconTel.CommunicationCenter.Kernel.ConfigurationDialogBase"/> class. Instances of the
    /// <see cref="T:ProconTel.CommunicationCenter.Kernel.ConfigurationDialogBase"/> class allows users of ProconTEL Communication
    /// Console to configure this endpoint, with advanced options provided by endpoint.</para>
    /// <para>Notice that this property is invoked seperately without calling the <c>Initialize</c> method before.
    /// Eventual configuration deserializing becomes necessary here.</para>
    /// </remarks>
    public override ConfigurationDialogBase GetConfigurationDialog(EndpointPluginRuntimeContext context)
    {
      return new ConfigurationDialog();
    }

    /// <summary>
    /// Gets value indicating whether endpoint has a status control in specified runtime context.
    /// For more information see remarks.
    /// </summary>
    /// <param name="context">The status dialog runtime context.</param>
    /// <returns>
    ///   <c>true</c> when endpoint has a status dialog, otherwise returns <c>false</c>.
    /// </returns>
    public override bool HasStatusControl(IEndpointStatusController context)
    {
      return true; // has status control
    }

    /// <summary>
    /// Gets an instance of the <see cref="!:ProconTel.CommunicationCenter.Kernel.StatusControlBase"/> class.
    /// Return <c>null</c> if your endpoint does not provide a status control. For more details, see Remarks.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns>
    /// instance of the <see cref="!:ProconTel.CommunicationCenter.Kernel.StatusControlBase"/> class or
    /// <c>null</c> if endpoint does not provide a status control.
    /// </returns>
    public override IEndpointStatusControlProvider GetStatusControl(IEndpointStatusController context)
    {
      return new DefaultWinFormsStatusControlProvider(new StatusControl(context));
    }

    public static ServiceHost CreateHost(int port, object serviceInstance, Type contract, string uri)
    {
      var host = new ServiceHost(serviceInstance, new Uri(String.Format("net.tcp://localhost:{0}", port)));
      host.AddServiceEndpoint(contract, new NetTcpBinding
                                          {
                                            Security = new NetTcpSecurity
                                                         {
                                                           Mode = SecurityMode.None
                                                         }
                                          }, uri);

      host.Description.Behaviors.Add(new ServiceMetadataBehavior());

      host.AddServiceEndpoint(ServiceMetadataBehavior.MexContractName,
        MetadataExchangeBindings.CreateMexTcpBinding(),
        String.Format("net.tcp://localhost:{0}/mex", port));

      host.Open();
      return host;
    }
  }
}