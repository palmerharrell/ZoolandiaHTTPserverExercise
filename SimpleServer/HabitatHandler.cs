using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace SimpleServer
{
  public class HabitatHandler
  {
    public HabitatHandler()
    {
    }

    public string getHabitat(string IdHabitat)
    {
      string response = "<div class=\"content\">";

      // HABITAT NAME AND TYPE
      string query = @"
				SELECT
          h.name,
          ht.name
        FROM Habitat h
        INNER JOIN HabitatType ht ON ht.IdHabitatType = h.IdHabitatType
				WHERE h.IdHabitat = 
				" + IdHabitat;

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
              response += "<div>";
              response += "<h2>" + reader[0] + "</h2>";
              response += "<div>(" + reader[1] + ")</div>";
              response += "</div>";
            }
          }
        }
      }

      // ANIMALS IN HABITAT
      query = @"
        SELECT
          a.name,
          s.commonName
        FROM Animal a
        INNER JOIN Habitat h ON h.IdHabitat = a.IdHabitat
        INNER JOIN Species s ON s.IdSpecies = a.IdSpecies
        WHERE h.IdHabitat = " + IdHabitat;

      response += "<div><h3>Animals</h3><ul>";

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
              response += "<li>" + reader[0] + " (" + reader[1] + ")</li>";
            }
            response += "</ul></div>";
          }
        }
      }

      // HABITAT EMPLOYEES
      query = @"
        SELECT
          e.name
        FROM Employee e
        INNER JOIN HabitatEmployees he ON he.IdEmployee = e.IdEmployee
        INNER JOIN Habitat h ON h.IdHabitat = he.IdHabitat
        WHERE he.IdHabitat = " + IdHabitat;

      response += "<div><h3>Employees</h3><ul>";

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
            response += "</ul></div>";
          }
        }
      }

      return response;
    }

    public string getAllHabitats()
    {
      string response = "<div class=\"content\">";

      const string query = @"
      SELECT
        h.IdHabitat, 
        h.name,
        COUNT (a.IdHabitat) AS [NumberOfAnimals]
      FROM Habitat h
      INNER JOIN Animal a ON a.IdHabitat = h.IdHabitat
      GROUP BY h.IdHabitat, h.name
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
              response += "<div>";
              response += "<a href=\"/habitats/" + reader[0] + "\">" + reader[1] + "</a> (Animals: ";
              response += reader[2] + ")</div>";
            }
            Console.WriteLine(response);
          }
          return response;
        }
      }
    }
  }
}
