using biblioteca_dotnet.Data;
using biblioteca_dotnet.Dto;
using biblioteca_dotnet.Helper;
using biblioteca_dotnet.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace biblioteca_dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookService Service;
    
        public BooksController(DataContext context)
        {
            this.Service = new BookService(context);
        }

        [HttpGet]
        [Route("/[action]/{id}")]
        public async Task<IActionResult> SearchById(int id)
        {
            Response<BookDTO> response = await this.Service.FetchBookById(id);
            switch (response.StatusCode)
            {
                case 200:
                    return Ok(response);
                case 404:
                    return NotFound(response);
                case 500:
                    return StatusCode(500, response);
                default:
                    return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("/[action]")]
        public async Task<IActionResult> SearchFilter([FromQuery] string title)
        {
            Response<BookDTO> response = await this.Service.FetchByFilter(title);
            return Ok(response);
        }
    }
}
