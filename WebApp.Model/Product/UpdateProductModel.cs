namespace WebApp.Model.Product
{
    public class UpdateProductModel
    {
        public string Name { get; set; } = string.Empty;
        public float Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Categorie { get; set; } = string.Empty;
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
