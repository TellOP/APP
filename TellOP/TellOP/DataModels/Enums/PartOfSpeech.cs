// <copyright file="PartOfSpeech.cs" company="University of Murcia">
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
    // Note: the numeric values associated to the enum are needed for the SQLite
    // "integer to enum" mapping in the local dictionary to work. At the time of
    // writing, there is no way to store the enum values as text (a patch was
    // accepted upstream, however).

    /// <summary>
    /// A list of parts of speech.
    /// </summary>
    public enum PartOfSpeech
    {
        /// <summary>
        /// An adjective.
        /// </summary>
        Adjective = 0,

        /// <summary>
        /// An adverb.
        /// </summary>
        Adverb = 1,

        /// <summary>
        /// A clause opener.
        /// </summary>
        ClauseOpener = 2,

        /// <summary>
        /// A conjunction.
        /// </summary>
        Conjunction = 3,

        /// <summary>
        /// A determiner.
        /// </summary>
        Determiner = 4,

        /// <summary>
        /// A determiner/pronoun.
        /// </summary>
        DeterminerPronoun = 5,

        /// <summary>
        /// An existential particle.
        /// </summary>
        ExistentialParticle = 6,

        /// <summary>
        /// A foreign word.
        /// </summary>
        ForeignWord = 7,

        /// <summary>
        /// A genitive.
        /// </summary>
        Genitive = 8,

        /// <summary>
        /// An infinitive marker.
        /// </summary>
        InfinitiveMarker = 9,

        /// <summary>
        /// An interjection or discourse marker.
        /// </summary>
        InterjectionOrDiscourseMarker = 10,

        /// <summary>
        /// A letter of the alphabet treated as a word.
        /// </summary>
        LetterAsWord = 11,

        /// <summary>
        /// A negative marker.
        /// </summary>
        NegativeMarker = 12,

        /// <summary>
        /// A common noun.
        /// </summary>
        CommonNoun = 13,

        /// <summary>
        /// A proper noun.
        /// </summary>
        ProperNoun = 14,

        /// <summary>
        /// A part of a proper noun.
        /// </summary>
        PartOfProperNoun = 15,

        /// <summary>
        /// A cardinal number.
        /// </summary>
        CardinalNumber = 16,

        /// <summary>
        /// An ordinal number.
        /// </summary>
        Ordinal = 17,

        /// <summary>
        /// A preposition.
        /// </summary>
        Preposition = 18,

        /// <summary>
        /// A pronoun.
        /// </summary>
        Pronoun = 19,

        /// <summary>
        /// An unclassified part of speech.
        /// </summary>
        Unclassified = 20,

        /// <summary>
        /// A verb.
        /// </summary>
        Verb = 21,

        /// <summary>
        /// A modal verb.
        /// </summary>
        ModalVerb = 22,

        /// <summary>
        /// An auxiliary verb.
        /// </summary>
        AuxiliaryVerb = 23,

        /// <summary>
        /// An exclamation.
        /// </summary>
        Exclamation = 24
    }
}
