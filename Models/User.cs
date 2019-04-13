using Microsoft.AspNetCore.Identity;

namespace WorkOutTogether
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
    }
}