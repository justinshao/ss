using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Permissions;
using Common.Core.Helpers;

namespace Common.Core
{
    /// <summary>
    /// 日志管理类
    /// </summary>
    public class LogManager
    {
        /// <summary>
        /// Logger对象字典
        /// </summary>
        private static Dictionary<string, Logger> Loggers = new Dictionary<string, Logger>();
        /// <summary>
        /// 根据模块名称获取Logger对象
        /// </summary>
        /// <param name="moduleName">模块名称</param>
        /// <returns>Logger对象</returns>
        public static Logger GetLogger(string moduleName = "")
        {
            if (moduleName == null) moduleName = "";
            
            Logger logger = null;
            try
            {
                if (!Loggers.TryGetValue(moduleName, out logger))
                {
                    logger = new Logger(moduleName);
                    Loggers.Add(moduleName, logger);
                }
            }
            catch
            {
            }
            return logger;

        }
    }

    /// <summary>
    /// 日志类
    /// </summary>
    public class Logger : IDisposable
    {
        /// <summary>
        /// 写数据流
        /// </summary>
        private StreamWriter _streamWriter = null;
        /// <summary>
        /// 文件夹路径
        /// </summary>
        private string _path;
        /// <summary>
        /// 模块名称
        /// </summary>
        private string _moduleName = "";

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="moduleName">模块名称</param>
        public Logger(string moduleName = null)
        {
            _moduleName = moduleName == null ? "" : moduleName;

            string path = AppDomain.CurrentDomain.BaseDirectory + "log";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            _path = path + "\\" + DateTime.Now.ToString("yyyyMMdd");
            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }
        }

        private void GetWriter()
        {
            try
            {
                string fileName = "";
                if (string.IsNullOrEmpty(_moduleName))
                {
                    fileName = _path + "\\log.txt";
                }
                else
                {
                    fileName = _path + "\\" + _moduleName + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".txt";
                }
                _streamWriter = new StreamWriter(fileName);
            }
            catch (Exception)
            { }
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="level">日志等级</param>
        /// <param name="msg">信息</param>
        private void Write(string level, object msg)
        {
            try
            {
                if (_streamWriter == null)
                {
                    GetWriter();
                }
                else if (_streamWriter.BaseStream.Length >= LogOption.FileMaxSize * 1024 * 1024)
                {
                    _streamWriter.Close();
                    GetWriter();
                }
                string s = DateTime.Now.ToString() + " " + level + " " + msg;
                byte[] bytes = Encoding.Default.GetBytes(s);
                lock (_streamWriter)
                {
                    _streamWriter.WriteLine(s);
                    _streamWriter.Flush();
                }
                //写控制台日志
                if (LogOption.IsConsoleEnabled)
                {
                    Console.WriteLine(s);
                }
            }
            catch (Exception)
            { }
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="level">日志等级</param>
        /// <param name="msg">消息</param>
        /// <param name="ex">异常</param>
        private void Write(string level, object msg, Exception ex)
        {
            Write(level, msg + " 异常消息：" + ex.Message + " 异常堆栈:" + ex.StackTrace);
        }

        /// <summary>
        /// 写调试日志
        /// </summary>
        /// <param name="msg">消息</param>
        public void Debug(object msg)
        {
            if (LogOption.IsDebugEnabled)
            {
                Write("Debug", msg);
            }
        }

        /// <summary>
        /// 写调试日志
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="ex">异常</param>
        public void Debug(object msg, Exception ex)
        {
            if (LogOption.IsDebugEnabled)
            {
                Write("Debug", msg, ex);
            }
        }

        /// <summary>
        /// 写错误日志
        /// </summary>
        /// <param name="msg">消息</param>
        public void Error(object msg)
        {
            if (LogOption.IsErrorEnabled)
            {
                Write("Error", msg);
            }
        }

        /// <summary>
        /// 写错误日志
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="ex">异常</param>
        public void Error(object msg, Exception ex)
        {
            if (LogOption.IsErrorEnabled)
            {
                Write("Error", msg, ex);
            }
        }

        /// <summary>
        /// 写致命错误日志
        /// </summary>
        /// <param name="msg">消息</param>
        public void Fatal(object msg)
        {
            if (LogOption.IsFatalEnabled)
            {
                Write("Fatal", msg);
            }
        }

        /// <summary>
        /// 写致命错误日志
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="ex">异常</param>
        public void Fatal(object msg, Exception ex)
        {
            if (LogOption.IsFatalEnabled)
            {
                Write("Fatal", msg, ex);
            }
        }

        /// <summary>
        /// 写信息日志
        /// </summary>
        /// <param name="msg">消息</param>
        public void Info(object msg)
        {
            if (LogOption.IsInfoEnabled)
            {
                Write("Info", msg);
            }
        }

        /// <summary>
        /// 写信息日志
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="ex">异常</param>
        public void Info(object msg, Exception ex)
        {
            if (LogOption.IsInfoEnabled)
            {
                Write("Info", msg, ex);
            }
        }

        /// <summary>
        /// 写警告日志
        /// </summary>
        /// <param name="msg">消息</param>
        public void Warn(object msg)
        {
            if (LogOption.IsWarnEnabled)
            {
                Write("Warn", msg);
            }
        }

        /// <summary>
        /// 写警告日志
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="ex">异常</param>
        public void Warn(object msg, Exception ex)
        {
            if (LogOption.IsWarnEnabled)
            {
                Write("Warn", msg, ex);
            }
        }

        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            if (_streamWriter != null)
            {
                _streamWriter.Close();
                _streamWriter.Dispose();
            }
        }
    }

