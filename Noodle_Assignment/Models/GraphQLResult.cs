namespace Noodle_Assignment.Models
{
    public class GraphResultData
    {
        public GraphCustomersResult Customers { get; set; }
    }
    public class GraphCustomersResult
    {
        public int Count { get; set; }
        public List<Customer> Results { get; set; }
    }
}
