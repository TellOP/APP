// <copyright file="DictionaryConverter.cs" company="University of Murcia">
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
    using System.Linq;
    using Xamarin.Forms;

    /// <summary>
    /// A class used to convert values using a dictionary.
    /// </summary>
    /// <typeparam name="TKey">The data type of the source value.</typeparam>
    /// <typeparam name="TValue">The data type of the target value.</typeparam>
    public class DictionaryConverter<TKey, TValue> : BaseConverter, IValueConverter
    {
        /// <summary>
        /// Gets or sets the dictionary to be used for data conversion.
        /// </summary>
        public Dictionary<TKey, TValue> ConverterDictionary { get; set; }

        /// <summary>
        /// Converts a value of type <typeparamref name="TKey"/> to the corresponding value of type
        /// <typeparamref name="TValue"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The type of the target property.</param>
        /// <param name="parameter">An optional parameter to be used in the conversion logic.</param>
        /// <param name="culture">The culture to apply during the conversion.</param>
        /// <returns>The target value corresponding to the given source value.</returns>
        /// <exception cref="ArgumentException"><paramref name="value"/> is not of type <typeparamref name="TKey"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException"><see cref="ConverterDictionary"/> was not set before calling
        /// this method, so the converter is unable to determine the converted value corresponding to
        /// <paramref name="value"/>.</exception>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is TKey))
            {
                throw new ArgumentException("The value has an incorrect type", "value");
            }

            if (this.ConverterDictionary == null)
            {
                throw new InvalidOperationException("The converter dictionary is not initialized");
            }

            return this.ConverterDictionary.FirstOrDefault(x => x.Key.Equals(value)).Value;
        }

        /// <summary>
        /// Converts a value of type <typeparamref name="TValue"/> to the corresponding value of type
        /// <typeparamref name="TKey"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The type of the target property.</param>
        /// <param name="parameter">An optional parameter to be used in the conversion logic.</param>
        /// <param name="culture">The culture to apply during the conversion.</param>
        /// <returns>The target value corresponding to the given source value.</returns>
        /// <exception cref="ArgumentException"><paramref name="value"/> is not of type <typeparamref name="TValue"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException"><see cref="ConverterDictionary"/> was not set before calling
        /// this method, so the converter is unable to determine the converted value corresponding to
        /// <paramref name="value"/>.</exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is TValue))
            {
                throw new ArgumentException("The value has an incorrect type", "value");
            }

            if (this.ConverterDictionary == null)
            {
                throw new InvalidOperationException("The converter dictionary is not initialized");
            }

            return this.ConverterDictionary.FirstOrDefault(x => x.Value.Equals(value)).Key;
        }
    }
}
