using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCCRUDAJAX.Models
{
    public class Employee
    {
        public int EmpId { get; set; }
        [Required]
        [DisplayName("Employee Name")]
        public string EmployeeName { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Salary must be numeric")]
        public string Salary { get; set; }
    }
}