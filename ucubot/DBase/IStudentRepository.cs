using System.Collections.Generic;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using ucubot.Model;

namespace ucubot.Database
{
    public interface IStudentRepository
    {
        IEnumerable<Student> Students(MySqlConnection connection);

        Student GetStudent(MySqlConnection connection, int id);

        int Create(MySqlConnection connection, Student entity);
        
        int Update(MySqlConnection connection, Student entity);

        int Delete(MySqlConnection connection, long id);
    
    }
}