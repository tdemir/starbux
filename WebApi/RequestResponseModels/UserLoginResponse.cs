namespace WebApi.RequestResponseModels;

public class UserLoginResponse
{
    public string Token { get; set; }
    public DateTime TokenExpireDate { get; set; }
}
