using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MySql.Data.MySqlClient;
using ucubot.Model;
using Dapper;



namespace ucubot.Database
{
    public class LessonSignalRepository : ILessonSignalRepository
    {
        
        
        
        public IEnumerable<LessonSignalDto> GetSignals(MySqlConnection connection)
        {
            return connection.Query<LessonSignalDto>("SELECT lesson_signal.Id Id, lesson_signal.Timestamp Timestamp, lesson_signal.SignalType Type, student.user_id UserId FROM lesson_signal LEFT JOIN student ON (lesson_signal.student_id = student.id);").ToList();
        }

        public LessonSignalDto GetSignal(MySqlConnection connection, long id)
        {
            var queryResult = connection.Query<LessonSignalDto>("SELECT lesson_signal.Id Id, lesson_signal.Timestamp Timestamp, lesson_signal.SignalType Type, student.user_id UserId  FROM lesson_signal LEFT JOIN student ON (lesson_signal.student_id = student.id) WHERE lesson_signal.Id=" + id + ";").ToList();
	        if (queryResult.Count > 0)
	        {
		        return queryResult[0];
	        }
	        return null;
        }

        public int CreateSignal(MySqlConnection connection, SlackMessage message)
        {	var userId = message.user_id;
	        var signalType = message.text.ConvertSlackMessageToSignalType();
	        var parameter = connection.Query<LessonSignalDto>("SELECT * FROM student WHERE user_id = @userId;", new {UserId = userId}).ToList();	
				
	        if(parameter.Count == 0){
		        return 100; 
	        }
	        var id = parameter[0].Id;
	        var parameter2 = connection.Query<LessonSignalDto>("SELECT * FROM lesson_signal WHERE student_id = @Id;", new {Id = id}).ToList();
	        if((parameter2.Count != 0 && !(parameter2.Count < 0))){
		        return 409; 
	        }

				
	        var command = new MySqlCommand("INSERT INTO lesson_signal (student_id, signal_type, time_stamp) VALUES (@id, @signalType, @timeStamp);", connection);
	        command.Parameters.AddWithValue("id", id);
	        command.Parameters.AddWithValue("signalType", signalType);
	        command.Parameters.AddWithValue("timeStamp", DateTime.Now);
	        return 200;
        }
    

        public int DeleteSignal(long id,string connectionString)
        {	var connection = new MySqlConnection(connectionString);
	        var command = new MySqlCommand("DELETE FROM lesson_signal WHERE id = @id;", connection);
	        var checkUserId = new MySqlCommand
	        (
		        "SELECT COUNT(*) FROM lesson_signal LEFT JOIN student ON (lesson_signal.student_id = student.id) WHERE lesson_signal.Id=@ID;",connection
	        );
            
	        checkUserId.Parameters.AddWithValue("@ID", id);
            
	        var Exist = (long) checkUserId.ExecuteScalar();

	        if(Exist < 1)
	        {
		        
		        return 400;
	        }
	        command.ExecuteNonQuery();

	        return 200;
        }
    }
}

