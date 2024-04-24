using Microsoft.Data.SqlClient;

namespace WebApplication1.AnimalAPIStructure;

public class AnimalService : IAnimalService
{
    private readonly IConfiguration _configuration;
    public AnimalService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public List<Animal> GetAnimals(string orderBy)
    {
        string sql = "SELECT * FROM ANIMAL";
        var output = new List<Animal>();

        if (string.IsNullOrEmpty(orderBy))
        {
            orderBy = "name";
        }

        using (var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]))
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
                            CATEGORY = reader["Category"].ToString(),
                            AREA = reader["Area"].ToString()

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
        using (var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]))
        {
            using (var sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = connection;
                connection.Open();

                
                sqlCommand.CommandText = "SELECT COUNT(*) FROM ANIMAL WHERE IdAnimal = @IDANIMAL";
                sqlCommand.Parameters.AddWithValue("@IDANIMAL", animal.IdAnimal);
                int existingCount = (int)sqlCommand.ExecuteScalar();

                if (existingCount > 0)
                { 
                    return -1; 
                }

                sqlCommand.CommandText = "INSERT INTO ANIMAL VALUES(@IDANIMAL, @NAME, @DESCRIPTION, @CATEGORY, @AREA)";
                sqlCommand.Parameters.AddWithValue("IDANIMAL", animal.IdAnimal);
                sqlCommand.Parameters.AddWithValue("NAME", animal.Name);
                sqlCommand.Parameters.AddWithValue("DESCRIPTION", animal.Description);
                sqlCommand.Parameters.AddWithValue("CATEGORY", animal.CATEGORY);
                sqlCommand.Parameters.AddWithValue("AREA", animal.AREA);
                sqlCommand.ExecuteNonQuery();

                connection.Close();
                return 1;
            }
        }
    }

    public int UpdateAnimal(Animal animal, int idAnimal)
    {
        using (var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]))
        {
            using (var sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = connection;
                connection.Open();

                sqlCommand.CommandText = "SELECT COUNT(*) FROM ANIMAL WHERE IdAnimal = @IDANIMAL";
                sqlCommand.Parameters.AddWithValue("@IDANIMAL", idAnimal);
                int existingCount = (int)sqlCommand.ExecuteScalar();

                if (existingCount == 0)
                {
                    return -1; 
                }

                sqlCommand.CommandText = "UPDATE ANIMAL SET NAME = @NAME, DESCRIPTION = @DESCRIPTION, CATEGORY = @CATEGORY, AREA = @AREA WHERE IDANIMAL = @IDANIMAL";
                sqlCommand.Parameters.AddWithValue("NAME", animal.Name);
                sqlCommand.Parameters.AddWithValue("DESCRIPTION", animal.Description);
                sqlCommand.Parameters.AddWithValue("CATEGORY", animal.CATEGORY);
                sqlCommand.Parameters.AddWithValue("AREA", animal.AREA);
                sqlCommand.Parameters.AddWithValue("IDANIMAL", idAnimal);
                int result = sqlCommand.ExecuteNonQuery();

                connection.Close();
                return result;
            }
        }
    }


    public int DeleteAnimal(int idAnimal)
    {
        using (var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]))
        {
            using (var sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = connection;
                connection.Open();

                sqlCommand.CommandText = "SELECT COUNT(*) FROM ANIMAL WHERE IdAnimal = @IDANIMAL";
                sqlCommand.Parameters.AddWithValue("@IDANIMAL", idAnimal);
                int existingCount = (int)sqlCommand.ExecuteScalar();

                if (existingCount == 0)
                {
                    return -1; 
                }

                sqlCommand.CommandText = "DELETE FROM ANIMAL WHERE IDANIMAL = @IDANIMAL";
                sqlCommand.Parameters.AddWithValue("IDANIMAL", idAnimal);
                int result = sqlCommand.ExecuteNonQuery();

                connection.Close();
                return result;
            }
        }
    }
}