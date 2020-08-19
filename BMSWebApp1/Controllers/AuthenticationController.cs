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
                        var encodedEmail = Encryption.base64Encode(email).Replace('+', '.').Replace('/', '_').Replace('=', '-');
                        //var encodedEmail = Encryption.base64Encode(email);
                        var link = "https://" + HttpContext.Current.Request.Url.Authority + "/reset-password/" + HttpUtility.UrlEncode(encodedEmail);
                        EmailVerificationLink.EmailLinkGenerator(email, link, "ResetPassword");
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
                var decodedEmail = Encryption.base64Decode(resetPassword.token.Replace('.', '+').Replace('_', '/').Replace('-', '='));
                var isTimeoutResult = BMSDbMethods.isResetPasswordTimeout(decodedEmail);
                switch (isTimeoutResult)
                {
                    case "Not Timeout":
                        if (BMSDbMethods.EditPassword(decodedEmail, resetPassword.newpassword) == 1)
                        {
                            EmailVerificationLink.EmailLinkGenerator(decodedEmail, "", "PostResetPassword");
                            return Ok("Password has been changed.");

                        }
                        else
                        {
                            throw new InvalidOperationException("Change password failed.");
                        }

                    case "Timeout":
                        throw new InvalidOperationException("Reset password session has expired.");

                    default:
                        throw new InvalidOperationException(isTimeoutResult);
            }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                if (ex.Message== "Reset password session has expired.") {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message));
                }
                else if (ex.Message.Contains("Error in base64Decod"))
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid Token"));

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
                var result = BMSDbMethods.VerifyAccount(HttpUtility.UrlDecode(token).Replace('.', '+').Replace('_', '/').Replace('-', '='));
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
