using AutoMapper;
using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Contexts;
using ServiceLayer.Dtos.Contact;
using ServiceLayer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public class ContactService : IContactService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ContactService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<List<ContactDto>> GetAllAsync()
        {
            List<Contact> list = await _context.Contacts.ToListAsync();

            return _mapper.Map<List<ContactDto>>(list);
        }


        public async Task<ContactDto> GetByIdAsync(int id)
        {
            Contact entity = await _context.Contacts.FindAsync(id);

            return _mapper.Map<ContactDto>(entity);
        }


        public async Task CreateAsync(ContactCreateDto dto)
        {
            Contact entity = _mapper.Map<Contact>(dto);

            await _context.Contacts.AddAsync(entity);
            await _context.SaveChangesAsync();
        }


        public async Task RemoveAsync(int id)
        {
            Contact entity = await _context.Contacts.FindAsync(id);

            if (entity != null)
            {
                _context.Contacts.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
