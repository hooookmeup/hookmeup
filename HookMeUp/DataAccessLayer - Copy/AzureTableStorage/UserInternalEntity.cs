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
        /// Gets or sets the Score Property value.
        /// </summary>
        public  int      	Score                     {get; set;}
        
        /// <summary>
        /// Gets or sets the CompatIndex Property value.
        /// </summary>
        public  int      	CompatIndex               {get; set;}
        
	
	


    }    

 }
