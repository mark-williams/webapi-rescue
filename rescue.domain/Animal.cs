using System;

namespace rescue.domain
{
    

    public class Animal
    {
        public int Id { get; set; }
        public AnimalType AnimalType { get; set; }
        public string Name { get; set; }
        public Sex Sex { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Notes { get; set; }
        public DateTime Acquired { get; set; }
        public bool AvailableForRehoming { get; set; }
    }
}
