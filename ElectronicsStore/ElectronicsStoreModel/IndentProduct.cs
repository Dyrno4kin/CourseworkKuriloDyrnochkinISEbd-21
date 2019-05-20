

namespace ElectronicsStoreModel
{
    public class IndentProduct
    {
        public int Id { get; set; }

        public int IndentId { get; set; }

        public int ProductId { get; set; }

        public int Count { get; set; }

        public virtual Indent Indent { get; set; }

        public virtual Product Product { get; set; }
    }
}
