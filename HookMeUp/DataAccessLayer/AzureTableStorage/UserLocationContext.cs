//------------------------------------------------------------------------------
// <autogenerated>
//    This code was generated by a CodeSmith Template.
//
//    This is for customizing generated code
//    All changes made in this file will be preserved.
// </autogenerated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;
using System.Globalization;
using System.IO;
using System.Data.Services.Client;
using SS.Framework.Common;

namespace SS.DL.AzureTableStorage
{
    public partial class  UserLocationContext 
    {
        readonly double PROXIMITY_RADIUS = 10.0; 

    /* 
	
	//   This is an example of custom developed query
	public List<UserLocation> MethodName(list of method parameters )
        {
            List<UserLocation> rez = new List<UserLocation>();
            NextRecordInfo nextRec = new NextRecordInfo() ;
            	do	
				{
                	DataServiceQuery<UserLocations> Qtran = (DataServiceQuery<UserLocations>)servCtx.CreateQuery<UserLocations>(TableName).Where(c => enter query details here );
                   	nextRec = GetNextPage(ref rez, Qtran, nextRec, 0);
            	}
				while (!string.IsNullOrEmpty(nextRec.NextPartition));        
            return rez;
        }
	
	*/
	  
     //Add your extension methods here for  UserLocationContext class.

       

		public List<UserLocation> GetProximityUsers(double latitude , double longitude)
        {
            List<UserLocation> rez = new List<UserLocation>();
            RadiusBox radBox = RadiusBox.Create(latitude, longitude, PROXIMITY_RADIUS);
            

            NextRecordInfo nextRec = new NextRecordInfo() ;
            	do	
				{
                    //DataServiceQuery<UserLocations> Qtran = (DataServiceQuery<UserLocations>)servCtx.CreateQuery<UserLocations>(TableName).Where(c => c.PartitionKey <= topline 
                    //                                                                                            && double.Parse(c.PartitionKey) >= radBox.BottomLine
                    //                                                                                            &&  double.Parse(c.RowKey) <=radBox.RightLine 
                    //                                                                                            && double.Parse(c.RowKey) >=radBox.LeftLine);



                    DataServiceQuery<UserLocations> Qtran = (DataServiceQuery<UserLocations>)servCtx.CreateQuery<UserLocations>(TableName).Where(c => c.PartitionKey.CompareTo(radBox.TopLine.ToString()) < 0
                                                                                                              && c.PartitionKey.CompareTo(radBox.BottomLine.ToString()) > 0
                                                                                                              && c.RowKey.CompareTo(radBox.RightLine.ToString()) > 0
                                                                                                              && c.RowKey.CompareTo(radBox.LeftLine.ToString()) < 0);

                   	nextRec = GetNextPage(ref rez, Qtran, nextRec, 0);
            	}
				while (!string.IsNullOrEmpty(nextRec.NextPartition));        
            return rez;
        }

    }    

 }

