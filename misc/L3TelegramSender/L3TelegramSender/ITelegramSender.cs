using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace L3TelegramSender
{
  [ServiceContract]
  public interface ITelegramSender
  {
    [OperationContract]
    void SendTelegram(string xml);
  }
}
