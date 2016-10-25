// <copyright file="OfflineWord.cs" company="University of Murcia">
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

namespace TellOP.DataModels.SQLiteModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Enums;
    using Nito.AsyncEx;
    using SQLite;

    /// <summary>
    /// A word extracted from the offline words list.
    /// </summary>
    [Table("words")]
    public class OfflineWord : IWord
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OfflineWord"/> class.
        /// </summary>
        public OfflineWord()
        {
            this.Level = new AsyncLazy<LanguageLevelClassification>(() =>
            {
                return this.JSONLevel;
            });
        }

        /// <summary>
        /// Gets or sets the CEFR level of this word.
        /// </summary>
        [Column("cefr_level")]
        public LanguageLevelClassification JSONLevel { get; set; }

        /// <summary>
        /// Gets the CEFR level of this word. Used by the <see cref="IWord"/> interface.
        /// </summary>
        public AsyncLazy<LanguageLevelClassification> Level { get; private set; }

        /// <summary>
        /// Gets or sets the part of speech for this word.
        /// </summary>
        [Column("part_of_speech")]
        public PartOfSpeech PartOfSpeech { get; set; }

        /// <summary>
        /// Gets or sets the string representation of this word.
        /// </summary>
        [Column("word")]
        [Indexed]
        [MaxLength(100)]
        public string Term { get; set; }

        /// <summary>
        /// Gets or sets the category this word belongs to.
        /// </summary>
        [Column("category")]
        [MaxLength(100)]
        public string Category { get; set; }

        // TODO: check if this can be expressed using the SupportedLanguage enum

        /// <summary>
        /// Gets or sets the language of this word.
        /// </summary>
        [Column("language")]
        [MaxLength(9)]
        public string Language { get; set; }

        /// <summary>
        /// Perform a search for a word in the application's SQLite database.
        /// </summary>
        /// <param name="word">The word to search.</param>
        /// <returns>An <see cref="IList{IWord}"/> object containing the words that were found.</returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Need to return a list inside a Task")]
        public static Task<IList<IWord>> Search(string word)
        {
            return Search(word, SupportedLanguage.English);
        }

        /// <summary>
        /// Perform a search for a word in the application's SQLite database.
        /// </summary>
        /// <param name="word">The word to search.</param>
        /// <param name="language">The supported language the word belongs to.</param>
        /// <returns>An <see cref="IList{IWord}"/> object containing the words that were found.</returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Need to return a list inside a Task")]
        public static async Task<IList<IWord>> Search(string word, SupportedLanguage language)
        {
            // Forced lowercase to avoid mismatch
            word = word.ToLower();

            string msg = "Searched word: '" + word + "'";

            // my fix for incredibly stupid errors
            if (word == "are" || word == "is" || word == "am")
            {
                Tools.Logger.Log("OfflineWord", msg + "\tHardcoded verb: (be as " + PartOfSpeech.Verb + ")");
                return new ReadOnlyCollection<IWord>(new List<IWord>()
                {
                    new OfflineWord()
                    {
                        Term = "be",
                        PartOfSpeech = PartOfSpeech.Verb,
                        Language = SupportedLanguage.English.ToString(),
                        JSONLevel = LanguageLevelClassification.A1,
                        Category = "Hardcoded result"
                    }
                });
            }

            // my fix for incredibly stupid errors
            if (word == "has" || word == "have")
            {
                Tools.Logger.Log("OfflineWord", msg + "\tHardcoded verb: (have as " + PartOfSpeech.Verb + ")");
                return new ReadOnlyCollection<IWord>(new List<IWord>()
                {
                    new OfflineWord()
                    {
                        Term = "have",
                        PartOfSpeech = PartOfSpeech.Verb,
                        Language = SupportedLanguage.English.ToString(),
                        JSONLevel = LanguageLevelClassification.A1,
                        Category = "Hardcoded result"
                    }
                });
            }

            // my fix for incredibly stupid errors
            if (word == "i")
            {
                Tools.Logger.Log("OfflineWord", msg + "\tHardcoded pronoun: (I as " + PartOfSpeech.Pronoun + ")");
                return new ReadOnlyCollection<IWord>(new List<IWord>()
                {
                    new OfflineWord()
                    {
                        Term = "I",
                        PartOfSpeech = PartOfSpeech.Pronoun,
                        Language = SupportedLanguage.English.ToString(),
                        JSONLevel = LanguageLevelClassification.A1,
                        Category = "Hardcoded result"
                    }
                });
            }

            // my fix for incredibly stupid errors
            int val;
            if (int.TryParse(word, out val))
            {
                Tools.Logger.Log("OfflineWord", msg + "\tHardcoded number: (" + val + " as " + PartOfSpeech.CardinalNumber + ")");
                return new ReadOnlyCollection<IWord>(new List<IWord>()
                {
                    new OfflineWord()
                    {
                        Term = string.Empty + val,
                        PartOfSpeech = PartOfSpeech.CardinalNumber,
                        Language = SupportedLanguage.English.ToString(),
                        JSONLevel = LanguageLevelClassification.A1,
                        Category = "Hardcoded result"
                    }
                });
            }

            string wordLCID = (string)new SupportedLanguageToLcidConverter().Convert(language, typeof(string), null, CultureInfo.InvariantCulture);

            IList<IWord> retList = new List<IWord>();

            // First check if any identic word exists.
            await Task.Run(() =>
            {
                lock (SQLiteManager.LocalWordsDictionaryConnection)
                {
                    Expression<Func<OfflineWord, bool>> exp = w => w.Term.Equals(word);
                    var query = SQLiteManager.LocalWordsDictionaryConnection.Table<OfflineWord>().Where(exp);

                    if (query.Count() > 0)
                    {
                        msg += "\tFound:\t";
                    }

                    foreach (OfflineWord w in query)
                    {
                        msg += " (" + w.Term + " as " + w.PartOfSpeech + ")";
                        retList.Add(w);
                    }
                }
            });

            // If, and only if, there aren't valid results, expand the search algorithm.
            // Moreover, the word must be larger than 3 chars.(too many results otherwise).
            if (retList.Count == 0 && word.Length >= 3)
            {
                if (word.EndsWith("s"))
                {
                    word = word.Substring(0, word.Length - 1);
                    Tools.Logger.Log("OfflineWord", msg);
                    IList<IWord> result = await OfflineWord.Search(word, language);
                    if (result.Count > 0)
                    {
                        return result;
                    }
                }
            }

            // If, and only if, there aren't valid results, expand the search algorithm.
            // Moreover, the word must be larger than 3 chars.(too many results otherwise).
            if (retList.Count == 0 && word.Length >= 3)
            {
                msg += "\tExpanding the search";
                await Task.Run(() =>
                {
                    lock (SQLiteManager.LocalWordsDictionaryConnection)
                    {
                        // Default where clause.
                        Expression<Func<OfflineWord, bool>> exp = w => (w.Term.StartsWith(word) || w.Term.EndsWith(word));
                        var query = SQLiteManager.LocalWordsDictionaryConnection.Table<OfflineWord>().Where(exp);

                        if (query.Count() > 0)
                        {
                            msg += "\tFound:\t";
                        }
                        else
                        {
                            msg += "\tNothing found";
                        }

                        foreach (OfflineWord w in query)
                        {
                            msg += " (" + w.Term + " as " + w.PartOfSpeech + ")";
                            retList.Add(w);
                        }
                    }
                });
            }

            Tools.Logger.Log("OfflineWord", msg);

            return new ReadOnlyCollection<IWord>(retList);
        }
    }
}
