
namespace CleanArchitecture.Domain.Employees
{
    #region Usings
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CleanArchitecture.Domain.Abstraction;

    #endregion
    public sealed class Employee : Entity
    {
        
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;

        public string FullName => string.Join(" ", FirstName, LastName);

        public DateOnly BirthOfDate { get; set; }
        public decimal Salary { get; set; }

        // Value Object
        public PersonalInformation Information { get; set; } = default!;
        // Value Object
        public Address Address { get; set; } = default!;

    }
}
