using Microsoft.AspNetCore.Identity;
using System;

namespace Innovare.Shop.Data.Models.Identity
{
    public class UserModel : IdentityUser<Guid>
    {
        public string DisplayFirstName { get; set; }

        public string DisplayLastName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}