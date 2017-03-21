// <copyright file="OxfordViewCell.xaml.cs" company="University of Murcia">
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
    using DataModels.ApiModels;
    using Xamarin.Forms;

    /// <summary>
    /// A <see cref="ViewCell"/> representing a <see cref="OxfordWord"/>.
    /// </summary>
    public partial class OxfordViewCell : ViewCell
    {
        /// <summary>
        /// Background colors for the alternate rows of the grid
        /// </summary>
        private static Color[] backgroundColors =
        {
            Color.FromHex("#FFEBEE"),
            Color.FromHex("#FCE4EC"),
            Color.FromHex("#F3E5F5"),
            Color.FromHex("#EDE7F6"),
            Color.FromHex("#E8EAF6"),
            Color.FromHex("#E3F2FD"),
            Color.FromHex("#E1F5FE"),
            Color.FromHex("#E0F7FA"),
            Color.FromHex("#E0F2F1"),
            Color.FromHex("#E8F5E9"),
            Color.FromHex("#F1F8E9"),
            Color.FromHex("#F9FBE7"),
            Color.FromHex("#FFFDE7"),
            Color.FromHex("#FFF8E1"),
            Color.FromHex("#FFF3E0"),
            Color.FromHex("#FBE9E7")
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="OxfordViewCell"/> class.
        /// </summary>
        public OxfordViewCell()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Called when the details panel is expanded or collapsed.
        /// </summary>
        /// <param name="sender">The object sending the event.</param>
        /// <param name="e">The event parameters.</param>
        private void InvertDetailsPanel(object sender, EventArgs e)
        {
            this.DetailsPanel.IsVisible = !this.DetailsPanel.IsVisible;
            this.DictLabel.Text = this.DetailsPanel.IsVisible ? Properties.Resources.OxfordViewCell_DictionaryName_Expanded : Properties.Resources.OxfordViewCell_DictionaryName_Contracted;
            this.ForceUpdateSize();
        }

        private void Handle_Tapped(object sender, EventArgs e)
        {
            this.ForceUpdateSize();
        }

        /// <summary>
        /// Called when the binding context for this cell is changed.
        /// </summary>
        /// <param name="sender">The object sending the event.</param>
        /// <param name="e">The event parameters.</param>
        private void ViewCell_BindingContextChanged(object sender, EventArgs e)
        {
            // Since we can not used nested ListViews and we do not know the number of senses a priori, populate the
            // details panel here in code.
            // We need to put this in an event handler because, when the constructor is called, the BindingContext is
            // null
            OxfordWord word = (OxfordWord)this.BindingContext;
            if (word != null)
            {
                this.DetailsPanel.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

                Grid definitionGrid = new Grid();
                definitionGrid.BackgroundColor = OxfordViewCell.backgroundColors[1 % OxfordViewCell.backgroundColors.Length];

                definitionGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                definitionGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });

                int lastRow = 0;

                if (word.Definitions.Count > 0)
                {
                    Binding definitionBinding = new Binding("Definitions") { Converter = new StringListToHumanReadableListConverter() };
                    Label definitionLabel = new Label();
                    definitionLabel.FontSize = 14;
                    definitionLabel.LineBreakMode = LineBreakMode.WordWrap;
                    definitionLabel.SetBinding(Label.TextProperty, definitionBinding);

                    definitionGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                    definitionGrid.Children.Add(
                        new Label()
                        {
                            FontSize = 14,
                            FontAttributes = FontAttributes.Bold,
                            Text = Properties.Resources.CollinsViewCell_Definition,
                        },
                        0,
                        lastRow);
                    definitionGrid.Children.Add(definitionLabel, 1, lastRow);

                    ++lastRow;
                }

                if (word.Examples.Count > 0)
                {
                    Binding examplesBinding = new Binding("Examples") { Converter = new StringListToHumanReadableListConverter() };
                    Label examplesLabel = new Label();
                    examplesLabel.FontSize = 14;
                    examplesLabel.LineBreakMode = LineBreakMode.WordWrap;
                    examplesLabel.SetBinding(Label.TextProperty, examplesBinding);

                    definitionGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                    definitionGrid.Children.Add(
                        new Label()
                        {
                            FontSize = 14,
                            FontAttributes = FontAttributes.Bold,
                            Text = Properties.Resources.CollinsViewCell_Example
                        },
                        0,
                        lastRow);
                    definitionGrid.Children.Add(examplesLabel, 1, lastRow);

                    ++lastRow;
                }

                this.DetailsPanel.Children.Add(definitionGrid, 0, 0);
            }
        }
    }
}
