// <copyright file="About.xaml.cs" company="University of Murcia">
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

namespace TellOP
{
    using System;
    using System.Globalization;
    using Version.Plugin;
    using Xamarin.Forms;

    /// <summary>
    /// The About window for the application.
    /// </summary>
    public partial class About : ContentPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="About"/> class.
        /// </summary>
        public About()
        {
            this.InitializeComponent();
            this.VersionLabel.Text = string.Format(CultureInfo.CurrentUICulture, Properties.Resources.About_Version, CrossVersion.Current.Version);
        }

        /// <summary>
        /// Called when the "Third party licenses" button is clicked.
        /// </summary>
        /// <param name="sender">The object sending the event.</param>
        /// <param name="e">The event parameters.</param>
        private async void ThirdPartyLicensesButton_Clicked(object sender, EventArgs e)
        {
            await this.Navigation.PushAsync(new ThirdPartyLicenses());
        }
    }
}
