using BookListing.Application.Common.Security;
using BookListing.Application.UserAccess.Commands.CreateUserAccess;
using BookListing.Application.UserAccess.Commands.DeleteUserAccess;
using BookListing.Application.UserAccess.Queries;
using Microsoft.AspNetCore.Mvc;

namespace BookListing.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserAccess : ApiControllerBase
    {       
        [HttpPost]
        public async Task<ActionResult<UserAccessDto>> Create(CreateUserAccessCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await Mediator.Send(new DeleteUserAccessCommand { Id = id });

            return NoContent();
        }
    }
}
