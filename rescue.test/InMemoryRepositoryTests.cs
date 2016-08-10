using System;
using System.Linq;
using NUnit.Framework;
using recue.data;
using rescue.domain;

namespace rescue.test
{
    public class InMemoryRepositoryTests
    {
        private const string TestName = "TEST-NAME";
        private IRepository _repository;

        [SetUp]
        public void Setup()
        {
            _repository = new InMemoryRepository();
            CreateAnimalInRepository();
        }

        [Test]
        public void GivenARepositoryWhenICallGetAnimalsThenAnimalsAreReturned()
        {
            var animals = _repository.GetAnimals();

            Assert.That(animals, Is.Not.Null, "Null returned");
        }

        [Test]
        public void GivenARepositoryWhenICallGetAnimalTheCorrectAnimalIsReturned()
        {
            var newAnimal = GetNewAnimal(AnimalType.Dog, "NEW ANIMAL");
            _repository.CreateAnimal(newAnimal);

            var animal = _repository.GetAnimal(newAnimal.Id);

            Assert.That(animal, Is.Not.Null, "Null returned");
            Assert.That(animal.Id, Is.EqualTo(newAnimal.Id), "Incorrect animal Id");
        }


        [Test]
        public void GivenARepositoryWhenICreateAnAnimalTheResultHasAValidId()
        {
            var animal = new Animal()
            {
                Acquired = DateTime.Now,
                AnimalType = AnimalType.Cat,
                DateOfBirth = DateTime.Now.AddYears(-3),
                Notes = string.Empty,
                Sex = Sex.Female,
                Name = "Tabby"
            };

            var created = _repository.CreateAnimal(animal);

            var animals = _repository.GetAnimals().ToList();
            Assert.That(animals.Any(a => a.Id == created.Id), Is.True, "Animal not created");
            Assert.That(animals.Max(a => a.Id), Is.EqualTo(created.Id), "Created animal has incorrect Id");
        }

        [Test]
        public void GivenARepositoryWithNoAnimalsWhenICreateAnAnimalTheResultHasAValidId()
        {
            DeleteAllAnimalsInRepository();
            var animal = CreateAnimalInRepository();

            var animals = _repository.GetAnimals().ToList();
            Assert.That(animals.Any(a => a.Id == animal.Id), Is.True, "Animal not created");
            Assert.That(animal.Id, Is.EqualTo(1), "Created animal has incorrect Id");
        }

        
        [Test]
        public void GivenARepositoryWhenIUpdateAnAnimalTheReturnedValueHasTheUpdatedValues()
        {
            const string testNotes = "TEST-NOTES";
            var testAcquired = DateTime.MinValue;
            var testDob = DateTime.Now.AddYears(-3);
            
            var toUpdate = _repository.GetAnimals().FirstOrDefault();
            
            toUpdate.Acquired = testAcquired;
            toUpdate.AvailableForRehoming = false;
            toUpdate.DateOfBirth = testDob;
            toUpdate.Name = TestName;
            toUpdate.Notes = testNotes;

            var result = _repository.SaveAnimal(toUpdate);

            Assert.That(result.Name, Is.EqualTo(TestName), "Name not updated");
            Assert.That(result.Notes, Is.EqualTo(testNotes), "Notes not updated");
            Assert.That(result.Acquired, Is.EqualTo(testAcquired), "Acquired not updated");
            Assert.That(result.DateOfBirth, Is.EqualTo(testDob), "DateOfBirth not updated");
            Assert.That(result.AvailableForRehoming, Is.False, "AvailableForRehoming not updated");
            Assert.That(result.Id, Is.EqualTo(toUpdate.Id), "Id was changed");
        }

        [Test]
        public void GivenARepositoryWhenIAttempToUpdateAnAnimalThatDoesntExistANotFoundExceptionIsThrown()
        {
            var animal = GetNewAnimal(AnimalType.Dog, TestName);
            animal.Id = 1000;
           
            Assert.Throws<NotFoundException>(() => _repository.SaveAnimal(animal), "Did not throw exception");

        }

        [Test]
        public void GivenARepositoryWhenIDeleteAnAnimalItCanNoLongerBeRetrieved()
        {
            CreateAnimalInRepository();
            var toDelete = _repository.GetAnimals().FirstOrDefault();
            if (toDelete == null)
            {
                Assert.Fail("Cannot find an animal to delete");
            }

            _repository.DeleteAnimal(toDelete.Id);

            var animals = _repository.GetAnimals();

            Assert.That(animals.Any(a => a.Id == toDelete.Id), Is.False, "Animal was not deleted");

        }

        private Animal CreateAnimalInRepository()
        {
            var animal = GetNewAnimal(AnimalType.Dog, TestName);
            return _repository.CreateAnimal(animal);
        }

        private static Animal GetNewAnimal(AnimalType animalType, string name)
        {
            var animal = new Animal()
            {
                Acquired = DateTime.Now,
                AnimalType = AnimalType.Cat,
                DateOfBirth = DateTime.Now.AddYears(-3),
                Notes = string.Empty,
                Sex = Sex.Female,
                Name = "Tabby"
            };
            return animal;
        }

        private void DeleteAllAnimalsInRepository()
        {
            var animals = _repository.GetAnimals().ToList();
            animals.ForEach(a => _repository.DeleteAnimal(a.Id));
        }
    }
}
