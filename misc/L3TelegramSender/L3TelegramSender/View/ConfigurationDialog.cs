using System;
using ProconTel.CommunicationCenter.Kernel;

namespace L3TelegramSender.View
{
  /// <summary>
  /// Allows configuration of this plugin.
  /// </summary>
  public partial class ConfigurationDialog : ConfigurationDialogBase
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationDialog"/> class.
    /// </summary>
    public ConfigurationDialog()
    {
      InitializeComponent();
    }

    /// <summary>
    /// Loads the specified configuration into this dialog. For details, see Remarks.
    /// </summary>
    /// <param name="endpointConfigurationController">The endpoint configuration controller.</param>
    /// <remarks>
    /// <para>This method is invoked before this dialog is showed.</para>
    /// <para>Endpoint configuration controller can be also accesseed through property <see cref="ConfigurationDialogBase.ConfigurationController"/>.</para>
    /// <para>All changes made to <paramref name="endpointConfigurationController"/> will be stored to disk files after
    /// method <see cref="ConfigurationDialogBase.SaveConfiguration"/> will finish.</para>
    /// </remarks>
    public override void LoadConfiguration(IEndpointConfigurationController endpointConfigurationController)
    {
    }

    /// <summary>
    /// Saves changes in endpoint configuration. For details, see Remarks.
    /// </summary>
    /// <param name="endpointConfigurationController">The endpoint configuration controller.</param>
    /// <remarks>
    /// <para>This method is invoked after this dialog is closed with <see cref="ConfigurationDialogBase.DialogResult"/>
    /// set to <see cref="System.Windows.Forms.DialogResult.OK"/>.</para>
    /// <para>Endpoint configuration controller can be also accesseed through property <see cref="ConfigurationDialogBase.ConfigurationController"/>.</para>
    /// </remarks>
    public override void SaveConfiguration(IEndpointConfigurationController endpointConfigurationController)
    {
    }
  }
}
