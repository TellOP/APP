// <copyright file="CollinsWord.cs" company="University of Murcia">
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

namespace TellOP.DataModels.APIModels.Collins
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
    using Enums;
    using SQLiteModels;

    /// <summary>
    /// A word extracted from the Collins dictionary.
    /// </summary>
    public class CollinsWord : IWord
    {
        /// <summary>
        /// Cache for the remote Collins word entry.
        /// </summary>
        private CollinsJSONEnglishDictionaryEntryContentEntry _collinsEntry;

        /// <summary>
        /// Cache for the local DB word corresponding to the Collins word.
        /// </summary>
        private IWord _localWord;

        /// <summary>
        /// Initializes a new instance of the <see cref="CollinsWord"/> class.
        /// </summary>
        /// <param name="e">The <see cref="CollinsJSONEnglishDictionaryEntryContentEntry"/>
        /// corresponding to this word.</param>
        /// <param name="id">The unique ID of this word.</param>
        /// <param name="label">The label (word name) of this word.</param>
        /// <param name="url">The URL pointing to the page on the Collins
        /// Dictionary Web site where the definition can be seen.</param>
        public CollinsWord(
            CollinsJSONEnglishDictionaryEntryContentEntry e,
            string id,
            string label,
            Uri url)
        {
            if (e == null)
            {
                throw new ArgumentNullException(
                    "e",
                    "The Collins dictionary entry can not be null");
            }

            if (id == null)
            {
                throw new ArgumentNullException(
                    "id",
                    "The definition ID can not be null");
            }

            if (label == null)
            {
                throw new ArgumentNullException(
                    "label",
                    "The definition label can not be null");
            }

            if (url == null)
            {
                throw new ArgumentNullException(
                    "url",
                    "The definition URL can not be null");
            }

            this._collinsEntry = e;
            this.EntryId = id;
            this.Term = label;
            this.Url = url;
        }

        /// <summary>
        /// Gets the CEFR level of this word.
        /// </summary>
        public LanguageLevelClassification Level
        {
            get
            {
                if (this._localWord == null)
                {
                    this.CacheLocalWord();
                }

                return this._localWord.Level;
            }
        }

        /// <summary>
        /// Gets the part of speech this term is categorized into.
        /// </summary>
        public PartOfSpeech PartOfSpeech
        {
            get
            {
                return this._collinsEntry.PartOfSpeech;
            }
        }

        /// <summary>
        /// Gets the string representation of this word.
        /// </summary>
        public string Term { get; private set; }

        /// <summary>
        /// Gets the unique entry ID used by the Collins dictionary API.
        /// </summary>
        public string EntryId { get; private set; }

        /// <summary>
        /// Gets the URL for the entry.
        /// </summary>
        public Uri Url { get; private set; }

        /// <summary>
        /// Gets the content of the definition for this word.
        /// </summary>
        public string Content
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                for (int senseCounter = 0;
                    senseCounter < this._collinsEntry.Senses.Count;
                    ++senseCounter)
                {
                    sb.AppendFormat(
                        Properties.Resources.CollinsWord_SenseCounter,
                        senseCounter);
                    if (this._collinsEntry.Senses[senseCounter].Definitions.Count > 0)
                    {
                        StringBuilder dsb = new StringBuilder();
                        int defCounter = 1;
                        foreach (string definition in this._collinsEntry.Senses[senseCounter].Definitions)
                        {
                            dsb.AppendFormat(
                                "{0}. {1}\r\n",
                                new object[] { defCounter, definition });
                            ++defCounter;
                        }

                        sb.AppendFormat(
                            Properties.Resources.CollinsWord_DefinitionTemplate,
                            dsb.ToString());
                    }

                    if (this._collinsEntry.Senses[senseCounter].Examples.Count > 0)
                    {
                        StringBuilder esb = new StringBuilder();
                        int exCounter = 1;
                        foreach (string example in this._collinsEntry.Senses[senseCounter].Examples)
                        {
                            esb.AppendFormat(
                                "{0}. {1}\r\n",
                                new object[] { exCounter, example });
                            ++exCounter;
                        }

                        sb.AppendFormat(
                            Properties.Resources.CollinsWord_ExamplesTemplate,
                            esb.ToString());
                    }

                    if (this._collinsEntry.Senses[senseCounter].SeeAlso.Count > 0)
                    {
                        StringBuilder sasb = new StringBuilder();
                        int saCounter = 1;
                        foreach (CollinsJSONLinkedWord seeAlso in this._collinsEntry.Senses[senseCounter].SeeAlso)
                        {
                            sasb.AppendFormat(
                                "{0}. {1}\r\n",
                                new object[] { saCounter, seeAlso.Content });
                            ++saCounter;
                        }

                        sb.AppendFormat(
                            Properties.Resources.CollinsWord_ExamplesTemplate,
                            sasb.ToString());
                    }

                    if (this._collinsEntry.Senses[senseCounter].Related.Count > 0)
                    {
                        StringBuilder rsb = new StringBuilder();
                        int rCounter = 1;
                        foreach (CollinsJSONLinkedWord related in this._collinsEntry.Senses[senseCounter].Related)
                        {
                            rsb.AppendFormat(
                                "{0}. {1}\r\n",
                                new object[] { rCounter, related.Content });
                            ++rCounter;
                        }

                        sb.AppendFormat(
                            Properties.Resources.CollinsWord_ExamplesTemplate,
                            rsb.ToString());
                    }
                }

                return sb.ToString();
            }
        }

        /// <summary>
        /// Gets the <see cref="CollinsWord.Content"/> parsed as multiple
        /// senses.
        /// </summary>
        public IList<CollinsWordDefinitionSense> Senses
        {
            get
            {
                return this._collinsEntry.Senses;
            }
        }

        /// <summary>
        /// Caches a local DB word corresponding to the Collins word.
        /// </summary>
        private async void CacheLocalWord()
        {
            if (this._localWord == null)
            {
                try
                {
                    this._localWord = WordSearchUtilities.GetMostProbable(await OfflineWord.Search(this.Term, Enums.SupportedLanguage.English));
                }
                catch (Exception ex)
                {
                    Tools.Logger.Log(this, ex);
                }
            }
        }
    }
}
