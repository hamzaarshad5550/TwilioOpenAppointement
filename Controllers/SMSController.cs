using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TwilioOpenAppointement;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;



[ApiController]
[Route("api/[controller]")]
public class SmsController : ControllerBase
{
    private readonly TwilioSettings _twilioSettings;

    public SmsController(IOptions<TwilioSettings> twilioSettings)
    {
        _twilioSettings = twilioSettings.Value;
    }

    [HttpPost]
    public IActionResult SendSms([FromBody] SmsRequest request)
    {
        TwilioClient.Init(_twilioSettings.AccountSID, _twilioSettings.AuthToken);

        var message = MessageResource.Create(
            body: request.Message,
            from: new Twilio.Types.PhoneNumber(_twilioSettings.FromPhoneNumber),
            to: new Twilio.Types.PhoneNumber(request.To)
        );

        return Ok(new { message.Sid, message.Status });
    }
}
