using EpiDHL.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace EpiDHL.Controllers
{
    public class UsersController : Controller
    {
        // GET: Users
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Signup()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Signup([Bind(Include = "Email, Password")] User admin)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Db.conn.Open();
                    var cmdSelect = new SqlCommand("SELECT COUNT(*) FROM Users WHERE Email = @email", Db.conn);
                    cmdSelect.Parameters.AddWithValue("@email", admin.Email);
                    int count = (int)cmdSelect.ExecuteScalar();

                    if (count == 0)
                    {
                        
                        var cmdInsert = new SqlCommand(@"INSERT INTO Users (Email, Password) VALUES (@email, @password)", Db.conn);
                        cmdInsert.Parameters.AddWithValue("@email", admin.Email);
                        cmdInsert.Parameters.AddWithValue("@password", admin.Password);
                        int rowsAffected = cmdInsert.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            return RedirectToAction("Login", "Users");
                        }
                        else
                        {
                            ViewBag.ErrorMessage = "Errore durante l'inserimento dell'utente.";
                        }
                    }
                    else
                    {
                     
                        ViewBag.ErrorMessage = "L'email fornita è già associata a un altro utente.";
                        return View();
                    }
                }
                catch (Exception ex)
                {
                  
                    ViewBag.ErrorMessage = "Si è verificato un errore durante l'elaborazione della richiesta.";
                  
                }
                finally
                {
                    Db.conn.Close();
                }
            }

          
            return View(admin);
        }


        [HttpGet]
        public ActionResult Login()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "Email, Password")] User user)
        {

            try
            {
                Db.conn.Open();

                var cmd = new SqlCommand("SELECT * FROM Users WHERE Email = @email AND Password = @password", Db.conn);
                cmd.Parameters.AddWithValue("@email", user.Email);
                cmd.Parameters.AddWithValue("@password", user.Password);

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    FormsAuthentication.SetAuthCookie(reader["User_ID"].ToString(), true);
                    return RedirectToAction("LoggedIn", "Users");

                }
                else
                {
                    ViewBag.ErrorMessage = "L'account non esiste";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Si è verificato un errore durante l'elaborazione della richiesta.";
            }
            finally
            {
                Db.conn.Close();
            }
            

            
            return RedirectToAction("Index");
        }


        [Authorize]
        [HttpGet]
        public ActionResult LoggedIn()
        {
            try
            {
                Db.conn.Open();
                var cmd = new SqlCommand("SELECT * FROM Users WHERE User_ID = @id", Db.conn);
                cmd.Parameters.AddWithValue("@id",int.Parse(HttpContext.User.Identity.Name));
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    if (reader.Read()) 
                    {
                        var utente = new User()
                        {
                            Email = (string)reader["Email"],
                            Password = (string)reader["Password"],
                        };

                        ViewBag.Utente = utente;
                        Db.conn.Close();
                        return View();
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Utente non trovato nel database.";
                }

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Problemi con il login";
            }
            finally
            {
                Db.conn.Close();
            }

            return View();
        }


        [Authorize, HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            // Sloggare l'utente

            FormsAuthentication.SignOut();

            // Ridirezionarlo da qualche parte

            return RedirectToAction("Index", "Home");
        }
    }
}