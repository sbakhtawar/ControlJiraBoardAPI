namespace JiraTicketsAPI.Models.Tickets
{
    public class MetaDataResponse
    {
        public Fields fields { get; set; }

    }
    public class Fields
    {
        public Summary summary { get; set; }

    }
    public class Summary
    {
        public bool required { get; set; }
        public Schema schema { get; set; }
        public string name { get; set; }
        public string key { get; set; }
        public bool hasDefaultValue { get; set; }
        public List<string> operations { get; set; }
        public List<string> allowedValues { get; set; }
        public string defaultValue { get; set; }
    }
    public class Schema
    {
        public string type { get; set; }
        public string items { get; set; }
        public string custom { get; set; }
        public int customId { get; set; }
    }


}
