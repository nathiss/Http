#region Copyrights
// This file is a part of the Http project.
//
// Copyright (c) 2020 Kamil Rusin
// Licensed under the MIT License.
// See LICENSE.txt file in the project root for full license information.
#endregion

using Http.Common.StatusCode;
using Http.Common.Version;

namespace Http.Http11.Response
{
    /// <summary>
    /// This interface provides the functionality to operate on the objects representing HTTP status-lines.
    /// </summary>
    /// <seealso href="https://tools.ietf.org/html/rfc7230#section-3.1.2">
    /// RFC 7230 (Section 3.1.2. - Status line)
    /// </seealso>
    public interface IStatusLine : IStartLine
    {
        /// <summary>
        /// This property represents the HTTP-version of the HTTP response.
        /// </summary>
        /// <value>
        /// Check <see cref="HttpVersionRepository" /> class to see all known HTTP-versions.
        /// </value>
        HttpVersion HttpVersion { get; }

        /// <summary>
        /// This property represents the status-code of the HTTP response.
        /// </summary>
        /// <value>
        /// Check <see cref="HttpStatusCodeRepository" /> to see all supported status-codes.
        /// </value>
        HttpStatusCode StatusCode { get; }

        /// <summary>
        /// This property represents the reason-phrase of the HTTP response.
        /// </summary>
        /// <value>
        /// The value is dynamically set based on the <see cref="StatusCode" /> and it's not possible to set this value
        /// separately. If the <see cref="StatusCode" /> is unknown or null, then the value of this property is null.
        /// </value>
        /// <exception type="UnknownHttpStatusCodeException">
        /// An exception of this type is thrown if either the <see cref="StatusCode" /> is invalid or it does not have
        /// a reason-phrase.
        /// </exception>
        string ReasonPhrase { get; }
    }
}
