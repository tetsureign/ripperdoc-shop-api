using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RipperdocShop.Api.Modules.Customers.Admin;

[Route("api/admin/customers")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
public class CustomersListController(IAdminCustomerListService adminCustomerListService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] bool includeDeleted = false, [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var (customers, totalCount, totalPages) = await adminCustomerListService.GetAllAsync(includeDeleted, page, pageSize);
        var response = new AdminCustomerListResponse()
        {
            Customers = customers,
            TotalCount = totalCount,
            TotalPages = totalPages
        };
        return Ok(response);
    }
}
