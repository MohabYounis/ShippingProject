﻿using System.ComponentModel.DataAnnotations;

namespace Shipping.DTOs.Employee
{
    public class UpdateEmployeeDTO
    {

            // app user fields
            public string Name { get; set; }
            public string Email { get; set; }     
            public string Phone { get; set; }
            public string Address { get; set; }

            public string Role { get; set; }

            // branch fields
            public int branchId { get; set; }

    
    }
}
