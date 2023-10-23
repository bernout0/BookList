using BookListing.Application.Common.Security;
using BookListing.Application.UserAccess.Queries;
using Microsoft.AspNetCore.Mvc;

namespace BookListing.Web.Controllers
{
    [Authorize(Roles="Admin")]
    public class UsersController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<UserDTO>>> Get()
        {
            return await Mediator.Send(new GetUsersQuery());
        }

    }
}
