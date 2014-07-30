using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using SS.DL.AzureTableStorage;
using Hookmeup.Models;
using System.Web.Http.Cors;

namespace Hookmeup.Controllers.Apis
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class QuestionsController : ApiController
    {

        static QuestionsRepository questionRepository = new QuestionsRepository();

             // GET api/question
        public IEnumerable<Question> Get()
        {
            return questionRepository.GetAll();
        }

        // GET api/question/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/question
        public void Post([FromBody]string value)
        {
        }

        // PUT api/question/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/question/5
        public void Delete(int id)
        {
        }
    }
}
