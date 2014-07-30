using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using WPS.Framework.Common;
using System.Xml.Serialization;
using System.IO;

namespace  WPS.DL.Entity
{
    public enum ElementType { Indicia, Qrbarcode, Text, DestAddress, OriginAddress }


    public class GraphicElement
    {
        public ElementSetting setting { get; set; }
        public double xPos { get; set; }
        public double yPos { get; set; }
        public string Value { get; set; }
    }
    [Serializable]
    public class ElementSetting
    {
        public ElementType Type { get; set; }
        public double x0 { get; set; }
        public double y0 { get; set; }
        public double dx { get; set; }
        public double dy { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public RotateFlipType Rotation { get; set; }

    }

    public class CellData
    {
        public int Location;
        public GraphicElement[] elements;
    }

    [Serializable]
    public class SheetTemplate
    {
        public string DisplayName { get; set; }
        public double SheetWidth { get; set; }
        public double SheetHeight { get; set; }
        public int ColumnsCount { get; set; }
        public int RowsCount { get; set; }
        public double xPrinterMargin { get; set; }
        public double yPrinterMargin { get; set; }
        public int xPrinterDpi { get; set; }
        public int yPrinterDpi { get; set; }
        public ElementSetting[] ElementSettings { get; set; }

        public static SheetTemplate getFullSheet
        {

            get
            {
                ElementSetting[] gre = new ElementSetting[2];

                //Sheet of Stamps

                //gre[0] = new ElementSetting();
                //gre[0].Type = ElementType.Indicia;

                //gre[0].Height = 1.4833333333;
                //gre[0].Width = 0.8;
                //gre[0].dx = 1.55;
                //gre[0].dy = 2.056;
                //gre[0].x0 = 1.43;
                //gre[0].y0 = 0.69;
                //gre[0].Rotation = RotateFlipType.Rotate90FlipNone;


                gre[0] = new ElementSetting();
                gre[0].Type = ElementType.Qrbarcode;

                gre[0].Height = 0.8;
                gre[0].Width = 0.8;
                gre[0].dx = 1.764;
                gre[0].dy = 3.361;
                gre[0].x0 = 1.967;
                gre[0].y0 = .804;
                gre[0].Rotation = RotateFlipType.RotateNoneFlipNone;




                gre[1] = new ElementSetting();
                gre[1].Type = ElementType.Indicia;

                gre[1].Height = 1.4833333333;
                gre[1].Width = 0.8;
                gre[1].dx = 1.764;
                gre[1].dy = 3.361;
                gre[1].x0 = 1.967;
                gre[1].y0 = 2.105;
                gre[1].Rotation = RotateFlipType.Rotate90FlipNone;

                SheetTemplate rz = new SheetTemplate
                {
                    DisplayName = "Sheet of Stamps with QR Barcode",
                    SheetWidth = 8.5,
                    SheetHeight = 11,
                    ColumnsCount = 4,
                    RowsCount = 4,
                    xPrinterMargin = 0.6,
                    yPrinterMargin = 0.6,
                    xPrinterDpi = 300,
                    yPrinterDpi = 300,
                    ElementSettings = gre
                };

                return rz;
            }
        }

    }

    public class SheetTemplates : CashableVariable
    {
        Dictionary<string, SheetTemplate> TemplateDic;

        private static SheetTemplates _setting;


        public static SheetTemplates Current
        {
            get
            {
                if (_setting == null || _setting.NeedUpdate)
                {
                    lock (singletonLock)
                    {
                        if (_setting == null || _setting.NeedUpdate)
                        {
                            _setting = new SheetTemplates();
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
            XmlSerializer serializer = new XmlSerializer(typeof(List<SheetTemplate>));
            MemoryStream stream = Utility.readFromBlob("SheetTemplates.xml");
            List<SheetTemplate> TemplatesList = (List<SheetTemplate>)serializer.Deserialize(stream);
            TemplateDic = new Dictionary<string, SheetTemplate>();
            foreach (var tpl in TemplatesList)
                TemplateDic.Add(tpl.DisplayName, tpl);
        }


        public static void SaveInitialTemplates()
        {

            List<SheetTemplate> list = new List<SheetTemplate>();
            SheetTemplate fr = SheetTemplate.getFullSheet;
            list.Add(fr);

            XmlSerializer serializer = new XmlSerializer(typeof(List<SheetTemplate>));
            MemoryStream stream = new MemoryStream();
            serializer.Serialize(stream, list);
            stream.Seek(0, SeekOrigin.Begin);
            Utility.UpdateBlob("wpsprivate", "SheetTemplates.xml", stream, "text/xml");

        }


        public static SheetTemplate Template(string DisplayName)
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
