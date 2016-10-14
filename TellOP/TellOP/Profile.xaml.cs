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
            this.RetrieveUserInformation();

            this.refreshButton.Clicked += this.RefreshButton_Clicked;
            this.settingsButton.Clicked += this.SettingsButton_Clicked;

            this.oldPassEntry.Completed += (sender, e) =>
            {
                this.newPassEntry.Focus();
            };
            this.newPassEntry.Completed += (sender, e) =>
            {
                this.checkNewPassEntry.Focus();
            };
            this.checkNewPassEntry.Completed += (sender, e) =>
            {
                this.ChangePassword_Clicked(this.checkNewPassEntry, null);
            };
        }

        private void SettingsButton_Clicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void DashboardButton_Clicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void RefreshButton_Clicked(object sender, EventArgs e)
        {
            this.RetrieveUserInformation();
        }

        private void RetrieveUserInformation()
        {
            // FIXME
            throw new NotImplementedException();
        }

        private void OldPassEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.oldPassEntry.BackgroundColor = Color.Default;
            this.oldPassEntry.TextColor = Color.Default;
        }

        private void NewPassEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.newPassEntry.BackgroundColor = Color.Default;
            this.newPassEntry.TextColor = Color.Default;
        }

        private void CheckNewPassEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.checkNewPassEntry.BackgroundColor = Color.Default;
            this.checkNewPassEntry.TextColor = Color.Default;
        }

        private void ChangePassword_Clicked(object sender, EventArgs e)
        {
            this.errorPanel.Children.Clear();

            bool okOld = true;
            bool okNew = true;
            bool okNewCheck = true;

            if (string.IsNullOrEmpty(this.oldPassEntry.Text))
            {
                okOld = false;
                this.oldPassEntry.BackgroundColor = Color.FromHex("FF9B9B");
                this.oldPassEntry.TextColor = Color.Black;
                this.errorPanel.Children.Add(new Label
                {
                    Text = "You must insert the old password",
                    TextColor = Color.Red,
                    HorizontalTextAlignment = TextAlignment.Center
                });
                this.errorPanel.Focus();
            }

            if (string.IsNullOrEmpty(this.newPassEntry.Text))
            {
                okNew = false;
                this.newPassEntry.BackgroundColor = Color.FromHex("FF9B9B");
                this.newPassEntry.TextColor = Color.Black;
                this.errorPanel.Children.Add(new Label
                {
                    Text = "You must insert the new password",
                    TextColor = Color.Red,
                    HorizontalTextAlignment = TextAlignment.Center
                });
                this.errorPanel.Focus();
            }

            if (string.IsNullOrEmpty(this.checkNewPassEntry.Text))
            {
                okNewCheck = false;
                this.checkNewPassEntry.BackgroundColor = Color.FromHex("FF9B9B");
                this.checkNewPassEntry.TextColor = Color.Black;
                this.errorPanel.Children.Add(new Label
                {
                    Text = "You must insert the new password check",
                    TextColor = Color.Red,
                    HorizontalTextAlignment = TextAlignment.Center
                });
                this.errorPanel.Focus();
            }

            if (!okOld || !okNew || !okNewCheck)
            {
                return;
            }

            if (!this.checkNewPassEntry.Text.Equals(this.newPassEntry.Text))
            {
                this.checkNewPassEntry.BackgroundColor = Color.FromHex("FF9B9B");
                this.checkNewPassEntry.TextColor = Color.Black;
                this.errorPanel.Children.Add(new Label
                {
                    Text = "The new password doesn't match",
                    TextColor = Color.Red,
                    HorizontalTextAlignment = TextAlignment.Center
                });
                this.errorPanel.Focus();
                return;
            }

            // FIXME
        }
    }
}
