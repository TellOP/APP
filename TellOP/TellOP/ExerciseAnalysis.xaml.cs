// <copyright file="ExerciseAnalysis.xaml.cs" company="University of Murcia">
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
    using DataModels;
    using DataModels.Activity;
    using DataModels.Enums;
    using ViewModels;
    using Xamarin.Forms;

    /// <summary>
    /// Display the exercise results
    /// </summary>
    public partial class ExerciseAnalysis : ContentPage
    {
        /// <summary>
        /// EssayExercise object
        /// </summary>
        private EssayExercise ex;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExerciseAnalysis"/> class.
        /// </summary>
        /// <param name="ex">EssayExercise object</param>
        public ExerciseAnalysis(EssayExercise ex)
        {
            this.ex = ex;

            this.InitializeComponent();

            this.analysisButton.Icon = "toolbar_analysis.png";
            this.analysisButton.Clicked += this.AnalysisButton_Clicked;

            this.ExTitleLabel.Text = this.ExTitle;
            this.ExTitleSub.Text = this.ExSub;

            this._initSectionsRelatedToOfflineContent();
            this._initSectionsRelatedToOnlineContent();
        }

        /// <summary>
        /// Gets the exercise title for binding
        /// </summary>
        public string ExTitle
        {
            get
            {
                return this.ex.Title;
            }
        }

        /// <summary>
        /// Gets the exercise subtitle for binding
        /// </summary>
        public string ExSub
        {
            get
            {
                return this.ExTypeOfExercise + ", " + this.ExLanguage + " " + this.ExLevel;
            }
        }

        /// <summary>
        /// Gets the exercise language for binding
        /// </summary>
        public SupportedLanguage ExLanguage
        {
            get
            {
                return this.ex.Language;
            }
        }

        /// <summary>
        /// Gets the exercise level for binding
        /// </summary>
        public LanguageLevelClassification ExLevel
        {
            get
            {
                return this.ex.Level;
            }
        }

        /// <summary>
        /// Gets the exercise type for binding
        /// </summary>
        public string ExTypeOfExercise
        {
            get
            {
                return this.ex.ToNiceString();
            }
        }

        /// <summary>
        /// Gets the exercise description for binding
        /// </summary>
        public string ExDescription
        {
            get
            {
                return this.ex.Description;
            }
        }

        /// <summary>
        /// Gets the exercise content for binding
        /// </summary>
        public string ExContent
        {
            get
            {
                return this.ex.Contents;
            }
        }

        /// <summary>
        /// Initialize and populate all the sections related to the offline content.
        /// </summary>
        private async void _initSectionsRelatedToOfflineContent()
        {
            await this.ex.PerformFullOfflineAnalysis();
            this._initSectionLanguageLevel();
            this._initSectionExploreYourText();
        }

        /// <summary>
        /// Initialize and populate all the sections related to the online content.
        /// </summary>
        private async void _initSectionsRelatedToOnlineContent()
        {
            await this.ex.PerformFullOfflineAnalysis();

            // Tthe panels are built asynchronously.
            this._initSectionRelatedRatiosAndIndexes();
            this._initSectionKFrequencyDistribution();
        }

        /// <summary>
        /// Initialize Language Level section
        /// </summary>
        private void _initSectionLanguageLevel()
        {
            try
            {
                Dictionary<LanguageLevelClassification, float> levels = this.ex.LevelClassificationDistribution;

                try
                {
                    this.Label_A1.BackgroundColor = LanguageLevelClassification.A1.ToColor();
                    this.Value_A1.Text = string.Empty + Convert.ToInt32(levels[LanguageLevelClassification.A1] * 100) + "%";
                    LanguageLevelWordsListView stackA1Content = new LanguageLevelWordsListView(LanguageLevelClassification.A1, this.ex.LevelClassification[LanguageLevelClassification.A1], this.StackA1);
                    this.Label_A1.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => { this.InvertStatusLanguagePanelDetails(this.StackA1, stackA1Content, this.Label_A1, 0); }) });
                    this.StackA1.Children.Add(stackA1Content);
                }
                catch (Exception ex)
                {
                    Tools.Logger.Log(this, "Error with group A1", ex);
                    this.Label_A1.BackgroundColor = LanguageLevelClassification.A1.ToColor();
                    this.Value_A1.Text = string.Empty + "-";
                    this.StackA1.Children.Add(new StackLayout());
                }

                try
                {
                    this.Label_A2.BackgroundColor = LanguageLevelClassification.A2.ToColor();
                    this.Value_A2.Text = string.Empty + Convert.ToInt32(levels[LanguageLevelClassification.A2] * 100) + "%";
                    LanguageLevelWordsListView stackA2Content = new LanguageLevelWordsListView(LanguageLevelClassification.A2, this.ex.LevelClassification[LanguageLevelClassification.A2], this.StackA2);
                    this.Label_A2.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => { this.InvertStatusLanguagePanelDetails(this.StackA2, stackA2Content, this.Label_A2, 1); }) });
                    this.StackA2.Children.Add(stackA2Content);
                }
                catch (Exception ex)
                {
                    Tools.Logger.Log(this, "Error with group A2", ex);
                    this.Label_A2.BackgroundColor = LanguageLevelClassification.A2.ToColor();
                    this.Value_A2.Text = string.Empty + "-";
                    this.StackA2.Children.Add(new StackLayout());
                }

                try
                {
                    this.Label_B1.BackgroundColor = LanguageLevelClassification.B1.ToColor();
                    this.Value_B1.Text = string.Empty + Convert.ToInt32(levels[LanguageLevelClassification.B1] * 100) + "%";
                    LanguageLevelWordsListView stackB1Content = new LanguageLevelWordsListView(LanguageLevelClassification.B1, this.ex.LevelClassification[LanguageLevelClassification.B1], this.StackB1);
                    this.Label_B1.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => { this.InvertStatusLanguagePanelDetails(this.StackB1, stackB1Content, this.Label_B1, 2); }) });
                    this.StackB1.Children.Add(stackB1Content);
                }
                catch (Exception ex)
                {
                    Tools.Logger.Log(this, "Error with group B1", ex);
                    this.Label_B1.BackgroundColor = LanguageLevelClassification.A2.ToColor();
                    this.Value_B1.Text = string.Empty + "-";
                    this.StackB1.Children.Add(new StackLayout());
                }

                try
                {
                    this.Label_B2.BackgroundColor = LanguageLevelClassification.B2.ToColor();
                    this.Value_B2.Text = string.Empty + Convert.ToInt32(levels[LanguageLevelClassification.B2] * 100) + "%";
                    LanguageLevelWordsListView stackB2Content = new LanguageLevelWordsListView(LanguageLevelClassification.B2, this.ex.LevelClassification[LanguageLevelClassification.B2], this.StackB2);
                    this.Label_B2.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => { this.InvertStatusLanguagePanelDetails(this.StackB2, stackB2Content, this.Label_B2, 3); }) });
                    this.StackB2.Children.Add(stackB2Content);
                }
                catch (Exception ex)
                {
                    Tools.Logger.Log(this, "Error with group B2", ex);
                    this.Label_B2.BackgroundColor = LanguageLevelClassification.B2.ToColor();
                    this.Value_B2.Text = string.Empty + "-";
                    this.StackB2.Children.Add(new StackLayout());
                }

                try
                {
                    this.Label_C1.BackgroundColor = LanguageLevelClassification.C1.ToColor();
                    this.Value_C1.Text = string.Empty + Convert.ToInt32(levels[LanguageLevelClassification.C1] * 100) + "%";
                    LanguageLevelWordsListView stackC1Content = new LanguageLevelWordsListView(LanguageLevelClassification.C1, this.ex.LevelClassification[LanguageLevelClassification.C1], this.StackC1);
                    this.Label_C1.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => { this.InvertStatusLanguagePanelDetails(this.StackC1, stackC1Content, this.Label_C1, 4); }) });
                    this.StackC1.Children.Add(stackC1Content);
                }
                catch (Exception ex)
                {
                    Tools.Logger.Log(this, "Error with group C1", ex);
                    this.Label_C1.BackgroundColor = LanguageLevelClassification.C1.ToColor();
                    this.Value_C1.Text = string.Empty + "-";
                    this.StackC1.Children.Add(new StackLayout());
                }

                try
                {
                    this.Label_C2.BackgroundColor = LanguageLevelClassification.C2.ToColor();
                    this.Value_C2.Text = string.Empty + Convert.ToInt32(levels[LanguageLevelClassification.C2] * 100) + "%";
                    LanguageLevelWordsListView stackC2Content = new LanguageLevelWordsListView(LanguageLevelClassification.C2, this.ex.LevelClassification[LanguageLevelClassification.C2], this.StackC2);
                    this.Label_C2.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => { this.InvertStatusLanguagePanelDetails(this.StackC2, stackC2Content, this.Label_C2, 5); }) });
                    this.StackC2.Children.Add(stackC2Content);
                }
                catch (Exception ex)
                {
                    Tools.Logger.Log(this, "Error with group C2", ex);
                    this.Label_C2.BackgroundColor = LanguageLevelClassification.C2.ToColor();
                    this.Value_C2.Text = string.Empty + "-";
                    this.StackC2.Children.Add(new StackLayout());
                }

                try
                {
                    this.Label_UNKNOWN.BackgroundColor = LanguageLevelClassification.UNKNOWN.ToColor();
                    this.Value_UNKNOWN.Text = string.Empty + Convert.ToInt32(levels[LanguageLevelClassification.UNKNOWN] * 100) + "%";
                    LanguageLevelWordsListView stackUNKNOWNContent = new LanguageLevelWordsListView(LanguageLevelClassification.UNKNOWN, this.ex.LevelClassification[LanguageLevelClassification.UNKNOWN], this.StackUNKNOWN);
                    this.Label_UNKNOWN.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => { this.InvertStatusLanguagePanelDetails(this.StackUNKNOWN, stackUNKNOWNContent, this.Label_UNKNOWN, 6); }) });
                    this.StackUNKNOWN.Children.Add(stackUNKNOWNContent);
                }
                catch (Exception ex)
                {
                    Tools.Logger.Log(this, "Error with group UNKNOWN", ex);
                    this.Label_UNKNOWN.BackgroundColor = LanguageLevelClassification.UNKNOWN.ToColor();
                    this.Value_UNKNOWN.Text = string.Empty + "-";
                    this.StackUNKNOWN.Children.Add(new StackLayout());
                }
            }
            catch (Exception ex)
            {
                Tools.Logger.Log(this, "_initSectionLanguageLevel method", ex);
            }
        }

        private void InvertStatusLanguagePanelDetails(StackLayout stack, LanguageLevelWordsListView stackContent, Frame label, int originGridColumn)
        {
            bool vis = stack.IsVisible;
            if (!vis)
            {
                Grid.SetColumn(label, 0);
                Grid.SetColumnSpan(label, 7);
                stack.IsVisible = true;
                stackContent.InnerFrameVisibilityInverter(true);

                this.Label_A1.IsVisible = false;
                this.Label_A2.IsVisible = false;
                this.Label_B1.IsVisible = false;
                this.Label_B2.IsVisible = false;
                this.Label_C1.IsVisible = false;
                this.Label_C2.IsVisible = false;
                this.Label_UNKNOWN.IsVisible = false;
                label.IsVisible = true;
            }
            else
            {
                Grid.SetColumn(label, originGridColumn);
                Grid.SetColumnSpan(label, 1);
                stack.IsVisible = false;
                stackContent.InnerFrameVisibilityInverter(false);

                this.Label_A1.IsVisible = true;
                this.Label_A2.IsVisible = true;
                this.Label_B1.IsVisible = true;
                this.Label_B2.IsVisible = true;
                this.Label_C1.IsVisible = true;
                this.Label_C2.IsVisible = true;
                this.Label_UNKNOWN.IsVisible = true;
            }
        }

        /// <summary>
        /// Initialize Related Ratios and Indexes section
        /// </summary>
        /// <returns>True if the operation is completed successfully</returns>
        private bool _initSectionRelatedRatiosAndIndexes()
        {
            try
            {
                this.LexTutorWordsInText.Text = string.Empty + this.ex.LexTutorResult.Ratios.WordsInText;
                this.LexTutorDifferentWords.Text = string.Empty + this.ex.LexTutorResult.Ratios.DifferentWords;
                this.LexTutorTypeTokenRatio.Text = string.Empty + this.ex.LexTutorResult.Ratios.TypeTokenRatio;
                this.LexTutorTokensPerType.Text = string.Empty + this.ex.LexTutorResult.Ratios.TokensPerType;

                this.LexTutorTokens.Text = string.Empty + this.ex.LexTutorResult.Ratios.TokensOnList;
                this.LexTutorTypes.Text = string.Empty + this.ex.LexTutorResult.Ratios.TypesOnList;
                this.LexTutorFamilies.Text = string.Empty + this.ex.LexTutorResult.Ratios.FamiliesOnList;
                this.LexTutorTokensPerFamily.Text = string.Empty + this.ex.LexTutorResult.Ratios.TokensPerFamily;
                this.LexTutorTypesPerFamily.Text = string.Empty + this.ex.LexTutorResult.Ratios.TypesPerFamily;

                // TODO: remove the return value
                return true;
            }
            catch (Exception ex)
            {
                Tools.Logger.Log(this, "_initSectionRelatedRatiosAndIndexes method", ex);
                return false;
            }
        }

        /// <summary>
        /// Initialize K-Frequency Distribution section
        /// </summary>
        /// <returns>True if the operation was successful</returns>
        private bool _initSectionKFrequencyDistribution()
        {
            // TODO
            return false;
        }

        /// <summary>
        /// Initialize Explore Your Text section
        /// </summary>
        private void _initSectionExploreYourText()
        {
            // TODO: Possible async improvement? Right now only the Populate method is async but is executed and not waited by the constructor
            this.PoSStack.Children.Add(new PartOfSpeechWordsListView(PartOfSpeech.Adjective, this.ex.Adjectives));
            this.PoSStack.Children.Add(new PartOfSpeechWordsListView(PartOfSpeech.Adverb, this.ex.Adverbs));
            this.PoSStack.Children.Add(new PartOfSpeechWordsListView(PartOfSpeech.CommonNoun, this.ex.CommonNouns));
            this.PoSStack.Children.Add(new PartOfSpeechWordsListView(PartOfSpeech.ProperNoun, this.ex.ProperNouns));
            this.PoSStack.Children.Add(new PartOfSpeechWordsListView(PartOfSpeech.PartOfProperNoun, this.ex.PartsOfProperNouns));
            this.PoSStack.Children.Add(new PartOfSpeechWordsListView(PartOfSpeech.Verb, this.ex.Verbs));
            this.PoSStack.Children.Add(new PartOfSpeechWordsListView(PartOfSpeech.Unclassified, this.ex.UnclassifiedWords));
        }

        /// <summary>
        /// Event handler for analysis button
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">EventArgs object</param>
        private void AnalysisButton_Clicked(object sender, EventArgs e)
        {
            Tools.Logger.Log(this, "Clear the interface and recompute");
            this.PoSStack.Children.Clear();
            this.StackA1.Children.Clear();
            this.StackA2.Children.Clear();
            this.StackB1.Children.Clear();
            this.StackB2.Children.Clear();
            this.StackC1.Children.Clear();
            this.StackC2.Children.Clear();
            this.StackUNKNOWN.Children.Clear();
            this._initSectionsRelatedToOfflineContent();
            this._initSectionsRelatedToOnlineContent();
        }
    }
}
