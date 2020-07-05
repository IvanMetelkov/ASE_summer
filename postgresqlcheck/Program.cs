using System;
using Npgsql;

namespace postgresqlcheck
{
    class Program
    {
        static void Main(string[] args)
        {
            string cs = "Host=localhost;Username=postgres;Password=root;Database=testdatabase";

            using var con1 = new NpgsqlConnection(cs);
            con1.Open();

            var sql1 = "SELECT table_name FROM information_schema.tables  where table_schema='public' ORDER BY table_name;";

            using var cmd1 = new NpgsqlCommand(sql1, con1);

            using NpgsqlDataReader rdr1 = cmd1.ExecuteReader();

            while (rdr1.Read())
            {
                Console.WriteLine("{0}", rdr1.GetString(0));
            }
            using var con2 = new NpgsqlConnection(cs);
            con2.Open();
            var sql2 = "SELECT routines.routine_name FROM information_schema.routines LEFT JOIN information_schema.parameters ON routines.specific_name = parameters.specific_name WHERE routines.specific_schema = 'public' ORDER BY routines.routine_name, parameters.ordinal_position;";
            using var cmd2 = new NpgsqlCommand(sql2, con2);
            using NpgsqlDataReader rdr2 = cmd2.ExecuteReader();
            while (rdr2.Read())
            {
                Console.WriteLine("{0}",rdr2.GetString(0));
            }
        }
    }
}
