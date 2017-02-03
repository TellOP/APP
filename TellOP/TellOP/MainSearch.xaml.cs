// <copyright file="MainSearch.xaml.cs" company="University of Murcia">
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
    using Api;
    using DataModels;
    using DataModels.Enums;
    using System;
    using Tools;
    using ViewModels;
    using Xamarin.Forms;

    /// <summary>
    /// Word search page.
    /// </summary>
    public partial class MainSearch : ContentPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainSearch"/> class.
        /// </summary>
        public MainSearch()
        {
            this.PreInitialize();
            this.InitializeComponent();
            this.PostInitialize();

            this.BTNShowDefinitions.Clicked += this.ShowCorrectPanel;
            this.BTNShowStringNetStack.Clicked += this.ShowCorrectPanel;
            this.settingsButton.Clicked += this.SettingsButton_Clicked;
            this.refreshButton.Clicked += this.RefreshButton_Clicked;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainSearch"/> class.
        /// </summary>
        /// <param name="term">Term to be searched</param>
        public MainSearch(string term)
        {
            this.PreInitialize();
            this.InitializeComponent();
            this.SearchBar.Text = term;
            this.PostInitialize();

            this.BTNShowDefinitions.Clicked += this.ShowCorrectPanel;
            this.BTNShowStringNetStack.Clicked += this.ShowCorrectPanel;
            this.settingsButton.Clicked += this.SettingsButton_Clicked;
            this.refreshButton.Clicked += this.RefreshButton_Clicked;

            this.SearchBar_SearchButtonPressed(this.SearchBar, null);
        }

        /// <summary>
        /// Pre-initialization process.
        /// </summary>
        private void PreInitialize()
        {
            switch (App.ActiveSearchLanguage)
            {
                case SupportedLanguage.Spanish:
                    {
                        this.BindingContext = new SpanishSearchDataModel();
                        break;
                    }

                case SupportedLanguage.German:
                    {
                        this.BindingContext = new GermanSearchDataModel();
                        break;
                    }

                case SupportedLanguage.English:
                case SupportedLanguage.USEnglish:
                default:
                    {
                        this.BindingContext = new EnglishSearchDataModel();
                        break;
                    }
            }
        }

        /// <summary>
        /// Post initialization process.
        /// </summary>
        private void PostInitialize()
        {
            switch (App.ActiveSearchLanguage)
            {
                case SupportedLanguage.Spanish:
                    {
                        this.SearchListOxford.ItemTemplate = new DataTemplate(typeof(OxfordViewCell));
                        this.SearchListStringNetBefore.ItemTemplate = new DataTemplate(typeof(StringNetCollocationsViewCell));
                        this.SearchListStringNetAfter.ItemTemplate = new DataTemplate(typeof(StringNetCollocationsViewCell));

                        this.DefinitionsStack.Children.Remove(this.Stands4ElementsGroup);
                        this.DefinitionsStack.Children.Remove(this.CollinsElementsGroup);

                        this.Stands4ElementsGroup.IsVisible = false;
                        this.Stands4ElementsGroup.IsEnabled = false;
                        this.CollinsElementsGroup.IsVisible = false;
                        this.CollinsElementsGroup.IsEnabled = false;

                        this.Title = Properties.Resources.MainSearch_Title + " " + Properties.Resources.Language_Spanish;
                        break;
                    }

                case SupportedLanguage.German:
                    {
                        this.SearchListCollins.ItemTemplate = new DataTemplate(typeof(CollinsViewCell));
                        this.SearchListStringNetBefore.ItemTemplate = new DataTemplate(typeof(StringNetCollocationsViewCell));
                        this.SearchListStringNetAfter.ItemTemplate = new DataTemplate(typeof(StringNetCollocationsViewCell));

                        this.DefinitionsStack.Children.Remove(this.OxfordElementsGroup);
                        this.DefinitionsStack.Children.Remove(this.Stands4ElementsGroup);

                        this.OxfordElementsGroup.IsVisible = false;
                        this.OxfordElementsGroup.IsEnabled = false;

                        this.Title = Properties.Resources.MainSearch_Title + " " + Properties.Resources.Language_German;
                        break;
                    }

                case SupportedLanguage.English:
                case SupportedLanguage.USEnglish:
                default:
                    {
                        this.SearchListStands4.ItemTemplate = new DataTemplate(typeof(Stands4ViewCell));
                        this.SearchListCollins.ItemTemplate = new DataTemplate(typeof(CollinsViewCell));
                        this.SearchListStringNetBefore.ItemTemplate = new DataTemplate(typeof(StringNetCollocationsViewCell));
                        this.SearchListStringNetAfter.ItemTemplate = new DataTemplate(typeof(StringNetCollocationsViewCell));

                        this.DefinitionsStack.Children.Remove(this.OxfordElementsGroup);

                        this.OxfordElementsGroup.IsVisible = false;
                        this.OxfordElementsGroup.IsEnabled = false;

                        this.Title = Properties.Resources.MainSearch_Title + " " + Properties.Resources.Language_English;
                        break;
                    }
            }
        }

        /// <summary>
        /// Settings button handler.
        /// </summary>
        /// <param name="sender">Button object</param>
        /// <param name="e">Event Args</param>
        private async void SettingsButton_Clicked(object sender, EventArgs e)
        {
            await this.Navigation.PushAsync(new SettingsPage());
            this.PreInitialize();
            this.PostInitialize();
        }

        /// <summary>
        /// Refresh event handler.
        /// </summary>
        /// <param name="sender">Button object</param>
        /// <param name="e">Event Args</param>
        private void RefreshButton_Clicked(object sender, EventArgs e)
        {
            this.PreInitialize();
            this.PostInitialize();
        }

        /// <summary>
        /// Event handler for the show/hide panels logic.
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Eventargs object</param>
        private void ShowCorrectPanel(object sender, System.EventArgs e)
        {
            // Check the sender object
            if (sender == this.BTNShowDefinitions)
            {
                // Change the status of Stands4, leave the other hidden
                this.EnableDefinitionsPanel(!this.DefinitionsStack.IsVisible, false);
            }
            else if (sender == this.BTNShowStringNetStack)
            {
                // Change the status of StringNet, leave the other hidden
                this.EnableDefinitionsPanel(false, !this.StringNetStack.IsVisible);
            }
        }

        /// <summary>
        /// Hide or show the corresponding panel.
        /// </summary>
        /// <param name="stands4">If true, shows the Stands4 stack and disable the button.</param>
        /// <param name="stringnet">If true, shows the StringNet stack and disable the button.</param>
        private void EnableDefinitionsPanel(bool stands4, bool stringnet)
        {
            this.DefinitionsStack.IsVisible = stands4;
            this.BTNShowDefinitions.IsEnabled = !stands4;

            this.StringNetStack.IsVisible = stringnet;
            this.BTNShowStringNetStack.IsEnabled = !stringnet;
        }

        /// <summary>
        /// Called when the user taps on "Search" in the search bar.
        /// </summary>
        /// <param name="sender">The object sending the event.</param>
        /// <param name="e">The event parameters.</param>
        private async void SearchBar_SearchButtonPressed(object sender, System.EventArgs e)
        {
            SearchBar searchBar = (SearchBar)sender;

            // Note: this warning might not appear on Android platforms where null/empty/whitespace strings are already
            // handled and refused by the component itself.
            if (string.IsNullOrWhiteSpace(searchBar.Text))
            {
                await this.DisplayAlert(Properties.Resources.Search_NoTermEntered_Title, Properties.Resources.Search_NoTermEntered_Text, Properties.Resources.ButtonOK);
                return;
            }

            if (await ConnectivityCheck.AskToEnableConnectivity(this))
            {
                ((ISearchDataModel)this.BindingContext).SearchForWord(searchBar.Text);

                // Hide all panels, enable the buttons
                this.EnableDefinitionsPanel(true, false);
            }

            this.StringNetAfterTitle.Text = string.Format(Properties.Resources.MainSearch_StringNet_CollocationsAfterTitle, searchBar.Text);
            this.StringNetBeforeTitle.Text = string.Format(Properties.Resources.MainSearch_StringNet_CollocationsBeforeTitle, searchBar.Text);
        }

        /// <summary>
        /// Called when the user taps on a result in one of the search result lists.
        /// </summary>
        /// <param name="sender">The object sending the event.</param>
        /// <param name="e">The event parameters.</param>
        private void SearchList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (((ISearchDataModel)this.BindingContext).IsSearchEnabled)
            {
                ((ListView)sender).SelectedItem = null;
            }
            else
            {
                // TODO: display alert?
            }
        }
    }
}
