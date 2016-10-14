// <copyright file="CollinsEntriesWrapper.cs" company="University of Murcia">
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

namespace TellOP.ViewModels.Collins
{
    using System;
    using System.Threading.Tasks;
    using DataModels.APIModels.Collins;
    using Xamarin.Forms;

    /// <summary>
    /// Rendering CollinsWords
    /// </summary>
    public class CollinsEntriesWrapper : Grid
    {
        /// <summary>
        /// Bindable property for dictionary label.
        /// </summary>
        private Label dictionaryLabel;

        /// <summary>
        /// Bindable property for term label.
        /// </summary>
        private Label termLabel;

        /// <summary>
        /// Bindable property for uid label.
        /// </summary>
        private Label uIDLabel;

        /// <summary>
        /// Detail panel.
        /// </summary>
        private StackLayout detailsPanel;

        private CollinsJSONEnglishDictionarySingleResult entry;

        private ActivityIndicator activityIndicator;

        /// <summary>
        /// Initializes a new instance of the <see cref="CollinsEntriesWrapper"/> class.
        /// </summary>
        /// <param name="singleResult">Word object</param>
        public CollinsEntriesWrapper(CollinsJSONEnglishDictionarySingleResult singleResult)
        {
            this.BackgroundColor = Color.Yellow;
            this.Padding = 1;
            this.Margin = 1;
            this.entry = singleResult;
            this.HorizontalOptions = LayoutOptions.FillAndExpand;

            this.ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(4, GridUnitType.Star) },
                new ColumnDefinition() { Width = new GridLength(2, GridUnitType.Star) },
            };
            this.RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition() { Height = GridLength.Auto },
                new RowDefinition() { Height = GridLength.Auto }
            };

            this._builtPreview();
        }

        /// <summary>
        /// Build the basic grid with the definition preview
        /// </summary>
        public void _builtPreview()
        {
            this.dictionaryLabel = new Label
            {
                Text = "Collins",
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.Start,
                FontAttributes = FontAttributes.Bold,
                FontSize = 12d
            };
            this.dictionaryLabel.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => this._invertDetailsPanel()) });

            this.termLabel = new Label
            {
                Text = this.entry.EntryLabel,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                FontAttributes = FontAttributes.Bold,
                FontSize = 14d
            };
            this.termLabel.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => this._invertDetailsPanel()) });

            this.uIDLabel = new Label
            {
                Text = "[ID: " + this.entry.EntryID + "]",
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.End,
                FontSize = 10d
            };
            this.uIDLabel.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => this._invertDetailsPanel()) });

            this.activityIndicator = new ActivityIndicator()
            {
                IsVisible = false,
                HeightRequest = 10,
                IsRunning = false
            };

            this.Children.Add(this.dictionaryLabel, 0, 0);
            this.Children.Add(this.termLabel, 1, 0);
            this.Children.Add(this.uIDLabel, 2, 0);
            this.Children.Add(this.activityIndicator, 2, 0);
        }

        public async Task<bool> _builtDetails()
        {
            try
            {
                this.SwitchActivityIndicator(true);
                this.detailsPanel = new CollinsMultipleWordsWrapper(this.entry.EntryID);
                this.detailsPanel.HorizontalOptions = LayoutOptions.FillAndExpand;
                this.detailsPanel.VerticalOptions = LayoutOptions.StartAndExpand;
                this.detailsPanel.Padding = 1;
                this.detailsPanel.Margin = 1;
                this.detailsPanel.BackgroundColor = Color.Green;

                await ((CollinsMultipleWordsWrapper)this.detailsPanel).Populate();
                this.Children.Add(this.detailsPanel, 0, 1);
                Grid.SetColumnSpan(this.detailsPanel, 3);
                this.SwitchActivityIndicator(false);
                return true;
            }
            catch (Exception ex)
            {
                Tools.Logger.Log(this, "_builtDetails method", ex);
                this.SwitchActivityIndicator(false);
                return false;
            }
        }

        /// <summary>
        /// Turn on/off the activity indicator
        /// </summary>
        /// <param name="status">True = on</param>
        public void SwitchActivityIndicator(bool status)
        {
            this.uIDLabel.IsVisible = !status;
            this.activityIndicator.IsRunning = status;
            this.activityIndicator.IsVisible = status;
        }

        /// <summary>
        /// Manage the hide/show behaviour of the detail panel
        /// </summary>
        private async void _invertDetailsPanel()
        {
            if (this.detailsPanel != null)
            {
                if (this.detailsPanel.IsVisible)
                {
                    this.detailsPanel.IsVisible = false;
                }
                else
                {
                    this.detailsPanel.IsVisible = true;
                }
            }
            else
            {
                await this._builtDetails();
            }
        }
    }
}
