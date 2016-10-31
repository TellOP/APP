// <copyright file="SupportedLanguageToNameConverter.cs" company="University of Murcia">
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

    /// <summary>
    /// Converts a value of <see cref="SupportedLanguage"/> to the corresponding human-readable name.
    /// </summary>
    public class SupportedLanguageToNameConverter : DictionaryConverter<SupportedLanguage, string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SupportedLanguageToNameConverter"/> class.
        /// </summary>
        public SupportedLanguageToNameConverter()
        {
            this.ConverterDictionary = new Dictionary<SupportedLanguage, string>()
            {
                { SupportedLanguage.English, Properties.Resources.Language_English },
                { SupportedLanguage.USEnglish, Properties.Resources.Language_USEnglish },
                { SupportedLanguage.French, Properties.Resources.Language_French },
                { SupportedLanguage.German, Properties.Resources.Language_German },
                { SupportedLanguage.Italian, Properties.Resources.Language_Italian },
                { SupportedLanguage.Spanish, Properties.Resources.Language_Spanish }
            };
        }
    }
}
