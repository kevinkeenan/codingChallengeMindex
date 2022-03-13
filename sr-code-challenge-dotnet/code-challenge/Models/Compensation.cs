//https://stackoverflow.com/questions/19811180/best-data-annotation-for-a-decimal18-2
//data annontation reference to ensure salary cant be negative and can have at most 2 decimal places
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.Models
{
    public class Compensation
    {
        [Key]
        public int CompensationId { get; set; }

        [Required]
        [ForeignKey("EmployeeId")]
        public Employee employee { get; set; }

        [Required]
        public string EmployeeId { get; set; }

        [Required]
        [RegularExpression(@"^\d+(\.\d{1,2})?$")]
        [Range(1, 9999999999999999.99)] public double Salary { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime EffectiveDate { get; set; } = DateTime.UtcNow;
    }
}
