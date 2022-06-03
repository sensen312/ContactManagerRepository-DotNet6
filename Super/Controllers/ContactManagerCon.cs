using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Super.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactManagerCon : ControllerBase
    {
      
        private readonly DataContext context; // from DBcontext

        public ContactManagerCon(DataContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Contact>>>Get() // gets all contacts
        {
            return Ok(await this.context.Contacts.ToListAsync()); // returns all the contacts as a list
        }
        [HttpGet("{ID}")]
        public async Task<ActionResult<Contact>> Get(Guid ID) // get 1 contact
        {
            var contact = await this.context.Contacts.FindAsync(ID);
            if (contact == null)
                return NotFound("Contact not found!");
            
            return Ok(contact);// returns the one contact
        }

        // NO NEED FOR RETURNS AFTER GET

        [HttpPost]
        public async Task<ActionResult<List<Contact>>> AddContact([FromBody]Contact contact) // adds contact
        {
            this.context.Contacts.Add(contact);
            await this.context.SaveChangesAsync(); // saves to database
            return Ok(await this.context.Contacts.ToListAsync()); // shows us current database contacts  does this need a return?
        }

        [HttpPut]
        public async Task<ActionResult<List<Contact>>> UpdateContact([FromBody] Contact request) //update contact
        {
            var dbcontact = await this.context.Contacts.FindAsync(request.ID);
            if (dbcontact == null)
                return BadRequest("Contact not found!"); // I can refactor if I want
            dbcontact.FirstName = request.FirstName;
            dbcontact.LastName = request.LastName;
            dbcontact.BirthDate = request.BirthDate;

            await this.context.SaveChangesAsync(); // saves to database

            return Ok(await this.context.Contacts.ToListAsync()); 
        }

        [HttpDelete("{ID}")]
        public async Task<ActionResult<List<Contact>>> DeleteContact([FromBody] Guid ID) //cant get rid of body?????
        {
            var dbcontact = await this.context.Contacts.FindAsync(ID);
            if (dbcontact == null)
                return BadRequest("Contact not found!"); // Again I can refactor

            this.context.Contacts.Remove(dbcontact);
            // I have to change it to this.context.DatabaseName

            await this.context.SaveChangesAsync(); // saves to database

            return Ok(await this.context.Contacts.ToListAsync());
        }

    }

}
