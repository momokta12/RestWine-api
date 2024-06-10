using WineLib;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RestWine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WineController : ControllerBase
    {

        private WineRepository _wineLibrepo;
        public WineController(WineRepository wineLibrepo)
        {
            _wineLibrepo = wineLibrepo;
        }


        // GET: api/<WineController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public ActionResult<IEnumerable<Wine>> Get()
        {
            IEnumerable<Wine> wines = _wineLibrepo.GetAll();
            if (wines.Count() == 0)
            {
                return NoContent();
            }
            else
                {
                return Ok(wines);
            }
        }

        // GET api/<WineController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Wine> Get(int id)
        {
            Wine? wine = _wineLibrepo.GetById(id);
            if (wine is null)
            {
                return NotFound();
            }
            else
            {
                return Ok(wine);
            }
        }

        // POST api/<WineController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Wine> Post([FromBody] Wine wine)
        {
            //try gør det at vi prøver at tilføje en vin, hvis det ikke lykkedes, så returneres en BadRequest
            //hvis det lykkedes, så returneres en CreatedAtAction
            //CreatedAtAction betyder at vi returnerer en 201 Created status code
            try
            {                 
                Wine newWine = _wineLibrepo.Add(wine);
                return CreatedAtAction(nameof(Get), new { id = newWine.Id }, newWine);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<WineController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Wine> Put([FromBody] Wine wine)
        {
            try
            {
                Wine updatedWine = _wineLibrepo.Update(wine);
                return Ok(updatedWine);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<WineController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Wine> Delete(int id)
        {
            Wine DeleteWine = _wineLibrepo.Delete(id);
            if (_wineLibrepo.GetById(id) is null)
            {
                return Ok();
            }
            else
            { 
                return NotFound();
            }
        }
    }
}
