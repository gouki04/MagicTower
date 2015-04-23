using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Utils
{
    public class Logger : Singleton<Logger>
    {
        public enum ELevel
        {
            Debug = 0,
            Info = 1,
            Warn = 2,
            Error = 3,
            Fatal = 4,
        }

        private FileStream mFile;
        private ELevel mLogLevel = ELevel.Debug;

        public Logger()
        {

        }

        public void SetLoggerFile(string file_path)
        {
            if (mFile != null)
                mFile.Close();

            mFile = File.Open(file_path, FileMode.Create);
        }

        public void SetLogLevel(ELevel level)
        {
            mLogLevel = level;
        }

        public void Log(ELevel level, string msg)
        {
            if (level >= mLogLevel)
            {
                msg = string.Format("[{0}]{1}", System.Enum.GetName(typeof(ELevel), level), msg);

                UnityEngine.Debug.Log(msg);

                if (mFile != null)
                {
                    int end_offset = (int)mFile.Seek(0, SeekOrigin.End);
                    var bytes = System.Text.Encoding.UTF8.GetBytes(msg);
                    mFile.Write(bytes, end_offset, bytes.Length);
                }
            }
        }

        public static void LogDebug(string msg, params object[] args)
        {
            Logger.Instance.Log(ELevel.Debug, string.Format(msg, args));
        }

        public static void LogInfo(string msg, params object[] args)
        {
            Logger.Instance.Log(ELevel.Info, string.Format(msg, args));
        }

        public static void LogWarn(string msg, params object[] args)
        {
            Logger.Instance.Log(ELevel.Warn, string.Format(msg, args));
        }

        public static void LogError(string msg, params object[] args)
        {
            Logger.Instance.Log(ELevel.Error, string.Format(msg, args));
        }

        public static void LogFatal(string msg, params object[] args)
        {
            Logger.Instance.Log(ELevel.Fatal, string.Format(msg, args));
        }
    }
    
}