#region Copyrights
// This file is a part of the Http project.
//
// Copyright (c) 2020 Kamil Rusin
// Licensed under the MIT License.
// See LICENSE.txt file in the project root for full license information.
#endregion

using Http.Common.Method;
using Http.Common.Version;
using Uri;

namespace Http.Http11
{
    /// <summary>
    /// This class represents the request-line of HTTP request.
    /// </summary>
    public class RequestLine : IRequestLine
    {
        /// <inheritdoc />
        public HttpMethod Method { get; set; }

        /// <inheritdoc />
        public UniformResourceIdentifier Target { get; set; }

        /// <inheritdoc />
        public HttpVersion HttpVersion { get; set; }
    }
}
