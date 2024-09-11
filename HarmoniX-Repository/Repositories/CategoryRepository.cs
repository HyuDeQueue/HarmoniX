using HarmoniX_Repository.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarmoniX_Repository.Repositories
{
    public class CategoryRepository
    {
        private HarmonixDbContext _context;

        public async Task<List<Category>> GetAll()
        {
            _context = new();
            return await _context.Categories.ToListAsync();
        }

        public async Task AddCategory(Category category)
        {
            _context = new();
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }
    }
}
