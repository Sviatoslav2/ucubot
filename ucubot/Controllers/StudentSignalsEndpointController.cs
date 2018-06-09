using System;
using ucubot.Model;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using ucubot.Database;
using ucubot.DBase;


namespace ucubot.Controllers
{
    [Route("api/[controller]")]

    public class StudentSignalsEndpointController : Controller
    {
        private readonly IConfiguration _configuration;

        private readonly ISignalRepository _repository;

        public StudentSignalsEndpointController(IConfiguration configuration, ISignalRepository repository)
        {
            _configuration = configuration;
            _repository = repository;
        }

        [HttpGet]
        public IEnumerable<StudentSignal> ShowSignals()
        {   
            var connectionString = _configuration.GetConnectionString("BotDatabase");
            var Connection = new MySqlConnection(connectionString);
            var Res = _repository.GetSignals(Connection);
            using (Connection)
            {
                try
                {
                    Connection.Open();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

                return Res;

            }

        }
    }
}