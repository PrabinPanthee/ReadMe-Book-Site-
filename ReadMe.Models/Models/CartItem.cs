using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadMe.Models.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }  // Primary Key

        [Required]
        public int CartId { get; set; }  // Foreign Key

        [ForeignKey(nameof(CartId))]
        public virtual Cart Cart { get; set; }  // Navigation Property

        [Required]
        public int ProductId { get; set; }  // Foreign Key

        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; }  // Navigation Property

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }  // Quantity of the product in the cart

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
        public double Price { get; set; }  // Price of the product at the time of adding to the cart
    }
}

