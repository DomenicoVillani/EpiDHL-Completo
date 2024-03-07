using EpiDHL.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace EpiDHL.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            List<Cliente> clienti = new List<Cliente>();
            try
            {
                Db.conn.Open();

                var cmd = new SqlCommand("SELECT * FROM Clienti", Db.conn);
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while ( reader.Read())
                    {
                        var cliente = new Cliente()
                        {
                            Cliente_ID = Convert.ToInt32(reader["Cliente_ID"]),
                            Azienda = (bool)reader["Azienda"],
                            Cod_Fisc = reader["Cod_Fisc"].ToString(),
                            PI = reader["PI"].ToString(),
                            Email = reader["Email"].ToString(),
                            Tel = reader["Tel"].ToString(),
                            Nome = reader["Nome"].ToString(),
                            Cognome = reader["Cognome"].ToString()
                        };

                        clienti.Add(cliente);
                    }

                    reader.Close();
                    return View(clienti);
                }


            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
            finally
            {
                Db.conn.Close();
            }

            return View(clienti);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add([Bind(Include = "Azienda, Cod_Fisc, PI, Email, Tel, Nome, Cognome")] Cliente cliente)
        {
            if (cliente.Azienda == true )
            {
                if (cliente.Cod_Fisc == null && cliente.PI == null)
                {
                    ViewBag.ErrorMessage = "Partita Iva è un campo obbligatorio";
                }
            }
            else if (cliente.Azienda == false)
            {
                if (cliente.Cod_Fisc == null && cliente.PI == null)
                {
                    ViewBag.ErrorMessage = "Codice Fiscale è un campo obbligatorio";
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Db.conn.Open();

                    string query = @"INSERT INTO Clienti (Azienda, Cod_Fisc, PI, Email, Tel, Nome, Cognome)
                 VALUES (@azienda, @cod_fisc, @pi, @email, @tel, @nome, @cognome)";

                    using (SqlCommand cmd = new SqlCommand(query, Db.conn))
                    {
                        cmd.Parameters.AddWithValue("@azienda", cliente.Azienda);

                        if (cliente.Azienda)
                        {
                         
                            if (string.IsNullOrEmpty(cliente.PI))
                            {
                                if (ModelState.IsValid)
                                {
                                    return View(cliente);
                                }
                            }
                            cmd.Parameters.AddWithValue("@pi", cliente.PI);
                            cmd.Parameters.AddWithValue("@cod_fisc", DBNull.Value); 
                        }
                        else 
                        {
                           
                            if (string.IsNullOrEmpty(cliente.Cod_Fisc))
                            {
                                if(ModelState.IsValid)
                                {
                                    return View(cliente);
                                }
                             
                            }
                            cmd.Parameters.AddWithValue("@cod_fisc", cliente.Cod_Fisc);
                            cmd.Parameters.AddWithValue("@pi", DBNull.Value); 
                        }

                        
                        cmd.Parameters.AddWithValue("@email", cliente.Email);
                        cmd.Parameters.AddWithValue("@tel", cliente.Tel);
                        cmd.Parameters.AddWithValue("@nome", cliente.Nome);
                        cmd.Parameters.AddWithValue("@cognome", cliente.Cognome);

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    return View(ex.Message);
                }
                finally
                {
                    Db.conn.Close();
                }

                return RedirectToAction("Index");
            }

            return View(cliente);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            Cliente cliente = null;

            try
            {
                Db.conn.Open();

                var cmd = new SqlCommand("SELECT * FROM Clienti WHERE Cliente_ID = @id", Db.conn);
                cmd.Parameters.AddWithValue("@id", id);
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    cliente = new Cliente
                    {
                        Cliente_ID = Convert.ToInt32(reader["Cliente_ID"]),
                        Azienda = (bool)reader["Azienda"],
                        Cod_Fisc = reader["Cod_Fisc"].ToString(),
                        PI = reader["PI"].ToString(),
                        Email = reader["Email"].ToString(),
                        Tel = reader["Tel"].ToString(),
                        Nome = reader["Nome"].ToString(),
                        Cognome = reader["Cognome"].ToString()
                    };

                }
                reader.Close();

            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
            finally
            {
                Db.conn.Close();
            }

            return View(cliente);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete([Bind(Include = "Cliente_ID")] Cliente cliente)
        {
           try
            {
                Db.conn.Open();

                var cmd = new SqlCommand("DELETE FROM Clienti WHERE Cliente_ID = @id", Db.conn);
                cmd.Parameters.AddWithValue("@id", cliente.Cliente_ID);
                cmd.ExecuteNonQuery();


            }
            catch (Exception ex)
            {

            }
            finally
            {
                Db.conn.Close();
            }


            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            Cliente cliente = null;

            try
            {
                Db.conn.Open();

                var cmd = new SqlCommand("SELECT * FROM Clienti WHERE Cliente_ID = @id", Db.conn);
                cmd.Parameters.AddWithValue("@id", id);

                var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    cliente = new Cliente()
                    {
                        Cliente_ID = Convert.ToInt32(reader["Cliente_ID"]),
                        Azienda = (bool)reader["Azienda"],
                        Cod_Fisc = reader["Cod_Fisc"].ToString(),
                        PI = reader["PI"].ToString(),
                        Email = reader["Email"].ToString(),
                        Tel = reader["Tel"].ToString(),
                        Nome = reader["Nome"].ToString(),
                        Cognome = reader["Cognome"].ToString()
                    };
                }


            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
            finally
            {
                Db.conn.Close();
            }

            return View(cliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Azienda, Cod_Fisc, PI, Email, Tel, Nome, Cognome")] Cliente cliente)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Db.conn.Open();

                    var cmd = new SqlCommand(@"UPDATE Clienti
                                        SET Azienda = @azienda,
                                            Cod_Fisc = @cod_fisc,
                                            PI = @pi,
                                            Email = @email,
                                            Tel = @tel,
                                            Nome = @nome,
                                            Cognome = @cognome
                                            WHERE Cliente_ID = @id", Db.conn);

                    cmd.Parameters.AddWithValue("@azienda", cliente.Azienda);
                    cmd.Parameters.AddWithValue("@cod_fisc", cliente.Cod_Fisc);
                    cmd.Parameters.AddWithValue("@pi", cliente.PI);
                    cmd.Parameters.AddWithValue("@email", cliente.Email);
                    cmd.Parameters.AddWithValue("@tel", cliente.Tel);
                    cmd.Parameters.AddWithValue("@nome", cliente.Nome);
                    cmd.Parameters.AddWithValue("@cognome", cliente.Cognome);
                    cmd.Parameters.AddWithValue("@id", cliente.Cliente_ID);

                    cmd.ExecuteNonQuery();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(cliente);
                }
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
            finally
            {
                Db.conn.Close();
            }
        }

        [HttpGet]
        public ActionResult Ricerca()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Ricerca(Cliente cliente)
        {

            List<Cliente> clienti = new List<Cliente>();
            try
            {
                Db.conn.Open();
                var cmd = new SqlCommand("SELECT * FROM Clienti WHERE Nome LIKE '%' + @nome + '%'", Db.conn);
                cmd.Parameters.AddWithValue("@nome", cliente.Nome);

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    cliente = new Cliente()
                    {
                        Cliente_ID = (int)reader["Cliente_ID"],
                        Azienda = (bool)reader["Azienda"],
                        Cod_Fisc = reader["Cod_Fisc"].ToString(),
                        PI = reader["PI"].ToString(),
                        Email = reader["Email"].ToString(),
                        Tel = reader["Tel"].ToString(),
                        Nome = reader["Nome"].ToString(),
                        Cognome = reader["Cognome"].ToString()
                    };

                    clienti.Add(cliente);
                }
                reader.Close();

                if (clienti.Count > 0)
                {
                    return View(clienti);
                }
                else
                {
                    ViewBag.ErrorMessage = "Nessun cliente trovato con questo nome";
                }

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Errore nella ricerca";
            }
            finally
            {
                Db.conn.Close();
            }

            return View();
        }

        [HttpGet]
        public ActionResult Dettagli(int id)
        {
            Cliente cliente = null;

            try
            {
                Db.conn.Open();

                var cmd = new SqlCommand("SELECT * FROM Clienti WHERE Cliente_ID = @id", Db.conn);
                cmd.Parameters.AddWithValue("@id", id);
                var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    cliente = new Cliente()
                    {
                        Cliente_ID = Convert.ToInt32(reader["Cliente_ID"]),
                        Azienda = (bool)reader["Azienda"],
                        Cod_Fisc = reader["Cod_Fisc"].ToString(),
                        PI = reader["PI"].ToString(),
                        Email = reader["Email"].ToString(),
                        Tel = reader["Tel"].ToString(),
                        Nome = reader["Nome"].ToString(),
                        Cognome = reader["Cognome"].ToString()
                    };
                }
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
            finally
            {
                Db.conn.Close();
            }

            return View(cliente);
        }
        [HttpGet]
        public ActionResult RicercaOrdine()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RicercaOrdine(string ricerca)
        {
            List<Spedizioni> spedizioni = new List<Spedizioni>();
            try
            {
                Db.conn.Open();

                var cmd = new SqlCommand(@"SELECT *
                                FROM Spedizioni
                                JOIN Clienti ON Spedizioni.Cliente_ID = Clienti.Cliente_ID
                                WHERE Cod_Fisc = @ricerca OR PI = @ricerca", Db.conn);
                cmd.Parameters.AddWithValue("@ricerca", ricerca);
                var reader = cmd.ExecuteReader();
                if(reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Spedizioni spedizione = new Spedizioni()
                        {
                            Spedizione_ID = Convert.ToInt32(reader["Spedizione_ID"]),
                            Data_Spedizione = (DateTime)reader["Data_Spedizione"],
                            Cod_Sped = (int)reader["Cod_Sped"],
                            Peso = (decimal)reader["Peso"],
                            Citta_Dest = reader["Citta_Dest"].ToString(),
                            Indirizzo = reader["Indirizzo"].ToString(),
                            Destinatario = reader["Destinatario"].ToString(),
                            Costo = (decimal)reader["Costo"],
                            Data_Prev = (DateTime)reader["Data_Prev"],
                            Cliente_ID = Convert.ToInt32(reader["Cliente_ID"]),
                        };

                        spedizioni.Add(spedizione);
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
            finally
            {
                Db.conn.Close();
            }
            return View(spedizioni);
        }

        public ActionResult DettaglioOrdine(int id)
        {
            Spedizioni spedizione = null;

            try
            {
                Db.conn.Open();

                var query = "SELECT * FROM Spedizioni WHERE Spedizione_ID = @id";
                var cmd = new SqlCommand(query, Db.conn);
                cmd.Parameters.AddWithValue("@id", id);

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();

                    spedizione = new Spedizioni
                    {
                        Spedizione_ID = Convert.ToInt32(reader["Spedizione_ID"]),
                        Data_Spedizione = (DateTime)reader["Data_Spedizione"],
                        Cod_Sped = (int)reader["Cod_Sped"],
                        Peso = (decimal)reader["Peso"],
                        Citta_Dest = reader["Citta_Dest"].ToString(),
                        Indirizzo = reader["Indirizzo"].ToString(),
                        Destinatario = reader["Destinatario"].ToString(),
                        Costo = (decimal)reader["Costo"],
                        Data_Prev = (DateTime)reader["Data_Prev"],
                        Cliente_ID = Convert.ToInt32(reader["Cliente_ID"])
                    };
                }
                reader.Close();
                return View(spedizione);
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
            finally
            {
                Db.conn.Close();
            }
        }

        
        public ActionResult StatoSpedizione(int id)
        {
            List<Aggiornamenti> aggiornamenti = new List<Aggiornamenti>();
            try
            {
                Db.conn.Open();
                var query = "SELECT * FROM Spedizioni INNER JOIN Aggiornamenti ON Spedizioni.Spedizione_ID = Aggiornamenti.Spedizione_ID WHERE Spedizioni.Spedizione_ID=@id";
                var cmd = new SqlCommand(query, Db.conn);
                cmd.Parameters.AddWithValue("@id", id);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    if (reader.HasRows)
                    {

                        Aggiornamenti aggiornamento = new Aggiornamenti
                        {
                            Aggiornamento_ID = Convert.ToInt32(reader["Aggiornamento_ID"]),
                            Stato = reader["Stato"].ToString(),
                            Luogo = reader["Luogo"].ToString(),
                            Descrizione = reader["Descrizione"].ToString(),
                            Data_Agg = (DateTime)reader["Data_Agg"],
                            Spedizione_ID = Convert.ToInt32(reader["Spedizione_ID"]),
                        };
                        aggiornamenti.Add(aggiornamento);
                    }
                }
                reader.Close();
                return View(aggiornamenti);
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
            finally
            { 
                Db.conn.Close(); 
            }
        }

        [HttpGet]
        public ActionResult StatoOrdini()
        {
            return View();
        }

        [HttpPost]
        public ActionResult StatoOrdini(int id)
        {
            return RedirectToAction("InserimentoStato", new { id = id });
        }


        [HttpGet]
        public ActionResult InserimentoStato(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("StatoOrdini");  
            }

            ViewBag.IdOrdine = id;  
            return View();
        }

        [HttpPost]
        public ActionResult InserimentoStato(Aggiornamenti aggiornamenti, int id)
        {
            try
            {
                Db.conn.Open();

                var command = new SqlCommand(@"INSERT INTO Aggiornamenti
                               (Stato, Luogo, Descrizione, Data_Agg, Spedizione_ID)
                               VALUES (@stato, @luogo, @descrizione, @data_agg, @sped_id)", Db.conn);

                command.Parameters.AddWithValue("@stato", aggiornamenti.Stato);
                command.Parameters.AddWithValue("@luogo", aggiornamenti.Luogo);
                command.Parameters.AddWithValue("@descrizione", aggiornamenti.Descrizione);
                command.Parameters.AddWithValue("@data_agg", aggiornamenti.Data_Agg);
                command.Parameters.AddWithValue("@sped_id", id);

                command.ExecuteNonQuery();

                if (id != 0)
                {
                    return RedirectToAction("DettaglioOrdine", "Home", new { id = id });
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Si è verificato un errore durante l'aggiunta dell'aggiornamento.";
                Console.WriteLine(ex.ToString());
                return View(aggiornamenti);
            }
            finally
            {
                Db.conn.Close();
            }
        }





        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}