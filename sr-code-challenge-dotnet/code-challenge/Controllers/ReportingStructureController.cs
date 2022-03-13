using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using challenge.Services;
using challenge.Models;
using System.Net.Http;

namespace challenge.Controllers
{
    [ApiController]
    [Route("api/ReportingStructure")]
    public class ReportingStructureController : Controller
    {
        private readonly ILogger _logger;
        private readonly IEmployeeService _employeeService;

        public ReportingStructureController(ILogger<ReportingStructureController> logger, IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        [HttpGet("{id}", Name = "GetReportingStructureByEmpId")]
        public IActionResult GetReportingStructureByEmpId(string id)
        {
            _logger.LogDebug($"Received ReportingStructure get request for employee id: '{id}'");

            var rs = _employeeService.GetRSById(id);

            if (rs == null)
                return NotFound();

            return Ok(rs);
        }


    }
}
