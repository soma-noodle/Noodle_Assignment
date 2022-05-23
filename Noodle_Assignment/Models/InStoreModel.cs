namespace Noodle_Assignment.Models
{
    public class InStoreModel
    {
        public string CustomerId { get; set; }
        public string StoreId { get; set; }
        public LineItemModel LineItemModel { get; set; }
    }
}
