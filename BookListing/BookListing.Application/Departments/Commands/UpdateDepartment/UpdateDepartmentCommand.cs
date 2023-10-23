using BookListing.Application.Common.Exceptions;
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

namespace BookListing.Application.Departments.Commands.UpdateDepartment
{
    [Authorize(Roles = "Admin")]
    [Authorize(Policy = "DepartmentsManagement")]
    public class UpdateDepartmentCommand : IRequest
    {
        [Required(ErrorMessage = "DepartmentId is required.")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }


    }

    public class UpdateTodoListCommandHandler : IRequestHandler<UpdateDepartmentCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateTodoListCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
        {

            var entity = await _context.Departments
                .FindAsync(new object[] { request.Id }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Department), request.Id);
            }


            entity.Name = request.Name;



            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
