// <copyright file="SQLiteManager.cs" company="University of Murcia">
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

namespace TellOP.DataModels.SQLiteModels
{
    using System.Diagnostics.CodeAnalysis;
    using global::SQLite;
    using Xamarin.Forms;

    /// <summary>
    /// A SQLite connection manager.
    /// </summary>
    public sealed class SQLiteManager
    {
        /// <summary>
        /// The connection to the local words dictionary.
        /// </summary>
        private SQLiteAsyncConnection _localWordsDictionary;

        /// <summary>
        /// The connection to the local lemmas dictionary.
        /// </summary>
        private SQLiteAsyncConnection _localLemmasDictionary;

        /// <summary>
        /// Initializes a new instance of the <see cref="SQLiteManager"/> class.
        /// </summary>
        private SQLiteManager()
        {
        }

        /// <summary>
        /// Gets the current instance of the <see cref="SQLiteManager"/> singleton.
        /// </summary>
        public static SQLiteManager Instance
        {
            get
            {
                return SQLiteManagerNested.Instance;
            }
        }

        /// <summary>
        /// Gets the SQLite connection to the local words dictionary database.
        /// </summary>
        public SQLiteAsyncConnection LocalWordsDictionaryConnection
        {
            get
            {
                if (this._localWordsDictionary == null)
                {
                    // TODO: consider | SQLiteOpenFlags.SharedCache | SQLiteOpenFlags.FullMutex if needed
                    this._localWordsDictionary = new SQLiteAsyncConnection(DependencyService.Get<ISQLite>().GetConnectionString("LocalDictionary.sqlite"), SQLiteOpenFlags.ReadOnly, false);
                }

                return this._localWordsDictionary;
            }
        }

        /// <summary>
        /// Gets the SQLite connection to the local lemmas database.
        /// </summary>
        public SQLiteAsyncConnection LocalLemmasDictionaryConnection
        {
            get
            {
                if (this._localLemmasDictionary == null)
                {
                    // TODO: consider | SQLiteOpenFlags.SharedCache | SQLiteOpenFlags.FullMutex if needed
                    this._localLemmasDictionary = new SQLiteAsyncConnection(DependencyService.Get<ISQLite>().GetConnectionString("LocalLemmasDictionary.sqlite"), SQLiteOpenFlags.ReadOnly, false);
                }

                return this._localLemmasDictionary;
            }
        }

        /// <summary>
        /// An internal nested class used to hold the current instance of the <see cref="SQLiteManager"/> singleton.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Needed for the singleton pattern")]
        private class SQLiteManagerNested
        {
            /// <summary>
            /// Gets the current instance of the <see cref="SQLiteManager"/> singleton.
            /// </summary>
            internal static readonly SQLiteManager Instance = new SQLiteManager();

            /// <summary>
            /// Initializes static members of the <see cref="SQLiteManagerNested"/> class.
            /// </summary>
            [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = "Needed for the singleton pattern")]
            static SQLiteManagerNested()
            {
                // This constructor is empty, but should be left in place to tell the compiler not to mark the type
                // as beforefieldinit. See http://csharpindepth.com/Articles/General/Beforefieldinit.aspx
            }
        }
    }
}
