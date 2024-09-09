using HarmoniX_Repository.Models;
using Microsoft.EntityFrameworkCore;

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
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }
    }
}
