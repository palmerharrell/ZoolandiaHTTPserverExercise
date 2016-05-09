using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace SimpleServer
{
  class EmployeeHandler
  {
    public EmployeeHandler()
    {
    }

    public string getAllEmployees()
    {
      string response = "<div class=\"content\">";

      const string query = @"
      SELECT
        e.name,
        e.IdEmployee
      FROM Employee e
      ";

      const string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"C:\\windows-workspace\\c-sharp-http-server\\SimpleServer\\Zoolandia.mdf\";Integrated Security=True";

      using (SqlConnection connection = new SqlConnection(connectionString))
      using (SqlCommand cmd = new SqlCommand(query, connection))
      {
        connection.Open();
        using (SqlDataReader reader = cmd.ExecuteReader())
        {
          if (reader.HasRows)
          {
            while (reader.Read()) // Read advances to the next row.
            {
              response += "<div><a href=\"/employees/" + reader[1] + "\">" + reader[0] + "</a></div>";
            }
            Console.WriteLine(response);
          }
          return response;
        }
      }
    }

    public string getEmployee(string IdEmployee)
    {
      string response = "<div class=\"content\">";

      // EMPLOYEE NAME AND AGE
      string query = @"
				SELECT
          e.name,
          e.age
        FROM Employee e
        WHERE e.IdEmployee = " + IdEmployee;

      const string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"C:\\windows-workspace\\c-sharp-http-server\\SimpleServer\\Zoolandia.mdf\";Integrated Security=True";

      using (SqlConnection connection = new SqlConnection(connectionString))
      using (SqlCommand cmd = new SqlCommand(query, connection))
      {
        connection.Open();
        using (SqlDataReader reader = cmd.ExecuteReader())
        {
          if (reader.HasRows)
          {
            while (reader.Read())
            {
              response += "<h2>" + reader[0] + "</h2>";
              response += "<div>Age: " + reader[1] + "</div>";
            }
          }
        }
      }

      query = @"
        SELECT
            h.name
          FROM Habitat h
          INNER JOIN HabitatEmployees he ON he.IdHabitat = h.IdHabitat
          INNER JOIN Employee e ON e.IdEmployee = he.IdEmployee
          WHERE e.IdEmployee = " + IdEmployee;

      response += "<h3>Habitat Assignments</h3><ul>";

      using (SqlConnection connection = new SqlConnection(connectionString))
      using (SqlCommand cmd = new SqlCommand(query, connection))
      {
        connection.Open();
        using (SqlDataReader reader = cmd.ExecuteReader())
        {
          if (reader.HasRows)
          {
            while (reader.Read())
            {
              response += "<li>" + reader[0] + "</li>";
            }
          }
        }
      }
      response += "</ul></div>";
      return response;
    }
  }
}
