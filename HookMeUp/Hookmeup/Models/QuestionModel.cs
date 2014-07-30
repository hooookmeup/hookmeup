using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SS.DL.AzureTableStorage;

namespace Hookmeup.Models
{

    public enum ResultCode
    {
         Success = 0 , 
        
        // System error 101 - 200
        
        BadRequest = 101 , 
        GenericError = 102 , 
        ResourceNotFound = 103,

        //Validation error - 201 - 300

        InvalidUserName = 201 , 
        InvalidPassword = 202 , 
        InavlidUNamepass = 203 , 

    }

    public enum Category
    {
        Agreeableness,
        Consientiousness,
        Extraversion,
        Neuroticism
    }

    public enum Country
    {
        us,
        br,
        ch
    }

    public class QuestionModel
    {
        public string Name;
        public string Category;
        public  QuestionType InputType;
        public QuestionOptions[] Options;
        public bool IsAnswerReq;

    }

    public class AnswerModel
    {
        public string UserId { get; set; }
        public string QuestionId { get; set; }
        public int Answer { get; set; }
    }
}