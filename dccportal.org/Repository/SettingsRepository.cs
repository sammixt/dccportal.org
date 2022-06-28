using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using dccportal.org.Dto;
using dccportal.org.Entities;
using dccportal.org.Interface;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace dccportal.org.Repository
{

    public class SettingsRepository : ISettingsRepository
    {
        private readonly dccportaldbContext _context;
        private readonly IMapper _mapper;
        public SettingsRepository(dccportaldbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<SelectListItem>> GetStates()
        {
            List<SelectListItem> states = new List<SelectListItem>();
            var query = await _context.States.ToListAsync();
            foreach (var item in query)
            {
                states.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Name
                });
            }
            return states;
        }

        public async Task<List<RolesDto>> GetRoles()
        {
            List<RolesDto> roles = new List<RolesDto>();
            var query = await _context.Roles.ToListAsync();
            foreach (var item in query)
            {
                roles.Add(new RolesDto
                {
                    RoleName = item.RoleName,
                    RoleId = item.RoleId,
                    RoleType = item.RoleType
                });
            }
            return roles;
        }

        public async Task<List<PostDto>> GetPost()
        {
            List<PostDto> post = new List<PostDto>();
            var query = await _context.Posts.ToListAsync();
            var postDto = _mapper.Map<List<Post>, List<PostDto>>(query);
            return postDto;
        }
    }
}