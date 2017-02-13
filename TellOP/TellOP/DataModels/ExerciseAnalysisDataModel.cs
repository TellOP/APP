// <copyright file="ExerciseAnalysisDataModel.cs" company="University of Murcia">
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

namespace TellOP.DataModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Threading.Tasks;
    using Activity;
    using DataModels.Enums;
    using Nito.AsyncEx;
    using ApiModels.LexTutor;

    /// <summary>
    /// The essay exercise analysis data model.
    /// </summary>
    public class ExerciseAnalysisDataModel : INotifyPropertyChanged
    {
        /// <summary>
        /// The exercise to be analyzed.
        /// </summary>
        private EssayExercise _exercise;

        /// <summary>
        /// The level classification of the exercise text.
        /// </summary>
        private INotifyTaskCompletion<Dictionary<LanguageLevelClassification, List<IWord>>> _levelClassification;

        /// <summary>
        /// The level classification distribution of the exercise text.
        /// </summary>
        private INotifyTaskCompletion<Dictionary<LanguageLevelClassification, float>> _levelClassificationDistribution;

        // LexTutor-related properties.
        // Since each of them depends on the AsyncLazy LexTutorResults property (which is only initialized at runtime)
        // and data binding requires a default value to be available immediately, we can not bind a XAML property to
        // a member of LexTutorResults. We need to wrap them in individual INotifyTaskCompletion properties.

        /// <summary>
        /// The number of words appearing in the text.
        /// </summary>
        private INotifyTaskCompletion<int> _lexTutorWordsInText;

        /// <summary>
        /// The number of different words appearing in the text.
        /// </summary>
        private INotifyTaskCompletion<int> _lexTutorDifferentWords;

        /// <summary>
        /// The type/token ratio of the text.
        /// </summary>
        private INotifyTaskCompletion<float> _lexTutorTypeTokenRatio;

        /// <summary>
        /// The tokens per type number of the text.
        /// </summary>
        private INotifyTaskCompletion<float> _lexTutorTokensPerType;

        /// <summary>
        /// The number of tokens (words) in the text.
        /// </summary>
        private INotifyTaskCompletion<int> _lexTutorTokens;

        /// <summary>
        /// The number of different word types appearing in the text.
        /// </summary>
        private INotifyTaskCompletion<int> _lexTutorTypes;

        /// <summary>
        /// The number of word families appearing in the text.
        /// </summary>
        private INotifyTaskCompletion<int> _lexTutorFamilies;

        /// <summary>
        /// The number of tokens per family.
        /// </summary>
        private INotifyTaskCompletion<float> _lexTutorTokensPerFamily;

        /// <summary>
        /// The number of word types per family.
        /// </summary>
        private INotifyTaskCompletion<float> _lexTutorTypesPerFamily;

        /// <summary>
        /// A read-only list of words appearing in the text grouped by part of speech.
        /// </summary>
        private INotifyTaskCompletion<ReadOnlyDictionary<PartOfSpeech, ICollection<IWord>>> _wordsInTextByPartOfSpeech;

        /// <summary>
        /// A read-only list of words appearing in the text grouped by language level.
        /// </summary>
        private INotifyTaskCompletion<ReadOnlyObservableCollection<Grouping<IWord>>> _wordsInTextByLevel;

        /// <summary>
        /// Lext Tutor Family K1
        /// </summary>
        private INotifyTaskCompletion<LexTutorResultFrequencyDetails> _lexTutorFamilyK1;
        /// <summary>
        /// Lext Tutor Family K2
        /// </summary>
        private INotifyTaskCompletion<LexTutorResultFrequencyDetails> _lexTutorFamilyK2;
        /// <summary>
        /// Lext Tutor Family K3
        /// </summary>
        private INotifyTaskCompletion<LexTutorResultFrequencyDetails> _lexTutorFamilyK3;
        /// <summary>
        /// Lext Tutor Family K4
        /// </summary>
        private INotifyTaskCompletion<LexTutorResultFrequencyDetails> _lexTutorFamilyK4;
        /// <summary>
        /// Lext Tutor Family K5
        /// </summary>
        private INotifyTaskCompletion<LexTutorResultFrequencyDetails> _lexTutorFamilyK5;
        /// <summary>
        /// Lext Tutor Family K6
        /// </summary>
        private INotifyTaskCompletion<LexTutorResultFrequencyDetails> _lexTutorFamilyK6;
        /// <summary>
        /// Lext Tutor Family K7
        /// </summary>
        private INotifyTaskCompletion<LexTutorResultFrequencyDetails> _lexTutorFamilyK7;
        /// <summary>
        /// Lext Tutor Family K8
        /// </summary>
        private INotifyTaskCompletion<LexTutorResultFrequencyDetails> _lexTutorFamilyK8;
        /// <summary>
        /// Lext Tutor Family K9
        /// </summary>
        private INotifyTaskCompletion<LexTutorResultFrequencyDetails> _lexTutorFamilyK9;
        /// <summary>
        /// Lext Tutor Family Offlist
        /// </summary>
        private INotifyTaskCompletion<LexTutorResultFrequencyDetails> _lexTutorFamilyOfflist;
        /// <summary>
        /// Lext Tutor Family Total
        /// </summary>
        private INotifyTaskCompletion<LexTutorResultFrequencyDetails> _lexTutorFamilyTotal;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExerciseAnalysisDataModel"/> class.
        /// </summary>
        /// <param name="exercise">The <see cref="EssayExercise"/> to analyze.</param>
        public ExerciseAnalysisDataModel(EssayExercise exercise)
        {
            this.Exercise = exercise;
        }

        /// <summary>
        /// Fired when a property is changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the exercise to be analyzed.
        /// </summary>
        public EssayExercise Exercise
        {
            get
            {
                return this._exercise;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                this._exercise = value;

                // Fire the PropertyChanged event for the Exercise property and also for the ones that depend on it
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Exercise"));
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ExerciseTitle"));
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ExerciseSubtitle"));

                // Refresh the analyses now that they have become invalid
                this.RefreshAnalysis();
            }
        }

        /// <summary>
        /// Gets the exercise title.
        /// </summary>
        public string ExerciseTitle
        {
            get
            {
                return this._exercise.Title;
            }
        }

        /// <summary>
        /// Gets the exercise subtitle.
        /// </summary>
        public string ExerciseSubtitle
        {
            get
            {
                string exerciseType = (string)new ExerciseToNameConverter().Convert(this._exercise, typeof(string), null, CultureInfo.CurrentUICulture);
                string language = (string)new SupportedLanguageToNameConverter().Convert(this._exercise.Language, typeof(string), null, CultureInfo.CurrentUICulture);
                string level = (string)new LanguageLevelClassificationToShortDescriptionConverter().Convert(this._exercise.Level, typeof(string), null, CultureInfo.CurrentUICulture);
                return string.Format(CultureInfo.CurrentUICulture, Properties.Resources.ExerciseAnalysis_Subtitle, exerciseType, language, level);
            }
        }

        /// <summary>
        /// Gets the level classification for the exercise text.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Need to return a list of values as a dictionary value")]
        public INotifyTaskCompletion<Dictionary<LanguageLevelClassification, List<IWord>>> LevelClassification
        {
            get
            {
                return this._levelClassification;
            }

            private set
            {
                this._levelClassification = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LevelClassification"));
            }
        }

        /// <summary>
        /// Gets the level classification distribution for the exercise text.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Need to return a list of values as a dictionary value")]
        public INotifyTaskCompletion<Dictionary<LanguageLevelClassification, float>> LevelClassificationDistribution
        {
            get
            {
                return this._levelClassificationDistribution;
            }

            private set
            {
                this._levelClassificationDistribution = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LevelClassificationDistribution"));
            }
        }

        /// <summary>
        /// Gets the number of words in the exercise text.
        /// </summary>
        public INotifyTaskCompletion<int> LexTutorWordsInText
        {
            get
            {
                return this._lexTutorWordsInText;
            }

            private set
            {
                this._lexTutorWordsInText = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LexTutorWordsInText"));
            }
        }

        /// <summary>
        /// Gets the number of different words in the exercise text.
        /// </summary>
        public INotifyTaskCompletion<int> LexTutorDifferentWords
        {
            get
            {
                return this._lexTutorDifferentWords;
            }

            private set
            {
                this._lexTutorDifferentWords = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LexTutorDifferentWords"));
            }
        }

        /// <summary>
        /// Gets the type/token ratio for the exercise text.
        /// </summary>
        public INotifyTaskCompletion<float> LexTutorTypeTokenRatio
        {
            get
            {
                return this._lexTutorTypeTokenRatio;
            }

            private set
            {
                this._lexTutorTypeTokenRatio = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LexTutorTypeTokenRatio"));
            }
        }

        /// <summary>
        /// Gets the number of tokens per type for the exercise text.
        /// </summary>
        public INotifyTaskCompletion<float> LexTutorTokensPerType
        {
            get
            {
                return this._lexTutorTokensPerType;
            }

            private set
            {
                this._lexTutorTokensPerType = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LexTutorTokensPerType"));
            }
        }

        /// <summary>
        /// Gets the number of tokens that appear in the exercise text.
        /// </summary>
        public INotifyTaskCompletion<int> LexTutorTokens
        {
            get
            {
                return this._lexTutorTokens;
            }

            private set
            {
                this._lexTutorTokens = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LexTutorTokens"));
            }
        }

        /// <summary>
        /// Gets the number of different word types appearing in the exercise text.
        /// </summary>
        public INotifyTaskCompletion<int> LexTutorTypes
        {
            get
            {
                return this._lexTutorTypes;
            }

            private set
            {
                this._lexTutorTypes = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LexTutorTypes"));
            }
        }

        /// <summary>
        /// Gets the number of families appearing in the exercise text.
        /// </summary>
        public INotifyTaskCompletion<int> LexTutorFamilies
        {
            get
            {
                return this._lexTutorFamilies;
            }

            private set
            {
                this._lexTutorFamilies = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LexTutorFamilies"));
            }
        }

        /// <summary>
        /// Gets the number of tokens per family for the exercise text.
        /// </summary>
        public INotifyTaskCompletion<float> LexTutorTokensPerFamily
        {
            get
            {
                return this._lexTutorTokensPerFamily;
            }

            private set
            {
                this._lexTutorTokensPerFamily = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LexTutorTokensPerFamily"));
            }
        }

        /// <summary>
        /// Gets the number of types per family for the exercise text.
        /// </summary>
        public INotifyTaskCompletion<float> LexTutorTypesPerFamily
        {
            get
            {
                return this._lexTutorTypesPerFamily;
            }

            private set
            {
                this._lexTutorTypesPerFamily = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LexTutorTypesPerFamily"));
            }
        }

        /// <summary>
        /// Gets a read-only list of words grouped by part of speech in the analyzed text.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Need to return a grouping as it is needed")]
        public INotifyTaskCompletion<ReadOnlyDictionary<PartOfSpeech, ICollection<IWord>>> WordsInTextByPartOfSpeech
        {
            get
            {
                return this._wordsInTextByPartOfSpeech;
            }

            private set
            {
                this._wordsInTextByPartOfSpeech = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("WordsInTextByPartOfSpeech"));
            }
        }

        /// <summary>
        /// Gets a read-only list of words grouped by language level in the analyzed text.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Need to return a grouping as it is needed by the ListView")]
        public INotifyTaskCompletion<ReadOnlyObservableCollection<Grouping<IWord>>> WordsInTextByLevel
        {
            get
            {
                return this._wordsInTextByLevel;
            }

            private set
            {
                this._wordsInTextByLevel = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("WordsInTextByLevel"));
            }
        }

        /// <summary>
        /// Gets the family class.
        /// </summary>
        public INotifyTaskCompletion<LexTutorResultFrequencyDetails> LexTutorFamilyK1
        {
            get
            {
                return this._lexTutorFamilyK1;
            }

            private set
            {
                this._lexTutorFamilyK1 = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LexTutorFamilyK1"));
            }
        }

        /// <summary>
        /// Gets the family class.
        /// </summary>
        public INotifyTaskCompletion<LexTutorResultFrequencyDetails> LexTutorFamilyK2
        {
            get
            {
                return this._lexTutorFamilyK2;
            }

            private set
            {
                this._lexTutorFamilyK2 = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LexTutorFamilyK2"));
            }
        }

        /// <summary>
        /// Gets the family class.
        /// </summary>
        public INotifyTaskCompletion<LexTutorResultFrequencyDetails> LexTutorFamilyK3
        {
            get
            {
                return this._lexTutorFamilyK3;
            }

            private set
            {
                this._lexTutorFamilyK3 = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LexTutorFamilyK3"));
            }
        }

        /// <summary>
        /// Gets the family class.
        /// </summary>
        public INotifyTaskCompletion<LexTutorResultFrequencyDetails> LexTutorFamilyK4
        {
            get
            {
                return this._lexTutorFamilyK4;
            }

            private set
            {
                this._lexTutorFamilyK4 = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LexTutorFamilyK4"));
            }
        }

        /// <summary>
        /// Gets the family class.
        /// </summary>
        public INotifyTaskCompletion<LexTutorResultFrequencyDetails> LexTutorFamilyK5
        {
            get
            {
                return this._lexTutorFamilyK5;
            }

            private set
            {
                this._lexTutorFamilyK5 = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LexTutorFamilyK5"));
            }
        }

        /// <summary>
        /// Gets the family class.
        /// </summary>
        public INotifyTaskCompletion<LexTutorResultFrequencyDetails> LexTutorFamilyK6
        {
            get
            {
                return this._lexTutorFamilyK6;
            }

            private set
            {
                this._lexTutorFamilyK6 = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LexTutorFamilyK6"));
            }
        }

        /// <summary>
        /// Gets the family class.
        /// </summary>
        public INotifyTaskCompletion<LexTutorResultFrequencyDetails> LexTutorFamilyK7
        {
            get
            {
                return this._lexTutorFamilyK7;
            }

            private set
            {
                this._lexTutorFamilyK7 = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LexTutorFamilyK7"));
            }
        }

        /// <summary>
        /// Gets the family class.
        /// </summary>
        public INotifyTaskCompletion<LexTutorResultFrequencyDetails> LexTutorFamilyK8
        {
            get
            {
                return this._lexTutorFamilyK8;
            }

            private set
            {
                this._lexTutorFamilyK8 = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LexTutorFamilyK8"));
            }
        }

        /// <summary>
        /// Gets the family class.
        /// </summary>
        public INotifyTaskCompletion<LexTutorResultFrequencyDetails> LexTutorFamilyK9
        {
            get
            {
                return this._lexTutorFamilyK9;
            }

            private set
            {
                this._lexTutorFamilyK9 = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LexTutorFamilyK9"));
            }
        }

        /// <summary>
        /// Gets the family class.
        /// </summary>
        public INotifyTaskCompletion<LexTutorResultFrequencyDetails> LexTutorFamilyOfflist
        {
            get
            {
                return this._lexTutorFamilyOfflist;
            }

            private set
            {
                this._lexTutorFamilyOfflist = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LexTutorFamilyOfflist"));
            }
        }

        /// <summary>
        /// Gets the family class.
        /// </summary>
        public INotifyTaskCompletion<LexTutorResultFrequencyDetails> LexTutorFamilyTotal
        {
            get
            {
                return this._lexTutorFamilyTotal;
            }

            private set
            {
                this._lexTutorFamilyTotal = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LexTutorFamilyTotal"));
            }
        }

        /// <summary>
        /// Refreshes the exercise analysis.
        /// </summary>
        public void RefreshAnalysis()
        {
            this.LevelClassification = NotifyTaskCompletion.Create(this.GetLevelClassificationAsync());
            this.LevelClassificationDistribution = NotifyTaskCompletion.Create(this.GetLevelClassificationDistributionAsync());
            this.LexTutorWordsInText = NotifyTaskCompletion.Create(this.GetLexTutorWordsInTextAsync());
            this.LexTutorDifferentWords = NotifyTaskCompletion.Create(this.GetLexTutorDifferentWordsAsync());
            this.LexTutorTypeTokenRatio = NotifyTaskCompletion.Create(this.GetLexTutorTypeTokenRatioAsync());
            this.LexTutorTokensPerType = NotifyTaskCompletion.Create(this.GetLexTutorTokensPerTypeAsync());
            this.LexTutorTokens = NotifyTaskCompletion.Create(this.GetLexTutorTokensAsync());
            this.LexTutorTypes = NotifyTaskCompletion.Create(this.GetLexTutorTypesAsync());
            this.LexTutorFamilies = NotifyTaskCompletion.Create(this.GetLexTutorFamiliesAsync());
            this.LexTutorTokensPerFamily = NotifyTaskCompletion.Create(this.GetLexTutorTokensPerFamilyAsync());
            this.LexTutorTypesPerFamily = NotifyTaskCompletion.Create(this.GetLexTutorTypesPerFamilyAsync());
            this.WordsInTextByPartOfSpeech = NotifyTaskCompletion.Create(this.GetWordsInTextByPartOfSpeechAsync());
            this.WordsInTextByLevel = NotifyTaskCompletion.Create(this.GetWordsInTextByLevelAsync());
            this.LexTutorFamilyK1 = NotifyTaskCompletion.Create(this.GetLexTutorFamilyK1());
            this.LexTutorFamilyK2 = NotifyTaskCompletion.Create(this.GetLexTutorFamilyK2());
            this.LexTutorFamilyK3 = NotifyTaskCompletion.Create(this.GetLexTutorFamilyK3());
            this.LexTutorFamilyK4 = NotifyTaskCompletion.Create(this.GetLexTutorFamilyK4());
            this.LexTutorFamilyK5 = NotifyTaskCompletion.Create(this.GetLexTutorFamilyK5());
            this.LexTutorFamilyK6 = NotifyTaskCompletion.Create(this.GetLexTutorFamilyK6());
            this.LexTutorFamilyK7 = NotifyTaskCompletion.Create(this.GetLexTutorFamilyK7());
            this.LexTutorFamilyK8 = NotifyTaskCompletion.Create(this.GetLexTutorFamilyK8());
            this.LexTutorFamilyK9 = NotifyTaskCompletion.Create(this.GetLexTutorFamilyK9());
            this.LexTutorFamilyOfflist = NotifyTaskCompletion.Create(this.GetLexTutorFamilyOfflist());
            this.LexTutorFamilyTotal = NotifyTaskCompletion.Create(this.GetLexTutorFamilyTotal());
        }

        /// <summary>
        /// Gets the level classification asynchronously.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task<Dictionary<LanguageLevelClassification, List<IWord>>> GetLevelClassificationAsync()
        {
            return await this._exercise.LevelClassification;
        }

        /// <summary>
        /// Gets the level classification distribution for a given level asynchronously.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task<Dictionary<LanguageLevelClassification, float>> GetLevelClassificationDistributionAsync()
        {
            return await this._exercise.LevelClassificationDistribution;
        }

        /// <summary>
        /// Gets the number of words appearing in the text asynchronously.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task<int> GetLexTutorWordsInTextAsync()
        {
            return (await this._exercise.LexTutorResult).Ratios.WordsInText;
        }

        /// <summary>
        /// Gets the number of different words appearing in the text asynchronously.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task<int> GetLexTutorDifferentWordsAsync()
        {
            return (await this._exercise.LexTutorResult).Ratios.DifferentWords;
        }

        /// <summary>
        /// Gets the type/token ratio of the text asynchronously.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task<float> GetLexTutorTypeTokenRatioAsync()
        {
            return (await this._exercise.LexTutorResult).Ratios.TypeTokenRatio;
        }

        /// <summary>
        /// Gets the number of tokens per type of the text asynchronously.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task<float> GetLexTutorTokensPerTypeAsync()
        {
            return (await this._exercise.LexTutorResult).Ratios.TokensPerType;
        }

        /// <summary>
        /// Gets the number of tokens appearing in the text asynchronously.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task<int> GetLexTutorTokensAsync()
        {
            return (await this._exercise.LexTutorResult).Ratios.TokensOnList;
        }

        /// <summary>
        /// Gets the number of types in the text asynchronously.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task<int> GetLexTutorTypesAsync()
        {
            return (await this._exercise.LexTutorResult).Ratios.TypesOnList;
        }

        /// <summary>
        /// Gets the number of families in the text asynchronously.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task<int> GetLexTutorFamiliesAsync()
        {
            return (await this._exercise.LexTutorResult).Ratios.FamiliesOnList;
        }

        /// <summary>
        /// Gets the number of types appearing per family asynchronously.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task<float> GetLexTutorTokensPerFamilyAsync()
        {
            return (await this._exercise.LexTutorResult).Ratios.TokensPerFamily;
        }

        /// <summary>
        /// Gets the number of types appearing per family asynchronously.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task<float> GetLexTutorTypesPerFamilyAsync()
        {
            return (await this._exercise.LexTutorResult).Ratios.TypesPerFamily;
        }

        /// <summary>
        /// Gets the words appearing in the exercise text, grouped by part of speech, asynchronously.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task<ReadOnlyDictionary<PartOfSpeech, ICollection<IWord>>> GetWordsInTextByPartOfSpeechAsync()
        {
            Dictionary<PartOfSpeech, ICollection<IWord>> wordGroups = new Dictionary<PartOfSpeech, ICollection<IWord>>();

            // Groups must be added one at a time as, if they do not contain any elements, they show up as empty
            // groups in the list.
            // TODO: find a more elegant way to perform grouping (using a dictionary, maybe)?
            ICollection<IWord> adjectives = (await this._exercise.Adjectives).Keys;
            if (adjectives.Count > 0)
            {
                wordGroups.Add(PartOfSpeech.Adjective, adjectives);
            }

            ICollection<IWord> adverbs = (await this._exercise.Adverbs).Keys;
            if (adverbs.Count > 0)
            {
                wordGroups.Add(PartOfSpeech.Adverb, adverbs);
            }

            ICollection<IWord> cardinalNumbers = (await this._exercise.CardinalNumbers).Keys;
            if (cardinalNumbers.Count > 0)
            {
                wordGroups.Add(PartOfSpeech.CardinalNumber, cardinalNumbers);
            }

            ICollection<IWord> clauseOpeners = (await this._exercise.ClauseOpeners).Keys;
            if (clauseOpeners.Count > 0)
            {
                wordGroups.Add(PartOfSpeech.ClauseOpener, clauseOpeners);
            }

            ICollection<IWord> commonNouns = (await this._exercise.CommonNouns).Keys;
            if (commonNouns.Count > 0)
            {
                wordGroups.Add(PartOfSpeech.CommonNoun, commonNouns);
            }

            ICollection<IWord> conjunctions = (await this._exercise.Conjunctions).Keys;
            if (conjunctions.Count > 0)
            {
                wordGroups.Add(PartOfSpeech.Conjunction, conjunctions);
            }

            ICollection<IWord> determinerPronouns = (await this._exercise.DeterminerPronouns).Keys;
            if (determinerPronouns.Count > 0)
            {
                wordGroups.Add(PartOfSpeech.DeterminerPronoun, determinerPronouns);
            }

            ICollection<IWord> determiners = (await this._exercise.Determiners).Keys;
            if (determiners.Count > 0)
            {
                wordGroups.Add(PartOfSpeech.Determiner, determiners);
            }

            ICollection<IWord> existentialParticles = (await this._exercise.ExistentialParticles).Keys;
            if (existentialParticles.Count > 0)
            {
                wordGroups.Add(PartOfSpeech.ExistentialParticle, existentialParticles);
            }

            ICollection<IWord> foreignWords = (await this._exercise.ForeignWords).Keys;
            if (foreignWords.Count > 0)
            {
                wordGroups.Add(PartOfSpeech.ForeignWord, foreignWords);
            }

            ICollection<IWord> genitives = (await this._exercise.Genitives).Keys;
            if (genitives.Count > 0)
            {
                wordGroups.Add(PartOfSpeech.Genitive, genitives);
            }

            ICollection<IWord> infinitiveMarkers = (await this._exercise.InfinitiveMarkers).Keys;
            if (infinitiveMarkers.Count > 0)
            {
                wordGroups.Add(PartOfSpeech.InfinitiveMarker, infinitiveMarkers);
            }

            ICollection<IWord> interjections = (await this._exercise.Interjections).Keys;
            if (interjections.Count > 0)
            {
                wordGroups.Add(PartOfSpeech.InterjectionOrDiscourseMarker, interjections);
            }

            ICollection<IWord> lettersAsWords = (await this._exercise.LettersOfAlphabet).Keys;
            if (lettersAsWords.Count > 0)
            {
                wordGroups.Add(PartOfSpeech.LetterAsWord, lettersAsWords);
            }

            ICollection<IWord> modalVerbs = (await this._exercise.ModalVerbs).Keys;
            if (modalVerbs.Count > 0)
            {
                wordGroups.Add(PartOfSpeech.ModalVerb, modalVerbs);
            }

            ICollection<IWord> negativeMarkers = (await this._exercise.NegativeMarkers).Keys;
            if (negativeMarkers.Count > 0)
            {
                wordGroups.Add(PartOfSpeech.NegativeMarker, negativeMarkers);
            }

            ICollection<IWord> ordinals = (await this._exercise.OrdinalNumbers).Keys;
            if (ordinals.Count > 0)
            {
                wordGroups.Add(PartOfSpeech.Ordinal, ordinals);
            }

            ICollection<IWord> partsOfProperNouns = (await this._exercise.PartsOfProperNouns).Keys;
            if (partsOfProperNouns.Count > 0)
            {
                wordGroups.Add(PartOfSpeech.PartOfProperNoun, partsOfProperNouns);
            }

            ICollection<IWord> prepositions = (await this._exercise.Prepositions).Keys;
            if (prepositions.Count > 0)
            {
                wordGroups.Add(PartOfSpeech.Preposition, prepositions);
            }

            ICollection<IWord> pronouns = (await this._exercise.Pronouns).Keys;
            if (pronouns.Count > 0)
            {
                wordGroups.Add(PartOfSpeech.Pronoun, pronouns);
            }

            ICollection<IWord> properNouns = (await this._exercise.ProperNouns).Keys;
            if (properNouns.Count > 0)
            {
                wordGroups.Add(PartOfSpeech.ProperNoun, properNouns);
            }

            ICollection<IWord> unclassifiedWords = (await this._exercise.UnclassifiedWords).Keys;
            if (unclassifiedWords.Count > 0)
            {
                wordGroups.Add(PartOfSpeech.Unclassified, unclassifiedWords);
            }

            ICollection<IWord> verbs = (await this._exercise.Verbs).Keys;
            if (verbs.Count > 0)
            {
                wordGroups.Add(PartOfSpeech.Verb, verbs);
            }

            return new ReadOnlyDictionary<PartOfSpeech, ICollection<IWord>>(wordGroups);
        }

        /// <summary>
        /// Gets the words appearing in the exercise text, grouped by level, asynchronously.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task<ReadOnlyObservableCollection<Grouping<IWord>>> GetWordsInTextByLevelAsync()
        {
            Collection<Grouping<IWord>> wordGroups = new Collection<Grouping<IWord>>();
            LanguageLevelClassificationToShortDescriptionConverter shortLanguageConverter = new LanguageLevelClassificationToShortDescriptionConverter();
            LanguageLevelClassificationToLongDescriptionConverter longLanguageConverter = new LanguageLevelClassificationToLongDescriptionConverter();
            Dictionary<LanguageLevelClassification, List<IWord>> exerciseResults = await this._exercise.LevelClassification;

            foreach (KeyValuePair<LanguageLevelClassification, List<IWord>> exercisePair in exerciseResults)
            {
                if (exercisePair.Value.Count > 0)
                {
                    wordGroups.Add(new Grouping<IWord>(
                        (string)longLanguageConverter.Convert(exercisePair.Key, typeof(string), null, CultureInfo.CurrentUICulture),
                        (string)shortLanguageConverter.Convert(exercisePair.Key, typeof(string), null, CultureInfo.CurrentUICulture),
                        exercisePair.Value));
                }
            }

            return new ReadOnlyObservableCollection<Grouping<IWord>>(new ObservableCollection<Grouping<IWord>>(wordGroups));
        }

        /// <summary>
        /// Gets the family.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task<LexTutorResultFrequencyDetails> GetLexTutorFamilyK1()
        {
            return (await this._exercise.LexTutorResult).FrequencyLevels.K1Words;
        }

        /// <summary>
        /// Gets the family.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task<LexTutorResultFrequencyDetails> GetLexTutorFamilyK2()
        {
            return (await this._exercise.LexTutorResult).FrequencyLevels.K2Words;
        }

        /// <summary>
        /// Gets the family.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task<LexTutorResultFrequencyDetails> GetLexTutorFamilyK3()
        {
            return (await this._exercise.LexTutorResult).FrequencyLevels.K3Words;
        }

        /// <summary>
        /// Gets the family.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task<LexTutorResultFrequencyDetails> GetLexTutorFamilyK4()
        {
            return (await this._exercise.LexTutorResult).FrequencyLevels.K4Words;
        }

        /// <summary>
        /// Gets the family.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task<LexTutorResultFrequencyDetails> GetLexTutorFamilyK5()
        {
            return (await this._exercise.LexTutorResult).FrequencyLevels.K5Words;
        }

        /// <summary>
        /// Gets the family.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task<LexTutorResultFrequencyDetails> GetLexTutorFamilyK6()
        {
            return (await this._exercise.LexTutorResult).FrequencyLevels.K6Words;
        }

        /// <summary>
        /// Gets the family.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task<LexTutorResultFrequencyDetails> GetLexTutorFamilyK7()
        {
            return (await this._exercise.LexTutorResult).FrequencyLevels.K7Words;
        }

        /// <summary>
        /// Gets the family.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task<LexTutorResultFrequencyDetails> GetLexTutorFamilyK8()
        {
            return (await this._exercise.LexTutorResult).FrequencyLevels.K8Words;
        }

        /// <summary>
        /// Gets the family.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task<LexTutorResultFrequencyDetails> GetLexTutorFamilyK9()
        {
            return (await this._exercise.LexTutorResult).FrequencyLevels.K9Words;
        }

        /// <summary>
        /// Gets the family.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task<LexTutorResultFrequencyDetails> GetLexTutorFamilyOfflist()
        {
            return (await this._exercise.LexTutorResult).FrequencyLevels.OffList;
        }

        /// <summary>
        /// Gets the family.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task<LexTutorResultFrequencyDetails> GetLexTutorFamilyTotal()
        {
            return (await this._exercise.LexTutorResult).FrequencyLevels.Total;
        }
    }
}
