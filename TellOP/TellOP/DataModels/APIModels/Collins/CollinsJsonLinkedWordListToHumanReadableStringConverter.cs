// <copyright file="CollinsJsonLinkedWordListToHumanReadableStringConverter.cs" company="University of Murcia">
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

namespace TellOP.DataModels.ApiModels
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
    using Collins;
    using Xamarin.Forms;

    /// <summary>
    /// Converts a list of <see cref="CollinsJsonLinkedWord"/> to a human-readable string.
    /// </summary>
    public class CollinsJsonLinkedWordListToHumanReadableStringConverter : BaseConverter, IValueConverter
    {
        /// <summary>
        /// Converts a list of <see cref="CollinsJsonLinkedWord"/> to a human-readable string.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The type of the target property.</param>
        /// <param name="parameter">An optional parameter to be used in the conversion logic.</param>
        /// <param name="culture">The culture to apply during the conversion.</param>
        /// <returns>A human-readable list of strings.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            IList<CollinsJsonLinkedWord> valueList = value as IList<CollinsJsonLinkedWord>;

            if (valueList == null)
            {
                throw new ArgumentException("The value to convert must be a list of CollinsJsonLinkedWord", "value");
            }

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < valueList.Count; ++i)
            {
                if (i > 0)
                {
                    sb.Append(", ");
                }

                sb.Append(valueList[i].Content);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Converts a human-readable string to a list of <see cref="CollinsJsonLinkedWord"/>.
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
