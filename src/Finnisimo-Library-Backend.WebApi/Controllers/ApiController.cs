using ErrorOr;
using Finnisimo_Library_Backend.WebApi.Common.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Finnisimo_Library_Backend.WebApi.Controllers;

[ApiController]
public class ApiController : ControllerBase
{
  protected IActionResult Problem(List<Error> errors)
  {
    if (errors.Count == 0)
      return Problem();

    if (errors.All(e => e.Type == ErrorType.Validation))
    {
      var modelState = new ModelStateDictionary();
      foreach (var error in errors)
        modelState.AddModelError(error.Code, error.Description);
      return ValidationProblem(modelState);
    }

    HttpContext.Items[HttpContextItemKeys.Errors] = errors;
    return Problem(errors[0]);
  }

  private IActionResult Problem(Error error)
  {
    var statusCode =
        error.Type switch
        {
          ErrorType.Conflict => StatusCodes.Status409Conflict,
          ErrorType.Validation =>
              StatusCodes.Status400BadRequest,
          ErrorType.NotFound => StatusCodes.Status404NotFound,
          _ => StatusCodes.Status500InternalServerError
        };

    return Problem(statusCode: statusCode, title: error.Description,
                   detail: error.Code);
  }
}
