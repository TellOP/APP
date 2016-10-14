// <copyright file="LanguageLevelClassification.cs" company="University of Murcia">
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
// <author>Alessandro Menti</author>

namespace TellOP.DataModels.Enums
{
    // Note: the numeric values associated to the enum are needed for the SQLite
    // "integer to enum" mapping in the local dictionary to work. At the time of
    // writing, there is no way to store the enum values as text (a patch was
    // accepted upstream, however).

    /// <summary>
    /// The language level classification of the user according to the
    /// Common European Framework of Reference for Languages.
    /// </summary>
    public enum LanguageLevelClassification
    {
        /// <summary>
        /// Breakthrough: can understand and use familiar everyday
        /// expressions and very basic phrases aimed at the satisfaction of
        /// needs of a concrete type. Can introduce him/herself and others
        /// and can ask and answer questions about personal details such as
        /// where he/she lives, people he/she knows and things he/she has.
        /// Can interact in a simple way provided the other person talks
        /// slowly and clearly and is prepared to help.
        /// </summary>
        A1 = 0,

        /// <summary>
        /// Waystage: can understand sentences and frequently used
        /// expressions related to areas of most immediate relevance (e.g.
        /// very basic personal and family information, shopping, local
        /// geography, employment). Can communicate in simple and routine
        /// tasks requiring a simple and direct exchange of information on
        /// familiar and routine matters. Can describe in simple terms
        /// aspects of his/her background, immediate environment and
        /// matters in areas of immediate need.
        /// </summary>
        A2 = 1,

        /// <summary>
        /// Threshold: can understand the main points of clear standard
        /// input on familiar matters regularly encountered in work,
        /// school, leisure, etc. Can deal with most situations likely to
        /// arise whilst travelling in an area where the language is
        /// spoken. Can produce simple connected text on topics which are
        /// familiar or of personal interest. Can describe experiences and
        /// events, dreams, hopes and ambitions and briefly give reasons
        /// and explanations for opinions and plans.
        /// </summary>
        B1 = 2,

        /// <summary>
        /// Vantage: can understand the main ideas of complex text on both
        /// concrete and abstract topics, including technical discussions
        /// in his/her field of specialisation. Can interact with a degree
        /// of fluency and spontaneity that makes regular interaction with
        /// native speakers quite possible without strain for either party.
        /// Can produce clear, detailed text on a wide range of subjects
        /// and explain a viewpoint on a topical issue giving the
        /// advantages and disadvantages of various options.
        /// </summary>
        B2 = 3,

        /// <summary>
        /// Effective operational proficiency: can understand a wide range
        /// of demanding, longer texts, and recognise implicit meaning. Can
        /// express him/herself fluently and spontaneously without much
        /// obvious searching for expressions. Can use language flexibly
        /// and effectively for social, academic and professional purposes.
        /// Can produce clear, well-structured, detailed text on complex
        /// subjects, showing controlled use of organisational patterns,
        /// connectors and cohesive devices.
        /// </summary>
        C1 = 4,

        /// <summary>
        /// Mastery: can understand with ease virtually everything heard or
        /// read. Can summarise information from different spoken and
        /// written sources, reconstructing arguments and accounts in a
        /// coherent presentation. Can express him/herself spontaneously,
        /// very fluently and precisely, differentiating finer shades of
        /// meaning even in more complex situations.
        /// </summary>
        C2 = 5,

        /// <summary>
        /// A default value, used to highlight that a classification is
        /// missing or not available at the moment.
        /// </summary>
        UNKNOWN = 6
    }
}
