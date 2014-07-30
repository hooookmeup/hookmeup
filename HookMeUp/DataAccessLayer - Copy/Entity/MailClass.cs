using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPS.DL.Entity
{
   public class MailClass
   {
       public int ID { get; set; }
       public int CarrierID { get; set; }
       public string Name { get; set; }
       public string DisplayName { get; set; }
       public string Description { get; set; }       
       public int MaxWeightLB { get; set; }
       public int MaxWeightOz { get; set; }
       public bool Active { get; set; }
       public bool Domestic { get; set; }
    //   public Dictionary<int, PackageType> packagesSupported { get; set; }
       public Dictionary<int, List<SpecialServices>> SpecialServicesSupported { get; set; }
       public List<AdditionalOptions>[] AdditionalOptions { get; set; }
        
    }


  
}
