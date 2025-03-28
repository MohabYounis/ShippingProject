﻿using System.ComponentModel.DataAnnotations;

namespace Shipping.DTOs.Employee
{
    public class CreateEmployeeDTO
    {
        // app user fields
        public string Name { get; set; }
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        // branch fields
        public int branchId { get; set; }
    }
}
