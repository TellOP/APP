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
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Enums;
    using SQLite;
    using Xamarin.Forms;

    /// <summary>
    /// A word extracted from the offline words list.
    /// </summary>
    [Table("words")]
    public class OfflineWord : IWord
    {
        /// <summary>
        /// The SQLite connection to the local database.
        /// </summary>
        private static SQLiteConnection databaseConnection = null;

        /// <summary>
        /// The SQLite database lock.
        /// </summary>
        private static object databaseLock = new object();

        /// <summary>
        /// Gets or sets the CEFR level of this word.
        /// </summary>
        [Column("cefr_level")]
        public LanguageLevelClassification Level { get; set; }

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
        /// <returns>An <see cref="IList{IWord}"/> object containing the
        /// words that were found.</returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Need to return a list inside a Task")]
        public static Task<IList<IWord>> Search(string word)
        {
            return Search(word, SupportedLanguage.English);
        }

        /// <summary>
        /// Perform a search for a word in the application's SQLite database.
        /// </summary>
        /// <param name="word">The word to search.</param>
        /// <param name="language">The supported language the word belongs
        /// to.</param>
        /// <returns>An <see cref="IList{IWord}"/> object containing the
        /// words that were found.</returns>
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
                        Level = LanguageLevelClassification.A1,
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
                        Level = LanguageLevelClassification.A1,
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
                        Level = LanguageLevelClassification.A1,
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
                        Level = LanguageLevelClassification.A1,
                        Category = "Hardcoded result"
                    }
                });
            }

            string wordLCID =
                SupportedLanguageExtension.GetLCID(language);

            if (databaseConnection == null)
            {
                databaseConnection = DependencyService.Get<ISQLite>().GetConnection("LocalDictionary.sqlite", SQLiteOpenFlags.ReadOnly);
            }

            IList<IWord> retList = new List<IWord>();

            // First check if any identic word exists.
            await Task.Run(() =>
            {
                lock (databaseLock)
                {
                    Expression<Func<OfflineWord, bool>> exp = w => w.Term.Equals(word);
                    var query = databaseConnection.Table<OfflineWord>().Where(exp);

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
                    msg += "\tRemoving final 's' and reload the search (new word: " + word + ")";
                    Tools.Logger.Log("OfflineWord", msg);
                    IList<IWord> result = await OfflineWord.Search(word, language);
                    if (result.Count > 0)
                    {
                        return result;
                    }
                }

                if (word.EndsWith("ing"))
                {
                    word = word.Substring(0, word.Length - 3);
                    msg += "\tRemoving final 'ing' and reload the search (new word: " + word + ")";
                    Tools.Logger.Log("OfflineWord", msg);
                    IList<IWord> result = await OfflineWord.Search(word, language);
                    if (result.Count > 0)
                    {
                        return result;
                    }
                }

                if (word.EndsWith("d"))
                {
                    word = word.Substring(0, word.Length - 1);
                    msg += "\tRemoving final 'ed' and reload the search (new word: " + word + ")";
                    Tools.Logger.Log("OfflineWord", msg);
                    IList<IWord> result = await OfflineWord.Search(word, language);
                    if (result.Count > 0)
                    {
                        return result;
                    }
                }

                if (word.EndsWith("ed"))
                {
                    word = word.Substring(0, word.Length - 2);
                    msg += "\tRemoving final 'ed' and reload the search (new word: " + word + ")";
                    Tools.Logger.Log("OfflineWord", msg);
                    IList<IWord> result = await OfflineWord.Search(word, language);
                    if (result.Count > 0)
                    {
                        return result;
                    }
                }

                if (word.EndsWith("ment"))
                {
                    word = word.Substring(0, word.Length - 4);
                    msg += "\tRemoving final 'ment' and reload the search (new word: " + word + ")";
                    Tools.Logger.Log("OfflineWord", msg);
                    IList<IWord> result = await OfflineWord.Search(word, language);
                    if (result.Count > 0)
                    {
                        return result;
                    }
                }

                if (word.EndsWith("iful"))
                {
                    word = word.Replace("iful", "y");
                    msg += "\tReplace final 'iful' with 'y' and reload the search (new word: " + word + ")";
                    Tools.Logger.Log("OfflineWord", msg);
                    IList<IWord> result = await OfflineWord.Search(word, language);
                    if (result.Count > 0)
                    {
                        return result;
                    }
                }

                if (word.EndsWith("ssion"))
                {
                    word = word.Replace("ssion", "t");
                    msg += "\tReplace final 'ssion' with 't' and reload the search (new word: " + word + ")";
                    Tools.Logger.Log("OfflineWord", msg);
                    IList<IWord> result = await OfflineWord.Search(word, language);
                    if (result.Count > 0)
                    {
                        return result;
                    }
                }

                if (word.EndsWith("ility"))
                {
                    word = word.Replace("ility", "e");
                    msg += "\tReplace final 'ility' with 'e' and reload the search (new word: " + word + ")";
                    Tools.Logger.Log("OfflineWord", msg);
                    IList<IWord> result = await OfflineWord.Search(word, language);
                    if (result.Count > 0)
                    {
                        return result;
                    }
                }

                /*
                if (word.EndsWith("XXXXX"))
                {
                    word = word.Substring(0, word.Length - XXXX);
                    msg += "\tRemoving final 'XXXXX' and reload the search (new word: " + word + ")";
                    Tools.Logger.Log("OfflineWord", msg);
                    IList<IWord> result = await OfflineWord.Search(word, language);
                    if (result.Count > 0)
                    {
                        return result;
                    }
                }
                */
            }

            // If, and only if, there aren't valid results, expand the search algorithm.
            // Moreover, the word must be larger than 3 chars.(too many results otherwise).
            if (retList.Count == 0 && word.Length >= 3)
            {
                msg += "\tExpanding the search";
                await Task.Run(() =>
                {
                    lock (databaseLock)
                    {
                        // Default where clause.
                        Expression<Func<OfflineWord, bool>> exp = w => (w.Term.StartsWith(word) || w.Term.EndsWith(word));
                        var query = databaseConnection.Table<OfflineWord>().Where(exp);

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

            /* FIXME: not sure that this case returns any correct result. I don't think.
            // If there aren't still valid results, expand the search algorithm to any generic contains.
            // Moreover, the word must be larger than 3 chars.(too many results otherwise).
            if (retList.Count == 0 && word.Length >= 3)
            {
                Debug.WriteLine("OfflineWord: Expanding the search algorithm");
                await Task.Run(() =>
                {
                    lock (databaseLock)
                    {
                        // Default where clause.
                        Expression<Func<OfflineWord, bool>> exp = w => w.Term.Contains(word);
                        var query = databaseConnection.Table<OfflineWord>().Where(exp);

                        foreach (OfflineWord w in query)
                        {
                            retList.Add(w);
                        }
                    }
                });
            }
            */

            Tools.Logger.Log("OfflineWord", msg);

            return new ReadOnlyCollection<IWord>(retList);
        }

        /// <summary>
        /// Replace all the authorized english contraction with the uncontracted string.
        /// https://en.wikipedia.org/wiki/Wikipedia:List_of_English_contractions
        /// </summary>
        /// <param name="text">Text to be cleaned</param>
        /// <returns>Clean text</returns>
        public static string ReplaceAllEnglishContractions(string text)
        {
            Tools.Logger.Log("OfflineWord", "Real Text: \n" + text);

            // The order is from the longer to the shorter to avoid the match on a single part.
            text = text.Replace("something'dn't've", "something had not have");
            text = text.Replace("somebody'dn't've", "somebody had not have");
            text = text.Replace("someone'dn't've", "someone had not have");
            text = text.Replace("cap'm or cap'n", "captain");
            text = text.Replace("something'dn't", "something had not");
            text = text.Replace("something'd've", "something would have");
            text = text.Replace("y'all'll'ven't", "you all will have not");
            text = text.Replace("somebody'dn't", "somebody had not");
            text = text.Replace("somebody'd've", "somebody would have");
            text = text.Replace("there'dn't've", "there would not have");
            text = text.Replace("they'lln't've", "they will not have");
            text = text.Replace("they'll'ven't", "they will have not");
            text = text.Replace("y'all'dn't've", "you all would not have");
            text = text.Replace("shouldn't've", "should not have");
            text = text.Replace("someone'dn't", "someone had not");
            text = text.Replace("someone'd've", "someone would have");
            text = text.Replace("something'll", "something shall");
            text = text.Replace("they'dn't've", "they would not have");
            text = text.Replace("they'd'ven't", "they would have not");
            text = text.Replace("couldn't've", "could not have");
            text = text.Replace("mightn't've", "might not have");
            text = text.Replace("oughtn't've", "ought not to have");
            text = text.Replace("she'dn't've", "she had not have");
            text = text.Replace("somebody'll", "somebody shall");
            text = text.Replace("something'd", "something had");
            text = text.Replace("something's", "something has");
            text = text.Replace("we'lln't've", "we will not have");
            text = text.Replace("wouldn't've", "would not have");
            text = text.Replace("y'all'll've", "you all will have");
            text = text.Replace("he'dn't've", "he had not have");
            text = text.Replace("it'dn't've", "it had not have ");
            text = text.Replace("mustn't've", "must not have");
            text = text.Replace("somebody'd", "somebody had");
            text = text.Replace("somebody's", "somebody has");
            text = text.Replace("someone'll", "someone shall");
            text = text.Replace("there'dn't", "there had not");
            text = text.Replace("there'd've", "there would have");
            text = text.Replace("they'ven't", "they have not");
            text = text.Replace("we'dn't've", "we had not have");
            text = text.Replace("y'all'd've", "you all would have");
            text = text.Replace("y'all'on't", "you all will not");
            text = text.Replace("hadn't've", "had not have");
            text = text.Replace("i'dn't've", "i had not have");
            text = text.Replace("should've", "should have");
            text = text.Replace("shouldn't", "should not");
            text = text.Replace("someone'd", "someone had");
            text = text.Replace("someone's", "someone has");
            text = text.Replace("they'dn't", "they had not");
            text = text.Replace("they'd've", "they would have");
            text = text.Replace("you'ren't", "you are not");
            text = text.Replace("you'ven't", "you have not");
            text = text.Replace("could've", "could have");
            text = text.Replace("couldn't", "could not");
            text = text.Replace("mightn't", "might not");
            text = text.Replace("might've", "might have");
            text = text.Replace("oughtn't", "ought not");
            text = text.Replace("she'dn't", "she had not");
            text = text.Replace("she'd've", "she would have");
            text = text.Replace("she'sn't", "she has not");
            text = text.Replace("there're", "there are");
            text = text.Replace("where've", "where have");
            text = text.Replace("who'd've", "who would have");
            text = text.Replace("won't've", "will not have");
            text = text.Replace("would've", "would have");
            text = text.Replace("wouldn't", "would not");
            text = text.Replace("y'all'll", "you all will");
            text = text.Replace("y'all're", "you all are");
            text = text.Replace("you'd've", "you would have");
            text = text.Replace("doesn't", "does not");
            text = text.Replace("haven't", "have not");
            text = text.Replace("he'dn't", "he had not");
            text = text.Replace("he'd've", "he would have");
            text = text.Replace("he'sn't", "he has not ");
            text = text.Replace("i'ven't", "i have not");
            text = text.Replace("it'dn't", "it had not ");
            text = text.Replace("it'd've", "it would have");
            text = text.Replace("it'sn't", "it has not ");
            text = text.Replace("mustn't", "must not");
            text = text.Replace("must've", "must have");
            text = text.Replace("needn't", "need not");
            text = text.Replace("o'clock", "of the clock");
            text = text.Replace("that'll", "that will");
            text = text.Replace("there'd", "there would");
            text = text.Replace("there's", "there has");
            text = text.Replace("they'll", "they will");
            text = text.Replace("they're", "they are");
            text = text.Replace("they've", "they have");
            text = text.Replace("we'd've", "we would have");
            text = text.Replace("we'dn't", "we would not");
            text = text.Replace("weren't", "were not");
            text = text.Replace("what'll", "what will");
            text = text.Replace("what're", "what are");
            text = text.Replace("what've", "what have");
            text = text.Replace("where'd", "where did");
            text = text.Replace("where's", "where has");
            text = text.Replace("aren't", "are not");
            text = text.Replace("didn't", "did not");
            text = text.Replace("hadn't", "had not");
            text = text.Replace("hasn't", "has not");
            text = text.Replace("how'll", "how will");
            text = text.Replace("i'dn't", "i had not");
            text = text.Replace("i'd've", "i would have");
            text = text.Replace("not've", "not have");
            text = text.Replace("shan't", "shall not");
            text = text.Replace("she'll", "she shall");
            text = text.Replace("s'pose", "suppose");
            text = text.Replace("that's", "that has");
            text = text.Replace("that'd", "that would");
            text = text.Replace("they'd", "they had");
            text = text.Replace("wasn't", "was not");
            text = text.Replace("what'd", "what did");
            text = text.Replace("what's", "what has");
            text = text.Replace("when's", "when has");
            text = text.Replace("who'll", "who shall");
            text = text.Replace("who're", "who are");
            text = text.Replace("who've", "who have");
            text = text.Replace("why'll", "why will");
            text = text.Replace("why're", "why are");
            text = text.Replace("you'll", "you shall");
            text = text.Replace("you're", "you are");
            text = text.Replace("you've", "you have");
            text = text.Replace("ain't", "am not");
            text = text.Replace("amn't", "am not");
            text = text.Replace("can't", "cannot");
            text = text.Replace("don't", "do not");
            text = text.Replace("gonna", "going to");
            text = text.Replace("gotta", "got to");
            text = text.Replace("he'll", "he shall");
            text = text.Replace("how'd", "how did");
            text = text.Replace("how's", "how is");
            text = text.Replace("isn't", "is not");
            text = text.Replace("it'll", "it shall ");
            text = text.Replace("let's", "let us");
            text = text.Replace("ma'am", "madam");
            text = text.Replace("ne'er", "never");
            text = text.Replace("she'd", "she had");
            text = text.Replace("she's", "she has");
            text = text.Replace("'twas", "it was");
            text = text.Replace("wanna", "want to");
            text = text.Replace("we'll", "we will");
            text = text.Replace("we're", "we are");
            text = text.Replace("we've", "we have");
            text = text.Replace("who'd", "who would");
            text = text.Replace("who's", "who has");
            text = text.Replace("why'd", "why did");
            text = text.Replace("why's", "why has ");
            text = text.Replace("won't", "will not");
            text = text.Replace("ya'll", "you all");
            text = text.Replace("y'all", "you all");
            text = text.Replace("you'd", "you had");
            text = text.Replace("e'er", "ever");
            text = text.Replace("he'd", "he had");
            text = text.Replace("he's", "he has");
            text = text.Replace("i'll", "i will");
            text = text.Replace("i've", "i have");
            text = text.Replace("it'd", "it had ");
            text = text.Replace("it's", "it is");
            text = text.Replace("'sup", "what is up");
            text = text.Replace("'tis", "it is");
            text = text.Replace("we'd", "we had ");
            text = text.Replace("i'd", "i had");
            text = text.Replace("i'm", "i am");
            text = text.Replace("ol'", "old");
            Tools.Logger.Log("OfflineWord", "New Text: \n" + text);
            return text;
        }
    }
}
