using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace Evals.IV.Infrastructure.Identity
{
    public interface IUserRepository : IDisposable
    {


        ClaimsIdentityFactory<Customer> ClaimsIdentityFactory { get; set; }
        IPasswordHasher PasswordHasher { get; set; }
        IIdentityValidator<string> PasswordValidator { get; set; }
        IUserStore<Customer> Store { get; }
        bool SupportsUserClaim { get; set; }
        bool SupportsUserLogin { get; set; }
        bool SupportsUserPassword { get; set; }
        bool SupportsUserRole { get; set; }
        bool SupportsUserSecurityStamp { get; set; }
        IIdentityValidator<Customer> UserValidator { get; set; }
        IAuthenticationManager AuthenticationManager { get; set; }

        [DebuggerStepThrough]
        Task<IdentityResult> AddClaimAsync(string userId, Claim claim);
        [DebuggerStepThrough]
        Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo login);
        [DebuggerStepThrough]
        Task<IdentityResult> AddPasswordAsync(string userId, string password);
        [DebuggerStepThrough]
        Task<IdentityResult> AddToRoleAsync(string userId, string role);
        [DebuggerStepThrough]
        Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
        [DebuggerStepThrough]
        Task<IdentityResult> CreateAsync(Customer user);
        [DebuggerStepThrough]
        Task<IdentityResult> CreateAsync(Customer user, string password);
        Task<ClaimsIdentity> CreateIdentityAsync(Customer user, string authenticationType);

        Task<Customer> FindAsync(UserLoginInfo login);
        [DebuggerStepThrough]
        Task<Customer> FindAsync(string userName, string password);
        [DebuggerStepThrough]
        Task<Customer> FindByIdAsync(string userId);
        [DebuggerStepThrough]
        Task<Customer> FindByNameAsync(string userName);
        [DebuggerStepThrough]
        Task<IList<Claim>> GetClaimsAsync(string userId);
        [DebuggerStepThrough]
        Task<IList<UserLoginInfo>> GetLoginsAsync(string userId);
        [DebuggerStepThrough]
        Task<IList<string>> GetRolesAsync(string userId);
        [DebuggerStepThrough]
        Task<bool> HasPasswordAsync(string userId);
        [DebuggerStepThrough]
        Task<bool> IsInRoleAsync(string userId, string role);
        [DebuggerStepThrough]
        Task<IdentityResult> RemoveClaimAsync(string userId, Claim claim);
        [DebuggerStepThrough]
        Task<IdentityResult> RemoveFromRoleAsync(string userId, string role);
        [DebuggerStepThrough]
        Task<IdentityResult> RemoveLoginAsync(string userId, UserLoginInfo login);
        [DebuggerStepThrough]
        Task<IdentityResult> RemovePasswordAsync(string userId);
        [DebuggerStepThrough]
        Task<IdentityResult> UpdateAsync(Customer user);
        [DebuggerStepThrough]
        Task<IdentityResult> UpdateSecurityStampAsync(string userId);
        [DebuggerStepThrough]
        Task SignInAsync(Customer user, bool isPersistent);
    }
}
