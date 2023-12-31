﻿using biblioteca_dotnet.Data;
using biblioteca_dotnet.Dto;
using biblioteca_dotnet.Helper;
using biblioteca_dotnet.Services;
using biblioteca_dotnet.Lib;
using Microsoft.AspNetCore.Mvc;

namespace biblioteca_dotnet.Controllers
{
    [Route("api")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookService Service;
        private readonly string Enviorment;

        public BooksController(DataContext context, IWebHostEnvironment env)
        {
            this.Enviorment = env.EnvironmentName;
            this.Service = new BookService(context, Enviorment);
        }

        [HttpGet]
        [Route("[controller]/[action]/{id}")]
        public async Task<IActionResult> SearchById(int id)
        {
            Response<BookDTO> response = await this.Service.FetchById(id);
            switch (response.StatusCode)
            {
                case 200:
                    return Ok(response);
                case 404:
                    return NotFound(response);
                default:
                    return StatusCode(500, response);
            }
        }

        [HttpGet]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> SearchByFilter(
            [FromQuery] string q,
            [FromQuery] string? title = "sad",
            [FromQuery] int page = 1,
            [FromQuery] int take = 10)
        {
            Response<BookDTO> response = await this.Service.FetchByFilter(q,title, page, take);
            switch (response.StatusCode)
            {
                case 200:
                    return Ok(response);
                case 404:
                    return NotFound(response);
                default:
                    return StatusCode(500, response);
            }
        }

        [HttpGet]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> MostRentedBooks()
        {
            Response<BookDTO> response = await this.Service.FetchMostRented();
            switch (response.StatusCode)
            {
                case 200:
                    return Ok(response);
                case 404:
                    return NotFound(response);
                default:
                    return StatusCode(500, response);
            }
        }
    }
}
