namespace Todo.Core.Dtos.AuthDto;

public class TwoFactorInfo
{
    public bool HasAuthenticator { get; set; }
    public bool Is2faEnabled { get; set; }
    public bool IsMachineRemembered { get; set; }
    public int RecoveryCodesLeft { get; set; }
    public string SharedKey { get; set; } = string.Empty;
}