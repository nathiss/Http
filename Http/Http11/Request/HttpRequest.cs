#region Copyrights
// This file is a part of the Http project.
//
// Copyright (c) 2020 Kamil Rusin
// Licensed under the MIT License.
// See LICENSE.txt file in the project root for full license information.
#endregion

using System;
using Http.Common.Headers;
using Http.Common.MessageBody;
using Http.Common.Method;
using Http.Common.Version;
using Uri;

namespace Http.Http11.Request
{
    /// <summary>
    /// This class implements functionality to access and read a HTTP request.
    /// </summary>
    internal class HttpRequest : IRequest
    {
        internal HttpRequest(IRequestLine requestLine, IHttpHeaders httpHeaders, IMessageBody messageBody)
        {
            _requestLine = requestLine;
            _httpHeaders = httpHeaders;
            MessageBody = messageBody;
        }

        /// <inheritdoc />
        public HttpMethodType Method => _requestLine.Method.Type;

        /// <inheritdoc />
        public UniformResourceIdentifier Target => _requestLine.Target;

        /// <inheritdoc />
        public HttpVersionType HttpVersion => _requestLine.HttpVersion.Version;

        /// <inheritdoc />
        public IMessageBody MessageBody { get; }

        /// <inheritdoc />
        public string GetHeader(string fieldName)
        {
            if (fieldName is null)
            {
                throw new ArgumentNullException(nameof(fieldName));
            }

            try
            {
                return _httpHeaders[fieldName];
            }
            catch (UnknownHeaderFieldException)
            {
                // TODO: log exception as a warning
                return null;
            }
        }

        /// <summary>
        /// This field contins the request line of the request.
        /// </summary>
        private readonly IRequestLine _requestLine;

        /// <summary>
        /// This field contains headers of the request.
        /// </summary>
        private readonly IHttpHeaders _httpHeaders;
    }
}
