// <copyright file="PartOfSpeechToStringConverter.cs" company="University of Murcia">
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

namespace TellOP.DataModels.Enums
{
    using System.Collections.Generic;

    /// <summary>
    /// This class includes several static methods used to represent <see cref="PartOfSpeech"/> objects in a view.
    /// </summary>
    public class PartOfSpeechToStringConverter : DictionaryMonoDirectionalConverter<PartOfSpeech, string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PartOfSpeechToStringConverter"/> class.
        /// </summary>
        public PartOfSpeechToStringConverter()
        {
            this.ConverterDictionary = new Dictionary<PartOfSpeech, string>()
            {
                { PartOfSpeech.Adjective, Properties.Resources.PartOfSpeech_Adjective },
                { PartOfSpeech.Adverb, Properties.Resources.PartOfSpeech_Adverb },
                { PartOfSpeech.AuxiliaryVerb, Properties.Resources.PartOfSpeech_AuxiliaryVerb },
                { PartOfSpeech.CardinalNumber, Properties.Resources.PartOfSpeech_CardinalNumber },
                { PartOfSpeech.ClauseOpener, Properties.Resources.PartOfSpeech_ClauseOpener },
                { PartOfSpeech.CommonNoun, Properties.Resources.PartOfSpeech_CommonNoun },
                { PartOfSpeech.Conjunction, Properties.Resources.PartOfSpeech_Conjunction },
                { PartOfSpeech.Determiner, Properties.Resources.PartOfSpeech_Determiner },
                { PartOfSpeech.DeterminerPronoun, Properties.Resources.PartOfSpeech_Determiner_Pronoun },
                { PartOfSpeech.Exclamation, Properties.Resources.PartOfSpeech_Exclamation },
                { PartOfSpeech.ExistentialParticle, Properties.Resources.PartOfSpeech_ExistentialParticle },
                { PartOfSpeech.ForeignWord, Properties.Resources.PartOfSpeech_ForeignWord },
                { PartOfSpeech.Genitive, Properties.Resources.PartOfSpeech_Genitive },
                { PartOfSpeech.InfinitiveMarker, Properties.Resources.PartOfSpeech_InfinitiveMarker },
                { PartOfSpeech.InterjectionOrDiscourseMarker, Properties.Resources.PartOfSpeech_Interjection },
                { PartOfSpeech.LetterAsWord, Properties.Resources.PartOfSpeech_LetterAsWord },
                { PartOfSpeech.ModalVerb, Properties.Resources.PartOfSpeech_ModalVerb },
                { PartOfSpeech.NegativeMarker, Properties.Resources.PartOfSpeech_NegativeMarker },
                { PartOfSpeech.Ordinal, Properties.Resources.PartOfSpeech_Ordinal },
                { PartOfSpeech.PartOfProperNoun, Properties.Resources.PartOfSpeech_PartProperNoun },
                { PartOfSpeech.Preposition, Properties.Resources.PartOfSpeech_Preposition },
                { PartOfSpeech.Pronoun, Properties.Resources.PartOfSpeech_Pronoun },
                { PartOfSpeech.ProperNoun, Properties.Resources.PartOfSpeech_ProperNoun },
                { PartOfSpeech.Unclassified, Properties.Resources.PartOfSpeech_Unclassified },
                { PartOfSpeech.Verb, Properties.Resources.PartOfSpeech_Verb }
            };
        }
    }
}
