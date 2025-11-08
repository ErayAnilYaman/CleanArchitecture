using CleanArchitecture.Domain.Employees;
using FluentValidation;
using GenericRepository;
using Mapster;
using MediatR;
using TS.Result;

public sealed record EmployeeCreateCommand(
    string FirstName,
    string LastName,
    DateOnly BirtOfDate,
    decimal Salary,
    PersonalInformation Information,
    Address? Address,
    bool IsActive

    ) : IRequest<Result<string>>;


public sealed class EmployeeCreateCommandValidator : AbstractValidator<EmployeeCreateCommand>
{
    public EmployeeCreateCommandValidator()
    {
        RuleFor(x => x.FirstName).MinimumLength(3).WithMessage("Ad Alani en az 3 karakter olmalidir.");
        RuleFor(x => x.LastName).MinimumLength(3).WithMessage("Soyad Alani en az 3 karakter olmalidir.");
        RuleFor(x => x.Information.TCNo).Must(y => y.Length == 11).WithMessage("Gecerli bir TC numarasi giriniz");



    }
}
internal sealed class EmployeeCreateCommandHandler
    (IEmployeeRepository employeeRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<EmployeeCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(EmployeeCreateCommand request, CancellationToken cancellationToken)
    {

        var isEmployeeExists =
            await employeeRepository.AnyAsync(p => p.Information.TCNo == request.Information.TCNo, cancellationToken);


        if (isEmployeeExists)
        {
            return Result<string>.Failure("Bu tc numarasi daha once kaydedilmis");
        }

        Employee employee = request.Adapt<Employee>();

        employeeRepository.Add(employee);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Personel kaydi basariyla tamamlandi .");
    }
}
