//-----------------------------------------------------------------------
// <copyright file="Model.IssuedAccessToken.cs" company="Andrew Arnott">
//     Copyright (c) Andrew Arnott. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace RelyingPartyLogic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using DotNetOpenAuth.OAuth.ChannelElements;

    public partial class IssuedAccessToken : IServiceProviderAccessToken
    {
        /// <summary>
        /// Gets the roles that the OAuth principal should belong to.
        /// </summary>
        /// <value>
        /// The roles that the user belongs to, or a subset of these according to the rights
        /// granted when the user authorized the request token.
        /// </value>
        string[] IServiceProviderAccessToken.Roles
        {
            get
            {
                List<string> roles = new List<string>();

                // Include the roles the user who authorized this OAuth token has.
                roles.AddRange(this.User.Roles.Select(r => r.Name));

                // Always add an extra role to indicate this is an OAuth-authorized request.
                // This allows us to deny access to account management pages to OAuth requests.
                roles.Add("delegated");

                return roles.ToArray();
            }
        }

        /// <summary>
        /// Gets the username of the principal that will be impersonated by this access token.
        /// </summary>
        /// <value>
        /// The name of the user who authorized the OAuth request token originally.
        /// </value>
        string IServiceProviderAccessToken.Username
        {
            get
            {
                // We don't really have the concept of a single username, but we
                // can use any of the authentication tokens instead since that
                // is what the rest of the web site expects.
                if (!this.UserReference.IsLoaded)
                {
                    this.UserReference.Load();
                }
                if (!this.User.AuthenticationTokens.IsLoaded)
                {
                    this.User.AuthenticationTokens.Load();
                }
                return this.User.AuthenticationTokens.First().ClaimedIdentifier;
            }
        }

        /// <summary>
        /// Gets the expiration date (local time) for the access token.
        /// </summary>
        /// <value>
        /// The expiration date, or <c>null</c> if there is no expiration date.
        /// </value>
        DateTime? IServiceProviderAccessToken.ExpirationDate
        {
            get { return this.ExpirationDateUtc.HasValue ? (DateTime?)this.ExpirationDateUtc.Value.ToLocalTime() : null; }
        }

        partial void OnExpirationDateUtcChanging(DateTime? value)
        {
            if (value.HasValue)
            {
                Utilities.VerifyThrowNotLocalTime(value.Value);
            }
        }
    }
}
