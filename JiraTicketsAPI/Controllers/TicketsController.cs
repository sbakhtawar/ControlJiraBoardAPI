using JiraTicketsAPI.Models.Tickets;
using JiraTicketsAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace JiraTicketsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {

        [Route("CreateTaskTicket")]
        [HttpPost]
        public async Task<IActionResult> CreateTaskTicket(string Summary, string Description)
        {
            HttpClient client = new HttpClient();

            string mergedCredentials = String.Format("{0}:{1}",JiraInfo.userName,JiraInfo.apiToken);

            var authData= System.Text.Encoding.UTF8.GetBytes(mergedCredentials);
            var basicAuth= Convert.ToBase64String(authData);

            Uri jiraURI= new Uri(JiraInfo.jiraOrg);
            client.BaseAddress = jiraURI;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basicAuth);
            var data = new
            {
                fields = new
                {
                    issuetype= new { id= 10001},
                    summary= Summary,
                    project= new { key= "TES" },
                    description = new
                    {
                        version = 1,
                        type = "doc",
                        content = new[] {
                        new {
                            type = "paragraph",
                            content = new []{
                                new {
                                    type = "text",
                                    text = Description
                                }
                            }
                        }
                    }
                  }
                }
            };
            var result= await client.PostAsJsonAsync("/rest/api/3/issue", data);
            if(result.StatusCode == System.Net.HttpStatusCode.Created) 
            {
                    var response= await result.Content.ReadFromJsonAsync<JiraResponse>();
                    return Ok(response);
            }
            else
            {
                return NotFound();
            }
        }

        [Route("EditTaskTicket")]
        [HttpPut]
        public async Task<IActionResult> EditJiraticket(string Key, string Summary,string Description)
        {
            HttpClient client = new HttpClient();

            string mergedCredentials = String.Format("{0}:{1}", JiraInfo.userName, JiraInfo.apiToken);

            var authData = System.Text.Encoding.UTF8.GetBytes(mergedCredentials);
            var basicAuth = Convert.ToBase64String(authData);

            Uri jiraURI = new Uri(JiraInfo.jiraOrg);
            client.BaseAddress = jiraURI;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basicAuth);
            var data = new
            {
                update = new
                {
                    summary = new[]
                    {
                        new {
                            set= Summary
                        }
                    }
                },
                fields = new
                {
                        description = new
                        {
                            version = 1,
                            type = "doc",
                            content = new[] {
                            new {
                                type = "paragraph",
                                content = new []{
                                    new {
                                        type = "text",
                                        text = Description
                                    }
                                }
                            }
                        }
                  }
              }
            };
            var dataJson = JsonConvert.SerializeObject(data);
            var path = "/rest/api/3/issue/" + Key;
            var result = await client.PutAsJsonAsync(path, data);
            if (result.IsSuccessStatusCode)
            {
                return Ok("Ticket Issue:" + Key + " was edited with summary " + Summary +"and description "+ Description);
            }
            else
            {
                return NotFound();
            }
        }

        [Route("AssignTaskTicket")]
        [HttpPut]
        public async Task<IActionResult> AssignJiraticket(string Key, string accountID)
        {
            HttpClient client = new HttpClient();

            string mergedCredentials = String.Format("{0}:{1}", JiraInfo.userName, JiraInfo.apiToken);

            var authData = System.Text.Encoding.UTF8.GetBytes(mergedCredentials);
            var basicAuth = Convert.ToBase64String(authData);

            Uri jiraURI = new Uri(JiraInfo.jiraOrg);
            client.BaseAddress = jiraURI;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basicAuth);

            var data = new
            {
                accountId = accountID,
            };

            var dataJson = JsonConvert.SerializeObject(data);
            var path = "/rest/api/3/issue/" + Key+"/assignee";
            var result = await client.PutAsJsonAsync(path, data);
            if (result.IsSuccessStatusCode)
            {
                return Ok("Ticket Issue:" + Key + " was edited wasassigned succesdfully");
            }
            else
            {
                return NotFound();
            }
        }

        [Route("DeleteTaskTicket")]
        [HttpDelete]
        public async Task<IActionResult> DeleteJiraticket(string Key)
        {
            HttpClient client = new HttpClient();

            string mergedCredentials = String.Format("{0}:{1}", JiraInfo.userName, JiraInfo.apiToken);

            var authData = System.Text.Encoding.UTF8.GetBytes(mergedCredentials);
            var basicAuth = Convert.ToBase64String(authData);

            Uri jiraURI = new Uri(JiraInfo.jiraOrg);
            client.BaseAddress = jiraURI;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basicAuth);
          
            var result = await client.DeleteAsync("/rest/api/3/issue/" + Key);
            if (result.IsSuccessStatusCode)
            {
                return Ok("Ticket Issue:"+Key+" is deleted !");
            }
            else
            {
                return NotFound();
            }
        }

        [Route("GetEditIssueMetaData")]
        [HttpGet]
        public async Task<IActionResult> GetEditIssueMetaData(string Key)
        {
            HttpClient client = new HttpClient();

            string mergedCredentials = String.Format("{0}:{1}", JiraInfo.userName, JiraInfo.apiToken);

            var authData = System.Text.Encoding.UTF8.GetBytes(mergedCredentials);
            var basicAuth = Convert.ToBase64String(authData);

            Uri jiraURI = new Uri(JiraInfo.jiraOrg);
            client.BaseAddress = jiraURI;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basicAuth);
            
            var result = await client.GetAsync("/rest/api/3/issue/"+Key+"/editmeta");
            if (result.IsSuccessStatusCode)
            {
                var response = await result.Content.ReadFromJsonAsync<MetaDataResponse>();
                return Ok(response);
            }
            else
            {
                return NotFound();
            }
        }
        [Route("GetIssueData")]
        [HttpGet]
        public async Task<IActionResult> GetIssueData(string Key)
        {
            HttpClient client = new HttpClient();

            string mergedCredentials = String.Format("{0}:{1}", JiraInfo.userName, JiraInfo.apiToken);

            var authData = System.Text.Encoding.UTF8.GetBytes(mergedCredentials);
            var basicAuth = Convert.ToBase64String(authData);

            Uri jiraURI = new Uri(JiraInfo.jiraOrg);
            client.BaseAddress = jiraURI;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basicAuth);

            var result = await client.GetAsync("/rest/api/3/issue/" + Key);
            if (result.IsSuccessStatusCode)
            {
                //var response = await result.Content.ReadFromJsonAsync<IssueDetailsResponse>();
                string apiResponse = await result.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<IssueDetailsResponse>(apiResponse);
                var test = response;
                return Ok(response);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
