using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using System.Data.Services.Client;

namespace SS.Framework.Common
{
    public class UserError
    {
        public string errorCode {get;set;}
        public string errorMessage { get; set; }
        public int eventId { get; set; }
        public bool isUserError { get; set; }
    }


    internal class ErrorMappings : TableServiceEntity
    {

        public ErrorMappings()
        {

        }

        public string ErrorMessage { get; set; }
        public bool UserError { get; set; }

    }


    public class ErrorCodeMapping : TableServiceEntity
    {
        public string ClassName { get; set; }
        public string MethodName { get; set; }
        public string ErrorMessage { get; set; }
        public int EventId { get; set; }
    }

    public class ErrorMapContext : CashableVariable
    {
        private string TableName = "ErrorMappings";
        private string ConfigurationName = "RemoteDataStorage";
        private TableServiceContext servCtx;
        private CloudStorageAccount StorageAccount;

        private static Dictionary<string, UserError> _mapping = null;

        private static ErrorMapContext _currentInstance;
        private Dictionary<string, Dictionary<string, UserError>> _errorCodeMap;

        public ErrorMapContext()
        {
         
            StorageAccount = CloudStorageAccount.FromConfigurationSetting(ConfigurationName);
            CloudTableClient TableClient = StorageAccount.CreateCloudTableClient();
            TableClient.CreateTableIfNotExist(TableName);
            servCtx = TableClient.GetDataServiceContext();

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

        

        /// <summary>
        /// Load instance of  ErrorMapping object from Context.
        /// </summary>
        /// <param name="p_Partition">Partition key of the table</param>
        /// <param name="p_Row">Row key of desired object </param>


   /*     private ErrorMappings GetById(string p_Partition, string p_Row)
        {
            ErrorMappings rez = null;
            if (!servCtx.TryGetEntity<ErrorMappings>(IdentityURI(p_Partition, p_Row), out rez))
            {
                var Qtran = servCtx.CreateQuery<ErrorMappings>(TableName).Where(c => c.PartitionKey == p_Partition && c.RowKey == p_Row);
                var enumer = Qtran.GetEnumerator();
                if (enumer.MoveNext())
                    rez = enumer.Current;
            }
            return rez;
        }
*/
        /// <summary>
        /// Load List of all ErrorMapping objects from Context for Partition Key.
        /// </summary>
        /// <param name="p_Partition">Partition key of the table</param>
        /// <returns>List of all ErrorMapping </returns>

        private List<ErrorMappings> GetAllErrorMappings()
        {
            List<ErrorMappings> rez = new List<ErrorMappings>();
            DataServiceQuery<ErrorMappings> Qtran = (DataServiceQuery<ErrorMappings>)servCtx.CreateQuery<ErrorMappings>(TableName).Where(c => c.PartitionKey == "WPS");
            IEnumerable<ErrorMappings> qt = Qtran.Execute();
              rez.AddRange(qt.ToList());
            return rez;
        }

        private List<ErrorCodeMapping> GetAllErrorCodeMappings()
        {
            List<ErrorCodeMapping> rez = new List<ErrorCodeMapping>();
            DataServiceQuery<ErrorCodeMapping> Qtran = (DataServiceQuery<ErrorCodeMapping>)servCtx.CreateQuery<ErrorCodeMapping>(TableName).Where(c => c.PartitionKey == "WPSERRCODES");
            IEnumerable<ErrorCodeMapping> qt = Qtran.Execute();
            rez.AddRange(qt.ToList());
            return rez;
        }


      
        protected Uri IdentityURI(string p_Partition, string p_Row)
        {
            return new Uri(String.Format("{0}{1}(PartitionKey='{2}',RowKey='{3}')", servCtx.BaseUri, TableName, p_Partition, p_Row));

        }


        private Dictionary<string, Dictionary<string, UserError>> ErrorCodeMap
        {
            get
            {
                if (_errorCodeMap == null || NeedUpdate)
                {
                    _errorCodeMap = new Dictionary<string, Dictionary<string, UserError>>();
                    
                    List<ErrorCodeMapping> eml = GetAllErrorCodeMappings();
                    foreach (ErrorCodeMapping em in eml)
                    {
                        Dictionary<string, UserError> KeyDictionary;
                       if (_errorCodeMap.ContainsKey(em.ClassName))
                       {
                           KeyDictionary = _errorCodeMap[em.ClassName];
                       }

                       else
                       {
                           KeyDictionary = new Dictionary<string, UserError>();
                           _errorCodeMap.Add(em.ClassName, KeyDictionary);
                       }
                       UserError ue = new UserError();
                       ue.errorCode = em.RowKey;
                       ue.errorMessage = em.ErrorMessage;
                       ue.eventId = em.EventId;
                       KeyDictionary.Add(em.MethodName, ue);
                    }
                    this.UpdateLoadInfo();
                }
                return _errorCodeMap;
            }
        }

        private Dictionary<string, UserError> Mapping
        {
            get
            {
                if (_mapping == null || NeedUpdate)
                {
                    _mapping = new Dictionary<string, UserError>();

                    List<ErrorMappings> eml = GetAllErrorMappings();
                    foreach (ErrorMappings em in eml)
                    {
                        UserError ue = new UserError();
                        ue.errorCode = em.RowKey;
                        ue.errorMessage = em.ErrorMessage;
                        ue.isUserError = em.UserError;
                        _mapping.Add(em.RowKey, ue);
                    }
                    this.UpdateLoadInfo();
                }
                return _mapping;
            }
        }


        private static ErrorMapContext CurrentInstance
        {
             get
            {
                if (_currentInstance == null || _currentInstance.NeedUpdate)
                {
                    lock (singletonLock)
                        if (_currentInstance == null || _currentInstance.NeedUpdate)
                        {
                            _currentInstance = new ErrorMapContext();
                            _currentInstance.UpdateLoadInfo();
                        }
                }
                return _currentInstance;

            }
        }
       


   

        public UserError InstanceMapError(string ErrorCode, string[] pars)
        {
            UserError rez = new UserError();
            rez.errorCode = ErrorCode;
            if (Mapping.ContainsKey(ErrorCode))
            {
                rez.errorMessage = String.Format(Mapping[ErrorCode].errorMessage, pars);
                rez.isUserError = Mapping[ErrorCode].isUserError;
            }

            return rez;
        }


        private UserError InstanceMapErrorCode(Exception ex)
        {
            UserError rez = null;
            var trace = new System.Diagnostics.StackTrace(ex);
            foreach (var frame in trace.GetFrames())
            {
                var method = frame.GetMethod();
                if (method.Name.Equals("MapExceptionLogStack"))
                    continue;
                if (method.ReflectedType != null)
                {
                    string ExClassName = method.ReflectedType.Name;
                    if (ErrorCodeMap.ContainsKey( ExClassName ))
                    {
                        var classDict = ErrorCodeMap[ExClassName];
                        if (classDict.ContainsKey(method.Name))
                        {
                            rez = classDict[method.Name];
                            
                        }
                    }

                  }
            }

            return rez;
        }



        #region bublicaly used methods

        public static UserError MapError(string ErrorCode, string[] pars)
        {
            return CurrentInstance.InstanceMapError(ErrorCode, pars);
    }


        public static UserError MapErrorCode(Exception ex)
        {
            return CurrentInstance.InstanceMapErrorCode(ex);
        }

       

        #endregion



}


}

