using System;
using System.Data;
using System.Data.SqlClient;
//using Mono.Data.Sqlite;

namespace SimpleServer
{
  public class AnimalHandler
	{
		public AnimalHandler ()
		{
		}

		public string getAnimal(string IdAnimal) {
			string response = "<div class=\"content\">";

			string query = @"
				SELECT
          a.name,
          s.commonName,
          s.scientificName,
          h.name AS [Habitat],
          a.age
        FROM Animal a
        INNER JOIN Species s ON s.IdSpecies = a.IdSpecies
        INNER JOIN Habitat h ON h.IdHabitat = a.IdHabitat
        WHERE a.IdAnimal = 
				" + IdAnimal;

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
              //response += "<div class=\"animal animal-id-" + reader[0] + "\">";
              response += "<h2>" + reader[0] + "</h2>";
              response += "<p>Common Name: " + reader[1] + "</p>";
              response += "<p>Scientific Name: " + reader[2] + "</p>";
              response += "<p>Habitat: " + reader[3] + "</p>";
              response += "<p>Age: " + reader[4] + "</p>";

            }
            response += "</div>";
            Console.WriteLine(response);
          }

          return response;
        }
      }
		}

    public string getAllAnimals()
    {
      string response = "<div class=\"content\">";

        const string query = @"
      SELECT 
        a.IdAnimal,
        a.name, 
        h.name HabitatName,
        ht.name HabitatType,
        s.commonName,
        s.scientificName
      FROM Animal a
      INNER JOIN Species s ON a.IdSpecies = s.IdSpecies
      INNER JOIN Habitat h ON h.IdHabitat = a.IdHabitat
      INNER JOIN HabitatType ht on ht.IdHabitatType = h.IdHabitatType
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
                response += "<div class=\"animal animal-id-" + reader[0] + "\">";
                response += "<h2>" + reader[1] + "</h2>";
                response += "<div><a href='/animals/" + reader[0] + "'>" + reader[5] + "</a></div>";
                response += "<div>Lives in the " + reader[2] + " (" + reader[3] + " type) habitat</div>";
                response += "</div>";
            }
            response += "</div>";
            Console.WriteLine(response);
          }
          return response;
        }
      }
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

