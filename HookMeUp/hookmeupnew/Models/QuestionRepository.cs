using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SS.DL.AzureTableStorage;

namespace Hookmeup.Models
{
   

    public class QuestionsRepository
    {
        private QuestionContext qctx = new QuestionContext();

        public List<Question> GetAll()
        {

            List<Question> questions = new List<Question>();

            foreach (Category cat in Enum.GetValues(typeof(Category)))
            {
                var rez  = GetAll(cat, 4, Country.us)  ; 
                questions.AddRange(rez);
            }

            return questions;

        }

        public List<Question> GetAll(Category category , int count , Country country)
        {
            string catId = "Agreeableness_us";

            if (string.IsNullOrEmpty(catId)) throw new ArgumentNullException();

            switch (country)
            {
                case Country.us:
                    catId = category + "_" + country.ToString();
                    break;
                default:
                    break;
                      
            }

            var  ques = qctx.GetAllForPartition(catId);
            var posQues = ques.Where(q => q.IsPositive == true).Take(count/2);
            var posNeg = ques.Where(q => q.IsPositive == false).Take(count / 2);
            
            List<Question> questions = new List<Question>();
            questions.AddRange(posQues);
            questions.AddRange(posNeg);

            return questions;
        }

        public void Update(string catId , Question updatedQuestion)
        {
            throw new NotImplementedException();
            
        }

        public Question Get(string catId , string questId)
        {
            return qctx.GetById(catId, questId);
        }

        public void Post(Question question)
        {
            throw new NotImplementedException();
        }

        public Question Delete(string catId, string questId)
        {
            throw new NotImplementedException();
        }

        internal List<Question> GetQuestions(List<UserQuestion> questionIds)
        {
            List<Question> questions = new List<Question>();
            foreach (var q in questionIds)
            {
                Question ques = qctx.GetById(q.CategoryId, q.QuestionId);
                ques.Answer = q.Answer; 
                questions.Add(ques);
            }

            return questions;
        }


        internal List<UserQuestion> AssignQuestionsIds(string userid )
        {
            UserQuestionContext uqc = new UserQuestionContext();
            var quassign = uqc.GetAllForPartition(userid);
            if (quassign.Count == 0 )
            {
                var rez = GetAll();
                foreach (var item in rez)
                {
                    UserQuestion uq = new UserQuestion();
                    uq.UserId = userid;
                    uq.QuestionId = item.QuestionId;
                    uq.CategoryId = item.CategoryId;
                    uq.IsActive = true;
                    uqc.AddObject(uq);    
                }
                
            }

            quassign = uqc.GetAllForPartition(userid);
            return quassign.ToList<UserQuestion>();


        }

        internal void RemoveQuestions(string userid)
        {
            UserQuestionContext uqc = new UserQuestionContext();
            var quassign = uqc.GetAllForPartition(userid);
            if (quassign != null)
            {
                foreach (var item in quassign)
                {
                    uqc.DeleteObject(item,false);
                }
            }
        }

        internal void UpdateAnswer(string id, string questionId, int answer)
        {
            UserQuestionContext uqc = new UserQuestionContext();
            UserQuestion uq =  uqc.GetById(id, questionId);

            // should be int

            uq.Answer = answer;
            uqc.UpdateObject(uq, false);

            // Change to calculate score for each category 

            
        }

      
    }
}