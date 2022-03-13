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
    [Route("api/compensation")]
    public class CompensationController : Controller
    {
        private readonly ILogger _logger;
        private readonly ICompensationService _compensationService;
        private readonly IEmployeeService _employeeService;

        public CompensationController(ILogger<CompensationController> logger, ICompensationService compensationService, IEmployeeService employeeService)
        {
            _logger = logger;
            _compensationService = compensationService;
            _employeeService = employeeService;
        }

        /// <summary>
        /// Auto generated Date
        /// Data annotations to force salary to be greater than 1, and have max of 2 decimals
        /// Checks to ensure that the employee ID foreign key maps to an existing employee
        /// CAlls comp service to create new comp
        /// Data persists by writing to Json file
        /// </summary>
        /// <param name="compensation"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateCompensation([FromBody] Compensation compensation)
        {
            _logger.LogDebug($"Received compensation create request for employee Id: '{compensation}'");
            _logger.LogDebug($"Received compensation create request for compensation Id: '{compensation}'");

            var employee = _employeeService.GetById(compensation.EmployeeId);
            if(employee.EmployeeId != compensation.EmployeeId)
            {
                _logger.LogDebug($"There is no such employee Id: '{compensation}'. Provide valid ID");
                return null;
            }

            _compensationService.Create(compensation);

            return CreatedAtRoute("getCompByEmpId", new { id = compensation }, compensation);
        }

        /// <summary>
        /// In takes an integer for the primary key and returns compensation object
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "getCompByEmpId")]
        public IActionResult GetCompByEmpId(int id)
        {
            _logger.LogDebug($"Received compensation get request for employee id: '{id}'");

            var compensation = _compensationService.GetById(id);

            if (compensation == null)
                return NotFound();

            return Ok(compensation);
        }
    }
}
