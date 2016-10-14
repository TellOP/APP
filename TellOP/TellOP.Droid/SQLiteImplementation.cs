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

[assembly: Xamarin.Forms.Dependency(typeof(TellOP.Droid.SQLiteImplementation))]

namespace TellOP.Droid
{
    using System;
    using System.IO;
    using SQLite;

    /// <summary>
    /// An Android implementation of the <see cref="ISQLite"/> interface.
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
            string directoryPath = Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData),
                "Tell-OP");
            string databasePath = Path.Combine(
                directoryPath,
                databaseName);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            if (!File.Exists(databasePath))
            {
                using (BinaryReader dbReader
                    = new BinaryReader(Xamarin.Forms.Forms.Context.Assets.Open(
                        databaseName)))
                {
                    using (BinaryWriter dbWriter = new BinaryWriter(
                        new FileStream(databasePath, FileMode.Create)))
                    {
                        byte[] dbBuffer = new byte[2048];
                        int len = 0;
                        while ((len
                            = dbReader.Read(dbBuffer, 0, dbBuffer.Length)) > 0)
                        {
                            dbWriter.Write(dbBuffer, 0, len);
                        }
                    }
                }
            }

            // Initialize the connection.
            return new SQLiteConnection(databasePath, openFlags);
        }
    }
}
