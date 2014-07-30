//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by a CodeSmith Template.
//
//     DO NOT MODIFY contents of this file. Changes to this
//     file will be lost if the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using System.Data.Services.Client;
using SS.Framework.Common; 

namespace SS.DL.AzureTableStorage
{
  
  public partial class UserQuestionContext : EntityContext
    {

     

    #region Constructors
	
	/// <summary>
    /// Initializes a new instance of the <see cref="UserQuestionContext"/> class with default configuration.
    /// </summary>
    
	public UserQuestionContext()
          : base("UserQuestions", _defaultConfiguration)
      {

      }
	
	/// <summary>
    /// Initializes a new instance of the <see cref="UserQuestionContext"/> class for different from default configuration.
    /// </summary>
    
    public UserQuestionContext(string p_ConfigurationName)
          : base("UserQuestions", p_ConfigurationName)
        {
        }

    #endregion
	
	
	#region CRUD
	
			/// <summary>
		    /// Insert  for UserQuestion object 
			/// if excepion happeneds during Table operation it must be cought in calling method.
			/// </summary>
			/// <param name="obj">instance of UserQuestion class to be inserted </param>
	
        public void AddObject(UserQuestion obj )
        {
            servCtx.AddObject(TableName, obj.InternalEntity);
            servCtx.SaveChangesWithRetries();
        }
		

			/// <summary>
		    /// Update  for UserQuestion object
			/// if excepion happeneds during Table operation it must be cought in calling method.
			/// </summary>
			/// <param name="obj">instance of UserQuestion class to be inserted </param>
			/// <param name="AddToContext">bool paremeter  tell the Context to attach object if it is not tracked by this instance of the Context </param>
			
        public void UpdateObject(UserQuestion obj, bool AddToContext)
        {
		     if (AddToContext)
                  servCtx.AttachTo(TableName, obj.InternalEntity, "*");
           	 servCtx.UpdateObject(obj.InternalEntity);
             servCtx.SaveChangesWithRetries();
        }

			/// <summary>
		    /// Delete  for UserQuestion object
			/// if excepion happeneds during Table operation it must be cought in calling method.
			/// </summary>
			/// <param name="obj">instance of UserQuestion class to be inserted </param>
			/// <param name="AddToContext">bool paremeter  tell the Context to attach object if it is not tracked by this instance of the Context </param>

		public void DeleteObject(UserQuestion obj, bool AddToContext)
        {
            if (AddToContext)
				 servCtx.AttachTo(TableName, obj.InternalEntity, "*");
            servCtx.DeleteObject(obj.InternalEntity);
            servCtx.SaveChangesWithRetries();
        }
		
	
	#endregion

	#region Standard queries
			
			/// <summary>
		    /// Load instance of  UserQuestion object from Context.
			/// </summary>
			/// <param name="p_Partition">Partition key of the table</param>
			/// <param name="p_Row">Row key of desired object </param>
			
	
        public UserQuestion GetById(string p_Partition, string p_Row)
        {
            UserQuestions rez = null;
			if (!servCtx.TryGetEntity<UserQuestions>(IdentityURI(p_Partition, p_Row), out rez))
			{
            	var Qtran = servCtx.CreateQuery<UserQuestions>(TableName).Where(c => c.PartitionKey == p_Partition && c.RowKey == p_Row);
            	  try
		                {
		                    var enumer = Qtran.GetEnumerator();
		                    if (enumer.MoveNext())
		                        rez = enumer.Current;
		                }
		                catch (Exception e)
		                {
		                    Logger.LogError(e.InnerException.Message);
                		}
                	}
			  if (rez != null)
            	return new UserQuestion(rez);
            else
                return null;
            
        }
		
        /// <summary>
        /// Load List of all UserQuestion objects from Context for Partition Key.
        /// </summary>
        /// <param name="p_Partition">Partition key of the table</param>
        /// <returns>List of all UserQuestion </returns>
	
       public List<UserQuestion> GetAllForPartition( string PartitionKey )
        {
            List<UserQuestion> rez = new List<UserQuestion>();
            NextRecordInfo nextRec = new NextRecordInfo() ;
            	do	
				{
                	DataServiceQuery<UserQuestions> Qtran = (DataServiceQuery<UserQuestions>)servCtx.CreateQuery<UserQuestions>(TableName).Where(c => c.PartitionKey == PartitionKey);
                   	nextRec = GetNextPage(ref rez, Qtran, nextRec, 0);
            	}
				while (!string.IsNullOrEmpty(nextRec.NextPartition));        
            return rez;
        }


       /// <summary>
       /// Load  next page of UserQuestion objects from Context for the passed in query and
       /// adds this page to passed in lis.
       /// </summary>
       /// <param name="list">List results will be added to.</param>
       /// <param name="Query">DataServiceQuery object</param>
       /// <param name="nextRec"> NextRecordInfo object define begining of the page to load</param>
       /// <param name="PageSize">Size of the page.  If value is 0  or value greater than 1000 system will limit paze size to 1000 records  </param>
       /// <returns> NextRecordInfo object for the next page</returns>

		private NextRecordInfo GetNextPage(ref List<UserQuestion> list, DataServiceQuery<UserQuestions> Query, NextRecordInfo nextRec, int PageSize)
        {
           NextRecordInfo Result = new NextRecordInfo();
           QueryOperationResponse qor;
           if (!string.IsNullOrEmpty(nextRec.NextPartition))
               Query = Query.AddQueryOption("NextPartitionKey", nextRec.NextPartition).AddQueryOption("NextRowKey", nextRec.NextKey);
           if (PageSize > 0)
               qor = (QueryOperationResponse)((DataServiceQuery<UserQuestions>)Query.Take(PageSize)).Execute();
           else
               qor = (QueryOperationResponse)(Query).Execute();
           string nextPartition = String.Empty;
           string nextRow = String.Empty;
           qor.Headers.TryGetValue("x-ms-continuation-NextPartitionKey", out nextPartition);
           qor.Headers.TryGetValue("x-ms-continuation-NextRowKey", out nextRow);
           IQueryable<UserQuestions> qt = (IQueryable<UserQuestions>)qor.AsQueryable();
           Result.NextKey = nextRow;
           Result.NextPartition = nextPartition;
		   	   var enumer = qt.GetEnumerator();
           while (enumer.MoveNext())
            list.Add( new UserQuestion(enumer.Current));
           return Result;
		   
       }
	
	#endregion


    }
}

