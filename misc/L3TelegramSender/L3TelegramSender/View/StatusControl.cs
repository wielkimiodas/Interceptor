using System;
using System.Windows.Forms;
using ProconTel.CommunicationCenter.Kernel;

namespace L3TelegramSender.View
{
  /// <summary>
  /// Responsible for displaying status of this plugin during runtime.
  /// </summary>
  public partial class StatusControl : UserControl, IEndpointStatusControl
  {
    /// <summary>
    /// Stores the object representing a context which the status control was created in. 
    /// </summary>
    private readonly IEndpointStatusController _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="StatusControl"/> class.
    /// </summary>
    public StatusControl()
    {
      InitializeComponent();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="StatusControl"/> class with specified context.
    /// </summary>
    /// <param name="context">The context.</param>
    public StatusControl(IEndpointStatusController context)
      : this()
    {
      _context = context;
    }

    /// <summary>
    /// This method is invoked to display the specified status on this dialog. For details, see Remarks.
    /// </summary>
    /// <param name="statusInformation">The status information.</param>
    public void DisplayStatus(object statusInformation)
    {
    }

    /// <summary>
    /// Occurs before status control is hidden.
    /// </summary>
    public void OnStatusControlHidden()
    {
    }

    /// <summary>
    /// Occurs after status control is shown.
    /// </summary>
    public void OnStatusControlShown()
    {
    }
  }
}
