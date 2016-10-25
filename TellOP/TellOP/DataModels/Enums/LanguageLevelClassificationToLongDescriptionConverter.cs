// <copyright file="LanguageLevelClassificationToLongDescriptionConverter.cs" company="University of Murcia">
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
    using Properties;

    /// <summary>
    /// Converts a value of <see cref="LanguageLevelClassification"/> to its corresponding human-readable long
    /// description.
    /// </summary>
    public class LanguageLevelClassificationToLongDescriptionConverter : DictionaryMonoDirectionalConverter<LanguageLevelClassification, string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageLevelClassificationToLongDescriptionConverter"/> class.
        /// </summary>
        public LanguageLevelClassificationToLongDescriptionConverter()
        {
            this.ConverterDictionary = new Dictionary<LanguageLevelClassification, string>()
            {
                { LanguageLevelClassification.A1, Resources.LanguageLevel_A1_Long },
                { LanguageLevelClassification.A2, Resources.LanguageLevel_A2_Long },
                { LanguageLevelClassification.B1, Resources.LanguageLevel_B1_Long },
                { LanguageLevelClassification.B2, Resources.LanguageLevel_B2_Long },
                { LanguageLevelClassification.C1, Resources.LanguageLevel_C1_Long },
                { LanguageLevelClassification.C2, Resources.LanguageLevel_C2_Long },
                { LanguageLevelClassification.Unknown, Resources.LanguageLevel_Unknown_Long }
            };
        }
    }
}
