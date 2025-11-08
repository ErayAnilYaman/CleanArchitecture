using CleanArchitecture.Domain.Employees;
using MediatR;
using TS.Result;

namespace CleanArchitecture.Application.Employees;
public sealed record EmployeeGetQuery(Guid Id) : IRequest<Result<Employee>>;

internal sealed class GetEmployeeQueryHandler(IEmployeeRepository employeeRepository)
     : IRequestHandler<EmployeeGetQuery, Result<Employee>>
{
    public async Task<Result<Employee>> Handle(EmployeeGetQuery request, CancellationToken cancellationToken)
    {
        var response = await employeeRepository.FirstOrDefaultAsync(e => e.Id == request.Id , cancellationToken);
        if (response is null)
        {
            return Result<Employee>.Failure("Personel Bulunamadi !");
        }
        return response;

    }
}