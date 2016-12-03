// <copyright file="Stands4Word.cs" company="University of Murcia">
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

namespace TellOP.DataModels.ApiModels.Stands4
{
    using System;
    using Api;
    using Enums;
    using Nito.AsyncEx;
    using SQLiteModels;

    /// <summary>
    /// A word returned by the <see cref="Stands4Dictionary"/> API controller.
    /// </summary>
    public class Stands4Word : IWord
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Stands4Word"/> class.
        /// </summary>
        /// <param name="definition">An instance of <see cref="DictionarySingleDefinition"/> containing the definition
        /// extracted from the Web service.</param>
        public Stands4Word(DictionarySingleDefinition definition)
        {
            if (definition == null)
            {
                throw new ArgumentNullException("definition");
            }

            this.PartOfSpeech = definition.PartOfSpeech;
            this.Definition = definition.Definition;
            this.Example = definition.Example;
            this.Term = definition.Term;
            this.Synonyms = definition.Synonyms;
            this.Antonyms = definition.Antonyms;
            this.Level = new AsyncLazy<LanguageLevelClassification>(async () =>
            {
                return await WordSearchUtilities.GetMostProbable(await OfflineWord.Search(this.Term, SupportedLanguage.English)).Level;
            });
        }

        /// <summary>
        /// Gets the CEFR level of this word.
        /// </summary>
        public AsyncLazy<LanguageLevelClassification> Level { get; private set; }

        /// <summary>
        /// Gets the part of speech this term is categorized into.
        /// </summary>
        public PartOfSpeech PartOfSpeech { get; private set; }

        /// <summary>
        /// Gets the string representation of this word.
        /// </summary>
        public string Term { get; private set; }

        /// <summary>
        /// Gets the definition obtained by the remote API.
        /// </summary>
        public string Definition { get; private set; }

        /// <summary>
        /// Gets the examples obtained by the remote API.
        /// </summary>
        public string Example { get; private set; }

        /// <summary>
        /// Gets the synonyms obtained by the remote API.
        /// </summary>
        public string Synonyms { get; private set; }

        /// <summary>
        /// Gets the antonyms obtained by the remote API.
        /// </summary>
        public string Antonyms { get; private set; }
    }
}
