using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketLightAPI.Models
{
    public class Ticket
    {
        [Key]
        public int TicketId { get; set; }

        [Required]
        public int ApplicationId { get; set; }

        public string Barcode { get; set; }
        public string QRCode { get; set; }
        public DateTime ExpiryDate { get; set; }

        [ForeignKey("ApplicationId")]
        public Application Application { get; set; }
    }
}
