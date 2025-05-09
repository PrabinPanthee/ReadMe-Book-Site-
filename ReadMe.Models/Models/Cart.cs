using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadMe.Models.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }  // Primary Key

        [Required]
        public string ApplicationUserId { get; set; }  // Foreign Key

        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }  // Navigation Property

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // One-to-Many relationship: A cart can have multiple cart items
        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
