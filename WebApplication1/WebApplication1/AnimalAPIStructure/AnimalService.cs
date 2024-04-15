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
            using (var comm = new SqlCommand())
            {
                comm.Connection = connection;

                if (orderBy == "name" || orderBy == "description" || orderBy == "area" || orderBy == "category")
                {

                    comm.CommandText = $"{sql} ORDER BY {orderBy} ASC";

                    connection.Open();

                    var reader = comm.ExecuteReader();

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
        throw new NotImplementedException();
    }

    public int UpdateAnimal(Animal animal, int idAnimal)
    {
        throw new NotImplementedException();
    }

    public int DeleteAnimal(int idAnimal)
    {
        throw new NotImplementedException();
    }
}