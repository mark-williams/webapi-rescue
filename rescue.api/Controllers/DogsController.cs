using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using recue.data;
using rescue.domain;

namespace rescue.api.Controllers
{
    public class DogsController : ApiController
    {
        private readonly IRepository _repo;

        public DogsController(IRepository repository)
        {
            _repo = repository;
        }

        // GET: api/Dogs
        public IEnumerable<Animal> Get()
        {
            var animals = _repo.GetAnimals().Where(a => a.AnimalType == AnimalType.Dog);
            return animals;
        }

        // GET: api/Dogs/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Dogs
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Dogs/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Dogs/5
        public void Delete(int id)
        {
        }
    }
}
