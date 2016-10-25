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

using TellOP.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(CertificateVerifier))]

namespace TellOP.Droid
{
    using System.Net;
    using System.Net.Security;

    /// <summary>
    /// A certificate verifier for the Android platform.
    /// </summary>
    public class CertificateVerifier : ICertificateVerifier
    {
        /// <summary>
        /// Sets the certificate verifier to use in the TLS certificate checking process.
        /// </summary>
        public void SetCertificateVerifier()
        {
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback((sender, certificate, chain, policyErrors) =>
                {
                    if (policyErrors == SslPolicyErrors.None)
                    {
                        return true;
                    }

                    if (certificate == null)
                    {
                        return false;
                    }

#if DEBUG
                    // Pin the debug certificate used by the testing Web server
                    if (certificate.GetPublicKeyString().Equals("3082020A0282"
                        + "020100ABCCECDD6F64D4C95EC6CEB79381F966579A77D22136"
                        + "36114476E6F7BEFDB0C4957D640E212A7E35CC834531591AEE"
                        + "77A93ECB408F5D03B8C4511EAE514AE984FF0F691FD6EC7B4D"
                        + "BAF2195877CFBA93DD35291E852092218259138CB9610F85F3"
                        + "AF2A798FD827F53EB6A5937891DD15670FCC71ABA03D48DB9C"
                        + "A042C73A74D80019BEFBE3C19D8B133640FE4DAEFA4BC194B9"
                        + "D858FB3D35E26DE167CB23001E124D4E225749E631A2AEB208"
                        + "DE9EB9968DC5516D4F60FD4F94E49AF623BF233DDA497F49D0"
                        + "1D610939318A283683116EB836A53329197CCBA4107B2ECD15"
                        + "BC6A09E09C020DC3022158F3558736DB206E11568F754833A2"
                        + "D86AF9F84DB8F0EE44D45393E8C64A33C3A33AC25EF71F749C"
                        + "CE6727F4E26E7CFA2FAC1A8A694816C44CB26D1CC4A3812868"
                        + "FC58D6E90E5357C1002E055344072AFC2EA9B34279A8E549BB"
                        + "732731C22A6D0595472D0166D0A0E53C176AC000B181725C28"
                        + "737F5930557B8139F15DFD28FC6B9A56F9A2FACDE7D97A94B8"
                        + "71EBB79AE47A13290C242B77983D2317E36726902ACE043112"
                        + "C11A952F06CC1A471B4C7416F89FC84C9CFD5F453D16DB23C9"
                        + "9AD61231CD5D1256BE1C370C4132AEC29D0B7CEA0A0F4662E6"
                        + "23DDC2574758785F86725526540793F15E39A6A95416BB8380"
                        + "0C9622568BF893A1E45516E9178D7E65EE1EBA32612CBD2B91"
                        + "6EE5F6B506E11787A2B142437E17330203010001"))
                    {
                        return true;
                    }
#endif

                    /* TODO: pin the public key certificate (or, even better, the intermediate CA) of the real Web
                     * server here */

                    return false;
                });
        }
    }
}
