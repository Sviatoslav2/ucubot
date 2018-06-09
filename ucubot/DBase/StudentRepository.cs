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
using ucubot.Database;
 

namespace ucubot.DBase
{
	[Route("api/[controller]")]
	public class StudentRepository : IStudentRepository
	{
		private string connectionString;

		public StudentRepository(IConfiguration configuration)
		{
			connectionString = configuration.GetConnectionString("BotDatabase");
		}


		public IEnumerable<Student> Students (MySqlConnection connection)
		{
			return  connection.Query<Student>("SELECT id Id, first_name FirstName, second_name LastName, user_id UserId FROM student;").ToList();
		}
		
		public Student GetStudent(MySqlConnection connection, int id)
		{
			using (connection)
			{
				var studs = connection.Query<Student>("SELECT id as Id, user_id as UserId, first_name as FirstName, last_name as LastName FROM student WHERE id = @id;", new {Id = id}).ToList();
				if(studs.Count == 0){
					return null; 
				}
				return studs[0];
			}
		}

		public int Create(MySqlConnection connection, Student entity)
		{
            
			var userId = entity.UserId;
			var firstName = entity.FirstName;
			var lastName = entity.LastName;
            
			var checkUserId = new MySqlCommand("SELECT COUNT(*) FROM student WHERE user_id=?userID;" , connection);
			checkUserId.Parameters.AddWithValue("?userID", userId);

			var userExist = (long) checkUserId.ExecuteScalar();
			if(userExist > 0)
			{
				return 409;
			}

			const string mysqlCmdString = "INSERT INTO student (first_name, second_name, user_id) VALUES (?param2, ?param3, ?param4);";
			var cmd = new MySqlCommand(mysqlCmdString, connection);

			cmd.Parameters.Add("?param2", MySqlDbType.VarChar).Value = firstName;
			cmd.Parameters.Add("?param3", MySqlDbType.VarChar).Value = lastName;
			cmd.Parameters.Add("?param4", MySqlDbType.VarChar).Value = userId;
			cmd.CommandType = CommandType.Text;

			cmd.ExecuteNonQuery();   
        
			return 200;
		}

		public int Update(MySqlConnection connection, Student student)
		{
			int RES = 200;
			var id = student.Id;
			var userId = student.UserId;
			var firstName = student.FirstName;
			var lastName = student.LastName;
            
			var checkUserId = new MySqlCommand("SELECT COUNT(*) FROM student WHERE user_id=(@userID);" , connection);
			checkUserId.Parameters.AddWithValue("@userID", userId);

			var userExist = (long) checkUserId.ExecuteScalar();


			if (!(userExist > 0))//
			{
				RES = 400;
			}
            
			
			const string mysqlCmdString = "UPDATE student SET id=@id, first_name=@first_name, second_name=@last_name, user_id=@user_id WHERE id=@that_id;";  
            
			var cmd = new MySqlCommand(mysqlCmdString, connection);
            
			cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
			cmd.Parameters.Add("@that_id", MySqlDbType.Int32).Value = id;
			cmd.Parameters.Add("@first_name", MySqlDbType.VarChar).Value = firstName;
			cmd.Parameters.Add("@last_name", MySqlDbType.VarChar).Value = lastName;
			cmd.Parameters.Add("@user_id", MySqlDbType.VarChar).Value = userId;
            
			cmd.CommandType = CommandType.Text;
            
			cmd.ExecuteNonQuery();   
			return RES;
		}

		public int Delete(MySqlConnection connection, long id)
		{
			int RES = 200;
			var checkUserId = new MySqlCommand("SELECT COUNT(*) FROM student WHERE id=?Id;" , connection);
			checkUserId.Parameters.AddWithValue("?Id", id);

			var userExist = (long) checkUserId.ExecuteScalar();
                
			var checkUserId2 = new MySqlCommand("SELECT COUNT(*) FROM student WHERE id=?Id;" , connection);
			checkUserId2.Parameters.AddWithValue("?Id", id);

			if(userExist > 0)
			{
				var myCommand = new MySqlCommand("DELETE FROM student WHERE id=" + id + ";", connection);
				try
				{
					myCommand.ExecuteNonQuery();
				}
				catch
				{
					RES = 409;
				}
			}
			else
			{
				RES = 409;
			}

			return RES;
		}
	}
}


