using System.Collections.Generic;

namespace rescue.domain
{
    public interface IRepository
    {
        IEnumerable<Animal> GetAnimals();
        IEnumerable<Animal> GetAnimalsByType(AnimalType type);
        Animal GetAnimal(int id);
        Animal CreateAnimal(Animal animal);
        Animal SaveAnimal(Animal animal);
        void DeleteAnimal(Animal animal);
    }
}
