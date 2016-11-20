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
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Security.Cryptography;

    /// <summary>
    /// An Android implementation of the <see cref="ISQLite"/> interface.
    /// </summary>
    public class SQLiteImplementation : ISQLite
    {
        /// <summary>
        /// Gets a SQLite database connection given a database name. The database will be loaded from the application's
        /// assets (and copied in an accessible position if necessary).
        /// </summary>
        /// <param name="databaseName">The database file name.</param>
        /// <returns>The required connection string.</returns>
        [SuppressMessage("Microsoft.Usage", "CA2202:DoNotDisposeObjectsMultipleTimes", Justification = "The using directives take care of this")]
        public string GetConnectionString(string databaseName)
        {
            // Copy the database to the "Local application data" folder. (This is required because we can not load a
            // SQLite database directly from the .apk package; SQLite requires R/W access and we would like to avoid
            // memory backing).
            string directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Tell-OP");
            string databasePath = Path.Combine(directoryPath, databaseName);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Check if the database is not present or if a new revision is available in the app package. Note that
            // asset streams do not support seeking, so we have to open the file twice in case we need to copy it.
            bool needsCopy = false;
            if (!File.Exists(databasePath))
            {
                needsCopy = true;
            }
            else
            {
                byte[] appDbHash;
                byte[] localDbHash;

                using (Stream appDb = Xamarin.Forms.Forms.Context.Assets.Open(databaseName))
                {
                    appDbHash = this.Sha256Stream(appDb);
                    appDb.Close();
                }

                using (Stream localDb = new FileStream(databasePath, FileMode.Open))
                {
                    localDbHash = this.Sha256Stream(localDb);
                    localDb.Close();
                }

                needsCopy = !appDbHash.Equals(localDbHash);
            }

            if (needsCopy)
            {
                using (BinaryReader dbReader = new BinaryReader(Xamarin.Forms.Forms.Context.Assets.Open(databaseName)))
                {
                    using (BinaryWriter dbWriter = new BinaryWriter(new FileStream(databasePath, FileMode.OpenOrCreate)))
                    {
                        byte[] dbBuffer = new byte[2048];
                        int len = 0;
                        while ((len = dbReader.Read(dbBuffer, 0, dbBuffer.Length)) > 0)
                        {
                            dbWriter.Write(dbBuffer, 0, len);
                        }
                    }
                }
            }

            return databasePath;
        }

        /// <summary>
        /// Gets the SHA256 hash of a stream.
        /// </summary>
        /// <param name="input">The stream.</param>
        /// <returns>The SHA256 hash of the contents of <paramref name="input"/>, in array form.</returns>
        private byte[] Sha256Stream(Stream input)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                return sha256Hash.ComputeHash(input);
            }
        }
    }
}
