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
using TellOP.iOS;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(Authentication), typeof(AuthenticationRenderer))]

#pragma warning disable SA1300
namespace TellOP.iOS
#pragma warning restore SA1300
{
    using TellOP;
    using Xamarin.Auth;
    using Xamarin.Forms.Platform.iOS;

    /// <summary>
    /// Authentication renderer for iOS.
    /// </summary>
    public class AuthenticationRenderer : PageRenderer
    {
        /// <summary>
        /// Specifies whether the authentication process was already completed, either by having the user log in or
        /// using a stored account.
        /// </summary>
        /// <param name="animated">Specifies if the interface was shown with an animation.</param>
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            // Use a stored account if available, otherwise show the authentication UI
            // FIXME: for now, we ask for a token every time so as not to handle
            // the case where the token is expired. Consider implementing
            // refresh tokens or RFC 7662 (token introspection) in the future

            /* Account appAccount = OAuthAccountStoreFactory.Create()
                .FindAccountsForService(App.AppName).FirstOrDefault();
            if (appAccount == null)
            { */
            OAuth2Authenticator auth = App.GetOAuthAuthenticator();
            auth.Completed += this.OnAuthenticationCompleted;
            auth.Error += this.OnAuthenticationError;
            this.PresentViewController(auth.GetUI(), true, null);
            /* }
            else
            {
                App.SuccessfulLoginAction.Invoke();
            } */
        }

        /// <summary>
        /// Called when the authentication process is complete.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private async void OnAuthenticationCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            if (e.IsAuthenticated)
            {
                await App.LoadUserData(e.Account);
            }
            else
            {
                App.LogOnAuthenticationCompletedButNotAuthenticatedAction.Invoke();
            }
        }

        /// <summary>
        /// Called if the authentication process failed.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnAuthenticationError(object sender, AuthenticatorErrorEventArgs e)
        {
            App.LogOnAuthenticationErrorAction.Invoke();
        }
    }
}
