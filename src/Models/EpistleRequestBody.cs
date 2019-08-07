using System.Text;

using Newtonsoft.Json;
using GlitchedPolygons.Services.Cryptography.Asymmetric;
using GlitchedPolygons.GlitchedEpistle.Client.Models.DTOs;

namespace GlitchedPolygons.GlitchedEpistle.Client.Models
{
    /// <summary>
    /// Request body for Epistle Web API endpoints that require authentication.
    /// </summary>
    public class EpistleRequestBody
    {
        /// <summary>
        /// The requesting user's ID.
        /// </summary>
        [JsonProperty("userId")]
        public string UserId { get; set; }

        /// <summary>
        /// The requesting user's authentication token.
        /// </summary>
        [JsonProperty("auth")]
        public string Auth { get; set; }

        /// <summary>
        /// The request body. These are the endpoint parameters.<para> </para>
        /// Typically, this is some request DTO (like for example <see cref="ConvoCreationRequestDto"/>)
        /// that was serialized into JSON and gzipped.<para> </para>
        /// If the body is really short and/or represents only a single value, you can also assign the value directly instead of serializing + gzipping it.
        /// </summary>
        [JsonProperty("body")]
        public string Body { get; set; }

        /// <summary>
        /// The request's signature.<para> </para>
        /// The signed data should be the sum of the three fields
        /// (<see cref="UserId"/> + <see cref="Auth"/> + <see cref="Body"/>).<para> </para>
        /// Only ever sign using the user's private message RSA key that the server does not have!<para> </para>
        /// <see cref="User.PrivateKeyPem"/>.
        /// <seealso cref="User"/>
        /// </summary>
        [JsonProperty("sig")]
        public string Signature { get; set; }

        /// <summary>
        /// Implicitly converts an epistle request to <c>string</c> for easy signature verification.<para> </para>
        /// The returned value is (<see cref="UserId"/> + <see cref="Auth"/> + <see cref="Body"/>).<para> </para>
        /// For signing: sign what is returned from this conversion and assign the result to <see cref="EpistleRequestBody.Signature"/>!
        /// </summary>
        /// <param name="requestBody">The <see cref="EpistleRequestBody"/> instance to prepare for signing/verifying.</param>
        /// <returns><see cref="UserId"/> + <see cref="Auth"/> + <see cref="Body"/></returns>
        public static implicit operator string(EpistleRequestBody requestBody)
        {
            return new StringBuilder(1024).Append(requestBody.UserId).Append(requestBody.Auth).Append(requestBody.Body).ToString();
        }

        /// <summary>
        /// Fluent way to sign an <see cref="EpistleRequestBody"/>.<para> </para>
        /// Assigns the calculated signature to <see cref="Signature"/> and returns itself.
        /// </summary>
        /// <param name="crypto">The <see cref="IAsymmetricCryptographyRSA"/> implementation to use for signing.</param>
        /// <param name="privateSigningKeyPem">The private RSA signing key (PEM-formatted).</param>
        public EpistleRequestBody Sign(IAsymmetricCryptographyRSA crypto, string privateSigningKeyPem)
        {
            Signature = crypto.Sign(this, privateSigningKeyPem);
            return this;
        }

        /// <summary>
        /// Verifies the <see cref="Signature"/> using the provided <paramref name="crypto"/> instance.
        /// </summary>
        /// <param name="crypto">The <see cref="IAsymmetricCryptographyRSA"/> instance to use for signature verification.</param>
        /// <param name="publicRsaKeyPem">The public RSA key (PEM-formatted) to use for verifying the signature.</param>
        /// <returns>Whether the <see cref="Signature"/> could be verified or not.</returns>
        /// <seealso cref="IAsymmetricCryptographyRSA.Verify(string,string,string)"/>
        public bool VerifySignature(IAsymmetricCryptographyRSA crypto, string publicRsaKeyPem)
        {
            return crypto.Verify(this, Signature, publicRsaKeyPem);
        }
    }
}
