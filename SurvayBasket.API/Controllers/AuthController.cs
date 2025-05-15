
namespace SurvayBasket.API.Controllers;
[Route("[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    [HttpPost("")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request, CancellationToken cancellationToken = default)
    {
        var AuthResponse= await _authService.GetTokenAsync(request.Email, request.Password, cancellationToken);

        return AuthResponse is null? BadRequest("Email or password is incorrect") : Ok(AuthResponse);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        var AuthResponse = await _authService.GetRefreshTokenAsync(request.token, request.refreshToken, cancellationToken);

        return AuthResponse is null ? BadRequest("Invalid Token") : Ok(AuthResponse);
    }

}
