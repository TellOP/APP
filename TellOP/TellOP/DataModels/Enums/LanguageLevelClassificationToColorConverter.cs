// <copyright file="LanguageLevelClassificationToColorConverter.cs" company="University of Murcia">
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
    using Xamarin.Forms;

    /// <summary>
    /// Converts a value of <see cref="LanguageLevelClassification"/> to the corresponding color.
    /// </summary>
    public class LanguageLevelClassificationToColorConverter : DictionaryConverter<LanguageLevelClassification, Color>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageLevelClassificationToColorConverter"/> class.
        /// </summary>
        public LanguageLevelClassificationToColorConverter()
        {
            this.ConverterDictionary = new Dictionary<LanguageLevelClassification, Color>()
            {
                { LanguageLevelClassification.A1, Color.FromHex("#FF9800") },
                { LanguageLevelClassification.A2, Color.FromHex("#FFC107") },
                { LanguageLevelClassification.B1, Color.FromHex("#FFEB3B") },
                { LanguageLevelClassification.B2, Color.FromHex("#CDDC39") },
                { LanguageLevelClassification.C1, Color.FromHex("#8BC34A") },
                { LanguageLevelClassification.C2, Color.FromHex("#4CAF50") },
                { LanguageLevelClassification.Unknown, Color.FromHex("#9E9E9E") }
            };
        }
    }
}
