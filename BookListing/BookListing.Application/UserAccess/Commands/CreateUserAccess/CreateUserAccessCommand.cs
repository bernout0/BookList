using AutoMapper;
using BookListing.Application.Books.Commands.CreateBook;
using BookListing.Application.Books.Queries;
using BookListing.Application.Common.Interfaces;
using BookListing.Application.Common.Security;
using BookListing.Application.UserAccess.Queries;
using BookListing.Domain.Entities;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace BookListing.Application.UserAccess.Commands.CreateUserAccess
{
    [Authorize(Roles = "Admin")]
    [Authorize(Policy = "UserManagement")]
    public class CreateUserAccessCommand : IRequest<UserAccessDto>
    {
        public string UserId { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? DepartmentId { get; set; }

    }

    public class CreateUserAccessCommandHandler : IRequestHandler<CreateUserAccessCommand, UserAccessDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public CreateUserAccessCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper)
        {
            _context = context;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<UserAccessDto> Handle(CreateUserAccessCommand request, CancellationToken cancellationToken)
        {
            var entity = new BookListing.Domain.Identity.UserAccess()
            {
                UserId = request.UserId,
                CategoryId = request.CategoryId,
                DepartmentId = request.DepartmentId,
            };

            _context.UserAccesses.Add(entity);


            await _context.SaveChangesAsync(cancellationToken);

            var userAccessDto = new UserAccessDto
            {
                Category = new Categories.Queries.CategoryDTO { Id = request.CategoryId != null ? request.CategoryId.Value : Guid.Empty },
                Department = new Departments.Queries.DepartmentDTO { Id = request.DepartmentId != null ? request.DepartmentId.Value : Guid.Empty }
            };

            return userAccessDto;
        }
    }
}
