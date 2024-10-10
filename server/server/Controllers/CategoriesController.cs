using DataAccessLayer.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() 
        {
            var result = await _categoryRepository.GetAllAsync();
            if (result.IsSuccess)
            {                
                return Ok(result.Value);
            }

            return BadRequest();
        } 
        [HttpGet("{category}")]
        public async Task<IActionResult> GetCategory(string category) 
        {
            var result = await _categoryRepository.GetAsync(category);
            if (result.IsFailed)
            {
                switch (result.Errors[0].Message)
                {
                    case ErrorTypes.NotFound: return NotFound(new { message = "Kategorija nerasta." });
                }
            }
            if (result.IsSuccess)
            {                
                return Ok(result.Value);
            }
            return BadRequest();

        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody]string newCategory)
        {
            var result = await _categoryRepository.CreateAsync(newCategory);
            if (result.IsFailed)
            {
                switch (result.Errors[0].Message)
                {
                    case ErrorTypes.Exists: return Conflict(new { message = "Kategorija jau egzistuoja." });
                }
            }
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            return BadRequest();
        }
        [HttpDelete("{category}")]
        [Authorize]
        public async Task<IActionResult> Delete(string category)
        {
            var result = await _categoryRepository.DeleteAsync(category);
            if (result.IsFailed)
            {
                switch (result.Errors[0].Message)
                {
                    case ErrorTypes.NotFound: return NotFound(new { message = "Kategorija nerasta." });
                }
            }
            if (result.IsSuccess)
            {
                return Ok();
            }

            return NotFound();
        } 
    }
}
