using formularioWEECLAIMS.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace formularioWEECLAIMS.Controllers
{
    public class HomeController : Controller
    {
        static HttpClient client = new HttpClient();
        Database1Entities db = new Database1Entities();
        public ActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> ConsultaCedula(string cedula)
        {
            var path = "https://cedulaprofesional.sep.gob.mx/cedula/buscaCedulaJson.action?json=%7B%27maxResult%27:%27100%27,%27nombre%27:%27%27,%27paterno%27:%27%27,%27materno%27:%27%27,%27idCedula%27:%27" + cedula + @"%27%7D)";
            HttpResponseMessage response = await client.GetAsync(path);
            string respuesta = null;
            if (response.IsSuccessStatusCode)
            {
                respuesta = await response.Content.ReadAsStringAsync();
            }
            return Json(new { respuesta = respuesta });
        }
        public async Task<JsonResult> RegistraUsuario(string dataUsuario)
        {
            string respuesta = null;
            try
            {
                JObject json = JObject.Parse(dataUsuario);
                Solicitantes_Empleo solicitantes = new Solicitantes_Empleo();
                solicitantes.Compañia = json.GetValue("nombre").ToString();
                solicitantes.Cedula = json.GetValue("cedula").ToString();
                solicitantes.Nombre = json.GetValue("nombreRealcion").ToString();
                solicitantes.Titulo = json.GetValue("titulo").ToString();
                solicitantes.Correo = json.GetValue("email").ToString().Replace("%40", "@");
                solicitantes.Telefono = json.GetValue("telefono").ToString();
                db.Solicitantes_Empleo.Add(solicitantes);
                db.SaveChanges();
                respuesta = "Success";
            }
            catch (Exception e)
            {
                respuesta = e.Message;
            }
            return Json(new { respuesta = respuesta });
        }
        public async Task<JsonResult> Registrados(string dataUsuario)
        {
            return Json(db.Solicitantes_Empleo.ToList());
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