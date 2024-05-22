using MyWebApplication.Data;
using MyWebApplication.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MyWebApplication.Controllers
{
    public class QuoteController : ApiController
    {
        MyDbContext myDbContext = new MyDbContext();
        // GET: api/Quote
        public IHttpActionResult Get()
        {
            var quote = myDbContext.Quotes ;
            return Ok(quote);
        }

        // GET: api/Quote/Id
        public IHttpActionResult Get(int id)
        {
            var quote = myDbContext.Quotes.Find(id);
            if (quote == null)
            {
                return NotFound();
            }
            return Ok(quote);
        }

        // POST: api/Quote
        public IHttpActionResult Post([FromBody]Quote quote)
        {
            myDbContext.Quotes.Add(quote);
            myDbContext.SaveChanges();
            return StatusCode(HttpStatusCode.Created);
        }

        // PUT: api/Quote/id
        public IHttpActionResult Put(int id, [FromBody]Quote quote)
        {
            var entity = myDbContext.Quotes.FirstOrDefault(q => q.Id == id);
            if (entity == null)
            {
                return BadRequest("No record found against this Id!");
            }
            entity.Title = quote.Title;
            entity.Description = quote.Description;
            entity.Author = quote.Author;
            myDbContext.SaveChanges();
            return Ok("Record updated successfully...");

        }

        // DELETE: api/Quote/5
        public IHttpActionResult Delete(int id)
        {
            var quote = myDbContext.Quotes.Find(id) ;
            if (quote == null)
            {
                return BadRequest("No record found against this Id!");// we can also return NotFound() method.

            }
            myDbContext.Quotes.Remove(quote);
            myDbContext.SaveChanges() ;
            return Ok("Quote deleted successfully...");

        }
    }
} 