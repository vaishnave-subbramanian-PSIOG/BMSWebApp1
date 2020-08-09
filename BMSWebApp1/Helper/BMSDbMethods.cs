using System;
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
                    var keyNew = Encryption.GeneratePassword(10);
                    var password = Encryption.EncodePassword(customer.CustomerPassword, keyNew);
                    customer.CustomerPassword = password;
                    customer.AccountCreateDate = DateTime.Now;
                    customer.VCode = keyNew;
                    db.CUSTOMERs.Add(customer);
                    int a = db.SaveChanges();
                    return a;

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
                            return "Incorrect";
                        }
                    }
                    else
                    {
                        customer = null;
                        return "Invalid Credentials";
                    }
                }
            }
            catch (Exception ex)
            {

                Log.Write(ex);
                customer = null;
                return "Exception Caused";
            }
        }

        //Movie functions
        //public static int AddMovie() { }
    }
}