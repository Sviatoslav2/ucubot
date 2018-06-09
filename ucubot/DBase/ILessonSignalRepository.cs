using System.Collections.Generic;
using MySql.Data.MySqlClient;
using ucubot.Model;

namespace ucubot.Database
{
    public interface ILessonSignalRepository
    {
        IEnumerable<LessonSignalDto> GetSignals(MySqlConnection connection);

        LessonSignalDto GetSignal(MySqlConnection connection, long id);

        int CreateSignal(MySqlConnection connection, SlackMessage message);

        int DeleteSignal(long id,string connectionString);

    }
}