// <copyright file="ExerciseToNameConverter.cs" company="University of Murcia">
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
    using System;
    using System.Globalization;
    using Xamarin.Forms;

    /// <summary>
    /// Converts an instance of <see cref="Exercise"/> to the corresponding exercise type.
    /// </summary>
    public class ExerciseToNameConverter : BaseConverter, IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // This null check is required (since this converter might be bound to a lazily-initialized property, the
            // first value to be converted might be null).
            if (value == null)
            {
                return null;
            }

            Type exType = value.GetType();

            if (exType.Equals(typeof(EssayExercise)))
            {
                return Properties.Resources.Exercise_EssayName;
            }

            if (exType.Equals(typeof(DictionarySearchExercise)))
            {
                return Properties.Resources.Exercise_DictionarySearchName;
            }

            // TODO: support other exercise types
            return Properties.Resources.Exercise_GenericName;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
