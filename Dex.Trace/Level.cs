namespace Dex.Trace.Core
{
    using System;

    public class Level
    {
        public static readonly Level Off = new Level(int.MaxValue, "OFF");
        public static readonly Level Emergency = new Level(120000, "EMERGENCY");
        public static readonly Level Fatal = new Level(110000, "FATAL");
        public static readonly Level Alert = new Level(100000, "ALERT");
        public static readonly Level Critical = new Level(90000, "CRITICAL");
        public static readonly Level Severe = new Level(80000, "SEVERE");
        public static readonly Level Error = new Level(70000, "ERROR");
        public static readonly Level Warn = new Level(60000, "WARN");
        public static readonly Level Notice = new Level(50000, "NOTICE");
        public static readonly Level Info = new Level(40000, "INFO");
        public static readonly Level Debug = new Level(30000, "DEBUG");
        public static readonly Level Fine = new Level(30000, "FINE");
        public static readonly Level Trace = new Level(20000, "TRACE");
        public static readonly Level Finer = new Level(20000, "FINER");
        public static readonly Level Verbose = new Level(10000, "VERBOSE");
        public static readonly Level Finest = new Level(10000, "FINEST");
        public static readonly Level All = new Level(int.MinValue, "ALL");
        
        private readonly int m_nlevelValue;
        private readonly string m_nlevelName;
        private readonly string m_nlevelDisplayName;

        public Level (int level, string levelName, string displayName)
        {
            if (levelName == null)
                throw new ArgumentNullException("levelName");
            if (displayName == null)
                throw new ArgumentNullException("displayName");
            this.m_nlevelValue = level;
            this.m_nlevelName = string.Intern(levelName);
            this.m_nlevelDisplayName = displayName;
        }

        public Level (int level, string levelName) : this(level, levelName, levelName)
        {
        }
    }
}