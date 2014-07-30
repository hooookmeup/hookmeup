using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.Framework.Common
{
   public sealed class ErrorCodes
    {
        private ErrorCodes() { }
              
       //user profile internal
        public const string UserDoesNotExist = "WPS001";
         public const string InvalidToken = "WPS002";
         public const string UserDisabled = "WPS005";
       
        // Generic 

        // parameter
        public const string NullParameter = "WPS201";
        public const string LengthExceeds = "WPS202";
        public const string InvalidParameter = "WPS204";
        
        // IOP Proxy  execution
        
        public const string IOPError = "WPS301";
        public const string IOPExeption = "WPS302";

        // Refunds 
        public const string RefundGenericException = "WPS401";

       // Address book 
        public const string AddressBookGenericException = "WPS501";
        public const string AddressBookVerificationError = "WPS502";

        //EAI Proxy errors
        public const string EAIError = "WPS601";
        public const string EAIExeption = "WPS602";
        public const string EAINullParameter = "WPS603";
        public const string EAIFileFormatError = "WPS604";
        public const string EAINoPreferredPaymentMethod = "WPS605";
        public const string EAINoSufficientPrepayBalance = "WPS606";
        public const string EAINoCustomerData = "WPS607";
        public const string EAINoSubscriptionDetails = "WPS608";
        public const string EAINoPrepaiddetails = "WPS609";
        public const string EAIINVALIDRESERVEACCOUNT = "WPS610";
        public const string EAIINSUFFICENTRESERVEACCOUNT = "WPS611";
        public const string EAIPURCHASEPOWERAUTHFAILED = "WPS612";
        public const string EAIRESERVEACCOUNTNOTSETPROPERLY = "WPS613";
        public const string EAIINVALIDCANACCOUNT = "WPS614";
        public const string EAICreditCardNotAuth = "WPS615";
        public const string EAIBLOCKEDCANACCOUNT = "WPS616"; 

       //Label Error codes
        public const string LabelGenericException = "WPS701";
        public const string LabelValidationError = "WPS702";
        public const string MaxReprintCount = "WPS703";
        public const string BalanceNotAvailable = "WPS704";
        public const string ReprintNotAvailable = " WPS705";
        public const string InvalidZipCode = "WPS706";
        public const string InvalidShippingDate = "WPS707";

    
        // Printing
        public const string InvalidPrintState = "WPS801";
        // Home Errors
        public const string HomeGenericException = "WPS901";
       //Reports Error
        public const string ReportGenericException = "WPS1001";
        public const string InvalidTrackingNumber ="WPS1002";

       // Stamp Error codes 
        public const string StampGenericException = "WPS1101";
        public const string StampSheetUsedByAnotherUser = "WPS1102";
        public const string StampOnly25PerSheet = "WPS1103";
        public const string StampInvalidSheetNumber = "WPS1104";
        public const string StampsBalanceNotAvailable = "WPS1105";
        public const string StampsSheetBetaVersion = "WPS1106";
        public const string StampsOnlyOneReprint = "WPS1107";
        public const string StampsNoReprintWithoutDestination = "WPS1108";
        public const string StampsOnlyEnvNotAvilable = "WPS2001";
       
       //Fraud detection Codes
        public const string DisabledPurchaseByFDS = "WPS1201";

       
   }

      /* 
       
       */
   [Serializable]
   public sealed class ErrorInfo
   {

       public ErrorInfo()
       {

       }

       public ErrorInfo(string code)
       {
           Code = code;

       }

       public ErrorInfo(string code, string message)
       {
           Code = code;
           Message = message;

       }

       public ErrorInfo(string code, string message, string details)
       {
           Code = code;
           Message = message;
           Details = details;
       }

       public ErrorInfo(string code, string message, string details, string source)
       {
           Code = code;
           Message = message;
           Details = details;
           Source = source;
       }

       public ErrorInfo(string code, string message, string details, string source, string Cust_d)
       {
           Code = code;
           Message = message;
           Details = details;
           Source = source;
           CustomerID = Cust_d;
       }

       public static ErrorInfo NoError { get { return new ErrorInfo(); } }

       public string Code { get; set; }
       public string Message { get; set; }
       public string Details { get; set; }
       public string Source { get; set; }
       public string CustomerID { get; set; }
       public bool IsError { get { return !string.IsNullOrEmpty(Code); } }

       public void ThrowException()
       {
           Logger.LogError(ErrorMessage);
           throw new WPSProcessException(this);
       }


       private string ErrorMessage
       {
           get
           {
               string rez = this.Code + ": " + Message;
               if (!string.IsNullOrEmpty(CustomerID))
                   rez +="  CustomerID: " + CustomerID;
               if (!string.IsNullOrEmpty(Details))
                   rez += " Details: " + Details;
               return rez;
           }

       }
   }

       public class WPSProcessException : Exception
       {
           public ErrorInfo Error { get; set; }

           public WPSProcessException(ErrorInfo error)
               : base(error.Message)
           {
               Error = error;
           }

           public WPSProcessException(ErrorInfo error, Exception innerexception)
               : base(error.Message, innerexception)
           {
               Error = error;
           }

          }


    }

