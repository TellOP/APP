// <copyright file="SupportedLanguageExtension.cs" company="University of Murcia">
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
    /// <summary>
    /// A list of languages supported by the application (in exercises, etc.).
    /// </summary>
    public static class SupportedLanguageExtension
    {
        /// <summary>
        /// Returns the LCID string.
        /// </summary>
        /// <param name="language">Supported language enum</param>
        /// <returns>LCID string</returns>
        public static string ToLCID(this SupportedLanguage language)
        {
            switch (language)
            {
                case SupportedLanguage.English: return "en-GB";
                case SupportedLanguage.USEnglish: return "en-US";
                case SupportedLanguage.French: return "fr-FR";
                case SupportedLanguage.German: return "de-DE";
                case SupportedLanguage.Italian: return "it-IT";
                case SupportedLanguage.Spanish: return "es-ES";
                default: return "en-GB";
            }
        }

        /// <summary>
        /// Returns the enum from the corresponding LCID string.
        /// </summary>
        /// <param name="languageLCID">Supported language LCID</param>
        /// <returns>Enum</returns>
        public static SupportedLanguage FromLCID(string languageLCID)
        {
            switch (languageLCID)
            {
                case "en-GB": default: return SupportedLanguage.English;
                case "en-US": return SupportedLanguage.USEnglish;
                case "fr-FR": return SupportedLanguage.French;
                case "de-DE": return SupportedLanguage.German;
                case "it-IT": return SupportedLanguage.Italian;
                case "es-ES": return SupportedLanguage.Spanish;
            }
        }
    }
}
