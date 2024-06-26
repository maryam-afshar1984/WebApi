﻿using MyWebApplication.Data;
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
        [HttpGet]
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