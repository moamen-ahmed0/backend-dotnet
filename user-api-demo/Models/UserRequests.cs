namespace user_api_demo.Models;

public record CreateUserRequest(
    string Username,
    string Password
);

public record UpdateUserRequest(
    string Username,
    string Password
);
