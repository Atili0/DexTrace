namespace Dex.Trace
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Xml;
    using System.Reflection;

    internal sealed class Diagnostic
    {
        public string ExceptionError(Exception p_exception)
        {
            return Diagnostic.ExceptionToTraceString(p_exception);
        }

        public static string ExceptionParameter()
        {
            return Diagnostic.GetParameter();
        }


        private static string ExceptionToTraceString (Exception exception)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GetErrorInformation(exception));
            XmlTextWriter xmlTextWriter = new XmlTextWriter((TextWriter)new StringWriter(sb, (IFormatProvider)CultureInfo.CurrentCulture));
            xmlTextWriter.WriteStartElement("Exception");
            xmlTextWriter.WriteElementString("ExceptionType", Diagnostic.XmlEncode(exception.GetType().AssemblyQualifiedName));
            xmlTextWriter.WriteElementString("Message", Diagnostic.XmlEncode(exception.Message));
            xmlTextWriter.WriteElementString("StackTrace", Diagnostic.XmlEncode(Diagnostic.StackTraceString(exception)));
            xmlTextWriter.WriteElementString("ExceptionString", Diagnostic.XmlEncode(exception.ToString()));
            Win32Exception win32Exception = exception as Win32Exception;
            if (win32Exception != null)
                xmlTextWriter.WriteElementString("NativeErrorCode", win32Exception.NativeErrorCode.ToString("X", (IFormatProvider)CultureInfo.InvariantCulture));
            if (exception.Data != null && exception.Data.Count > 0)
            {
                xmlTextWriter.WriteStartElement("DataItems");
                foreach (object key in (IEnumerable)exception.Data.Keys)
                {
                    xmlTextWriter.WriteStartElement("Data");
                    xmlTextWriter.WriteElementString("Key", Diagnostic.XmlEncode(key.ToString()));
                    xmlTextWriter.WriteElementString("Value", Diagnostic.XmlEncode(exception.Data[key].ToString()));
                    xmlTextWriter.WriteEndElement();
                }
                xmlTextWriter.WriteEndElement();
            }
            if (exception.InnerException != null)
            {
                xmlTextWriter.WriteStartElement("InnerException");
                xmlTextWriter.WriteRaw(Diagnostic.ExceptionToTraceString(exception.InnerException));
                xmlTextWriter.WriteEndElement();
            }
            xmlTextWriter.WriteEndElement();
            return sb.ToString();
        }

        private static string GetErrorInformation(Exception exception)
        {
            StringBuilder sw = new StringBuilder();
            sw.AppendLine("======================================================================================================================");
            sw.AppendLine("Source\t: " + (exception.Source != null ? exception.Source.ToString().Trim() : "Not Provided"));
            sw.AppendLine("Method\t: " + (exception.TargetSite != (MethodBase)null ? exception.TargetSite.Name.ToString() : "Not Provided"));
            sw.AppendLine("Date\t: " + DateTime.Now.ToLongTimeString());
            sw.AppendLine("Time\t: " + DateTime.Now.ToShortDateString());
            sw.AppendLine("Error\t: " + (string.IsNullOrEmpty(exception.Message) ? "Not Provided" : exception.Message.ToString().Trim()));
            sw.AppendLine("Stack Trace\t: " + (string.IsNullOrEmpty(exception.StackTrace) ? "Not Provided" : exception.StackTrace.ToString().Trim()));
            sw.AppendLine("======================================================================================================================");

            return sw.ToString();
        }

        public static string XmlEncode (string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;
            int length = text.Length;
            StringBuilder stringBuilder = new StringBuilder(length + 8);
            for (int index = 0; index < length; ++index)
            {
                char ch = text[index];
                switch (ch)
                {
                    case '&':
                        stringBuilder.Append("&amp;");
                        break;
                    case '<':
                        stringBuilder.Append("&lt;");
                        break;
                    case '>':
                        stringBuilder.Append("&gt;");
                        break;
                    default:
                        stringBuilder.Append(ch);
                        break;
                }
            }
            return stringBuilder.ToString();
        }

        private static string GetParameter()
        {
            StringBuilder buffer = new StringBuilder();
            StackTrace trace = new StackTrace();
            buffer.AppendLine("======================================================================================================================");
            buffer.AppendLine($"Frame Count: {trace.FrameCount}");
            for (int i = 0; i < trace.FrameCount; i++)
            {
                StackFrame frame = trace.GetFrame(i);
                buffer.AppendLine($"-> Frame { i } FileName: {frame.GetFileName()}");

                MethodBase method = frame.GetMethod();
                buffer.AppendLine($"-> -> MethodName: { method.Name}");

                buffer.AppendLine($"-> -> MethodName param:{method.ToString()}");

                ParameterInfo[] parameters = method.GetParameters();
                buffer.AppendLine($"-> -> Params: {parameters.Length}");
                foreach (ParameterInfo param in parameters)
                {
                    buffer.AppendLine($"-> -> -> Name: {param.Name}");
                    buffer.AppendLine($"-> -> -> type: {param.ToString()}");
                }
            }
            buffer.AppendLine("======================================================================================================================");
            return buffer.ToString();
        }

        private static string StackTraceString (Exception exception)
        {
            string stackTrace = exception.StackTrace;
            if (string.IsNullOrEmpty(stackTrace))
            {
                StackFrame[] frames = new StackTrace(false).GetFrames();
                int skipFrames = 0;
                bool flag = false;
                foreach (StackFrame stackFrame in frames)
                {
                    string name = stackFrame.GetMethod().Name;
                    switch (name)
                    {
                        case "StackTraceString":
                        case "AddExceptionToTraceString":
                        case "GetAdditionalPayload":
                            ++skipFrames;
                            break;
                        default:
                            if (name.StartsWith("ThrowHelper", StringComparison.Ordinal))
                            {
                                ++skipFrames;
                                break;
                            }
                            flag = true;
                            break;
                    }
                    if (flag)
                        break;
                }
                stackTrace = new StackTrace(skipFrames, false).ToString();
            }
            return stackTrace;
        }

    }
}