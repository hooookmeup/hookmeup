using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using WPS.Framework.Common;
using System.Xml.Serialization;
using System.IO;

namespace WPS.DL.Entity
{


    public class PrinterSettingTranslation : CashableVariable
    {
        Dictionary<string, int> ValueDic;

        private static PrinterSettingTranslation _setting;




        public static PrinterSettingTranslation Current
        {
            get
            {
                if (_setting == null || _setting.NeedUpdate)
                {
                    lock (singletonLock)
                    {
                        if (_setting == null || _setting.NeedUpdate)
                        {
                            _setting = new PrinterSettingTranslation();
                            _setting.LoadTemplates();
                            _setting.UpdateLoadInfo();

                        }
                    }

                }
                return _setting;
            }
        }

        private void LoadTemplates()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<string>));
            MemoryStream stream = Utility.readFromBlob("PrinterFeedValues.xml");
            List<string> settingsList = (List<string>)serializer.Deserialize(stream);
            ValueDic = new Dictionary<string, int>();
            foreach (var key in settingsList)
            {
                string[] keyval = key.Split(',');
                if (keyval.Length ==2)
                ValueDic.Add(keyval[0].Trim(), int.Parse(keyval[1].Trim()));
            }
        }


        public static void SaveInitialSetting()
        {

            List<string> settingsList = new List<string>();
          
            settingsList.Add("left_faceup_bottomfeed,1");
            settingsList.Add("middle_faceup_bottomfeed,2");
            settingsList.Add("right_faceup_bottomfeed,3");

            settingsList.Add("left_facedown_bottomfeed,1");
            settingsList.Add("middle_facedown_bottomfeed,2");
            settingsList.Add("right_facedown_bottomfeed,3");

            settingsList.Add("left_faceup_topfeed,1");
            settingsList.Add("middle_faceup_topfeed,2");
            settingsList.Add("right_faceup_topfeed,3");

            settingsList.Add("left_facedown_topfeed,1");
            settingsList.Add("middle_facedown_topfeed,2");
            settingsList.Add("right_facedown_topfeed,3");




            XmlSerializer serializer = new XmlSerializer(typeof(List<string>));

            MemoryStream stream = new MemoryStream();
            serializer.Serialize(stream, settingsList);
            stream.Seek(0, SeekOrigin.Begin);
            Utility.UpdateBlob("wpsprivate", "PrinterFeedValues.xml", stream, "text/xml");

        }


        public static int InternalNumber(string key)
        {
            if (Current.ValueDic.ContainsKey(key))
                return Current.ValueDic[key];
            else
                return 2;

        }
        
    }


    public class CustomerPrinterSetting
    {
        public bool isFeedFromUp { get; set; }
        public bool isFaceUp { get; set; }
        public int OrientationNumber { get; set; }
        public int InternalNumber
        {
            get
            {
                return PrinterSettingTranslation.InternalNumber(settingKey);
            }

        }

        private string settingKey
        {

            get
            {
                string key = "middle";
                switch (OrientationNumber)
                {
                    case 1:
                        key = "left";
                        break;
                    case 2:
                        key ="middle";
                        break;
                    case 3:
                        key="right";
                        break;

                }
                if ( isFaceUp)
                    key = key+"_faceup";
                else
                    key = key+"_facedown";

                if ( isFeedFromUp )
                    key = key + "_topfeed";
                else
                   key = key+"_bottomfeed";
                    
                
                return key;
            }

             
        }
    }
}
