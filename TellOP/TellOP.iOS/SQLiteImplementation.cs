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

[assembly: Xamarin.Forms.Dependency(typeof(TellOP.iOS.SQLiteImplementation))]

#pragma warning disable SA1300
namespace TellOP.iOS
#pragma warning restore SA1300
{
    using System;
    using System.IO;
    using System.Security;
    using Foundation;
    using SQLite;

    /// <summary>
    /// An iOS implementation of the <see cref="ISQLite"/> interface.
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
            // Copy the database to the "Local application data" folder.
            // (This is required because we can not load a SQLite database
            // directly from the .apk package; SQLite requires R/W access and
            // we would like to avoid memory backing).
            // TODO: do we need to add "Tell-OP" at the end? See
            // https://developer.xamarin.com/guides/ios/application_fundamentals/working_with_the_file_system/
            string directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library", "Application Support");
            string databasePath = Path.Combine(directoryPath, databaseName);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            if (!File.Exists(databasePath))
            {
                char[] splitChars = { '.' };
                string[] splitDBName = databaseName.Split(splitChars);
                string splitName;
                string splitExt;

                if (splitChars.GetLength(0) == 1)
                {
                    splitName = splitDBName[0];
                    splitExt = string.Empty;
                }
                else
                {
                    splitName = splitDBName[0];
                    splitExt = splitDBName[1];
                }

                File.Copy(NSBundle.MainBundle.PathForResource(splitName, splitExt), databasePath);
            }

            // Initialize the connection.
            return new SQLiteConnection(databasePath, openFlags);
        }
    }
}
