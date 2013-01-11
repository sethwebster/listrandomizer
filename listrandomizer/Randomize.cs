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
            Random.Org.Random r = new Random.Org.Random();
#if DEBUG || TEST
            r.UseLocalMode = false;
#endif
            string[] value = model.list.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            List<List<string>> lists = new List<List<string>>();

            int listContainerSize = (int)Math.Round((double)value.Length / model.numLists);
            if (listContainerSize == 0)
                throw new InvalidOperationException("You cannot have more lists than items");

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
            int listIndex = 0;
            for (int i = 0; i < value.Length; i++)
            {
                lists[listIndex].Add(value[i]);
                if (i > 0 && i % listContainerSize == 0)
                    listIndex++;
                if (listIndex == lists.Count)
                {
                    listIndex -= 1;
                }

            }
        }

        private static void ShuffleList(Random.Org.Random r, string[] value)
        {
            var resequence = r.RandomizeSequence(0, value.Length-1);

            bool changeMade = false;

            int i = 0;
            do
            {
                var currVal = value[i];
                value[i] = value[resequence[i]];
                value[resequence[i]] = currVal;
                i++;

            } while (i < value.Length);
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