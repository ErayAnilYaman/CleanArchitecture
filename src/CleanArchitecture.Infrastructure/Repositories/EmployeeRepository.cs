using CleanArchitecture.Domain.Employees;
using CleanArchitecture.Infrastructure.Context;
using GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Repositories
{
    internal sealed class EmployeeRepository : Repository<Employee , ApplicationDbContext> , IEmployeeRepository
    {

        public EmployeeRepository(ApplicationDbContext db) : base(db)
        {
            
        }
    }
}
