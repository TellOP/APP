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
    using System;
    using System.Threading.Tasks;
    using DataModels.Enums;
    using Tools;
    using Xamarin.Forms;

    /// <summary>
    /// Word search page.
    /// </summary>
    public partial class MainSearch : TabbedPage
    {
        private SearchCollinsTab collinsTab;
        private SearchOxfordTab oxfordTab;
        private SearchStands4Tab stands4Tab;
        private SearchStringNetTab stringNetTab;

        // private SearchNetSpeakTab netSpeakTab;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainSearch"/> class.
        /// </summary>
        public MainSearch()
        {
            this.PreInitialize();
            this.InitializeComponent();
            this.PostInitialize();

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
            this.SetSearchTerm(term);
            this.PostInitialize();

            this.settingsButton.Clicked += this.SettingsButton_Clicked;
            this.refreshButton.Clicked += this.RefreshButton_Clicked;

            this.SearchBar_SearchButtonPressed(term, null);
        }

        /// <summary>
        /// Called when the user taps on "Search" in the search bar.
        /// </summary>
        /// <param name="sender">The object sending the event.</param>
        /// <param name="e">The event parameters.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task SearchBar_SearchButtonPressed(object sender, System.EventArgs e)
        {
            string term = string.Empty;
            if (sender is SearchBar)
            {
                term = ((SearchBar)sender).Text;
            }
            else if (sender is string)
            {
                term = (string)sender;
            }

            // Note: this warning might not appear on Android platforms where null/empty/whitespace strings are already
            // handled and refused by the component itself.
            if (string.IsNullOrWhiteSpace(term))
            {
                await this.DisplayAlert(Properties.Resources.Search_NoTermEntered_Title, Properties.Resources.Search_NoTermEntered_Text, Properties.Resources.ButtonOK);
                return;
            }

            if (await ConnectivityCheck.AskToEnableConnectivity(this))
            {
                switch (App.ActiveSearchLanguage)
                {
                    case SupportedLanguage.Spanish:
                        {
                            this.oxfordTab.SearchModel.SearchForWord(term);
                            this.stringNetTab.SearchModel.SearchForWord(term);

                            // this.netSpeakTab.SearchModel.SearchForWord(term);
                            break;
                        }

                    case SupportedLanguage.German:
                        {
                            this.collinsTab.SearchModel.SearchForWord(term);
                            this.stringNetTab.SearchModel.SearchForWord(term);

                            // this.netSpeakTab.SearchModel.SearchForWord(term);
                            break;
                        }

                    case SupportedLanguage.English:
                    case SupportedLanguage.USEnglish:
                    default:
                        {
                            this.collinsTab.SearchModel.SearchForWord(term);
                            this.stands4Tab.SearchModel.SearchForWord(term);
                            this.stringNetTab.SearchModel.SearchForWord(term);

                            // this.netSpeakTab.SearchModel.SearchForWord(term);
                            break;
                        }
                }
            }
        }

        /// <summary>
        /// Pre-initialization process.
        /// </summary>
        private void PreInitialize()
        {
            this.Children.Clear();

            switch (App.ActiveSearchLanguage)
            {
                case SupportedLanguage.Spanish:
                    {
                        this.oxfordTab = new SearchOxfordTab(this);
                        this.stringNetTab = new SearchStringNetTab(this);

                        // this.netSpeakTab = new SearchNetSpeakTab(this);
                        break;
                    }

                case SupportedLanguage.German:
                    {
                        this.collinsTab = new SearchCollinsTab(this);
                        this.stringNetTab = new SearchStringNetTab(this);

                        // this.netSpeakTab = new SearchNetSpeakTab(this);
                        break;
                    }

                case SupportedLanguage.English:
                case SupportedLanguage.USEnglish:
                default:
                    {
                        this.collinsTab = new SearchCollinsTab(this);
                        this.stands4Tab = new SearchStands4Tab(this);
                        this.stringNetTab = new SearchStringNetTab(this);

                        // this.netSpeakTab = new SearchNetSpeakTab(this);
                        break;
                    }
            }
        }

        /// <summary>
        /// Update search term process.
        /// </summary>
        /// <param name="term">Search term</param>
        private void SetSearchTerm(string term)
        {
            switch (App.ActiveSearchLanguage)
            {
                case SupportedLanguage.Spanish:
                    {
                        this.oxfordTab.Search.Text = term;
                        this.stringNetTab.Search.Text = term;

                        // this.netSpeakTab.Search.Text = term;
                        break;
                    }

                case SupportedLanguage.German:
                    {
                        this.collinsTab.Search.Text = term;
                        this.stringNetTab.Search.Text = term;

                        // this.netSpeakTab.Search.Text = term;
                        break;
                    }

                case SupportedLanguage.English:
                case SupportedLanguage.USEnglish:
                default:
                    {
                        this.collinsTab.Search.Text = term;
                        this.stands4Tab.Search.Text = term;
                        this.stringNetTab.Search.Text = term;

                        // this.netSpeakTab.Search.Text = term;
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
                        this.Children.Add(this.oxfordTab);
                        this.Children.Add(this.stringNetTab);

                        // this.Children.Add(this.netSpeakTab);

                        this.Title = Properties.Resources.MainSearch_Title + " " + Properties.Resources.Language_Spanish;
                        break;
                    }

                case SupportedLanguage.German:
                    {
                        this.Children.Add(this.collinsTab);
                        this.Children.Add(this.stringNetTab);

                        // this.Children.Add(this.netSpeakTab);

                        this.Title = Properties.Resources.MainSearch_Title + " " + Properties.Resources.Language_German;
                        break;
                    }

                case SupportedLanguage.English:
                case SupportedLanguage.USEnglish:
                default:
                    {
                        this.Children.Add(this.collinsTab);
                        this.Children.Add(this.stands4Tab);
                        this.Children.Add(this.stringNetTab);

                        // this.Children.Add(this.netSpeakTab);

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
    }
}
