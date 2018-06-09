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
using ucubot.DBase;

namespace ucubot.Controllers
{
	[Route("api/[controller]")]
	public class StudentEndpointController : Controller
	{
		private readonly IConfiguration _configuration;
		private StudentRepository studentRepository;
		

		public StudentEndpointController(IConfiguration configuration)
		{
			_configuration = configuration;
			studentRepository = new StudentRepository(configuration);
		}

		[HttpGet]
		public IEnumerable<Student> ShowStudents()
		{
			var connectionString = _configuration.GetConnectionString("BotDatabase");// Jason
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
				return studentRepository.Students(connection);
			}
		}
		
		[HttpGet("{id}")]
		public Student ShowStudent(long id)
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
                
                                
                                
			}
			return studentRepository.GetStudent(connection, (int)id);
		}
		
		[HttpPost]
		public async Task<IActionResult> CreateStudent(Student stud)
		{	var connectionString = _configuration.GetConnectionString("BotDatabase");
			var connection = new MySqlConnection(connectionString);
			var Key = studentRepository.Create(connection, stud);
			if (Key ==  409)
			{
				return StatusCode(409);
			}

			return Accepted();
			 
		}


		[HttpPut]
		public async Task<IActionResult> UpdateStudent(Student stud){
			var connectionString = _configuration.GetConnectionString("BotDatabase");
			var connection = new MySqlConnection(connectionString);
			var id = stud.Id;
			var userId = stud.UserId;
			var firstName = stud.FirstName;
			var lastName = stud.LastName;
			using (connection)
			{
				connection.Open();
				try{
					var command =
						new MySqlCommand(
							"UPDATE student SET user_id = @userId, first_name = @firstName, last_name = @lastName WHERE id = @id;",
							connection);
					command.Parameters.AddWithValue("userId", userId);
					command.Parameters.AddWithValue("firstName", firstName);
					command.Parameters.AddWithValue("lastName", lastName);
					command.Parameters.AddWithValue("id", id);
					await command.ExecuteNonQueryAsync();
				}
				catch{
					return StatusCode(409);
				}

				connection.Close();
			}
			return Accepted();
		}
		
		
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteStudent(long id)
		{
			var connectionString = _configuration.GetConnectionString("BotDatabase");
			var connection = new MySqlConnection(connectionString);
			var key = studentRepository.Delete(connection, id);
			if (key == 409)
			{
				return StatusCode(409);
			}
			
				return Accepted();
			
		}
	}
} 