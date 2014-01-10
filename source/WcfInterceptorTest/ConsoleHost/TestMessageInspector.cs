using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;

namespace ConsoleHost
{
  class TestMessageInspector : IDispatchMessageInspector
  {
    public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
    {
      throw new Exception("blub");
      Console.WriteLine("After request");
      return null;
    }

    public void BeforeSendReply(ref Message reply, object correlationState)
    {
      Console.WriteLine("Before reply");
    }
  }
}
