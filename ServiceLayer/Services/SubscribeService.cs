using AutoMapper;
using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Contexts;
using ServiceLayer.Dtos.Subscribe;
using ServiceLayer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public class SubscribeService : ISubscribeService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public SubscribeService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<List<SubscribeDto>> GetAllAsync()
        {
            List<Subscribe> list = await _context.Subscribes.AsNoTracking().ToListAsync();

            return _mapper.Map<List<SubscribeDto>>(list);
        }


        public async Task<SubscribeDto> GetByIdAsync(int id)
        {
            Subscribe? entity = await _context.Subscribes.AsNoTracking().SingleOrDefaultAsync(x=> x.Id == id);

            return _mapper.Map<SubscribeDto>(entity);
        }


        public async Task CreateAsync(SubscribeCreateDto dto)
        {
            Subscribe? entity = _mapper.Map<Subscribe>(dto);

            await _context.Subscribes.AddAsync(entity);
            await _context.SaveChangesAsync();
        }


        public async Task RemoveAsync(int id)
        {
            Subscribe? entity = await _context.Subscribes.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);

            if (entity != null)
            {
                _context.Subscribes.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
