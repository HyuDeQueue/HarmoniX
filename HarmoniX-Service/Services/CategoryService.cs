using HarmoniX_Repository.Models;
using HarmoniX_Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarmoniX_Service.Services
{
    public class CategoryService
    {
        private CategoryRepository _repo = new();

        public async Task<List<Category>> GetAllCategories()
        {
            return await _repo.GetAll();
        }

        public async Task AddCategory(Category category) 
            => await _repo.AddCategory(category);
    }
}
