using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectronicsStoreModel
{
    public class Client
    {
        public int Id { get; set; }

        [Required]
        public string ClientFIO { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public DateTime DateRegistration { get; set; }

        public int Bonus { get; set; }

        public bool ClientStatus { get; set; }

        [ForeignKey("ClientId")]
        public virtual List<Order> Orders { get; set; }
    }
}
