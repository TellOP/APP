// <copyright file="EssayExerciseView.xaml.cs" company="University of Murcia">
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
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Api;
    using DataModels.Activity;
    using DataModels.ApiModels.Exercise;
    using DataModels.Enums;
    using Tools;
    using Xamarin.Forms;

    /// <summary>
    /// EssayExercise application view.
    /// </summary>
    public partial class EssayExerciseView : ContentPage
    {
        // TODO: the routines here should be made async and the code should be moved to a ViewModel where appropriate.

        /// <summary>
        /// EssayExercise object
        /// </summary>
        private EssayExercise ex;
        private bool _isAnotherAnalysisRunning = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="EssayExerciseView"/> class.
        /// </summary>
        /// <param name="essay">EssayExercise object</param>
        public EssayExerciseView(EssayExercise essay)
        {
            if (essay == null)
            {
                throw new ArgumentNullException("essay");
            }

            this.InitializeComponent();

            this.SizeChanged += this.EssayExerciseView_SizeChanged;

            this.FillData(essay);

            this.imgReloadVerbs.GestureRecognizers.Add(new TapGestureRecognizer() { Command = new Command(() => this.AnalysisButton_Clicked(this.imgReloadVerbs, null)) });
            this.imgReloadNouns.GestureRecognizers.Add(new TapGestureRecognizer() { Command = new Command(() => this.AnalysisButton_Clicked(this.imgReloadNouns, null)) });
            this.imgReloadAdjective.GestureRecognizers.Add(new TapGestureRecognizer() { Command = new Command(() => this.AnalysisButton_Clicked(this.imgReloadAdjective, null)) });
            this.imgReloadAdverbs.GestureRecognizers.Add(new TapGestureRecognizer() { Command = new Command(() => this.AnalysisButton_Clicked(this.imgReloadAdverbs, null)) });
            this.imgReloadPreposition.GestureRecognizers.Add(new TapGestureRecognizer() { Command = new Command(() => this.AnalysisButton_Clicked(this.imgReloadPreposition, null)) });
            this.imgReloadUnclassified.GestureRecognizers.Add(new TapGestureRecognizer() { Command = new Command(() => this.AnalysisButton_Clicked(this.imgReloadUnclassified, null)) });

            this._changeActivityIndicatorsStatus(false);

            this.ShowRefTextButton_Clicked(null, null);

            if (Device.Idiom == TargetIdiom.Tablet || Device.Idiom == TargetIdiom.Desktop)
            {
                // TODO: move these settings to XAML. Use a syntax similar to this one (which, however, does not work):
                // BackgroundColor = "{Binding Converter={enums:LanguageLevelClassificationToColorConverter}, ConverterParameter={x:Static enums:LanguageLevelClassification.A1}}"
                // This is tracked in <https://github.com/TellOP/APP/issues/1>
                LanguageLevelClassificationToColorConverter langToColorConverter = new LanguageLevelClassificationToColorConverter();
                this.A1Frame.BackgroundColor = (Color)langToColorConverter.Convert(LanguageLevelClassification.A1, typeof(Color), null, CultureInfo.CurrentUICulture);
                this.A2Frame.BackgroundColor = (Color)langToColorConverter.Convert(LanguageLevelClassification.A2, typeof(Color), null, CultureInfo.CurrentUICulture);
                this.B1Frame.BackgroundColor = (Color)langToColorConverter.Convert(LanguageLevelClassification.B1, typeof(Color), null, CultureInfo.CurrentUICulture);
                this.B2Frame.BackgroundColor = (Color)langToColorConverter.Convert(LanguageLevelClassification.B2, typeof(Color), null, CultureInfo.CurrentUICulture);
                this.C1Frame.BackgroundColor = (Color)langToColorConverter.Convert(LanguageLevelClassification.C1, typeof(Color), null, CultureInfo.CurrentUICulture);
                this.C2Frame.BackgroundColor = (Color)langToColorConverter.Convert(LanguageLevelClassification.C2, typeof(Color), null, CultureInfo.CurrentUICulture);

                this.ExSupportTextContent.Text = string.IsNullOrWhiteSpace(this.ex.PreliminaryText) ? Properties.Resources.EssayExerciseView_ExSupportTextContentError : this.ex.PreliminaryText;
            }
        }

        /// <summary>
        /// Gets the title of the exercise.
        /// </summary>
        public string ExTitle
        {
            get { return this.ex.Title; }
        }

        /// <summary>
        /// Gets the language of the exercise.
        /// </summary>
        public SupportedLanguage ExLanguage
        {
            get { return this.ex.Language; }
        }

        /// <summary>
        /// Gets the level of the exercise.
        /// </summary>
        public LanguageLevelClassification ExLevel
        {
            get { return this.ex.Level; }
        }

        /// <summary>
        /// Gets the type of the exercise.
        /// </summary>
        public string ExTypeOfExercise
        {
            get { return Properties.Resources.Exercise_EssayName; }
        }

        /// <summary>
        /// Gets a description of the exercise.
        /// </summary>
        public string ExDescription
        {
            get { return this.ex.Description; }
        }

        /// <summary>
        /// Gets the contents of the exercise.
        /// </summary>
        public string ExContent
        {
            get
            {
                if (this.ex is EssayExercise)
                {
                    EssayExercise eex = this.ex;
                    if (string.IsNullOrEmpty(eex.Contents))
                    {
                        return string.Empty;
                    }
                    else
                    {
                        return eex.Contents;
                    }
                }
                else
                {
                    throw new NotSupportedException(Properties.Resources.EssayExerciseViewStringContentError);
                }
            }
        }

        /// <summary>
        /// Gets the exercise identifier.
        /// </summary>
        public int ExUid
        {
            get
            {
                return this.ex.Uid;
            }
        }

        /// <summary>
        /// Gets the exercise status.
        /// </summary>
        public ExerciseStatus ExStatus
        {
            get
            {
                return this.ex.Status;
            }
        }

        /// <summary>
        /// Handle the sizechanged event. Specifically detect if the device has changed orientation.
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arg.</param>
        private void EssayExerciseView_SizeChanged(object sender, EventArgs e)
        {
            bool isPortrait = this.Height > this.Width;

            if (Device.Idiom == TargetIdiom.Tablet || Device.Idiom == TargetIdiom.Desktop)
            {
                if (isPortrait)
                {
                    this.ApplyPortraitLayoutTablets();
                }
                else
                {
                    this.ApplyLandscapeLayoutTablets();
                }
            }
            else if (isPortrait)
            {
                this.ApplyPortraitLayoutPhones();
            }
            else
            {
                this.ApplyLandscapeLayoutPhones();
            }
        }

        /// <summary>
        /// Apply landscape layout for tablets and desktops.
        /// </summary>
        private void ApplyLandscapeLayoutTablets()
        {
            bool isTextEditorEmpty = string.IsNullOrWhiteSpace(this.ExContentEditor.Text);
            this.MainGrid.Children.Clear();

            this.MainGrid.Padding = 5;
            this.MainGrid.Margin = 5;

            // Initialize the coordinates and the cell locations
            int exTitleLabel_Coord_X = 0;
            int exTitleLabel_Coord_Y = 0;
            int exTitleLabel_ColumnSpan = 18;
            int exTitleLabel_RowSpan = 2;

            int exDescriptionLabel_Coord_X = 18;
            int exDescriptionLabel_Coord_Y = 0;
            int exDescriptionLabel_ColumnSpan = 24;
            int exDescriptionLabel_RowSpan = 1;

            int exCountLabel_Coord_X = 18;
            int exCountLabel_Coord_Y = 1;
            int exCountLabel_ColumnSpan = 24;
            int exCountLabel_RowSpan = 1;

            int exSupportTextLabel_Coord_X = 0;
            int exSupportTextLabel_Coord_Y = 3;
            int exSupportTextLabel_ColumnSpan = 18;
            int exSupportTextLabel_RowSpan = 1;

            int exSupportText_Coord_X = 0;
            int exSupportText_Coord_Y = 4;
            int exSupportText_ColumnSpan = 18;
            int exSupportText_RowSpan = 18;

            int exContentEditor_Coord_X = 18;
            int exContentEditor_Coord_Y = 3;
            int exContentEditor_ColumnSpan = 24;
            int exContentEditor_RowSpan = 21;

            int verbs_Coord_X = 0;
            int verbs_Coord_Y = 23;
            int verbs_ColumnSpan = 3;
            int verbs_RowSpan = 1;

            int nouns_Coord_X = 3;
            int nouns_Coord_Y = 23;
            int nouns_ColumnSpan = 3;
            int nouns_RowSpan = 1;

            int preposition_Coord_X = 6;
            int preposition_Coord_Y = 23;
            int preposition_ColumnSpan = 3;
            int preposition_RowSpan = 1;

            int adverbs_Coord_X = 9;
            int adverbs_Coord_Y = 23;
            int adverbs_ColumnSpan = 3;
            int adverbs_RowSpan = 1;

            int adjective_Coord_X = 12;
            int adjective_Coord_Y = 23;
            int adjective_ColumnSpan = 3;
            int adjective_RowSpan = 1;

            int unclassified_Coord_X = 15;
            int unclassified_Coord_Y = 23;
            int unclassified_ColumnSpan = 3;
            int unclassified_RowSpan = 1;

            int a1_Coord_X = 0;
            int a1_Coord_Y = 21;
            int a1_ColumnSpan = 3;
            int a1_RowSpan = 2;

            int a2_Coord_X = 3;
            int a2_Coord_Y = 21;
            int a2_ColumnSpan = 3;
            int a2_RowSpan = 2;

            int b1_Coord_X = 6;
            int b1_Coord_Y = 21;
            int b1_ColumnSpan = 3;
            int b1_RowSpan = 2;

            int b2_Coord_X = 9;
            int b2_Coord_Y = 21;
            int b2_ColumnSpan = 3;
            int b2_RowSpan = 2;

            int c1_Coord_X = 12;
            int c1_Coord_Y = 21;
            int c1_ColumnSpan = 3;
            int c1_RowSpan = 2;

            int c2_Coord_X = 15;
            int c2_Coord_Y = 21;
            int c2_ColumnSpan = 3;
            int c2_RowSpan = 2;

            this.MainGrid.RowDefinitions = new RowDefinitionCollection()
                {
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #1
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #2
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #3
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #4
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #5
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #6
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #7
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #8
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #9
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #10
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #11
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #12
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #13
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #14
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #15
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #16
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #17
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #18
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #19
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #20
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #21
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #22
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #23
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #24
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #25
                };

            this.MainGrid.ColumnDefinitions = new ColumnDefinitionCollection()
                {
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #1
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #2
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #3
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #4
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #5
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #6
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #7
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #8
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #9
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #10
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #11
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #12
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #13
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #14
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #15
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #16
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #17
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #18
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #19
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #20
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #21
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #22
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #23
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #24
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #25
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #26
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #27
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #28
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #29
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #30
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #31
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #32
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #33
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #34
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #35
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #36
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #37
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #38
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #39
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #40
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #41
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #42
            };

            this.ExTitleLabel.HorizontalOptions = LayoutOptions.CenterAndExpand;
            this.ExTitleLabel.VerticalOptions = LayoutOptions.CenterAndExpand;
            this.MainGrid.Children.Add(this.ExTitleLabel, exTitleLabel_Coord_X, exTitleLabel_Coord_Y);
            Grid.SetColumnSpan(this.ExTitleLabel, exTitleLabel_ColumnSpan);
            Grid.SetRowSpan(this.ExTitleLabel, exTitleLabel_RowSpan);

            this.ExDescriptionLabel.HorizontalOptions = LayoutOptions.CenterAndExpand;
            this.ExDescriptionLabel.VerticalOptions = LayoutOptions.CenterAndExpand;
            this.MainGrid.Children.Add(this.ExDescriptionLabel, exDescriptionLabel_Coord_X, exDescriptionLabel_Coord_Y);
            Grid.SetColumnSpan(this.ExDescriptionLabel, exDescriptionLabel_ColumnSpan);
            Grid.SetRowSpan(this.ExDescriptionLabel, exDescriptionLabel_RowSpan);

            this.ExCountLabel.HorizontalOptions = LayoutOptions.CenterAndExpand;
            this.ExCountLabel.VerticalOptions = LayoutOptions.CenterAndExpand;
            this.MainGrid.Children.Add(this.ExCountLabel, exCountLabel_Coord_X, exCountLabel_Coord_Y);
            Grid.SetColumnSpan(this.ExCountLabel, exCountLabel_ColumnSpan);
            Grid.SetRowSpan(this.ExCountLabel, exCountLabel_RowSpan);

            this.MainGrid.Children.Add(this.ExSupportTextLabel, exSupportTextLabel_Coord_X, exSupportTextLabel_Coord_Y);
            Grid.SetColumnSpan(this.ExSupportTextLabel, exSupportTextLabel_ColumnSpan);
            Grid.SetRowSpan(this.ExSupportTextLabel, exSupportTextLabel_RowSpan);

            this.MainGrid.Children.Add(this.ExSupportTextContent, exSupportText_Coord_X, exSupportText_Coord_Y);
            Grid.SetColumnSpan(this.ExSupportTextContent, exSupportText_ColumnSpan);
            Grid.SetRowSpan(this.ExSupportTextContent, exSupportText_RowSpan);

            this.MainGrid.Children.Add(this.ExContentEditor, exContentEditor_Coord_X, exContentEditor_Coord_Y);
            Grid.SetColumnSpan(this.ExContentEditor, exContentEditor_ColumnSpan);
            Grid.SetRowSpan(this.ExContentEditor, exContentEditor_RowSpan);

            this.MainGrid.Children.Add(this.statVerbsLabel, verbs_Coord_X, verbs_Coord_Y);
            Grid.SetColumnSpan(this.statVerbsLabel, verbs_ColumnSpan);
            Grid.SetRowSpan(this.statVerbsLabel, verbs_RowSpan);
            this.MainGrid.Children.Add(this.statVerbs, verbs_Coord_X, verbs_Coord_Y + 1);
            Grid.SetColumnSpan(this.statVerbs, verbs_ColumnSpan);
            Grid.SetRowSpan(this.statVerbs, verbs_RowSpan);
            this.MainGrid.Children.Add(this.aiVerbs, verbs_Coord_X, verbs_Coord_Y + 1);
            Grid.SetColumnSpan(this.aiVerbs, verbs_ColumnSpan);
            Grid.SetRowSpan(this.aiVerbs, verbs_RowSpan);
            this.MainGrid.Children.Add(this.imgReloadVerbs, verbs_Coord_X, verbs_Coord_Y + 1);
            Grid.SetColumnSpan(this.imgReloadVerbs, verbs_ColumnSpan);
            Grid.SetRowSpan(this.imgReloadVerbs, verbs_RowSpan);
            this.statVerbsLabel.IsVisible = true;
            this.statVerbs.IsVisible = !isTextEditorEmpty;
            this.imgReloadVerbs.IsVisible = isTextEditorEmpty;

            this.MainGrid.Children.Add(this.statNounsLabel, nouns_Coord_X, nouns_Coord_Y);
            Grid.SetColumnSpan(this.statNounsLabel, nouns_ColumnSpan);
            Grid.SetRowSpan(this.statNounsLabel, nouns_RowSpan);
            this.MainGrid.Children.Add(this.statNouns, nouns_Coord_X, nouns_Coord_Y + 1);
            Grid.SetColumnSpan(this.statNouns, nouns_ColumnSpan);
            Grid.SetRowSpan(this.statNouns, nouns_RowSpan);
            this.MainGrid.Children.Add(this.aiNouns, nouns_Coord_X, nouns_Coord_Y + 1);
            Grid.SetColumnSpan(this.aiNouns, nouns_ColumnSpan);
            Grid.SetRowSpan(this.aiNouns, nouns_RowSpan);
            this.MainGrid.Children.Add(this.imgReloadNouns, nouns_Coord_X, nouns_Coord_Y + 1);
            Grid.SetColumnSpan(this.imgReloadNouns, nouns_ColumnSpan);
            Grid.SetRowSpan(this.imgReloadNouns, nouns_RowSpan);
            this.statNounsLabel.IsVisible = true;
            this.statNouns.IsVisible = !isTextEditorEmpty;
            this.imgReloadNouns.IsVisible = isTextEditorEmpty;

            this.MainGrid.Children.Add(this.statPrepositionLabel, preposition_Coord_X, preposition_Coord_Y);
            Grid.SetColumnSpan(this.statPrepositionLabel, preposition_ColumnSpan);
            Grid.SetRowSpan(this.statPrepositionLabel, preposition_RowSpan);
            this.MainGrid.Children.Add(this.statPreposition, preposition_Coord_X, preposition_Coord_Y + 1);
            Grid.SetColumnSpan(this.statPreposition, preposition_ColumnSpan);
            Grid.SetRowSpan(this.statPreposition, preposition_RowSpan);
            this.MainGrid.Children.Add(this.aiPreposition, preposition_Coord_X, preposition_Coord_Y + 1);
            Grid.SetColumnSpan(this.aiPreposition, preposition_ColumnSpan);
            Grid.SetRowSpan(this.aiPreposition, preposition_RowSpan);
            this.MainGrid.Children.Add(this.imgReloadPreposition, preposition_Coord_X, preposition_Coord_Y + 1);
            Grid.SetColumnSpan(this.imgReloadPreposition, preposition_ColumnSpan);
            Grid.SetRowSpan(this.imgReloadPreposition, preposition_RowSpan);
            this.statPrepositionLabel.IsVisible = true;
            this.statPreposition.IsVisible = !isTextEditorEmpty;
            this.imgReloadPreposition.IsVisible = isTextEditorEmpty;

            this.MainGrid.Children.Add(this.statAdverbsLabel, adverbs_Coord_X, adverbs_Coord_Y);
            Grid.SetColumnSpan(this.statAdverbsLabel, adverbs_ColumnSpan);
            Grid.SetRowSpan(this.statAdverbsLabel, adverbs_RowSpan);
            this.MainGrid.Children.Add(this.statAdverbs, adverbs_Coord_X, adverbs_Coord_Y + 1);
            Grid.SetColumnSpan(this.statAdverbs, adverbs_ColumnSpan);
            Grid.SetRowSpan(this.statAdverbs, adverbs_RowSpan);
            this.MainGrid.Children.Add(this.aiAdverbs, adverbs_Coord_X, adverbs_Coord_Y + 1);
            Grid.SetColumnSpan(this.aiAdverbs, adverbs_ColumnSpan);
            Grid.SetRowSpan(this.aiAdverbs, adverbs_RowSpan);
            this.MainGrid.Children.Add(this.imgReloadAdverbs, adverbs_Coord_X, adverbs_Coord_Y + 1);
            Grid.SetColumnSpan(this.imgReloadAdverbs, adverbs_ColumnSpan);
            Grid.SetRowSpan(this.imgReloadAdverbs, adverbs_RowSpan);
            this.statAdverbsLabel.IsVisible = true;
            this.statAdverbs.IsVisible = !isTextEditorEmpty;
            this.imgReloadAdverbs.IsVisible = isTextEditorEmpty;

            this.MainGrid.Children.Add(this.statAdjectiveLabel, adjective_Coord_X, adjective_Coord_Y);
            Grid.SetColumnSpan(this.statAdjectiveLabel, adjective_ColumnSpan);
            Grid.SetRowSpan(this.statAdjectiveLabel, adjective_RowSpan);
            this.MainGrid.Children.Add(this.statAdjective, adjective_Coord_X, adjective_Coord_Y + 1);
            Grid.SetColumnSpan(this.statAdjective, adjective_ColumnSpan);
            Grid.SetRowSpan(this.statAdjective, adjective_RowSpan);
            this.MainGrid.Children.Add(this.aiAdjective, adjective_Coord_X, adjective_Coord_Y + 1);
            Grid.SetColumnSpan(this.aiAdjective, adjective_ColumnSpan);
            Grid.SetRowSpan(this.aiAdjective, adjective_RowSpan);
            this.MainGrid.Children.Add(this.imgReloadAdjective, adjective_Coord_X, adjective_Coord_Y + 1);
            Grid.SetColumnSpan(this.imgReloadAdjective, adjective_ColumnSpan);
            Grid.SetRowSpan(this.imgReloadAdjective, adjective_RowSpan);
            this.statAdjectiveLabel.IsVisible = true;
            this.statAdjective.IsVisible = !isTextEditorEmpty;
            this.imgReloadAdjective.IsVisible = isTextEditorEmpty;

            this.MainGrid.Children.Add(this.statUnclassifiedLabel, unclassified_Coord_X, unclassified_Coord_Y);
            Grid.SetColumnSpan(this.statUnclassifiedLabel, unclassified_ColumnSpan);
            Grid.SetRowSpan(this.statUnclassifiedLabel, unclassified_RowSpan);
            this.MainGrid.Children.Add(this.statUnclassified, unclassified_Coord_X, unclassified_Coord_Y + 1);
            Grid.SetColumnSpan(this.statUnclassified, unclassified_ColumnSpan);
            Grid.SetRowSpan(this.statUnclassified, unclassified_RowSpan);
            this.MainGrid.Children.Add(this.aiUnclassified, unclassified_Coord_X, unclassified_Coord_Y + 1);
            Grid.SetColumnSpan(this.aiUnclassified, unclassified_ColumnSpan);
            Grid.SetRowSpan(this.aiUnclassified, unclassified_RowSpan);
            this.MainGrid.Children.Add(this.imgReloadUnclassified, unclassified_Coord_X, unclassified_Coord_Y + 1);
            Grid.SetColumnSpan(this.imgReloadUnclassified, unclassified_ColumnSpan);
            Grid.SetRowSpan(this.imgReloadUnclassified, unclassified_RowSpan);
            this.statUnclassifiedLabel.IsVisible = true;
            this.statUnclassified.IsVisible = !isTextEditorEmpty;
            this.imgReloadUnclassified.IsVisible = isTextEditorEmpty;

            this.MainGrid.Children.Add(this.A1Frame, a1_Coord_X, a1_Coord_Y);
            Grid.SetColumnSpan(this.A1Frame, a1_ColumnSpan);
            Grid.SetRowSpan(this.A1Frame, a1_RowSpan);
            this.A1Frame.IsVisible = true;

            this.MainGrid.Children.Add(this.A2Frame, a2_Coord_X, a2_Coord_Y);
            Grid.SetColumnSpan(this.A2Frame, a2_ColumnSpan);
            Grid.SetRowSpan(this.A2Frame, a2_RowSpan);
            this.A2Frame.IsVisible = true;

            this.MainGrid.Children.Add(this.B1Frame, b1_Coord_X, b1_Coord_Y);
            Grid.SetColumnSpan(this.B1Frame, b1_ColumnSpan);
            Grid.SetRowSpan(this.B1Frame, b1_RowSpan);
            this.B1Frame.IsVisible = true;

            this.MainGrid.Children.Add(this.B2Frame, b2_Coord_X, b2_Coord_Y);
            Grid.SetColumnSpan(this.B2Frame, b2_ColumnSpan);
            Grid.SetRowSpan(this.B2Frame, b2_RowSpan);
            this.B2Frame.IsVisible = true;

            this.MainGrid.Children.Add(this.C1Frame, c1_Coord_X, c1_Coord_Y);
            Grid.SetColumnSpan(this.C1Frame, c1_ColumnSpan);
            Grid.SetRowSpan(this.C1Frame, c1_RowSpan);
            this.C1Frame.IsVisible = true;

            this.MainGrid.Children.Add(this.C2Frame, c2_Coord_X, c2_Coord_Y);
            Grid.SetColumnSpan(this.C2Frame, c2_ColumnSpan);
            Grid.SetRowSpan(this.C2Frame, c2_RowSpan);
            this.C2Frame.IsVisible = true;
        }

        /// <summary>
        /// Apply portrait layout for tablets and desktops.
        /// </summary>
        private void ApplyPortraitLayoutTablets()
        {
            bool isTextEditorEmpty = string.IsNullOrWhiteSpace(this.ExContentEditor.Text);
            this.MainGrid.Children.Clear();

            this.MainGrid.Padding = 5;
            this.MainGrid.Margin = 5;

            // Initialize the coordinates and the cell locations
            int exTitleLabel_Coord_X = 0;
            int exTitleLabel_Coord_Y = 0;
            int exTitleLabel_ColumnSpan = 25;
            int exTitleLabel_RowSpan = 2;

            int exDescriptionLabel_Coord_X = 0;
            int exDescriptionLabel_Coord_Y = 2;
            int exDescriptionLabel_ColumnSpan = 20;
            int exDescriptionLabel_RowSpan = 1;

            int exCountLabel_Coord_X = 20;
            int exCountLabel_Coord_Y = 2;
            int exCountLabel_ColumnSpan = 5;
            int exCountLabel_RowSpan = 1;

            int exSupportTextLabel_Coord_X = 0;
            int exSupportTextLabel_Coord_Y = 7;
            int exSupportTextLabel_ColumnSpan = 25;
            int exSupportTextLabel_RowSpan = 1;

            int exSupportText_Coord_X = 0;
            int exSupportText_Coord_Y = 8;
            int exSupportText_ColumnSpan = 25;
            int exSupportText_RowSpan = 10;

            int exContentEditor_Coord_X = 0;
            int exContentEditor_Coord_Y = 18;
            int exContentEditor_ColumnSpan = 25;
            int exContentEditor_RowSpan = 24;

            int verbs_Coord_X = 0;
            int verbs_Coord_Y = 3;
            int verbs_ColumnSpan = 4;
            int verbs_RowSpan = 1;

            int nouns_Coord_X = 4;
            int nouns_Coord_Y = 3;
            int nouns_ColumnSpan = 4;
            int nouns_RowSpan = 1;

            int preposition_Coord_X = 8;
            int preposition_Coord_Y = 3;
            int preposition_ColumnSpan = 4;
            int preposition_RowSpan = 1;

            int adverbs_Coord_X = 0;
            int adverbs_Coord_Y = 5;
            int adverbs_ColumnSpan = 4;
            int adverbs_RowSpan = 1;

            int adjective_Coord_X = 4;
            int adjective_Coord_Y = 5;
            int adjective_ColumnSpan = 4;
            int adjective_RowSpan = 1;

            int unclassified_Coord_X = 8;
            int unclassified_Coord_Y = 5;
            int unclassified_ColumnSpan = 4;
            int unclassified_RowSpan = 1;

            int a1_Coord_X = 14;
            int a1_Coord_Y = 3;
            int a1_ColumnSpan = 4;
            int a1_RowSpan = 2;

            int a2_Coord_X = 14;
            int a2_Coord_Y = 5;
            int a2_ColumnSpan = 4;
            int a2_RowSpan = 2;

            int b1_Coord_X = 18;
            int b1_Coord_Y = 3;
            int b1_ColumnSpan = 4;
            int b1_RowSpan = 2;

            int b2_Coord_X = 18;
            int b2_Coord_Y = 5;
            int b2_ColumnSpan = 4;
            int b2_RowSpan = 2;

            int c1_Coord_X = 22;
            int c1_Coord_Y = 3;
            int c1_ColumnSpan = 4;
            int c1_RowSpan = 2;

            int c2_Coord_X = 22;
            int c2_Coord_Y = 5;
            int c2_ColumnSpan = 4;
            int c2_RowSpan = 2;

            this.MainGrid.RowDefinitions = new RowDefinitionCollection()
                {
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #1
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #2
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #3
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #4
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #5
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #6
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #7
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #8
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #9
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #10
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #11
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #12
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #13
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #14
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #15
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #16
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #17
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #18
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #19
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #20
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #21
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #22
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #23
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #24
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #25
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #26
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #27
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #28
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #29
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #30
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #31
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #32
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #33
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #34
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #35
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #36
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #37
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #38
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #39
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #40
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #41
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } }, // Row #42
                };

            this.MainGrid.ColumnDefinitions = new ColumnDefinitionCollection()
                {
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #1
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #2
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #3
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #4
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #5
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #6
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #7
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #8
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #9
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #10
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #11
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #12
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #13
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #14
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #15
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #16
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #17
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #18
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #19
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #20
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #21
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #22
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #23
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #24
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } }, // Column #25
            };

            this.ExTitleLabel.HorizontalOptions = LayoutOptions.CenterAndExpand;
            this.ExTitleLabel.VerticalOptions = LayoutOptions.CenterAndExpand;
            this.MainGrid.Children.Add(this.ExTitleLabel, exTitleLabel_Coord_X, exTitleLabel_Coord_Y);
            Grid.SetColumnSpan(this.ExTitleLabel, exTitleLabel_ColumnSpan);
            Grid.SetRowSpan(this.ExTitleLabel, exTitleLabel_RowSpan);

            this.ExDescriptionLabel.HorizontalOptions = LayoutOptions.CenterAndExpand;
            this.ExDescriptionLabel.VerticalOptions = LayoutOptions.CenterAndExpand;
            this.MainGrid.Children.Add(this.ExDescriptionLabel, exDescriptionLabel_Coord_X, exDescriptionLabel_Coord_Y);
            Grid.SetColumnSpan(this.ExDescriptionLabel, exDescriptionLabel_ColumnSpan);
            Grid.SetRowSpan(this.ExDescriptionLabel, exDescriptionLabel_RowSpan);

            this.ExCountLabel.HorizontalOptions = LayoutOptions.CenterAndExpand;
            this.ExCountLabel.VerticalOptions = LayoutOptions.CenterAndExpand;
            this.MainGrid.Children.Add(this.ExCountLabel, exCountLabel_Coord_X, exCountLabel_Coord_Y);
            Grid.SetColumnSpan(this.ExCountLabel, exCountLabel_ColumnSpan);
            Grid.SetRowSpan(this.ExCountLabel, exCountLabel_RowSpan);

            this.MainGrid.Children.Add(this.ExSupportTextLabel, exSupportTextLabel_Coord_X, exSupportTextLabel_Coord_Y);
            Grid.SetColumnSpan(this.ExSupportTextLabel, exSupportTextLabel_ColumnSpan);
            Grid.SetRowSpan(this.ExSupportTextLabel, exSupportTextLabel_RowSpan);

            this.MainGrid.Children.Add(this.ExSupportTextContent, exSupportText_Coord_X, exSupportText_Coord_Y);
            Grid.SetColumnSpan(this.ExSupportTextContent, exSupportText_ColumnSpan);
            Grid.SetRowSpan(this.ExSupportTextContent, exSupportText_RowSpan);

            this.MainGrid.Children.Add(this.ExContentEditor, exContentEditor_Coord_X, exContentEditor_Coord_Y);
            Grid.SetColumnSpan(this.ExContentEditor, exContentEditor_ColumnSpan);
            Grid.SetRowSpan(this.ExContentEditor, exContentEditor_RowSpan);

            this.MainGrid.Children.Add(this.statVerbsLabel, verbs_Coord_X, verbs_Coord_Y);
            Grid.SetColumnSpan(this.statVerbsLabel, verbs_ColumnSpan);
            Grid.SetRowSpan(this.statVerbsLabel, verbs_RowSpan);
            this.MainGrid.Children.Add(this.statVerbs, verbs_Coord_X, verbs_Coord_Y + 1);
            Grid.SetColumnSpan(this.statVerbs, verbs_ColumnSpan);
            Grid.SetRowSpan(this.statVerbs, verbs_RowSpan);
            this.MainGrid.Children.Add(this.aiVerbs, verbs_Coord_X, verbs_Coord_Y + 1);
            Grid.SetColumnSpan(this.aiVerbs, verbs_ColumnSpan);
            Grid.SetRowSpan(this.aiVerbs, verbs_RowSpan);
            this.MainGrid.Children.Add(this.imgReloadVerbs, verbs_Coord_X, verbs_Coord_Y + 1);
            Grid.SetColumnSpan(this.imgReloadVerbs, verbs_ColumnSpan);
            Grid.SetRowSpan(this.imgReloadVerbs, verbs_RowSpan);
            this.statVerbsLabel.IsVisible = true;
            this.statVerbs.IsVisible = !isTextEditorEmpty;
            this.imgReloadVerbs.IsVisible = isTextEditorEmpty;

            this.MainGrid.Children.Add(this.statNounsLabel, nouns_Coord_X, nouns_Coord_Y);
            Grid.SetColumnSpan(this.statNounsLabel, nouns_ColumnSpan);
            Grid.SetRowSpan(this.statNounsLabel, nouns_RowSpan);
            this.MainGrid.Children.Add(this.statNouns, nouns_Coord_X, nouns_Coord_Y + 1);
            Grid.SetColumnSpan(this.statNouns, nouns_ColumnSpan);
            Grid.SetRowSpan(this.statNouns, nouns_RowSpan);
            this.MainGrid.Children.Add(this.aiNouns, nouns_Coord_X, nouns_Coord_Y + 1);
            Grid.SetColumnSpan(this.aiNouns, nouns_ColumnSpan);
            Grid.SetRowSpan(this.aiNouns, nouns_RowSpan);
            this.MainGrid.Children.Add(this.imgReloadNouns, nouns_Coord_X, nouns_Coord_Y + 1);
            Grid.SetColumnSpan(this.imgReloadNouns, nouns_ColumnSpan);
            Grid.SetRowSpan(this.imgReloadNouns, nouns_RowSpan);
            this.statNounsLabel.IsVisible = true;
            this.statNouns.IsVisible = !isTextEditorEmpty;
            this.imgReloadNouns.IsVisible = isTextEditorEmpty;

            this.MainGrid.Children.Add(this.statPrepositionLabel, preposition_Coord_X, preposition_Coord_Y);
            Grid.SetColumnSpan(this.statPrepositionLabel, preposition_ColumnSpan);
            Grid.SetRowSpan(this.statPrepositionLabel, preposition_RowSpan);
            this.MainGrid.Children.Add(this.statPreposition, preposition_Coord_X, preposition_Coord_Y + 1);
            Grid.SetColumnSpan(this.statPreposition, preposition_ColumnSpan);
            Grid.SetRowSpan(this.statPreposition, preposition_RowSpan);
            this.MainGrid.Children.Add(this.aiPreposition, preposition_Coord_X, preposition_Coord_Y + 1);
            Grid.SetColumnSpan(this.aiPreposition, preposition_ColumnSpan);
            Grid.SetRowSpan(this.aiPreposition, preposition_RowSpan);
            this.MainGrid.Children.Add(this.imgReloadPreposition, preposition_Coord_X, preposition_Coord_Y + 1);
            Grid.SetColumnSpan(this.imgReloadPreposition, preposition_ColumnSpan);
            Grid.SetRowSpan(this.imgReloadPreposition, preposition_RowSpan);
            this.statPrepositionLabel.IsVisible = true;
            this.statPreposition.IsVisible = !isTextEditorEmpty;
            this.imgReloadPreposition.IsVisible = isTextEditorEmpty;

            this.MainGrid.Children.Add(this.statAdverbsLabel, adverbs_Coord_X, adverbs_Coord_Y);
            Grid.SetColumnSpan(this.statAdverbsLabel, adverbs_ColumnSpan);
            Grid.SetRowSpan(this.statAdverbsLabel, adverbs_RowSpan);
            this.MainGrid.Children.Add(this.statAdverbs, adverbs_Coord_X, adverbs_Coord_Y + 1);
            Grid.SetColumnSpan(this.statAdverbs, adverbs_ColumnSpan);
            Grid.SetRowSpan(this.statAdverbs, adverbs_RowSpan);
            this.MainGrid.Children.Add(this.aiAdverbs, adverbs_Coord_X, adverbs_Coord_Y + 1);
            Grid.SetColumnSpan(this.aiAdverbs, adverbs_ColumnSpan);
            Grid.SetRowSpan(this.aiAdverbs, adverbs_RowSpan);
            this.MainGrid.Children.Add(this.imgReloadAdverbs, adverbs_Coord_X, adverbs_Coord_Y + 1);
            Grid.SetColumnSpan(this.imgReloadAdverbs, adverbs_ColumnSpan);
            Grid.SetRowSpan(this.imgReloadAdverbs, adverbs_RowSpan);
            this.statAdverbsLabel.IsVisible = true;
            this.statAdverbs.IsVisible = !isTextEditorEmpty;
            this.imgReloadAdverbs.IsVisible = isTextEditorEmpty;

            this.MainGrid.Children.Add(this.statAdjectiveLabel, adjective_Coord_X, adjective_Coord_Y);
            Grid.SetColumnSpan(this.statAdjectiveLabel, adjective_ColumnSpan);
            Grid.SetRowSpan(this.statAdjectiveLabel, adjective_RowSpan);
            this.MainGrid.Children.Add(this.statAdjective, adjective_Coord_X, adjective_Coord_Y + 1);
            Grid.SetColumnSpan(this.statAdjective, adjective_ColumnSpan);
            Grid.SetRowSpan(this.statAdjective, adjective_RowSpan);
            this.MainGrid.Children.Add(this.aiAdjective, adjective_Coord_X, adjective_Coord_Y + 1);
            Grid.SetColumnSpan(this.aiAdjective, adjective_ColumnSpan);
            Grid.SetRowSpan(this.aiAdjective, adjective_RowSpan);
            this.MainGrid.Children.Add(this.imgReloadAdjective, adjective_Coord_X, adjective_Coord_Y + 1);
            Grid.SetColumnSpan(this.imgReloadAdjective, adjective_ColumnSpan);
            Grid.SetRowSpan(this.imgReloadAdjective, adjective_RowSpan);
            this.statAdjectiveLabel.IsVisible = true;
            this.statAdjective.IsVisible = !isTextEditorEmpty;
            this.imgReloadAdjective.IsVisible = isTextEditorEmpty;

            this.MainGrid.Children.Add(this.statUnclassifiedLabel, unclassified_Coord_X, unclassified_Coord_Y);
            Grid.SetColumnSpan(this.statUnclassifiedLabel, unclassified_ColumnSpan);
            Grid.SetRowSpan(this.statUnclassifiedLabel, unclassified_RowSpan);
            this.MainGrid.Children.Add(this.statUnclassified, unclassified_Coord_X, unclassified_Coord_Y + 1);
            Grid.SetColumnSpan(this.statUnclassified, unclassified_ColumnSpan);
            Grid.SetRowSpan(this.statUnclassified, unclassified_RowSpan);
            this.MainGrid.Children.Add(this.aiUnclassified, unclassified_Coord_X, unclassified_Coord_Y + 1);
            Grid.SetColumnSpan(this.aiUnclassified, unclassified_ColumnSpan);
            Grid.SetRowSpan(this.aiUnclassified, unclassified_RowSpan);
            this.MainGrid.Children.Add(this.imgReloadUnclassified, unclassified_Coord_X, unclassified_Coord_Y + 1);
            Grid.SetColumnSpan(this.imgReloadUnclassified, unclassified_ColumnSpan);
            Grid.SetRowSpan(this.imgReloadUnclassified, unclassified_RowSpan);
            this.statUnclassifiedLabel.IsVisible = true;
            this.statUnclassified.IsVisible = !isTextEditorEmpty;
            this.imgReloadUnclassified.IsVisible = isTextEditorEmpty;

            this.MainGrid.Children.Add(this.A1Frame, a1_Coord_X, a1_Coord_Y);
            Grid.SetColumnSpan(this.A1Frame, a1_ColumnSpan);
            Grid.SetRowSpan(this.A1Frame, a1_RowSpan);
            this.A1Frame.IsVisible = true;

            this.MainGrid.Children.Add(this.A2Frame, a2_Coord_X, a2_Coord_Y);
            Grid.SetColumnSpan(this.A2Frame, a2_ColumnSpan);
            Grid.SetRowSpan(this.A2Frame, a2_RowSpan);
            this.A2Frame.IsVisible = true;

            this.MainGrid.Children.Add(this.B1Frame, b1_Coord_X, b1_Coord_Y);
            Grid.SetColumnSpan(this.B1Frame, b1_ColumnSpan);
            Grid.SetRowSpan(this.B1Frame, b1_RowSpan);
            this.B1Frame.IsVisible = true;

            this.MainGrid.Children.Add(this.B2Frame, b2_Coord_X, b2_Coord_Y);
            Grid.SetColumnSpan(this.B2Frame, b2_ColumnSpan);
            Grid.SetRowSpan(this.B2Frame, b2_RowSpan);
            this.B2Frame.IsVisible = true;

            this.MainGrid.Children.Add(this.C1Frame, c1_Coord_X, c1_Coord_Y);
            Grid.SetColumnSpan(this.C1Frame, c1_ColumnSpan);
            Grid.SetRowSpan(this.C1Frame, c1_RowSpan);
            this.C1Frame.IsVisible = true;

            this.MainGrid.Children.Add(this.C2Frame, c2_Coord_X, c2_Coord_Y);
            Grid.SetColumnSpan(this.C2Frame, c2_ColumnSpan);
            Grid.SetRowSpan(this.C2Frame, c2_RowSpan);
            this.C2Frame.IsVisible = true;
        }

        /// <summary>
        /// Apply landscape layout for smarthphones.
        /// </summary>
        private void ApplyLandscapeLayoutPhones()
        {
            bool isTextEditorEmpty = string.IsNullOrWhiteSpace(this.ExContentEditor.Text);
            this.MainGrid.Children.Clear();

            this.MainGrid.RowDefinitions = new RowDefinitionCollection()
                {
                    { new RowDefinition() { Height = new GridLength(3, GridUnitType.Star) } },
                    { new RowDefinition() { Height = new GridLength(3, GridUnitType.Star) } },
                    { new RowDefinition() { Height = new GridLength(2, GridUnitType.Star) } },
                    { new RowDefinition() { Height = new GridLength(2, GridUnitType.Star) } },
                    { new RowDefinition() { Height = new GridLength(2, GridUnitType.Star) } },
                    { new RowDefinition() { Height = new GridLength(2, GridUnitType.Star) } },
                    { new RowDefinition() { Height = new GridLength(2, GridUnitType.Star) } },
                    { new RowDefinition() { Height = new GridLength(2, GridUnitType.Star) } },
                };

            this.MainGrid.ColumnDefinitions = new ColumnDefinitionCollection()
                {
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } },
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } },
                    { new ColumnDefinition() { Width = new GridLength(3, GridUnitType.Star) } },
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } },
                };

            this.MainGrid.Children.Add(this.ExTitleLabel, 0, 0);
            Grid.SetColumnSpan(this.ExTitleLabel, 3);

            this.MainGrid.Children.Add(this.ExDescriptionLabel, 0, 1);
            Grid.SetColumnSpan(this.ExDescriptionLabel, 2);

            this.MainGrid.Children.Add(this.ExCountLabel, 3, 0);

            this.MainGrid.Children.Add(this.ExContentEditor, 2, 1);
            Grid.SetRowSpan(this.ExContentEditor, 8);
            Grid.SetColumnSpan(this.ExContentEditor, 2);

            this.MainGrid.Children.Add(this.statVerbsLabel, 0, 2);
            this.MainGrid.Children.Add(this.statVerbs, 0, 3);
            this.MainGrid.Children.Add(this.aiVerbs, 0, 3);
            this.MainGrid.Children.Add(this.imgReloadVerbs, 0, 3);

            this.MainGrid.Children.Add(this.statNounsLabel, 0, 4);
            this.MainGrid.Children.Add(this.statNouns, 0, 5);
            this.MainGrid.Children.Add(this.aiNouns, 0, 5);
            this.MainGrid.Children.Add(this.imgReloadNouns, 0, 5);

            this.MainGrid.Children.Add(this.statPrepositionLabel, 0, 6);
            this.MainGrid.Children.Add(this.statPreposition, 0, 7);
            this.MainGrid.Children.Add(this.aiPreposition, 0, 7);
            this.MainGrid.Children.Add(this.imgReloadPreposition, 0, 7);

            this.MainGrid.Children.Add(this.statAdverbsLabel, 1, 2);
            this.MainGrid.Children.Add(this.statAdverbs, 1, 3);
            this.MainGrid.Children.Add(this.aiAdverbs, 1, 3);
            this.MainGrid.Children.Add(this.imgReloadAdverbs, 1, 3);

            this.MainGrid.Children.Add(this.statAdjectiveLabel, 1, 4);
            this.MainGrid.Children.Add(this.statAdjective, 1, 5);
            this.MainGrid.Children.Add(this.aiAdjective, 1, 5);
            this.MainGrid.Children.Add(this.imgReloadAdjective, 1, 5);

            this.MainGrid.Children.Add(this.statUnclassifiedLabel, 1, 6);
            this.MainGrid.Children.Add(this.statUnclassified, 1, 7);
            this.MainGrid.Children.Add(this.aiUnclassified, 1, 7);
            this.MainGrid.Children.Add(this.imgReloadUnclassified, 1, 7);

            this.statPreposition.IsVisible = true;
            this.statPrepositionLabel.IsVisible = true;

            this.statUnclassified.IsVisible = true;
            this.statUnclassifiedLabel.IsVisible = true;

            // Hide labels and show the recompute indicator
            this.statAdjective.IsVisible = !isTextEditorEmpty;
            this.imgReloadAdjective.IsVisible = isTextEditorEmpty;
            this.statAdverbs.IsVisible = !isTextEditorEmpty;
            this.imgReloadAdverbs.IsVisible = isTextEditorEmpty;
            this.statNouns.IsVisible = !isTextEditorEmpty;
            this.imgReloadNouns.IsVisible = isTextEditorEmpty;
            this.statVerbs.IsVisible = !isTextEditorEmpty;
            this.imgReloadVerbs.IsVisible = isTextEditorEmpty;
            this.statPreposition.IsVisible = !isTextEditorEmpty;
            this.imgReloadPreposition.IsVisible = isTextEditorEmpty;
            this.statUnclassified.IsVisible = !isTextEditorEmpty;
            this.imgReloadUnclassified.IsVisible = isTextEditorEmpty;
        }

        /// <summary>
        /// Apply portrait layout for smarthphones.
        /// </summary>
        private void ApplyPortraitLayoutPhones()
        {
            bool isTextEditorEmpty = string.IsNullOrWhiteSpace(this.ExContentEditor.Text);
            this.MainGrid.Children.Clear();

            this.MainGrid.RowDefinitions = new RowDefinitionCollection()
                {
                    { new RowDefinition() { Height = new GridLength(2, GridUnitType.Star) } },
                    { new RowDefinition() { Height = new GridLength(2, GridUnitType.Star) } },
                    { new RowDefinition() { Height = new GridLength(20, GridUnitType.Star) } },
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } },
                    { new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) } },
                };

            this.MainGrid.ColumnDefinitions = new ColumnDefinitionCollection()
                {
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } },
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } },
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } },
                    { new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) } },
                };

            this.MainGrid.Children.Add(this.ExTitleLabel, 0, 0);
            Grid.SetColumnSpan(this.ExTitleLabel, 4);

            this.MainGrid.Children.Add(this.ExDescriptionLabel, 0, 1);
            Grid.SetColumnSpan(this.ExDescriptionLabel, 3);

            this.MainGrid.Children.Add(this.ExCountLabel, 3, 1);

            this.MainGrid.Children.Add(this.ExContentEditor, 0, 2);
            Grid.SetColumnSpan(this.ExContentEditor, 4);

            this.MainGrid.Children.Add(this.statVerbsLabel, 0, 3);
            this.MainGrid.Children.Add(this.statVerbs, 0, 4);
            this.MainGrid.Children.Add(this.aiVerbs, 0, 4);
            this.MainGrid.Children.Add(this.imgReloadVerbs, 0, 4);

            this.MainGrid.Children.Add(this.statNounsLabel, 1, 3);
            this.MainGrid.Children.Add(this.statNouns, 1, 4);
            this.MainGrid.Children.Add(this.aiNouns, 1, 4);
            this.MainGrid.Children.Add(this.imgReloadNouns, 1, 4);

            this.MainGrid.Children.Add(this.statAdverbsLabel, 2, 3);
            this.MainGrid.Children.Add(this.statAdverbs, 2, 4);
            this.MainGrid.Children.Add(this.aiAdverbs, 2, 4);
            this.MainGrid.Children.Add(this.imgReloadAdverbs, 2, 4);

            this.MainGrid.Children.Add(this.statAdjectiveLabel, 3, 3);
            this.MainGrid.Children.Add(this.statAdjective, 3, 4);
            this.MainGrid.Children.Add(this.aiAdjective, 3, 4);
            this.MainGrid.Children.Add(this.imgReloadAdjective, 3, 4);

            this.statPreposition.IsVisible = false;
            this.statPrepositionLabel.IsVisible = false;
            this.aiPreposition.IsVisible = false;
            this.imgReloadPreposition.IsVisible = false;

            this.statUnclassified.IsVisible = false;
            this.statUnclassifiedLabel.IsVisible = false;
            this.aiUnclassified.IsVisible = false;
            this.imgReloadUnclassified.IsVisible = false;

            // Hide labels and show the recompute indicator
            this.statAdjective.IsVisible = !isTextEditorEmpty;
            this.imgReloadAdjective.IsVisible = isTextEditorEmpty;
            this.statAdverbs.IsVisible = !isTextEditorEmpty;
            this.imgReloadAdverbs.IsVisible = isTextEditorEmpty;
            this.statNouns.IsVisible = !isTextEditorEmpty;
            this.imgReloadNouns.IsVisible = isTextEditorEmpty;
            this.statVerbs.IsVisible = !isTextEditorEmpty;
            this.imgReloadVerbs.IsVisible = isTextEditorEmpty;
        }

        /// <summary>
        /// Event handler
        /// </summary>
        /// <param name="sender">Sender param</param>
        /// <param name="e">evet param</param>
        private async void ShowRefTextButton_Clicked(object sender, EventArgs e)
        {
            string messageText;
            if (string.IsNullOrEmpty(this.ex.PreliminaryText))
            {
                messageText = string.Format(CultureInfo.CurrentUICulture, Properties.Resources.Exercise_ExerciseSpecificationsText_WithoutReferenceText, this.ex.Description);
            }
            else
            {
                messageText = string.Format(CultureInfo.CurrentUICulture, Properties.Resources.Exercise_ExerciseSpecificationsText_WithReferenceText, this.ex.Description, this.ex.PreliminaryText);
            }

            await this.DisplayAlert(Properties.Resources.Exercise_ExerciseSpecifications, messageText, Properties.Resources.ButtonHide);
        }

        /// <summary>
        /// Event handler
        /// </summary>
        /// <param name="sender">Sender param</param>
        /// <param name="e">evet param</param>
        private async void LoadButton_Clicked(object sender, EventArgs e)
        {
            if (!await ConnectivityCheck.AskToEnableConnectivity(this))
            {
                try
                {
                    ExerciseApi exAPI = new ExerciseApi(App.OAuth2Account, this.ex.Uid);
                    Exercise result = await exAPI.CallEndpointAsExerciseModel();
                    if (!(result is EssayExercise))
                    {
                        throw new NotImplementedException(Properties.Resources.EssayExerciseViewExerciseTypeNotSupported);
                    }

                    await this.FillData((EssayExercise)result);
                }
                catch (NotImplementedException ex)
                {
                    // This shouldn't happen
                    await Tools.Logger.LogWithErrorMessage(this, "LoadButton_Clicked - This shouln't happen!", ex);
                }
                catch (UnsuccessfulApiCallException ex)
                {
                    Tools.Logger.Log(this.GetType().ToString(), "LoadButton_Clicked method", ex);
                }
                catch (Exception ex)
                {
                    Tools.Logger.Log(this.GetType().ToString(), "LoadButton_Clicked method", ex);
                }
            }
        }

        /// <summary>
        /// Resets the current exercise status.
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arg</param>
        private async void ResetButton_Clicked(object sender, EventArgs e)
        {
            if (await this.DisplayAlert(Properties.Resources.EssayExerciseView_ResetAlertTitle, Properties.Resources.EssayExerciseView_ResetAlertMessage, Properties.Resources.ButtonOK, Properties.Resources.ButtonCancel))
            {
                this.ExContentEditor.Text = string.Empty;
                this.ExContentEditor_TextChanged(null, null);
                this._changeActivityIndicatorsStatus(false); // Stop Activity Indicators.

                // Hide labels and show the recompute indicator
                this.statAdjective.IsVisible = false;
                this.imgReloadAdjective.IsVisible = true;
                this.statAdverbs.IsVisible = false;
                this.imgReloadAdverbs.IsVisible = true;
                this.statNouns.IsVisible = false;
                this.imgReloadNouns.IsVisible = true;
                this.statVerbs.IsVisible = false;
                this.imgReloadVerbs.IsVisible = true;
                this.statPreposition.IsVisible = false;
                this.imgReloadPreposition.IsVisible = true;
                this.statUnclassified.IsVisible = false;
                this.imgReloadUnclassified.IsVisible = true;
            }
        }

        /// <summary>
        /// Event handler for the changed text
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">TextChanged event arg</param>
        private void ExContentEditor_TextChanged(object sender, TextChangedEventArgs e)
        {
            int count = Regex.Matches(this.ExContentEditor.Text, "\\w+").Cast<Match>().Select(match => match.Value).Count();
            double value = (count - this.ex.MinimumWords) / (double)this.ex.MaximumWords;
            this.ExCountLabel.Text = string.Format(System.Globalization.CultureInfo.CurrentUICulture, Properties.Resources.EssayExerciseViewCountLabelText, count, this.ex.MaximumWords);

            if (value <= 0 || value >= 1)
            {
                this.ex.Status = ExerciseStatus.NotCompleted;
            }
            else
            {
                this.ex.Status = ExerciseStatus.Satisfactory;
            }

            this.ex.Contents = this.ExContentEditor.Text;

            // Hide labels and show the recompute indicator
            this.statAdjective.IsVisible = false;
            this.imgReloadAdjective.IsVisible = true;
            this.statAdverbs.IsVisible = false;
            this.imgReloadAdverbs.IsVisible = true;
            this.statNouns.IsVisible = false;
            this.imgReloadNouns.IsVisible = true;
            this.statVerbs.IsVisible = false;
            this.imgReloadVerbs.IsVisible = true;
            this.statPreposition.IsVisible = false;
            this.imgReloadPreposition.IsVisible = true;
            this.statUnclassified.IsVisible = false;
            this.imgReloadUnclassified.IsVisible = true;
        }

        /// <summary>
        /// Fill the fields with the appropriate data
        /// </summary>
        /// <param name="essex"><see cref="EssayExercise"/> param</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task FillData(EssayExercise essex)
        {
            if (Device.Idiom == TargetIdiom.Desktop || Device.Idiom == TargetIdiom.Tablet)
            {
                essex.ExcludeFunctionalWords = false;
            }

            this.ex = essex;

            this.Title = this.ExTypeOfExercise + " " + this.ExLevel;
            this.ExTitleLabel.Text = this.ExTitle;
            this.ExDescriptionLabel.Text = string.Format(Properties.Resources.EssayExercise_MinMaxWords, this.ex.MinimumWords, this.ex.MaximumWords);

            this.ExContentEditor.Text = this.ExContent;

            this.ExContentEditor.TextChanged += this.ExContentEditor_TextChanged;

            // Reanalize only if the exercise was already (partially) done
            if (this.ex.Contents.Length > 0)
            {
                try
                {
                    await this.AnalyzeText();
                }
                catch (OperationCanceledException ex)
                {
                    Tools.Logger.Log(this.GetType().ToString(), ex);
                }
            }
            else
            {
                this.ExCountLabel.Text = string.Format(Properties.Resources.EssayExerciseViewCountLabelText, 0, this.ex.MaximumWords);
            }

            // Force relayout
            this.EssayExerciseView_SizeChanged(null, null);
        }

        /// <summary>
        /// Event handler
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">EventArgs param</param>
        private async void SearchButton_Clicked(object sender, EventArgs e)
        {
            await this.Navigation.PushAsync(new MainSearch());
        }

        /// <summary>
        /// Event handler
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">EventArgs param</param>
        private async void AnalysisButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                await this.AnalyzeText();
            }
            catch (OperationCanceledException ex)
            {
                await Tools.Logger.LogWithErrorMessage(this, ex.Message, ex);
            }
        }

        private async void SubmitButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                int numWords = Regex.Matches(this.ExContentEditor.Text, "\\w+").Cast<Match>().Select(m => m.Value).Count();

                this.ExCountLabel.Text = string.Format(Properties.Resources.EssayExerciseViewCountLabelText, numWords, this.ex.MaximumWords);

                if (numWords < this.ex.MinimumWords)
                {
                    throw new OperationCanceledException(string.Format(Properties.Resources.EssayExerciseViewAnalyzeTextExceptionTextTooShortForSubmission, this.ex.MinimumWords, numWords));
                }
                else if (numWords > this.ex.MaximumWords)
                {
                    throw new OperationCanceledException(string.Format(Properties.Resources.EssayExerciseViewAnalyzeTextExceptionTextTooLongForSubmission, this.ex.MaximumWords, numWords));
                }

                await this.AnalyzeText();
                await this.SaveText();
                await this.Navigation.PushAsync(new ExerciseAnalysis(this.ex));
            }
            catch (OperationCanceledException ex)
            {
                await Tools.Logger.LogWithErrorMessage(this, ex.Message, ex);
            }
        }

        /// <summary>
        /// Enable or disable the activity indicator icon.
        /// </summary>
        /// <param name="activate">True = show activity indicator.</param>
        private void _changeActivityIndicatorsStatus(bool activate)
        {
            this.aiNouns.IsVisible = activate;
            this.aiNouns.IsRunning = activate;
            this.statNouns.IsVisible = !activate;

            this.aiAdverbs.IsVisible = activate;
            this.aiAdverbs.IsRunning = activate;
            this.statAdverbs.IsVisible = !activate;

            this.aiAdjective.IsVisible = activate;
            this.aiAdjective.IsRunning = activate;
            this.statAdjective.IsVisible = !activate;

            this.aiVerbs.IsVisible = activate;
            this.aiVerbs.IsRunning = activate;
            this.statVerbs.IsVisible = !activate;

            this.aiPreposition.IsVisible = activate;
            this.aiPreposition.IsRunning = activate;
            this.statPreposition.IsVisible = !activate;

            this.aiUnclassified.IsVisible = activate;
            this.aiUnclassified.IsRunning = activate;
            this.statUnclassified.IsVisible = !activate;

            if (Device.Idiom == TargetIdiom.Tablet || Device.Idiom == TargetIdiom.Desktop)
            {
                // FIXME: add the others
            }
        }

        /// <summary>
        /// Perform a full analysis
        /// </summary>
        /// <returns>True if the operation is completed correctly</returns>
        private async Task AnalyzeText()
        {
            if (string.IsNullOrWhiteSpace(this.ExContentEditor.Text))
            {
                throw new AnalysisAbortedOutOfBoundsException(string.Format(Properties.Resources.EssayExerciseViewAnalyzeTextExceptionTextTooShort, this.ex.MinimumWords, 0));
            }

            int numWords = this.GetNumWords();

            this.ExCountLabel.Text = string.Format(Properties.Resources.EssayExerciseViewCountLabelText, numWords, this.ex.MaximumWords);

            if (numWords <= this.ex.MinimumWords * 2 / 3)
            {
                throw new AnalysisAbortedOutOfBoundsException(string.Format(Properties.Resources.EssayExerciseViewAnalyzeTextExceptionTextTooShort, this.ex.MinimumWords, numWords));
            }

            if (this._isAnotherAnalysisRunning)
            {
                throw new AnalysisAbortedException(Properties.Resources.EssayExerciseViewAnalyzeTextExceptionAnotherAnalysisIsRunning);
            }

            this.imgReloadVerbs.IsVisible = false;
            this.imgReloadNouns.IsVisible = false;
            this.imgReloadAdverbs.IsVisible = false;
            this.imgReloadAdjective.IsVisible = false;
            this.imgReloadPreposition.IsVisible = false;
            this.imgReloadUnclassified.IsVisible = false;

            if (this.ExContentEditor.Text.Contains("’"))
            {
                this.ExContentEditor.Text = this.ExContentEditor.Text.Replace("’", "'");
            }

            if (this.ExContentEditor.Text.Contains("`"))
            {
                this.ExContentEditor.Text = this.ExContentEditor.Text.Replace("`", "'");
            }

            if (this.ExContentEditor.Text.Contains("、"))
            {
                this.ExContentEditor.Text = this.ExContentEditor.Text.Replace("、", "'");
            }

            if (this.ExContentEditor.Text.Contains("،"))
            {
                this.ExContentEditor.Text = this.ExContentEditor.Text.Replace("،", "'");
            }

            if (this.ExContentEditor.Text.Contains("‘"))
            {
                this.ExContentEditor.Text = this.ExContentEditor.Text.Replace("‘", "'");
            }

            if (this.ExContentEditor.Text.Contains("“"))
            {
                this.ExContentEditor.Text = this.ExContentEditor.Text.Replace("“", "\"");
            }

            if (this.ExContentEditor.Text.Contains("”"))
            {
                this.ExContentEditor.Text = this.ExContentEditor.Text.Replace("”", "\"");
            }

            Tools.Logger.Log(this.GetType().ToString(), "Replace all the contractions");

            // FIXME this.ex.Contents = OfflineWord.ReplaceAllEnglishContractions(this.ExContentEditor.Text.ToLower());
            Tools.Logger.Log(this.GetType().ToString(), "Start new offline analysis.");
            this._changeActivityIndicatorsStatus(true);

            try
            {
                this.statNouns.Text = string.Format("{0:P0}", await this.ex.GetNumWordsPercentage(new List<PartOfSpeech> { PartOfSpeech.CommonNoun, PartOfSpeech.ProperNoun, PartOfSpeech.PartOfProperNoun }));
                this.statAdverbs.Text = string.Format("{0:P0}", await this.ex.GetNumWordsPercentage(new List<PartOfSpeech> { PartOfSpeech.Adverb }));
                this.statAdjective.Text = string.Format("{0:P0}", await this.ex.GetNumWordsPercentage(new List<PartOfSpeech> { PartOfSpeech.Adjective }));
                this.statVerbs.Text = string.Format("{0:P0}", await this.ex.GetNumWordsPercentage(new List<PartOfSpeech> { PartOfSpeech.Verb, PartOfSpeech.ModalVerb, PartOfSpeech.AuxiliaryVerb }));
                this.statPreposition.Text = string.Format("{0:P0}", await this.ex.GetNumWordsPercentage(new List<PartOfSpeech> { PartOfSpeech.Preposition }));
                this.statUnclassified.Text = string.Format("{0:P0}", await this.ex.GetNumWordsPercentage(new List<PartOfSpeech> { PartOfSpeech.Unclassified }));

                if (Device.Idiom == TargetIdiom.Tablet || Device.Idiom == TargetIdiom.Desktop)
                {
                    int a1Count = (await this.ex.LevelClassification)[LanguageLevelClassification.A1].Count;
                    int a2Count = (await this.ex.LevelClassification)[LanguageLevelClassification.A2].Count;
                    int b1Count = (await this.ex.LevelClassification)[LanguageLevelClassification.B1].Count;
                    int b2Count = (await this.ex.LevelClassification)[LanguageLevelClassification.B2].Count;
                    int c1Count = (await this.ex.LevelClassification)[LanguageLevelClassification.C1].Count;
                    int c2Count = (await this.ex.LevelClassification)[LanguageLevelClassification.C2].Count;

                    int tot = a1Count + a2Count + b1Count + b2Count + c1Count + c2Count;

                    this.A1Content.Text = string.Format("{0:P0}", a1Count / tot);
                    this.A2Content.Text = string.Format("{0:P0}", a2Count / tot);
                    this.B1Content.Text = string.Format("{0:P0}", b1Count / tot);
                    this.B2Content.Text = string.Format("{0:P0}", b2Count / tot);
                    this.C1Content.Text = string.Format("{0:P0}", c1Count / tot);
                    this.C2Content.Text = string.Format("{0:P0}", c2Count / tot);
                }

                this._changeActivityIndicatorsStatus(false);

                this._isAnotherAnalysisRunning = false;
            }
            catch (Exception ex)
            {
                Tools.Logger.Log(this.GetType().ToString(), "Offline analysis didn't exit correctly.", ex);

                bool userChoice = await this.DisplayAlert(
                    Properties.Resources.Error,
                    Properties.Resources.EssayExerciseViewAnalyzeTextExceptionErrorGeneric + ex.Message,
                    Properties.Resources.ButtonRetry,
                    Properties.Resources.ButtonCancel);
                if (userChoice)
                {
                    this._isAnotherAnalysisRunning = false;
                    await this.AnalyzeText();
                }
                else
                {
                    this._isAnotherAnalysisRunning = false;
                    this._changeActivityIndicatorsStatus(false);
                }
            }
        }

        private async Task SaveText()
        {
            try
            {
                int numWords = this.GetNumWords();

                // If the text is within the min/max word limits, submit it to the server
                if (numWords >= this.ex.MinimumWords && numWords <= this.ex.MaximumWords)
                {
                    UserActivityEssay essayActivity = new UserActivityEssay();
                    essayActivity.ActivityId = this.ex.Uid;
                    essayActivity.Text = this.ExContentEditor.Text;
                    ExerciseSubmissionApi submissionEndpoint = new ExerciseSubmissionApi(App.OAuth2Account, essayActivity);
                    await submissionEndpoint.CallEndpointAsync();
                }
            }
            catch (Exception)
            {
                if (await this.DisplayAlert(Properties.Resources.Error, Properties.Resources.EssayExerciseViewSaveExceptionAPI, Properties.Resources.ButtonRetry, Properties.Resources.ButtonCancel))
                {
                    await this.SaveText();
                }
            }
        }

        private int GetNumWords()
        {
            return Regex.Matches(this.ExContentEditor.Text, "\\w+").Cast<Match>().Select(m => m.Value).Count();
        }
    }
}
