using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Twilio.Jwt.AccessToken;
using TwilioOpenAppointement.Models;
using TwilioOpenAppointement;

[ApiController]
[Route("api/[controller]")]
public class VideoController : ControllerBase
{
    private readonly IConfiguration _config;

    public VideoController(IConfiguration config)
    {
        _config = config;
    }

    [HttpPost("token")]
    public IActionResult GetVideoToken([FromBody] VideoTokenRequest request)
    {
        var accountSid = _config["TwilioSettings:AccountSID"];
        var apiKeySid = _config["TwilioSettings:ApiKeySID"];
        var apiKeySecret = _config["TwilioSettings:ApiKeySecret"];


        var videoGrant = new VideoGrant { Room = request.RoomName };
        var token = new Token(
            accountSid,
            apiKeySid,
            apiKeySecret,
            request.Identity,
            grants: new HashSet<IGrant> { videoGrant }
        );

        return Ok(new { token = token.ToJwt() });
    }
}
