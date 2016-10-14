// <copyright file="ExerciseStatusExtensions.cs" company="University of Murcia">
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

namespace TellOP.DataModels.Activity
{
    using Xamarin.Forms;

    /// <summary>
    /// This class provides several representation and conversion methods for
    /// the <see cref="ExerciseStatus"/> enumeration.
    /// </summary>
    public static class ExerciseStatusExtensions
    {
        /// <summary>
        /// Gets the color code corresponding to an exercise status.
        /// </summary>
        /// <param name="status">A member of <see cref="ExerciseStatus"/>
        /// representing the exercise status to convert.</param>
        /// <returns>A <see cref="Color"/> representing the color with which
        /// the status should be represented.</returns>
        public static Color ToColor(this ExerciseStatus status)
        {
            switch (status)
            {
                case ExerciseStatus.Satisfactory:
                    return Color.FromHex("#81C784");
                case ExerciseStatus.Unsatisfactory:
                    return Color.Black;
                case ExerciseStatus.NotCompleted:
                default:
                    return Color.FromHex("#E57373");
            }
        }

        // TODO: is this needed?
        /*
        /// <summary>
        /// Convert back from color code to enum
        /// </summary>
        /// <param name="c">Color object</param>
        /// <returns>Corresponding Enum, default is <see cref="ExerciseStatus.NO"/></returns>
        public static ExerciseStatus FromColor(Color c)
        {
            if (c.Equals(new Color(0.0, 0.48, 0)))
            {
                return ExerciseStatus.OK;
            }

            if (c.Equals(Color.Black))
            {
                return ExerciseStatus.FAILED;
            }

            return ExerciseStatus.NO;
        }
        */
    }
}
