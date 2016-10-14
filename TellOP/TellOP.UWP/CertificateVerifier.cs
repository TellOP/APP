// <copyright file="CertificateVerifier.cs" company="University of Murcia">
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

using TellOP.UWP;

[assembly: Xamarin.Forms.Dependency(typeof(CertificateVerifier))]

namespace TellOP.UWP
{
    /// <summary>
    /// A certificate verifier for the UWP platform.
    /// </summary>
    public class CertificateVerifier : TellOP.ICertificateVerifier
    {
        /// <summary>
        /// Sets the certificate verifier to use in the TLS certificate
        /// checking process.
        /// </summary>
        public void SetCertificateVerifier()
        {
            // Omitted - UWP requires the certificate to be set per connection.
        }
    }
}
