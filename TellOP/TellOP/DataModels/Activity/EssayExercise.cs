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
    using System.Linq;
    using System.Text.RegularExpressions;
    using API;
    using APIModels.Adelex;
    using APIModels.LexTutor;
    using Enums;
    using SQLiteModels;
    using Xamarin.Forms;
    using System.Threading.Tasks;

    /// <summary>
    /// An essay exercise.
    /// </summary>
    public class EssayExercise : Exercise
    {
        /// <summary>
        /// A cache for the Adelex analysis results.
        /// </summary>
        private AdelexResultEntry _adelexResult;

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
        /// A cache for the LexTutor analysis results.
        /// </summary>
        private LexTutorResultEntry _lexTutorResult;

        /// <summary>
        /// A cache for the offline analysis results.
        /// </summary>
        private List<IWord> _offlineAnalysisCache;

        /// <summary>
        /// Determines whether the text needs to be analyzed offline.
        /// </summary>
        private bool _offlineAnalysisDirtyFlag;

        /// <summary>
        /// Determines whether the text needs to be analyzed online.
        /// </summary>
        private bool _onlineAnalysisDirtyFlag;

        /// <summary>
        /// Initializes a new instance of the <see cref="EssayExercise"/> class.
        /// </summary>
        public EssayExercise()
        {
            this._adelexResult = null;
            this._essayContents = string.Empty;
            this._essayDescription = string.Empty;
            this._essayMaximumWords = 250;
            this._essayMinimumWords = 80;
            this._essayTags = new List<string>();
            this._essayTitle = string.Empty;
            this._lexTutorResult = null;
            this._offlineAnalysisCache = new List<IWord>();
            this._offlineAnalysisDirtyFlag = false;
            this._onlineAnalysisDirtyFlag = false;
            this.Status = ExerciseStatus.NotCompleted;
        }

        /// <summary>
        /// Gets or sets the contents (user development) of this exercise.
        /// </summary>
        /// <remarks>If the contents are changed, the analysis result cache is
        /// preserved (even though it refers to the old contents).</remarks>
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

                // Only update the value and the dirty flags if the new
                // contents differ from the old ones.
                if (!value.Equals(this._essayContents))
                {
                    this._essayContents = value;
                    this._offlineAnalysisDirtyFlag = true;
                    this._onlineAnalysisDirtyFlag = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets the description of this essay (instructions the user
        /// must follow).
        /// </summary>
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
                    throw new ArgumentNullException(
                        "value",
                        "The essay description can not be null");
                }

                this._essayDescription = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether an offline analysis of the text is
        /// needed.
        /// </summary>
        public bool IsOfflineAnalysisNeeded
        {
            get
            {
                return this._offlineAnalysisDirtyFlag;
            }
        }

        /// <summary>
        /// Gets a value indicating whether an online analysis of the text is
        /// needed.
        /// </summary>
        public bool IsOnlineAnalysisNeeded
        {
            get
            {
                return this._onlineAnalysisDirtyFlag;
            }
        }

        /// <summary>
        /// Gets or sets the maximum number of words this essay should have.
        /// </summary>
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
                    throw new ArgumentOutOfRangeException(
                        "value",
                        "The maximum number of words must be at least 1");
                }

                this._essayMaximumWords = value;
            }
        }

        /// <summary>
        /// Gets or sets the minimum number of words this essay should have.
        /// </summary>
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
                    throw new ArgumentOutOfRangeException(
                        "value",
                        "The minimum number of words must be at least 1");
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
                    throw new ArgumentNullException(
                        "value",
                        "The essay title can not be null");
                }

                this._essayTitle = value;
            }
        }

        /// <summary>
        /// Gets the adjectives in the exercise text.
        /// </summary>
        public IDictionary<IWord, int> Adjectives
        {
            get
            {
                return this.GetWordsByPartOfSpeech(PartOfSpeech.Adjective);
            }
        }

        /// <summary>
        /// Gets the adverbs in the exercise text.
        /// </summary>
        public IDictionary<IWord, int> Adverbs
        {
            get
            {
                return this.GetWordsByPartOfSpeech(PartOfSpeech.Adverb);
            }
        }

        /// <summary>
        /// Gets the clause openers in the exercise text.
        /// </summary>
        public IDictionary<IWord, int> ClauseOpeners
        {
            get
            {
                return this.GetWordsByPartOfSpeech(PartOfSpeech.ClauseOpener);
            }
        }

        /// <summary>
        /// Gets the conjunctions in the exercise text.
        /// </summary>
        public IDictionary<IWord, int> Conjunctions
        {
            get
            {
                return this.GetWordsByPartOfSpeech(PartOfSpeech.Conjunction);
            }
        }

        /// <summary>
        /// Gets the determiners in the exercise text.
        /// </summary>
        public IDictionary<IWord, int> Determiners
        {
            get
            {
                return this.GetWordsByPartOfSpeech(PartOfSpeech.Determiner);
            }
        }

        /// <summary>
        /// Gets the determiners used as pronouns in the exercise text.
        /// </summary>
        public IDictionary<IWord, int> DeterminerPronouns
        {
            get
            {
                return this.GetWordsByPartOfSpeech(PartOfSpeech.DeterminerPronoun);
            }
        }

        /// <summary>
        /// Gets the existential particles in the exercise text.
        /// </summary>
        public IDictionary<IWord, int> ExistentialParticles
        {
            get
            {
                return this.GetWordsByPartOfSpeech(PartOfSpeech.ExistentialParticle);
            }
        }

        /// <summary>
        /// Gets the foreign words in the exercise text.
        /// </summary>
        public IDictionary<IWord, int> ForeignWords
        {
            get
            {
                return this.GetWordsByPartOfSpeech(PartOfSpeech.ForeignWord);
            }
        }

        /// <summary>
        /// Gets the genitives in the exercise text.
        /// </summary>
        public IDictionary<IWord, int> Genitives
        {
            get
            {
                return this.GetWordsByPartOfSpeech(PartOfSpeech.Genitive);
            }
        }

        /// <summary>
        /// Gets the infinitive markers in the exercise text.
        /// </summary>
        public IDictionary<IWord, int> InfinitiveMarkers
        {
            get
            {
                return this.GetWordsByPartOfSpeech(PartOfSpeech.InfinitiveMarker);
            }
        }

        /// <summary>
        /// Gets the interjection or discourse markers in the exercise text.
        /// </summary>
        public IDictionary<IWord, int> Interjections
        {
            get
            {
                return this.GetWordsByPartOfSpeech(PartOfSpeech.InterjectionOrDiscourseMarker);
            }
        }

        /// <summary>
        /// Gets the letters of the alphabet treated as words in the exercise
        /// text.
        /// </summary>
        public IDictionary<IWord, int> LettersOfAlphabet
        {
            get
            {
                return this.GetWordsByPartOfSpeech(PartOfSpeech.LetterAsWord);
            }
        }

        /// <summary>
        /// Gets the negative markers in the exercise text.
        /// </summary>
        public IDictionary<IWord, int> NegativeMarkers
        {
            get
            {
                return this.GetWordsByPartOfSpeech(PartOfSpeech.NegativeMarker);
            }
        }

        /// <summary>
        /// Gets the common nouns in the exercise text.
        /// </summary>
        public IDictionary<IWord, int> CommonNouns
        {
            get
            {
                return this.GetWordsByPartOfSpeech(PartOfSpeech.CommonNoun);
            }
        }

        /// <summary>
        /// Gets the proper nouns in the exercise text.
        /// </summary>
        public IDictionary<IWord, int> ProperNouns
        {
            get
            {
                return this.GetWordsByPartOfSpeech(PartOfSpeech.ProperNoun);
            }
        }

        /// <summary>
        /// Gets the parts of a proper noun in the exercise text.
        /// </summary>
        public IDictionary<IWord, int> PartsOfProperNouns
        {
            get
            {
                return this.GetWordsByPartOfSpeech(PartOfSpeech.PartOfProperNoun);
            }
        }

        /// <summary>
        /// Gets the cardinal numbers in the exercise text.
        /// </summary>
        public IDictionary<IWord, int> CardinalNumbers
        {
            get
            {
                return this.GetWordsByPartOfSpeech(PartOfSpeech.CardinalNumber);
            }
        }

        /// <summary>
        /// Gets the ordinal numbers in the exercise text.
        /// </summary>
        public IDictionary<IWord, int> OrdinalNumbers
        {
            get
            {
                return this.GetWordsByPartOfSpeech(PartOfSpeech.Ordinal);
            }
        }

        /// <summary>
        /// Gets the prepositions in the exercise text.
        /// </summary>
        public IDictionary<IWord, int> Prepositions
        {
            get
            {
                return this.GetWordsByPartOfSpeech(PartOfSpeech.Preposition);
            }
        }

        /// <summary>
        /// Gets the pronouns in the exercise text.
        /// </summary>
        public IDictionary<IWord, int> Pronouns
        {
            get
            {
                return this.GetWordsByPartOfSpeech(PartOfSpeech.Pronoun);
            }
        }

        /// <summary>
        /// Gets the unclassified words in the exercise text.
        /// </summary>
        public IDictionary<IWord, int> UnclassifiedWords
        {
            get
            {
                return this.GetWordsByPartOfSpeech(PartOfSpeech.Unclassified);
            }
        }

        /// <summary>
        /// Gets the verbs in the exercise text.
        /// </summary>
        public IDictionary<IWord, int> Verbs
        {
            get
            {
                return this.GetWordsByPartOfSpeech(PartOfSpeech.Verb);
            }
        }

        /// <summary>
        /// Gets the modal verbs in the exercise text.
        /// </summary>
        public IDictionary<IWord, int> ModalVerbs
        {
            get
            {
                return this.GetWordsByPartOfSpeech(PartOfSpeech.ModalVerb);
            }
        }

        /// <summary>
        /// Gets the result of the Adelex analysis.
        /// </summary>
        public AdelexResultEntry AdelexResult
        {
            get
            {
                if (this._onlineAnalysisDirtyFlag)
                {
                    this.PerformFullOnlineAnalysis();
                }

                return this._adelexResult;
            }
        }

        /// <summary>
        /// Gets a list of words in the essay content grouped by their language
        /// level.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Need to return a list of values as a key")]
        public Dictionary<LanguageLevelClassification, List<IWord>> LevelClassification
        {
            get
            {
                return (from word in this._offlineAnalysisCache
                        group word by word.Level into wl
                        select new { Level = wl.Key, Words = wl.ToList() })
                        .ToDictionary(w => w.Level, w => w.Words);
            }
        }

        /// <summary>
        /// Gets the distribution relative to the language distribution of words.
        /// </summary>
        /// <returns>A dictionary having the possible language levels as keys and the relative word frequency as its
        /// value.</returns>
        public Dictionary<LanguageLevelClassification, float> LevelClassificationDistribution
        {
            get
            {
                // TODO: are all groups returned?
                return (from word in this._offlineAnalysisCache
                        group word by word.Level into wl
                        select new { Level = wl.Key, WordCount = wl.ToList().Count })
                        .ToDictionary(w => w.Level, w => w.WordCount / (float)this._offlineAnalysisCache.Count);
            }
        }

        /// <summary>
        /// Gets the result of the LexTutor analysis.
        /// </summary>
        public LexTutorResultEntry LexTutorResult
        {
            get
            {
                if (this._onlineAnalysisDirtyFlag)
                {
                    this.PerformFullOnlineAnalysis();
                }

                return this._lexTutorResult;
            }
        }

        /// <summary>
        /// Gets the number of offline analyzed words in the essay.
        /// </summary>
        public int NumWords
        {
            get
            {
                return this._offlineAnalysisCache.Count;
            }
        }

        // TODO: move these to converters!

        /// <summary>
        /// Gets the color associated to the exercise.
        /// </summary>
        public Color StatusColor
        {
            get
            {
                return this.Status.ToColor();
            }
        }

        /// <summary>
        /// Gets a human-readable name for the exercise type.
        /// </summary>
        /// <returns>The human-readable name for the exercise type.</returns>
        public new string ToNiceString()
        {
            return Properties.Resources.Exercise_EssayName;
        }

        /// <summary>
        /// Returns the cumulative percentage of the passed dictionaries with
        /// respect to the total amount of words.
        /// </summary>
        /// <param name="dictionaries">An <see cref="IList{PartOfSpeech}"/>
        /// containing the dictionaries this function should examine.</param>
        /// <returns>The cumulative percentage of the dictionaries listed in
        /// <paramref name="dictionaries"/>.</returns>
        public float GetNumWordsPercentage(IList<PartOfSpeech> dictionaries)
        {
            return this._offlineAnalysisCache.Where(w => dictionaries.Contains(w.PartOfSpeech)).Count() / (float)this._offlineAnalysisCache.Count;
        }

        /// <summary>
        /// Gets a list of words according to their parts of speech.
        /// </summary>
        /// <param name="part">The part of speech the words belong to.</param>
        /// <returns>A <see cref="Dictionary{IWord, Integer}"/> containing all
        /// the words in the essay contents belonging to the specified part of
        /// speech and the number of their occurrencies.</returns>
        public IDictionary<IWord, int> GetWordsByPartOfSpeech(PartOfSpeech part)
        {
            return this._offlineAnalysisCache
                .Where(w => w.PartOfSpeech == part)
                .GroupBy(w => w)
                .Where(w => w.Count() >= 1)
                .ToDictionary(w => w.Key, w => w.Count());
        }

        /// <summary>
        /// Performs a full offline analysis of the contents of the essay, excluding all functional words.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task PerformFullOfflineAnalysis()
        {
            await this.PerformFullOfflineAnalysis(true);
        }

        /// <summary>
        /// Performs a full offline analysis of the contents of the essay.
        /// </summary>
        /// <param name="excludeFunctional">If <c>true</c>, exclude all
        /// functional words from the analysis.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task PerformFullOfflineAnalysis(bool excludeFunctional)
        {
            if (!this._offlineAnalysisDirtyFlag)
            {
                return;
            }

            this._offlineAnalysisCache = new List<IWord>();

            foreach (string token in Regex.Matches(this._essayContents, "\\w+").Cast<Match>().Select(m => m.Value))
            {
                IWord w;
                IList<IWord> offlineWords = await OfflineWord.Search(token);
                if (offlineWords.Count > 0)
                {
                    w = WordSearchUtilities.GetMostProbable(offlineWords);
                }
                else
                {
                    // TODO: find a better way to add a word that is not found
                    // in the database.
                    Tools.Logger.Log(this, "Count is zero, so I'm creating a new word with " + PartOfSpeech.Unclassified + " PoS and " + LanguageLevelClassification.UNKNOWN + " language");
                    w = new OfflineWord()
                    {
                        Term = token,
                        Level = LanguageLevelClassification.UNKNOWN,
                        PartOfSpeech = PartOfSpeech.Unclassified,
                        Language = "en-US"
                    };
                }

                if (excludeFunctional && (
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
                    || w.PartOfSpeech == PartOfSpeech.Preposition
                    || w.PartOfSpeech == PartOfSpeech.Pronoun
                    || w.PartOfSpeech == PartOfSpeech.ModalVerb))
                {
                    continue;
                }

                this._offlineAnalysisCache.Add(w);
            }

            this._printResult();
            this._offlineAnalysisDirtyFlag = false;
        }

        /// <summary>
        /// Performs a full online analysis of the contents of the essay.
        /// </summary>
        public async void PerformFullOnlineAnalysis()
        {
            if (!this._onlineAnalysisDirtyFlag)
            {
                return;
            }

            LexTutor lexTutor = new LexTutor(App.OAuth2Account, this._essayTitle, this._essayContents);
            try
            {
                this._lexTutorResult = await lexTutor.CallEndpointAsObjectAsync();
            }
            catch (UnsuccessfulAPICallException ex)
            {
                Tools.Logger.Log(this, "PerformFullOnlineAnalysis method - LexTutor", ex);
            }
            catch (System.Exception ex)
            {
                Tools.Logger.Log(this, "PerformFullOnlineAnalysis method - LexTutor", ex);
            }

            /*
            Adelex adelex = new Adelex(App.OAuth2Account, this._essayContents, AdelexOrder.Frequency);
            try
            {
                this._adelexResult = await adelex.CallEndpointAsObjectAsync();
            }
            catch (UnsuccessfulAPICallException ex)
            {
                Tools.Logger.Log(this, "PerformFullOnlineAnalysis method - Adelex", ex);
            }
            catch (System.Exception ex)
            {
                Tools.Logger.Log(this, "PerformFullOnlineAnalysis method - Adelex", ex);
            }
            */

            this._onlineAnalysisDirtyFlag = false;
        }

        private float countpercent = 0;
        private int counttot = 0;

        private void _printResult()
        {
            string msg = "Offline Analysis Result";

            msg += "\nTotal words: " + this.NumWords;
            msg += "\n" + this.getResultStringForASinglePartOfSpeech(PartOfSpeech.Adjective);
            msg += "\n" + this.getResultStringForASinglePartOfSpeech(PartOfSpeech.Adverb);
            msg += "\n" + this.getResultStringForASinglePartOfSpeech(PartOfSpeech.ClauseOpener);
            msg += "\n" + this.getResultStringForASinglePartOfSpeech(PartOfSpeech.Conjunction);
            msg += "\n" + this.getResultStringForASinglePartOfSpeech(PartOfSpeech.Determiner);
            msg += "\n" + this.getResultStringForASinglePartOfSpeech(PartOfSpeech.DeterminerPronoun);
            msg += "\n" + this.getResultStringForASinglePartOfSpeech(PartOfSpeech.ExistentialParticle);
            msg += "\n" + this.getResultStringForASinglePartOfSpeech(PartOfSpeech.ForeignWord);
            msg += "\n" + this.getResultStringForASinglePartOfSpeech(PartOfSpeech.Genitive);
            msg += "\n" + this.getResultStringForASinglePartOfSpeech(PartOfSpeech.InfinitiveMarker);
            msg += "\n" + this.getResultStringForASinglePartOfSpeech(PartOfSpeech.InterjectionOrDiscourseMarker);
            msg += "\n" + this.getResultStringForASinglePartOfSpeech(PartOfSpeech.LetterAsWord);
            msg += "\n" + this.getResultStringForASinglePartOfSpeech(PartOfSpeech.NegativeMarker);
            msg += "\n" + this.getResultStringForASinglePartOfSpeech(PartOfSpeech.CommonNoun);
            msg += "\n" + this.getResultStringForASinglePartOfSpeech(PartOfSpeech.ProperNoun);
            msg += "\n" + this.getResultStringForASinglePartOfSpeech(PartOfSpeech.PartOfProperNoun);
            msg += "\n" + this.getResultStringForASinglePartOfSpeech(PartOfSpeech.CardinalNumber);
            msg += "\n" + this.getResultStringForASinglePartOfSpeech(PartOfSpeech.Ordinal);
            msg += "\n" + this.getResultStringForASinglePartOfSpeech(PartOfSpeech.Preposition);
            msg += "\n" + this.getResultStringForASinglePartOfSpeech(PartOfSpeech.Pronoun);
            msg += "\n" + this.getResultStringForASinglePartOfSpeech(PartOfSpeech.Unclassified);
            msg += "\n" + this.getResultStringForASinglePartOfSpeech(PartOfSpeech.Verb);
            msg += "\n" + this.getResultStringForASinglePartOfSpeech(PartOfSpeech.ModalVerb);
            msg += "\n" + this.getResultStringForASinglePartOfSpeech(PartOfSpeech.AuxiliaryVerb);
            msg += "\n" + this.getResultStringForASinglePartOfSpeech(PartOfSpeech.Exclamation);

            msg += "\n Total counted: " + counttot + " - Percentage: " + countpercent.ToString("0.00");

            msg += "\nUnclassified:";
            foreach (IWord w in this.GetWordsByPartOfSpeech(PartOfSpeech.Unclassified).Keys)
            {
                msg += " " + w.Term;
            }

            Tools.Logger.Log(this, msg);
        }

        private string getResultStringForASinglePartOfSpeech(PartOfSpeech elem)
        {
            IDictionary<IWord, int> f1 = this.GetWordsByPartOfSpeech(elem);
            counttot += f1.Count;

            float f2 = this.GetNumWordsPercentage(new List<PartOfSpeech>() { elem });
            this.countpercent += f2;

            return f1.Count + "\t" + f2.ToString("0.00") + " " + elem;
        }
    }
}
