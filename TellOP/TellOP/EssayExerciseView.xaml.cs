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

            if (isPortrait)
            {
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
            }
            else
            {
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
            }
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

            if (value < 0)
            {
                value = 0d;
            }

            if (value > 1)
            {
                value = 1d;
            }

            this.ex.Contents = this.ExContentEditor.Text;
        }

        /// <summary>
        /// Fill the fields with the appropriate data
        /// </summary>
        /// <param name="essex"><see cref="EssayExercise"/> param</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task FillData(EssayExercise essex)
        {
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
