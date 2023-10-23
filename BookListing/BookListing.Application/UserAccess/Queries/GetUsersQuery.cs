using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookListing.Application.Categories.Queries;
using BookListing.Application.Common.Interfaces;
using BookListing.Application.Common.Security;
using BookListing.Application.Departments.Queries;
using BookListing.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookListing.Application.UserAccess.Queries
{
    [Authorize(Roles = "Admin")]
    [Authorize(Policy = "UserManagement")]
    public class GetUsersQuery : IRequest<List<UserDTO>>
    {
    }
    public class GeUsersQueryHandler : IRequestHandler<GetUsersQuery, List<UserDTO>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public GeUsersQueryHandler(IApplicationDbContext context, IMapper mapper, UserManager<User> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<List<UserDTO>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userManager.Users
               .Include(u => u.UserAccesses).ThenInclude(uc => uc.Category)
               .Include(u => u.UserAccesses).ThenInclude(ud => ud.Department)
               .Where(i=> !i.UserName.Equals("admin@localhost"))//workaround to not include admin
               .Select(u => new UserDTO
               {
                   Id = new Guid(u.Id),
                   UserName = u.UserName,
                   UserAccesses = u.UserAccesses.Select(u => new UserAccessDto
                   {
                       Id = u.Id,
                       Category = _mapper.Map<CategoryDTO>( u.Category),
                       Department = _mapper.Map<DepartmentDTO>(u.Department)
                   }).ToList()
               })
               .ToListAsync();

            return users;

        }
    }
}
