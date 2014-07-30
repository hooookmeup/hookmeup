﻿//------------------------------------------------------------------------------
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
using System.Runtime.Serialization.Json;
using SS.Framework.Common;

namespace SS.DL.AzureTableStorage
{
    public class  UserDevice 
	
    {
    
		 UserDevices _UserDevices;
		
		public  UserDevice( UserDevices Par)
        {
			_UserDevices = Par;
        }
		public  UserDevice( )
        {
			_UserDevices = new UserDevices ();
        }
		
		public string RowKey  
		{
			get 
			{
				return _UserDevices.RowKey;
			}
		}
        /// <summary>
        /// Gets or sets the UserId Property value.
        /// </summary>
        public  string   	UserId                    { get { return _UserDevices.PartitionKey; } set { _UserDevices.PartitionKey = value; } }
        
        /// <summary>
        /// Gets or sets the DeviceId Property value.
        /// </summary>
        public  string   	DeviceId                  { get { return _UserDevices.RowKey; } set { _UserDevices.RowKey = value; } }
        
        /// <summary>
        /// Gets or sets the DeviceName Property value.
        /// </summary>
        public  string   	DeviceName                { get { return _UserDevices.DeviceName; } set { _UserDevices.DeviceName = value; } }
        
        /// <summary>
        /// Gets or sets the DeviceVersion Property value.
        /// </summary>
        public  string   	DeviceVersion             { get { return _UserDevices.DeviceVersion; } set { _UserDevices.DeviceVersion = value; } }
        
        /// <summary>
        /// Gets or sets the DevicePlatform Property value.
        /// </summary>
        public  string   	DevicePlatform            { get { return _UserDevices.DevicePlatform; } set { _UserDevices.DevicePlatform = value; } }
        
        /// <summary>
        /// Gets or sets the DevicePhGapVersion Property value.
        /// </summary>
        public  string   	DevicePhGapVersion        { get { return _UserDevices.DevicePhGapVersion; } set { _UserDevices.DevicePhGapVersion = value; } }
        

		
		public UserDevices InternalEntity 
		{ get
		  {
			return _UserDevices;
		  }
		}

    }    

 }