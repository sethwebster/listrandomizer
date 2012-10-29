using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace listrandomizer
{
    public class Randomize : ApiController
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

        // POST api/<controller>
        public IEnumerable<string[]> Post(int numLists, string list)
        {
            Random.Random r = new Random.Random();
#if DEBUG || TEST
            r.UseLocalMode = true;
#endif
            string[] value = list.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            List<string[]> lists = new List<string[]>();
            int listContainerSize = (int)Math.Round((double)value.Length / numLists);
            for (int i = 0; i < numLists; i++)
            {
                lists.Add(new string[listContainerSize]);
            }
            Queue<string> todo = new Queue<string>(value);
            while (todo.Count > 0)
            {
                var item = todo.Dequeue();
                int listIndex = 0;
                int position = 0;
                do
                {
                    listIndex = r.Next(0, numLists);
                    position = r.Next(0, listContainerSize);
                }
                while (!string.IsNullOrEmpty(lists[listIndex][position]));
                lists[listIndex][position] = item;
            }
            return lists;
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