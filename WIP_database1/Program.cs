using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Newtonsoft.Json.Linq;
namespace WIP_database1
{
    class Program
    {
        static void Main(string[] args)
        {
            string JsonString = File.ReadAllText(@"c:\database_structure1.json");
            JObject Dstructure = JObject.Parse(JsonString);
            int DType = (int)Dstructure.GetValue("database");
            Console.WriteLine("The database type (primary or secondary):");
            Console.WriteLine(DType);
            IList<JToken> Tables = Dstructure["tables"].Children().ToList();
            IList<JToken> Functions = Dstructure["functions"].Children().ToList();
            IList<String> TablesList = new List<String>();
            foreach (JToken Result in Tables)
            {
                String TableName = (string)Result;
                TablesList.Add(TableName);
            }
            Console.WriteLine("The list of database tables:");
            foreach (String ThisTable in TablesList)
            {
                Console.WriteLine(ThisTable);
            }
            IList<String> FunctionList = new List<String>();
            foreach (JToken Result in Functions)
            {
                String FunctionName = (string)Result;
                FunctionList.Add(FunctionName);
            }
            Console.WriteLine("The list of database functions:");
            foreach (String ThisFunction in FunctionList)
            {
                Console.WriteLine(ThisFunction);
            }
        }
    }
}