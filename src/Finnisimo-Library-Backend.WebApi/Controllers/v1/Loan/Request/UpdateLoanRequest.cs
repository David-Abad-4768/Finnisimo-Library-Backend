using Finnisimo_Library_Backend.Domain.Entities.Loans;

namespace Finnisimo_Library_Backend.WebApi.Controllers.v1.Loan.Request;

public sealed class UpdateLoanRequest
{
  public LoanStatus Status { get; set; }
  public int? LoanDurationDays { get; set; }
}
