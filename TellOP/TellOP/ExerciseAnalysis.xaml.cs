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
// <author>Alessandro Menti</author>

namespace TellOP
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using DataModels;
    using DataModels.Activity;
    using DataModels.Enums;
    using Tools;
    using Xamarin.Forms;

    /// <summary>
    /// A content page displaying the analysis results for a single essay exercise.
    /// </summary>
    public partial class ExerciseAnalysis : ContentPage
    {
        /// <summary>
        /// Current Language
        /// </summary>
        private SupportedLanguage currentLanguage;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExerciseAnalysis"/> class.
        /// </summary>
        /// <param name="ex">The <see cref="EssayExercise"/> containing the exercise to be analyzed.</param>
        public ExerciseAnalysis(EssayExercise ex)
        {
            ex.ExcludeFunctionalWords = false;
            this.currentLanguage = ex.Language;

            ExerciseAnalysisDataModel binding = new ExerciseAnalysisDataModel(ex);
            this.BindingContext = binding;
            this.InitializeComponent();

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
            this.UNKNOWNFrame.BackgroundColor = (Color)langToColorConverter.Convert(LanguageLevelClassification.Unknown, typeof(Color), null, CultureInfo.CurrentUICulture);
        }

        /// <summary>
        /// Called when the user taps on the "Refresh" button.
        /// </summary>
        /// <param name="sender">The object sending the event.</param>
        /// <param name="e">The event parameters.</param>
        private async void Refresh_Clicked(object sender, EventArgs e)
        {
            if (await ConnectivityCheck.AskToEnableConnectivity(this))
            {
                ((ExerciseAnalysisDataModel)this.BindingContext).RefreshAnalysis();
            }
        }

        /// <summary>
        /// Called when the user taps on an item in a word list.
        /// </summary>
        /// <param name="sender">The object sending the event.</param>
        /// <param name="e">The event parameters.</param>
        private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (await ConnectivityCheck.AskToEnableConnectivity(this))
            {
                await this.CallSearchPage(((IWord)e.Item).Term);
            }
        }

        /// <summary>
        /// Call the search page optimized for the multi language.
        /// </summary>
        /// <param name="term">Search Term</param>
        /// <returns>Awaitable object</returns>
        private async Task CallSearchPage(string term)
        {
            switch (this.currentLanguage)
            {
                case SupportedLanguage.Spanish:
                case SupportedLanguage.German:
                    {
                        await this.Navigation.PushAsync(new MainSearch(term));
                        return;
                    }

                default:
                    {
                        await this.Navigation.PushAsync(new SingleWordExploration(term));
                        return;
                    }
            }
        }

        /// <summary>
        /// Called when the user taps on a word in the list.
        /// </summary>
        /// <param name="sender">The object sending the event.</param>
        /// <param name="e">The event parameters.</param>
        private async void TableWord_Tapped(object sender, EventArgs e)
        {
            if (await ConnectivityCheck.AskToEnableConnectivity(this))
            {
                TextCell senderCell = sender as TextCell;
                if (senderCell == null)
                {
                    return;
                }

                if (senderCell.BindingContext is IWord)
                {
                    await this.CallSearchPage(((IWord)senderCell.BindingContext).Term);
                }
                else
                {
                    await this.CallSearchPage(senderCell.Text);
                }
            }
        }

        /// <summary>
        /// Called when the user selects an item in a ListView.
        /// </summary>
        /// <param name="sender">The object sending the event.</param>
        /// <param name="e">The event parameters.</param>
        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        // HACK: since it is impossible to insert a ListView inside a ScrollView and enable nested scrolling (unless
        // we use custom renderers, which are quite easy to do on Android, not so on iOS/UWP), and since TableViews
        // do not support grouping, for now we are populating two TableViews in the code behind.
        // The best way would be subclassing TableView and support typical grouping properties - we will leave this
        // for v2.

        /// <summary>
        /// Called when the user taps on a title label.
        /// </summary>
        /// <param name="sender">The object sending the event.</param>
        /// <param name="e">The event parameters.</param>
        private void Title_Tapped(object sender, EventArgs e)
        {
            if (sender == this.ProfileLevelTitle)
            {
                // Needs || to include the errors in the logic.
                this.EnablePanels(!(this.SectionProfileLevels.IsVisible || this.ErrorProfileLevel.IsVisible), false, false, false);
            }
            else if (sender == this.LexTutorTitle)
            {
                // Needs || to include the errors in the logic.
                this.EnablePanels(false, !(this.SectionLexTutorResults.IsVisible || this.ErrorLexTutor.IsVisible), false, false);
            }
            else if (sender == this.LexTutorFamiliesTitle)
            {
                // Needs || to include the errors in the logic.
                this.EnablePanels(false, false, !(this.SectionLexTutorFamiliesResults.IsVisible || this.ErrorLexTutorFamilies.IsVisible), false);
            }
            else if (sender == this.ExploreTitle)
            {
                this.EnablePanels(false, false, false, !this.WordsByPoSTable.IsVisible);
            }
        }

        /// <summary>
        /// Enable or disable the result panels.
        /// </summary>
        /// <param name="profile">Show/hide the profile panel.</param>
        /// <param name="lextutor">Show/hide the lextutor panel.</param>
        /// <param name="families">Show/hide the lextutor families panel.</param>
        /// <param name="explore">Show/hide the explore panel.</param>
        private async void EnablePanels(bool profile, bool lextutor, bool families, bool explore)
        {
            var e = ((ExerciseAnalysisDataModel)this.BindingContext).Exercise;

            if (profile)
            {
                try
                {
                    // Initialize the list on the first tap.
                    if (this.WordsByLevelTableSection.Count == 0)
                    {
                        await ((ExerciseAnalysisDataModel)this.BindingContext).LevelClassification.Task;
                        this.FillLevelList(((ExerciseAnalysisDataModel)this.BindingContext).LevelClassification.Result[LanguageLevelClassification.A1]);
                    }

                    this.SectionProfileLevels.IsVisible = true;
                    this.WordsByLevelTable.IsVisible = true;
                    this.ProfileLevelTitle.Text = Properties.Resources.ExerciseAnalysis_ProfileLevelTitle_Expanded;
                    this.ErrorProfileLevel.IsVisible = false;
                }
                catch (Exception ex)
                {
                    if (ex is KeyNotFoundException && e.Language != SupportedLanguage.English && e.Language != SupportedLanguage.USEnglish)
                    {
                        this.ErrorProfileLevel.Text = string.Format(Properties.Resources.ExerciseAnalysis_ErrorLanguageUnsupported, e.Language.ToString());
                        Logger.Log(ex.GetType() + ":" + e.Language, "User requested an unsupported language");
                    }
                    else
                    {
                        this.ErrorProfileLevel.Text = Properties.Resources.UnexpectedException_Text;
                        Logger.Log("ExerciseAnalysis-EnablePanels", ex);
                    }

                    this.SectionProfileLevels.IsVisible = false;
                    this.WordsByLevelTable.IsVisible = false;
                    this.ProfileLevelTitle.Text = Properties.Resources.ExerciseAnalysis_ProfileLevelTitle_Expanded;
                    this.ErrorProfileLevel.IsVisible = true;
                }
            }
            else
            {
                this.SectionProfileLevels.IsVisible = false;
                this.WordsByLevelTable.IsVisible = false;
                this.ProfileLevelTitle.Text = Properties.Resources.ExerciseAnalysis_ProfileLevelTitle_Contracted;
                this.ErrorProfileLevel.IsVisible = false;
            }

            if (App.WantsAdvancedReports && (e.Language == SupportedLanguage.English || e.Language == SupportedLanguage.USEnglish))
            {
                this.SectionLexTutorResults.IsVisible = lextutor;
                this.LexTutorTitle.Text = lextutor ? Properties.Resources.ExerciseAnalysis_RelatedRatiosIndices_Expanded : Properties.Resources.ExerciseAnalysis_RelatedRatiosIndices_Contracted;
                this.ErrorLexTutor.IsVisible = false;

                this.SectionLexTutorFamiliesResults.IsVisible = families;
                this.LexTutorFamiliesTitle.Text = families ? Properties.Resources.ExerciseAnalysis_Families_Expanded : Properties.Resources.ExerciseAnalysis_Families_Contracted;
                this.ErrorLexTutorFamilies.IsVisible = false;
            }
            else
            {
                this.SectionLexTutorResults.IsVisible = false;
                this.SectionLexTutorResults.IsEnabled = false;
                this.LexTutorTitle.Text = lextutor ? Properties.Resources.ExerciseAnalysis_RelatedRatiosIndices_Expanded : Properties.Resources.ExerciseAnalysis_RelatedRatiosIndices_Contracted;
                if (e.Language != SupportedLanguage.English && e.Language != SupportedLanguage.USEnglish)
                {
                    this.ErrorLexTutor.Text = string.Format(Properties.Resources.ExerciseAnalysis_ErrorLanguageUnsupported, e.Language.ToString());
                }
                else
                {
                    this.ErrorLexTutor.Text = Properties.Resources.ExerciseAnalysis_ErrorEnableAdvancedFunctionalities;
                }

                this.ErrorLexTutor.IsVisible = lextutor;

                this.SectionLexTutorFamiliesResults.IsVisible = false;
                this.SectionLexTutorFamiliesResults.IsEnabled = false;
                this.LexTutorFamiliesTitle.Text = lextutor ? Properties.Resources.ExerciseAnalysis_Families_Expanded : Properties.Resources.ExerciseAnalysis_Families_Contracted;
                if (e.Language != SupportedLanguage.English && e.Language != SupportedLanguage.USEnglish)
                {
                    this.ErrorLexTutorFamilies.Text = string.Format(Properties.Resources.ExerciseAnalysis_ErrorLanguageUnsupported, e.Language.ToString());
                }
                else
                {
                    this.ErrorLexTutorFamilies.Text = Properties.Resources.ExerciseAnalysis_ErrorEnableAdvancedFunctionalities;
                }

                this.ErrorLexTutorFamilies.IsVisible = families;
            }

            this.SectionPoS.IsVisible = explore;
            this.WordsByPoSTable.IsVisible = explore;
            this.ExploreTitle.Text = explore ? Properties.Resources.ExerciseAnalysis_ExploreText_Expanded : Properties.Resources.ExerciseAnalysis_ExploreText_Contracted;
        }

        /// <summary>
        /// Called when the user taps on a level frame.
        /// </summary>
        /// <param name="sender">The object sending the event.</param>
        /// <param name="e">The event parameters.</param>
        private async void LevelFrame_Tapped(object sender, EventArgs e)
        {
            ExerciseAnalysisDataModel binding = (ExerciseAnalysisDataModel)this.BindingContext;
            await binding.LevelClassification.Task;

            if (sender == this.A1Frame)
            {
                this.FillLevelList(binding.LevelClassification.Result[LanguageLevelClassification.A1]);
            }
            else if (sender == this.A2Frame)
            {
                this.FillLevelList(binding.LevelClassification.Result[LanguageLevelClassification.A2]);
            }
            else if (sender == this.B1Frame)
            {
                this.FillLevelList(binding.LevelClassification.Result[LanguageLevelClassification.B1]);
            }
            else if (sender == this.B2Frame)
            {
                this.FillLevelList(binding.LevelClassification.Result[LanguageLevelClassification.B2]);
            }
            else if (sender == this.C1Frame)
            {
                this.FillLevelList(binding.LevelClassification.Result[LanguageLevelClassification.C1]);
            }
            else if (sender == this.C2Frame)
            {
                this.FillLevelList(binding.LevelClassification.Result[LanguageLevelClassification.C2]);
            }
            else if (sender == this.UNKNOWNFrame)
            {
                this.FillLevelList(binding.LevelClassification.Result[LanguageLevelClassification.Unknown]);
            }
        }

        private void FillLevelList(List<IWord> words)
        {
            this.WordsByLevelTableSection.Clear();

            // Equivalent to Distinct(), but based on term field and not the whole object
            foreach (IWord w in words.GroupBy(d => new { d.Term }).Select(d => d.First()).ToList())
            {
                TextCell cell = new TextCell();
                cell.SetBinding(TextCell.TextProperty, "Term");
                cell.BindingContext = w;
                cell.Tapped += this.TableWord_Tapped;
                this.WordsByLevelTableSection.Add(cell);
            }

            this.WordsByLevelTable.HeightRequest = words.Count * this.WordsByLevelTable.RowHeight;
        }

        /// <summary>
        /// Called when the user taps on a level frame.
        /// </summary>
        /// <param name="sender">The object sending the event.</param>
        /// <param name="e">The event parameters.</param>
        private async void PoSFrame_Tapped(object sender, EventArgs e)
        {
            ExerciseAnalysisDataModel binding = (ExerciseAnalysisDataModel)this.BindingContext;
            await binding.WordsInTextByPartOfSpeech.Task;

            // TODO: improve localization here!
            if (sender == this.PoS_Adjective_Frame)
            {
                if (!this.PoS_Adjective_Caption.Text.Contains(Properties.Resources.GenericOpenSymbol))
                {
                    this.FillPoSList(binding.WordsInTextByPartOfSpeech.Result, PartOfSpeech.Adjective);
                    this.RestoreOriginalPoSLabels();
                    this.PoS_Adjective_Caption.Text = Properties.Resources.GenericOpenSymbol + Properties.Resources.PartOfSpeech_Adjective;
                }
                else
                {
                    this.PoS_Adjective_Caption.Text = Properties.Resources.PartOfSpeech_Adjective;
                    this.WordsByPoSTableSection.Clear();
                    this.WordsByPoSTable.HeightRequest = 0;
                }
            }
            else if (sender == this.PoS_Adverb_Frame)
            {
                if (!this.PoS_Adverb_Caption.Text.Contains(Properties.Resources.GenericOpenSymbol))
                {
                    this.FillPoSList(binding.WordsInTextByPartOfSpeech.Result, PartOfSpeech.Adverb);
                    this.RestoreOriginalPoSLabels();
                    this.PoS_Adverb_Caption.Text = Properties.Resources.GenericOpenSymbol + Properties.Resources.PartOfSpeech_Adverb;
                }
                else
                {
                    this.PoS_Adverb_Caption.Text = Properties.Resources.PartOfSpeech_Adverb;
                    this.WordsByPoSTableSection.Clear();
                    this.WordsByPoSTable.HeightRequest = 0;
                }
            }
            else if (sender == this.PoS_ClauseOpener_Frame)
            {
                if (!this.PoS_ClauseOpener_Caption.Text.Contains(Properties.Resources.GenericOpenSymbol))
                {
                    this.FillPoSList(binding.WordsInTextByPartOfSpeech.Result, PartOfSpeech.ClauseOpener);
                    this.RestoreOriginalPoSLabels();
                    this.PoS_ClauseOpener_Caption.Text = Properties.Resources.GenericOpenSymbol + Properties.Resources.PartOfSpeech_ClauseOpener;
                }
                else
                {
                    this.PoS_ClauseOpener_Caption.Text = Properties.Resources.PartOfSpeech_ClauseOpener;
                    this.WordsByPoSTableSection.Clear();
                    this.WordsByPoSTable.HeightRequest = 0;
                }
            }
            else if (sender == this.PoS_Conjunction_Frame)
            {
                if (!this.PoS_Conjunction_Caption.Text.Contains(Properties.Resources.GenericOpenSymbol))
                {
                    this.FillPoSList(binding.WordsInTextByPartOfSpeech.Result, PartOfSpeech.Conjunction);
                    this.RestoreOriginalPoSLabels();
                    this.PoS_Conjunction_Caption.Text = Properties.Resources.GenericOpenSymbol + Properties.Resources.PartOfSpeech_Conjunction;
                }
                else
                {
                    this.PoS_Conjunction_Caption.Text = Properties.Resources.PartOfSpeech_Conjunction;
                    this.WordsByPoSTableSection.Clear();
                    this.WordsByPoSTable.HeightRequest = 0;
                }
            }
            else if (sender == this.PoS_Determiner_Frame)
            {
                if (!this.PoS_Determiner_Caption.Text.Contains(Properties.Resources.GenericOpenSymbol))
                {
                    this.FillPoSList(binding.WordsInTextByPartOfSpeech.Result, PartOfSpeech.Determiner);
                    this.RestoreOriginalPoSLabels();
                    this.PoS_Determiner_Caption.Text = Properties.Resources.GenericOpenSymbol + Properties.Resources.PartOfSpeech_Determiner;
                }
                else
                {
                    this.PoS_Determiner_Caption.Text = Properties.Resources.PartOfSpeech_Determiner;
                    this.WordsByPoSTableSection.Clear();
                    this.WordsByPoSTable.HeightRequest = 0;
                }
            }
            else if (sender == this.PoS_DeterminerPronoun_Frame)
            {
                if (!this.PoS_DeterminerPronoun_Caption.Text.Contains(Properties.Resources.GenericOpenSymbol))
                {
                    this.FillPoSList(binding.WordsInTextByPartOfSpeech.Result, PartOfSpeech.DeterminerPronoun);
                    this.RestoreOriginalPoSLabels();
                    this.PoS_DeterminerPronoun_Caption.Text = Properties.Resources.GenericOpenSymbol + Properties.Resources.PartOfSpeech_Determiner_Pronoun;
                }
                else
                {
                    this.PoS_DeterminerPronoun_Caption.Text = Properties.Resources.PartOfSpeech_Determiner_Pronoun;
                    this.WordsByPoSTableSection.Clear();
                    this.WordsByPoSTable.HeightRequest = 0;
                }
            }
            else if (sender == this.PoS_ExistentialParticle_Frame)
            {
                if (!this.PoS_ExistentialParticle_Caption.Text.Contains(Properties.Resources.GenericOpenSymbol))
                {
                    this.FillPoSList(binding.WordsInTextByPartOfSpeech.Result, PartOfSpeech.ExistentialParticle);
                    this.RestoreOriginalPoSLabels();
                    this.PoS_ExistentialParticle_Caption.Text = Properties.Resources.GenericOpenSymbol + Properties.Resources.PartOfSpeech_ExistentialParticle;
                }
                else
                {
                    this.PoS_ExistentialParticle_Caption.Text = Properties.Resources.PartOfSpeech_ExistentialParticle;
                    this.WordsByPoSTableSection.Clear();
                    this.WordsByPoSTable.HeightRequest = 0;
                }
            }
            else if (sender == this.PoS_ForeignWord_Frame)
            {
                if (!this.PoS_ForeignWord_Caption.Text.Contains(Properties.Resources.GenericOpenSymbol))
                {
                    this.FillPoSList(binding.WordsInTextByPartOfSpeech.Result, PartOfSpeech.ForeignWord);
                    this.RestoreOriginalPoSLabels();
                    this.PoS_ForeignWord_Caption.Text = Properties.Resources.GenericOpenSymbol + Properties.Resources.PartOfSpeech_ForeignWord;
                }
                else
                {
                    this.PoS_ForeignWord_Caption.Text = Properties.Resources.PartOfSpeech_ForeignWord;
                    this.WordsByPoSTableSection.Clear();
                    this.WordsByPoSTable.HeightRequest = 0;
                }
            }
            else if (sender == this.PoS_Genitive_Frame)
            {
                if (!this.PoS_Genitive_Caption.Text.Contains(Properties.Resources.GenericOpenSymbol))
                {
                    this.FillPoSList(binding.WordsInTextByPartOfSpeech.Result, PartOfSpeech.Genitive);
                    this.RestoreOriginalPoSLabels();
                    this.PoS_Genitive_Caption.Text = Properties.Resources.GenericOpenSymbol + Properties.Resources.PartOfSpeech_Genitive;
                }
                else
                {
                    this.PoS_Genitive_Caption.Text = Properties.Resources.PartOfSpeech_Genitive;
                    this.WordsByPoSTableSection.Clear();
                    this.WordsByPoSTable.HeightRequest = 0;
                }
            }
            else if (sender == this.PoS_InfinitiveMarker_Frame)
            {
                if (!this.PoS_InfinitiveMarker_Caption.Text.Contains(Properties.Resources.GenericOpenSymbol))
                {
                    this.FillPoSList(binding.WordsInTextByPartOfSpeech.Result, PartOfSpeech.InfinitiveMarker);
                    this.RestoreOriginalPoSLabels();
                    this.PoS_InfinitiveMarker_Caption.Text = Properties.Resources.GenericOpenSymbol + Properties.Resources.PartOfSpeech_InfinitiveMarker;
                }
                else
                {
                    this.PoS_InfinitiveMarker_Caption.Text = Properties.Resources.PartOfSpeech_InfinitiveMarker;
                    this.WordsByPoSTableSection.Clear();
                    this.WordsByPoSTable.HeightRequest = 0;
                }
            }
            else if (sender == this.PoS_InterjectionOrDiscourseMarker_Frame)
            {
                if (!this.PoS_InterjectionOrDiscourseMarker_Caption.Text.Contains(Properties.Resources.GenericOpenSymbol))
                {
                    this.FillPoSList(binding.WordsInTextByPartOfSpeech.Result, PartOfSpeech.InterjectionOrDiscourseMarker);
                    this.RestoreOriginalPoSLabels();
                    this.PoS_InterjectionOrDiscourseMarker_Caption.Text = Properties.Resources.GenericOpenSymbol + Properties.Resources.PartOfSpeech_Interjection;
                }
                else
                {
                    this.PoS_InterjectionOrDiscourseMarker_Caption.Text = Properties.Resources.PartOfSpeech_Interjection;
                    this.WordsByPoSTableSection.Clear();
                    this.WordsByPoSTable.HeightRequest = 0;
                }
            }
            else if (sender == this.PoS_LetterAsWord_Frame)
            {
                if (!this.PoS_LetterAsWord_Caption.Text.Contains(Properties.Resources.GenericOpenSymbol))
                {
                    this.FillPoSList(binding.WordsInTextByPartOfSpeech.Result, PartOfSpeech.LetterAsWord);
                    this.RestoreOriginalPoSLabels();
                    this.PoS_LetterAsWord_Caption.Text = Properties.Resources.GenericOpenSymbol + Properties.Resources.PartOfSpeech_LetterAsWord;
                }
                else
                {
                    this.PoS_LetterAsWord_Caption.Text = Properties.Resources.PartOfSpeech_LetterAsWord;
                    this.WordsByPoSTableSection.Clear();
                    this.WordsByPoSTable.HeightRequest = 0;
                }
            }
            else if (sender == this.PoS_NegativeMarker_Frame)
            {
                if (!this.PoS_NegativeMarker_Caption.Text.Contains(Properties.Resources.GenericOpenSymbol))
                {
                    this.FillPoSList(binding.WordsInTextByPartOfSpeech.Result, PartOfSpeech.NegativeMarker);
                    this.RestoreOriginalPoSLabels();
                    this.PoS_NegativeMarker_Caption.Text = Properties.Resources.GenericOpenSymbol + Properties.Resources.PartOfSpeech_NegativeMarker;
                }
                else
                {
                    this.PoS_NegativeMarker_Caption.Text = Properties.Resources.PartOfSpeech_NegativeMarker;
                    this.WordsByPoSTableSection.Clear();
                    this.WordsByPoSTable.HeightRequest = 0;
                }
            }
            else if (sender == this.PoS_CommonNoun_Frame)
            {
                if (!this.PoS_CommonNoun_Caption.Text.Contains(Properties.Resources.GenericOpenSymbol))
                {
                    this.FillPoSList(binding.WordsInTextByPartOfSpeech.Result, PartOfSpeech.CommonNoun);
                    this.RestoreOriginalPoSLabels();
                    this.PoS_CommonNoun_Caption.Text = Properties.Resources.GenericOpenSymbol + Properties.Resources.PartOfSpeech_CommonNoun;
                }
                else
                {
                    this.PoS_CommonNoun_Caption.Text = Properties.Resources.PartOfSpeech_CommonNoun;
                    this.WordsByPoSTableSection.Clear();
                    this.WordsByPoSTable.HeightRequest = 0;
                }
            }
            else if (sender == this.PoS_ProperNoun_Frame)
            {
                if (!this.PoS_ProperNoun_Caption.Text.Contains(Properties.Resources.GenericOpenSymbol))
                {
                    this.FillPoSList(binding.WordsInTextByPartOfSpeech.Result, PartOfSpeech.ProperNoun);
                    this.RestoreOriginalPoSLabels();
                    this.PoS_ProperNoun_Caption.Text = Properties.Resources.GenericOpenSymbol + Properties.Resources.PartOfSpeech_ProperNoun;
                }
                else
                {
                    this.PoS_ProperNoun_Caption.Text = Properties.Resources.PartOfSpeech_ProperNoun;
                    this.WordsByPoSTableSection.Clear();
                    this.WordsByPoSTable.HeightRequest = 0;
                }
            }
            else if (sender == this.PoS_PartOfProperNoun_Frame)
            {
                if (!this.PoS_PartOfProperNoun_Caption.Text.Contains(Properties.Resources.GenericOpenSymbol))
                {
                    this.FillPoSList(binding.WordsInTextByPartOfSpeech.Result, PartOfSpeech.PartOfProperNoun);
                    this.RestoreOriginalPoSLabels();
                    this.PoS_PartOfProperNoun_Caption.Text = Properties.Resources.GenericOpenSymbol + Properties.Resources.PartOfSpeech_PartProperNoun;
                }
                else
                {
                    this.PoS_PartOfProperNoun_Caption.Text = Properties.Resources.PartOfSpeech_PartProperNoun;
                    this.WordsByPoSTableSection.Clear();
                    this.WordsByPoSTable.HeightRequest = 0;
                }
            }
            else if (sender == this.PoS_CardinalNumber_Frame)
            {
                if (!this.PoS_CardinalNumber_Caption.Text.Contains(Properties.Resources.GenericOpenSymbol))
                {
                    this.FillPoSList(binding.WordsInTextByPartOfSpeech.Result, PartOfSpeech.CardinalNumber);
                    this.RestoreOriginalPoSLabels();
                    this.PoS_CardinalNumber_Caption.Text = Properties.Resources.GenericOpenSymbol + Properties.Resources.PartOfSpeech_CardinalNumber;
                }
                else
                {
                    this.PoS_CardinalNumber_Caption.Text = Properties.Resources.PartOfSpeech_CardinalNumber;
                    this.WordsByPoSTableSection.Clear();
                    this.WordsByPoSTable.HeightRequest = 0;
                }
            }
            else if (sender == this.PoS_Ordinal_Frame)
            {
                if (!this.PoS_Ordinal_Caption.Text.Contains(Properties.Resources.GenericOpenSymbol))
                {
                    this.FillPoSList(binding.WordsInTextByPartOfSpeech.Result, PartOfSpeech.Ordinal);
                    this.RestoreOriginalPoSLabels();
                    this.PoS_Ordinal_Caption.Text = Properties.Resources.GenericOpenSymbol + Properties.Resources.PartOfSpeech_Ordinal;
                }
                else
                {
                    this.PoS_Ordinal_Caption.Text = Properties.Resources.PartOfSpeech_Ordinal;
                    this.WordsByPoSTableSection.Clear();
                    this.WordsByPoSTable.HeightRequest = 0;
                }
            }
            else if (sender == this.PoS_Preposition_Frame)
            {
                if (!this.PoS_Preposition_Caption.Text.Contains(Properties.Resources.GenericOpenSymbol))
                {
                    this.FillPoSList(binding.WordsInTextByPartOfSpeech.Result, PartOfSpeech.Preposition);
                    this.RestoreOriginalPoSLabels();
                    this.PoS_Preposition_Caption.Text = Properties.Resources.GenericOpenSymbol + Properties.Resources.PartOfSpeech_Preposition;
                }
                else
                {
                    this.PoS_Preposition_Caption.Text = Properties.Resources.PartOfSpeech_Preposition;
                    this.WordsByPoSTableSection.Clear();
                    this.WordsByPoSTable.HeightRequest = 0;
                }
            }
            else if (sender == this.PoS_Pronoun_Frame)
            {
                if (!this.PoS_Pronoun_Caption.Text.Contains(Properties.Resources.GenericOpenSymbol))
                {
                    this.FillPoSList(binding.WordsInTextByPartOfSpeech.Result, PartOfSpeech.Pronoun);
                    this.RestoreOriginalPoSLabels();
                    this.PoS_Pronoun_Caption.Text = Properties.Resources.GenericOpenSymbol + Properties.Resources.PartOfSpeech_Pronoun;
                }
                else
                {
                    this.PoS_Pronoun_Caption.Text = Properties.Resources.PartOfSpeech_Pronoun;
                    this.WordsByPoSTableSection.Clear();
                    this.WordsByPoSTable.HeightRequest = 0;
                }
            }
            else if (sender == this.PoS_Unclassified_Frame)
            {
                if (!this.PoS_Unclassified_Caption.Text.Contains(Properties.Resources.GenericOpenSymbol))
                {
                    this.FillPoSList(binding.WordsInTextByPartOfSpeech.Result, PartOfSpeech.Unclassified);
                    this.RestoreOriginalPoSLabels();
                    this.PoS_Unclassified_Caption.Text = Properties.Resources.GenericOpenSymbol + Properties.Resources.PartOfSpeech_Unclassified;
                }
                else
                {
                    this.PoS_Unclassified_Caption.Text = Properties.Resources.PartOfSpeech_Unclassified;
                    this.WordsByPoSTableSection.Clear();
                    this.WordsByPoSTable.HeightRequest = 0;
                }
            }
            else if (sender == this.PoS_Verb_Frame)
            {
                if (!this.PoS_Verb_Caption.Text.Contains(Properties.Resources.GenericOpenSymbol))
                {
                    this.FillPoSList(binding.WordsInTextByPartOfSpeech.Result, PartOfSpeech.Verb);
                    this.RestoreOriginalPoSLabels();
                    this.PoS_Verb_Caption.Text = Properties.Resources.GenericOpenSymbol + Properties.Resources.PartOfSpeech_Verb;
                }
                else
                {
                    this.PoS_Verb_Caption.Text = Properties.Resources.PartOfSpeech_Verb;
                    this.WordsByPoSTableSection.Clear();
                    this.WordsByPoSTable.HeightRequest = 0;
                }
            }
            else if (sender == this.PoS_ModalVerb_Frame)
            {
                if (!this.PoS_ModalVerb_Caption.Text.Contains(Properties.Resources.GenericOpenSymbol))
                {
                    this.FillPoSList(binding.WordsInTextByPartOfSpeech.Result, PartOfSpeech.ModalVerb);
                    this.RestoreOriginalPoSLabels();
                    this.PoS_ModalVerb_Caption.Text = Properties.Resources.GenericOpenSymbol + Properties.Resources.PartOfSpeech_ModalVerb;
                }
                else
                {
                    this.PoS_ModalVerb_Caption.Text = Properties.Resources.PartOfSpeech_ModalVerb;
                    this.WordsByPoSTableSection.Clear();
                    this.WordsByPoSTable.HeightRequest = 0;
                }
            }
            else if (sender == this.PoS_AuxiliaryVerb_Frame)
            {
                if (!this.PoS_AuxiliaryVerb_Caption.Text.Contains(Properties.Resources.GenericOpenSymbol))
                {
                    this.FillPoSList(binding.WordsInTextByPartOfSpeech.Result, PartOfSpeech.AuxiliaryVerb);
                    this.RestoreOriginalPoSLabels();
                    this.PoS_AuxiliaryVerb_Caption.Text = Properties.Resources.GenericOpenSymbol + Properties.Resources.PartOfSpeech_AuxiliaryVerb;
                }
                else
                {
                    this.PoS_AuxiliaryVerb_Caption.Text = Properties.Resources.PartOfSpeech_AuxiliaryVerb;
                    this.WordsByPoSTableSection.Clear();
                    this.WordsByPoSTable.HeightRequest = 0;
                }
            }
            else
            {
                Tools.Logger.Log("ExerciseAnalysis", "Missing frame? " + sender.ToString(), new KeyNotFoundException("Missing Frame"));
            }
        }

        /// <summary>
        /// Restores all the titles with the correct name (removes the +/- indicator)
        /// </summary>
        private void RestoreOriginalPoSLabels()
        {
            this.PoS_Adjective_Caption.Text = Properties.Resources.PartOfSpeech_Adjective;
            this.PoS_Adverb_Caption.Text = Properties.Resources.PartOfSpeech_Adverb;
            this.PoS_ClauseOpener_Caption.Text = Properties.Resources.PartOfSpeech_ClauseOpener;
            this.PoS_Conjunction_Caption.Text = Properties.Resources.PartOfSpeech_Conjunction;
            this.PoS_Determiner_Caption.Text = Properties.Resources.PartOfSpeech_Determiner;
            this.PoS_DeterminerPronoun_Caption.Text = Properties.Resources.PartOfSpeech_Determiner_Pronoun;
            this.PoS_ExistentialParticle_Caption.Text = Properties.Resources.PartOfSpeech_ExistentialParticle;
            this.PoS_ForeignWord_Caption.Text = Properties.Resources.PartOfSpeech_ForeignWord;
            this.PoS_Genitive_Caption.Text = Properties.Resources.PartOfSpeech_Genitive;
            this.PoS_InfinitiveMarker_Caption.Text = Properties.Resources.PartOfSpeech_InfinitiveMarker;
            this.PoS_InterjectionOrDiscourseMarker_Caption.Text = Properties.Resources.PartOfSpeech_Interjection;
            this.PoS_LetterAsWord_Caption.Text = Properties.Resources.PartOfSpeech_LetterAsWord;
            this.PoS_NegativeMarker_Caption.Text = Properties.Resources.PartOfSpeech_NegativeMarker;
            this.PoS_CommonNoun_Caption.Text = Properties.Resources.PartOfSpeech_CommonNoun;
            this.PoS_ProperNoun_Caption.Text = Properties.Resources.PartOfSpeech_ProperNoun;
            this.PoS_PartOfProperNoun_Caption.Text = Properties.Resources.PartOfSpeech_PartProperNoun;
            this.PoS_CardinalNumber_Caption.Text = Properties.Resources.PartOfSpeech_CardinalNumber;
            this.PoS_Ordinal_Caption.Text = Properties.Resources.PartOfSpeech_Ordinal;
            this.PoS_Preposition_Caption.Text = Properties.Resources.PartOfSpeech_Preposition;
            this.PoS_Pronoun_Caption.Text = Properties.Resources.PartOfSpeech_Pronoun;
            this.PoS_Unclassified_Caption.Text = Properties.Resources.PartOfSpeech_Unclassified;
            this.PoS_Verb_Caption.Text = Properties.Resources.PartOfSpeech_Verb;
            this.PoS_ModalVerb_Caption.Text = Properties.Resources.PartOfSpeech_ModalVerb;
            this.PoS_AuxiliaryVerb_Caption.Text = Properties.Resources.PartOfSpeech_AuxiliaryVerb;
        }

        /// <summary>
        /// Fills the table detailing the words (filtered by part of speech).
        /// </summary>
        /// <param name="words">A <see cref="ReadOnlyDictionary{PartOfSpeech, ICollection}"/> containing the words
        /// grouped by <see cref="PartOfSpeech"/>.</param>
        /// <param name="group">The <see cref="PartOfSpeech"/> to use for filtering.</param>
        private void FillPoSList(ReadOnlyDictionary<PartOfSpeech, ICollection<IWord>> words, PartOfSpeech group)
        {
            this.WordsByPoSTableSection.Clear();
            this.WordsByPoSTable.IsVisible = true;

            int rowCount = 0;
            try
            {
                // Distinct word based on Term
                foreach (IWord w in words[group].GroupBy(d => new { d.Term }).Select(d => d.First()).ToList())
                {
                    TextCell cell = new TextCell();
                    cell.SetBinding(TextCell.TextProperty, "Term");
                    cell.BindingContext = w;
                    cell.Tapped += this.TableWord_Tapped;
                    this.WordsByPoSTableSection.Add(cell);
                    ++rowCount;
                }
            }
            catch (KeyNotFoundException ex)
            {
                Tools.Logger.Log("ExerciseAnalysis.FillPoSList", "PoS " + group + " not found.", ex);
                this.WordsByPoSTableSection.Clear();
                this.WordsByPoSTableSection.Add(new TextCell() { Text = Properties.Resources.ExerciseAnalysis_EmptyPoSList });
                rowCount = 1;
            }

            this.WordsByPoSTable.HeightRequest = rowCount * this.WordsByPoSTable.RowHeight;
        }
    }
}
