namespace Finnisimo_Library_Backend.WebApi.Controllers.v1.Statistics.Request;

public record LogReadingSessionRequest(Guid LoanId, int StartPage, int EndPage);
