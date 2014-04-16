using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;


namespace Evals.IV.Infrastructure.Identity
{
    [Export(typeof(IUserRepository))]
    internal class UserRepository : IUserRepository
    {
        private UserManager<Customer> userManager { get; set; }
        [ImportingConstructor]
        public UserRepository()
        {
            this.userManager = new UserManager<Customer>(new UserStore<Customer>(new UserDbContext()));
        }
        public ClaimsIdentityFactory<Customer> ClaimsIdentityFactory { get; set; }
        public IPasswordHasher PasswordHasher { get; set; }
        public IIdentityValidator<string> PasswordValidator { get; set; }
        public IUserStore<Customer> Store { get; set; }
        public bool SupportsUserClaim { get; set; }
        public bool SupportsUserLogin { get; set; }
        public bool SupportsUserPassword { get; set; }
        public bool SupportsUserRole { get; set; }
        public bool SupportsUserSecurityStamp { get; set; }
        public IIdentityValidator<Customer> UserValidator { get; set; }


        public async Task<IdentityResult> AddClaimAsync(string userId, System.Security.Claims.Claim claim)
        {
            return await userManager.AddClaimAsync(userId, claim);
        }

        public async Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo login)
        {
            return await userManager.AddLoginAsync(userId, login);
        }

        public async Task<IdentityResult> AddPasswordAsync(string userId, string password)
        {
            return await userManager.AddPasswordAsync(userId, password);
        }

        public async Task<IdentityResult> AddToRoleAsync(string userId, string role)
        {
            return await userManager.AddToRoleAsync(userId, role);
        }

        public async Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            return await userManager.ChangePasswordAsync(userId, currentPassword, newPassword);
        }

        public async Task<IdentityResult> CreateAsync(Customer user)
        {
            return await userManager.CreateAsync(user);
        }

        public async Task<IdentityResult> CreateAsync(Customer user, string password)
        {
            return await userManager.CreateAsync(user, password);
        }

        public async Task<System.Security.Claims.ClaimsIdentity> CreateIdentityAsync(Customer user, string authenticationType)
        {
            return await userManager.CreateIdentityAsync(user, authenticationType);
        }

        public async Task<Customer> FindAsync(UserLoginInfo login)
        {
            return await userManager.FindAsync(login);
        }

        public async Task<Customer> FindAsync(string userName, string password)
        {
            return await userManager.FindAsync(userName, password);
        }

        public async Task<Customer> FindByIdAsync(string userId)
        {
            return await userManager.FindByIdAsync(userId);
        }

        public async Task<Customer> FindByNameAsync(string userName)
        {
            return await userManager.FindByNameAsync(userName);
        }

        public async Task<IList<System.Security.Claims.Claim>> GetClaimsAsync(string userId)
        {
            return await userManager.GetClaimsAsync(userId);
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(string userId)
        {
            return await userManager.GetLoginsAsync(userId);
        }

        public async Task<IList<string>> GetRolesAsync(string userId)
        {
            return await userManager.GetRolesAsync(userId);
        }

        public async Task<bool> HasPasswordAsync(string userId)
        {
            return await userManager.HasPasswordAsync(userId);
        }

        public async Task<bool> IsInRoleAsync(string userId, string role)
        {
            return await userManager.IsInRoleAsync(userId, role);
        }

        public async Task<IdentityResult> RemoveClaimAsync(string userId, System.Security.Claims.Claim claim)
        {
            return await userManager.RemoveClaimAsync(userId, claim);
        }

        public async Task<IdentityResult> RemoveFromRoleAsync(string userId, string role)
        {
            return await userManager.RemoveFromRoleAsync(userId, role);
        }

        public async Task<IdentityResult> RemoveLoginAsync(string userId, UserLoginInfo login)
        {
            return await userManager.RemoveLoginAsync(userId, login);
        }

        public async Task<IdentityResult> RemovePasswordAsync(string userId)
        {
            return await userManager.RemovePasswordAsync(userId);
        }

        public async Task<IdentityResult> UpdateAsync(Customer user)
        {
            return await userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> UpdateSecurityStampAsync(string userId)
        {
            return await userManager.UpdateSecurityStampAsync(userId);
        }
        public async Task SignInAsync(Customer user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        public IAuthenticationManager AuthenticationManager
        {
            get;
            set;
        }
        public void Dispose()
        {
            this.userManager.Dispose();
            userManager = null;

        }
    }
}
