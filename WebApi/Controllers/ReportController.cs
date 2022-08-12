using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services;

namespace WebApi.Controllers;

[Authorize(Roles = Constants.Role.Admin)]
public class ReportController : BaseController
{
    private readonly IReportService reportService;
    public ReportController(IConfiguration configuration, IReportService reportService) : base(configuration)
    {
        this.reportService = reportService;
    }

    [HttpGet("TotalOrderAmountByCustomer")]
    public IActionResult GetTotalOrderAmountByCustomer()
    {
        var data = reportService.GetTotalOrderAmountByCustomer();
        return Ok(data);
    }

    [HttpGet("MostUsedToppingsForEachDrink")]
    public IActionResult GetMostUsedToppingsForEachDrink()
    {
        var data = reportService.GetMostUsedToppingsForEachDrink();
        return Ok(data);
    }

}
