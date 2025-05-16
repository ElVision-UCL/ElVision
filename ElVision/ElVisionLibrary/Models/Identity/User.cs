using Microsoft.AspNetCore.Identity;

namespace ElVisionLibrary.Models.Identity
{
    public class User : IdentityUser
    {
        public string ElOverblikApiKey { get; set; }
    }
}
