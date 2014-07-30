using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPS.DL.Entity
{
   public struct  ReportSummary
    {
       public int StampPrinted;
       public int LabelPrinted;
       public int InsuredPackage;
       public double PostagePurchaged;
       public double PostageSpent;
       public bool NeedStampRecovery;
       public string StampRecoveryId;
    }
}
