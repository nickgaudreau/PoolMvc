using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web;

namespace PoolHockeyDAL.Log
{
    /// <summary>
    /// The Class Writes Exception and Error information into a log file named ErrorLog.txt.
    /// </summary>
    public class LogError
    {
        /// <summary>
        /// Writes error occured in log file,if log file does not exist,it creates the file first.
        /// </summary>
        /// <param name="exception">Exception</param>
        /// <param name="details"></param>
        public static void Write(Exception exception, string details)
        {
            try
            {
                var logFile = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["ErrorLog"]);
                StreamWriter logWriter;
                if (File.Exists(logFile))
                    logWriter = File.AppendText(logFile);
                else
                    logWriter = File.CreateText(logFile);
                logWriter.WriteLine("=>" + DateTime.Now + " " + " An Error occured : " +
                    exception.StackTrace + " Message : " + exception.Message + " " + details + ". " + "\n\n");
                logWriter.Close();
                throw exception;

            }
            catch (Exception e)
            {
                //throw;
                var tmp = e;
            }
            finally
            {
                //throw;
            }
        }

        /// <summary>
        /// Writes Many error occured in log file,if log file does not exist,it creates the file first.
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="details"></param>
        public static void WriteMany(Exception exception, List<string> details)
        {
            try
            {
                var logFile = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["ErrorLog"]);
                StreamWriter logWriter;
                if (File.Exists(logFile))
                    logWriter = File.AppendText(logFile);
                else
                    logWriter = File.CreateText(logFile);
                logWriter.WriteLine("=>" + DateTime.Now + " " + " An Error occured : " +
                    exception.StackTrace + " Message : " + exception.Message + ". " + "\n");
                foreach (var detail in details)
                {
                    logWriter.WriteLine("\t - Detail: " + detail + ". " + "\n\n");
                }
                logWriter.Close();
                throw exception;

            }
            catch (Exception e)
            {
                //throw;
                var tmp = e;
            }
            finally
            {
                //throw;
            }
        }
    }
}
