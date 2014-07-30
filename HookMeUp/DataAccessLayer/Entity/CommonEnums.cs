using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPS.DL.Entity
{
    public enum UserRole
    {
        SystemAdmin,
        CSR,
        Admin,
        User
    }


    public enum DimensionUOM
    {
        Inches,
        cm       
    }

    public enum WeigthUOM
    {
        LBS,
        KG
    }   

public  enum AddressType
 {
 Sender,
 Recipient,
 AltReturn
 }

public enum EAIAddressType
{
    HOME,
    WORK,
    BILLING
}

public enum PaymentType
{
    PREPAY,
    SUBSCRIPTION,
    SUPPLY

}

public enum PaymentMethodName
{
    CC,
    RA,
    PP,
    ACH
}

public  enum LabelLocation   {
    
    lhs,
    rhs
}

   public  enum WPSCarrier {
    
   
        USPS=1,
        UPS=2,      
        FedEx=3,       
        DHL=4
    }




   public enum ProcessorType 
    {
        ShippingLabelOrderProcessor=0,
        StampOrderProcessor = 1,
        ShippingLabelReprintProcessor = 2,
        ShippingLabelRefundProcessor = 3,
        PackageTrackingProcessor=4,
        ShippingLabelBulkScanFormProcessor = 5,

    }


   public enum ServiceType
   {
       ShippingLabelDispense=0,
       SheetOfStampsDispense=1
   
   }
   public enum Feature 
    {
       None=0,
        BuyPostage=1,
        PrintShippingLabels=1,
        PrintStamps=2,
        PrintEnvelopes=3,
        ViewSimpleReport=4,
        AssignTransactionToAcct=5,
        PurchaseInsuranceforParcels=6,
        EditAddressBook=7,
        AllCarriers=8,
        SetupPeripherals=9,
        PrintBulkScanForm=10,
        PrintCustomForm=11,
        PurchaseSupplies=12,
        PrintReturnLabel=13,
        SchedulePickup=14,
        TrackShipment=15,
        GetHelp=16,
        SetAccountProfile=17,
        ViewAllUserSimpleReports=18,
        EditAccountingTransations=19,
        PurchaseFeatures=20,
        ChangeSubscription=21,
        ChangePaymentMethod=22,
        AddEditUser=23,
        AddEditSimpleAccount=24
    
    }

   public enum LabelSize
   {
       S85x11,
       S2x7,
       S4x6
   }

   public enum AddressBookFilterType
   {  
       eNone = 0, 
       eLastName, 
       eFirstName  ,
       eCompany , 
       eAddress, 
       eCity,
       eState, 
       eZip,
       eCategory,
       TextSearch=200,
       eLastNameStartWith=100
   }
   
   public enum DispenseRequestType
   {
       Stamp,
       PlainPaperLabel,
       LabelPrinterLabel,
       PostageCorrection
   }

   public enum RefundOriginatorType
   {
       User=1,
       Partner
   }
   public enum EnvelopeOrientation
   {
       VerticalInditiaFirst = 1,
       VerticalInditiaLast =2,
       HorizontalInditiaLast =3,
        HorizontalInditiaFirst =4
   }
}
