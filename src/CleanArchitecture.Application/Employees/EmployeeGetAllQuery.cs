using CleanArchitecture.Domain.Abstraction;
using CleanArchitecture.Domain.Employees;
using CleanArchitecture.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Application.Employees;
public sealed record EmployeeGetAllQuery()
    : IRequest<IQueryable<EmployeeGetAllQueryResponse>>;

public sealed class EmployeeGetAllQueryResponse : EntityDto
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public DateOnly BirthOfDate { get; set; }
    public decimal Salary { get; set; }
    public string TCNo { get; set; } = default!;
}

internal sealed class EmployeeGetAllQueryHandler(IEmployeeRepository employeeRepository, UserManager<AppUser> userManager)
    : IRequestHandler<EmployeeGetAllQuery, IQueryable<EmployeeGetAllQueryResponse>>
{
    public Task<IQueryable<EmployeeGetAllQueryResponse>> Handle(EmployeeGetAllQuery request, CancellationToken cancellationToken)
    {

        var response = (from employee in employeeRepository.GetAll()
                        join create_user in userManager.Users.AsQueryable()
                        on employee.CreatedUserId equals create_user.Id

                        join update_user in userManager.Users.AsQueryable()
                        on employee.UpdatedUserId equals update_user.Id
                        into update_user
                        from update_users in update_user.DefaultIfEmpty()

                        join delete_user in userManager.Users.AsQueryable()
                        on employee.DeletedUserId equals delete_user.Id
                        into delete_user
                        from delete_users in delete_user.DefaultIfEmpty()
                        select new EmployeeGetAllQueryResponse
                        {
                            Id = employee.Id,
                            BirthOfDate = employee.BirthOfDate,
                            CreatedAt = employee.CreatedAt,
                            UpdatedAt = employee.UpdatedAt,
                            DeletedAt = employee.DeletedAt,
                            FirstName = employee.FirstName,
                            LastName = employee.LastName,
                            Salary = employee.Salary,
                            TCNo = employee.Information.TCNo,
                            CreatedUserId = create_user.Id,
                            CreatedUserName =  $"{create_user.FirstName}  {create_user.LastName} ({create_user.Email})",

                            UpdatedUserName = employee.UpdatedUserId == null ? null : $"{update_users.FirstName} {update_users.LastName} ({update_users.Email})",
                            UpdatedUserId = employee.UpdatedUserId,

                            DeletedUserId = employee.DeletedUserId,
                            DeletedUserName = employee.DeletedUserId == null ? null : $"{delete_users.FirstName} {delete_users.LastName} ({delete_users.Email})",
                            
                            IsDeleted = employee.IsDeleted,

                        }).AsQueryable();

        return Task.FromResult(response);
    }
}