namespace DE.Forms.Models;

public sealed class CaptchaValidationResult
{
    public CaptchaValidationResult(bool isSuccess, string message)
    {
        IsSuccess = isSuccess;
        Message = message;
    }

    public bool IsSuccess { get; }

    public string Message { get; }
}
