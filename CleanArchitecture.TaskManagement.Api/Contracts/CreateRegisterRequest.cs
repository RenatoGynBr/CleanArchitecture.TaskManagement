namespace CleanArchitecture.TaskManagement.Api.Contracts;

public sealed record CreateRegisterRequest
(
    string FullName,
    string Email,
    string Password
);