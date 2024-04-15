using Microsoft.Data.SqlClient;

namespace WebApplication1.AnimalAPIStructure;

public class AnimalService : IAnimalService
{
    private readonly string _sqlConnection = "Data Source=db-mssql16.pjwstk.edu.pl;Initial Catalog=2019SBD;Integrated Security=True";
    public List<Animal> GetAnimals(string orderBy)
    {
        string sql = "SELECT * FROM ANIMAL";
        var output = new List<Animal>();

        if (string.IsNullOrEmpty(orderBy))
        {
            orderBy = "name";
        }

        using (var connection = new SqlConnection(_sqlConnection))
        {
            using (var sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = connection;

                if (orderBy == "name" || orderBy == "description" || orderBy == "area" || orderBy == "category")
                {

                    sqlCommand.CommandText = $"{sql} ORDER BY {orderBy} ASC";

                    connection.Open();

                    var reader = sqlCommand.ExecuteReader();

                    if (!reader.HasRows)
                    {
                        Console.WriteLine("Empty rows");
                    }

                    while (reader.Read())
                    {
                        output.Add( new Animal
                        {
                            IdAnimal = int.Parse( reader["IdAnimal"].ToString() ),
                            Name = reader["Name"].ToString(),
                            Description = reader["Description"].ToString(),
                            Category = reader["Category"].ToString(),
                            Area = reader["Area"].ToString()

                        } );
                    }
                }


                connection.Close();
            }
        }

        return output;
    }

    public int AddAnimal(Animal animal)
    {
        if (animal.IdAnimal != 0 || !string.IsNullOrEmpty(animal.Name) || !string.IsNullOrEmpty(animal.Description) || !string.IsNullOrEmpty(animal.Category) || !string.IsNullOrEmpty(animal.Area))
        {
            using (var connection = new SqlConnection(_sqlConnection))
            {
                using (var sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = connection;
                    connection.Open();

                    sqlCommand.CommandText = "INSERT INTO ANIMAL VALUES(@NAME, @DESCRIPTION, @CATEGORY, @AREA)";
                    sqlCommand.Parameters.AddWithValue( "NAME", animal.Name);
                    sqlCommand.Parameters.AddWithValue( "DESCRIPTION", animal.Description);
                    sqlCommand.Parameters.AddWithValue( "CATEGORY", animal.Category);
                    sqlCommand.Parameters.AddWithValue( "AREA", animal.Area);
                    sqlCommand.ExecuteNonQuery();

                    connection.Close();
                    return 1;
                }
            }
        }
        return 0;
    }

    public int UpdateAnimal(Animal animal, int idAnimal)
    {
        using (var connection = new SqlConnection(_sqlConnection))
        {
            using (var sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = connection;
                connection.Open();

                sqlCommand.CommandText = "UPDATE ANIMAL SET NAME = @NAME, DESCRIPTION = @DESCRIPTION, CATEGORY = @CATEGORY, AREA = @AREA WHERE IDANIMAL = @IDANIMAL";
                sqlCommand.Parameters.AddWithValue( "NAME", animal.Name);
                sqlCommand.Parameters.AddWithValue( "DESCRIPTION", animal.Description);
                sqlCommand.Parameters.AddWithValue( "CATEGORY", animal.Category);
                sqlCommand.Parameters.AddWithValue( "AREA", animal.Area);
                sqlCommand.Parameters.AddWithValue( "IDANIMAL", idAnimal);
                int result = sqlCommand.ExecuteNonQuery();

                connection.Close();
                return result;
            }

        }
    }

    public int DeleteAnimal(int idAnimal)
    {
        using (var connection = new SqlConnection(_sqlConnection))
        {
            using (var sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = connection;
                connection.Open();
                
                sqlCommand.CommandText = "DELETE FROM ANIMAL WHERE IDANIMAL = @IDANIMAL";
                sqlCommand.Parameters.AddWithValue( "IDANIMAL", idAnimal);

                int result = sqlCommand.ExecuteNonQuery();

                connection.Close();

                return result;
            }
        }
    }
}