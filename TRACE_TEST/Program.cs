using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using ColorConsole;
using CrmEarlyBound;
using Dex.Trace;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Tooling.Connector;

namespace TRACE_TEST
{
    class Program
    {

        static ConsoleWriter console;

        static void Main(string[] args)
        {
            console = new ConsoleWriter();
            GetError();
            //GetPlugin();

        }

        //public static void GetPlugin()
        //{
        //    IOrganizationService service = null;

        //        var crmSvs = CrmServiceClient;
        //        service = crmSvs.OrganizationServiceProxy;


        //    ServiceContext m_serviceContext = new ServiceContext(service);

        //    var c1 = (from plu in m_serviceContext.PluginAssemblySet
        //        select plu).ToList<PluginAssembly>();

        //    using (System.IO.StreamWriter file =
        //        new System.IO.StreamWriter(@"C:\Users\atilio.rosas.ce\Documents\arosas.ce\Deloitte\Dex Project\TRACE\Dex.Trace\PL_TRACE_TEST\WriteLines2.xml"))
        //    {
        //        foreach (var c1_1 in c1)
        //        {
        //            file.WriteLine($"<field>");
        //            foreach (var attribute in c1_1.Attributes)
        //            {
        //                file.WriteLine($"<{attribute.Key}>{attribute.Value.ToString()}</{attribute.Key}>");
        //            }
        //            file.WriteLine($"</field>");
        //        }
        //    }
        //}

        public static void GetError()
        {
            int x = 0, y = 0;

            try
            {
                texto("1");
                GetError1(x, y);
            }
            catch (Exception ex)
            {
                

                MethodBase site = ex.TargetSite;
                string methodName = site == null ? null : site.Name;
                console.WriteLine("MethodBase", ConsoleColor.Red);
                console.WriteLine(methodName, ConsoleColor.Red);
                console.WriteLine(ex, ConsoleColor.Red);

                System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(ex, true);
                console.WriteLine("StackTrace", ConsoleColor.Red);
                console.WriteLine(trace, ConsoleColor.Red);
                console.WriteLine(trace.GetFrame(0), ConsoleColor.Red);

                Console.ReadLine();
            }
        }

        //TODO BORRAR
        private static CrmServiceClient CrmServiceClient
        {
            //AuthType=AD;Url=http://vm058409.cloud.es.deloitte.com:5555/RELAX; Domain=NAVSGG; Username=arosas; Password=Zaragoza1
            //Url=https://dynamics2g.crm4.dynamics.com;AuthType=Office365;UserName=systemadmin@mvp007.onmicrosoft.com;Password=3!arcrm40;

            get
            {
                //CrmServiceClient crmSvc =
                //    new CrmServiceClient(
                //        "AuthType=AD;Url=http://vm058409.cloud.es.deloitte.com:5555/RELAX; Domain=NAVSGG; Username=arosas; Password=Zaragoza1");
                CrmServiceClient crmSvc = new CrmServiceClient(new System.Net.NetworkCredential("arosas", "Zaragoza1", "navsgg"),
                    "vm058409.cloud.es.deloitte.com", "5555", "RELAX");
                return crmSvc;
            }
        }

        public static void GetError1(int x, int y)
        {
            IOrganizationService service = null;

            try
            {
                var crmSvs = CrmServiceClient;
                 service = (IOrganizationService)crmSvs.OrganizationWebProxyClient != null ? 
                    (IOrganizationService)crmSvs.OrganizationWebProxyClient : (IOrganizationService)crmSvs.OrganizationServiceProxy;

                //service = new OrganizationServiceProxy(crmSvs);


                texto("2");
                var z = 10/y;
            }
            catch (Exception ex)
            {
                ITrace dx = new Dex.Trace.Trace(service);

                //dx.IsInfoEnabled = true;
                console.WriteLine(dx.Error(ex, service), ConsoleColor.Green);

                //MethodBase site = ex.TargetSite;
                //string methodName = site == null ? null : site.Name;

                //console.WriteLine("MethodBase", ConsoleColor.Red);
                //console.WriteLine(methodName, ConsoleColor.Red);
                //console.WriteLine(ex, ConsoleColor.Red);

                //System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(ex, true);
                //console.WriteLine("StackTrace", ConsoleColor.Red);
                //console.WriteLine(trace, ConsoleColor.Red);
                //console.WriteLine(trace.GetFrame(0), ConsoleColor.Red);


               

                Console.ReadLine();
            }
        }



        static StringBuilder complete = new StringBuilder();
        public static void texto(String x)
        {
            complete.Append(x);
        }
    }
}

