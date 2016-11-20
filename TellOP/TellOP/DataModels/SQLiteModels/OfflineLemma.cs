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
// <author>Alessandro Menti</author>

namespace TellOP.DataModels.SQLiteModels
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using global::SQLite;

    /// <summary>
    /// A lemma in the locale SQLite dictionary.
    /// </summary>
    [Table("lemmas")]
    public class OfflineLemma
    {
        /// <summary>
        /// A dictionary containing a list of English contractions not containing an apostrophe.
        /// </summary>
        private static readonly Dictionary<string, string> EnglishContractionsWithoutApostrophe = new Dictionary<string, string>()
        {
            { "gonna", "going to" },
            { "gotta", "got to" },
            { "wanna", "want to" }
        };

        /// <summary>
        /// A dictionary containing a list of English contractions containing an apostrophe.
        /// </summary>
        /// <remarks>The contractions should be listed from the longer one to the shorter one to avoid situations
        /// where a shorter contraction is a part of a longer one (and the shorter one is replaced first).</remarks>
        private static readonly Dictionary<string, string> EnglishContractionsWithApostrophe = new Dictionary<string, string>()
        {
            { "something'dn't've", "something had not have" },
            { "somebody'dn't've", "somebody had not have" },
            { "someone'dn't've", "someone had not have" },
            { "something'dn't", "something had not" },
            { "something'd've", "something would have" },
            { "y'all'll'ven't", "you all will have not" },
            { "somebody'dn't", "somebody had not" },
            { "somebody'd've", "somebody would have" },
            { "there'dn't've", "there would not have" },
            { "they'lln't've", "they will not have" },
            { "they'll'ven't", "they will have not" },
            { "y'all'dn't've", "you all would not have" },
            { "shouldn't've", "should not have" },
            { "someone'dn't", "someone had not" },
            { "someone'd've", "someone would have" },
            { "something'll", "something shall" },
            { "they'dn't've", "they would not have" },
            { "they'd'ven't", "they would have not" },
            { "couldn't've", "could not have" },
            { "mightn't've", "might not have" },
            { "oughtn't've", "ought not to have" },
            { "she'dn't've", "she had not have" },
            { "somebody'll", "somebody shall" },
            { "something'd", "something had" },
            { "something's", "something has" },
            { "we'lln't've", "we will not have" },
            { "wouldn't've", "would not have" },
            { "y'all'll've", "you all will have" },
            { "he'dn't've", "he had not have" },
            { "it'dn't've", "it had not have " },
            { "mustn't've", "must not have" },
            { "somebody'd", "somebody had" },
            { "somebody's", "somebody has" },
            { "someone'll", "someone shall" },
            { "there'dn't", "there had not" },
            { "there'd've", "there would have" },
            { "they'ven't", "they have not" },
            { "we'dn't've", "we had not have" },
            { "y'all'd've", "you all would have" },
            { "y'all'on't", "you all will not" },
            { "hadn't've", "had not have" },
            { "i'dn't've", "i had not have" },
            { "should've", "should have" },
            { "shouldn't", "should not" },
            { "someone'd", "someone had" },
            { "someone's", "someone has" },
            { "they'dn't", "they had not" },
            { "they'd've", "they would have" },
            { "you'ren't", "you are not" },
            { "you'ven't", "you have not" },
            { "could've", "could have" },
            { "couldn't", "could not" },
            { "mightn't", "might not" },
            { "might've", "might have" },
            { "oughtn't", "ought not" },
            { "she'dn't", "she had not" },
            { "she'd've", "she would have" },
            { "she'sn't", "she has not" },
            { "there're", "there are" },
            { "where've", "where have" },
            { "who'd've", "who would have" },
            { "won't've", "will not have" },
            { "would've", "would have" },
            { "wouldn't", "would not" },
            { "y'all'll", "you all will" },
            { "y'all're", "you all are" },
            { "you'd've", "you would have" },
            { "doesn't", "does not" },
            { "haven't", "have not" },
            { "he'dn't", "he had not" },
            { "he'd've", "he would have" },
            { "he'sn't", "he has not " },
            { "i'ven't", "i have not" },
            { "it'dn't", "it had not " },
            { "it'd've", "it would have" },
            { "it'sn't", "it has not" },
            { "mustn't", "must not" },
            { "must've", "must have" },
            { "needn't", "need not" },
            { "o'clock", "of the clock" },
            { "that'll", "that will" },
            { "there'd", "there would" },
            { "there's", "there has" },
            { "they'll", "they will" },
            { "they're", "they are" },
            { "they've", "they have" },
            { "we'd've", "we would have" },
            { "we'dn't", "we would not" },
            { "weren't", "were not" },
            { "what'll", "what will" },
            { "what're", "what are" },
            { "what've", "what have" },
            { "where'd", "where did" },
            { "where's", "where has" },
            { "aren't", "are not" },
            { "didn't", "did not" },
            { "hadn't", "had not" },
            { "hasn't", "has not" },
            { "how'll", "how will" },
            { "i'dn't", "i had not" },
            { "i'd've", "i would have" },
            { "not've", "not have" },
            { "shan't", "shall not" },
            { "she'll", "she shall" },
            { "s'pose", "suppose" },
            { "that's", "that has" },
            { "that'd", "that would" },
            { "they'd", "they had" },
            { "wasn't", "was not" },
            { "what'd", "what did" },
            { "what's", "what has" },
            { "when's", "when has" },
            { "who'll", "who shall" },
            { "who're", "who are" },
            { "who've", "who have" },
            { "why'll", "why will" },
            { "why're", "why are" },
            { "you'll", "you shall" },
            { "you're", "you are" },
            { "you've", "you have" },
            { "ain't", "am not" },
            { "amn't", "am not" },
            { "can't", "cannot" },
            { "cap'm", "captain" },
            { "cap'n", "captain" },
            { "don't", "do not" },
            { "he'll", "he shall" },
            { "how'd", "how did" },
            { "how's", "how is" },
            { "isn't", "is not" },
            { "it'll", "it shall" },
            { "let's", "let us" },
            { "ma'am", "madam" },
            { "ne'er", "never" },
            { "she'd", "she had" },
            { "she's", "she has" },
            { "'twas", "it was" },
            { "we'll", "we will" },
            { "we're", "we are" },
            { "we've", "we have" },
            { "who'd", "who would" },
            { "who's", "who has" },
            { "why'd", "why did" },
            { "why's", "why has " },
            { "won't", "will not" },
            { "ya'll", "you all" },
            { "y'all", "you all" },
            { "you'd", "you had" },
            { "e'er", "ever" },
            { "he'd", "he had" },
            { "he's", "he has" },
            { "i'll", "i will" },
            { "i've", "i have" },
            { "it'd", "it had" },
            { "it's", "it is" },
            { "'sup", "what is up" },
            { "'tis", "it is" },
            { "we'd", "we had" },
            { "i'd", "i had" },
            { "i'm", "i am" },
            { "ol'", "old" }
        };

        /// <summary>
        /// Gets or sets the lemma.
        /// </summary>
        [Column("lemma")]
        [Indexed]
        [MaxLength(50)]
        public string Lemma { get; set; }

        /// <summary>
        /// Gets or sets the base word for <see cref="Lemma"/>.
        /// </summary>
        [Column("base")]
        [MaxLength(50)]
        public string Base { get; set; }

        /// <summary>
        /// Retrieves a lemma from the local SQLite database.
        /// </summary>
        /// <param name="lemma">The lemma to search for.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task<string> RetrieveBase(string lemma)
        {
            lemma = ReplaceAllEnglishContractions(lemma);

            StringBuilder result = new StringBuilder();

            // A foreach is requested in order to parse each possible word in the combination.
            // e.g. "something'dn't've" -> "something had not have" -> "something have not have"
            foreach (string singleWord in Regex.Matches(lemma, "[\\w']+").Cast<Match>().Select(m => m.Value))
            {
                if (singleWord != null)
                {
                    string dbResult = await CallDB(singleWord).ConfigureAwait(false);
                    if (!string.IsNullOrEmpty(dbResult))
                    {
                        result.Append(" ");
                        result.Append(dbResult);
                    }
                    else
                    {
                        result.Append(" ");
                        result.Append(singleWord);
                    }
                }
            }

            return result.ToString().TrimStart();
        }

        /// <summary>
        /// Replace all possible English contractions with the uncontracted string.
        /// </summary>
        /// <param name="text">The text to be analyzed.</param>
        /// <returns>The text without any contractions.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="text"/> is <c>null</c>.</exception>
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "TellOP.Tools.Logger.Log(System.String,System.String)", Justification = "This affects only a log string which must not be localized")]
        public static string ReplaceAllEnglishContractions(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException("text");
            }

            // For optimization purposes, replace contractions containing an apostrophe only if the original text
            // contains an apostrophe.
            // TODO: does this really bring a performance gain?
            IEnumerable<string> tokenizedText = new List<string>(Regex.Matches(text, "[\\w']+").Cast<Match>().Select(m => m.Value));
            tokenizedText = tokenizedText.Select(i => EnglishContractionsWithoutApostrophe.ContainsKey(i) ? EnglishContractionsWithoutApostrophe[i] : i);
            if (text.Contains("'"))
            {
                tokenizedText = tokenizedText.Select(i => EnglishContractionsWithApostrophe.ContainsKey(i) ? EnglishContractionsWithApostrophe[i] : i);
            }

            string newText = string.Join(" ", tokenizedText);

            if (!newText.Equals(text))
            {
                Tools.Logger.Log("OfflineLemmaPreparse", "Replaced '" + text + "' with '" + text + "'");
            }

            return newText;
        }

        /// <summary>
        /// Search the lemma inside the local SQLite database.
        /// </summary>
        /// <param name="singleWord">The word to search for.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private static async Task<string> CallDB(string singleWord)
        {
            string result = null;
            try
            {
                Expression<Func<OfflineLemma, bool>> exp = l => l.Lemma.Equals(singleWord);
                OfflineLemma query;
                SQLiteManager dbManager = SQLiteManager.Instance;

                query = await dbManager.LocalLemmasDictionaryConnection.Table<OfflineLemma>().Where(exp).FirstOrDefaultAsync().ConfigureAwait(false);

                if (query != null)
                {
                    if (query.Base != null)
                    {
                        result = query.Base;
                    }
                }
            }
            catch (Exception ex)
            {
                // TODO: remove the exception
                Tools.Logger.Log("DBSearch", ex);
            }

            return result;
        }
    }
}
