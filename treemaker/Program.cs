using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Newtonsoft.Json.Linq;
namespace treemaker
{
    class Node
    {
        public int ID { get; set; }
        public int Root_ID { get; set; }
        public string Name { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            //List<Node> treenodes = new List<Node>();
            /* Node test = new Node { ID = 2, Root_ID = 1, Name = "TEST" };
             string json = JsonSerializer.Serialize<Node>(test);
             Console.WriteLine(json);
             List<Node> treenodes = new List<Node>();
             treenodes.Add(JsonSerializer.Deserialize<Node>(json));
             treenodes.Add(new Node { ID = 1, Root_ID = 0, Name = "TEST2" });*/
            /*using (StreamReader file = File.OpenText(@"c:\tree1.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                Node newnode = (Node)serializer.Deserialize(file, typeof(Node));
            }*/
            string jsonString = File.ReadAllText(@"c:\tree3.json");
            Console.WriteLine(jsonString);
            JObject treenodes = JObject.Parse(jsonString);
            // get JSON result objects into a list
            IList<JToken> results = treenodes["nodes"].Children().ToList();
            // serialize JSON results into .NET objects
            IList<Node> searchResults = new List<Node>();
            foreach (JToken result in results)
            {
                Node searchResult = result.ToObject<Node>();
                searchResults.Add(searchResult);
            }
            foreach (Node curnode in searchResults)
            {
                Console.WriteLine(curnode.ID);
                Console.WriteLine(curnode.Root_ID);
                Console.WriteLine(curnode.Name);
            }
            /*  string[] JsonStr = File.ReadAllLines(@"c:\tree1.json");
              int count = (JsonStr.Length - 2) / 5;
              Console.WriteLine(count);
              Console.WriteLine(JsonStr[2]);
              for (int i = 0; i < count; i++)
              {
                  string s1 = String.Concat("{", JsonStr[2+i*5]);
                  s1 = String.Concat(s1, JsonStr[3 + i * 5]);
                  s1 = String.Concat(s1, JsonStr[4 + i * 5]);
                  s1 = String.Concat(s1, "}");
                  Console.WriteLine(s1);
                  treenodes.Add(JsonSerializer.Deserialize<Node>(s1));
              }*/
           /* foreach (Node curnode in treenodes)
                {
                    Console.WriteLine(curnode.ID);
                    Console.WriteLine(curnode.Root_ID);
                    Console.WriteLine(curnode.Name);
                }*/
        }
    }
}
