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

namespace SS.DL.AzureTableStorage
{
    public class  Users : TableServiceEntity
    {
        
		public  Users()
        {

        }
		
	 	
        
        
        /// <summary>
        /// Gets or sets the UserName Property value.
        /// </summary>
        public  string   	UserName                  {get; set;}
        
        /// <summary>
        /// Gets or sets the Password Property value.
        /// </summary>
        public  string   	Password                  {get; set;}
        
        /// <summary>
        /// Gets or sets the IsAdmin Property value.
        /// </summary>
        public  bool     	IsAdmin                   {get; set;}
        
        /// <summary>
        /// Gets or sets the Gender Property value.
        /// </summary>
        public  string   	Gender                    {get; set;}
        
        /// <summary>
        /// Gets or sets the LastLoginDate Property value.
        /// </summary>
        public  string   	LastLoginDate             {get; set;}
        
        /// <summary>
        /// Gets or sets the IsActive Property value.
        /// </summary>
        public  bool     	IsActive                  {get; set;}
        
        /// <summary>
        /// Gets or sets the Latitude Property value.
        /// </summary>
        public  double   	Latitude                  {get; set;}
        
        /// <summary>
        /// Gets or sets the Longitude Property value.
        /// </summary>
        public  double   	Longitude                 {get; set;}
        
        /// <summary>
        /// Gets or sets the ChatId Property value.
        /// </summary>
        public  string   	ChatId                    {get; set;}
        
        /// <summary>
        /// Gets or sets the CompatIndex Property value.
        /// </summary>
        public  int      	CompatIndex               {get; set;}
        
        /// <summary>
        /// Gets or sets the DeviceName Property value.
        /// </summary>
        public  string   	DeviceName                {get; set;}
        
        /// <summary>
        /// Gets or sets the DeviceVersion Property value.
        /// </summary>
        public  string   	DeviceVersion             {get; set;}
        
        /// <summary>
        /// Gets or sets the DevicePlatform Property value.
        /// </summary>
        public  string   	DevicePlatform            {get; set;}
        
        /// <summary>
        /// Gets or sets the DeviceId Property value.
        /// </summary>
        public  string   	DeviceId                  {get; set;}
        
        /// <summary>
        /// Gets or sets the DevicePhGapVersion Property value.
        /// </summary>
        public  string   	DevicePhGapVersion        {get; set;}
        
        /// <summary>
        /// Gets or sets the IsOnline Property value.
        /// </summary>
        public  bool     	IsOnline                  {get; set;}

        /// <summary>
        /// Gets or sets the AgeCheck Property value.
        /// </summary>
        public bool AgeCheck { get; set; }

        /// <summary>
        /// Gets or sets the TCCheck Property value.
        /// </summary>
        public bool TCCheck { get; set; }


        /// <summary>
        /// Gets or sets the FacebookId Property value.
        /// </summary>
        public  string   	FacebookId                {get; set;}
        
        /// <summary>
        /// Gets or sets the AppState Property value.
        /// </summary>
        public  int      	AppState                  {get; set;}
        
        /// <summary>
        /// Gets or sets the Score Property value.
        /// </summary>
        public  string   	Score                     {get; set;}

        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the FacebookId Property value.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the FacebookId Property value.
        /// </summary>
        public string Locale { get; set; }

	


    }    

 }
