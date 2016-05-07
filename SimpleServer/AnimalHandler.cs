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
			string response = "";

			string query = @"
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
              response += "<div class=\"animal animal-id-" + reader[0] + "\">";
              response += "<h2>" + reader[1] + "</h2>";
              response += "<div>" + reader[5] + "</div>";
              response += "<div>Lives in the " + reader[2] + " (" + reader[3] + " type) habitat</div>";
              response += "</div>";
            }
            Console.WriteLine(response);
          }

          return response;
        }
      }
		}

    public string getAllAnimals()
    {
      string response = "";

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
            Console.WriteLine(response);
          }
          return response;
        }
      }
    }

    public string getHabitat(string IdHabitat)
    {
      string response = "";

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
            response += "</ul>";
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

      response += "<h3>Employees</h3><ul>";

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
      string response = "";

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
              response += "<div><a href=\"/habitats/" + reader[0] + "\">" + reader[1] + "</a> (Animals: ";
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

