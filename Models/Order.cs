using System;
using System.ComponentModel.DataAnnotations;

namespace WebT.Models
{
    public class Order
    {
        [Required]
        public Int32 Id { get; set; }
        [Required, MaxLength(80), Display(Name = "Наименование")]
        public String Name { get; set; }
        [Required, Display(Name = "Сумма"), DataType(DataType.Currency)]
        public decimal Sum { get; set; }

        public User User { get; set; }
    }
}