// <copyright file="LanguageLevelClassificationExtension.cs" company="University of Murcia">
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
    using Properties;
    using Xamarin.Forms;

    /// <summary>
    /// Static extensions for the <see cref="LanguageLevelClassification"/>
    /// enumeration.
    /// </summary>
    public static class LanguageLevelClassificationExtension
    {
        /// <summary>
        /// Given a level classification, returns the corresponding color.
        /// </summary>
        /// <param name="self">A level classification.</param>
        /// <returns>The material color corresponding to the level
        /// classification.</returns>
        public static Color ToColor(this LanguageLevelClassification self)
        {
            switch (self)
            {
                case LanguageLevelClassification.A1:
                    return Color.FromHex("#FF9800");
                case LanguageLevelClassification.A2:
                    return Color.FromHex("#FFC107");
                case LanguageLevelClassification.B1:
                    return Color.FromHex("#FFEB3B");
                case LanguageLevelClassification.B2:
                    return Color.FromHex("#CDDC39");
                case LanguageLevelClassification.C1:
                    return Color.FromHex("#8BC34A");
                case LanguageLevelClassification.C2:
                    return Color.FromHex("#4CAF50");
                case LanguageLevelClassification.UNKNOWN:
                default:
                    return Color.FromHex("#9E9E9E");
            }
        }

        /// <summary>
        /// Given a language level classification, returns the corresponding
        /// parameter suitable for use in an HTTP request.
        /// </summary>
        /// <param name="level">A level classification.</param>
        /// <returns>The parameter corresponding to the given
        /// classification.</returns>
        public static string GetHTTPParam(LanguageLevelClassification level)
        {
            switch (level)
            {
                case LanguageLevelClassification.A1:
                    return "A1";
                case LanguageLevelClassification.A2:
                    return "A2";
                case LanguageLevelClassification.B1:
                    return "B1";
                case LanguageLevelClassification.B2:
                    return "B2";
                case LanguageLevelClassification.C1:
                    return "C1";
                case LanguageLevelClassification.C2:
                    return "C2";
                case LanguageLevelClassification.UNKNOWN:
                default:
                    return "unknown";
            }
        }

        /// <summary>
        /// Given a language level classification, returns the corresponding human-readable long description.
        /// </summary>
        /// <param name="level">A level classification.</param>
        /// <returns>The human-readable long description corresponding to the given classification.</returns>
        public static string LevelToLongDescription(LanguageLevelClassification level)
        {
            switch (level)
            {
                case LanguageLevelClassification.A1:
                    return Resources.LanguageLevel_A1_Long;
                case LanguageLevelClassification.A2:
                    return Resources.LanguageLevel_A2_Long;
                case LanguageLevelClassification.B1:
                    return Resources.LanguageLevel_B1_Long;
                case LanguageLevelClassification.B2:
                    return Resources.LanguageLevel_B2_Long;
                case LanguageLevelClassification.C1:
                    return Resources.LanguageLevel_C1_Long;
                case LanguageLevelClassification.C2:
                    return Resources.LanguageLevel_C2_Long;
                case LanguageLevelClassification.UNKNOWN:
                default:
                    return Resources.LanguageLevel_Unknown_Long;
            }
        }

        /// <summary>
        /// Given a language level classification, returns the corresponding human-readable short title.
        /// </summary>
        /// <param name="level">A level classification.</param>
        /// <returns>The human-readable long description corresponding to the given classification.</returns>
        public static string LevelToShortTitle(LanguageLevelClassification level)
        {
            switch (level)
            {
                case LanguageLevelClassification.A1:
                    return Resources.LanguageLevel_A1_Short;
                case LanguageLevelClassification.A2:
                    return Resources.LanguageLevel_A2_Short;
                case LanguageLevelClassification.B1:
                    return Resources.LanguageLevel_B1_Short;
                case LanguageLevelClassification.B2:
                    return Resources.LanguageLevel_B2_Short;
                case LanguageLevelClassification.C1:
                    return Resources.LanguageLevel_C1_Short;
                case LanguageLevelClassification.C2:
                    return Resources.LanguageLevel_C2_Short;
                case LanguageLevelClassification.UNKNOWN:
                default:
                    return Resources.LanguageLevel_Unknown_Short;
            }
        }
    }
}
