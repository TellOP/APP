// <copyright file="PartOfSpeechWordsListView.cs" company="University of Murcia">
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
    using System.Globalization;
    using DataModels;
    using DataModels.Enums;
    using Xamarin.Forms;

    /// <summary>
    /// Words' list render as <see cref="Frame"/> element.
    /// </summary>
    public class PartOfSpeechWordsListView : Frame
    {
        /// <summary>
        /// Gets or sets the internal frame with the words list
        /// </summary>
        private Grid _innerFrame;

        /// <summary>
        /// Gets or sets the title lavel
        /// </summary>
        private Label _nameLabel;

        /// <summary>
        /// Gets or sets the image with the SHOW icon
        /// </summary>
        private Image _spoilerButtonSHOW;

        /// <summary>
        /// Gets or sets the image with the HIDE icon
        /// </summary>
        private Image _spoilerButtonHIDE;

        /// <summary>
        /// Relative <see cref="PartOfSpeech"/> for the panel.
        /// </summary>
        private PartOfSpeech _pos;

        /// <summary>
        /// Gesture recognizer for the tap event.
        /// </summary>
        private TapGestureRecognizer _tapGestureRecFromInnerFrameVisibilityInverter = new TapGestureRecognizer();

        /// <summary>
        /// Initializes a new instance of the <see cref="PartOfSpeechWordsListView"/> class.
        /// </summary>
        /// <param name="pos">Part of speech identifier</param>
        /// <param name="userTextList">Dictionary of words</param>
        public PartOfSpeechWordsListView(PartOfSpeech pos, IDictionary<IWord, int> userTextList)
        {
            if (userTextList == null)
            {
                throw new ArgumentNullException("userTextList");
            }

            this._pos = pos;
            this._buildNameLabel();
            this._buildSpoilerButton();
            this._buildInnerFrame();
            this._buildOuterStack();

            this.Content = this.OuterStack;
            this.Padding = 0;
            this.Margin = 0;

            this._tapGestureRecFromInnerFrameVisibilityInverter.Tapped += (s, e) =>
            {
                this.InnerFrameVisibilityInverter();
            };
            this.Populate(new List<IWord>(userTextList.Keys));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PartOfSpeechWordsListView"/> class.
        /// </summary>
        /// <param name="pos">Part of speech identifier</param>
        /// <param name="words">List of <see cref="IWord"/> objects</param>
        public PartOfSpeechWordsListView(PartOfSpeech pos, List<IWord> words)
        {
            this._pos = pos;
            this._buildNameLabel();
            this._buildSpoilerButton();
            this._buildInnerFrame();
            this._buildOuterStack();

            this.Content = this.OuterStack;
            this.Padding = 0;
            this.Margin = 0;

            this._tapGestureRecFromInnerFrameVisibilityInverter.Tapped += (s, e) =>
            {
                this.InnerFrameVisibilityInverter();
            };

            this.Populate(words);
        }

        /// <summary>
        /// Gets External StackLayout
        /// </summary>
        public StackLayout OuterStack { get; private set; }

        /// <summary>
        /// Gets Main words' list
        /// </summary>
        public List<IWord> Words { get; private set; }

        /// <summary>
        /// Asynchronously update the content of the InnerFrame.
        /// </summary>
        /// <param name="words">Word List</param>
        /// <returns>True iff everything was completed correctly.</returns>
        public bool Populate(List<IWord> words)
        {
            this.Words = words;
            this._nameLabel.Text = this._pos.ToString() + " [" + this.Words.Count + "]";

            try
            {
                int row = 0;
                int col = 0;

                foreach (IWord w in this.Words)
                {
                    Label tmpLabel = new Label { Text = w.Term, FontSize = 12d, HorizontalOptions = LayoutOptions.Center };
                    tmpLabel.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => this.OnWordClicked(tmpLabel)) });
                    this._innerFrame.Children.Add(tmpLabel, col, row);

                    if (col < 2)
                    {
                        col++;
                    }
                    else
                    {
                        col = 0;
                        row++;
                        this._innerFrame.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.Logger.Log(this.GetType().ToString(), "Populate Inner Frame", ex);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Initialize the inner frame
        /// </summary>
        private void _buildInnerFrame()
        {
            this._innerFrame = new Grid()
            {
                RowDefinitions =
                {
                    new RowDefinition() { Height = GridLength.Auto }
                },
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Start,
                IsVisible = false,
                Margin = 0,
                Padding = 2
            };
            this._innerFrame.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            this._innerFrame.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            this._innerFrame.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        }

        /// <summary>
        /// Initialize the external stacklayout
        /// </summary>
        private void _buildOuterStack()
        {
            this.OuterStack = new StackLayout
            {
                Margin = 0,
                Padding = 2
            };

            Grid tmpGrid = new Grid()
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Auto }
                }
            };
            tmpGrid.Children.Add(this._nameLabel, 1, 0);
            tmpGrid.Children.Add(this._spoilerButtonSHOW, 0, 0);
            tmpGrid.Children.Add(this._spoilerButtonHIDE, 0, 0);

            this.OuterStack.Children.Add(tmpGrid);
            this.OuterStack.Children.Add(this._innerFrame);
        }

        /// <summary>
        /// Initialize the title page
        /// </summary>
        private void _buildNameLabel()
        {
            this._nameLabel = new Label
            {
                // FIXME
                Text = this._pos.ToString(),
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                FontAttributes = FontAttributes.Bold,
                FontSize = 14d
            };
            this._nameLabel.GestureRecognizers.Add(this._tapGestureRecFromInnerFrameVisibilityInverter);
        }

        /// <summary>
        /// Initialize the spoiler button
        /// </summary>
        private void _buildSpoilerButton()
        {
            this._spoilerButtonSHOW = new Image
            {
                Aspect = Aspect.AspectFit,
                Source = Device.OnPlatform(
                            iOS: ImageSource.FromFile("BTN_show24.png"),
                            Android: ImageSource.FromFile("BTN_show24.png"),
                            WinPhone: ImageSource.FromFile("BTN_show24.png")),
                Margin = 0,
                Opacity = 0
            };
            this._spoilerButtonHIDE = new Image
            {
                Aspect = Aspect.AspectFit,
                Source = Device.OnPlatform(
                            iOS: ImageSource.FromFile("BTN_hide24.png"),
                            Android: ImageSource.FromFile("BTN_hide24.png"),
                            WinPhone: ImageSource.FromFile("BTN_hide24.png")),
                Margin = 0
            };

            this._spoilerButtonSHOW.GestureRecognizers.Add(this._tapGestureRecFromInnerFrameVisibilityInverter);
            this._spoilerButtonHIDE.GestureRecognizers.Add(this._tapGestureRecFromInnerFrameVisibilityInverter);
        }

        /// <summary>
        /// Initialize the frame visibility inverter
        /// </summary>
        private void InnerFrameVisibilityInverter()
        {
            if (this._innerFrame == null)
            {
                this._buildInnerFrame();
            }

            if (this._innerFrame.IsVisible)
            {
                this._innerFrame.IsVisible = false;
                this._spoilerButtonSHOW.Opacity = 0;
                this._spoilerButtonHIDE.Opacity = 1;

                this.BackgroundColor = Color.Default;
            }
            else
            {
                this._innerFrame.IsVisible = true;
                this._spoilerButtonSHOW.Opacity = 1;
                this._spoilerButtonHIDE.Opacity = 0;
                this._innerFrame.Focus();

                this.BackgroundColor = (Color)new PartOfSpeechToColorConverter().Convert(this._pos, typeof(Color), null, CultureInfo.InvariantCulture);
            }
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
