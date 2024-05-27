using Microsoft.AspNet.Identity;
using QuotesWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.OutputCache.V2;

namespace QuotesWebAPI.Controllers
{
    //everything will be protected with adding Authorize attribute
    [Authorize]
    public class QuoteController : ApiController
    {
        ApplicationDbContext myDbContext = new ApplicationDbContext();

        // GET: api/Quote
        [AllowAnonymous] // with adding this we allow everyone without authentication and autorization have access to all list of quotes
        [HttpGet]
        [CacheOutput(ClientTimeSpan =60,ServerTimeSpan=60)]

        public IHttpActionResult LoadQuotes(String sort)
        {
            IQueryable<Quote> quotes;
            switch (sort)
            {
                case "desc":
                    quotes = myDbContext.Quotes.OrderByDescending(q => q.CreateAt);
                    break;
                case "asc":
                    quotes= myDbContext.Quotes.OrderBy(q => q.CreateAt);
                    break;
                default:
                    quotes= myDbContext.Quotes;
                    break;
            }
            return Ok(quotes);
        }

        [HttpGet]
        [Route("api/quote/PagingQuote/MyQuotes")]
        public IHttpActionResult MyQuotes()
        {
            string userId = User.Identity.GetUserId();

            var quote = myDbContext.Quotes.Where(q => q.UserId==userId);
            return Ok(quote);
        }

        [HttpGet]
        [Route("api/quote/PagingQuote/{pageNumber=}/{pageSize=}")]
        public IHttpActionResult PagingQuote(int pageNumber, int pageSize)
        {
            var quote = myDbContext.Quotes.OrderBy(q=>q.Id);
            return Ok(quote.Skip((pageNumber-1)*pageSize).Take(pageSize));
        }

        [HttpGet]
        [Route("api/quote/SearchQuote/{type=}")]
        public IHttpActionResult SearchQuote(string type)
        {
            var quote = myDbContext.Quotes.Where(q => q.Type.StartsWith(type));
            return Ok(quote);
        }

        // GET: api/Quote/Id
        [HttpGet]
        public IHttpActionResult LoadQuote(int id)
        {
            var quote = myDbContext.Quotes.Find(id);
            if (quote == null)
            {
                return NotFound();
            }
            return Ok(quote);
        }

        [HttpGet]
        [Route("api/Quote/Test/{id}")]
        public int Test(int id)
        {  
            return id;
        }

        // POST: api/Quote
        public IHttpActionResult Post([FromBody]Quote quote)
        {
            string userId = User.Identity.GetUserId();
            quote.UserId = userId;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            myDbContext.Quotes.Add(quote);
            myDbContext.SaveChanges();
            return StatusCode(HttpStatusCode.Created);
        }

        // PUT: api/Quote/id
        public IHttpActionResult Put(int id, [FromBody]Quote quote)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = myDbContext.Quotes.FirstOrDefault(q => q.Id == id);
            
            if (entity == null)
            {
                return BadRequest("No record found against this Id!");
            }

            string userId = User.Identity.GetUserId();

            if (userId != entity.UserId)
            {
                return BadRequest("You don't have right to update this record!!!");
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

            string userId = User.Identity.GetUserId();

            if (userId != quote.UserId)
            {
                return BadRequest("You don't have any right to delete this record!!!");
            }

            myDbContext.Quotes.Remove(quote);
            myDbContext.SaveChanges() ;
            return Ok("Quote deleted successfully...");

        }
    }
} 