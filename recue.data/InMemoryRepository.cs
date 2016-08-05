using System;
using System.Collections.Generic;
using System.Linq;
using rescue.domain;

namespace recue.data
{
    public class InMemoryRepository : IRepository
    {
        private readonly List<Animal> _animals;

        public InMemoryRepository()
        {
            _animals = new List<Animal>
            {
                new Animal { Id = 1000, AnimalType = AnimalType.Dog, Name = "Bert", DateOfBirth = new DateTime(2008, 1, 1), Acquired = new DateTime(2012, 3, 6), AvailableForRehoming = true, Sex = Sex.Male },
                new Animal { Id = 1001, AnimalType = AnimalType.Dog, Name = "Chaz", DateOfBirth = new DateTime(2009, 1, 1), Acquired = new DateTime(2012, 7, 2), AvailableForRehoming = true, Sex = Sex.Female },
                new Animal { Id = 1002, AnimalType = AnimalType.Dog, Name = "Wilf", DateOfBirth = new DateTime(2013, 6, 10), Acquired = new DateTime(2014, 7, 6), AvailableForRehoming = true, Sex = Sex.Male },
                new Animal { Id = 1003, AnimalType = AnimalType.Cat, Name = "Suzie", DateOfBirth = new DateTime(2009, 9, 24), Acquired = new DateTime(2016, 2, 18), AvailableForRehoming = true, Sex = Sex.Female }
            };
        }

        public IEnumerable<Animal> GetAnimals()
        {
            return _animals;
        }

        public IEnumerable<Animal> GetAnimalsByType(AnimalType type)
        {
            return _animals.Where(a => a.AnimalType == type);
        }

        public Animal GetAnimal(int id)
        {
            return _animals.FirstOrDefault(a => a.Id == id);
        }

        public Animal CreateAnimal(Animal animal)
        {
            animal.Id = GetNextId();
            _animals.Add(animal);
            return animal;
        }

        public Animal SaveAnimal(Animal animal)
        {
            var toUpdate = FindAnimalById(animal.Id);

            toUpdate.AvailableForRehoming = animal.AvailableForRehoming;
            toUpdate.Acquired = animal.Acquired;
            toUpdate.DateOfBirth = animal.DateOfBirth;
            toUpdate.Name = animal.Name;
            toUpdate.Notes = animal.Notes;
            toUpdate.Sex = animal.Sex;

            return toUpdate;
        }

      
        public void DeleteAnimal(Animal animal)
        {
            var toDelete = FindAnimalById(animal.Id);

            _animals.Remove(toDelete);
        }

        private Animal FindAnimalById(int id)
        {
            var updated = _animals.FirstOrDefault(a => a.Id == id);
            if (updated == null)
            {
                throw new Exception($"No animal found for Id: {id}");
            }
            return updated;
        }

        private int GetNextId()
        {
            return _animals.Max(a => a.Id) + 1;
        }

    }
}
