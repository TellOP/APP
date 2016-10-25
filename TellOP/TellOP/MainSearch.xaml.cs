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
    using System.Collections.Generic;
    using Api;
    using DataModels.APIModels.Collins;
    using DataModels.APIModels.Stands4;
    using ViewModels.Collins;
    using Xamarin.Forms;

    /// <summary>
    /// Main search page.
    /// </summary>
    public partial class MainSearch : ContentPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainSearch"/> class.
        /// </summary>
        public MainSearch()
        {
            this.InitializeComponent();
            this.searchBar.SearchButtonPressed +=
                this.SearchBar_SearchButtonPressed;
        }

        /// <summary>
         /// Turn on/off the activity indicator
         /// </summary>
         /// <param name="running">True = on</param>
        public void SwitchActivityIndicator(bool running)
        {
            this.AI.IsRunning = running;
            this.AI.IsVisible = running;
            if (running)
            {
                Grid.SetColumn(this.searchBar, 1);
                Grid.SetColumnSpan(this.searchBar, 1);
            }
            else
            {
                Grid.SetColumn(this.searchBar, 0);
                Grid.SetColumnSpan(this.searchBar, 2);
            }
        }

        /// <summary>
        /// Event handler for search button
        /// </summary>
        /// <param name="sender">Search bar sender object</param>
        /// <param name="e">EventArgs param</param>
        private async void SearchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            string word = ((SearchBar)sender).Text;
            this.MainStack.Children.Clear();
            try
            {
                this.SwitchActivityIndicator(true);
                Stands4Dictionary s4d = new Stands4Dictionary(App.OAuth2Account, word);
                IList<Stands4Word> s4wResult = await s4d.CallEndpointAsStands4Word().ConfigureAwait(false);
                foreach (Stands4Word w in s4wResult)
                {
                    this.MainStack.Children.Add(new Stands4SearchListItemView(w));
                }

                this.SwitchActivityIndicator(false);
            }
            catch (UnsuccessfulApiCallException ex)
            {
                Tools.Logger.Log(this.GetType().ToString(), "Stands4 Exception", ex);
                this.SwitchActivityIndicator(false);
            }
            catch (Exception ex)
            {
                Tools.Logger.Log(this.GetType().ToString(), "Stands4 Exception", ex);
                this.SwitchActivityIndicator(false);
            }

            try
            {
                this.SwitchActivityIndicator(true);
                CollinsEnglishDictionary dictionaryAPI = new CollinsEnglishDictionary(App.OAuth2Account, word);
                CollinsJsonEnglishDictionary result = await dictionaryAPI.CallEndpointAsObjectAsync().ConfigureAwait(false);

                // Create a panel, but avoid the remote call
                foreach (CollinsJsonEnglishDictionarySingleResult entry in result.Results)
                {
                    this.MainStack.Children.Add(new CollinsEntriesWrapper(entry));
                }

                this.SwitchActivityIndicator(false);
            }
            catch (UnsuccessfulApiCallException ex)
            {
                Tools.Logger.Log(this.GetType().ToString(), "Collins Exception", ex);
                this.SwitchActivityIndicator(false);
            }
            catch (Exception ex)
            {
                Tools.Logger.Log(this.GetType().ToString(), "Collins Exception", ex);
                this.SwitchActivityIndicator(false);
            }
        }
    }
}
