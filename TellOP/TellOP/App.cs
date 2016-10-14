// <copyright file="App.cs" company="University of Murcia">
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
// <author>Mattia Zago</author>
// <author>Alessandro Menti</author>

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using TellOP.API;
using TellOP.DataModels;
using Xamarin.Auth;
using Xamarin.Forms;

[assembly: InternalsVisibleTo("TellOP.UWP")]

namespace TellOP
{
    /// <summary>
    /// Application object.
    /// </summary>
    public class App : Application
    {
        /// <summary>
        /// The application name.
        /// </summary>
        public const string AppName = "TellOP";

        /// <summary>
        /// The login page.
        /// </summary>
        private static NavigationPage loginPage;

        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// </summary>
        public App()
        {
            // Localize the app.
            LocalizationUtils = DependencyService.Get<ILocalize>();
            if (Device.OS != TargetPlatform.WinPhone)
            {
                LocalizationUtils.SetLocale();
            }

            // Set the certificate verifier
            DependencyService.Get<ICertificateVerifier>().SetCertificateVerifier();

            // Show the main page
            loginPage = new NavigationPage(new Login());
            this.MainPage = loginPage;
        }

        /// <summary>
        /// Gets a platform-dependent implementation of <see cref="ILocalize"/>
        /// containing several localization utilities.
        /// </summary>
        public static ILocalize LocalizationUtils { get; private set; }

        /// <summary>
        /// Gets or sets the OAuth 2.0 account associated to this app.
        /// </summary>
        public static Account OAuth2Account { get; set; }

        /// <summary>
        /// Gets or sets the user account associated to this app.
        /// </summary>
        public static User User { get; set; }

        /// <summary>
        /// Gets a value indicating whether the user is logged in.
        /// </summary>
        public static bool IsLoggedIn
        {
            get
            {
                return User != null;
            }
        }

        /// <summary>
        /// Gets the action performed when the login operation succeeds.
        /// </summary>
        public static Action SuccessfulLoginAction
        {
            get
            {
                return new Action(() =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        App.Current.MainPage = new NavigationPage(new Dashboard());
                    });
                });
            }
        }

        /// <summary>
        /// Gets the action performed when the login operation fails due to
        /// an error in the authentication process.
        /// </summary>
        public static Action LoginAuthenticationErrorAction
        {
            get
            {
                return new Action(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await App.loginPage.DisplayAlert(
                            TellOP.Properties.Resources.OAuth2_AuthenticationErrorTitle,
                            TellOP.Properties.Resources.OAuth2_AuthenticationError,
                            TellOP.Properties.Resources.ButtonOK).ConfigureAwait(false);
                        await App.loginPage.PopAsync().ConfigureAwait(false);
                    });
                });
            }
        }

        /// <summary>
        /// Gets the action performed when the login operation fails due to
        /// an error in the authentication process (authentication is complete,
        /// but the user is still not logged in).
        /// </summary>
        public static Action LoginAuthenticationCompletedButNotAuthenticatedAction
        {
            get
            {
                return new Action(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await App.loginPage.DisplayAlert(
                            TellOP.Properties.Resources.OAuth2_AuthenticationCompleteNotAuthenticatedErrorTitle,
                            TellOP.Properties.Resources.OAuth2_AuthenticationCompleteNotAuthenticatedError,
                            TellOP.Properties.Resources.ButtonOK).ConfigureAwait(false);
                        await App.loginPage.PopAsync().ConfigureAwait(false);
                    });
                });
            }
        }

        /// <summary>
        /// Gets the action performed when the login operation fails due to
        /// an error while calling the "Get user profile API" endpoint.
        /// </summary>
        public static Action LoginAPICallErrorAction
        {
            get
            {
                return new Action(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await App.loginPage.DisplayAlert(
                            TellOP.Properties.Resources.API_UserDataErrorTitle,
                            TellOP.Properties.Resources.API_UserDataError,
                            TellOP.Properties.Resources.ButtonOK).ConfigureAwait(false);
                        await App.loginPage.PopAsync().ConfigureAwait(false);
                    });
                });
            }
        }

        /// <summary>
        /// Gets the action performed when the login operation fails due to
        /// an error while reading the response from the "Get user profile API"
        /// endpoint.
        /// </summary>
        public static Action LoginAPIJsonExceptionAction
        {
            get
            {
                return new Action(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await App.loginPage.DisplayAlert(
                            TellOP.Properties.Resources.API_UserDataJSONErrorTitle,
                            TellOP.Properties.Resources.API_UserDataJSONError,
                            TellOP.Properties.Resources.ButtonOK).ConfigureAwait(false);
                        await App.loginPage.PopAsync().ConfigureAwait(false);
                    });
                });
            }
        }

        /// <summary>
        /// Loads the user profile data into the app.
        /// </summary>
        /// <param name="account">The user account.</param>
        public static async void LoadUserData(Account account)
        {
            // Get the profile data from the Web application
            UserProfile userController = new UserProfile(account);
            try
            {
                User apiResponse = await userController.CallEndpointAsObjectAsync().ConfigureAwait(false);
                account.Username = apiResponse.Email;

                // TODO: remove when the OAuth introspection RFC is implemented
                /* AccountStore.Create(this.Context).Save(
                    account,
                    App.AppName); */

                App.OAuth2Account = account;
                App.User = apiResponse;
                App.SuccessfulLoginAction.Invoke();
            }
            catch (UnsuccessfulAPICallException)
            {
                App.LoginAPICallErrorAction.Invoke();
            }
            catch (Exception e)
                when (e is JsonSerializationException || e is JsonReaderException)
            {
                App.LoginAPIJsonExceptionAction.Invoke();
            }
        }

        /// <summary>
        /// Gets the authenticator used for OAuth 2.0 authentication
        /// operations.
        /// </summary>
        /// <returns>The instance of <see cref="OAuth2Authenticator"/> used by
        /// the app to authenticate.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Needs to generate a fresh authenticator")]
        public static OAuth2Authenticator GetOAuthAuthenticator()
        {
            OAuth2Authenticator auth = new OAuth2Authenticator(
                Config.TellOPConfiguration.OAuth2ClientId,
                Config.TellOPConfiguration.OAuth2Scopes,
                Config.TellOPConfiguration.OAuth2AuthorizeURL,
                Config.TellOPConfiguration.OAuth2RedirectURL);
            auth.AllowCancel = false;
            auth.Title = TellOP.Properties.Resources.OAuth2_Title;
            return auth;
        }
    }
}
