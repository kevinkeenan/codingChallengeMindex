using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.Models
{
    public class ReportingStructure
    {
        [Key]
        public String EmployeeId { get; set; }
        public int NumberOfReports { get; set; }
    }
}
