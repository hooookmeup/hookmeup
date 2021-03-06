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

namespace SS.DL.AzureTableStorage
{
  
  public partial class QuestionaireContext : EntityContext
    {

     

    #region Constructors
	
	/// <summary>
    /// Initializes a new instance of the <see cref="QuestionaireContext"/> class with default configuration.
    /// </summary>
    
	public QuestionaireContext()
          : base("Questionaires", _defaultConfiguration)
      {

      }
	
	/// <summary>
    /// Initializes a new instance of the <see cref="QuestionaireContext"/> class for different from default configuration.
    /// </summary>
    
    public QuestionaireContext(string p_ConfigurationName)
          : base("Questionaires", p_ConfigurationName)
        {
        }

    #endregion
	
	
	#region CRUD
	
			/// <summary>
		    /// Insert  for Questionaire object 
			/// if excepion happeneds during Table operation it must be cought in calling method.
			/// </summary>
			/// <param name="obj">instance of Questionaire class to be inserted </param>
	
        public void AddObject(Questionaire obj )
        {
            servCtx.AddObject(TableName, obj.InternalEntity);
            servCtx.SaveChangesWithRetries();
        }
		

			/// <summary>
		    /// Update  for Questionaire object
			/// if excepion happeneds during Table operation it must be cought in calling method.
			/// </summary>
			/// <param name="obj">instance of Questionaire class to be inserted </param>
			/// <param name="AddToContext">bool paremeter  tell the Context to attach object if it is not tracked by this instance of the Context </param>
			
        public void UpdateObject(Questionaire obj, bool AddToContext)
        {
		     if (AddToContext)
                  servCtx.AttachTo(TableName, obj.InternalEntity, "*");
           	 servCtx.UpdateObject(obj.InternalEntity);
             servCtx.SaveChangesWithRetries();
        }

			/// <summary>
		    /// Delete  for Questionaire object
			/// if excepion happeneds during Table operation it must be cought in calling method.
			/// </summary>
			/// <param name="obj">instance of Questionaire class to be inserted </param>
			/// <param name="AddToContext">bool paremeter  tell the Context to attach object if it is not tracked by this instance of the Context </param>

		public void DeleteObject(Questionaire obj, bool AddToContext)
        {
            if (AddToContext)
				 servCtx.AttachTo(TableName, obj.InternalEntity, "*");
            servCtx.DeleteObject(obj.InternalEntity);
            servCtx.SaveChangesWithRetries();
        }
		
	
	#endregion

	#region Standard queries
			
			/// <summary>
		    /// Load instance of  Questionaire object from Context.
			/// </summary>
			/// <param name="p_Partition">Partition key of the table</param>
			/// <param name="p_Row">Row key of desired object </param>
			
	
        public Questionaire GetById(string p_Partition, string p_Row)
        {
            Questionaires rez = null;
			if (!servCtx.TryGetEntity<Questionaires>(IdentityURI(p_Partition, p_Row), out rez))
			{
            	var Qtran = servCtx.CreateQuery<Questionaires>(TableName).Where(c => c.PartitionKey == p_Partition && c.RowKey == p_Row);
            	var enumer = Qtran.GetEnumerator();
            	if (enumer.MoveNext())
            	    rez = enumer.Current;
			}	
			  if (rez != null)
            	return new Questionaire(rez);
            else
                return null;
            
        }
		
        /// <summary>
        /// Load List of all Questionaire objects from Context for Partition Key.
        /// </summary>
        /// <param name="p_Partition">Partition key of the table</param>
        /// <returns>List of all Questionaire </returns>
	
       public List<Questionaire> GetAllForPartition( string PartitionKey )
        {
            List<Questionaire> rez = new List<Questionaire>();
            NextRecordInfo nextRec = new NextRecordInfo() ;
            	do	
				{
                	DataServiceQuery<Questionaires> Qtran = (DataServiceQuery<Questionaires>)servCtx.CreateQuery<Questionaires>(TableName).Where(c => c.PartitionKey == PartitionKey);
                   	nextRec = GetNextPage(ref rez, Qtran, nextRec, 0);
            	}
				while (!string.IsNullOrEmpty(nextRec.NextPartition));        
            return rez;
        }


       /// <summary>
       /// Load  next page of Questionaire objects from Context for the passed in query and
       /// adds this page to passed in lis.
       /// </summary>
       /// <param name="list">List results will be added to.</param>
       /// <param name="Query">DataServiceQuery object</param>
       /// <param name="nextRec"> NextRecordInfo object define begining of the page to load</param>
       /// <param name="PageSize">Size of the page.  If value is 0  or value greater than 1000 system will limit paze size to 1000 records  </param>
       /// <returns> NextRecordInfo object for the next page</returns>

		private NextRecordInfo GetNextPage(ref List<Questionaire> list, DataServiceQuery<Questionaires> Query, NextRecordInfo nextRec, int PageSize)
        {
           NextRecordInfo Result = new NextRecordInfo();
           QueryOperationResponse qor;
           if (!string.IsNullOrEmpty(nextRec.NextPartition))
               Query = Query.AddQueryOption("NextPartitionKey", nextRec.NextPartition).AddQueryOption("NextRowKey", nextRec.NextKey);
           if (PageSize > 0)
               qor = (QueryOperationResponse)((DataServiceQuery<Questionaires>)Query.Take(PageSize)).Execute();
           else
               qor = (QueryOperationResponse)(Query).Execute();
           string nextPartition = String.Empty;
           string nextRow = String.Empty;
           qor.Headers.TryGetValue("x-ms-continuation-NextPartitionKey", out nextPartition);
           qor.Headers.TryGetValue("x-ms-continuation-NextRowKey", out nextRow);
           IQueryable<Questionaires> qt = (IQueryable<Questionaires>)qor.AsQueryable();
           Result.NextKey = nextRow;
           Result.NextPartition = nextPartition;
		   	   var enumer = qt.GetEnumerator();
           while (enumer.MoveNext())
            list.Add( new Questionaire(enumer.Current));
           return Result;
		   
       }
	
	#endregion


    }
}

