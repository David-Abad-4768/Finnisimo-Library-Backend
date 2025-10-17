using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Queries;

namespace Finnisimo_Library_Backend.Application.Queries.Books.GetAllGenres;

public sealed record GetAllGenresQuery() : IQuery<IReadOnlyList<string>>;
