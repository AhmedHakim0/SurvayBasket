namespace SurvayBasket.API.Controllers;
[Route("[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    [HttpPost("")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request, CancellationToken cancellationToken = default)
    {
        var AuthResult = await _authService.GetTokenAsync(request.Email, request.Password, cancellationToken);

        return AuthResult.IsSuccess
        ? Ok(AuthResult.Value)
        : Problem(statusCode: StatusCodes.Status404NotFound, title: AuthResult.Error.code, detail: AuthResult.Error.Description);

    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        var AuthResult = await _authService.GetRefreshTokenAsync(request.token, request.refreshToken, cancellationToken);

        return AuthResult is not null
            ? Ok(AuthResult.Value)
            : Problem(statusCode: StatusCodes.Status404NotFound, title: AuthResult.Error.code, detail: AuthResult.Error.Description);
    }

}
