using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPS.DL.Entity
{
    public class SpecialServices 
    {
       public int ID { get; set; }
       public int CarrierID { get; set; }
       public string Name { get; set; }
       public string  DisplayName { get; set; }
       public string Description { get; set; }    
       public bool RequiredInputValue { get; set; }
       public int MaxInputValue { get; set; }
       public bool Active { get; set; }
       public int Type { get; set; }
       public bool HasForm { get; set; }
       public string FormUrl { get; set; }
       public string FormName { get; set; }
       public RequredPrereqService[] RequiredServices { get; set; }
       public string[] ExcludedServices { get; set; }
       public double ShippingCost { get; set; }
       public bool PartOfBaseRate { get; set; }
       public string More { get; set; }
       public override string ToString()
       {
           return "Sp service - "+this.DisplayName;
       }


       public SpecialServices copyForJson()
       {

           SpecialServices rez = new SpecialServices();
           rez.ID = ID;
           rez.CarrierID = CarrierID;
           rez.Name = Name;
           rez.DisplayName = DisplayName;
           rez.Description = Description;
           rez.RequiredInputValue = RequiredInputValue;
           rez.MaxInputValue = MaxInputValue;
           rez.Active = Active;
           rez.Type = Type;
           rez.HasForm = HasForm;
           rez.FormUrl = FormUrl;
           rez.FormName = FormName;
           rez.RequiredServices = RequiredServices;
           rez.ExcludedServices = ExcludedServices;
           rez.More = More; 
           return rez;

       }

    }

    public class RequredPrereqService
    {
        public int InputValue { get; set; }
        public string ServiceName { get; set; }    
     
    }


    public class AdditionalOptions
    {
        public int ID { get; set; }
        public int CarrierID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }
    }
   

}
