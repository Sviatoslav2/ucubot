using System.Collections.Generic;
using MySql.Data.MySqlClient;
using ucubot.Model;

namespace ucubot.Database
{
    public interface ISignalRepository
    {
        IEnumerable<StudentSignal> GetSignals(MySqlConnection connection);
    }
}