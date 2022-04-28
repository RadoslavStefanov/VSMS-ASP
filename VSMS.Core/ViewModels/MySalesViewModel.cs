namespace VSMS.Core.ViewModels
{
    public class MySalesViewModel
    {
        public string DateTime { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal AtPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string Seller { get; set; }
        public int kgPerPiece { get; set; }
    }
}
