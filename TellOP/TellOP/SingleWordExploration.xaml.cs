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
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using API;
    using DataModels;
    using DataModels.APIModels.Collins;
    using DataModels.APIModels.Stands4;
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
            if (word is CollinsWord)
            {
                this.OpenCollins(null, null);
            }
            else if (word is Stands4Word)
            {
                this.OpenStands4(null, null);
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
                throw new ArgumentNullException("word");
            }

            this.searchedWord = word.ToUpper();

            this.InitializeComponent();

            this.Title = this.searchedWord;
            this.TitleLabel.Text = this.searchedWord;
        }

        private async void OpenStands4(object sender, System.EventArgs e)
        {
            if (this.Stands4Stack.Children.Count == 0)
            {
                await this._InitializeStands4Stack();
            }

            this.Stands4Stack.IsVisible = true;
            this.CollinsStack.IsVisible = false;
        }

        private void OpenCollins(object sender, System.EventArgs e)
        {
            if (this.CollinsStack.Children.Count == 0)
            {
                this._InitializeCollinsStack();
            }

            this.Stands4Stack.IsVisible = false;
            this.CollinsStack.IsVisible = true;
        }

        private async Task<bool> _InitializeStands4Stack()
        {
            try
            {
                this.Stands4Stack.Children.Add(new ActivityIndicator());
                Stands4Dictionary s4d = new Stands4Dictionary(App.OAuth2Account, this.searchedWord);
                IList<Stands4Word> result = await s4d.CallEndpointAsStands4Word();
                foreach (Stands4Word word in result)
                {
                    this.Stands4Stack.Children.Add(new Stands4SearchListItemView(word));
                }

                return true;
            }
            catch (UnsuccessfulAPICallException ex)
            {
                Tools.Logger.Log(this, "_InitializeStands4Stack method", ex);
            }
            catch (System.Exception ex)
            {
                Tools.Logger.Log(this, "_InitializeStands4Stack method", ex);
            }

            return false;
        }

        private bool _InitializeCollinsStack()
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                Tools.Logger.Log(this, "Collins Stack", ex);
                return false;
            }
        }
    }
}
