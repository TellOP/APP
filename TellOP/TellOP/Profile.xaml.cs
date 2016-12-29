// <copyright file="Profile.xaml.cs" company="University of Murcia">
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

namespace TellOP
{
    using System;
    using System.Threading.Tasks;
    using Xamarin.Forms;

    /// <summary>
    /// The application User Profile page.
    /// </summary>
    public partial class Profile : ContentPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Profile"/> class.
        /// </summary>
        public Profile()
        {
            this.InitializeComponent();

            //this.RetrieveUserInformation();

            this.refreshButton.Clicked += this.RefreshButton_Clicked;
            this.settingsButton.Clicked += this.SettingsButton_Clicked;
        }

        private async void SettingsButton_Clicked(object sender, EventArgs e)
        {
            await Tools.Logger.LogWithErrorMessage(this, "This feature is currently under development.", new NotImplementedException());
        }

        private async void DashboardButton_Clicked(object sender, EventArgs e)
        {
            await Tools.Logger.LogWithErrorMessage(this, "This feature is currently under development.", new NotImplementedException());
        }

        private async void RefreshButton_Clicked(object sender, EventArgs e)
        {
            await this.RetrieveUserInformation();
        }

        private async Task RetrieveUserInformation()
        {
            await Tools.Logger.LogWithErrorMessage(this, "This feature is currently under development.", new NotImplementedException());
        }
    }
}
