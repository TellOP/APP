// <copyright file="DictionaryMonoDirectionalConverter.cs" company="University of Murcia">
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
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    /// <summary>
    /// A class used to convert values using a dictionary (only in the source to target direction).
    /// </summary>
    /// <typeparam name="TKey">The data type of the source value.</typeparam>
    /// <typeparam name="TValue">The data type of the target value.</typeparam>
    public class DictionaryMonoDirectionalConverter<TKey, TValue> : DictionaryConverter<TKey, TValue>
    {
        /// <summary>
        /// Converts a value of type <typeparamref name="TValue"/> to the corresponding value of type
        /// <typeparamref name="TKey"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The type of the target property.</param>
        /// <param name="parameter">An optional parameter to be used in the conversion logic.</param>
        /// <param name="culture">The culture to apply during the conversion.</param>
        /// <returns>Nothing.</returns>
        /// <exception cref="NotImplementedException">Always thrown.</exception>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "value", Justification = "Parameters are unused as this is a monodirectional converter")]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "targetType", Justification = "Parameters are unused as this is a monodirectional converter")]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "parameter", Justification = "Parameters are unused as this is a monodirectional converter")]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "culture", Justification = "Parameters are unused as this is a monodirectional converter")]
        public new object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
