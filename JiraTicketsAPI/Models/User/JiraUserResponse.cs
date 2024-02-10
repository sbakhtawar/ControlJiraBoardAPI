using Newtonsoft.Json;

namespace JiraTicketsAPI.Models.User
{
    public class JiraUserResponse
    {
        public string self { get; set; }
        public string key { get; set; }
        public string accountId { get; set; }
        public string accountType { get; set; }
        public string name { get; set; }
        //public AvatarUrls avatarUrls { get; set; }
        public string displayName { get; set; }
        public bool active { get; set; }
    }
    public class AvatarUrls
    {
        [JsonProperty("48x48")]
        public string _48x48 { get; set; }

        [JsonProperty("24x24")]
        public string _24x24 { get; set; }

        [JsonProperty("16x16")]
        public string _16x16 { get; set; }

        [JsonProperty("32x32")]
        public string _32x32 { get; set; }
    }


}
