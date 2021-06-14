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
            // Retrieving identity through Windows Authentication.
            var username = _contextAccessor.HttpContext.User.Identity.Name;
            if (username == null)
                throw new SecurityException("No such user"); // Oops, not authenticated on AD/Windows.

            // Success.
            input.Value = username;
        }
    }
}
