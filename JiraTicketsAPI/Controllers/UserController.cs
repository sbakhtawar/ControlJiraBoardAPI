using JiraTicketsAPI.Models.Tickets;
using JiraTicketsAPI.Models.User;
using JiraTicketsAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace JiraTicketsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [Route("GetAllUsers")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            HttpClient client = new HttpClient();

            string mergedCredentials = String.Format("{0}:{1}", JiraInfo.userName, JiraInfo.apiToken);

            var authData = System.Text.Encoding.UTF8.GetBytes(mergedCredentials);
            var basicAuth = Convert.ToBase64String(authData);

            Uri jiraURI = new Uri(JiraInfo.jiraOrg);
            client.BaseAddress = jiraURI;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basicAuth);

            var result = await client.GetAsync("/rest/api/3/users/search");
            if (result.IsSuccessStatusCode)
            { 
                string apiResponse = await result.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<List<JiraUserResponse>>(apiResponse);
                return Ok(response);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
