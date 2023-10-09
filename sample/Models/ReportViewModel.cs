using System.ComponentModel.DataAnnotations;

namespace sample.Models
{
    public class ReportViewModel
    {
        [Required]
        public DateTime FromDate { get; set; }

        [Required]
        public DateTime ToDate { get; set; }
    }
}
