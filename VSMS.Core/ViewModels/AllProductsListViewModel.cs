namespace VSMS.Core.ViewModels
{
    public class AllProductsListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public int Kilograms { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
