// <copyright file="CollinsJSONPartOfSpeechJSONConverter.cs" company="University of Murcia">
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

namespace TellOP.DataModels.APIModels.Collins
{
    using System;
    using Newtonsoft.Json;

    /// <summary>
    /// A converter between <see cref="PartOfSpeech"/> enum values and their
    /// JSON string representation.
    /// </summary>
    public class CollinsJSONPartOfSpeechJSONConverter : JsonConverter
    {
        // TODO: replace this with something neater, such as a 2-way dictionary
        private const string AdjectiveRepresentation = "adjective";
        private const string AdverbRepresentation = "adverb";
        private const string ConjunctionRepresentation = "conjunction";
        private const string DeterminerRepresentation = "determiner";
        private const string ExistentialParticleRepresentation = "exclamation";
        private const string CommonNounRepresentation = "commonNoun";
        private const string PrepositionRepresentation = "preposition";
        private const string PronounRepresentation = "pronoun";
        private const string UnclassifiedRepresentation = "unclassified";
        private const string VerbRepresentation = "verb";

        /// <summary>
        /// Determines whether this instance can convert the specified
        /// object type.
        /// </summary>
        /// <param name="objectType">The object type.</param>
        /// <returns><c>true</c> if <paramref name="objectType"/> is the type
        /// of <see cref="PartOfSpeech"/>, <c>false</c> otherwise.</returns>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(PartOfSpeech);
        }

        /// <summary>
        /// Converts a JSON representation, used by the Collins API endpoints,
        /// of a <see cref="PartOfSpeech"/> object to a .NET object.
        /// </summary>
        /// <param name="reader">A <see cref="JsonReader"/> object used to
        /// translate the JSON representation of the object to the object
        /// itself.</param>
        /// <param name="objectType">The object type.</param>
        /// <param name="existingValue">The existing value of the object being
        /// read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The value of the <see cref="PartOfSpeech"/> enumeration
        /// corresponding to the given JSON representation.</returns>
        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            string stringValue = ((string)reader.Value).ToLower();
            if (stringValue.Equals(AdjectiveRepresentation.ToLower()))
            {
                return PartOfSpeech.Adjective;
            }
            else if (stringValue.Equals(AdverbRepresentation.ToLower()))
            {
                return PartOfSpeech.Adverb;
            }
            else if (stringValue.Equals(ConjunctionRepresentation.ToLower()))
            {
                return PartOfSpeech.Conjunction;
            }
            else if (stringValue.Equals(DeterminerRepresentation.ToLower()))
            {
                return PartOfSpeech.Determiner;
            }
            else if (stringValue.Equals(ExistentialParticleRepresentation.ToLower()))
            {
                return PartOfSpeech.ExistentialParticle;
            }
            else if (stringValue.Equals(CommonNounRepresentation.ToLower()))
            {
                return PartOfSpeech.CommonNoun;
            }
            else if (stringValue.Equals(PrepositionRepresentation.ToLower()))
            {
                return PartOfSpeech.Preposition;
            }
            else if (stringValue.Equals(PronounRepresentation.ToLower()))
            {
                return PartOfSpeech.Pronoun;
            }
            else if (stringValue.Equals(VerbRepresentation.ToLower()))
            {
                return PartOfSpeech.Verb;
            }
            else
            {
                return PartOfSpeech.Unclassified;
            }
        }

        /// <summary>
        /// Converts a <see cref="PartOfSpeech"/> enum value to its JSON
        /// representation used by the Collins API endpoints. If
        /// <paramref name="value"/> is not a <see cref="PartOfSpeech"/>
        /// object, the conversion is not performed.
        /// </summary>
        /// <param name="writer">A <see cref="JsonWriter"/> object used to
        /// translate the object to its JSON representation.</param>
        /// <param name="value">The value to convert.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(
            JsonWriter writer,
            object value,
            JsonSerializer serializer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }

            PartOfSpeech part = (PartOfSpeech)value;
            switch (part)
            {
                case PartOfSpeech.Adjective:
                    writer.WriteValue(AdjectiveRepresentation);
                    break;
                case PartOfSpeech.Adverb:
                    writer.WriteValue(AdverbRepresentation);
                    break;
                case PartOfSpeech.CommonNoun:
                    writer.WriteValue(CommonNounRepresentation);
                    break;
                case PartOfSpeech.Conjunction:
                    writer.WriteValue(ConjunctionRepresentation);
                    break;
                case PartOfSpeech.Determiner:
                    writer.WriteValue(DeterminerRepresentation);
                    break;
                case PartOfSpeech.ExistentialParticle:
                    writer.WriteValue(ExistentialParticleRepresentation);
                    break;
                case PartOfSpeech.Preposition:
                    writer.WriteValue(PrepositionRepresentation);
                    break;
                case PartOfSpeech.Pronoun:
                    writer.WriteValue(PronounRepresentation);
                    break;
                case PartOfSpeech.Verb:
                    writer.WriteValue(VerbRepresentation);
                    break;
                case PartOfSpeech.Unclassified:
                default:
                    writer.WriteValue(UnclassifiedRepresentation);
                    break;
            }
        }
    }
}
