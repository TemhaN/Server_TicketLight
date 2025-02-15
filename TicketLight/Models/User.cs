using System;
using System.ComponentModel.DataAnnotations;

namespace TicketLightAPI.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public string PhoneNumber { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Role { get; set; }
    }
}
