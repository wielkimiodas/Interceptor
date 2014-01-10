using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;

namespace ConsoleHost
{
  class SecondMsgInsp : IDispatchMessageInspector
  {
    public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
    {
      Console.WriteLine("second after");
      return null;
    }

    public void BeforeSendReply(ref Message reply, object correlationState)
    {
      Console.WriteLine("second before");
    }
  }
}
