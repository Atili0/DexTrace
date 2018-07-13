namespace Dex.Trace
{
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Client;
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Web.Services.Protocols;

    public class Trace : ITrace
    {
        

        public string s_name { get; set; }  
        public  bool b_IsDebugEnabled { get; set; }
        public  bool b_IsInfoEnabled { get; set; }
        public bool b_ShowMessage { get; set; }
        public Guid g_TraceConfigId { get; set; }
        public  IOrganizationService OrgService { get; set; }
        public ITracingService TracingService { get; set; }
        public OrganizationServiceContext OrgContext { get; set; }
        public Guid g_Trace { get; set; }
        public System.Text.StringBuilder sb_info { get; set; }

        const string s_TraceConfig = "dx_traceconfig";
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p_orgService"></param>
        public Trace(IOrganizationService p_orgService, string p_PluginName)
        {
            #region Create Services Organization
            this.OrgService = p_orgService;
            this.OrgContext = new OrganizationServiceContext(p_orgService);
            this.sb_info = new System.Text.StringBuilder();
            #endregion

            #region Config Trace
            using (ServiceContext svcContext = new ServiceContext(p_orgService))
            {
                var config_trace = (from a in svcContext.dx_traceconfigSet
                              where a.dx_name == p_PluginName
                              select new {
                                  b_IsDebugEnabled = a.Attributes["dx_isdebugenabled"],
                                  b_IsInfoEnabled = a.Attributes["dx_isinfoenabled"],
                                  b_ShowMessage = a.Attributes["dx_showmessageclient"],
                                  g_TraceConfigId = a.Attributes["dx_traceconfigid"]
                              }).FirstOrDefault();

                this.b_IsDebugEnabled = Boolean.Parse(config_trace.b_IsDebugEnabled.ToString());
                this.b_IsInfoEnabled = Boolean.Parse(config_trace.b_IsInfoEnabled.ToString());
                this.b_ShowMessage = Boolean.Parse(config_trace.b_ShowMessage.ToString());
                this.g_TraceConfigId = Guid.Parse(config_trace.g_TraceConfigId.ToString());
            };
            #endregion

        }

        public  void Debug(string message)
        {
            if (!b_IsDebugEnabled) {
                SaveErrorInTrace(message);
            }
        }

        public void Debug(string message, Exception exception)
        {

        }


        /// <summary>
        /// Se usa solo en el caso, que no se este guardando los errores. 
        /// </summary>
        public void Close() {
            SaveInfoInTrace(sb_info.ToString());
        }

        /// <summary>
        /// Metodo de solo información no se mostrara nada al cliente. 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="p_orgservice"></param>
        public  void Info(string message)
        {
            if (b_IsInfoEnabled)
            {
                //Diagnostic dx_diag = new Diagnostic();
                sb_info.AppendLine(message);
                //SaveInfoInTrace(message);
            }
        }

        /// <summary>
        /// Metodo de solo información no se mostrara nada al cliente. 
        /// </summary>
        /// <param name="p_message"></param>
        /// <param name="p_exception"></param>
        /// <param name="p_orgservice"></param>
        public void Info(string p_message, Exception p_exception)
        {
            if (b_IsInfoEnabled)
            {

                Diagnostic dx_diag = new Diagnostic();
                var error = dx_diag.ExceptionError(p_exception);
                SaveInfoInTrace(p_message, error);
            }
        }
        
        public  void Error (string message)
        {
            if (!b_IsInfoEnabled)
            {
                Diagnostic dx_diag = new Diagnostic();
                SaveErrorInTrace(message);
            }
            if (b_ShowMessage)
                throw new InvalidPluginExecutionException(OperationStatus.Failed, message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p_message"></param>
        public void Error(Exception p_message)
        {
            //if (!IsInfoEnabled) return string.Empty;
            Diagnostic dx_diag = new Diagnostic();
            var error = dx_diag.ExceptionError(p_message);
            SaveErrorInTrace(error, p_message);
            //return error;


            if (b_ShowMessage)
                throw new InvalidPluginExecutionException(OperationStatus.Failed, error);
        }

        public  void Error(string message, Exception exception)
        {
            
        }

        public void SaveInfoInTrace(string p_messageInfo)
        {
            var buffer = DateFormat.buffer;
            new DateFormat().GetCompleteDate(DateTime.UtcNow, out buffer);

            var m_trace = new dx_trace()
            {
                dx_name = buffer.ToString(),
                dx_messageinfo = p_messageInfo,
                dx_traceconfigid = new EntityReference(s_TraceConfig, this.g_TraceConfigId)
            };

            OrgContext.AddObject(m_trace);
            OrgContext.SaveChanges();
        }

        public void SaveInfoInTrace(string p_messageInfo, string p_completeError)
        {
            var buffer = DateFormat.buffer;
            new DateFormat().GetCompleteDate(DateTime.UtcNow, out buffer);


            var m_trace = new dx_trace()
            {
                dx_messageinfo = p_messageInfo,
                dx_name = buffer.ToString(),
                dx_tracetext = p_completeError,
                dx_traceparameter = Diagnostic.ExceptionParameter(),
                dx_traceconfigid = new EntityReference(s_TraceConfig, this.g_TraceConfigId)
            };

            OrgContext.AddObject(m_trace);
            OrgContext.SaveChanges();
        }

        public void SaveErrorInTrace(string p_messageError)
        {
            var buffer = DateFormat.buffer;
            new DateFormat().GetCompleteDate(DateTime.UtcNow, out buffer);

            var m_trace = new dx_trace()
            {
                dx_name = buffer.ToString(),
                dx_messageexception = p_messageError,
                dx_traceconfigid = new EntityReference(s_TraceConfig, this.g_TraceConfigId),
            };

            OrgContext.AddObject(m_trace);
            OrgContext.SaveChanges();

        }


        /// <summary>
        /// Guarda el error y los parametros de este. 
        /// </summary>
        /// <param name="p_messageError"></param>
        /// <param name="p_Exception"></param>
        public void SaveErrorInTrace(string p_messageError, Exception p_Exception)
        {
            
            var buffer = DateFormat.buffer;
            new DateFormat().GetCompleteDate(DateTime.UtcNow, out buffer);

            #region Get Stack Error

            MethodBase site = p_Exception.TargetSite;
            string methodName = site == null ? null : site.Name;
            var trace = new StackTrace(p_Exception, true);

            #endregion

            var m_trace = new dx_trace()
            {
                dx_messageexception = p_messageError,
                dx_name = buffer.ToString(),
                dx_tracetext = trace.ToString(),
                dx_traceparameter = Diagnostic.ExceptionParameter(),
                dx_traceconfigid = new EntityReference(s_TraceConfig, this.g_TraceConfigId),
                dx_messageinfo = sb_info.ToString(),


            };

            OrgContext.AddObject(m_trace);
            OrgContext.SaveChanges();

            //if (!b_ShowMessage)
            //{
            //    throw new InvalidPluginExecutionException(p_messageError);
            //}
        }
    }
}