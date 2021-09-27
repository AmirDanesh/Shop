namespace Innovare.Shop.Business.Configurations.Identitiy
{
    public class IdentityOptions
    {
        public bool IsEnabled { get; set; }

        public TokenOptions Token { get; set; }

        public UserOptions User { get; set; }

        public PasswordOptions Password { get; set; }

        public SignInOptions SignIn { get; set; }

        public LockoutOptions Lockout { get; set; }
    }
}