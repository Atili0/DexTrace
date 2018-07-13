namespace Dex.Trace
{
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Client;
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Web.Services.Protocols;

    public interface ITrace
    {
        void Close();
        void Debug(string message);
        void Debug(string message, Exception exception);
        void Info(string message);
        void Info(string p_message, Exception p_exception);
        void Error (string message);
        void Error(Exception p_message);
        void Error(string message, Exception exception);
        void SaveInfoInTrace(string p_messageInfo);
        void SaveInfoInTrace(string p_messageInfo, string p_completeError);
        void SaveErrorInTrace(string p_messageError);
        void SaveErrorInTrace(string p_messageError, Exception p_Exception);
    }
}