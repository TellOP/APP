// <copyright file="CutoffConverter.cs" company="University of Murcia">
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

namespace TellOP.DataModels
{
    using System;
    using System.Globalization;
    using Xamarin.Forms;

    /// <summary>
    /// A converter class used to determine if an integer value lies over or under a cutoff.
    /// </summary>
    public class CutoffConverter : BaseConverter, IValueConverter
    {
        /// <summary>
        /// Gets or sets the cutoff to apply.
        /// </summary>
        public int Cutoff { get; set; }

        /// <summary>
        /// Converts an <see cref="int"/> to a <see cref="bool"/> that states whether the given integer lies over or
        /// under a cutoff.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The type of the target property.</param>
        /// <param name="parameter">An optional parameter to be used in the conversion logic.</param>
        /// <param name="culture">The culture to apply during the conversion.</param>
        /// <returns>The target value corresponding to the given source value.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // This null check is required (for unknown reasons, the first
            // value to be converted is null).
            if (value == null)
            {
                return false;
            }

            bool result = (int)value > this.Cutoff;
            return result;
        }

        /// <summary>
        /// Converts a <see cref="bool"/> to an <see cref="int"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The type of the target property.</param>
        /// <param name="parameter">An optional parameter to be used in the conversion logic.</param>
        /// <param name="culture">The culture to apply during the conversion.</param>
        /// <returns>Nothing.</returns>
        /// <exception cref="NotImplementedException">Always thrown.</exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
