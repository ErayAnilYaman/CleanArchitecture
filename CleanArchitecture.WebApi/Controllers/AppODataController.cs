using CleanArchitecture.Application.Employees;
using CleanArchitecture.Domain.Employees;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace CleanArchitecture.WebApi.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("odata")]
    [ApiController]
    [EnableQuery(PageSize = 50, MaxTop = 100, MaxExpansionDepth = 2)] // Odata daki filter skip gibi ozellikleri calistirabilmek icin

    public class AppODataController(ISender sender) : ODataController
    {

        public static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new ();
            builder.EnableLowerCamelCase();
            builder.EntitySet<EmployeeGetAllQueryResponse>("employees");
            return builder.GetEdmModel();
        }

        [HttpGet("employees")]
        public async Task<IActionResult> GetAllEmployees(CancellationToken cancellationToken)
        {
            try
            {
                var response = await sender.Send(new EmployeeGetAllQuery(), cancellationToken);
                return Ok(response);


            }
            catch (Exception ex)
            {
                return Problem("Beklenmeyen bir hata olustu!" + " " + ex.Message);
            }
        }

    }
}
