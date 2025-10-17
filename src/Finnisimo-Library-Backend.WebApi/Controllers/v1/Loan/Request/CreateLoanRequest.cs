namespace Finnisimo_Library_Backend.WebApi.Controllers.v1.Loan.Request;

public sealed class CreateLoanRequest
{
  public Guid BookId { get; set; }
  public int LoanDurationInDays { get; set; }
}
