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
    using System.Text;
    using Enums;
    using Nito.AsyncEx;
    using SQLiteModels;

    /// <summary>
    /// A word extracted from the Collins dictionary.
    /// </summary>
    public class CollinsWord : IWord
    {
        /// <summary>
        /// Cache for the remote Collins word entry.
        /// </summary>
        private CollinsJsonEnglishDictionaryEntryContentEntry _collinsEntry;

        /// <summary>
        /// Initializes a new instance of the <see cref="CollinsWord"/> class.
        /// </summary>
        /// <param name="entry">The <see cref="CollinsJsonEnglishDictionaryEntryContentEntry"/> corresponding to this
        /// word.</param>
        /// <param name="id">The unique ID of this word.</param>
        /// <param name="label">The label (word name) of this word.</param>
        /// <param name="url">The URL pointing to the page on the Collins Dictionary Web site where the definition can
        /// be seen.</param>
        public CollinsWord(CollinsJsonEnglishDictionaryEntryContentEntry entry, string id, string label, Uri url)
        {
            if (entry == null)
            {
                throw new ArgumentNullException("entry");
            }

            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            if (label == null)
            {
                throw new ArgumentNullException("label");
            }

            if (url == null)
            {
                throw new ArgumentNullException("url");
            }

            this._collinsEntry = entry;
            this.EntryId = id;
            this.Term = label;
            this.Url = url;
            this.Level = new AsyncLazy<LanguageLevelClassification>(async () =>
            {
                return await WordSearchUtilities.GetMostProbable(OfflineWord.Search(this.Term, SupportedLanguage.English)).Level;
            });
        }

        /// <summary>
        /// Gets the content of the definition for this word.
        /// </summary>
        public string Content
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                for (int senseCounter = 0; senseCounter < this._collinsEntry.Senses.Count; ++senseCounter)
                {
                    sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, Properties.Resources.CollinsWord_SenseCounter, senseCounter);
                    if (this._collinsEntry.Senses[senseCounter].Definitions.Count > 0)
                    {
                        StringBuilder dsb = new StringBuilder();
                        int defCounter = 1;
                        foreach (string definition in this._collinsEntry.Senses[senseCounter].Definitions)
                        {
                            dsb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "{0}. {1}\r\n", defCounter, definition);
                            ++defCounter;
                        }

                        sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, Properties.Resources.CollinsWord_DefinitionTemplate, dsb.ToString());
                    }

                    if (this._collinsEntry.Senses[senseCounter].Examples.Count > 0)
                    {
                        StringBuilder esb = new StringBuilder();
                        int exCounter = 1;
                        foreach (string example in this._collinsEntry.Senses[senseCounter].Examples)
                        {
                            esb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "{0}. {1}\r\n", exCounter, example);
                            ++exCounter;
                        }

                        sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, Properties.Resources.CollinsWord_ExamplesTemplate, esb.ToString());
                    }

                    if (this._collinsEntry.Senses[senseCounter].SeeAlso.Count > 0)
                    {
                        StringBuilder sasb = new StringBuilder();
                        int saCounter = 1;
                        foreach (CollinsJsonLinkedWord seeAlso in this._collinsEntry.Senses[senseCounter].SeeAlso)
                        {
                            sasb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "{0}. {1}\r\n", saCounter, seeAlso.Content);
                            ++saCounter;
                        }

                        sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, Properties.Resources.CollinsWord_ExamplesTemplate, sasb.ToString());
                    }

                    if (this._collinsEntry.Senses[senseCounter].Related.Count > 0)
                    {
                        StringBuilder rsb = new StringBuilder();
                        int rCounter = 1;
                        foreach (CollinsJsonLinkedWord related in this._collinsEntry.Senses[senseCounter].Related)
                        {
                            rsb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "{0}. {1}\r\n", rCounter, related.Content);
                            ++rCounter;
                        }

                        sb.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, Properties.Resources.CollinsWord_ExamplesTemplate, rsb.ToString());
                    }
                }

                return sb.ToString();
            }
        }

        /// <summary>
        /// Gets the unique entry ID used by the Collins dictionary API.
        /// </summary>
        public string EntryId { get; private set; }

        /// <summary>
        /// Gets the CEFR level of this word.
        /// </summary>
        public AsyncLazy<LanguageLevelClassification> Level { get; private set; }

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
        /// Gets the <see cref="Content"/> parsed as multiple senses.
        /// </summary>
        public IList<CollinsWordDefinitionSense> Senses
        {
            get
            {
                return this._collinsEntry.Senses;
            }
        }

        /// <summary>
        /// Gets the string representation of this word.
        /// </summary>
        public string Term { get; private set; }

        /// <summary>
        /// Gets the URL for the entry.
        /// </summary>
        public Uri Url { get; private set; }
    }
}
