// <copyright file="Stands4SearchListItemView.cs" company="University of Murcia">
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

namespace TellOP.DataModels.APIModels.Stands4
{
    using System.Globalization;
    using Enums;
    using TellOP.ViewModels;
    using Xamarin.Forms;

    /// <summary>
    /// TODO
    /// </summary>
    public class Stands4SearchListItemView : SearchListItemView
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
        /// Detail grid.
        /// </summary>
        private Grid detailsPanel;

        /// <summary>
        /// Initializes a new instance of the <see cref="Stands4SearchListItemView"/> class.
        /// </summary>
        /// <param name="word">Searched term</param>
        public Stands4SearchListItemView(Stands4Word word)
            : base(word)
        {
            this.ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) },
                new ColumnDefinition() { Width = new GridLength(4, GridUnitType.Star) },
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
        /// Build the basic grid with the definition preview.
        /// </summary>
        private void _builtPreview()
        {
            this.dictionaryLabel = new Label
            {
                Text = "Stands4",
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.Start,
                FontAttributes = FontAttributes.Bold,
                FontSize = 12d
            };
            this.dictionaryLabel.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => this._invertDetailsPanel()) });

            this.termLabel = new Label
            {
                Text = ((Stands4Word)this.Term).Term,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                FontAttributes = FontAttributes.Bold,
                FontSize = 14d
            };
            this.termLabel.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => this._invertDetailsPanel()) });
            string tmpFriendlyName = (string)new PartOfSpeechToStringConverter().Convert(((Stands4Word)this.Term).PartOfSpeech, typeof(string), null, CultureInfo.CurrentCulture);
            if (tmpFriendlyName.Length > 15)
            {
                tmpFriendlyName = tmpFriendlyName.Remove(15);
            }

            this.uIDLabel = new Label
            {
                Text = "[" + tmpFriendlyName + "]",
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.End,
                FontSize = 10d
            };
            this.uIDLabel.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => this._invertDetailsPanel()) });

            this.Children.Add(this.dictionaryLabel, 0, 0);
            this.Children.Add(this.termLabel, 1, 0);
            this.Children.Add(this.uIDLabel, 2, 0);
        }

        /// <summary>
        /// Build the internal panel with the definition details
        /// </summary>
        private void _builtDetails()
        {
            this.detailsPanel = new Grid
            {
                IsVisible = true,
                BackgroundColor = Color.FromHex("#64B5F6"),
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Auto }
                },
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto }
                }
            };

            this.detailsPanel.Children.Add(new Label { Text = "Definition", FontSize = 14, FontAttributes = FontAttributes.Bold }, 0, 0);
            this.detailsPanel.Children.Add(new Label { Text = ((Stands4Word)this.Term).Definition, FontSize = 14, LineBreakMode = LineBreakMode.WordWrap }, 1, 0);

            for (int i = 0; i < ((Stands4Word)this.Term).Examples.Length; i++)
            {
                this.detailsPanel.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                this.detailsPanel.Children.Add(new Label { Text = "Example", FontSize = 14, FontAttributes = FontAttributes.Bold }, 0, i + 1);
                this.detailsPanel.Children.Add(new Label { Text = ((Stands4Word)this.Term).Examples[i], FontSize = 14, LineBreakMode = LineBreakMode.WordWrap }, 1, i + 1);
            }

            this.Children.Add(this.detailsPanel, 0, 1);
            Grid.SetColumnSpan(this.detailsPanel, 3);
        }

        /// <summary>
        /// Manage the hide/show behaviour of the detail panel
        /// </summary>
        private void _invertDetailsPanel()
        {
            if (this.detailsPanel != null)
            {
                if (this.detailsPanel.IsVisible)
                {
                    this.detailsPanel.IsVisible = false;
                    this.detailsPanel.BackgroundColor = Color.Default;
                }
                else
                {
                    this.detailsPanel.IsVisible = true;
                    this.detailsPanel.BackgroundColor = Color.FromHex("#64B5F6");
                }
            }
            else
            {
                this._builtDetails();
            }
        }
    }
}
