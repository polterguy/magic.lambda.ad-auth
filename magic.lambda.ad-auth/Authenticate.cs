/*
 * Magic Cloud, copyright Aista, Ltd. See the attached LICENSE file for details.
 */

using System;
using System.Linq;
using System.DirectoryServices;
using magic.node;
using magic.node.contracts;
using magic.node.extensions;
using magic.signals.contracts;
using magic.lambda.auth.contracts;

namespace magic.lambda.ad_auth
{
    /// <summary>
    /// [auth.ad.authenticate] slot that authenticates a user over Active Directory.
    /// </summary>
    [Slot(Name = "auth.ad.authenticate")]
    public class Authenticate : ISlot
    {
        readonly IAuthSettings _settings;

        /// <summary>
        /// Creates an instance of your type
        /// </summary>
        /// <param name="configuration">Dependency injected configuration object</param>
        public Authenticate(IAuthSettings settings)
        {
            _settings = settings;
        }

        /// <summary>
        /// Slot implementation.
        /// </summary>
        /// <param name="signaler">Signaler that raised signal.</param>
        /// <param name="input">Arguments to slot.</param>
        public void Signal(ISignaler signaler, Node input)
        {
            // Retrieving LDAP configuration.
            var path = _settings.LDAP;

            // Retrieving username from arguments.
            var username = input.Children.FirstOrDefault(x => x.Name == "username")?.GetEx<string>() ??
                throw new HyperlambdaException("No [username] provided to [auth.ad.authentication]");

            // Retrieving password from arguments.
            var password = input.Children.FirstOrDefault(x => x.Name == "password")?.GetEx<string>() ??
                throw new HyperlambdaException("No [password] provided to [auth.ad.authentication]");

            using (var user = new DirectoryEntry(path, username, password))
            {
                if (user.NativeObject == null)
                    input.Value = false;
                else
                    input.Value = true;
            }
        }
    }
}
