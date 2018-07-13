namespace Dex.Trace
{
    using System;
    using System.Text;

    public class DateFormat
    {
        public static StringBuilder buffer = new StringBuilder();

        public void GetCompleteDate (DateTime dateToFormat, out StringBuilder buffer)
        {
           FormatDateWithoutMillis(dateToFormat,out buffer);
        }

        protected void FormatDateWithoutMillis(DateTime dateToFormat, out StringBuilder buffer)
        {
            buffer = new StringBuilder();
            String[] formats = { "dd MMM yyyy hh:mm tt UTC",
                           "dd MMM yyyy hh:mm tt UTC" };
            
            buffer.Append(dateToFormat.ToString(formats[1]));
        }
    }
}