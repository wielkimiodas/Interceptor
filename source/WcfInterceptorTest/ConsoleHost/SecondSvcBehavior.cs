using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;

namespace ConsoleHost
{
  class SecondSvcBehavior : IServiceBehavior
  {
    public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
    {
      Console.WriteLine("second validation");
    }

    public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints,
      BindingParameterCollection bindingParameters)
    {
      Console.WriteLine("Second binding");
    }

    public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
    {
      foreach (
                 ChannelDispatcher cDispatcher in serviceHostBase.ChannelDispatchers)
        foreach (EndpointDispatcher eDispatcher in
            cDispatcher.Endpoints)
          eDispatcher.DispatchRuntime.MessageInspectors.Add(
              new SecondMsgInsp());

      Console.WriteLine("Second Dispatching");
    }
  }
}
