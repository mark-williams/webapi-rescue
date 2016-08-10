using System;
using System.Collections.Generic;
using System.Linq;
using rescue.domain;

namespace recue.data
{
    public class InMemoryRepository : IRepository
    {
        private static readonly List<Animal> Animals = new List<Animal>();

        public IEnumerable<Animal> GetAnimals()
        {
            return Animals;
        }

        public IEnumerable<Animal> GetAnimalsByType(AnimalType type)
        {
            return Animals.Where(a => a.AnimalType == type);
        }

        public Animal GetAnimal(int id)
        {
            return Animals.FirstOrDefault(a => a.Id == id);
        }

        public Animal CreateAnimal(Animal animal)
        {
            animal.Id = GetNextId();
            Animals.Add(animal);
            return animal;
        }

        public Animal SaveAnimal(Animal animal)
        {
            var toUpdate = FindAnimalById(animal.Id);

            // NB We don't allow amendment of the animal type as cats and dogs are identified by different apis, so it
            // doesn'y make sense to use, say, /api/dogs to change a dog into a cat!
            toUpdate.AvailableForRehoming = animal.AvailableForRehoming;
            toUpdate.Acquired = animal.Acquired;
            toUpdate.DateOfBirth = animal.DateOfBirth;
            toUpdate.Name = animal.Name;
            toUpdate.Notes = animal.Notes;
            toUpdate.Sex = animal.Sex;

            return toUpdate;
        }

      
        public void DeleteAnimal(int id)
        {
            var toDelete = FindAnimalById(id);

            Animals.Remove(toDelete);
        }

        private Animal FindAnimalById(int id)
        {
            var updated = Animals.FirstOrDefault(a => a.Id == id);
            if (updated == null)
            {
                throw new NotFoundException($"No animal found for Id: {id}");
            }
            return updated;
        }

        private int GetNextId()
        {
            if (Animals.Count == 0)
            {
                return 1;
            }

            return Animals.Max(a => a.Id) + 1;
        }

    }
}
