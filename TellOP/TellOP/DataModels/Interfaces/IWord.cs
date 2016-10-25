// <copyright file="IWord.cs" company="University of Murcia">
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

namespace TellOP.DataModels
{
    using Enums;
    using Nito.AsyncEx;

    /// <summary>
    /// A single word used in the exercises.
    /// </summary>
    public interface IWord
    {
        /// <summary>
        /// Gets the string representation of the term.
        /// </summary>
        string Term { get; }

        /// <summary>
        /// Gets the part of speech this term is categorized into.
        /// </summary>
        PartOfSpeech PartOfSpeech { get; }

        /// <summary>
        /// Gets the CEFR level of this word.
        /// </summary>
        AsyncLazy<LanguageLevelClassification> Level { get; }
    }
}
