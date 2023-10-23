using BookListing.Application.Common.Interfaces;
using BookListing.Application.Common.Security;
using BookListing.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookListing.Application.Departments.Commands.CreateDepartment
{
    [Authorize(Roles = "Admin")]
    [Authorize(Policy = "DepartmentsManagement")]
    public class CreateDepartmentCommand : IRequest<Department>
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

    }

    public class CreateDepartmentCommandHandler : IRequestHandler<CreateDepartmentCommand, Department>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public CreateDepartmentCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Department> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
        {
            var entity = new Department()
            {
                Name = request.Name
            };

            _context.Departments.Add(entity);


            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}
