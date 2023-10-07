using System.ComponentModel.DataAnnotations;

namespace sample.Models
{
    public class Asset_Movement
    {
        [Key]
        public int Id { get; set; }
        [Required]

        public int AssetId { get; set; }

        public Asset? Asset { get; set; }
        public string? FromLocation { get; set; }

        public string? ToLocation { get; set; }

        public DateTime MoveDate { get; set; }

        public string? MoveReason { get; set; }

        public string? ResponsibleParty { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
