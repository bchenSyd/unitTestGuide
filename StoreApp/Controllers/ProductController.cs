using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using StoreApp.Models;

namespace StoreApp.Controllers
{
    public class ProductController : ApiController
    {
        private IStoreAppContext db = new StoreAppContext();

        public ProductController() { }

        /// <summary>
        /// for unit test
        /// </summary>
        public ProductController(IStoreAppContext context)
        {
            db = context;
        }

        /// <summary>
        /// in short, EF keeps track your model object and commit only changed objects to database when SaveChanges() is called;
        /// calling SaveChanges won't reset changeTracker;
        /// </summary>
        [Route("api/test")]
        public void ChangeTracker()
        {
            var ctx = db as StoreAppContext;
            var prod1 = ctx.Products.Find(5);
            Debug.WriteLine("change track count is now : "+ ctx.ChangeTracker.Entries().Count());

            var category = ctx.Categories.Find(5);
            Debug.WriteLine("change track count is now : " + ctx.ChangeTracker.Entries().Count());
            
            
            category.CategoryCode = "tool-updated";
            Debug.WriteLine("change track count is now : " + ctx.ChangeTracker.Entries().Count());
            ctx.SaveChanges();
            Debug.WriteLine("change track count is now : " + ctx.ChangeTracker.Entries().Count());

            prod1.CategoryId = 7;
            Debug.WriteLine("change track count is now : " + ctx.ChangeTracker.Entries().Count());
            ctx.SaveChanges();


            Debug.WriteLine("change track count is now : " + ctx.ChangeTracker.Entries().Count());
            category.CategoryCode = "too-update2";
            Debug.WriteLine("change track count is now : " + ctx.ChangeTracker.Entries().Count());
            ctx.SaveChanges();

            Debug.WriteLine("change track count is now : " + ctx.ChangeTracker.Entries().Count());
            ctx.Products.Add(new Product
            {
                Name = "from code",
                Category = new Category
                {
                    CategoryCode = "from Code",
                    Id = 10
                }

            });
            Debug.WriteLine("change track count is now : " + ctx.ChangeTracker.Entries().Count());

            ctx.SaveChanges();
            Debug.WriteLine("change track count is now : " + ctx.ChangeTracker.Entries().Count());
        }



        // GET: api/Product  ===> get all products
        public IQueryable<Product> GetProducts()
        {
            return db.Products;
        }

        // GET: api/Product/5   ==> get a particular product
        [ResponseType(typeof(Product))] 
        public IHttpActionResult GetProduct(int id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // PUT: api/Product/5  ==> PUT for update
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProduct(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.Id)
            {
                return BadRequest();
            }

            //db.Entry(product).State = EntityState.Modified;
            db.MarkAsModified(product);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Product   ==> POST for adding/creating
        [ResponseType(typeof(Product))]
        public IHttpActionResult PostProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Products.Add(product);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = product.Id }, product);
        }

        // DELETE: api/Product/5   ==> DELETE for deleting
        [ResponseType(typeof(Product))]
        public IHttpActionResult DeleteProduct(int id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            db.Products.Remove(product);
            db.SaveChanges();

            return Ok(product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int id)
        {
            return db.Products.Count(e => e.Id == id) > 0;
        }
    }
}