using System;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace postgresqlcheck
{
    class Program
    {
        class DBJsonStructure
        {
            public int database;
            public List<string> tables = new List<String>();
            public List<string> functions = new List<String>();
        }
        static void Main(string[] args)
        {
            string ConnectionString = File.ReadAllText(@"c:\appsettings.json");
            JObject DBconstring = JObject.Parse(ConnectionString);
            string cs = (string)DBconstring["ConnectionStrings"]["database0"];

            using var con1 = new NpgsqlConnection(cs);
            con1.Open();

            string sql1 = "SELECT table_name FROM information_schema.tables  where table_schema='public' ORDER BY table_name;";

            using var cmd1 = new NpgsqlCommand(sql1, con1);

            using NpgsqlDataReader rdr1 = cmd1.ExecuteReader();
            IList<String> DBtables = new List<String>();
            while (rdr1.Read())
            {
                String DBTableName = (string)rdr1.GetString(0);
                DBtables.Add(DBTableName);
                //Console.WriteLine("{0}", rdr1.GetString(0));
            }
            rdr1.Close();
            string sql2 = "SELECT routines.routine_name FROM information_schema.routines LEFT JOIN information_schema.parameters ON routines.specific_name = parameters.specific_name WHERE routines.specific_schema = 'public' ORDER BY routines.routine_name, parameters.ordinal_position;";
            using var cmd2 = new NpgsqlCommand(sql2, con1);
            using NpgsqlDataReader rdr2 = cmd2.ExecuteReader();
            IList<String> DBfunctions = new List<String>();
            while (rdr2.Read())
            {
                String DBFunctionName = (string)rdr2.GetString(0);
                DBfunctions.Add(DBFunctionName);
                //Console.WriteLine("{0}",rdr2.GetString(0));
            }
            rdr2.Close();
            Console.WriteLine("The list of present database tables:");
            foreach (String ThisTable in DBtables)
            {
                Console.WriteLine(ThisTable);
            }
            Console.WriteLine("The list of present database functions:");
            foreach (String ThisFunction in DBfunctions)
            {
                Console.WriteLine(ThisFunction);
            }
            string JsonString = File.ReadAllText(@"c:\database_structure1.json");
            JObject Dstructure = JObject.Parse(JsonString);
            int DType = (int)Dstructure.GetValue("database");
            Console.WriteLine("The database type (primary or secondary):");
            Console.WriteLine(DType);
            IList<JToken> Tables = Dstructure["tables"].Children().ToList();
            IList<JToken> Functions = Dstructure["functions"].Children().ToList();
            IList<String> TablesList = new List<String>();
            DBJsonStructure jsonreply = new DBJsonStructure();
            jsonreply.database = DType;
            foreach (JToken Result in Tables)
            {
                String TableName = (string)Result; 
                TablesList.Add(TableName);
            }
            Console.WriteLine("The list of expected database tables:");
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
            Console.WriteLine("The list of expected database functions:");
            foreach (String ThisFunction in FunctionList)
            {
                Console.WriteLine(ThisFunction);
            }
            int TablesCount = 0;
            int FunctionsCount = 0;
            var TablesIntersect = DBtables.Intersect(TablesList);
            Console.WriteLine("The list of tables that are both present in json structure file and DB:");
            foreach (String s in TablesIntersect)
            {
                Console.WriteLine(s);
            }
            var TablesMissing = TablesList.Except(TablesIntersect);
            TablesCount = TablesMissing.Count();
            if (TablesCount == 0)
                Console.WriteLine("All tables are present in DB");
            else
            {
                Console.WriteLine("The following tables are missing in DB:");
                foreach (String s in TablesMissing)
                {
                    //Console.WriteLine(s);
                    jsonreply.tables.Add(s);
                }
                foreach (String s in jsonreply.tables)
                {
                    Console.WriteLine(s);
                }
            }
            var FunctionsIntersect = DBfunctions.Intersect(FunctionList);
            Console.WriteLine("The list of functions that are both present in json structure file and DB:");
            foreach (String s in FunctionsIntersect)
            {
                Console.WriteLine(s);
            }
            var FuntionsMissing = FunctionList.Except(FunctionsIntersect);
            FunctionsCount = FuntionsMissing.Count();
            if(FunctionsCount == 0)
                Console.WriteLine("All functions are present in DB");
            else
            {
                Console.WriteLine("The following functions are missing in DB:");
                foreach (String s in FuntionsMissing)
                {
                    //Console.WriteLine(s);
                    jsonreply.functions.Add(s);
                }
                foreach (String s in jsonreply.functions)
                {
                    Console.WriteLine(s);
                }
            }
            if (FunctionsCount > 0 || TablesCount > 0)
            {
                string output = JsonConvert.SerializeObject(jsonreply);
                Console.WriteLine(output);
                using (StreamWriter file = File.CreateText(@"c:\Users\Иван\Desktop\jsonreply.json"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, jsonreply);
                }
            }
            else
                Console.WriteLine("The DB structure is correct");
        }
    }
}
