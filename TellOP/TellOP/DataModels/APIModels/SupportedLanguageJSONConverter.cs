// <copyright file="SupportedLanguageJSONConverter.cs" company="University of Murcia">
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

namespace TellOP.DataModels.APIModels
{
    using System;
    using Enums;
    using Newtonsoft.Json;

    /// <summary>
    /// Converts a <see cref="SupportedLanguage"/> object to its JSON
    /// representation and vice versa.
    /// </summary>
    public class SupportedLanguageJSONConverter : JsonConverter
    {
        /// <summary>
        /// Determines whether this instance can convert the specified
        /// object type.
        /// </summary>
        /// <param name="objectType">The object type.</param>
        /// <returns><c>true</c> if <paramref name="objectType"/> is a string,
        /// <c>false</c> otherwise.</returns>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        /// <summary>
        /// Converts a JSON representation, used by the exercise API endpoints,
        /// of a <see cref="SupportedLanguage"/> enumeration value to a string.
        /// </summary>
        /// <param name="reader">A <see cref="JsonReader"/> object used to
        /// translate the JSON representation of the object to the object
        /// itself.</param>
        /// <param name="objectType">The object type.</param>
        /// <param name="existingValue">The existing value of the object being
        /// read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The value of the <see cref="SupportedLanguage"/>
        /// enumeration corresponding to the given JSON representation.</returns>
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

            string stringValue = (string)reader.Value;
            if (stringValue.Equals("en-GB"))
            {
                return SupportedLanguage.English;
            }
            else if (stringValue.Equals("en-US"))
            {
                return SupportedLanguage.USEnglish;
            }
            else if (stringValue.Equals("fr-FR"))
            {
                return SupportedLanguage.French;
            }
            else if (stringValue.Equals("de-DE"))
            {
                return SupportedLanguage.German;
            }
            else if (stringValue.Equals("it-IT"))
            {
                return SupportedLanguage.Italian;
            }
            else if (stringValue.Equals("es-ES"))
            {
                return SupportedLanguage.Spanish;
            }

            return string.Empty;
        }

        /// <summary>
        /// Converts a <see cref="SupportedLanguage"/> enum value to its JSON
        /// representation used by the exercise API endpoints. If
        /// <paramref name="value"/> is not a <see cref="SupportedLanguage"/>
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

            SupportedLanguage lang = (SupportedLanguage)value;
            switch (lang)
            {
                case SupportedLanguage.English:
                    writer.WriteValue("en-GB");
                    break;
                case SupportedLanguage.French:
                    writer.WriteValue("fr-FR");
                    break;
                case SupportedLanguage.German:
                    writer.WriteValue("de-DE");
                    break;
                case SupportedLanguage.Italian:
                    writer.WriteValue("it-IT");
                    break;
                case SupportedLanguage.Spanish:
                    writer.WriteValue("es-ES");
                    break;
                case SupportedLanguage.USEnglish:
                    writer.WriteValue("en-US");
                    break;
            }
        }
    }
}
