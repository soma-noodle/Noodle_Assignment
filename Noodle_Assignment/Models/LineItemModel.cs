namespace Noodle_Assignment.Models
{
    public class LineItemModel
    {
        public string ProductId { get; set; }
        public long? VariantId { get; set; }
        public string Currency { get; set; }
        public decimal Price { get; set; }

    }
}
