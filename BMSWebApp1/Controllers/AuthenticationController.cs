using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using BMSDbEntities;
using BMSWebApp1.Helper;
using BMSWebApp1.Models;

namespace BMSWebApp1.Controllers
{
    public class AuthenticationController : ApiController
    {
        [HttpPost]
        [Route("api/authentication/forgotpassword")]
        public IHttpActionResult PostForgotPassword([FromUri] string email)
        {
            try
            {
                    if (BMSDbMethods.DoesUserExists(email))
                    {
                        string VerificationCode = Guid.NewGuid().ToString();
                        var encodedEmail = Encryption.base64Encode(email);
                        var link = "https://" + HttpContext.Current.Request.Url.Authority + "/reset-password/" + HttpUtility.UrlEncode(encodedEmail);
                        EmailVerificationLink.EmailLinkGenerator(email, VerificationCode, link, "ResetPassword");
                        var isTimeoutSet = BMSDbMethods.SetResetPasswordTimeout(email);
                        if (isTimeoutSet)
                        {
                            return Ok("A reset link has been sent.");
                        }
                        else
                        {
                            throw new InvalidOperationException("Timeout Not Set");

                    }
                }

                    else
                    {
                        throw new InvalidOperationException("This user doesn't exist.");
                    }

            }
            catch (Exception ex)
            {

                Log.Write(ex);
                if (ex.Message == "This user doesn't exist.")
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message));
                }
                else {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
                }
            }
        }

        [HttpPut]
        [Route("api/authentication/resetpassword")]
        public IHttpActionResult PutResetPassword([FromBody] ResetPasswordModel resetPassword)
        {
            try
            {
                var decodedEmail = Encryption.base64Decode(resetPassword.token);
                var isTimeoutResult = BMSDbMethods.isResetPasswordTimeout(decodedEmail);
                if (isTimeoutResult=="Not Timeout") {
                    if (BMSDbMethods.EditPassword(decodedEmail, resetPassword.newpassword)==1)
                    {
                        BMSDbMethods.NullResetPasswordTimeout(decodedEmail);
                        return Ok("Password has been changed.");

                    }
                    else
                    {
                        throw new InvalidOperationException("Change password failed.");
                    }
                }
                else if (isTimeoutResult == "Timeout")
                {
                    throw new InvalidOperationException("Reset password session has expired.");
                }
                else
                {
                    throw new InvalidOperationException(isTimeoutResult);
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                if (ex.Message== "Reset password session has expired.") {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message));
                }
                else
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
                }

            }
        }

        [HttpPost]
        [Route("api/authentication/verifyemail")]
        public IHttpActionResult PostVerifyEmail([FromUri] string token)
        {
            try
            {
                var result = BMSDbMethods.VerifyAccount(token);
                switch (result)
                {
                    case "Verified":
                        return Ok("This account has been verified.");

                    default:
                        throw new InvalidOperationException(result);

                }
            }
            catch (Exception ex)
            {

                Log.Write(ex);
                if (ex.Message=="Verification Failed") {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
                }
                else
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message));
                }
            }
        }
    }
}
