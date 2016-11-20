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
using System.Threading.Tasks;
using Newtonsoft.Json;
using TellOP.Api;
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

            // Automatically mark all unobserved task exceptions as observed after logging them.
            // TODO: this is a temporary fix put in place until more extensive investigation reveals why some
            // INotifyTaskCompletion exceptions are not caught by NotifyTaskCompletion (maybe because they are inside
            // inner tasks?)
            TaskScheduler.UnobservedTaskException += (sender, args) =>
            {
                foreach (Exception ex in args.Exception.InnerExceptions)
                {
                    Tools.Logger.Log(sender.GetType().ToString(), "Unhandled task exception", ex);
                }

                args.SetObserved();
            };

            // Show the main page
            loginPage = new NavigationPage(new Login());
            this.MainPage = loginPage;
        }

        /// <summary>
        /// Gets a platform-dependent implementation of <see cref="ILocalize"/> containing several localization
        /// utilities.
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
        public static Action SuccessfulLogOnAction
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
        /// Gets the action performed when the login operation fails due to an error in the authentication process.
        /// </summary>
        public static Action LogOnAuthenticationErrorAction
        {
            get
            {
                return new Action(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await App.loginPage.DisplayAlert(TellOP.Properties.Resources.OAuth2_AuthenticationErrorTitle, TellOP.Properties.Resources.OAuth2_AuthenticationError, TellOP.Properties.Resources.ButtonOK);
                        await App.loginPage.Navigation.PopModalAsync();
                    });
                });
            }
        }

        /// <summary>
        /// Gets the action performed when the login operation fails due to an error in the authentication process
        /// (authentication is complete, but the user is still not logged in).
        /// </summary>
        public static Action LogOnAuthenticationCompletedButNotAuthenticatedAction
        {
            get
            {
                return new Action(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await App.loginPage.DisplayAlert(TellOP.Properties.Resources.OAuth2_AuthenticationCompleteNotAuthenticatedErrorTitle, TellOP.Properties.Resources.OAuth2_AuthenticationCompleteNotAuthenticatedError, TellOP.Properties.Resources.ButtonOK);
                        await App.loginPage.Navigation.PopModalAsync();
                    });
                });
            }
        }

        /// <summary>
        /// Gets the action performed when the login operation fails due to an error while calling the "Get user
        /// profile API" endpoint.
        /// </summary>
        public static Action LogOnApiCallErrorAction
        {
            get
            {
                return new Action(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await App.loginPage.DisplayAlert(TellOP.Properties.Resources.API_UserDataErrorTitle, TellOP.Properties.Resources.API_UserDataError, TellOP.Properties.Resources.ButtonOK);
                        await App.loginPage.Navigation.PopModalAsync();
                    });
                });
            }
        }

        /// <summary>
        /// Gets the action performed when the login operation fails due to an error while reading the response from
        /// the "Get user profile API" endpoint.
        /// </summary>
        public static Action LogOnApiJsonExceptionAction
        {
            get
            {
                return new Action(() =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await App.loginPage.DisplayAlert(TellOP.Properties.Resources.API_UserDataJSONErrorTitle, TellOP.Properties.Resources.API_UserDataJSONError, TellOP.Properties.Resources.ButtonOK);
                        await App.loginPage.Navigation.PopModalAsync();
                    });
                });
            }
        }

        /// <summary>
        /// Loads the user profile data into the app.
        /// </summary>
        /// <param name="account">The user account.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task LoadUserData(Account account)
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
                App.SuccessfulLogOnAction.Invoke();
            }
            catch (UnsuccessfulApiCallException)
            {
                App.LogOnApiCallErrorAction.Invoke();
            }
            catch (Exception e)
                when (e is JsonSerializationException || e is JsonReaderException)
            {
                App.LogOnApiJsonExceptionAction.Invoke();
            }
        }

        /// <summary>
        /// Gets the authenticator used for OAuth 2.0 authentication operations.
        /// </summary>
        /// <returns>The instance of <see cref="OAuth2Authenticator"/> used by the app to authenticate.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Needs to generate a fresh authenticator")]
        public static OAuth2Authenticator GetOAuthAuthenticator()
        {
            OAuth2Authenticator auth = new OAuth2Authenticator(Config.TellOPConfiguration.OAuth2ClientId, Config.TellOPConfiguration.OAuth2Scopes, Config.TellOPConfiguration.OAuth2AuthorizeUrl, Config.TellOPConfiguration.OAuth2RedirectUrl);
            auth.AllowCancel = false;
            auth.Title = TellOP.Properties.Resources.OAuth2_Title;
            return auth;
        }
    }
}
