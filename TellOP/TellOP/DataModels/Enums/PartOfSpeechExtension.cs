// <copyright file="PartOfSpeechExtension.cs" company="University of Murcia">
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
    using Xamarin.Forms;

    /// <summary>
    /// This class includes several static methods used to represent
    /// <see cref="PartOfSpeech"/> objects in a view.
    /// </summary>
    internal static class PartOfSpeechExtension
    {
        /// <summary>
        /// Given a part of speech, returns the corresponding localized friendly
        /// name.
        /// </summary>
        /// <param name="p">The part of speech to localize.</param>
        /// <returns>The localized friendly named corresponding to the given
        /// part of speech.</returns>
        public static string GetFriendlyName(this PartOfSpeech p)
        {
            switch (p)
            {
                case PartOfSpeech.Adjective:
                    return Properties.Resources.PartOfSpeech_Adjective;
                case PartOfSpeech.Adverb:
                    return Properties.Resources.PartOfSpeech_Adverb;
                case PartOfSpeech.ClauseOpener:
                    return Properties.Resources.PartOfSpeech_ClauseOpener;
                case PartOfSpeech.Conjunction:
                    return Properties.Resources.PartOfSpeech_Conjunction;
                case PartOfSpeech.Determiner:
                    return Properties.Resources.PartOfSpeech_Determiner;
                case PartOfSpeech.DeterminerPronoun:
                    return Properties.Resources.PartOfSpeech_Determiner_Pronoun;
                case PartOfSpeech.ExistentialParticle:
                    return Properties.Resources.PartOfSpeech_ExistentialParticle;
                case PartOfSpeech.ForeignWord:
                    return Properties.Resources.PartOfSpeech_ForeignWord;
                case PartOfSpeech.Genitive:
                    return Properties.Resources.PartOfSpeech_Genitive;
                case PartOfSpeech.InfinitiveMarker:
                    return Properties.Resources.PartOfSpeech_InfinitiveMarker;
                case PartOfSpeech.InterjectionOrDiscourseMarker:
                    return Properties.Resources.PartOfSpeech_Interjection;
                case PartOfSpeech.LetterAsWord:
                    return Properties.Resources.PartOfSpeech_LetterAsWord;
                case PartOfSpeech.NegativeMarker:
                    return Properties.Resources.PartOfSpeech_NegativeMarker;
                case PartOfSpeech.CommonNoun:
                    return Properties.Resources.PartOfSpeech_CommonNoun;
                case PartOfSpeech.ProperNoun:
                    return Properties.Resources.PartOfSpeech_ProperNoun;
                case PartOfSpeech.PartOfProperNoun:
                    return Properties.Resources.PartOfSpeech_PartProperNoun;
                case PartOfSpeech.CardinalNumber:
                    return Properties.Resources.PartOfSpeech_CardinalNumber;
                case PartOfSpeech.Ordinal:
                    return Properties.Resources.PartOfSpeech_Ordinal;
                case PartOfSpeech.Preposition:
                    return Properties.Resources.PartOfSpeech_Preposition;
                case PartOfSpeech.Pronoun:
                    return Properties.Resources.PartOfSpeech_Pronoun;
                case PartOfSpeech.Unclassified:
                    return Properties.Resources.PartOfSpeech_Unclassified;
                case PartOfSpeech.Verb:
                    return Properties.Resources.PartOfSpeech_Verb;
                case PartOfSpeech.ModalVerb:
                    return Properties.Resources.PartOfSpeech_ModalVerb;
                default:
                    return Properties.Resources.PartOfSpeech_Unrecognized;
            }
        }

        /// <summary>
        /// Given a part of speech, returns the corresponding color.
        /// </summary>
        /// <param name="p">The part of speech to color.</param>
        /// <returns>The representation color corresponding to the given part
        /// of speech.</returns>
        public static Color GetColor(this PartOfSpeech p)
        {
            switch (p)
            {
                case PartOfSpeech.Adjective:
                    return Color.FromHex("#CE93D8");
                case PartOfSpeech.Adverb:
                    return Color.FromHex("#9575CD");
                case PartOfSpeech.ClauseOpener:
                    return Color.FromHex("#DCE775");
                case PartOfSpeech.Conjunction:
                    return Color.FromHex("#AED581");
                case PartOfSpeech.Determiner:
                    return Color.FromHex("#7986CB");
                case PartOfSpeech.DeterminerPronoun:
                    return Color.FromHex("#F06292");
                case PartOfSpeech.ExistentialParticle:
                    return Color.FromHex("#FFF176");
                case PartOfSpeech.ForeignWord:
                    return Color.Default;
                case PartOfSpeech.Genitive:
                    return Color.FromHex("#FFD54F");
                case PartOfSpeech.InfinitiveMarker:
                    return Color.FromHex("#FFB74D");
                case PartOfSpeech.InterjectionOrDiscourseMarker:
                    return Color.FromHex("#FF8A65");
                case PartOfSpeech.LetterAsWord:
                    return Color.Default;
                case PartOfSpeech.NegativeMarker:
                    return Color.Default;
                case PartOfSpeech.CommonNoun:
                    return Color.FromHex("#64B5F6");
                case PartOfSpeech.ProperNoun:
                    return Color.FromHex("#4FC3F7");
                case PartOfSpeech.PartOfProperNoun:
                    return Color.FromHex("#4DD0E1");
                case PartOfSpeech.CardinalNumber:
                    return Color.Default;
                case PartOfSpeech.Ordinal:
                    return Color.Default;
                case PartOfSpeech.Preposition:
                    return Color.FromHex("#E57373");
                case PartOfSpeech.Pronoun:
                    return Color.FromHex("#A1887F");
                case PartOfSpeech.Unclassified:
                    return Color.FromHex("#E0E0E0");
                case PartOfSpeech.Verb:
                    return Color.FromHex("#4DB6AC");
                case PartOfSpeech.ModalVerb:
                    return Color.FromHex("#81C784");
                default:
                    return Color.Default;
            }
        }
    }
}
