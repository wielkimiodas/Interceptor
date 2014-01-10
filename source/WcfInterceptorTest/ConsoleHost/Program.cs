using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using WcfServer;

namespace ConsoleHost
{
  internal class Program
  {
    private static void Main(string[] args)
    {
      using (var serviceHost = new ServiceHost(typeof(ExampleService)))
      {
        try
        {
          // Open the ServiceHost to start listening for messages.

          var debug = serviceHost.Description.Behaviors.Find<ServiceDebugBehavior>();

          // if not found - add behavior with setting turned on 
          if (debug == null)
          {
            serviceHost.Description.Behaviors.Add(
                 new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });
          }
          else
          {
            // make sure setting is turned ON
            if (!debug.IncludeExceptionDetailInFaults)
            {
              debug.IncludeExceptionDetailInFaults = true;
            }
          }

          serviceHost.Description.Behaviors.Add(new TestServiceBehavior());
          serviceHost.Description.Behaviors.Add(new SecondSvcBehavior());
          serviceHost.Open();

          // The service can now be accessed.
          Console.WriteLine("The service is ready.");
          Console.WriteLine("Press <ENTER> to terminate service.");
          Console.ReadLine();

          Console.WriteLine("Closing...");
          // Close the ServiceHost.
          serviceHost.Close();
        }
        catch (TimeoutException timeProblem)
        {
          Console.WriteLine(timeProblem.Message);
          Console.ReadLine();
        }
        catch (CommunicationException commProblem)
        {
          Console.WriteLine(commProblem.Message);
          Console.ReadLine();
        }
      }
    }
  }
}