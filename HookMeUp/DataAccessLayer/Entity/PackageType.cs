using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPS.DL.Entity
{
    public class PackageType 
    {

         public int ID { get; set; }
         public int CarrierID { get; set; }
         public string Name { get; set; }
         public string Description { get; set; }
         public bool IsDomestic { get; set; }
         public bool Active { get; set; }
         public PackageDimensionRules DimensionRules { get; set; }
         public PackageWeightRules WeightRules { get; set; }
         public bool DimensionRequired { get; set; }
         public bool WeightSupported { get; set; }
         //public string Valid{get;set;}
         public string Available { get; set; }   
         public string DisplayName { get; set; }
         public string IntlDescription { get; set; }
         public string Rollover { get; set; }
         public int SortOrder { get; set; }
         public override string ToString()
         {
             return "Package: \"" + Name + "\" Max w lb: " + WeightRules.MaxWeightLB + ",oz: " + WeightRules.MaxWeightOz;
         }


        public void Initialize()
        {
            Active = true;
            DimensionRequired = (DimensionRules.MinLength > 0 || DimensionRules.MinDepth > 0 || DimensionRules.MinWidth > 0 || DimensionRules.MaxGirth > 0);

            WeightSupported = (WeightRules.MaxWeightLB > 0 || WeightRules.MaxWeightOz > 0);
            IsDomestic = true;
        }

    }



    public class PackageWeightRules
    {
        public string WeigthUOM { get; set; }
        public int MaxWeightLB { get; set; }
        public int MaxWeightOz { get; set; }

    }

    public class PackageDimensionRules
    {
        public string DimensionUOM { get; set; }
        public decimal MinLength { get; set; }
        public decimal MinWidth { get; set; }
        public decimal MinDepth { get; set; }

        public decimal MaxLength { get; set; }
        public decimal MaxWidth { get; set; }
        public decimal MaxDepth { get; set; }
        public decimal MaxGirth { get; set; }
        public decimal MaxLengthWidthHeightSum { get; set; }

    }


    public class PackageDimencionWeightInfo
    {

        public DimensionUOM dimensionunits { get; set; }
        public WeigthUOM weightunits { get; set; }
        public decimal Height { get; set; }
        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public int KGorLB { get; set; }
        public int GMorOZ { get; set; }      
    }


    public class LabelPrintingInfo
    {
        public LabelSize Size { get; set; }
        public bool isPlainPaper { get; set; }
        public bool IncludeReceipt { get; set; }
        public LabelLocation lablocation { get; set; }
        public string PrinterName { get; set; }
    }

    public class Restrictioninfo
    {
        public string restrictions { get; set; }
        public string prohibtions { get; set; }
    }

    [Flags]
    public enum PackageAvailableOption
    {
        Label = 0x1,
        Stamp = 0x2,
        Envelope = 0x4
    }
}  
