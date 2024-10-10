using Domain.Models;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Result<Category>> GetAsync(string categoryName);
        Task<Result<List<Category>>> GetAllAsync();
        Task<Result<Category>> CreateAsync(string categoryName);
        Task<Result> DeleteAsync(string categoryName);
    }
}
