namespace WebApplication1.AnimalAPIStructure;

public interface IAnimalService
{
    public List<Animal> GetAnimals(string orderBy);

    public int AddAnimal (Animal animal);

    public int UpdateAnimal (Animal animal, int idAnimal);

    public int DeleteAnimal ( int idAnimal );

}