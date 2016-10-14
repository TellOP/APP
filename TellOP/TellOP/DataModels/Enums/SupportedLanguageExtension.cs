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
    using System;

    /// <summary>
    /// Static extensions for <see cref="SupportedLanguage"/>.
    /// </summary>
    public static class SupportedLanguageExtension
    {
        /// <summary>
        /// Given a supported language, return the corresponding LCID.
        /// </summary>
        /// <param name="language">An element of the
        /// <see cref="SupportedLanguage"/> enumeration.</param>
        /// <returns>The LCID corresponding to the given language.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if
        /// <paramref name="language"/> is not an element of
        /// <see cref="SupportedLanguage"/>.</exception>
        public static string GetLCID(SupportedLanguage language)
        {
            switch (language)
            {
                case SupportedLanguage.English:
                    return "en-GB";
                case SupportedLanguage.USEnglish:
                    return "en-US";
                case SupportedLanguage.French:
                    return "fr-FR";
                case SupportedLanguage.German:
                    return "de-DE";
                case SupportedLanguage.Italian:
                    return "it-IT";
                case SupportedLanguage.Spanish:
                    return "es-ES";
                default:
                    throw new ArgumentOutOfRangeException(
                        "language",
                        "The supported language is not in the SupportedLanguage enum");
            }
        }
    }
}
