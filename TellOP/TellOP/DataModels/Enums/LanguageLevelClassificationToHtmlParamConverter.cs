// <copyright file="LanguageLevelClassificationToHtmlParamConverter.cs" company="University of Murcia">
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
    /// Converts a value of <see cref="LanguageLevelClassification"/> to the corresponding HTML parameter.
    /// </summary>
    public class LanguageLevelClassificationToHtmlParamConverter : DictionaryConverter<LanguageLevelClassification, string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageLevelClassificationToHtmlParamConverter"/> class.
        /// </summary>
        public LanguageLevelClassificationToHtmlParamConverter()
        {
            this.ConverterDictionary = new Dictionary<LanguageLevelClassification, string>()
            {
                { LanguageLevelClassification.A1, "A1" },
                { LanguageLevelClassification.A2, "A2" },
                { LanguageLevelClassification.B1, "B1" },
                { LanguageLevelClassification.B2, "B2" },
                { LanguageLevelClassification.C1, "C1" },
                { LanguageLevelClassification.C2, "C2" },
                { LanguageLevelClassification.Unknown, string.Empty }
            };
        }
    }
}
