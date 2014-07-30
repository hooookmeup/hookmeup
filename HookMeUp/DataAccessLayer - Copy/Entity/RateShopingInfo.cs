using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPS.DL.Entity;

namespace WPS.DL.Entity
{
    public class RateShopingInfo
    {
        public WPSCarrier Carrier { get; set; }
        public MailClass MailClass { get; set; }
        public decimal ShippingCost { get; set; }
        public string MinDeliveryDays { get; set; }
        public string MaxDeliveryDays { get; set; }
        public string GauranteedDateTime { get; set; }
        public string ErrorInfo { get; set; }
        public bool IsDynammic { get; set; }


        public List<RateShopSpecialServices> AvailableSpecialServices { get; set; }

        public string DeliveryString
        {
            get
            {
                if (!string.IsNullOrEmpty(GauranteedDateTime))
                    return GauranteedDateTime;
                else
                {
                    int mindays;
                    int maxdays;
                    if (int.TryParse(MinDeliveryDays, out mindays) && int.TryParse(MaxDeliveryDays, out maxdays))
                    {
                        if (mindays != maxdays)
                            return string.Format("{0}-{1} Days", MinDeliveryDays, MaxDeliveryDays);
                        else
                        {

                            if (mindays > 1)
                                return string.Format("{0} Days", MinDeliveryDays);
                            else
                                return string.Format("{0} Day", MinDeliveryDays);
                        }
                    }
                    else
                    {
                        return MinDeliveryDays;
                    }
                }
            }
        }
    }
        public class RateShopSpecialServices
        {
                public string Name { get; set; }  
                public bool IsPartOfBaseRate { get; set; }  
               public decimal cost { get; set; }
        }
       
    }

