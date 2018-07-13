using Microsoft.Xrm.Sdk;
using System;
using dx_entity;
using Dex.Trace;
using Microsoft.Xrm.Sdk.Client;

namespace PL_TRACE_TEST
{
    public class TEST_PL : IPlugin
    {
        #region Secure/Unsecure Configuration Setup
        private string _secureConfig = null;
        private string _unsecureConfig = null;

        public TEST_PL(string unsecureConfig, string secureConfig)
        {
            _secureConfig = secureConfig;
            _unsecureConfig = unsecureConfig;
        }
        #endregion

        public void Execute(IServiceProvider serviceProvider)
        {
            ITracingService tracer = (ITracingService) serviceProvider.GetService(typeof(ITracingService));
            IPluginExecutionContext context =
                (IPluginExecutionContext) serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory factory =
                (IOrganizationServiceFactory) serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = factory.CreateOrganizationService(context.UserId);

            try
            {

                Entity entity = (Entity) context.InputParameters["Target"];
                int x = 0, y = 0;

                var z = 10 / y;

                //TODO: Do stuff
            }
            catch (Exception e)
            {
               
                ITrace y = new Trace();
                y.Error(e, service);


                throw new InvalidPluginExecutionException(e.Message+ context.RequestId);
            }
        }
    }
}
