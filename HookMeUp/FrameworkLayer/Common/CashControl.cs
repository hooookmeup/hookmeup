using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace SS.Framework.Common
{

    [Serializable]
    public class CashControl
    {

        private static CashControl _currentInstance = null;
        private static readonly object singletonLock = new object();

        public string DynamicResourcesVersion { get; set; }
        public int CashVersion { get; set; }
        public DateTime LastUpdate { get; set; }
        public int ReloadFrequence { get; set; }
        public List<string> DynamicPathList { get; set; }
        public string StaticResourcesVersion { get; set; }

        private static bool NeedUpdate
        {
            get
            {
                return (_currentInstance == null || (DateTime.Now - _currentInstance.LastUpdate) > TimeSpan.FromSeconds(_currentInstance.ReloadFrequence));
            }
        }

        public static CashControl CurrentInstance
        {
            get
            {
                if (NeedUpdate)
                {

                    lock (singletonLock)
                    {
                        if (NeedUpdate)
                        {
                            _currentInstance = CashControl.LoadData();
                            _currentInstance.LastUpdate = DateTime.Now;

                        }
                    }

                }
                return _currentInstance;
            }

        }

        private static CashControl LoadData()
        {

            XmlSerializer serializer = new XmlSerializer(typeof(CashControl));
            MemoryStream stream = Utility.readFromBlob("CashRules1.xml");
            return (CashControl)serializer.Deserialize(stream);
        }





        public static bool isMapDynamic(string requestUrl)
        {
            return (CurrentInstance.DynamicPathList.Contains(requestUrl.ToLower()));

        }


        public static void SaveInitialData()
        {
            CashControl init = new CashControl();
            init.DynamicResourcesVersion = "1.0";
            init.CashVersion = 1;
            init.ReloadFrequence = 100;
            init.DynamicPathList = new List<string>();
            init.DynamicPathList.Add("/image.jpeg");

            XmlSerializer serializer = new XmlSerializer(typeof(CashControl));
            MemoryStream stream = new MemoryStream();
            serializer.Serialize(stream, init);
            stream.Seek(0, SeekOrigin.Begin);
            Utility.UpdateBlob("wpsprivate", "CashRules1.xml", stream, "text/xml");


        }

        public static string HtmlDynamictVersion { get { return CurrentInstance.DynamicResourcesVersion; } }

        public static string HtmlStatictVersion { get { return CurrentInstance.StaticResourcesVersion; } }


    }


    public abstract class CashableVariable
    {
        int LastLoadedCashVersion;
        DateTime lastupdate;
        TimeSpan updateFrequency;

        protected static readonly object singletonLock = new object();



        protected CashableVariable()
        {
            updateFrequency = TimeSpan.FromDays(365);
            lastupdate = DateTime.Now;
        }

        protected CashableVariable(TimeSpan p_updateFrequency)
        {
            updateFrequency = p_updateFrequency;
            lastupdate = DateTime.Now;
        }

        public void UpdateLoadInfo()
        {

            lastupdate = DateTime.Now;
            LastLoadedCashVersion = CashControl.CurrentInstance.CashVersion;

        }

        public bool NeedUpdate
        {
            get
            {
                return (LastLoadedCashVersion != CashControl.CurrentInstance.CashVersion) || (DateTime.Now - lastupdate > updateFrequency);
            }
        }
    }
}
