using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Data;
using Npgsql;
using WIP3_database1.Models;

namespace WIP3_database1.Repository
{
    public class StructureRepository : IRepository<DbStructure>
    {
        private string connectionString;
        public StructureRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetValue<string>("DBInfo:ConnectionString");
        }

        internal IDbConnection Connection
        {
            get
            {
                return new NpgsqlConnection(connectionString);
            }
        }

        public IEnumerable<DbStructure> FindAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<DbStructure>("SELECT row_number() OVER () as rnum, table_name FROM information_schema.tables  where table_schema='public' ORDER BY table_name;");
            }
        }
    }
}
