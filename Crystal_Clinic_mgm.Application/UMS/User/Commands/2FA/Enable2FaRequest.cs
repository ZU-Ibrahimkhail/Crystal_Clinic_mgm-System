namespace Crystal_Clinic_Mgm.Application.UMS.User.Commands._2FA
{
    public class Enable2FaRequest
    {
        public string UserId { get; set; }

    }

    public class Generate2FaQrCodeAndUriResponse
    {
        public string? SharedKey { get; set; }
        public string? AuthenticatorUri { get; set; }
        public string? ErrorMessage { get; set; }
    }


    public class Disable2FaRequest
    {
        public string UserId { get; set; }
    }

    public class VerifyTheCode2FaRequest
    {
        public string UserId { get; set; }
        public string VerificationCode { get; set; }
    }

    public class VerifyTheCode2FaResponse
    {
        public VerifyTheCode2FaResponse()
        {
        }
        public VerifyTheCode2FaResponse(string error)
        {
            ErrorMessage = error;
        }
        public string? StatusMessage { get; set; }
        public IEnumerable<string> RecoveryCodes { get; set; } = Enumerable.Empty<string>();
        public string? ErrorMessage { get; set; }
    }


}
