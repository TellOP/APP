// <copyright file="CollinsWordSearchException.cs" company="University of Murcia">
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

namespace TellOP.DataModels
{
    using System;

    /// <summary>
    /// Exception thrown in case a word is searched for in the Collins English
    /// dictionary and is not found (or an error calling the remote API occurs).
    /// </summary>
    public class CollinsWordSearchException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="CollinsWordSearchException"/> class.
        /// </summary>
        public CollinsWordSearchException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="CollinsWordSearchException"/> class with the specified
        /// error message.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public CollinsWordSearchException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="CollinsWordSearchException"/> class with the specified
        /// error message and inner exception.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        public CollinsWordSearchException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
