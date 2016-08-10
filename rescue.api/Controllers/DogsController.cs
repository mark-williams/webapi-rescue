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

        [Route("api/dogs", Name="DefaultApi")]
        public IHttpActionResult Get()
        {
            var animals = _repo.GetAnimals().Where(a => a.AnimalType == AnimalType.Dog);
            return Ok(animals);
        }

        [Route("api/dogs")]
        public IHttpActionResult Get(string name)
        {
            var animals = _repo.GetAnimals().Where(a => a.AnimalType == AnimalType.Dog && a.Name.ToLower().Contains(name.ToLower()));
            return Ok(animals);
        }

        [Route("api/dogs/{id:int}")]
        public IHttpActionResult Get(int id)
        {
            var animal = _repo.GetAnimal(id);
            if (animal == null)
            {
                return NotFound();
            }

            return Ok(animal);
        }

        [Route("api/Dogs")]
        public IHttpActionResult Post([FromBody]Animal dog)
        {
            if (dog == null)
            {
                return BadRequest("Insuficient data was provided to create the record");
            }

            var result = _repo.CreateAnimal(dog);
            if (result == null)
            {
                return Conflict();
            }

            string uri = Url.Link("DefaultApi", new {id = result.Id});

            return Created(new Uri(uri), result);
        }

        [Route("api/dogs/{id:int}")]
        public IHttpActionResult Put(int id, [FromBody]Animal dog)
        {
            if (dog == null)
            {
                return BadRequest("Insuficient data was provided to update the record");
            }

            Animal result;
            try
            {
                dog.Id = id;
                result = _repo.SaveAnimal(dog);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }


            return Ok(result);
        }

        [Route("api/dogs/{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            _repo.DeleteAnimal(id);
            return Ok();
        }
    }
}
