using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectronicsStoreModel
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        public string CustomerFIO { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public DateTime DateRegistration { get; set; }
        [Required]
        public int Bonus { get; set; }
        [Required]
        public bool CustomerStatus { get; set; }

        [ForeignKey("CustomerId")]
        public virtual List<Indent> Indents { get; set; }
    }
}
