// <copyright file="LevelClassificationDistributionConverter.cs" company="University of Murcia">
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
    using System.Collections.Generic;
    using System.Globalization;
    using Enums;
    using Xamarin.Forms;

    /// <summary>
    /// Converts a floating point number to a human-readable percentage.
    /// </summary>
    public class LevelClassificationDistributionConverter : BaseConverter, IValueConverter
    {
        /// <summary>
        /// Converts a <see cref="float"/> to a human-readable percentage.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The type of the target property.</param>
        /// <param name="parameter">An optional parameter to be used in the conversion logic.</param>
        /// <param name="culture">The culture to apply during the conversion.</param>
        /// <returns>The target value corresponding to the given source value.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }

            LanguageLevelClassification llc = (LanguageLevelClassification)parameter;

            CultureInfo tmp = (CultureInfo)culture.Clone();
            tmp.NumberFormat.PercentDecimalDigits = 0;

            return ((Dictionary<LanguageLevelClassification, float>)value)[llc].ToString("P", tmp);
        }

        /// <summary>
        /// Converts a human-readable percentage to a floating point number.
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
