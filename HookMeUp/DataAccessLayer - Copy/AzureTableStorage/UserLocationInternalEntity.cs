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
using Microsoft.WindowsAzure.StorageClient;
using System.Globalization;
using System.IO;

namespace SS.DL.AzureTableStorage
{
    public class  UserLocations : TableServiceEntity
    {
        
		public  UserLocations()
        {

        }
		
	 	
        
        
        /// <summary>
        /// Gets or sets the UserId Property value.
        /// </summary>
        public  string   	UserId                    {get; set;}
        
        /// <summary>
        /// Gets or sets the LocationTime Property value.
        /// </summary>
        public  DateTime 	LocationTime              {get; set;}
        
	
	


    }    

 }

