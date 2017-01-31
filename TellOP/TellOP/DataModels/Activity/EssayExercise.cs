// <copyright file="EssayExercise.cs" company="University of Murcia">
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

namespace TellOP.DataModels.Activity
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Api;
    using ApiModels;
    using ApiModels.Adelex;
    using ApiModels.LexTutor;
    using DataModels.Enums;
    using Nito.AsyncEx;
    using SQLiteModels;

    /// <summary>
    /// An essay exercise.
    /// </summary>
    public class EssayExercise : Exercise
    {
        /// <summary>
        /// The contents (development) of the exercise.
        /// </summary>
        private string _essayContents;

        /// <summary>
        /// The description of the essay.
        /// </summary>
        private string _essayDescription;

        /// <summary>
        /// The maximum number of words the essay must have.
        /// </summary>
        private int _essayMaximumWords;

        /// <summary>
        /// The minimum number of words the essay must have.
        /// </summary>
        private int _essayMinimumWords;

        /// <summary>
        /// A list of tags associated to this essay.
        /// </summary>
        private IList<string> _essayTags;

        /// <summary>
        /// The title of the essay.
        /// </summary>
        private string _essayTitle;

        /// <summary>
        /// Initializes a new instance of the <see cref="EssayExercise"/> class.
        /// </summary>
        public EssayExercise()
        {
            this._essayContents = string.Empty;
            this._essayDescription = string.Empty;
            this._essayMaximumWords = 250;
            this._essayMinimumWords = 80;
            this._essayTags = new List<string>();
            this._essayTitle = string.Empty;
            this.ExcludeFunctionalWords = true;
            this.Status = ExerciseStatus.NotCompleted;
            this.InitializeOfflineAnalysisProperties();
            this.InitializeOnlineAnalysisProperties();
        }

        /// <summary>
        /// Gets or sets the contents (user development) of this exercise.
        /// </summary>
        /// <remarks>If the contents are changed, the analysis result cache is not preserved.</remarks>
        public string Contents
        {
            get
            {
                return this._essayContents;
            }

            set
            {
                if (value == null)
                {
                    value = string.Empty;
                }

                // Only update the value and the dirty flags if the new contents differ from the old ones.
                if (!value.Equals(this._essayContents))
                {
                    this._essayContents = value;
                    this.InitializeOfflineAnalysisProperties();
                    this.InitializeOnlineAnalysisProperties();
                }
            }
        }

        /// <summary>
        /// Gets or sets the description of this essay (instructions the user must follow).
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if the description is <c>null</c>.</exception>
        public string Description
        {
            get
            {
                return this._essayDescription;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                this._essayDescription = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum number of words this essay should have.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the maximum number of words is less than
        /// <c>1</c>.</exception>
        public int MaximumWords
        {
            get
            {
                return this._essayMaximumWords;
            }

            set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException("value");
                }

                this._essayMaximumWords = value;
            }
        }

        /// <summary>
        /// Gets or sets the minimum number of words this essay should have.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the minimum number of words is less than
        /// <c>1</c>.</exception>
        public int MinimumWords
        {
            get
            {
                return this._essayMinimumWords;
            }

            set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException("value");
                }

                this._essayMinimumWords = value;
            }
        }

        /// <summary>
        /// Gets or sets the text the user should read before writing the essay.
        /// </summary>
        /// <remarks>The property is <c>null</c> in case there is no preliminary text for this essay.</remarks>
        public string PreliminaryText { get; set; }

        /// <summary>
        /// Gets or sets the exercise status.
        /// </summary>
        public ExerciseStatus Status { get; set; }

        /// <summary>
        /// Gets a list of tags associated with this essay.
        /// </summary>
        public IList<string> Tags
        {
            get
            {
                return this._essayTags;
            }
        }

        /// <summary>
        /// Gets or sets the date and time this exercise was submitted.
        /// </summary>
        /// <remarks>If the exercise was not submitted (yet), this property is <c>null</c>.</remarks>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the title of this essay.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if the essay title is <c>null</c>.</exception>
        public string Title
        {
            get
            {
                return this._essayTitle;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                this._essayTitle = value;
            }
        }

        /// <summary>
        /// Gets the adjectives in the exercise text.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Need to return a list of values as a key")]
        public AsyncLazy<IDictionary<IWord, int>> Adjectives { get; private set; }

        /// <summary>
        /// Gets the adverbs in the exercise text.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Need to return a list of values as a key")]
        public AsyncLazy<IDictionary<IWord, int>> Adverbs { get; private set; }

        /// <summary>
        /// Gets the clause openers in the exercise text.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Need to return a list of values as a key")]
        public AsyncLazy<IDictionary<IWord, int>> ClauseOpeners { get; private set; }

        /// <summary>
        /// Gets the conjunctions in the exercise text.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Need to return a list of values as a key")]
        public AsyncLazy<IDictionary<IWord, int>> Conjunctions { get; private set; }

        /// <summary>
        /// Gets the determiners in the exercise text.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Need to return a list of values as a key")]
        public AsyncLazy<IDictionary<IWord, int>> Determiners { get; private set; }

        /// <summary>
        /// Gets the determiners used as pronouns in the exercise text.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Need to return a list of values as a key")]
        public AsyncLazy<IDictionary<IWord, int>> DeterminerPronouns { get; private set; }

        /// <summary>
        /// Gets the existential particles in the exercise text.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Need to return a list of values as a key")]
        public AsyncLazy<IDictionary<IWord, int>> ExistentialParticles { get; private set; }

        /// <summary>
        /// Gets the foreign words in the exercise text.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Need to return a list of values as a key")]
        public AsyncLazy<IDictionary<IWord, int>> ForeignWords { get; private set; }

        /// <summary>
        /// Gets the genitives in the exercise text.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Need to return a list of values as a key")]
        public AsyncLazy<IDictionary<IWord, int>> Genitives { get; private set; }

        /// <summary>
        /// Gets the infinitive markers in the exercise text.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Need to return a list of values as a key")]
        public AsyncLazy<IDictionary<IWord, int>> InfinitiveMarkers { get; private set; }

        /// <summary>
        /// Gets the interjection or discourse markers in the exercise text.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Need to return a list of values as a key")]
        public AsyncLazy<IDictionary<IWord, int>> Interjections { get; private set; }

        /// <summary>
        /// Gets the letters of the alphabet treated as words in the exercise text.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Need to return a list of values as a key")]
        public AsyncLazy<IDictionary<IWord, int>> LettersOfAlphabet { get; private set; }

        /// <summary>
        /// Gets the negative markers in the exercise text.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Need to return a list of values as a key")]
        public AsyncLazy<IDictionary<IWord, int>> NegativeMarkers { get; private set; }

        /// <summary>
        /// Gets the common nouns in the exercise text.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Need to return a list of values as a key")]
        public AsyncLazy<IDictionary<IWord, int>> CommonNouns { get; private set; }

        /// <summary>
        /// Gets the proper nouns in the exercise text.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Need to return a list of values as a key")]
        public AsyncLazy<IDictionary<IWord, int>> ProperNouns { get; private set; }

        /// <summary>
        /// Gets the parts of a proper noun in the exercise text.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Need to return a list of values as a key")]
        public AsyncLazy<IDictionary<IWord, int>> PartsOfProperNouns { get; private set; }

        /// <summary>
        /// Gets the cardinal numbers in the exercise text.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Need to return a list of values as a key")]
        public AsyncLazy<IDictionary<IWord, int>> CardinalNumbers { get; private set; }

        /// <summary>
        /// Gets the ordinal numbers in the exercise text.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Need to return a list of values as a key")]
        public AsyncLazy<IDictionary<IWord, int>> OrdinalNumbers { get; private set; }

        /// <summary>
        /// Gets the prepositions in the exercise text.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Need to return a list of values as a key")]
        public AsyncLazy<IDictionary<IWord, int>> Prepositions { get; private set; }

        /// <summary>
        /// Gets the pronouns in the exercise text.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Need to return a list of values as a key")]
        public AsyncLazy<IDictionary<IWord, int>> Pronouns { get; private set; }

        /// <summary>
        /// Gets the unclassified words in the exercise text.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Need to return a list of values as a key")]
        public AsyncLazy<IDictionary<IWord, int>> UnclassifiedWords { get; private set; }

        /// <summary>
        /// Gets the verbs in the exercise text.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Need to return a list of values as a key")]
        public AsyncLazy<IDictionary<IWord, int>> Verbs { get; private set; }

        /// <summary>
        /// Gets the modal verbs in the exercise text.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Need to return a list of values as a key")]
        public AsyncLazy<IDictionary<IWord, int>> ModalVerbs { get; private set; }

        /// <summary>
        /// Gets the result of the Adelex analysis.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Need to return a list of values as a key")]
        public AsyncLazy<AdelexResultEntry> AdelexResult { get; private set; }

        /// <summary>
        /// Gets the result of the LexTutor analysis.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Need to return a list of values as a key")]
        public AsyncLazy<LexTutorResultEntry> LexTutorResult { get; private set; }

        /// <summary>
        /// Gets a list of words in the essay content grouped by their language
        /// level.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Need to return a list of values as a key")]
        public AsyncLazy<Dictionary<LanguageLevelClassification, List<IWord>>> LevelClassification { get; private set; }

        /// <summary>
        /// Gets the distribution relative to the language distribution of words.
        /// </summary>
        /// <returns>A dictionary having the possible language levels as keys and the relative word frequency as its
        /// value.</returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Need to return a list of values as a key")]
        public AsyncLazy<Dictionary<LanguageLevelClassification, float>> LevelClassificationDistribution { get; private set; }

        /// <summary>
        /// Gets the number of offline analyzed words in the essay.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Need to return a list of values as a key")]
        public AsyncLazy<int> NumWords { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether functional words should be excluded from the offline analysis.
        /// </summary>
        public bool ExcludeFunctionalWords { get; set; }

        /// <summary>
        /// Gets or sets the cache for the offline analysis result.
        /// </summary>
        private AsyncLazy<List<IWord>> OfflineAnalysisResult { get; set; }

        /// <summary>
        /// Returns the cumulative percentage of the passed dictionaries with respect to the total amount of words.
        /// </summary>
        /// <param name="dictionaries">An <see cref="IList{PartOfSpeech}"/> containing the dictionaries this function
        /// should examine.</param>
        /// <returns>A <see cref="Task{Float}"/> containing the cumulative percentage of the dictionaries listed in
        /// <paramref name="dictionaries"/> as its result.</returns>
        public async Task<float> GetNumWordsPercentage(IList<PartOfSpeech> dictionaries)
        {
            List<IWord> offlineAnalysis = await this.OfflineAnalysisResult;
            return offlineAnalysis.Where(w => dictionaries.Contains(w.PartOfSpeech)).Count() / (float)offlineAnalysis.Count;
        }

        /// <summary>
        /// Gets a list of words according to their parts of speech.
        /// </summary>
        /// <param name="part">The part of speech the words belong to.</param>
        /// <returns>A <see cref="Task{Dictionary}"/> containing all the words in the essay contents belonging to the
        /// specified part of speech and the number of their occurrencies as its result.</returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Need to return an IWord as a key")]
        public async Task<IDictionary<IWord, int>> GetWordsByPartOfSpeech(PartOfSpeech part)
        {
            List<IWord> offlineAnalysis = await this.OfflineAnalysisResult;
            return offlineAnalysis.Where(w => w.PartOfSpeech == part).GroupBy(w => w).Where(w => w.Count() >= 1).ToDictionary(w => w.Key, w => w.Count());
        }

        private async Task<string> PreprocessSingleWord(string token)
        {
            string cleanToken = await OfflineLemmaEN.RetrieveBase(token);
            if (string.IsNullOrEmpty(cleanToken) || string.IsNullOrWhiteSpace(cleanToken))
            {
                cleanToken = token;
            }

            return cleanToken;
        }

        private async Task<IWord> ProcessSingleWord(string cleanToken)
        {
            string msg = "'" + cleanToken + "' ";
            IWord w;

            if (this.Language == SupportedLanguage.English)
            {
                IList<IWord> offlineWords = await OfflineWord.Search(cleanToken);
                if (offlineWords.Count > 0)
                {
                    w = WordSearchUtilities.GetMostProbable(offlineWords);
                }
                else
                {
                    // TODO: find a better way to add a word that is not found in the database.
                    msg += "SearchCount is zero, so I'm creating a new word with an unclassified part of speech and an unknown language level";
                    w = new OfflineWord()
                    {
                        Term = cleanToken,
                        JsonLevel = LanguageLevelClassification.Unknown,
                        PartOfSpeech = PartOfSpeech.Unclassified,
                        Language = (string)new SupportedLanguageToLcidConverter().Convert(SupportedLanguage.English, typeof(string), null, CultureInfo.InvariantCulture)
                    };
                }

                Tools.Logger.Log("ProcessSingleWord", msg);
                return w;
            }
            else if (this.Language == SupportedLanguage.English)
            {
                Tools.Logger.Log("ProcessSingleWord", "Call remote SpanishTagger");
                SpanishPOSTagger es_tagger = new SpanishPOSTagger(App.OAuth2Account, cleanToken);
                IList<SpanishWord> results = await es_tagger.CallEndpointAsObjectAsync();
                Tools.Logger.Log("ProcessSingleWord", "Got the results!");
                try
                {
                    return results.First();
                }
                catch (Exception)
                {
                    return new OfflineWord()
                    {
                        Term = cleanToken,
                        JsonLevel = LanguageLevelClassification.Unknown,
                        PartOfSpeech = PartOfSpeech.Unclassified,
                        Language = (string)new SupportedLanguageToLcidConverter().Convert(SupportedLanguage.Spanish, typeof(string), null, CultureInfo.InvariantCulture)
                    };
                }
            }

            return new OfflineWord()
            {
                Term = cleanToken,
                JsonLevel = LanguageLevelClassification.Unknown,
                PartOfSpeech = PartOfSpeech.Unclassified,
                Language = (string)new SupportedLanguageToLcidConverter().Convert(SupportedLanguage.English, typeof(string), null, CultureInfo.InvariantCulture)
            };
        }

        /// <summary>
        /// Initializes or reinitializes all properties which can be extracted from an offline analysis of the text.
        /// </summary>
        private void InitializeOfflineAnalysisProperties()
        {
            // TODO: catch errors in the UI!
            this.OfflineAnalysisResult = new AsyncLazy<List<IWord>>(async () =>
            {
                List<IWord> analysisCache = new List<IWord>();

                if (this.Language == SupportedLanguage.English)
                {
                    IList<Task<IWord>> searchTokenTasks = new List<Task<IWord>>();

                    // Dirty tokens
                    foreach (string token in Regex.Matches(this._essayContents, "[\\w']+").Cast<Match>().Select(m => m.Value))
                    {
                        foreach (string cleanToken in Regex.Matches(await this.PreprocessSingleWord(token), "[\\w']+").Cast<Match>().Select(m => m.Value))
                        {
                            searchTokenTasks.Add(this.ProcessSingleWord(cleanToken));
                        }
                    }

                    IEnumerable<IWord> results = await Task.WhenAll(searchTokenTasks);

                    Tools.Logger.Log("EssayExercise", "I've waited all of them!");
                    foreach (IWord w in results)
                    {
                        if (this.ExcludeFunctionalWords && (
                            w.PartOfSpeech == PartOfSpeech.ClauseOpener
                            || w.PartOfSpeech == PartOfSpeech.Conjunction
                            || w.PartOfSpeech == PartOfSpeech.Determiner
                            || w.PartOfSpeech == PartOfSpeech.DeterminerPronoun
                            || w.PartOfSpeech == PartOfSpeech.ExistentialParticle
                            || w.PartOfSpeech == PartOfSpeech.Genitive
                            || w.PartOfSpeech == PartOfSpeech.InfinitiveMarker
                            || w.PartOfSpeech == PartOfSpeech.InterjectionOrDiscourseMarker
                            || w.PartOfSpeech == PartOfSpeech.NegativeMarker
                            || w.PartOfSpeech == PartOfSpeech.CardinalNumber
                            || w.PartOfSpeech == PartOfSpeech.Ordinal
                            || w.PartOfSpeech == PartOfSpeech.Pronoun
                            || w.PartOfSpeech == PartOfSpeech.ModalVerb))
                        {
                            continue;
                        }
                        analysisCache.Add(w);
                    }
                }
                else if (this.Language == SupportedLanguage.Spanish)
                {
                    Tools.Logger.Log("EssayExerciseES", "Call remote SpanishTagger");
                    SpanishPOSTagger es_tagger = new SpanishPOSTagger(App.OAuth2Account, this._essayContents);
                    IList<SpanishWord> results = await es_tagger.CallEndpointAsObjectAsync();
                    Tools.Logger.Log("EssayExerciseES", "Got the results!");
                    analysisCache.AddRange(results);
                }

                return analysisCache;
            });
            this.LevelClassification = new AsyncLazy<Dictionary<LanguageLevelClassification, List<IWord>>>(async () =>
            {
                if (this.Language == SupportedLanguage.English)
                {
                    List<IWord> offlineAnalysis = await this.OfflineAnalysisResult;
                    Dictionary<LanguageLevelClassification, List<IWord>> result = new Dictionary<LanguageLevelClassification, List<IWord>>();

                    foreach (LanguageLevelClassification level in Enum.GetValues(typeof(LanguageLevelClassification)))
                    {
                        result.Add(level, new List<IWord>());
                    }
                    foreach (IWord word in offlineAnalysis)
                    {
                        LanguageLevelClassification level = await word.Level;
                        result[level].Add(word);
                    }
                    return result;
                }
                return new Dictionary<LanguageLevelClassification, List<IWord>>();
            });
            this.LevelClassificationDistribution = new AsyncLazy<Dictionary<LanguageLevelClassification, float>>(async () =>
            {
                if (this.Language == SupportedLanguage.English)
                {
                    // TODO: check for any possible loss of precision
                    List<IWord> offlineAnalysis = await this.OfflineAnalysisResult;
                    Dictionary<LanguageLevelClassification, float> result = new Dictionary<LanguageLevelClassification, float>();

                    foreach (LanguageLevelClassification level in Enum.GetValues(typeof(LanguageLevelClassification)))
                    {
                        result.Add(level, 0);
                    }
                    foreach (IWord word in offlineAnalysis)
                    {
                        LanguageLevelClassification level = await word.Level;
                        result[level] = result[level] + 1;
                    }
                    foreach (LanguageLevelClassification level in Enum.GetValues(typeof(LanguageLevelClassification)))
                    {
                        result[level] = result[level] / (float)offlineAnalysis.Count;
                    }
                    return result;
                }
                return new Dictionary<LanguageLevelClassification, float>();
            });
            this.NumWords = new AsyncLazy<int>(async () =>
            {
                List<IWord> offlineAnalysis = await this.OfflineAnalysisResult;
                return offlineAnalysis.Count;
            });
            this.Adjectives = new AsyncLazy<IDictionary<IWord, int>>(async () =>
            {
                return await this.GetWordsByPartOfSpeech(PartOfSpeech.Adjective);
            });
            this.Adverbs = new AsyncLazy<IDictionary<IWord, int>>(async () =>
            {
                return await this.GetWordsByPartOfSpeech(PartOfSpeech.Adverb);
            });
            this.ClauseOpeners = new AsyncLazy<IDictionary<IWord, int>>(async () =>
            {
                return await this.GetWordsByPartOfSpeech(PartOfSpeech.ClauseOpener);
            });
            this.Conjunctions = new AsyncLazy<IDictionary<IWord, int>>(async () =>
            {
                return await this.GetWordsByPartOfSpeech(PartOfSpeech.Conjunction);
            });
            this.Determiners = new AsyncLazy<IDictionary<IWord, int>>(async () =>
            {
                return await this.GetWordsByPartOfSpeech(PartOfSpeech.Determiner);
            });
            this.DeterminerPronouns = new AsyncLazy<IDictionary<IWord, int>>(async () =>
            {
                return await this.GetWordsByPartOfSpeech(PartOfSpeech.DeterminerPronoun);
            });
            this.ExistentialParticles = new AsyncLazy<IDictionary<IWord, int>>(async () =>
            {
                return await this.GetWordsByPartOfSpeech(PartOfSpeech.ExistentialParticle);
            });
            this.ForeignWords = new AsyncLazy<IDictionary<IWord, int>>(async () =>
            {
                return await this.GetWordsByPartOfSpeech(PartOfSpeech.ForeignWord);
            });
            this.Genitives = new AsyncLazy<IDictionary<IWord, int>>(async () =>
            {
                return await this.GetWordsByPartOfSpeech(PartOfSpeech.Genitive);
            });
            this.InfinitiveMarkers = new AsyncLazy<IDictionary<IWord, int>>(async () =>
            {
                return await this.GetWordsByPartOfSpeech(PartOfSpeech.InfinitiveMarker);
            });
            this.Interjections = new AsyncLazy<IDictionary<IWord, int>>(async () =>
            {
                return await this.GetWordsByPartOfSpeech(PartOfSpeech.InterjectionOrDiscourseMarker);
            });
            this.LettersOfAlphabet = new AsyncLazy<IDictionary<IWord, int>>(async () =>
            {
                return await this.GetWordsByPartOfSpeech(PartOfSpeech.LetterAsWord);
            });
            this.NegativeMarkers = new AsyncLazy<IDictionary<IWord, int>>(async () =>
            {
                return await this.GetWordsByPartOfSpeech(PartOfSpeech.NegativeMarker);
            });
            this.CommonNouns = new AsyncLazy<IDictionary<IWord, int>>(async () =>
            {
                return await this.GetWordsByPartOfSpeech(PartOfSpeech.CommonNoun);
            });
            this.ProperNouns = new AsyncLazy<IDictionary<IWord, int>>(async () =>
            {
                return await this.GetWordsByPartOfSpeech(PartOfSpeech.ProperNoun);
            });
            this.PartsOfProperNouns = new AsyncLazy<IDictionary<IWord, int>>(async () =>
            {
                return await this.GetWordsByPartOfSpeech(PartOfSpeech.PartOfProperNoun);
            });
            this.CardinalNumbers = new AsyncLazy<IDictionary<IWord, int>>(async () =>
            {
                return await this.GetWordsByPartOfSpeech(PartOfSpeech.CardinalNumber);
            });
            this.OrdinalNumbers = new AsyncLazy<IDictionary<IWord, int>>(async () =>
            {
                return await this.GetWordsByPartOfSpeech(PartOfSpeech.Ordinal);
            });
            this.Prepositions = new AsyncLazy<IDictionary<IWord, int>>(async () =>
            {
                return await this.GetWordsByPartOfSpeech(PartOfSpeech.Preposition);
            });
            this.Pronouns = new AsyncLazy<IDictionary<IWord, int>>(async () =>
            {
                return await this.GetWordsByPartOfSpeech(PartOfSpeech.Pronoun);
            });
            this.UnclassifiedWords = new AsyncLazy<IDictionary<IWord, int>>(async () =>
            {
                return await this.GetWordsByPartOfSpeech(PartOfSpeech.Unclassified);
            });
            this.Verbs = new AsyncLazy<IDictionary<IWord, int>>(async () =>
            {
                return await this.GetWordsByPartOfSpeech(PartOfSpeech.Verb);
            });
            this.ModalVerbs = new AsyncLazy<IDictionary<IWord, int>>(async () =>
            {
                return await this.GetWordsByPartOfSpeech(PartOfSpeech.ModalVerb);
            });
        }

        /// <summary>
        /// Initializes or reinitializes all properties which can be extracted from an online analysis of the text.
        /// </summary>
        private void InitializeOnlineAnalysisProperties()
        {
            // TODO: catch errors in the UI!
            this.AdelexResult = new AsyncLazy<AdelexResultEntry>(async () =>
            {
                AdelexApi adelex = new AdelexApi(App.OAuth2Account, this._essayContents, AdelexOrder.Frequency);
                return await adelex.CallEndpointAsObjectAsync();
            });
            this.LexTutorResult = new AsyncLazy<LexTutorResultEntry>(async () =>
            {
                LexTutorApi lexTutor = new LexTutorApi(App.OAuth2Account, this._essayTitle, this._essayContents);
                return await lexTutor.CallEndpointAsObjectAsync();
            });
        }
    }
}
