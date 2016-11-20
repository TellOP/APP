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
    using DataModels;
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
            this.BindingContext = new SearchDataModel();
            this.InitializeComponent();
            this.SearchListStands4.ItemTemplate = new DataTemplate(typeof(Stands4ViewCell));
            this.SearchListCollins.ItemTemplate = new DataTemplate(typeof(CollinsViewCell));

            this.BTNShowCollinsStack.Clicked += this.ShowCorrectPanel;
            this.BTNShowStands4Stack.Clicked += this.ShowCorrectPanel;
        }

        /// <summary>
        /// Event handler for the show/hide panels logic.
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Eventargs object</param>
        private void ShowCorrectPanel(object sender, System.EventArgs e)
        {
            // The buttons are in XOR, invert the status
            if (this.BTNShowStands4Stack.IsEnabled ^ this.BTNShowCollinsStack.IsEnabled)
            {
                this.EnableDefinitionsPanel(!this.Stands4Stack.IsVisible, !this.CollinsStack.IsVisible);
            }
            else
            {
                // Check the sender object
                if (sender == this.BTNShowStands4Stack)
                {
                    // Change the status of Stands4, leave the other unchanged
                    this.EnableDefinitionsPanel(!this.Stands4Stack.IsVisible, this.CollinsStack.IsVisible);
                }
                else
                {
                    // Change the status of Collins, leave the other unchanged
                    this.EnableDefinitionsPanel(this.Stands4Stack.IsVisible, !this.CollinsStack.IsVisible);
                }
            }
        }

        /// <summary>
        /// Hide or show the corresponding panel.
        /// </summary>
        /// <param name="stands4">If true, shows the stack and disable the button.</param>
        /// <param name="collins">If false, hide the stack and enable the button.</param>
        private void EnableDefinitionsPanel(bool stands4, bool collins)
        {
            this.Stands4Stack.IsVisible = stands4;
            this.BTNShowStands4Stack.IsEnabled = !stands4;

            this.CollinsStack.IsVisible = collins;
            this.BTNShowCollinsStack.IsEnabled = !collins;
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
                ((SearchDataModel)this.BindingContext).SearchForWord(searchBar.Text);

                // Hide both panels, enable the buttons
                this.EnableDefinitionsPanel(false, false);
            }
        }

        /// <summary>
        /// Called when the user taps on a result in one of the search result lists.
        /// </summary>
        /// <param name="sender">The object sending the event.</param>
        /// <param name="e">The event parameters.</param>
        private void SearchList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (((SearchDataModel)this.BindingContext).IsSearchEnabled)
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
