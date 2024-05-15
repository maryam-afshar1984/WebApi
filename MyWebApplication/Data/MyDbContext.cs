using MyWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MyWebApplication.Data
{
    public class MyDbContext:DbContext
    {
        public DbSet<Quote> Quotes { get; set; }
    }
}