using System.ComponentModel.DataAnnotations;

namespace sample.Models
{
    public class Asset
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }

        public string? Category { get; set; }

        public string? Location { get; set; }

        public string? status { get; set; }

        public DateTime PurchaseDate { get; set; }

        public int PurchaseCost { get; set; }

        public DateTime WarrantyExpiryDate { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
