using System;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using System.Configuration;
using System.Runtime.Serialization;


namespace SS.DL.AzureTableStorage
{
  
	
    public struct NextRecordInfo
    {
        public string NextPartition { get; set; }
        public string NextKey { get; set; }
    }
	
// Embeded attribute Enumerations	
    public enum QuestionType { Text, CheckBox, Select, MultiSelect, Radio, DropDown }
    public enum AppStateType { NoState, AccountCreated,UserInfoUpdated,AgeCheckComplete, QuestionaireCompleted, PrefernceComplete, TCComplete }
    // Embeded attribute structs
    [Serializable]
    public class CategoryScore {
    public  string   	CategoryId                { get; set; }
    public  int      	Score                     { get; set; }
    }
    
    [Serializable]
    public class QuestionOptions {
    public  string   	Name                      { get; set; }
    public  string   	Value                     { get; set; }
    }
    
    [Serializable]
    public class DeviceInfo {
    public  string   	Name                      { get; set; }
    public  string   	Version                   { get; set; }
    public  string   	Platform                  { get; set; }
    public  string   	UUID                      { get; set; }
    public  string   	PhGapVersion              { get; set; }
    }
    
    
       public class EntityContext
        {
          protected  string TableName;
          protected string ConfigurationName;
          protected TableServiceContext servCtx;
          protected CloudStorageAccount StorageAccount;
          public static byte[] EncKey = { 123, 12, 23, 45, 72, 98, 211, 23 };
           
            public EntityContext(string p_TableName, string p_ConfigurationName)
            {
                TableName = p_TableName;
                ConfigurationName = p_ConfigurationName;
                StorageAccount = CloudStorageAccount.FromConfigurationSetting(p_ConfigurationName);
                CloudTableClient TableClient = StorageAccount.CreateCloudTableClient();
                TableClient.CreateTableIfNotExist(TableName);
                servCtx = TableClient.GetDataServiceContext();
    
            }
    
            
            public void SaveChangesWithRetries()
            {
                 servCtx.SaveChangesWithRetries();
            }
    
           protected static string _defaultConfiguration;
    
            public static string DefaultConfiguration
            {
                get
                {
                    return _defaultConfiguration;
                }
                set
                {
                    _defaultConfiguration = value;
                }
            }
           protected Uri IdentityURI(string p_Partition, string p_Row)
           {
               return new Uri( String.Format("{0}{1}(PartitionKey='{2}',RowKey='{3}')", servCtx.BaseUri,TableName, p_Partition,p_Row));
     
           }
            
    		
    		
    		public static void  setForAppConfig()
               {
                   CloudStorageAccount.SetConfigurationSettingPublisher( (configName, configSetter) =>  { configSetter(ConfigurationManager.AppSettings[configName]); } );
               }
       }
    }
    
    
