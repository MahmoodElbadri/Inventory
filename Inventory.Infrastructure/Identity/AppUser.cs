
using Microsoft.AspNetCore.Identity;

namespace Inventory.Infrastructure.Identity;

public class AppUser: IdentityUser<int>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}
