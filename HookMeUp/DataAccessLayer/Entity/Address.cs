using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPS.DL.Entity
{
    [Serializable]
    public class Address
    {
        public const float MaxDaysForVerification = 60;
        public string AddressID { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string LastName { get; set; }
        public string Home { get; set; }
        public string Cell { get; set; }
        public string Work { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Zip4 { get; set; }
        public string Company { get; set; }
        public AddressType Type { get; set; }
        public string FirstName { get; set; }
        public string MI { get; set; }
        public string AddressmatchingFlag { get; set; }
        public string ThirdPartyEmail { get; set; }        
        public DateTime LastVerified{get;set;}
        public bool IsVerified { get { return ((DateTime.Now - LastVerified).TotalDays < Address.MaxDaysForVerification); } }
   
        private bool isUS
        {
            get
            {
                return ( Country==null || Country=="US");
            }
        }

        public string CombinedZip
        {
            get
            {
                if (isUS && !string.IsNullOrEmpty(Zip4))
                    return ZipCode + "-" + Zip4;
                else
                    return ZipCode;

            }
            set
            {
                if (isUS && !string.IsNullOrEmpty(value))
                {
                    string tzip, tzip4;
                    Address.SplitCombinedZip(value, out tzip, out tzip4);
                    ZipCode = tzip;
                    Zip4 = tzip4;
                 }
                else
                    ZipCode = value;
            }
        }

        


        public static void SplitCombinedZip(string combZip, out string zipcode, out string zip4)
        {

            string[] zipCodeSplit = combZip.Split('-');

            if (zipCodeSplit.Length > 1)
            {
                zipcode = zipCodeSplit[0].Trim();
                zip4 = zipCodeSplit[1].Trim();
            }
            else
            {
                zipcode = zipCodeSplit[0].Trim();
                zip4 = "";
            }

        }



   /*     public void CombineZip()
        {
            if (!string.IsNullOrEmpty(Zip4) && !string.IsNullOrEmpty(ZipCode))
            {
                ZipCode = ZipCode + "-" + Zip4;

            }

        }


     
        public EnvelopeAddress EnvelopeAddress
        {
            get
            {
                EnvelopeAddress addr = new EnvelopeAddress();
                if (!String.IsNullOrEmpty(Company))
                    addr.AddressLine1 = Company;
                if (!String.IsNullOrEmpty(Name))
                    addr.AddressLine2 = Name;
              
                    addr.AddressLine3 = Address1;

                    if (!String.IsNullOrEmpty(Address2))
                        addr.AddressLine3 +=  Address2;
                    addr.AddressLine4 = String.Format("{0} {1} {2}", City, State, CombinedZip);
                    addr.BarcodeBase = CombinedZip;
                    addr.PrintSmartBarcode = true;
                    return addr;
            }
        }
    */
    }

    [Serializable]
    public class AddressList
    {
        public Address[] Addresses { get; set; }
    }

    public class CountryInfo
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string ZipCodeFormat { get; set; }

        public CountryInfo(string p_code, string p_name, string p_zipform)
        {
            Code = p_code;
            Name = p_name;
            ZipCodeFormat = p_zipform;
        }

    }
    /*
    public class EnvelopeAddress
    {
        public string BarcodeBase  { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }
        public bool PrintSmartBarcode { get; set; }
    }
    */


    [Flags]

   public enum EnvelopePrintOptions
    {
        PrintBarcode = 0x1,
        PrintOrigin = 0x2,
        PrintDestination = 0x4,
        PrintBurcodeAfter = 0x8
        
    }
    [Serializable]
    public class EnvelopePrintSetting
    {
     public   EnvelopePrintOptions options;
     public string EnvelopeTemplate;
    }
} 
