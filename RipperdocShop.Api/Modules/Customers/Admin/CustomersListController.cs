using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RipperdocShop.Api.Modules.Customers.Queries;

namespace RipperdocShop.Api.Modules.Customers.Admin;

[Route("api/admin/customers")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
public class CustomersListController(ListAdminCustomersQuery listAdminCustomers) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] bool includeDeleted = false, [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        return Ok(await listAdminCustomers.ExecuteAsync(includeDeleted, page, pageSize));
    }
}
