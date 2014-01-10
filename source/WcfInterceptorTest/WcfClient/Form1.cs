using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WcfClient.ExampleServiceReference;

namespace WcfClient
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent();
    }

    private void btnResult_Click(object sender, EventArgs e)
    {
      int arg = 0;
      Int32.TryParse(textBox1.Text, out arg);
      try
      {
        rbResult.Text = ExecuteServerMethod(arg);
      }
      catch (Exception ex)
      {
        rbResult.Text = ex.Message;
      }
    }

    private string ExecuteServerMethod(int arg)
    {
      var sc = new ServiceClient();
      return sc.GetData(arg);
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
      Close();
    }
  }
}
