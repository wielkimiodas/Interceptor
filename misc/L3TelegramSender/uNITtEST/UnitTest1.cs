using System;
using System.Drawing.Design;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

namespace uNITtEST
{
  [TestClass]
  public class UnitTest1
  {
    [TestMethod]
    public void TestMethod1()
    {
      const string DBconnectionString = "Data Source=PMCDB; User Id=PMC; Password=PMC";
      using (var cn = new OracleConnection(DBconnectionString))
      {
        cn.Open();
        
        cn.Close();
      }
    }
  }
}
