using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using ucubot.Model;
using Dapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ucubot.Database;

namespace ucubot.Controllers
{
	[Route("api/[controller]")]
	public class LessonSignalEndpointController : Controller
	{
		private readonly IConfiguration _configuration;
		private LessonSignalRepository _repository;
		
		
		public LessonSignalEndpointController(IConfiguration configuration)
		{
			_configuration = configuration;
			_repository = new LessonSignalRepository();
			
		}

		[HttpGet]
		public IEnumerable<LessonSignalDto> ShowSignals()
		{
			var connectionString = _configuration.GetConnectionString("BotDatabase");
			var connection = new MySqlConnection(connectionString);
			using (connection){
				return connection.Query<LessonSignalDto>("SELECT lesson_signal.id as Id, time_stamp as Timestamp, signal_type as Type, student.user_id as UserId FROM lesson_signal INNER JOIN student ON student_id = student.id;").ToList();
			}
		}
		
		[HttpGet("{id}")]
		public LessonSignalDto ShowSignal(long id)
		{
			var connectionString = _configuration.GetConnectionString("BotDatabase");
			var connection = new MySqlConnection(connectionString);
			using (connection)
			{
				try{
					return connection.Query<LessonSignalDto>(
						"SELECT lesson_signal.id as Id, time_stamp as Timestamp, signal_type as Type, student.user_id as UserId FROM lesson_signal INNER JOIN student ON student_id = student.id WHERE lesson_signal.id = @id;",
						new {Id = id}).ToList()[0];
				}
				catch{
					return null;
				}
			}
		}
		
		[HttpPost]
		public async Task<IActionResult> CreateSignal(SlackMessage message)
		{
			
			var connectionString = _configuration.GetConnectionString("BotDatabase");
			var connection = new MySqlConnection(connectionString);
			using (connection)
			{
				connection.Open();
				var Key = _repository.CreateSignal(connection, message);
				if (Key == 409)
				{
					return StatusCode(409);
				}
				if (Key == 100)
				{
					return BadRequest();
				}
				connection.Close();
			}
			return Accepted();
		}
		
		[HttpDelete("{id}")]
		public async Task<IActionResult> RemoveSignal(long id)
		{
			var connectionString = _configuration.GetConnectionString("BotDatabase");
			var connection = new MySqlConnection(connectionString);
			using (connection)
			{
				try
				{

					connection.Open();
				}
				catch (Exception e)
				{
					Console.WriteLine(e.ToString());
  
				}
				var result = _repository.DeleteSignal(id,connectionString);

				switch (result)
				{
					case 400:
						return BadRequest();
					case 200:
						return Accepted();
                        
					default: StatusCode(result);
						break;
				}



				
				connection.Close();
			}
			return null;
		}
	}
} 