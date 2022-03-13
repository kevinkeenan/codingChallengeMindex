using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using challenge.Data;
using System.Threading;

namespace challenge.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeContext _employeeContext;
        private readonly ILogger<IEmployeeRepository> _logger;

        public EmployeeRepository(ILogger<IEmployeeRepository> logger, EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
            _logger = logger;
        }

        public Employee Add(Employee employee)
        {
            employee.EmployeeId = Guid.NewGuid().ToString();
            _employeeContext.Employees.Add(employee);
            return employee;
        }

        /// <summary>
        /// There is a rather strange bug that returns null direct reports when i run regularly but not when line by line
        /// NOTE: Fixed by querying for just directReports
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Employee GetById(string id)
        {
            _logger.LogDebug("Getting Id of: " + id);
            var emp = _employeeContext.Employees.SingleOrDefault(e => e.EmployeeId == id);
            if(emp == null)
            {
                return null;
            }
            var query = (from e in _employeeContext.Employees
                                        where e.EmployeeId == id
                                        select e.DirectReports).SingleOrDefault();
            emp.DirectReports = query;
            return emp;
        }

        /// <summary>
        /// inputs an employee id and returns a ReportingStruct
        /// calculated on the fly
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ReportingStructure getRSById(String id)
        {
            ReportingStructure rs = new ReportingStructure();
            rs.EmployeeId = id;
            rs.NumberOfReports = RecursiveReportCount(id);
            return rs;
        }

        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }

        public Employee Remove(Employee employee)
        {
            return _employeeContext.Remove(employee).Entity;
        }

        /// <summary>
        /// Recursive method to traverse down the structure and return total number of direct reports
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int RecursiveReportCount(string id)
        {
            var emp = GetById(id);
            if(emp == null)
            {
                _logger.LogDebug("No such employee exists");
                return -1;
            }
            if (emp.DirectReports.Count == 0)
            {
                _logger.LogDebug("Employee has no underlings (leaf node)");
                return 0;
            }
            else
            {
                //get the ids of those in direct reports and return RecursiveReportCount(1st) + RecursiveReportCount(2nd) etc
                var numReports = emp.DirectReports.Count;
                foreach (Employee e in emp.DirectReports)
                {
                    numReports += RecursiveReportCount(e.EmployeeId);
                }
                return numReports;
            }

        }
    }
}
