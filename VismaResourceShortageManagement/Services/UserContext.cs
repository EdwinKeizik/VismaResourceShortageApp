using System;

namespace VismaResourceShortageManagement.Services{
public class UserContext
{
    public string Name { get; private set; } = string.Empty;
    public bool IsAdmin { get; private set; }
    private const string AdminIdentifier = "admin";
    public void SetCurrentUser(string userName)
    {
        Name = userName;
        IsAdmin = userName.Equals(AdminIdentifier, StringComparison.OrdinalIgnoreCase);
    }
}
}