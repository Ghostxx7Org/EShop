namespace EShopEFDataProvider
{
    class ModelComplex
    {
        public int Id { get; set; }
        public int? ImageId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public short Warranty { get; set; }
        //Foreign key's
        public int CategoryId { get; set; }
        public short Availability { get; set; }
        public short Delivery { get; set; }
    }
}
