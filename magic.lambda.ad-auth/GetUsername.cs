/*
 * Aista Cloud, copyright Aista, Ltd. See the attached LICENSE file for details.
 * See the enclosed LICENSE file for details.
 */

using Microsoft.AspNetCore.Http;
using magic.node;
using magic.signals.contracts;

namespace magic.lambda.ad_auth
{
    /// <summary>
    /// [auth.ad.get-username] slot that authenticates a user over Active Directory using Windows Authentication,
    /// implying ZERO username/password authentication.
    /// </summary>
    [Slot(Name = "auth.ad.get-username")]
    public class GetUsername : ISlot
    {
        readonly IHttpContextAccessor _contextAccessor;

        /// <summary>
        /// Creates an instance of your type
        /// </summary>
        /// <param name="contextAccessor">Dependency injected HTTP context accessor</param>
        public GetUsername(IHttpContextAccessor contextAccessor)
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
            input.Value = _contextAccessor.HttpContext.User.Identity.Name;
        }
    }
}
