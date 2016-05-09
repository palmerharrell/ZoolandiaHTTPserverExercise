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
  }
}

