namespace CleanMoney.Application.DTOs
{
    public class Profile
    {
        public string FullName { get; set; } = default!;
        public string Username { get; set; } = default!;
        public string Email { get; set; } = default!;
        public bool IsActive { get; set; }
    }
}
