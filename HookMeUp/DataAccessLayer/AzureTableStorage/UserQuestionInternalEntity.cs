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
    public class  UserQuestions : TableServiceEntity
    {
        
		public  UserQuestions()
        {

        }
		
	 	
        
        
        /// <summary>
        /// Gets or sets the CategoryId Property value.
        /// </summary>
        public  string   	CategoryId                {get; set;}
        
        /// <summary>
        /// Gets or sets the Answer Property value.
        /// </summary>
        public  int      	Answer                    {get; set;}
        
        /// <summary>
        /// Gets or sets the IsActive Property value.
        /// </summary>
        public  bool     	IsActive                  {get; set;}
        
	
	


    }    

 }
