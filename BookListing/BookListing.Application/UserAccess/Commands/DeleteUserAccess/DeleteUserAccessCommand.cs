using BookListing.Application.Common.Exceptions;
using BookListing.Application.Common.Interfaces;
using BookListing.Application.Common.Security;
using BookListing.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookListing.Application.UserAccess.Commands.DeleteUserAccess
{
    [Authorize(Roles = "Admin")]
    [Authorize(Policy = "UserManagement")]
    public class DeleteUserAccessCommand : IRequest
    {
        public Guid Id { get; set; }
    }

    public class DeleteUserAccessCommandHandler : IRequestHandler<DeleteUserAccessCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteUserAccessCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteUserAccessCommand request, CancellationToken cancellationToken)
        {

            var entity = await _context.UserAccesses
                .FindAsync(new object[] { request.Id }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(UserAccess), request.Id);
            }

            _context.UserAccesses.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
