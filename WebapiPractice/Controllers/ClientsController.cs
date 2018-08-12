using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebapiPractice.Models;

namespace WebapiPractice.Controllers
{
    [RoutePrefix("api/client")]
    public class ClientsController : ApiController
    {
        private FabricsEntities db = new FabricsEntities();

        public ClientsController()
        {
            db.Configuration.LazyLoadingEnabled = false;
        }

        [Route("")]
        [HttpGet]
        public IHttpActionResult GetClient()
        {
            return Ok(db.Client);
        }


        [Route("{id:int}", Name = nameof(GetClientById))]
        [HttpGet]
        [ResponseType(typeof(Client))]
        public IHttpActionResult GetClientById(int id)
        {
            Client client = db.Client.Find(id);
            if (client == null)
            {
                return NotFound();
            }

            return Ok(client);
        }

        [Route("{id:int}/order")]
        [HttpGet]
        public HttpResponseMessage GetOrderByClientId(int id)
        {
            var orders = db.Order.Where(p => p.ClientId == id);
            return new HttpResponseMessage()
            {
                ReasonPhrase = "HELLO",
                StatusCode = HttpStatusCode.OK,
                Content = new ObjectContent<IQueryable<Order>>(orders,
                    GlobalConfiguration.Configuration.Formatters.JsonFormatter)
            };
        }

        [Route("{id:int}/order/{date:datetime}")]
        [HttpGet]
        public IHttpActionResult GetOrderByClientIdAndDate(int id, DateTime date)
        {
            return Ok(db.Order.Where(o => o.ClientId == id && o.OrderDate <= date).ToList());
        }

        [Route("{id:int}")]
        [HttpPut]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutClient(int id, Client client)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != client.ClientId)
            {
                return BadRequest();
            }

            db.Entry(client).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(id))
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

        [Route("")]
        [HttpPost]
        [ResponseType(typeof(Client))]
        public IHttpActionResult PostClient(Client client)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Client.Add(client);
            db.SaveChanges();

            return CreatedAtRoute(nameof(GetClientById), new { id = client.ClientId }, client);
        }

        [Route("{id:int}")]
        [HttpDelete]
        [ResponseType(typeof(Client))]
        public IHttpActionResult DeleteClient(int id)
        {
            Client client = db.Client.Find(id);
            if (client == null)
            {
                return NotFound();
            }

            db.Client.Remove(client);
            db.SaveChanges();

            return Ok(client);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ClientExists(int id)
        {
            return db.Client.Count(e => e.ClientId == id) > 0;
        }
    }
}