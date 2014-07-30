using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPS.DL.Entity;
using WPS.Framework.Common;


namespace WPS.DL.Entity
{
  
    [Serializable]
    public class CreditCardInfo
    {
        public string CCType { get; set; }
        public string number { get; set; }
        public string name { get; set; }
        public string expiration { get; set; }
        public DateTime LastChange { get; set; }

        public bool isSame(CreditCardInfo ccinfo)
        {
            return (number == ccinfo.number && name == ccinfo.name);
        }

    }


    [Serializable]
    public class UserProfile
    {
        public UserIDs UserIDs;
        public Address UserAddress;
        public ErrorInfo UserError;
        public bool PrepayAccountConfiged=false;
        public double PrepayAccountBalance=0.0;
        public bool PreferredPaymentMethodConfiged = false;
        public string PostagePaymentDislayInfo=string.Empty;
        public bool UserLogedIn = false;       
        public bool IsCSRAgent { get { return string.IsNullOrEmpty(UserIDs.CSRAgentID); } }
        public string LastTXId = String.Empty;
        public string PostagePaymentMethodDislayInfo = string.Empty;
        public bool isCCCustomer { get { return (CCInfo != null); } } 
        public string AccountSubsriptionStatus;
        public double PrepayPurchasedPostage = 0.0;
        public double PrepayFreePostage = 0.0;
        public bool IsPortalLogin { get { return (!String.IsNullOrEmpty(UserIDs.LoginProvider)&& UserIDs.LoginProvider.ToLowerInvariant().Contains("sbp")); }}
        public CreditCardInfo CCInfo { get; set; } 
        public string UserProductID { get; set; }
        public bool IsStampsOnly {
            get
            {
                ErrorInfo err = FeatureSetting.isFeatureEnabled("DispenseLabel", UserIDs.LoginID, UserProductID);
                return (err.IsError);
            }
        }

    }
   [Serializable]
    public class UserIDs
    {
        public string LoginID;
        public string UserBPN;
        public string CustomerBPN;
        public string PreapyAccountID;
        public string SubscriptionID;
        public string CAN;
        public string CSRAgentID;
        public string UserRoles;
        public string UserIPAddress;
        public string LoginProvider;
    }
}
