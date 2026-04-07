namespace ToDoPlatform.ViewModels;

public class UserVM
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string ProfilePicture { get; set; }
    public string Roles { get; set; }
    public bool IsAdmin { get; set; } = false;
}