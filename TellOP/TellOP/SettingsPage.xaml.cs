// <copyright file="SettingsPage.xaml.cs" company="University of Murcia">
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
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DataModels.Enums;
    using Xamarin.Forms;

    /// <summary>
    /// The application User Profile page.
    /// </summary>
    public partial class SettingsPage : ContentPage
    {

        /// <summary>
        /// If True will hide the menu.
        /// </summary>
        private bool HideMenu = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsPage"/> class.
        /// </summary>
        public SettingsPage()
        {
            this.InitializeComponent();

            this.SetUpButtons();

            this.refreshButton.Clicked += this.RefreshButton_Clicked;
            this.profileButton.Clicked += this.Profile_Clicked;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsPage"/> class.
        /// </summary>
        public SettingsPage(bool hideMenu)
        {
            this.HideMenu = hideMenu;
            this.InitializeComponent();

            this.SetUpButtons();

            this.refreshButton.Clicked += this.RefreshButton_Clicked;
            this.profileButton.Clicked += this.Profile_Clicked;
        }

        /// <summary>
        /// Initializes the buttons.
        /// </summary>
        private void SetUpButtons()
        {
            this.ToggleButtons();

            this.SwitchEnglish.Toggled += this.SwitchEnglish_Toggled;
            this.SwitchFrench.Toggled += this.SwitchFrench_Toggled;
            this.SwitchGerman.Toggled += this.SwitchGerman_Toggled;
            this.SwitchSpanish.Toggled += this.SwitchSpanish_Toggled;
            this.SwitchItalian.Toggled += this.SwitchItalian_Toggled;

            this.SelectedLanguage.Items.Clear();
            this.SelectedLanguage.Items.Add(Properties.Resources.Language_English);
            this.SelectedLanguage.Items.Add(Properties.Resources.Language_Spanish);
            this.SelectedLanguage.Items.Add(Properties.Resources.Language_German);
            this.SelectedLanguage.SelectedIndex = this.SelectedLanguage.Items.IndexOf(App.ActiveSearchLanguage.ToString());

            this.SelectedLanguage.SelectedIndexChanged += this.SelectedLanguage_SelectedIndexChanged;
        }

        /// <summary>
        /// Event Handler for the language selector
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arg</param>
        private void SelectedLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((Picker)sender).Items[((Picker)sender).SelectedIndex] == Properties.Resources.Language_English)
            {
                ((App)App.Current).ChangeSelectedLanguage(SupportedLanguage.English);
            }
            else if (((Picker)sender).Items[((Picker)sender).SelectedIndex] == Properties.Resources.Language_Spanish)
            {
                ((App)App.Current).ChangeSelectedLanguage(SupportedLanguage.Spanish);
            }
            else if (((Picker)sender).Items[((Picker)sender).SelectedIndex] == Properties.Resources.Language_German)
            {
                ((App)App.Current).ChangeSelectedLanguage(SupportedLanguage.German);
            }
            else if (((Picker)sender).Items[((Picker)sender).SelectedIndex] == Properties.Resources.Language_French)
            {
                ((App)App.Current).ChangeSelectedLanguage(SupportedLanguage.French);
            }
            else if (((Picker)sender).Items[((Picker)sender).SelectedIndex] == Properties.Resources.Language_Italian)
            {
                ((App)App.Current).ChangeSelectedLanguage(SupportedLanguage.Italian);
            }
        }

        /// <summary>
        /// Toogle all the buttons in the correct status.
        /// </summary>
        private void ToggleButtons()
        {
            this.SwitchEnglish.IsToggled = App.ActiveLanguages[SupportedLanguage.English];
            this.SwitchSpanish.IsToggled = App.ActiveLanguages[SupportedLanguage.Spanish];
            this.SwitchGerman.IsToggled = App.ActiveLanguages[SupportedLanguage.German];
            this.SwitchFrench.IsToggled = App.ActiveLanguages[SupportedLanguage.French];
            this.SwitchItalian.IsToggled = App.ActiveLanguages[SupportedLanguage.Italian];
        }

        /// <summary>
        /// Langugage handler for Spanish
        /// </summary>
        /// <param name="sender">Spanish switch</param>
        /// <param name="e">Event arg</param>
        private async void SwitchSpanish_Toggled(object sender, ToggledEventArgs e)
        {
            if (!((Switch)sender).IsToggled)
            {
                // If TRUE, Spanish is the only active language
                bool cannotDisable = !App.ActiveLanguages[SupportedLanguage.English]
                                  && !App.ActiveLanguages[SupportedLanguage.German]
                                  && !App.ActiveLanguages[SupportedLanguage.French]
                                  && !App.ActiveLanguages[SupportedLanguage.Italian];

                if (cannotDisable)
                {
                    bool answer = await this.DisplayAlert(
                        Properties.Resources.SettingPageQuestionTitle,
                        string.Format(Properties.Resources.SettingPageQuestionMessage, Properties.Resources.Language_Spanish, Properties.Resources.Language_English),
                        Properties.Resources.ButtonOK,
                        Properties.Resources.ButtonCancel);

                    if (answer)
                    {
                        ((App)App.Current).ToggleActiveLanguage(SupportedLanguage.English, true);
                        ((App)App.Current).ToggleActiveLanguage(SupportedLanguage.Spanish, ((Switch)sender).IsToggled);
                        this.ToggleButtons();
                    }
                }
                else
                {
                    ((App)App.Current).ToggleActiveLanguage(SupportedLanguage.Spanish, ((Switch)sender).IsToggled);
                }
            }
            else
            {
                // Enable
                ((App)App.Current).ToggleActiveLanguage(SupportedLanguage.Spanish, ((Switch)sender).IsToggled);
            }
        }

        /// <summary>
        /// Langugage handler for German
        /// </summary>
        /// <param name="sender">German switch</param>
        /// <param name="e">Event arg</param>
        private async void SwitchGerman_Toggled(object sender, ToggledEventArgs e)
        {
            if (!((Switch)sender).IsToggled)
            {
                // If TRUE, Spanish is the only active language
                bool cannotDisable = !App.ActiveLanguages[SupportedLanguage.English]
                                  && !App.ActiveLanguages[SupportedLanguage.Spanish]
                                  && !App.ActiveLanguages[SupportedLanguage.French]
                                  && !App.ActiveLanguages[SupportedLanguage.Italian];
                if (cannotDisable)
                {
                    bool answer = await this.DisplayAlert(
                    Properties.Resources.SettingPageQuestionTitle,
                    string.Format(Properties.Resources.SettingPageQuestionMessage, Properties.Resources.Language_German, Properties.Resources.Language_English),
                    Properties.Resources.ButtonOK,
                    Properties.Resources.ButtonCancel);

                    if (answer)
                    {
                        ((App)App.Current).ToggleActiveLanguage(SupportedLanguage.English, true);
                        ((App)App.Current).ToggleActiveLanguage(SupportedLanguage.German, ((Switch)sender).IsToggled);
                        this.ToggleButtons();
                    }
                    else
                    {
                        ((App)App.Current).ToggleActiveLanguage(SupportedLanguage.German, ((Switch)sender).IsToggled);
                    }
                }
                else
                {
                    ((App)App.Current).ToggleActiveLanguage(SupportedLanguage.German, ((Switch)sender).IsToggled);
                }
            }
            else
            {
                // Enable
                ((App)App.Current).ToggleActiveLanguage(SupportedLanguage.German, ((Switch)sender).IsToggled);
            }
        }

        /// <summary>
        /// Langugage handler for French
        /// </summary>
        /// <param name="sender">French switch</param>
        /// <param name="e">Event arg</param>
        private async void SwitchFrench_Toggled(object sender, ToggledEventArgs e)
        {
            if (!((Switch)sender).IsToggled)
            {
                // If TRUE, Spanish is the only active language
                bool cannotDisable = !App.ActiveLanguages[SupportedLanguage.English]
                                  && !App.ActiveLanguages[SupportedLanguage.German]
                                  && !App.ActiveLanguages[SupportedLanguage.Spanish]
                                  && !App.ActiveLanguages[SupportedLanguage.Italian];
                if (cannotDisable)
                {
                    bool answer = await this.DisplayAlert(
                    Properties.Resources.SettingPageQuestionTitle,
                    string.Format(Properties.Resources.SettingPageQuestionMessage, Properties.Resources.Language_French, Properties.Resources.Language_English),
                    Properties.Resources.ButtonOK,
                    Properties.Resources.ButtonCancel);

                    if (answer)
                    {
                        ((App)App.Current).ToggleActiveLanguage(SupportedLanguage.English, true);
                        ((App)App.Current).ToggleActiveLanguage(SupportedLanguage.French, ((Switch)sender).IsToggled);
                        this.ToggleButtons();
                    }
                    else
                    {
                        ((App)App.Current).ToggleActiveLanguage(SupportedLanguage.French, ((Switch)sender).IsToggled);
                    }
                }
                else
                {
                    ((App)App.Current).ToggleActiveLanguage(SupportedLanguage.French, ((Switch)sender).IsToggled);
                }
            }
            else
            {
                // Enable
                ((App)App.Current).ToggleActiveLanguage(SupportedLanguage.French, ((Switch)sender).IsToggled);
            }
        }

        /// <summary>
        /// Langugage handler for English
        /// </summary>
        /// <param name="sender">English switch</param>
        /// <param name="e">Event arg</param>
        private async void SwitchEnglish_Toggled(object sender, ToggledEventArgs e)
        {
            if (!((Switch)sender).IsToggled)
            {
                // If TRUE, Spanish is the only active language
                bool cannotDisable = !App.ActiveLanguages[SupportedLanguage.Italian]
                                  && !App.ActiveLanguages[SupportedLanguage.German]
                                  && !App.ActiveLanguages[SupportedLanguage.French]
                                  && !App.ActiveLanguages[SupportedLanguage.Spanish];

                if (cannotDisable)
                {
                    await this.DisplayAlert(
                        Properties.Resources.SettingPageQuestionTitle,
                        string.Format(Properties.Resources.SettingPageWarningDefaultLanguage, Properties.Resources.Language_English),
                        Properties.Resources.ButtonOK);
                    this.ToggleButtons();
                }
                else
                {
                    ((App)App.Current).ToggleActiveLanguage(SupportedLanguage.English, ((Switch)sender).IsToggled);
                }
            }
            else
            {
                // Enable
                ((App)App.Current).ToggleActiveLanguage(SupportedLanguage.English, ((Switch)sender).IsToggled);
            }
        }

        /// <summary>
        /// Langugage handler for Italian
        /// </summary>
        /// <param name="sender">Italian switch</param>
        /// <param name="e">Event arg</param>
        private async void SwitchItalian_Toggled(object sender, ToggledEventArgs e)
        {
            if (!((Switch)sender).IsToggled)
            {
                // If TRUE, Spanish is the only active language
                bool cannotDisable = !App.ActiveLanguages[SupportedLanguage.English]
                                  && !App.ActiveLanguages[SupportedLanguage.German]
                                  && !App.ActiveLanguages[SupportedLanguage.French]
                                  && !App.ActiveLanguages[SupportedLanguage.Spanish];
                if (cannotDisable)
                {
                    bool answer = await this.DisplayAlert(
                    Properties.Resources.SettingPageQuestionTitle,
                    string.Format(Properties.Resources.SettingPageQuestionMessage, Properties.Resources.Language_Italian, Properties.Resources.Language_English),
                    Properties.Resources.ButtonOK,
                    Properties.Resources.ButtonCancel);

                    if (answer)
                    {
                        ((App)App.Current).ToggleActiveLanguage(SupportedLanguage.English, true);
                        ((App)App.Current).ToggleActiveLanguage(SupportedLanguage.Italian, ((Switch)sender).IsToggled);
                        this.ToggleButtons();
                    }
                }
                else
                {
                    ((App)App.Current).ToggleActiveLanguage(SupportedLanguage.Italian, ((Switch)sender).IsToggled);
                }
            }
            else
            {
                // Enable
                ((App)App.Current).ToggleActiveLanguage(SupportedLanguage.Italian, ((Switch)sender).IsToggled);
            }
        }

        /// <summary>
        /// Langugage handler for the profile button
        /// </summary>
        /// <param name="sender">Button object</param>
        /// <param name="e">Event arg</param>
        private async void Profile_Clicked(object sender, EventArgs e)
        {
            if (this.HideMenu)
            {
                Tools.Logger.LogWithErrorMessage(this, "You must perform the login before using this function.", new UnauthorizedAccessException("You must perform the login before using this function"));
            }
            else
            {
                await this.DisplayAlert(Properties.Resources.Error, Properties.Resources.Dashboard_ProfileAddedSoon, Properties.Resources.ButtonOK);
            }
        }

        /// <summary>
        /// Langugage handler for the dashboard button
        /// </summary>
        /// <param name="sender">Button object</param>
        /// <param name="e">Event arg</param>
        private void DashboardButton_Clicked(object sender, EventArgs e)
        {
            if (this.HideMenu)
            {
                Tools.Logger.LogWithErrorMessage(this, "You must perform the login before using this function.", new UnauthorizedAccessException("You must perform the login before using this function"));
            }
            else
            {
                this.Navigation.PopAsync();
            }
        }

        /// <summary>
        /// Langugage handler for the refresh button
        /// </summary>
        /// <param name="sender">Button object</param>
        /// <param name="e">Event arg</param>
        private void RefreshButton_Clicked(object sender, EventArgs e)
        {
            ((App)App.Current).ReloadLanguagesFromProperties();
        }
    }
}
