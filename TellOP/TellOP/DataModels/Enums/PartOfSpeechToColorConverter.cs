// <copyright file="PartOfSpeechToColorConverter.cs" company="University of Murcia">
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
    using System.Collections.Generic;
    using Xamarin.Forms;

    /// <summary>
    /// Converts a value of <see cref="PartOfSpeech"/> to the corresponding color.
    /// </summary>
    public class PartOfSpeechToColorConverter : DictionaryConverter<PartOfSpeech, Color>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PartOfSpeechToColorConverter"/> class.
        /// </summary>
        public PartOfSpeechToColorConverter()
        {
            this.ConverterDictionary = new Dictionary<PartOfSpeech, Color>()
            {
                { PartOfSpeech.Adjective, Color.FromHex("#CE93D8") },
                { PartOfSpeech.Adverb, Color.FromHex("#9575CD") },
                { PartOfSpeech.AuxiliaryVerb, Color.Default },
                { PartOfSpeech.CardinalNumber, Color.Default },
                { PartOfSpeech.ClauseOpener, Color.FromHex("#DCE775") },
                { PartOfSpeech.CommonNoun, Color.FromHex("#64B5F6") },
                { PartOfSpeech.Conjunction, Color.Default },
                { PartOfSpeech.Determiner, Color.FromHex("#7986CB") },
                { PartOfSpeech.DeterminerPronoun, Color.FromHex("#F06292") },
                { PartOfSpeech.Exclamation, Color.Default },
                { PartOfSpeech.ExistentialParticle, Color.FromHex("#FFF176") },
                { PartOfSpeech.ForeignWord, Color.Default },
                { PartOfSpeech.Genitive, Color.FromHex("#FFD54F") },
                { PartOfSpeech.InfinitiveMarker, Color.FromHex("#FFB74D") },
                { PartOfSpeech.InterjectionOrDiscourseMarker, Color.FromHex("#FF8A65") },
                { PartOfSpeech.LetterAsWord, Color.Default },
                { PartOfSpeech.ModalVerb, Color.FromHex("#81C784") },
                { PartOfSpeech.NegativeMarker, Color.Default },
                { PartOfSpeech.Ordinal, Color.Default },
                { PartOfSpeech.PartOfProperNoun, Color.FromHex("#4DD0E1") },
                { PartOfSpeech.Preposition, Color.FromHex("#E57373") },
                { PartOfSpeech.Pronoun, Color.FromHex("#A1887F") },
                { PartOfSpeech.ProperNoun, Color.FromHex("#4FC3F7") },
                { PartOfSpeech.Unclassified, Color.FromHex("#E0E0E0") },
                { PartOfSpeech.Verb, Color.FromHex("#4DB6AC") }
            };
        }
    }
}
