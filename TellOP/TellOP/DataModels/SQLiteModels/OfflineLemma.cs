// <copyright file="OfflineLemma.cs" company="University of Murcia">
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

namespace TellOP.DataModels.SQLiteModels
{
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using SQLite;
    using System.Text.RegularExpressions;
    using System.Linq;

    /// <summary>
    /// Reflect the records in the SQLite Lemma table
    /// </summary>
    [Table("lemmas")]
    public class OfflineLemma
    {
        /// <summary>
        /// Gets Lemma
        /// </summary>
        [Column("lemma")]
        [Indexed]
        [MaxLength(50)]
        public string Lemma { get; set; } = null; // http://stackoverflow.com/a/40754/2875987

        /// <summary>
        /// Gets or sets Base
        /// </summary>
        [Column("base")]
        [Indexed]
        [MaxLength(50)]
        public string Base { get; set; } = null; // http://stackoverflow.com/a/40754/2875987

        /// <summary>
        /// Retrieve from the database the correct lemma.
        /// </summary>
        /// <param name="lemma">Single term</param>
        /// <returns>The corresponding base term</returns>
        public static string RetrieveBase(string lemma)
        {
            lemma = ReplaceAllEnglishContractions(lemma);

            string result = string.Empty;

            // A foreach is requested in order to parse each possible word in the combination.
            // e.g. "something'dn't've" -> "something had not have" -> "something have not have"
            foreach (string singleWord in Regex.Matches(lemma, "[\\w']+").Cast<Match>().Select(m => m.Value))
            {
                if (singleWord != null)
                {
                    string dbResult = CallDB(singleWord);
                    if (!string.IsNullOrEmpty(dbResult))
                    {
                        result += " " + dbResult;
                    }
                    else
                    {
                        result += " " + singleWord;
                    }
                }
            }

            return result.TrimStart();
        }

        /// <summary>
        /// Gets the <see cref="SQLiteConnection"/> object for the Lemma dictionary.
        /// </summary>
        private static SQLiteConnection LemmaConnection
        {
            get
            {
                return SQLiteManager.LocalLemmasDictionaryConnection;
            }
        }

        /// <summary>
        /// Perform the real call to the DB.
        /// </summary>
        /// <param name="singleWord">A single token</param>
        /// <returns>If exist, the real element</returns>
        private static string CallDB(string singleWord)
        {
            string result = null;
            try
            {
                lock (LemmaConnection)
                {
                    Expression<Func<OfflineLemma, bool>> exp = l => l.Lemma.Equals(singleWord);
                    OfflineLemma query = LemmaConnection.Table<OfflineLemma>().Where(exp).FirstOrDefault();
                    if (query != null)
                    {
                        if (query.Base != null)
                        {
                            result = query.Base;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Tools.Logger.Log("DBSearch", ex);
            }

            return result;
        }

        /// <summary>
        /// Replace all the authorized english contraction with the uncontracted string.
        /// https://en.wikipedia.org/wiki/Wikipedia:List_of_English_contractions
        /// </summary>
        /// <param name="text">Text to be cleaned</param>
        /// <returns>Clean text</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="text"/> is <c>null</c>.</exception>
        public static string ReplaceAllEnglishContractions(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException("text");
            }

            string tmpText = text;
            string msg = "Replaced '" + text + "'";

            // In the average case, the character ' is not present, so this should optimize this method.
            if (!text.Contains("'"))
            {
                text = text.Replace("gonna", "going to");
                text = text.Replace("gotta", "got to");
                text = text.Replace("wanna", "want to");
            }
            else
            {
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
            }

            if (!tmpText.Equals(text))
            {
                Tools.Logger.Log("OfflineLemmaPreparse", msg + "with '" + text + "'");
            }

            return text;
        }
    }
}
