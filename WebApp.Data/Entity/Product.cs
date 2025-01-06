using System.ComponentModel.DataAnnotations;

namespace WebApp.Data.Entity
{
    public class Product : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [StringLength(50)]
        public string Categorie { get; set; } = string.Empty;
        public float Price { get; set; }
        [Required]
        [StringLength(50)]
        public string? Description { get; set; } = string.Empty;

        [Required]
        public string? Image { get; set; } = string.Empty;
    }
}
