// <copyright file="LanguageLevelWordsListView.cs" company="University of Murcia">
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

namespace TellOP.ViewModels
{
    using System;
    using System.Collections.Generic;
    using DataModels;
    using DataModels.Enums;
    using Xamarin.Forms;

    /// <summary>
    /// Words' list render as <see cref="Frame"/> element.
    /// </summary>
    public class LanguageLevelWordsListView : Frame
    {
        /// <summary>
        /// Gets or sets the internal frame with the words list
        /// </summary>
        private Grid _panel;

        /// <summary>
        /// Relative <see cref="LanguageLevelClassification"/> for the panel.
        /// </summary>
        private LanguageLevelClassification _level;

        private StackLayout _parentPanel;

        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageLevelWordsListView"/> class.
        /// </summary>
        /// <param name="level">Language level identifier</param>
        /// <param name="words">List of <see cref="IWord"/> objects</param>
        /// <param name="parent">The parent stack layout.</param>
        public LanguageLevelWordsListView(LanguageLevelClassification level, IList<IWord> words, StackLayout parent)
        {
            this._parentPanel = parent;
            this._level = level;
            this._buildPanel();
            this.TapGestureRecFromInnerFrameVisibilityInverter = new TapGestureRecognizer();

            this.Content = this._panel;
            this.Padding = 0;
            this.Margin = 0;

            this.TapGestureRecFromInnerFrameVisibilityInverter.Tapped += (s, e) =>
            {
                this.InnerFrameVisibilityInverter(!this._panel.IsVisible);
            };
            this.Populate(words);
        }

        /// <summary>
        /// Gets the gesture recognizer for the tap event.
        /// </summary>
        public TapGestureRecognizer TapGestureRecFromInnerFrameVisibilityInverter { get; private set; }

        /// <summary>
        /// Gets Main words' list
        /// </summary>
        public IList<IWord> Words { get; private set; }

        /// <summary>
        /// Asynchronously update the content of the InnerFrame. FIXME: Apparently the words are not visible
        /// </summary>
        /// <param name="words">Word List</param>
        public void Populate(IList<IWord> words)
        {
            if (words == null)
            {
                throw new ArgumentNullException("words");
            }

            this.Words = words;
            this._panel.Children.Clear();

            for (int i = 0; i < words.Count; i++)
            {
                Label tmpLabel = new Label()
                {
                    Text = words[i].Term,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.Center
                };
                tmpLabel.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => this.OnWordClicked(tmpLabel)) });
                this._panel.Children.Add(
                    tmpLabel,
                    i % 3,
                    i / 3);
            }
        }

        /// <summary>
        /// Force the status of the inner grid according to the param.
        /// </summary>
        /// <param name="forceStatus">If true, shows the words list grid.</param>
        public void InnerFrameVisibilityInverter(bool forceStatus)
        {
            if (this._panel == null)
            {
                this._buildPanel();
            }

            if (!forceStatus)
            {
                this._parentPanel.IsVisible = false;
                this.BackgroundColor = Color.Default;
            }
            else
            {
                this._parentPanel.IsVisible = true;
                this._parentPanel.Focus();
            }
        }

        /// <summary>
        /// Initialize the inner frame
        /// </summary>
        private void _buildPanel()
        {
            this._panel = new Grid()
            {
                RowDefinitions =
                {
                    new RowDefinition() { Height = GridLength.Auto }
                },
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Start,
                IsVisible = true,
                Margin = 0,
                Padding = 2
            };
            this._panel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            this._panel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            this._panel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        }

        /// <summary>
        /// Event handler for word's click
        /// </summary>
        /// <param name="sender">Label with a word element</param>
        private async void OnWordClicked(Label sender)
        {
            await this.Navigation.PushModalAsync(new SingleWordExploration(sender.Text));
        }
    }
}
