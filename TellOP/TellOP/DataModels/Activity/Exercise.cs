// <copyright file="Exercise.cs" company="University of Murcia">
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

namespace TellOP.DataModels.Activity
{
    using Enums;
    using Xamarin.Forms;

    /// <summary>
    /// An exercise.
    /// </summary>
    public abstract class Exercise
    {
        /// <summary>
        /// Gets or sets the unique identifier of the exercise.
        /// </summary>
        public int Uid { get; set; }

        /// <summary>
        /// Gets or sets the CEFR level of the exercise.
        /// </summary>
        public LanguageLevelClassification Level { get; set; }

        /// <summary>
        /// Gets the color associated to the CEFR level of the exercise.
        /// </summary>
        public Color LevelColor
        {
            get
            {
                return this.Level.ToColor();
            }
        }

        /// <summary>
        /// Gets or sets the language of the exercise.
        /// </summary>
        public SupportedLanguage Language { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the exercise is featured.
        /// </summary>
        public bool Featured { get; set; }

        /// <summary>
        /// Gets a human-readable name for the exercise type.
        /// </summary>
        /// <returns>The human-readable name for the exercise type.</returns>
        public string ToNiceString()
        {
            return Properties.Resources.Exercise_GenericName;
        }
    }
}
