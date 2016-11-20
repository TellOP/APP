// <copyright file="Login.xaml.cs" company="University of Murcia">
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

namespace TellOP
{
    using System;
    using Config;
    using Plugin.Connectivity;
    using Tools;
    using Xamarin.Forms;

    /// <summary>
    /// The application login page.
    /// </summary>
    public partial class Login : ContentPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Login"/> class.
        /// </summary>
        public Login()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Called when the "Log in" button is clicked.
        /// </summary>
        /// <param name="sender">The object sending the event.</param>
        /// <param name="e">The event parameters.</param>
        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            if (await ConnectivityCheck.AskToEnableConnectivity(this))
            {
                await this.Navigation.PushModalAsync(new Authentication());
            }
        }

        /// <summary>
        /// Called when the "Sign up" link is tapped.
        /// </summary>
        /// <param name="sender">The object sending the event.</param>
        /// <param name="e">The event parameters.</param>
        private void SignUp_Tapped(object sender, EventArgs e)
        {
            Device.OpenUri(TellOPConfiguration.SignUpUrl);
        }

        /// <summary>
        /// Called when the "About" link is tapped.
        /// </summary>
        /// <param name="sender">The object sending the event.</param>
        /// <param name="e">The event parameters.</param>
        private async void About_Tapped(object sender, EventArgs e)
        {
            await this.Navigation.PushAsync(new About());
        }

        /// <summary>
        /// Called when the "Privacy" link is tapped.
        /// </summary>
        /// <param name="sender">The object sending the event.</param>
        /// <param name="e">The event parameters.</param>
        private void Privacy_Tapped(object sender, EventArgs e)
        {
            Device.OpenUri(TellOPConfiguration.PrivacyUrl);
        }
    }
}
