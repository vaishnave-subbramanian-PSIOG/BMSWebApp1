﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BMSDbEntities;

namespace BMSWebApp1.Helper
{
    public class BMSDbMethods
    {
        //User functions
        public static int UserRegistration(CUSTOMER customer) //Registers user
        {
            try
            {
                using (BMSApplicationEntities db = new BMSApplicationEntities())
                {
                    if (!DoesUserExists(customer.CustomerEmail)) {
                        var keyNew = Encryption.GeneratePassword(10);
                        var password = Encryption.EncodePassword(customer.CustomerPassword, keyNew);
                        customer.CustomerPassword = password;
                        customer.AccountCreateDate = DateTime.Now;
                        customer.VCode = keyNew;
                        customer.isVerified = false;
                        db.CUSTOMERs.Add(customer);
                        int a = db.SaveChanges();
                        return a;
                    }
                    else
                    {
                        return -2; //Email already exists in db

                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return -1;
            }
        }
        public static string LoginAttempt(string email, string password, out CUSTOMER customer) //Login
        {
            try
            {
                using (BMSApplicationEntities db = new BMSApplicationEntities())
                {
                    customer = db.CUSTOMERs.Where(l => l.CustomerEmail == email).FirstOrDefault();
                    if (customer != null)
                    {
                        if (customer.isVerified) 
                        { 
                            var hashCode = customer.VCode;
                            //Password Hasing Process Call Helper Class Method    
                            var encodingPasswordString = Encryption.EncodePassword(password, hashCode);
                            if (String.Compare(encodingPasswordString, customer.CustomerPassword) == 0)
                            {
                                return "Success";
                            }
                            else
                            {
                                customer = null;
                                return "Invalid Credentials";
                            }
                        }
                        else
                        {
                            return "User Not Verified";
                        }

                    }
                    else
                    {
                        customer = null;
                        return "User Doesn't Exist";
                    }
                }
            }
            catch (Exception ex)
            {

                Log.Write(ex);
                customer = null;
                return "Exception Occured";
            }
        }

        public static int EditPassword(string email,string password) //Changing password
        {
            try
            {
                using (BMSApplicationEntities entities = new BMSApplicationEntities())
                {
                    var entity = entities.CUSTOMERs.FirstOrDefault(c => c.CustomerEmail == email);
                    var keyNew = Encryption.GeneratePassword(10);
                    var encodedPassword = Encryption.EncodePassword(password, keyNew);
                    entity.CustomerPassword = encodedPassword;
                    entity.VCode = keyNew;
                    entity.ResetPasswordTimeout = null;
                    entities.SaveChanges();
                    return 1;

                }
            }
            catch (Exception ex)
            {

                Log.Write(ex);
                return 0;
            }
        }

        public static bool DoesUserExists(string email) //checks if email ID of user exists
        {
            try
            {
                using (BMSApplicationEntities entities = new BMSApplicationEntities())

                {
                    var entity = entities.CUSTOMERs.FirstOrDefault(c => c.CustomerEmail == email);
                    return entity != null;
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return false;
            }

        }

        public static string VerifyAccount(string encodedEmail) //changes verified account status for user
        {
            try
            {
                using (BMSApplicationEntities entities = new BMSApplicationEntities())

                {
                    var decodedEmail = Encryption.base64Decode(encodedEmail);
                    var entity = entities.CUSTOMERs.FirstOrDefault(c => c.CustomerEmail == decodedEmail);
                    if (entity!=null)
                    {
                        if(!entity.isVerified) 
                        { entity.isVerified = true; 
                            entities.SaveChanges();
                            return "Verified";
                        }
                        else 
                        { return "Already Verified"; }

                    }
                    else
                    {
                        return "Invalid Token";

                    }


                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                if (ex.Message.Contains("Error in base64Decod"))
                {
                    return "Invalid Token";
                }
                return "Verification Failed";
            }

        }

        public static bool SetResetPasswordTimeout(string email) //sets reset password timeout field for 15 minutes from current time
        {
            try
            {
                using (BMSApplicationEntities entities = new BMSApplicationEntities())

                {
                    var entity = entities.CUSTOMERs.FirstOrDefault(c => c.CustomerEmail == email);
                    entity.ResetPasswordTimeout = DateTime.Now.AddMinutes(15);
                    entities.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return false;
            }
        }
        public static string isResetPasswordTimeout(string email) //sets reset password timeout field for 15 minutes from current time
        {
            try
            {
                using (BMSApplicationEntities entities = new BMSApplicationEntities())

                {
                    var entity = entities.CUSTOMERs.FirstOrDefault(c => c.CustomerEmail == email);
                    if (entity.ResetPasswordTimeout!=null) {
                        if (entity.ResetPasswordTimeout > DateTime.Now)
                        {
                            return "Not Timeout";
                        }
                        else
                        {
                            entity.ResetPasswordTimeout = null;
                            return "Timeout";
                        }
                    }
                    else
                    {
                        return "Timeout";
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return ex.Message;
            }
        }



        //Movie functions
        //public static int AddMovie() { }
        //public static IEnumerable MoviesList() {
        //    using (BMSApplicationEntities entities = new BMSApplicationEntities())
        //    {
        //        List<MOVIE> AllMovies = entities.MOVIEs.ToList();
        //        List<DIRECTOR> AllDirectors = entities.DIRECTORs.ToList();
        //        List<GENRE> AllGenres = entities.GENREs.ToList();

        //        var movieInfo = (from m in AllMovies
        //                         join d in AllDirectors on m.DirectorID equals d.DirectorID
        //                         join g in AllGenres on m.GenreID equals g.GenreID
        //                         select new
        //                         {
        //                             MovieID = m.MovieID,
        //                             MovieName = m.MovieName,
        //                             DirectorName = d.DirectorName,
        //                             GenreName = g.GenreName,
        //                         });
        //        return movieInfo;
        //    }
        //}
    }
}