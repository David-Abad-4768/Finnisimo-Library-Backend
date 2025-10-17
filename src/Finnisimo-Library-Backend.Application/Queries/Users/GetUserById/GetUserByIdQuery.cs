using Finnisimo_Library_Backend.Application.Abstractions.Messaging.Queries;
using Finnisimo_Library_Backend.Application.Queries.Responses;

namespace Finnisimo_Library_Backend.Application.Queries.Users.GetUserById;

public sealed record GetUserByIdQuery(Guid Id) : IQuery<UserResponse> { }
