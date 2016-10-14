// <copyright file="AuthenticationRenderer.cs" company="University of Murcia">
// Copyright © 2016 University of Murcia
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// <author>Alessandro Menti</author>

using TellOP;
using TellOP.UWP;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(Authentication), typeof(AuthenticationRenderer))]

namespace TellOP.UWP
{
    using System;
    using System.Collections.Generic;
    using Windows.Security.Authentication.Web;
    using Windows.Security.Cryptography;
    using Xamarin.Auth;
    using Xamarin.Forms;

    /// <summary>
    /// Authentication renderer for UWP.
    /// </summary>
    public class AuthenticationRenderer : PageRenderer
    {
        /// <summary>
        /// Called when an element is changed.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected override async void OnElementChanged(
            ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);

            // Use a stored account if available, otherwise show the
            // authentication UI
            // FIXME: for now, we ask for a token every time so as not to handle
            // the case where the token is expired. Consider implementing
            // refresh tokens or RFC 7662 (token introspection) in the future

            /* Account appAccount = OAuthAccountStoreFactory.Create(this.Context)
                .FindAccountsForService(App.AppName).FirstOrDefault();
            if (appAccount == null)
            {*/

            // Start the token request using the OAuth 2.0 implicit grant
            string state = CryptographicBuffer.EncodeToHexString(
                CryptographicBuffer.GenerateRandom(25));
            Uri authorizationURI = new Uri(
                Config.TellOPConfiguration.OAuth2AuthorizeURL.ToString()
                + "?response_type=token&client_id="
                + Uri.EscapeDataString(Config.TellOPConfiguration.OAuth2ClientId)
                + "&redirect_uri="
                + Uri.EscapeDataString(Config.TellOPConfiguration.OAuth2RedirectURL.ToString())
                + "&scope="
                + Uri.EscapeDataString(Config.TellOPConfiguration.OAuth2Scopes)
                + "&state="
                + state);
            WebAuthenticationResult authResult;

            try
            {
                authResult = await WebAuthenticationBroker.AuthenticateAsync(
                        WebAuthenticationOptions.None,
                        authorizationURI,
                        Config.TellOPConfiguration.OAuth2RedirectURL);

                switch (authResult.ResponseStatus)
                {
                    case WebAuthenticationStatus.Success:
                        break;
                    case WebAuthenticationStatus.UserCancel:
                        // TODO: this shows an error message. Improve UX!
                        TellOP.App.LoginAuthenticationCompletedButNotAuthenticatedAction.Invoke();
                        return;
                    case WebAuthenticationStatus.ErrorHttp:
                    default:
                        TellOP.App.LoginAuthenticationErrorAction.Invoke();
                        return;
                }
            }
            catch (Exception)
            {
                TellOP.App.LoginAuthenticationErrorAction.Invoke();
                return;
            }

            // Analyze the OAuth 2.0 response data
            bool stateMatches = false;
            string accessToken = null;
            Dictionary<string, string> oauthResponse = new Dictionary<string, string>();
            string responseData = authResult.ResponseData;
            if (responseData.Contains("#"))
            {
                responseData = responseData.Split('#')[1];
            }

            string[] oAuthResponseParams = responseData.Split('&');
            foreach (string oAuthResponseParam in oAuthResponseParams)
            {
                string[] splitParam = oAuthResponseParam.Split('=');
                if (splitParam.Length != 2)
                {
                    continue;
                }

                // Note: plus signs should be removed from the URI
                // manually as UnescapeDataString does not do that (the
                // behavior is not standard across all URI schemes).
                // See <https://msdn.microsoft.com/en-us/library/system.uri.unescapedatastring(v=vs.110).aspx>
                string unescapedParam
                    = Uri.UnescapeDataString(splitParam[1]).Replace('+', ' ');
                oauthResponse.Add(splitParam[0], unescapedParam);

                switch (splitParam[0])
                {
                    case "state":
                        stateMatches = unescapedParam.Equals(state);
                        break;
                    case "access_token":
                        accessToken = unescapedParam;
                        break;
                    case "token_type":
                        if (!unescapedParam.Equals("Bearer"))
                        {
                            TellOP.App.LoginAuthenticationErrorAction.Invoke();
                            return;
                        }

                        break;
                    case "scope":
                        if (!unescapedParam.Equals(
                            Config.TellOPConfiguration.OAuth2Scopes))
                        {
                            TellOP.App.LoginAuthenticationErrorAction.Invoke();
                            return;
                        }

                        break;
                    default:
                        // Nothing to do
                        break;
                }
            }

            if (!stateMatches
                || string.IsNullOrWhiteSpace(accessToken))
            {
                TellOP.App.LoginAuthenticationErrorAction.Invoke();
                return;
            }

            // Create a Xamarin.Auth account and load user data
            Account authAccount = new Account(null, oauthResponse);
            TellOP.App.LoadUserData(authAccount);

            /*}
            else
            {
                App.LoadUserData(appAccount);
            }*/
        }
    }
}
