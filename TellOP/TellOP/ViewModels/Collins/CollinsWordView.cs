// <copyright file="CollinsWordView.cs" company="University of Murcia">
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
    using DataModels;
    using DataModels.APIModels.Collins;
    using Xamarin.Forms;

    /// <summary>
    /// Represent the CollinsWord object
    /// </summary>
    public class CollinsWordView : StackLayout
    {
        private CollinsWord word;

        /// <summary>
        /// Initializes a new instance of the <see cref="CollinsWordView"/> class.
        /// </summary>
        /// <param name="word">Collins Word</param>
        public CollinsWordView(CollinsWord word)
            : base()
        {
            this.BackgroundColor = Color.Aqua;
            this.Padding = 1;
            this.Margin = 1;
            this.word = word;
            this.HorizontalOptions = LayoutOptions.FillAndExpand;
            this.VerticalOptions = LayoutOptions.Start;

            for (int senseNum = 0; senseNum < this.word.Senses.Count; ++senseNum)
            {
                Grid senseGrid = new Grid()
                {
                    ColumnDefinitions = new ColumnDefinitionCollection()
                    {
                        { new ColumnDefinition() { Width = GridLength.Auto } },
                        { new ColumnDefinition() { Width = GridLength.Star } },
                    },
                    RowDefinitions = new RowDefinitionCollection()
                    {
                        { new RowDefinition() { Height = GridLength.Auto } },
                        { new RowDefinition() { Height = GridLength.Auto } }
                    },
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    BackgroundColor = CollinsMultipleWordsWrapper._bkgColors[senseNum % 6]
                };

                int rowCounter = 0;
                { // Init fill content
                    Label titleLabel = new Label
                    {
                        Text = "[" + this.word.PartOfSpeech.GetFriendlyName() + "] Sense #" + (senseNum + 1),
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        HorizontalOptions = LayoutOptions.Start,
                        FontAttributes = FontAttributes.Bold,
                        FontSize = 12d
                    };

                    senseGrid.Children.Add(titleLabel, 0, rowCounter);
                    Grid.SetColumnSpan(titleLabel, 2);

                    CollinsWordDefinitionSense currentSense = this.word.Senses[senseNum];
                    for (int definitionNum = 0; definitionNum < currentSense.Definitions.Count; ++definitionNum)
                    {
                        rowCounter++;
                        senseGrid.Children.Add(
                            new Label
                            {
                                Text = "Definition #" + (definitionNum + 1),
                                VerticalOptions = LayoutOptions.CenterAndExpand,
                                HorizontalOptions = LayoutOptions.Start,
                                FontAttributes = FontAttributes.Bold,
                                FontSize = 12d
                            },
                            0,
                            rowCounter);
                        senseGrid.Children.Add(
                            new Label
                            {
                                Text = currentSense.Definitions[definitionNum],
                                VerticalOptions = LayoutOptions.CenterAndExpand,
                                HorizontalOptions = LayoutOptions.Start,
                                FontAttributes = FontAttributes.Bold,
                                FontSize = 12d
                            },
                            1,
                            rowCounter);
                    } // End definition for

                    for (int exampleNum = 0; exampleNum < currentSense.Examples.Count; ++exampleNum)
                    {
                        rowCounter++;
                        senseGrid.Children.Add(
                            new Label
                            {
                                Text = "Example #" + (exampleNum + 1),
                                VerticalOptions = LayoutOptions.CenterAndExpand,
                                HorizontalOptions = LayoutOptions.Start,
                                FontAttributes = FontAttributes.Bold,
                                FontSize = 12d
                            },
                            0,
                            rowCounter);
                        senseGrid.Children.Add(
                            new Label
                            {
                                Text = currentSense.Examples[exampleNum],
                                VerticalOptions = LayoutOptions.CenterAndExpand,
                                HorizontalOptions = LayoutOptions.Start,
                                FontAttributes = FontAttributes.Bold,
                                FontSize = 12d
                            },
                            1,
                            rowCounter);
                    } // End example for
                } // End fill content

                this.Children.Add(senseGrid);
            } // End sense for
        }
    }
}
