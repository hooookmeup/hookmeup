using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Net;
using SS.DL.AzureTableStorage;
using SS.Framework.Common;

namespace Hookmeup.Models
{
    public class OnlineScheduler
    {

         /// <summary>
    /// Determines the status fo the Scheduler
    /// </summary>        
    public bool Cancelled
    {
        get { return _Cancelled; }
        set { _Cancelled = value; }
    }
    private bool _Cancelled = false;
 
 
    /// <summary>
    /// The frequency of checks against hte POP3 box are 
    /// performed in Seconds.
    /// </summary>
    private int CheckFrequency = 5;
 
    AutoResetEvent WaitHandle = new AutoResetEvent(false);
 
    object SyncLock = new Object();
 
    public OnlineScheduler()
    {
 
    }
 
    /// <summary>
    /// Starts the background thread processing       
    /// </summary>
    /// <param name="CheckFrequency">Frequency that checks are performed in seconds</param>
    public void Start(int checkFrequency)
    {
        // *** Ensure that any waiting instances are shut down
        //this.WaitHandle.Set();
 
        this.CheckFrequency = checkFrequency*1000;
        this.Cancelled = false;
 
        Thread t = new Thread(Run);
        t.Start();
    }
 
    /// <summary>
    /// Causes the processing to stop. If the operation is still
    /// active it will stop after the current message processing
    /// completes
    /// </summary>
    public void Stop()
    {
        lock (this.SyncLock)
        {
            if (Cancelled)
                return;
 
            this.Cancelled = true;
            this.WaitHandle.Set();
        }
    }
 
    /// <summary>
    /// Runs the actual processing loop by checking the mail box
    /// </summary>
    private void Run()
    {
        // *** Start out  waiting
        this.WaitHandle.WaitOne(this.CheckFrequency , true);

     

 
        while (!Cancelled)
        {

            UserContext utx = new UserContext();

            List<User> users = utx.GetOfflineUser(this.CheckFrequency);

            if (users != null)
            {
                foreach (User user in users)
                {
                    user.IsOnline = false;

                    utx.UpdateObject(user, false);
                }
            }
 
            // *** Put in 
            this.WaitHandle.WaitOne(this.CheckFrequency ,true);
 
        }
 
    
    }
 
        /*
    public void PingServer()
    {
        try
        {
            WebClient http = new WebClient();
            string Result = http.DownloadString(App.Configuration.PingUrl);
        }
        catch (Exception ex)
        {
            string Message = ex.Message;
        }
    }
         */
 
 
    
 
    public void Dispose()
    {
        this.Stop();
    }
    }
}