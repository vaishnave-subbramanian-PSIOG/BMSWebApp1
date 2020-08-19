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
        //[HttpPost]
        //[Route("api/user/user")]
        //public IHttpActionResult PostRegisterUser([FromBody] CUSTOMER customer)
        //{
        //    try
        //    {
        //        var returnStatus = BMSDbMethods.UserRegistration(customer);
        //        if (returnStatus > 0)
        //        {
        //            return Ok();

        //        }
        //        else if (returnStatus == -2)
        //        {
        //            throw new InvalidOperationException("This user already exists.");
        //        }
        //        else
        //        {
        //            throw new InvalidOperationException("User registration failed.");
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        Log.Write(ex);
        //        return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message)); ;
        //    }
        //}
        [HttpPost]
        [Route("api/user/user")]
        public IHttpActionResult PostRegisterUser([FromBody] CUSTOMER customer)
        {
            try
            {

                var returnStatus = BMSDbMethods.UserRegistration(customer);
                if (returnStatus > 0)
                {
                    var encodedEmail = Encryption.base64Encode(customer.CustomerEmail).Replace('.', '+').Replace('_', '/').Replace('-', '=');
                    var link = "http://localhost:3000/verification/" + HttpUtility.UrlEncode(encodedEmail);
                    EmailVerificationLink.EmailLinkGenerator(customer.CustomerEmail, link, "AccountVerification");
                    return Ok();
                }


                else if (returnStatus == -2)
                {
                    throw new InvalidOperationException("This user already exists.");
                }

                else
                {
                    throw new InvalidOperationException("User registration failed.");
                }


            }
            catch (Exception ex)
            {

                Log.Write(ex);
                
               if(ex.Message== "This user already exists.")
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message));
                }
                else {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
                };
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
        public IHttpActionResult GetUser(int id)
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
        public IHttpActionResult PutEditUser(int id, [FromBody] CUSTOMER customer)
        {
            try
            {
                using (BMSApplicationEntities entities = new BMSApplicationEntities())
                {
                    var entity = entities.CUSTOMERs.FirstOrDefault(c => c.CustomerID == id);
                    entity.CustomerName = customer.CustomerName;
                    //generate new salt in case of password  change
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
        public IHttpActionResult DeleteUser(int id)
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
