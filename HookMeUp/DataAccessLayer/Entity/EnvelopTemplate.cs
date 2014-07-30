using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPS.Framework.Common;
using System.Xml.Serialization;
using System.IO;

namespace WPS.DL.Entity
{
  public  class EnvelopeTemplate
    {
      public string DisplayName { get; set; }
      public double EnvWidth { get; set; }
      public double EnvHeight { get; set; }
      public double FimXOffset { get; set; }
      public double FimYOffset { get; set; }
      public double IndiciaXOffset { get; set; }
      public double IndiciaYOffset { get; set; }
      public double ReturnAddressXOffset { get; set; }
      public double ReturnAddressYOffset { get; set; }
      public double DestinationAddressXOffset { get; set; }
      public double DestinationAddressYOffset { get; set; }
      public double DPBarcodeXOffset { get; set; }
      public double DPBarcodeYOffset { get; set; }
      public int ReturnAddressFontSize { get; set; }
      public int DestinationAddressFontSize { get; set; }
     

        public static EnvelopeTemplate getEnv10
        {
            get
            {
                EnvelopeTemplate rz = new EnvelopeTemplate
                {
                    DisplayName = "Envelope 10",
                    EnvWidth = 9.25,
                    EnvHeight = 4.125,
                    FimXOffset = 7,
                    FimYOffset = 0,
                    IndiciaXOffset = 7.75,
                    IndiciaYOffset = 0.25,
                    ReturnAddressXOffset = 0.25,
                    ReturnAddressYOffset = 0.25,
                    DestinationAddressXOffset = 4,
                    DestinationAddressYOffset = 2.5,
                    DPBarcodeXOffset = 5.0,
                    DPBarcodeYOffset = 3.75,
                    ReturnAddressFontSize = 28,
                    DestinationAddressFontSize =36
                };

                return rz;
            }
        }


    }


  public class EnvelopeTemplates : CashableVariable
  {
      Dictionary<string, EnvelopeTemplate> TemplateDic;

      private static EnvelopeTemplates _setting;




      public static EnvelopeTemplates Current
      {
          get
          {
              if (_setting == null || _setting.NeedUpdate)
              {
                  lock (singletonLock)
                  {
                      if (_setting == null || _setting.NeedUpdate)
                      {
                          _setting = new EnvelopeTemplates();
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
          XmlSerializer serializer = new XmlSerializer(typeof(List<EnvelopeTemplate>));
          MemoryStream stream = Utility.readFromBlob("EnvelopeTemplates.xml");
          List<EnvelopeTemplate>  TemplatesList = (List<EnvelopeTemplate>)serializer.Deserialize(stream);
          TemplateDic = new Dictionary<string, EnvelopeTemplate>();
          foreach (var tpl in TemplatesList)
              TemplateDic.Add(tpl.DisplayName, tpl);
      }


      public static void SaveInitialTemplates()
      {

          List<EnvelopeTemplate> list = new List<EnvelopeTemplate>();

          EnvelopeTemplate fr = EnvelopeTemplate.getEnv10;

          list.Add(fr);


          XmlSerializer serializer = new XmlSerializer(typeof(List<EnvelopeTemplate>));

          MemoryStream stream = new MemoryStream();
          serializer.Serialize(stream, list);
          stream.Seek(0, SeekOrigin.Begin);
          Utility.UpdateBlob("wpsprivate", "EnvelopeTemplates.xml", stream, "text/xml");

      }


      public static EnvelopeTemplate Template(string DisplayName)
      {
          if (Current.TemplateDic.ContainsKey(DisplayName))
              return Current.TemplateDic[DisplayName];
          else
              return null;

      }
      public static List<string> TemplateNames
      {
          get
          {

              return new List<string>(Current.TemplateDic.Keys);


          }
      }
  }

}
