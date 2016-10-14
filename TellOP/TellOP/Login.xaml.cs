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
            this.SignUp.GestureRecognizers.Add(
                new TapGestureRecognizer
                {
                    Command = new Command(() =>
                        Device.OpenUri(
                            new Uri(Config.TellOPConfiguration.ServerBaseUrl + "/register")))
                });
            this.Privacy.GestureRecognizers.Add(
                new TapGestureRecognizer
                {
                    Command = new Command(() =>
                        Device.OpenUri(
                            new Uri(Config.TellOPConfiguration.ServerBaseUrl + "/privacy")))
                });
            this.About.GestureRecognizers.Add(
                new TapGestureRecognizer
                {
                    Command = new Command(() =>
                        Device.OpenUri(
                            new Uri("http://www.tellop.eu/about")))
                });
        }

        private void LoginButton_Clicked(object sender, EventArgs e)
        {
            this.Navigation.PushModalAsync(new Authentication());
        }
    }
}
