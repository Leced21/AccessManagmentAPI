using AccessManagmentAPI.Context;
using AccessManagmentAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AccessManagmentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class User:ControllerBase
    {
        private readonly ContetxtDb _contextdb;
        public User(ContetxtDb contetxtDb)
        {
            _contextdb = contetxtDb;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Userregister>>> Getalluser()
        {
            if (_contextdb.Userregisters == null)
            {
                return NotFound();
            }
            return await _contextdb.Userregisters.ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById( int id)
        {
        
            var user = await _contextdb.Userregisters.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<Userregister>> userregistration(Userregister user)
        {
            if (_contextdb.Userregisters == null)
            {
                return Problem("Entity set 'ContetxtDb.Userregisters'  is null.");
            }
            await _contextdb.Userregisters.AddAsync(user);
            await _contextdb.SaveChangesAsync();

            return CreatedAtAction("Getalluser", new { id = user.Id }, user);
        }
    }
}
