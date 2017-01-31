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

namespace TellOP.DataModels.ApiModels.Collins
{
    using System;
    using System.Collections.Generic;
    using DataModels.Enums;
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
                return await WordSearchUtilities.GetMostProbable(await OfflineWord.Search(this.Term, SupportedLanguage.English)).Level;
            });
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
        /// Gets the definition content parsed as multiple senses.
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
