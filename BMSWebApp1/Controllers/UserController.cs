using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using BMSDbEntities;
using BMSWebApp1.Helper;

namespace BMSWebApp1.Controllers
{
    public class UserController : ApiController
    {
        [HttpPost]
        [Route("api/user/user")]
        public IHttpActionResult PostRegisterUser([FromBody] CUSTOMER customer)
        {
            try
            {
                var returnStatus = BMSDbMethods.UserRegistration(customer);
                if (returnStatus>0)
                {
                    return Ok();

                }
                else
                {
                    throw new InvalidOperationException("User registration failed.");
                }

            }
            catch (Exception ex)
            {

                Log.Write(ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Database connection problem")); ;
            }
        }
        [HttpGet]
        [Route("api/user/users")]

        public IHttpActionResult GetUsers()
        {
            try
            {
                using (BMSApplicationEntities entities = new BMSApplicationEntities())
                {
                    return Ok(entities.CUSTOMERs.ToList());
                }
            }
            catch (Exception ex)
            {

                Log.Write(ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Database connection problem")); ;
            }
        }

        [HttpGet]
        [Route("api/user/user/{id?}")]
        public IHttpActionResult Get(int id)
        {
            try
            {
                using (BMSApplicationEntities entities = new BMSApplicationEntities())
                {
                    return Ok(entities.CUSTOMERs.FirstOrDefault(c => c.CustomerID == id));
                }
            }
            catch (Exception ex)
            {

                Log.Write(ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Database connection problem")); ;
            }
        }


        // PUT api/values/5
        [HttpPut]

        [Route("api/user/user/{id?}")]
        public IHttpActionResult Put(int id, [FromBody] CUSTOMER customer)
        {
            try
            {
                using (BMSApplicationEntities entities = new BMSApplicationEntities())
                {
                    var entity = entities.CUSTOMERs.FirstOrDefault(c => c.CustomerID == id);
                    entity.CustomerName = customer.CustomerName;
                    entity.CustomerEmail = customer.CustomerEmail;
                    entity.CustomerPassword = customer.CustomerEmail;
                    entity.CustomerAddress = customer.CustomerAddress;
                    entity.CustomerContact = customer.CustomerContact;
                    entities.SaveChanges();
                    return Ok();

                }
            }
            catch (Exception ex)
            {

                Log.Write(ex);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Database connection problem")); ;
            }
        }

        [HttpDelete]
        [Route("api/user/user/{id?}")]
        public IHttpActionResult Delete(int id)
        {

        try
        {
            using (BMSApplicationEntities entities = new BMSApplicationEntities())
            {
                entities.CUSTOMERs.Remove(entities.CUSTOMERs.FirstOrDefault(c => c.CustomerID == id));
                entities.SaveChanges();
                return Ok();

            }
        }
        catch (Exception ex)
        {

            Log.Write(ex);
            return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Database connection problem")); ;
        }
    }
 }

}
