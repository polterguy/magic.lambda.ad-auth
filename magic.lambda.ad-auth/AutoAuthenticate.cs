/*
 * Magic, Copyright(c) Thomas Hansen 2019 - 2021, thomas@servergardens.com, all rights reserved.
 * See the enclosed LICENSE file for details.
 */

using System.Security;
using Microsoft.AspNetCore.Http;
using magic.node;
using magic.signals.contracts;

namespace magic.lambda.ad_auth
{
    /// <summary>
    /// [ad-auth.authenticate] slot that authenticates a user over Active Directory using Windows Authentication,
    /// implying ZERO username/password authentication.
    /// </summary>
    [Slot(Name = "ad-auth.authenticate-auto")]
    public class AutoAuthenticate : ISlot
    {
        readonly IHttpContextAccessor _contextAccessor;

        /// <summary>
        /// Creates an instance of your type
        /// </summary>
        /// <param name="contextAccessor">Dependency injected HTTP context accessor</param>
        public AutoAuthenticate(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        /// <summary>
        /// Slot implementation.
        /// </summary>
        /// <param name="signaler">Signaler that raised signal.</param>
        /// <param name="input">Arguments to slot.</param>
        public void Signal(ISignaler signaler, Node input)
        {
            /*
             * Notice, if invocation is given an explicit [username]/[password]
             * combination, we use these to authenticate user to allow for users
             * to login with their own Windows credentials on machines were they're
             * not logged into their Windows accounts.
             */
            var username = input.Children.FirstOrDefault(x => x.Name == "username").GetEx<string>();
            var password = input.Children.FirstOrDefault(x => x.Name == "password").GetEx<string>();
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                using (var user = new DirectoryEntry(path, username, password))
                {
                    if (user.NativeObject == null)
                        input.Value = false;
                    else
                        input.Value = true;
                }
            }
            else
            {
                // Retrieving identity through Windows Authentication.
                input.Value = _contextAccessor.HttpContext.User.Identity.Name;
            }
        }
    }
}
