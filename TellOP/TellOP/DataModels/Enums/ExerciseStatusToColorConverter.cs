// <copyright file="ExerciseStatusToColorConverter.cs" company="University of Murcia">
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
    using Xamarin.Forms;

    /// <summary>
    /// Converts a value of <see cref="ExerciseStatus"/> to the corresponding color.
    /// </summary>
    public class ExerciseStatusToColorConverter : DictionaryConverter<ExerciseStatus, Color>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExerciseStatusToColorConverter"/> class.
        /// </summary>
        public ExerciseStatusToColorConverter()
        {
            this.ConverterDictionary = new Dictionary<ExerciseStatus, Color>()
            {
                { ExerciseStatus.NotCompleted, Color.FromHex("#E57373") },
                { ExerciseStatus.Satisfactory, Color.FromHex("#81C784") },
                { ExerciseStatus.Unsatisfactory, Color.Black }
            };
        }
    }
}
