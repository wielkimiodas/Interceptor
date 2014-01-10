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
  public class TestServiceBehavior : IServiceBehavior
  {
    public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
    {
      Console.WriteLine("Service Validation");
    }

    public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints,
      BindingParameterCollection bindingParameters)
    {
      Console.WriteLine("Service Binding");
    }

    public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
    {
      foreach (
           ChannelDispatcher cDispatcher in serviceHostBase.ChannelDispatchers)
        foreach (EndpointDispatcher eDispatcher in
            cDispatcher.Endpoints)
          eDispatcher.DispatchRuntime.MessageInspectors.Add(
              new TestMessageInspector());

      Console.WriteLine("Service Dispatching");
    }
  }
}
