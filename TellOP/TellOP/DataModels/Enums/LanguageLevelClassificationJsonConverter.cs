// <copyright file="LanguageLevelClassificationJsonConverter.cs" company="University of Murcia">
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

namespace TellOP.DataModels.Enums
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json;

    /// <summary>
    /// Converts a <see cref="LanguageLevelClassification"/> object to its JSON representation and vice versa.
    /// </summary>
    public class LanguageLevelClassificationJsonConverter : JsonConverter
    {
        private static readonly Dictionary<string, LanguageLevelClassification> _converterDictionary = new Dictionary<string, LanguageLevelClassification>()
        {
            { "A1", LanguageLevelClassification.A1 },
            { "A2", LanguageLevelClassification.A2 },
            { "B1", LanguageLevelClassification.B1 },
            { "B2", LanguageLevelClassification.B2 },
            { "C1", LanguageLevelClassification.C1 },
            { "C2", LanguageLevelClassification.C2 },
            { "UNKNOWN", LanguageLevelClassification.Unknown },
            { "U", LanguageLevelClassification.Unknown },
            { "a1", LanguageLevelClassification.A1 },
            { "a2", LanguageLevelClassification.A2 },
            { "b1", LanguageLevelClassification.B1 },
            { "b2", LanguageLevelClassification.B2 },
            { "c1", LanguageLevelClassification.C1 },
            { "c2", LanguageLevelClassification.C2 },
            { "unknown", LanguageLevelClassification.Unknown },
            { "u", LanguageLevelClassification.Unknown },
            { "0", LanguageLevelClassification.A1 },
            { "1", LanguageLevelClassification.A2 },
            { "2", LanguageLevelClassification.B1 },
            { "3", LanguageLevelClassification.B2 },
            { "4", LanguageLevelClassification.C1 },
            { "5", LanguageLevelClassification.C2 },
            { "6", LanguageLevelClassification.Unknown },
        };

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">The object type.</param>
        /// <returns><c>true</c> if <paramref name="objectType"/> is a string, <c>false</c> otherwise.</returns>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        /// <summary>
        /// Converts a JSON representation, used by the exercise API endpoints, of a <see cref="LanguageLevelClassification"/>
        /// enumeration value to a string.
        /// </summary>
        /// <param name="reader">A <see cref="JsonReader"/> object used to translate the JSON representation of the
        /// object to the object itself.</param>
        /// <param name="objectType">The object type.</param>
        /// <param name="existingValue">The existing value of the object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The value of the <see cref="LanguageLevelClassification"/> enumeration corresponding to the given JSON
        /// representation.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            string stringValue = (string)reader.Value;
            try
            {
                var ret = _converterDictionary.First(x => x.Key.Equals(stringValue)).Value;
                return ret;
            }
            catch (Exception ex)
            {
                Tools.Logger.Log("LanguageLevelClassificationConverter:85 (" + stringValue + ")", "Apparently something didn't work correctly.", ex);
                return LanguageLevelClassification.Unknown;
            }
        }

        /// <summary>
        /// Converts a <see cref="LanguageLevelClassification"/> enum value to its JSON representation used by the exercise API
        /// endpoints. If <paramref name="value"/> is not a <see cref="LanguageLevelClassification"/> object, the conversion is
        /// not performed.
        /// </summary>
        /// <param name="writer">A <see cref="JsonWriter"/> object used to translate the object to its JSON
        /// representation.</param>
        /// <param name="value">The value to convert.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }
            try
            {
                LanguageLevelClassification level = (LanguageLevelClassification)value;
                writer.WriteValue(_converterDictionary.First(x => x.Value.Equals(level)).Key);
            }
            catch (Exception ex)
            {
                Tools.Logger.Log("LanguageLevelClassificationConverter:113", "Apparently something didn't work correctly.", ex);
                writer.WriteValue(LanguageLevelClassification.Unknown);
            }
        }
    }
}
