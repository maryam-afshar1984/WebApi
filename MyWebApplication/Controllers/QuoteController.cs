using MyWebApplication.Data;
using MyWebApplication.Models;
using System;
using System.Collections.Generic;
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
        public Quote Get(int id)
        {
            var quote = myDbContext.Quotes.Find(id);
            return quote;
        }

        // POST: api/Quote
        public IHttpActionResult Post([FromBody]Quote quote)
        {
            myDbContext.Quotes.Add(quote);
            myDbContext.SaveChanges();
            return StatusCode(HttpStatusCode.Created);
        }

        // PUT: api/Quote/id
        public void Put(int id, [FromBody]Quote quote)
        {
            var entity = myDbContext.Quotes.FirstOrDefault(q => q.Id == id);
            entity.Title = quote.Title;
            entity.Description = quote.Description;
            entity.Author = quote.Author;
            myDbContext.SaveChanges();
        }

        // DELETE: api/Quote/5
        public void Delete(int id)
        {
            var quote = myDbContext.Quotes.Find(id) ;
            myDbContext.Quotes.Remove(quote);
            myDbContext.SaveChanges() ;

        }
    }
} 