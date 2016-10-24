using System.Collections.Generic;
using System.Security.Claims;
using IdentityServer4.Services.InMemory;

namespace STS.Configuration
{
    public static class Users
    {
        public static List<InMemoryUser> Get()
        {
            return new List<InMemoryUser>() {
                new InMemoryUser() {
                    Subject = "sample-bob",
                    Username = "bob",
                    Password = "bob",
                    Claims = new Claim[] {
                        new Claim(ClaimTypes.Name, "Bob Smith"),
                        new Claim(ClaimTypes.GivenName, "Bob"),
                        new Claim(ClaimTypes.Email, "BobSmith@email.com")
                    }
                }
            };
        }
    }
}
