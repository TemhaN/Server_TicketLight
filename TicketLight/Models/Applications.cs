using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketLightAPI.Models
{
    public class Application
    {
        [Key]
        public int ApplicationId { get; set; }

        public DateTime? ApprovalDate { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public string Status { get; set; }

        public DateTime SubmissionDate { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [ForeignKey("CategoryId")]
        public BenefitCategory Category { get; set; }
    }
}
