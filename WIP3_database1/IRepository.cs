using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WIP3_database1.Models;

namespace WIP3_database1.Repository
{
    public interface IRepository<T> where T : BaseEntity
    {
        IEnumerable<T> FindAll();
    }
}
