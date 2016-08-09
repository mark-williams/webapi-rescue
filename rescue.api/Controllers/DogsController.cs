using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
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
        public IHttpActionResult Get()
        {
            var animals = _repo.GetAnimals().Where(a => a.AnimalType == AnimalType.Dog);
            return Ok(animals);
        }

        // GET: api/Dogs/5
        public IHttpActionResult Get(int id)
        {
            var animal = _repo.GetAnimal(id);
            if (animal == null)
            {
                return NotFound();
            }

            return Ok(animal);
        }

        // POST: api/Dogs
        public IHttpActionResult Post([FromBody]Animal dog)
        {
            var result = _repo.CreateAnimal(dog);
            string uri = Url.Link("DefaultApi", new {id = result.Id});

            return Created(new Uri(uri), result);
        }

        // PUT: api/Dogs/5
        public IHttpActionResult Put(int id, [FromBody]Animal dog)
        {
            dog.Id = id;
            var result = _repo.SaveAnimal(dog);

            return Ok(result);
        }

        // DELETE: api/Dogs/5
        public IHttpActionResult Delete(int id)
        {
            _repo.DeleteAnimal(id);
            return Ok();
        }
    }
}
