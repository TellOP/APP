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

namespace TellOP.DataModels.SQLiteModels
{
    using SQLite;
    using Xamarin.Forms;

    /// <summary>
    /// Provides static method for SQLite
    /// </summary>
    public static class SQLiteManager
    {
        /// <summary>
        /// Internal SQLite localWordsDictionary
        /// </summary>
        private static SQLiteConnection localWordsDictionary;

        /// <summary>
        /// Internal SQLite localLemmasDictionary
        /// </summary>
        private static SQLiteConnection localLemmasDictionary;

        /// <summary>
        /// Gets the connection object to the LocalDictionaryConnection database
        /// </summary>
        public static SQLiteConnection LocalWordsDictionaryConnection
        {
            get
            {
                if (localWordsDictionary == null)
                {
                    localWordsDictionary = DependencyService.Get<ISQLite>().GetConnection("LocalDictionary.sqlite", SQLiteOpenFlags.ReadOnly);
                }

                return localWordsDictionary;
            }
        }

        /// <summary>
        /// Gets the connection object to the LocalDictionary database
        /// </summary>
        public static SQLiteConnection LocalLemmasDictionaryConnection
        {
            get
            {
                if (localLemmasDictionary == null)
                {
                    localLemmasDictionary = DependencyService.Get<ISQLite>().GetConnection("LocalLemmasDictionary.sqlite", SQLiteOpenFlags.ReadOnly);
                }

                return localLemmasDictionary;
            }
        }
    }
}
