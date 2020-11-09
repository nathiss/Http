#region Copyrights
// This file is a part of the Http project.
//
// Copyright (c) 2020 Kamil Rusin
// Licensed under the MIT License.
// See LICENSE.txt file in the project root for full license information.
#endregion

using System;
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
        public HttpMethod Method
        {
            get => _method;
            set => _method = value ?? throw new ArgumentNullException(nameof(Method));
        }

        /// <inheritdoc />
        public UniformResourceIdentifier Target
        {
            get => _target;
            set => _target = value ?? throw new ArgumentNullException(nameof(Target));
        }

        /// <inheritdoc />
        public HttpVersion HttpVersion
        {
            get => _httpVersion;
            set => _httpVersion = value ?? throw new ArgumentNullException(nameof(HttpVersion));
        }

        /// <summary>
        /// This field contains the HTTP method of the request line.
        /// </summary>
        private HttpMethod _method;

        /// <summary>
        /// This field contains the Target of the request line.
        /// </summary>
        private UniformResourceIdentifier _target;

        /// <summary>
        /// This field contains the HTTP version of the request line.
        /// </summary>
        private HttpVersion _httpVersion;
    }
}
