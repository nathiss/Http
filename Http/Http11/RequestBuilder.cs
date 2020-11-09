#region Copyrights
// This file is a part of the Http project.
//
// Copyright (c) 2020 Kamil Rusin
// Licensed under the MIT License.
// See LICENSE.txt file in the project root for full license information.
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using Http.Common.Headers;
using Http.Common.MessageBody;
using Http.Common.Method;
using Http.Common.Version;
using Uri;

namespace Http.Http11
{
    /// <summary>
    /// This class can be used to build a object of type <see cref="IRequest" />.
    /// </summary>
    public class RequestBuilder
    {
        /// <summary>
        /// Inversion of control.
        /// </summary>
        /// <param name="httpMethodRepository">
        /// This is the repository used to access all valid HTTP method objects.
        /// </param>
        /// <param name="httpVersionRepository">
        /// This is the repository used to access all valid HTTP version objects.
        /// </param>
        /// <param name="httpHeaders">
        /// This is the container used to hold all HTTP headers.
        /// </param>
        public RequestBuilder(IHttpMethodRepository httpMethodRepository, IHttpVersionRepository httpVersionRepository,
            IHttpHeaders httpHeaders)
        {
            _httpMethodRepository = httpMethodRepository;
            _httpVersionRepository = httpVersionRepository;
            _httpHeaders = httpHeaders;
        }

        /// <summary>
        /// This method sets the HTTP method of the request.
        /// </summary>
        /// <param name="httpMethodType">
        /// This value is used to create a HTTP method object.
        /// </param>
        /// <returns>
        /// This instance.
        /// </returns>
        public RequestBuilder SetMethod(HttpMethodType httpMethodType)
        {
            _httpMethod = _httpMethodRepository.GetMethod(httpMethodType);
            return this;
        }

        /// <summary>
        /// This method sets the target of the request.
        /// </summary>
        /// <param name="uri">
        /// The target of the request.
        /// </param>
        /// <returns>
        /// This instance.
        /// </returns>

        public RequestBuilder SetTarget(UniformResourceIdentifier uri)
        {
            _target = uri ?? throw new ArgumentNullException(nameof(uri));
            return this;
        }

        /// <summary>
        /// This method sets the HTTP version of the request.
        /// </summary>
        /// <param name="httpVersionType">
        /// The HTTP version of the request.
        /// </param>
        /// <returns>
        /// This instance.
        /// </returns>
        public RequestBuilder SetHttpVersion(HttpVersionType httpVersionType)
        {
            _httpVersion = _httpVersionRepository.GetHttpVersion(httpVersionType);
            return this;
        }

        /// <summary>
        /// This method sets a HTTP header to the given value.
        /// </summary>
        /// <param name="fieldName">
        /// The name of a HTTP header.
        /// </param>
        /// <param name="fieldValue">
        /// The value of a HTTP header.
        /// </param>
        /// <returns>
        /// This instance.
        /// </returns>
        public RequestBuilder SetHeader(string fieldName, string fieldValue)
        {
            if (fieldName is null)
            {
                throw new ArgumentNullException(nameof(fieldName));
            }

            _httpHeaders[fieldName] = fieldValue;
            return this;
        }

        /// <summary>
        /// This method sets the message body of the request.
        /// </summary>
        /// <param name="content">
        /// The message body represented as a sequence of bytes.
        /// </param>
        /// <returns>
        /// This instance.
        /// </returns>
        public RequestBuilder SetBody(List<byte> content)
        {
            _httpMessageBody.SetContent(content);
            return this;
        }

        /// <summary>
        /// This method sets the message body of the request.
        /// </summary>
        /// <param name="content">
        /// The message body represented as a string.
        /// </param>
        /// <param name="encoding">
        /// The encoding used to deserialize <paramref name="content" /> into a sequence of bytes.
        /// </param>
        /// <returns>
        /// This instance.
        /// </returns>
        public RequestBuilder SetBody(string content, Encoding encoding)
        {
            if (encoding is null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            _httpMessageBody.SetContent(content, encoding);
            return this;
        }

        /// <summary>
        /// This method builds and returns a new <see cref="IRequest" /> object.
        /// </summary>
        /// <returns>
        /// A new <see cref="IRequest" /> object is returned.
        /// </returns>
        public IRequest Build()
        {
            return new HttpRequest
            (
                BuildRequestLine(),
                _httpHeaders,
                _httpMessageBody
            );
        }

        /// <summary>
        /// This method builds and retruns a new <see cref="IRequestLine" /> object.
        /// </summary>
        /// <returns>
        /// A new <see cref="IRequestLine" /> object is returned.
        /// </returns>
        private IRequestLine BuildRequestLine()
        {
            return new RequestLine
            {
                Method = _httpMethod,
                Target = _target,
                HttpVersion = _httpVersion,
            };
        }

        /// <summary>
        /// This field contains a repository used to get access to all valid HTTP method objects.
        /// </summary>
        private readonly IHttpMethodRepository _httpMethodRepository;

        /// <summary>
        /// This field contains a repository used to get access to all valid HTTP version objects.
        /// </summary>
        private readonly IHttpVersionRepository _httpVersionRepository;

        /// <summary>
        /// This field contains a container for all HTTP headers of this request.
        /// </summary>
        private readonly IHttpHeaders _httpHeaders;

        /// <summary>
        /// This field contains a HTTP method object used to build a object of type <see cref="IRequest" />.
        /// </summary>
        private HttpMethod _httpMethod;

        /// <summary>
        /// This field contains a URI used to build a object of type <see cref="IRequest" />.
        /// </summary>
        private UniformResourceIdentifier _target;

        /// <summary>
        /// This field contains a HTTP version object used to build a object of type <see cref="IRequest" />.
        /// </summary>
        private HttpVersion _httpVersion;

        /// <summary>
        /// This field contains the message body of the request.
        /// </summary>
        private readonly HttpMessageBody _httpMessageBody = new HttpMessageBody();
    }
}
