#region Copyrights
// This file is a part of the Http project.
//
// Copyright (c) 2020 Kamil Rusin
// Licensed under the MIT License.
// See LICENSE.txt file in the project root for full license information.
#endregion

namespace Http.Common.Version
{
    /// <summary>
    /// This class is used to represent HTTP-versions used inside request-lines and status-lines.
    /// </summary>
    /// <seealso href="https://tools.ietf.org/html/rfc7230#section-2.6">RFC 7230 - 2.6. Protocol Versioning</seealso>
    public class HttpVersion
    {
        /// <summary>
        /// This property is used to determin which HTTP-version is represented by this object.
        /// </summary>
        public HttpVersionType Version { get; }

        /// <summary>
        /// This constructor is used to set the <see cref="Version" /> property to the given
        /// <paramref name="version" />.
        /// </summary>
        /// <param name="version">
        /// This value represents a HTTP-version.
        /// </param>
        internal HttpVersion(HttpVersionType version)
        {
            Version = version;
        }
    }
}
