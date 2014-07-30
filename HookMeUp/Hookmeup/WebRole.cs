using System;
using System.Linq;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using SS.Framework.Common;

namespace WebPostage
{
    public class WebRole : RoleEntryPoint
    {
        public override bool OnStart()
        {
            string logLevelstr =  RoleEnvironment.GetConfigurationSettingValue("LogLevel");
            string PerfCounterSpecifier =  RoleEnvironment.GetConfigurationSettingValue("PerfCounterSpecifier");
            string PerfSampleRate = RoleEnvironment.GetConfigurationSettingValue("PerfCounterSampleRate");
            LogLevel LogLevelEnum = LogLevel.Information;
            if (!string.IsNullOrEmpty(logLevelstr))
             LogLevelEnum = (LogLevel)Enum.Parse(typeof(LogLevel), logLevelstr, true); 

            

            DiagnosticMonitorConfiguration diagMonConf = DiagnosticMonitor.GetDefaultInitialConfiguration();
            Logger.SetScheduledLogStorage(ref diagMonConf, 1, LogLevelEnum);
            Logger.SetScheduledDiagnosticInfrastructureLogsStorage(ref diagMonConf, 1, LogLevel.Error);
            if (!string.IsNullOrEmpty(PerfCounterSpecifier))
            {
                int rate;
 
              if ( int.TryParse(PerfSampleRate, out rate))
                Logger.SetScheduledCountersStorage(ref diagMonConf, 1, PerfCounterSpecifier, rate);
            }
            
            diagMonConf.Directories.ScheduledTransferPeriod = TimeSpan.FromMinutes(1);

            diagMonConf.WindowsEventLog.ScheduledTransferLogLevelFilter = LogLevel.Error;
            diagMonConf.WindowsEventLog.ScheduledTransferPeriod = TimeSpan.FromMinutes(1);

            DiagnosticMonitor.Start("RemoteDataStorage", diagMonConf);

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            RoleEnvironment.Changing += RoleEnvironmentChanging;
            return base.OnStart();
        }

        private void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        {
            // If a configuration setting is changing
            if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
            {
                // Set e.Cancel to true to restart this role instance
                e.Cancel = true;
            }
        }
    }
}
