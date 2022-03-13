using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using challenge.Data;

namespace challenge.Repositories
{
    public class CompensationRepository : ICompensationRepository
    {
        private readonly EmployeeContext _employeeContext;
        private readonly ILogger<ICompensationRepository> _logger;

        public CompensationRepository(ILogger<ICompensationRepository> logger, EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
            _logger = logger;
        }

        /// <summary>
        /// Takes in compensation and calls database context to Create
        /// </summary>
        /// <param name="compensation"></param>
        /// <returns></returns>
        public Compensation Add(Compensation compensation)
        {
            _employeeContext.Compensations.Add(compensation);
            return compensation;
        }

        /// <summary>
        /// Takes in compensation ID primary key (integer - > not GUID) and calls database context to get
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Compensation GetById(int id)
        {
            return _employeeContext.Compensations.SingleOrDefault(c => c.CompensationId == id);
        }

        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }

    }
}
