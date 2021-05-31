using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PizzaOrder.Models
{
    public class OrderDetails
    {
        [Key]
        public int OrderId { get; set; }

        public int PizzaId { get; set; }
        [ForeignKey("PizzaId")]
        //public  Pizza Pizza { get; set; }
        
        public string CustomerName { get; set; }
        [Required]
        [StringLength(10)]
        public String PhoneNumber { get; set; }
        [Required]
        
        public string Crust { get; set; }
        [Required]
        public int quantity { get; set; }
        [Required]
        public string Toppings { get; set; }
        [Required]
        public DateTime DeliveryDate { get; set; }
        [Required]

        public float Amount { get; set; }



    }

}