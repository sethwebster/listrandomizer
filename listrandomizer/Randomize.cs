using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace listrandomizer
{
    public class RandomizeController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }
        public class RandomizeModel
        {
            public int numLists { get; set; }
            public string list { get; set; }
        }
        // POST api/<controller>
        public HttpResponseMessage Post(RandomizeModel model)
        {
            Random.Random r = new Random.Random();
#if DEBUG || TEST
            r.UseLocalMode = true;
#endif
            string[] value = model.list.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            List<List<string>> lists = new List<List<string>>();
            int listContainerSize = (int)Math.Round((double)value.Length / model.numLists);
            if (listContainerSize == 0)
                throw new HttpResponseException(HttpStatusCode.InternalServerError);

            for (int i = 0; i < model.numLists; i++)
            {
                lists.Add(new List<string>());
            }

            ShuffleList(r, value);
            SplitList(value, lists, listContainerSize);

            return ControllerContext.Request.CreateResponse<IEnumerable<IEnumerable<string>>>(HttpStatusCode.OK, lists);
        }

        private static void SplitList(string[] value, List<List<string>> lists, int listContainerSize)
        {
            int listIndex = -1;
            for (int i = 0; i < value.Length; i++)
            {
                if (i % listContainerSize == 0)
                    listIndex++;
                lists[listIndex].Add(value[i]);
            }
        }

        private static void ShuffleList(Random.Random r, string[] value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                int newPos = r.Next(0, value.Length);
                string currVal = value[newPos];
                value[newPos] = value[i];
                value[i] = currVal;
            }

        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}