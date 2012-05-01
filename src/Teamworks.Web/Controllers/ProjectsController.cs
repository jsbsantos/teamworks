using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Teamworks.Web.Controllers {
    public class ProjectsController : Controller {
        [System.Web.Mvc.HttpGet]
        [System.Web.Mvc.ActionName("View")]
        public ActionResult Index(int? id) {
            var url = new Uri(string.Format("{0}://{1}:{2}", Request.Url.Scheme, Request.Url.Host, Request.Url.Port));

            var client = new HttpClient
                         {
                             BaseAddress = url,
                         };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var request = client.GetAsync("/api/projects/" + id);

            while (!request.IsCompleted) {}
            var result = request.Result;
            if (result.IsSuccessStatusCode) {
                var project = JsonConvert.DeserializeObject<Models.Project>(result.Content.ReadAsStringAsync().Result);
                return View(project);
            }
            return null;
        }
    }
}