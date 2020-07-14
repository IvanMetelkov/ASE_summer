using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WIP3_database1.Models;
using WIP3_database1.Repository;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Data;
using Npgsql;

namespace WIP3_database1.Controllers
{
    public class StructureController : Controller
    {
        private readonly StructureRepository structureRepository;

        public StructureController(IConfiguration configuration)
        {
            structureRepository = new StructureRepository(configuration);
        }


        public IActionResult Index()
        {
            return View(structureRepository.FindAll());
        }
    }
}
