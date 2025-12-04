using KASHOP.DAL.Data;
using KASHOP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Category Create(Category Request)
        {
            _context.Add(Request);
            _context.SaveChanges();
            return Request;
        }

        public List<Category> GetALL()
        {
            return _context.Categories.ToList();
        }
    }
}
