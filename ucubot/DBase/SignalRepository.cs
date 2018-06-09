using System.Collections.Generic;
using System.Data;
using System.Linq;
using MySql.Data.MySqlClient;
using ucubot.Model;
using Dapper;



namespace ucubot.Database
{
    public class SignalRepository : ISignalRepository
    {
        public IEnumerable<StudentSignal> GetSignals(MySqlConnection connection)
        {
            return connection.Query<StudentSignal>("SELECT student_signals.first_name first_name, student_signals.second_name LastName, student_signals.signal_type SignalType, student_signals.count Count FROM student_signals").ToList();            
        }
    }
}