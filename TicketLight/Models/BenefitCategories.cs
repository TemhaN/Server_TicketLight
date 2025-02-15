using System.ComponentModel.DataAnnotations;

namespace TicketLightAPI.Models
{
    public class BenefitCategory
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        public string CategoryName { get; set; }

        public string Description { get; set; }
    }
}
