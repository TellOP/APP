// <copyright file="SingleWordExploration.xaml.cs" company="University of Murcia">
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
    using DataModels;
    using DataModels.ApiModels.Collins;
    using DataModels.ApiModels.Stands4;
    using ViewModels;
    using Xamarin.Forms;

    /// <summary>
    /// An app page detailing the meanings of a single word.
    /// </summary>
    public partial class SingleWordExploration : ContentPage
    {
        /// <summary>
        /// Word whose meanings are shown in the page.
        /// </summary>
        private string searchedWord;

        /// <summary>
        /// Initializes a new instance of the <see cref="SingleWordExploration"/> class.
        /// </summary>
        /// <param name="word">The word to analyze.</param>
        public SingleWordExploration(IWord word)
            : this(word.Term)
        {
            if (word == null)
            {
                return;
            }

            if (word is CollinsWord)
            {
                this.EnableDefinitionsPanel(false, true, false);
            }
            else if (word is Stands4Word)
            {
                this.EnableDefinitionsPanel(true, false, false);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SingleWordExploration"/> class.
        /// </summary>
        /// <param name="word">The word to analyze.</param>
        public SingleWordExploration(string word)
        {
            if (word == null)
            {
                throw new ArgumentNullException("Word cannot be null");
            }

            this.searchedWord = word.ToUpper();

            this.BindingContext = new SearchDataModel();
            this.InitializeComponent();
            this.SearchListStands4.ItemTemplate = new DataTemplate(typeof(Stands4ViewCell));
            this.SearchListCollins.ItemTemplate = new DataTemplate(typeof(CollinsViewCell));

            this.Title = this.searchedWord;

            this.BTNShowCollinsStack.Clicked += this.ShowCorrectPanel;
            this.BTNShowStands4Stack.Clicked += this.ShowCorrectPanel;
            this.BTNShowNetSpeakStack.Clicked += this.ShowCorrectPanel;

            ((SearchDataModel)this.BindingContext).SearchForWord(word);
        }

        /// <summary>
        /// Event handler for the show/hide panels logic.
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Eventargs object</param>
        private void ShowCorrectPanel(object sender, EventArgs e)
        {
            // Check the sender object
            if (sender == this.BTNShowStands4Stack)
            {
                // Change the status of Stands4, leave the other hidden
                this.EnableDefinitionsPanel(!this.Stands4Stack.IsVisible, false, false);
            }
            else if (sender == this.BTNShowCollinsStack)
            {
                // Change the status of Collins, leave the other hidden
                this.EnableDefinitionsPanel(false, !this.CollinsStack.IsVisible, false);
            }
            else if (sender == this.BTNShowNetSpeakStack)
            {
                // Change the status of Netspeak, leave the other hidden
                this.EnableDefinitionsPanel(false, false, !this.NetSpeakStack.IsVisible);
            }
        }

        /// <summary>
        /// Hide or show the corresponding panel.
        /// </summary>
        /// <param name="stands4">If true, shows the stack and disable the button.</param>
        /// <param name="collins">If false, hide the stack and enable the button.</param>
        /// <param name="netspeak">Same as the others</param>
        private void EnableDefinitionsPanel(bool stands4, bool collins, bool netspeak)
        {
            this.Stands4Stack.IsVisible = stands4;
            this.BTNShowStands4Stack.IsEnabled = !stands4;

            this.CollinsStack.IsVisible = collins;
            this.BTNShowCollinsStack.IsEnabled = !collins;

            this.NetSpeakStack.IsVisible = netspeak;
            this.BTNShowNetSpeakStack.IsEnabled = !netspeak;
        }

        /// <summary>
        /// Called when the user taps on a result in one of the search result lists.
        /// </summary>
        /// <param name="sender">The object sending the event.</param>
        /// <param name="e">The event parameters.</param>
        private void SearchList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }
    }
}
