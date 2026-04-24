namespace api.Data.DTOs
{
    public record ProductDto
    {
        public int Id { get; set; }

        public required string Title { get; set; }

        public string? Description { get; set; }

        public required string Image { get; init; }
    }

    public record ProductCreateDto
    {
        public required string Title { get; set; }

        public string? Description { get; set; }

        public required string Image { get; init; }
    }
}
