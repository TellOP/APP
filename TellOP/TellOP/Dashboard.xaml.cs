// <copyright file="Dashboard.xaml.cs" company="University of Murcia">
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
    /// Main application dashboard.
    /// </summary>
    public partial class Dashboard : TabbedPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Dashboard"/> class.
        /// </summary>
        public Dashboard()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Called when the "Profile" button is clicked.
        /// </summary>
        /// <param name="sender">The object sending the event.</param>
        /// <param name="e">The event parameters.</param>
        private async void ProfileButton_Clicked(object sender, EventArgs e)
        {
            await this.Navigation.PushAsync(new Profile());
        }

        /// <summary>
        /// Called when the "Refresh" button is clicked.
        /// </summary>
        /// <param name="sender">The object sending the event.</param>
        /// <param name="e">The event parameters.</param>
        private void RefreshButton_Clicked(object sender, EventArgs e)
        {
            if (this.CurrentPage is DashboardTabHistory)
            {
                ((DashboardTabHistory)this.CurrentPage).Refresh();
            }
            else if (this.CurrentPage is DashboardTabFeatured)
            {
                ((DashboardTabFeatured)this.CurrentPage).Refresh();
            }
        }

        /// <summary>
        /// Called when the "Search" button is clicked.
        /// </summary>
        /// <param name="sender">The object sending the event.</param>
        /// <param name="e">The event parameters.</param>
        private void SearchButton_Clicked(object sender, EventArgs e)
        {
            // TODO: why are two different kind of pushes used?
            Device.OnPlatform(
                iOS: () => this.Navigation.PushAsync(new MainSearch()),
                Default: () => this.Navigation.PushModalAsync(new MainSearch()));
        }

        /// <summary>
        /// Called when the "Settings" button is clicked.
        /// </summary>
        /// <param name="sender">The object sending the event.</param>
        /// <param name="e">The event parameters.</param>
        private async void SettingsButton_Clicked(object sender, EventArgs e)
        {
            // TODO: remove when implemented
            await this.DisplayAlert(Properties.Resources.Error, "The Settings panel will be added soon", Properties.Resources.ButtonOK);
        }
    }
}
