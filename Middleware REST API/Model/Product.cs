namespace Middleware_REST_API.Model
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public List<string> Images { get; set; }
        public string Category { get; set; }

        public Product()
        {
            Images = new List<string>();
        }
    }
}
