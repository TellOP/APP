// <copyright file="JsonCreationConverter.cs" company="University of Murcia">
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

namespace TellOP.DataModels.APIModels
{
    using System;
    using System.Reflection;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// A <see cref="Newtonsoft.Json"/> converter that determines the proper inherited subclass to use for object
    /// deserialization.
    /// </summary>
    /// <typeparam name="T">The superclass type for the converter.</typeparam>
    public abstract class JsonCreationConverter<T> : JsonConverter
    {
        /// <summary>
        /// Determines if <paramref name="objectType" /> is a subclass of <typeparamref name="T"/>.
        /// </summary>
        /// <param name="objectType">The object type.</param>
        /// <returns><c>true</c> if <paramref name="objectType"/> is a subclass of <typeparamref name="T"/>,
        /// <c>false</c> otherwise.</returns>
        public override bool CanConvert(Type objectType)
        {
            return typeof(T).GetTypeInfo().IsAssignableFrom(objectType.GetTypeInfo());
        }

        /// <summary>
        /// Reads the JSON representation of an object and returns it.
        /// </summary>
        /// <param name="reader">An instance of <see cref="JsonReader"/> containing the JSON reader in use.</param>
        /// <param name="objectType">The object type.</param>
        /// <param name="existingValue">The existing value of the object.</param>
        /// <param name="serializer">An instance of <see cref="JsonSerializer"/> containing the JSON serializer in
        /// use.</param>
        /// <returns>A subclass of <typeparamref name="T"/> containing the object corresponding to the passed JSON
        /// representation.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (serializer == null)
            {
                throw new ArgumentNullException("serializer");
            }

            JObject jsonObject = JObject.Load(reader);
            var target = this.Create(objectType, jsonObject);
            using (JsonReader jObjectReader = JsonCreationConverterExtensions.CopyReaderForObject(reader, jsonObject))
            {
                serializer.Populate(jObjectReader, target);
            }

            return target;
        }

        /// <summary>
        /// Writes the JSON representation of an object.
        /// </summary>
        /// <param name="writer">An instance of <see cref="JsonWriter"/> containing the JSON writer in use.</param>
        /// <param name="value">The value to write.</param>
        /// <param name="serializer">An instance of <see cref="JsonSerializer"/> containing the JSON serializer in
        /// use.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // TODO: check if this works correctly
            if (serializer == null)
            {
                throw new ArgumentNullException("serializer");
            }

            serializer.Serialize(writer, value);
        }

        /// <summary>
        /// Creates a new <see cref="JsonConverter"/> that converts <paramref name="jsonObject"/> to an appropriate
        /// subclass of <typeparamref name="T"/>.
        /// </summary>
        /// <param name="objectType">The object type.</param>
        /// <param name="jsonObject">The JSON object.</param>
        /// <returns>A new instance of the subclass of <typeparamref name="T"/> that is most appropriate for
        /// <paramref name="jsonObject"/>.</returns>
        protected abstract T Create(Type objectType, JObject jsonObject);
    }
}
