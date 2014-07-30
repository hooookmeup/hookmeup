using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
namespace SS.Framework.Common
{
    public class Logger
    {
        static Logger curInstance = null;
        private static readonly object singletonLock = new object();
        TraceSource TraceSource;


        Logger()
        {

            TraceSource = new TraceSource("AzureDiagnosticsSource");
            TraceSource.Switch = new SourceSwitch("AzureDiagnosticsSwitch", "Verbose");
            try
            {

                if (RoleEnvironment.IsAvailable)
                {
                    DiagnosticMonitorTraceListener tl = new DiagnosticMonitorTraceListener();
                    tl.Name = "AzureDiagnostics";
                    TraceSource.Listeners.Add(tl);

                }
            }
            catch (Exception ex)
            { 
                //ignore excetion in Role env
            }
        }

        private void LogTraceEvent(TraceEventType eventType, int EventId, string Message)
        {
            TraceSource.TraceEvent(eventType, EventId, Message);
        }

        static Logger CurrentInstance
        {
            get
            {
                if (curInstance==null)

                    lock (singletonLock)
                    {
                        if (curInstance == null)
                            curInstance = new Logger();
                    }
                return curInstance; 
            }
        }


        public static void SetScheduledLogStorage(ref DiagnosticMonitorConfiguration diagMonConf, int TransferPeriodmin,LogLevel level)
        {

            diagMonConf.Logs.ScheduledTransferLogLevelFilter = level;
            diagMonConf.Logs.ScheduledTransferPeriod = TimeSpan.FromMinutes(TransferPeriodmin);
       
        }

        public static void SetScheduledDiagnosticInfrastructureLogsStorage(ref DiagnosticMonitorConfiguration diagMonConf, int TransferPeriodmin, LogLevel level)
        {

            diagMonConf.DiagnosticInfrastructureLogs.ScheduledTransferLogLevelFilter = level;
            diagMonConf.DiagnosticInfrastructureLogs.ScheduledTransferPeriod = TimeSpan.FromMinutes(TransferPeriodmin);
           
        }


        public static void SetScheduledCountersStorage(ref DiagnosticMonitorConfiguration diagMonConf, int TransferPeriodmin, string CounterSpec, int sampleratesec)
        {
            //CounterSpec =  @"\Processor(*)\% Processor Time";

            PerformanceCounterConfiguration pcc = new PerformanceCounterConfiguration();
            pcc.CounterSpecifier = CounterSpec;
            pcc.SampleRate = TimeSpan.FromSeconds(sampleratesec);
            diagMonConf.PerformanceCounters.DataSources.Add(pcc);
       
            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            diagMonConf.PerformanceCounters.ScheduledTransferPeriod = TimeSpan.FromMinutes(TransferPeriodmin);
          
        }

        public static void LogInformaton(string Message)
        {
            LogMessageInternal(TraceEventType.Information, 0, FormatMessage(Message));
        }

        public static void LogWarning(string Message)
        {
            LogMessageInternal( TraceEventType.Warning, 0, FormatMessage(Message));
        }

        public static void LogError(string Message)
        {
            LogMessageInternal( TraceEventType.Error, 0, FormatMessage(Message));

        }

        public static void LogDebug(string Message)
        {
            LogMessageInternal( TraceEventType.Verbose, 0, FormatMessage(Message));

        }


        public static void LogError(string Message, Exception ex, bool showTrace)
        {
            int EvId = 0;
            UserError ue = MapExceptionLogStack(ex);
            if (ue != null )
                 EvId = ue.eventId;
             LogMessageInternal( TraceEventType.Error, EvId, FormatMessage(BuildErrorMessage(Message, ex, showTrace,ue)));
        }


        public static void LogEvent(TraceEventType EventType, string Message, int EvId)
        {
            LogMessageInternal(EventType , EvId, Message);

        }


        public static void LogError(Exception ex, bool showTrace)
        {
            LogError(null, ex, showTrace);
        
        }

        private static string FormatMessage(string Message)
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.tt") + Message;
        }

        private static void LogMessageInternal( TraceEventType eventType, int EventId, string Message)
        {

          CurrentInstance.LogTraceEvent( eventType, EventId, Message );
        }




        public static string BuildErrorMessage(string Message, Exception ex, bool showTrace, UserError ue)
    {
        


        StringBuilder MessBuilder = new StringBuilder();

       string CustId = null;
        WPSProcessException pex = ex as WPSProcessException;
        if (pex != null && pex.Error != null)
            CustId = pex.Error.CustomerID;
         
        if (!string.IsNullOrEmpty(Message))
        {
            MessBuilder.Append(Message);
            MessBuilder.Append(" - ");
        }

        if (!string.IsNullOrEmpty(CustId))
        {
            MessBuilder.Append(" CustomerId: ");
            MessBuilder.Append(CustId);
            MessBuilder.Append(" - ");
        }
        MessBuilder.Append("Exception: ");
        MessBuilder.Append(Message);
        MessBuilder.AppendLine(ex.Message);
        Exception inerex = ex;
        while (inerex.InnerException != null)
        {
            inerex = inerex.InnerException;
            MessBuilder.AppendLine(inerex.Message);

        }
        if (showTrace)
            MessBuilder.Append(ex.StackTrace);
        return MessBuilder.ToString();
    }
     
        public static UserError MapExceptionLogStack(Exception ex)
        {

            return ErrorMapContext.MapErrorCode(ex);
        }
    }
}
