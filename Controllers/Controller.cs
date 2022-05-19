using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecuLink.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Controller : ControllerBase
    {
        Models.User_Methods userControl = new();
        //Models.Card_Methods cardControl = new();

        // POST: /Requests
        /*
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public string Requests()
        {
            HttpRequest request = HttpContext.Request;
            var req = JsonConvert.DeserializeObject<Models.Request>(request.Form["request"]);
        }
        */

        // POST: /Mailbox_Users
        [HttpPost("~/Mailbox_Users")]
        //[ValidateAntiForgeryToken]
        public string Mailbox_Users()
        {
            
            HttpRequest request = HttpContext.Request;
            var userreq = JsonConvert.DeserializeObject<Models.Users_Req>(request.Form["usersForm"]);
            if (userreq.Username == "&&&&&&&&&" || userreq.Password_Enc == "%%%%%%%%%%" || userreq.CardId == 0)
            {
                Response_Send(200,"ERROR:0");
                Models.Response r_t = new(201, "Response_Send");
                JavaScriptSerializer jss_t = new();

                string json_strt = jss_t.Serialize(r_t);

                return json_strt;
            }
            userreq.RequestHandling(userreq.Id, userreq.Username, userreq.Password_Enc, userreq.CardId, userreq.Request);

            Models.Response r = new(201, "Response_Send");
            JavaScriptSerializer jss = new();

            string json_str = jss.Serialize(r);

            return json_str;
        }

        // POST: /Mailbox_Cards
        [HttpPost("~/Mailbox_Cards")]
        //[ValidateAntiForgeryToken]
        public string Mailbox_Cards()
        {
            HttpRequest request = HttpContext.Request;
            var cardreq = JsonConvert.DeserializeObject<Models.Cards_Req>(request.Form["cardsForm"]);
            if (cardreq.SerialNumber == "&&&&&&&&&" ||  cardreq.Pin == -1)
            {
                Models.Response r_t = new(201, "Response_Send");
                JavaScriptSerializer jss_t = new();

                string json_strt = jss_t.Serialize(r_t);

                return json_strt;
            }
            cardreq.RequestHandling(cardreq.Id, cardreq.SerialNumber, cardreq.Pin, cardreq.Request);
            Models.Response r = new(201, "Response_Send");
            JavaScriptSerializer jss = new();

            string json_str = jss.Serialize(r);

            return json_str;
        }

        // GET: /Response_Send
        [HttpGet("~/Response_Send")]
        public string Response_Send(int id,string resp)
        {
            // ERROR - Zahtjev nije prošao kako treba
            //  0 - Traženi objekt ne postoji
            //  1 - Traženi zahtjev ne postoji
            // DONE - Zahtjev podnešen, nema poslanih objekata
            // EXISTS - Zahtjev podnešen, traženi objekt postoji
            // SENT - Zahtjev podnešen, objekt poslan na /Object_Send

            Models.Response r = new(id, resp);
            JavaScriptSerializer jss = new();

            string json_str = jss.Serialize(r);

            return json_str;
        }
        // GET: /Object_Send
        [HttpGet("~/Object_Send")]
        public string Object_Send(object obj)
        {
            JavaScriptSerializer jss = new();

            string json_str = jss.Serialize(obj);

            return json_str;
        }
    }
}
