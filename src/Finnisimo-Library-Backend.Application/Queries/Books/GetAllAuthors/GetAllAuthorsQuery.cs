using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Queries;

namespace Finnisimo_Library_Backend.Application.Queries.Books.GetAllAuthors;

public sealed record GetAllAuthorsQuery() : IQuery<IReadOnlyList<string>>;
