#region Copyrights
// This file is a part of the Http project.
//
// Copyright (c) 2020 Kamil Rusin
// Licensed under the MIT License.
// See LICENSE.txt file in the project root for full license information.
#endregion

using Http.Method;
using Http.Version;
using Uri;

namespace Http.Http11
{
    /// <summary>
    /// This interface provides the functionality to operate on the objects representing HTTP request-lines.
    /// </summary>
    /// <seealso href="https://tools.ietf.org/html/rfc7230#section-3.1.1">
    /// RFC 7230 (Section 3.1.1 - Request line)
    /// </seealso>
    public interface IRequestLine : IStartLine
    {
        /// <summary>
        /// This property represents the method of the HTTP request.
        /// </summary>
        /// <value>
        /// Check <see cref="HttpMethodType" /> enum to see all supported HTTP methods.
        /// </value>
        HttpMethod Method { get; }

        /// <summary>
        /// This property represents the request-target of the HTTP request.
        /// </summary>
        /// <value>
        /// This URI must not contains Scheme and Authority components.
        /// </value>
        UniformResourceIdentifier Target { get; }

        /// <summary>
        /// This property represents the HTTP-version of the HTTP request.
        /// </summary>
        /// <value>
        /// Check <see cref="HttpVersionRepository" /> class to see all known HTTP-versions.
        /// </value>
        HttpVersion HttpVersion { get; }
    }
}
