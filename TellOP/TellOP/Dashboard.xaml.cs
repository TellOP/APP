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
        private DashboardTabHistory _historyTab;
        private DashboardTabFeatured _featuredTab;

        /// <summary>
        /// Initializes a new instance of the <see cref="Dashboard"/> class.
        /// </summary>
        public Dashboard()
        {
            this.InitializeComponent();

            this._historyTab = new DashboardTabHistory()
            {
                Title = TellOP.Properties.Resources.Dashboard_TabHistory,
                Icon = "TAB_History.png"
            };

            this._featuredTab = new DashboardTabFeatured()
            {
                Title = TellOP.Properties.Resources.Dashboard_TabFeatured,
                Icon = "TAB_Featured.png"
            };

            this.Children.Add(this._featuredTab);
            this.Children.Add(this._historyTab);

            /*
            this.Children.Add(new ContentPage()
            {
                Title = TellOP.Properties.Resources.Dashboard_TabStats,
                Icon = "TAB_Stats.png"
            });
            */

            this.refreshButton.Icon = "toolbar_refresh.png";
            this.searchButton.Icon = "toolbar_search.png";

            this.refreshButton.Clicked += this.RefreshButton_Clicked;
            this.searchButton.Clicked += this.SearchButton_Clicked;
            this.profileButton.Clicked += this.ProfileButton_Clicked;
            this.settingsButton.Clicked += this.SettingsButton_Clicked;
        }

        private void SearchButton_Clicked(object sender, EventArgs e)
        {
            Device.OnPlatform(
                iOS: () => this.Navigation.PushAsync(new MainSearch()),
                Default: () => this.Navigation.PushModalAsync(new MainSearch()));
        }

        private async void SettingsButton_Clicked(object sender, EventArgs e)
        {
            await Tools.Logger.LogWithErrorMessage(this, string.Empty, new NotImplementedException("Settings are not supported yet."));
        }

        private void RefreshButton_Clicked(object sender, EventArgs e)
        {
            if (this.CurrentPage is DashboardTabHistory)
            {
                this._historyTab.Refresh();
            }

            if (this.CurrentPage is DashboardTabFeatured)
            {
                this._featuredTab.Refresh();
            }
        }

        private async void ProfileButton_Clicked(object sender, EventArgs e)
        {
            await this.Navigation.PushAsync(new Profile());
        }
    }
}
