using Finnisimo_Library_Backend.Application.Commands.Books.CreateBook;
using Finnisimo_Library_Backend.Application.Queries.Books.GetAllAuthors;
using Finnisimo_Library_Backend.Application.Queries.Books.GetAllBooks;
using Finnisimo_Library_Backend.Application.Queries.Books.GetAllGenres;
using Finnisimo_Library_Backend.Application.Queries.Books.GetBookById;
using Finnisimo_Library_Backend.WebApi.Controllers.v1.Book.Request;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Finnisimo_Library_Backend.WebApi.Controllers.v1.Book;

[ApiController]
[Route("api/v1/books")]
public class BookController(IMediator mediator) : ApiController
{
  private readonly ISender _mediator = mediator;

  [HttpPost]
  public async Task<IActionResult>
  CreateBook([FromBody] CreateBookRequest request)
  {
    var command = new CreateBookCommand(
        request.Title, request.Author, request.Publisher,
        request.PublicationDate, request.Description, request.Genre,
        request.NumberOfPages, request.Language, request.CoverImageUrl,
        request.Stock, request.Location);
    var result = await _mediator.Send(command);
    return result.Match(message => StatusCode(201, new { message }), Problem);
  }

  [HttpGet]
  public async Task<IActionResult> GetAllBooks(
      [FromQuery] int page = 1, [FromQuery] int pageSize = 10,
      [FromQuery] string? title = null, [FromQuery] string? author = null,
      [FromQuery] int? publicationYear = null, [FromQuery] string? genre = null,
      [FromQuery] string? sortColumn = null,
      [FromQuery] string? sortOrder = null)
  {
    var query =
        new GetAllBooksQuery(page, pageSize, title, author, publicationYear,
                             genre, sortColumn, sortOrder);

    var result = await _mediator.Send(query);

    return result.Match(Ok, Problem);
  }

  [HttpGet("{id:guid}")]
  public async Task<IActionResult> GetBookById(Guid id)
  {
    var query = new GetBookByIdQuery(id);
    var result = await _mediator.Send(query);

    return result.Match(Ok, Problem);
  }

  [HttpGet("genres")]
  public async Task<IActionResult> GetAllGenres()
  {
    var query = new GetAllGenresQuery();
    var result = await _mediator.Send(query);

    return result.Match(Ok, Problem);
  }

  [HttpGet("authors")]
  public async Task<IActionResult> GetAllAuthors()
  {
    var query = new GetAllAuthorsQuery();
    var result = await _mediator.Send(query);

    return result.Match(Ok, Problem);
  }
}
