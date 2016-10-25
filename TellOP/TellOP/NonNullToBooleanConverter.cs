// <copyright file="NonNullToBooleanConverter.cs" company="University of Murcia">
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

namespace TellOP
{
    using System;
    using System.Globalization;
    using Xamarin.Forms;

    /// <summary>
    /// A class for converting <c>null</c> or empty values to <c>false</c> boolean values and vice versa.
    /// </summary>
    internal class NonNullToBooleanConverter : IValueConverter
    {
        /// <summary>
        /// Converts an object to a boolean value.
        /// </summary>
        /// <param name="value">The object to convert.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="culture">An instance of the <see cref="CultureInfo"/> class to use in the conversion
        /// process.</param>
        /// <returns><c>false</c> if <paramref name="value"/> is <c>null</c> or an empty string, <c>true</c>
        /// otherwise.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strValue = value as string;
            if (strValue != null)
            {
                return !string.IsNullOrEmpty((string)value);
            }
            else
            {
                return value != null;
            }
        }

        /// <summary>
        /// Converts a boolean value back to an object.
        /// </summary>
        /// <param name="value">The boolean value to convert.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="culture">An instance of the <see cref="CultureInfo"/> class to use in the conversion
        /// process.</param>
        /// <returns><c>null</c></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