    /// <summary>
    /// 日志选项
    /// </summary>
    public static class LogOption
    {
        //由于每项变更都会更改文件，会触发很多次，导致程序关闭，目前屏蔽
        //private static OptionWatcher watcher = new OptionWatcher();

        /// <summary>
        /// 是否开启控制台输出
        /// </summary>
        public static bool IsConsoleEnabled = true;

        /// <summary>
        /// 文件大小,单位M
        /// </summary>
        public static int FileMaxSize = 5;

        /// <summary>
        /// 是否开启Debug
        /// </summary>
        public static bool IsDebugEnabled = true;
        /// <summary>
        /// 是否开启Error
        /// </summary>
        public static bool IsErrorEnabled = true;
        /// <summary>
        /// 是否开启Fatal
        /// </summary>
        public static bool IsFatalEnabled = true;
        /// <summary>
        /// 是否开启Info
        /// </summary>
        public static bool IsInfoEnabled = true;
        /// <summary>
        /// 是否开启Warn
        /// </summary>
        public static bool IsWarnEnabled = true;
    }

    /// <summary>
    /// 配置变更监听
    /// </summary>
    public class OptionWatcher
    {
        FileSystemWatcher watch = null;

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public OptionWatcher()
        {
            watch = new FileSystemWatcher(AppDomain.CurrentDomain.BaseDirectory, "*.config");
            watch.Created += new FileSystemEventHandler(watch_Created);
            watch.Changed += new FileSystemEventHandler(watch_Changed);
            watch.Deleted += new FileSystemEventHandler(watch_Deleted);
            watch.Renamed += new RenamedEventHandler(watch_Renamed);
            watch.EnableRaisingEvents = true;
        }

        private void watch_Renamed(object sender, RenamedEventArgs e)
        {
            if (e.OldName.Equals("Option.config"))
            {
                //原文件为配置文件
                watch_Deleted(null, null);
            }
            if (e.Name.Equals("Option.config"))
            {
                //改名为log.config
                LoadOption();
            }
        }

        /// <summary>
        /// 配置文件删除后使用默认配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void watch_Deleted(object sender, FileSystemEventArgs e)
        {
            if (e == null || e.Name.Equals("Option.config"))
            {
                //文件被删除
                LogOption.IsConsoleEnabled = true;
                LogOption.IsDebugEnabled = true;
                LogOption.IsErrorEnabled = true;
                LogOption.IsFatalEnabled = true;
                LogOption.IsInfoEnabled = true;
                LogOption.IsWarnEnabled = true;
                LogOption.FileMaxSize = 5;
            }
        }

        private void watch_Changed(object sender, FileSystemEventArgs e)
        {
            if (e.Name.Equals("Option.config"))
            {
                LoadOption();
            }
        }

        private void watch_Created(object sender, FileSystemEventArgs e)
        {
            if (e.Name.Equals("Option.config"))
            {
                LoadOption();
            }
        }

        private void LoadOption()
        {
            //LogOption.IsConsoleEnabled = true;
            //LogOption.IsDebugEnabled = true;
            //LogOption.IsErrorEnabled = true;
            //LogOption.IsFatalEnabled = true;
            //LogOption.IsInfoEnabled = true;
            //LogOption.IsWarnEnabled = true;
            //LogOption.FileMaxSize = 5;

            LogOption.IsConsoleEnabled = OptionHelper.ReadBool("Log", "IsConsoleEnabled", true);
            LogOption.IsDebugEnabled = OptionHelper.ReadBool("Log", "IsDebugEnabled", true);
            LogOption.IsErrorEnabled = OptionHelper.ReadBool("Log", "IsErrorEnabled", true);
            LogOption.IsFatalEnabled = OptionHelper.ReadBool("Log", "IsFatalEnabled", true);
            LogOption.IsInfoEnabled = OptionHelper.ReadBool("Log", "IsInfoEnabled", true);
            LogOption.IsWarnEnabled = OptionHelper.ReadBool("Log", "IsWarnEnabled", true);
            LogOption.FileMaxSize = OptionHelper.ReadInt("Log", "FileMaxSize", 5);
        }
    }
}
