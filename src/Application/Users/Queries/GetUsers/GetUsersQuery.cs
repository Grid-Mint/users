namespace Users.Application.Users.Queries.GetUsers;

public sealed record GetUsersQuery(Guid? Id, int Skip, int Take);
