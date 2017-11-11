using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebT.Models
{
    public class User
    {
        public User()
        {
            Orders = new List<Order>();
        }

        public Int32 Id { get; set; }
        [Required, MaxLength(80), Display(Name = "Имя")]
        public String FirstName { get; set; }
        [MaxLength(80), Display(Name = "Отчество")]
        public String MiddleName { get; set; }
        [Required, MaxLength(80), Display(Name = "Фамилия")]
        public String LastName { get; set; }
        [Required, Display(Name = "Тип клиента")]
        public UserTypeEnum ClientType { get; set; }
        [Required, Display(Name = "Общая сумма"), DataType(DataType.Currency)]
        public decimal TotalSum { get; set; }
        public ICollection<Order> Orders { get; set; }

    }
}