namespace DotnetAPI.Dtos
{
    public partial class UserForLoginForConfirmationDto
    {
        public byte[] passwordHash { get; set; }
        public byte[] passwordSalt { get; set; }

        public UserForLoginForConfirmationDto()
        {
            if (passwordHash == null)
            {
                passwordHash = new byte[0];
            }
            if (passwordSalt == null)
            {
                passwordSalt = new byte[0];
            }
        }
    }
}
