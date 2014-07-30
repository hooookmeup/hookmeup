using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Web;

namespace SS.Framework.Common
{
    public class EmailManager
    {

        Cookie authCookie;

        public Cookie createCookie(string Str)
        {
            Cookie cook = null;
            Dictionary<string,string>  cookie = new  Dictionary<string,string>();
            string[] els = Str.Split(';');
            foreach (var str in els)
            {
                string str2 = str.Trim();
                int ind = str2.IndexOf('=');
                if (ind > 0)
                {
                    cookie.Add(str2.Substring(0,ind).ToLower(),str2.Substring(ind+1,str2.Length-ind-1).Trim());
                }
               
            }
            cook = new Cookie();
            cook.Domain = cookie["domain"];
            cook.Expires = DateTime.Parse(cookie["expires"]);
            cook.Path = cookie["path"];
            cook.Value = cookie["pubauth1"];
            cook.Name = "PubAuth1";
            return cook;

        }



        private void CtreateAuthCookie()
        {
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(Utility.GetHighstPriorityPatameter("Email.EmailBaseUrl") + String.Format("/api/login1?name={0}&cleartext={1}", "ccapi", "CC01@pb"));
            myReq.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)myReq.GetResponse();
            string cookieinfo = response.Headers["Set-Cookie"];
            authCookie = createCookie(cookieinfo);
               
        }




        public void Send(string address, string templateId, Dictionary<string,string> parameters)
        {
                if (authCookie == null || authCookie.Expires < DateTime.Now)
                    CtreateAuthCookie(); 


              

                ASCIIEncoding encoding = new ASCIIEncoding();
                string postData; 
                
                postData = ("aid=192378631");
                postData += ("&eid="+templateId);
                postData += ("&email=" + address);
           //     postData += ("&PROD_PCN1=18");
            //    postData += ("&b=1");
                if (bool.Parse(Utility.GetHighstPriorityPatameter("Email.UseStaging")))
                    postData += ("&Test=1");

                if (parameters != null)
                {
                    foreach (string key in parameters.Keys)
                    {
                        postData += string.Format("&{0}={1}", key,parameters[key]);
                    }
                }

              
                byte[] data = encoding.GetBytes(postData);

                HttpWebRequest mySendReq = (HttpWebRequest)WebRequest.Create(Utility.GetHighstPriorityPatameter("Email.EmailBaseUrl") + "/ebm/ebmtrigger1");
                mySendReq.Method = "POST";
                mySendReq.CookieContainer = new CookieContainer();
                mySendReq.CookieContainer.Add(authCookie);
                mySendReq.ContentType = "application/x-www-form-urlencoded";
               // mySendReq.ContentType = "text/plain";             
                mySendReq.ContentLength = data.Length;

                Stream newStream = mySendReq.GetRequestStream();
                // Send the data.
                newStream.Write(data, 0, data.Length);
                newStream.Close();
                HttpWebResponse WebSendResp = (HttpWebResponse)mySendReq.GetResponse();
                //Let's show some information about the response
                //Console.WriteLine(WebSendResp.StatusCode);
                //Console.WriteLine(WebSendResp.Server);

                //Now, we read the response (the string), and output it.
                Stream Answer = WebSendResp.GetResponseStream();
                StreamReader _Answer = new StreamReader(Answer);
               string rez = _Answer.ReadToEnd();
              
               Answer.Close();

            }
        }
}
