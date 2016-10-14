// <copyright file="SQLiteImplementation.cs" company="University of Murcia">
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

[assembly: Xamarin.Forms.Dependency(typeof(TellOP.UWP.SQLiteImplementation))]

namespace TellOP.UWP
{
    using System.IO;
    using SQLite;

    /// <summary>
    /// A UWP implementation of the <see cref="ISQLite"/> interface.
    /// </summary>
    public class SQLiteImplementation : ISQLite
    {
        /// <summary>
        /// Gets a SQLite database connection given a database name. The
        /// database will be loaded from the application's assets.
        /// </summary>
        /// <param name="databaseName">The database file name.</param>
        /// <param name="openFlags">The flags to be used while opening the
        /// database.</param>
        /// <returns>A <see cref="SQLiteConnection"/> object containing the
        /// required connection.</returns>
        public SQLiteConnection GetConnection(
            string databaseName,
            SQLiteOpenFlags openFlags)
        {
            // ApplicationData.Current.LocalFolder.Path,
            return new SQLiteConnection(
                Path.Combine(
                    Windows.ApplicationModel.Package.Current.InstalledLocation.Path,
                    databaseName),
                openFlags);
        }
    }
}
