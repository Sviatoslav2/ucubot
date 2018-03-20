using System;

using System.Collections.Generic;

using System.Data;

using System.Linq;

using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication;

using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.Configuration;

using MySql.Data.MySqlClient;

using ucubot.Model;



namespace ucubot.Controllers

{

    [Route("api/[controller]")]

    public class LessonSignalEndpointController : Controller

    {

        private readonly IConfiguration _configuration;



        public LessonSignalEndpointController(IConfiguration configuration)

        {

            _configuration = configuration;

        }



        [HttpGet]

        public IEnumerable<LessonSignalDto> ShowSignals()

        {

            var connectionString = _configuration.GetConnectionString("BotDatabase");

            // TODO: add query to get all signals

            var query = "SELECT * FROM lesson_signal";

            var conn = new MySqlConnection(connectionString); 

            var dataTable = new DataTable();

            var cmd = new MySqlCommand(query, conn);

            var da = new MySqlDataAdapter(cmd);

            try

            {

                conn.Open();

                da.Fill(dataTable);

                conn.Close();

            }

            catch

            {

                

            }

            //initialize list of objects

            

            var enumerable = new List<LessonSignalDto>();

            

            foreach(DataRow row in dataTable.Rows)

            {

                var obj = new LessonSignalDto

                {

                    Id = (int)row["id"],

                    UserId = (string)row["user_id"],

                    Type = (LessonSignalType)row["signal_type"],

                    Timestamp = (DateTime)row["time_stamp"]

                };

                // add object to list

                enumerable.Add(obj);

            }



            return enumerable;

            /**/





            // return list of objects

            //return new LessonSignalDto[0];

        }

        
        

        [HttpGet("{id}")]

        public LessonSignalDto ShowSignal(long id)

        {

            // TODO: add query to get a signal by the given id

            var connectionString = _configuration.GetConnectionString("BotDatabase");

             

            string query = "select * from lesson_signal where id = @id;";



            var conn = new MySqlConnection(connectionString);     

            conn.Open();

            var cmd = new MySqlCommand(query, conn);

            cmd.Parameters.Add("@id", id);

             

            var da = new MySqlDataAdapter(cmd);

            da.Fill(dataTable);

            LessonSignalDto sign;

            if (dataTable.Rows.Count > 0)

            {

                var row = dataTable.Rows[0];

                sign = new LessonSignalDto

                {

                    Id = (int)row["id"],

                    UserId = (string) row["user_id"],

                    Type = (LessonSignalType) row["signal_type"],

                    Timestamp = Convert.ToDateTime( row["timestamp"])

                       

                };

            }

            else

            {

                sign = null;

            }



            conn.Close();

            da.Dispose();

            return sign;

        }

        

        [HttpPost]

        public async Task<IActionResult> CreateSignal(SlackMessage message)

        {

            var userId = message.user_id;

            var signalType = message.text.ConvertSlackMessageToSignalType();



            // TODO: add insert command to store signal

            var connectionString = _configuration.GetConnectionString("BotDatabase");

             

            string query = "insert into lesson_signal(signal_type, user_id, timestamp) " +

                           "values(@signalType, @userId, @timestamp)  ;";



            var conn = new MySqlConnection(connectionString);        

            var cmd = new MySqlCommand(query, conn);

            cmd.Parameters.Add("@timestamp",DateTime.Now);

            cmd.Parameters.Add("@signalType", signalType);

            cmd.Parameters.Add("@userId", userId);

            conn.Open();

            cmd.ExecuteNonQuery();

            conn.Close();
            return Accepted();

        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> RemoveSignal(long id)

        {
            //TODO: add delete command to remove signal

            var connectionString = _configuration.GetConnectionString("BotDatabase");

            var query = "DELETE FROM lesson_signal WHERE id=@id";

            var conn = new MySqlConnection(connectionString);

            var cmd = new MySqlCommand(query, conn);

            cmd.Parameters.AddWithValue("id", id);

            try

            {

                conn.Open();

                cmd.ExecuteNonQuery();

                conn.Close();

            }

            catch

            {
                

            }

            return Accepted();

        }

    }

}