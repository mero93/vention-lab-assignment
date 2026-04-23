namespace api.Data.Models
{
    public class ProductModel
    {
        public int Id { get; set; }

        public required string Title { get; set; }

        public string? Description { get; set; }

        public required string Image { get; set; }
    }
}
