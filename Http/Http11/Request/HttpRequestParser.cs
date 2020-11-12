#region Copyrights
// This file is a part of the Http project.
//
// Copyright (c) 2020 Kamil Rusin
// Licensed under the MIT License.
// See LICENSE.txt file in the project root for full license information.
#endregion

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Http.Common.Method;
using Http.Common.Version;
using Uri;

namespace Http.Http11.Request
{
    /// <summary>
    /// This class implements the functionality to parse HTTP requests into objects of type <see cref="IRequest" />.
    /// </summary>
    public class HttpRequestParser : IRequestParser
    {
        /// <summary>
        /// This constant defines the hard limit of the length of request line in bytes.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc7230#section-3.1.1">RFC 7230 (Section 3.1.1)</seealso>
        public const int MaxRequestLineLength = 8000;

        /// <summary>
        /// This contant defines the hard limit of the length of a single header-field in bytes. The specification does
        /// not define any limit of the length of a header-field, so this library is using the same value as for
        /// <see cref="MaxRequestLineLength" />.
        /// </summary>
        public const int MaxHeaderFieldLength = 8000;

        /// <summary>
        /// This contant defines the hard limit of the amount of header-fields that can be present in a single request.
        /// The specification does not define any limit of the length of a header-field, so treat this value as magic
        /// number.
        /// </summary>
        public const int MaxHeaderFieldsCount = 500;

        /// <summary>
        /// This constant defines the hard limit of the length of the message body in bytes. Since the standard does not
        /// predefine any limit, this value is a magic number.
        /// </summary>
        /// <seealso href="https://tools.ietf.org/html/rfc7230#section-3.3.2">RFC 7230 (Section 3.3.2)</seealso>
        public const int MaxMessageBodyLength = 4 * 1024 * 1024;

        /// <summary>
        /// This contant defines the hard limit of the length of a single chunk in bytes. Since the standard does not
        /// predefine any limit, this value is a magic number.
        /// </summary>
        public const int MaxChunkLength = 4 * 1024 * 1024;

        /// <summary>
        /// This constructor uses <see cref="Clear" /> method to set up all fields & properties.
        /// </summary>
        public HttpRequestParser(RequestBuilder requestBuilder)
        {
            _requestBuilder = requestBuilder;
            Clear();
        }

        /// <inheritdoc />
        public ParserStatus Status
        {
            get
            {
                if (_requestLineStatus != ParserStatus.Ready)
                {
                    return _requestLineStatus;
                }

                if (_headersStatus != ParserStatus.Ready)
                {
                    return _headersStatus;
                }

                return _messageBodyStatus;
            }
        }

        /// <inheritdoc />
        public string Error
        {
            get => _error ?? throw new InvalidOperationException("No error has been raised in the parser.");
            protected set => _error = value;
        }

        /// <summary>
        /// This field contains the error message used by <see cref="Error" />.
        /// </summary>
        private string _error;

        /// <inheritdoc />
        public void FeedData(IList<byte> data)
        {
            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            _data.AddRange(data);
            UpdateStatus();
        }

        /// <inheritdoc />
        public void Clear()
        {
            _data.Clear();
            _requestLineBytes?.Clear();
            _headerBytes?.Clear();
            _requestBuilder.Clear();
            _requestLineStatus = ParserStatus.Empty;
            _headersStatus = ParserStatus.Empty;
            _transferEncodings?.Clear();
            _messageBodyStatus = ParserStatus.Empty;
            _messageBodyLength = -1;
            _currentChunkSize = -1;
            _lastChunkReceived = false;
            _needMoreDataForChunk = true;
        }

        /// <summary>
        /// This method analyses the data and updates the <see cref="Status" />.
        /// </summary>
        private void UpdateStatus()
        {
            if (Status == ParserStatus.Error || Status == ParserStatus.Ready)
            {
                return;
            }

            try
            {
                UpdateRequestLineStatus();
                UpdateHeadersStatus();
                UpdateMessageBodyStatus();
            }
            catch (HttpRequestParserException e)
            {
                // TODO: add logger
                Error = e.Message;
            }
        }

        /// <summary>
        /// This method analysis the data and updates <see cref="_requestLineStatus" />.
        /// </summary>
        private void UpdateRequestLineStatus()
        {
            if (_requestLineStatus == ParserStatus.Ready || _requestLineStatus == ParserStatus.Error)
            {
                return;
            }

            try
            {
                var endOfRequestLine = _data.FindIndex(b => b == 0x0A);  // "\n"
                if (endOfRequestLine == -1)
                {
                    if (_data.Count > MaxRequestLineLength)
                    {
                        throw new HttpRequestParserException("Request-line is too long.");
                    }

                    _requestLineStatus = ParserStatus.Unsatisfied;
                    return;
                }

                if (endOfRequestLine == 0)
                {
                    throw new HttpRequestParserException("Invalid character in the request-line.");
                }

                if (_data[endOfRequestLine - 1] != 0x0D)  // "\r"
                {
                    throw new HttpRequestParserException("Request-line must end with CRLF.");
                }

                _requestLineBytes = _data.GetRange(0, endOfRequestLine - 1);  // Do not include "\r"
                _data.RemoveRange(0, endOfRequestLine + 1);  // Remove request-line and "\n"

                ParseMethod();
                ParseTarget();
                ParseHttpVersion();

                _requestLineStatus = ParserStatus.Ready;
            }
            catch (HttpRequestParserException)
            {
                _requestLineStatus = ParserStatus.Error;
                throw;
            }
        }

        /// <summary>
        /// This method extracts the method from the data and adds it to <see cref="_requestBuilder">the request
        /// builder</see>.
        /// </summary>
        private void ParseMethod()
        {
            if (_requestBuilder.HasMethod)
            {
                return;
            }

            var endOfMethod = _requestLineBytes.FindIndex(b => b == 0x20);  // " "
            if (endOfMethod == -1)
            {
                throw new HttpRequestParserException("Method must end with a single SP.");
            }

            var methodBytes = _requestLineBytes.GetRange(0, endOfMethod);
            _requestLineBytes.RemoveRange(0, endOfMethod + 1);  // Remove method and the space

            var methodStr = Encoding.ASCII.GetString(methodBytes.ToArray());
            if (!methodStr.All(c => char.IsUpper(c)))
            {
                // RFC 7230 (Section 3.1.1) - "The request method is case-sensitive."
                throw new HttpRequestParserException("Unknown HTTP method.");
            }

            try
            {
                // We can ignore letter-case, we already checked this.
                var methodType = Enum.Parse<HttpMethodType>(methodStr, ignoreCase: true);
                _requestBuilder.SetMethod(methodType);
            }
            catch (ArgumentException)
            {
                throw new HttpRequestParserException("Unknown HTTP method.");
            }
        }

        /// <summary>
        /// This method extracts the target from the data and adds it to <see cref="_requestBuilder">the request
        /// builder</see>.
        /// </summary>
        private void ParseTarget()
        {
            if (_requestBuilder.HasTarget)
            {
                return;
            }

            var endOfTarget = _requestLineBytes.FindIndex(b => b == 0x20);  // " "
            if (endOfTarget == -1)
            {
                throw new HttpRequestParserException("Targe must end with a single SP.");
            }

            var targetBytes = _requestLineBytes.GetRange(0, endOfTarget);
            _requestLineBytes.RemoveRange(0, endOfTarget + 1);  // Remove target and the space

            var targetStr = Encoding.ASCII.GetString(targetBytes.ToArray());
            var target = UniformResourceIdentifier.FromString(targetStr);
            if (target == null)
            {
                throw new HttpRequestParserException("Target is ill-formed.");
            }

            _requestBuilder.SetTarget(target);
        }

        /// <summary>
        /// This method extracts the HTTP version from the data and adds it to <see cref="_requestBuilder">the request
        /// builder</see>.
        /// </summary>
        private void ParseHttpVersion()
        {
            if (_requestBuilder.HasHttpVersion)
            {
                return;
            }

            var httpVersionStr = Encoding.ASCII.GetString(_requestLineBytes.ToArray());

            try
            {
                _requestBuilder.SetHttpVersion(HttpVersionTypesMap[httpVersionStr]);
            }
            catch (KeyNotFoundException)
            {
                throw new HttpRequestParserException("Unknown HTTP version.");
            }
        }

        /// <summary>
        /// This map contains all valid translations of HTTP versions represented as strings into an enum values.
        /// </summary>
        /// <value></value>
        private static readonly IDictionary<string, HttpVersionType> HttpVersionTypesMap =
            new Dictionary<string, HttpVersionType>
            {
                { "HTTP/1.0", HttpVersionType.Http1_0 },
                { "HTTP/1.1", HttpVersionType.Http1_0 },
                { "HTTP/2", HttpVersionType.Http2_0 },
            };

        /// <summary>
        /// This method analysis the data and updates <see cref="_headersStatus" />.
        /// </summary>
        private void UpdateHeadersStatus()
        {
            if (_headersStatus == ParserStatus.Ready || _headersStatus == ParserStatus.Error)
            {
                return;
            }
            _headersStatus = ParserStatus.Unsatisfied;

            if (HandleHeaders())
            {
                _headersStatus = ParserStatus.Ready;
            }
        }

        /// <summary>
        /// This method reads <see cref="_data" /> and tries to extract HTTP header-fields.
        /// </summary>
        /// <returns>
        /// An indication of whether or not all headers has been extracted is returned.
        /// </returns>
        private bool HandleHeaders()
        {
            for (var endOfHeader = _data.FindIndex(b => b == 0x0A);
                endOfHeader != -1;
                endOfHeader = _data.FindIndex(b => b == 0x0A)
            )
            {
                try
                {
                    if (endOfHeader == 0)
                    {
                        throw new HttpRequestParserException("Invalid character in a HTTP header.");
                    }

                    if (_requestBuilder.HeadersCount > MaxHeaderFieldsCount)
                    {
                        throw new HttpRequestParserException("Maximum amount of header-fields has been reached.");
                    }

                    if (endOfHeader >= MaxHeaderFieldLength)
                    {
                        throw new HttpRequestParserException("Maximum length of a header-field has been exceeded.");
                    }

                    if (_data[endOfHeader - 1] != 0x0D)  // "\r"
                    {
                        throw new HttpRequestParserException("An HTTP header/HTTP headers section must end with CRLF.");
                    }

                    _headerBytes = _data.GetRange(0, endOfHeader - 1);  // Do not include "\r"
                    _data.RemoveRange(0, endOfHeader + 1);  // Remove request-line and "\n"

                    if (_headerBytes.Count == 0)
                    {
                        // Line which contains only CRLF indicates the end of a header section.
                        return true;
                    }

                    ParseHeader();
                }
                catch (HttpRequestParserException)
                {
                    _headersStatus = ParserStatus.Error;
                    throw;
                }
            }

            return false;
        }

        /// <summary>
        /// This method parses a header stored in <see cref="_headerBytes" /> and adds it to
        /// <see cref="_requestBuilder">the request builder</see>.
        /// </summary>
        private void ParseHeader()
        {
            if (_headerBytes.Count == 0)
            {
                return;
            }

            var endOfFieldName = _headerBytes.FindIndex(b => b == 0x3A);  // ":"

            if (endOfFieldName == -1)
            {
                throw new HttpRequestParserException(
                    "Field-name and field-value must be seperated by a single colon (\":\")."
                );
            }

            var fieldNameBytes = _headerBytes.GetRange(0, endOfFieldName);
            _headerBytes.RemoveRange(0, endOfFieldName + 1);  // Remove field-name and the colon

            var fieldName = Encoding.ASCII.GetString(fieldNameBytes.ToArray());

            // RFC 7230 (Section 3.2) -
            //"[..] optional leading whitespace, the field value, and optional trailing whitespace."
            var fieldValue = Encoding.ASCII.GetString(_headerBytes.ToArray()).Trim();

            try
            {
                if (_requestBuilder.HasHeader(fieldName))
                {
                    throw new HttpRequestParserException(
                        $"Header with field-name {fieldName} has already been defined."
                    );
                }
                _requestBuilder.SetHeader(fieldName, fieldValue);
            }
            catch (ArgumentNullException e)
            {
                throw new HttpRequestParserException(e.Message);
            }
            catch (ArgumentException e)
            {
                throw new HttpRequestParserException(e.Message);
            }
        }

        /// <summary>
        /// This method analysis the data and updates <see cref="_messageBodyStatus" />.
        /// </summary>
        private void UpdateMessageBodyStatus()
        {
            if (_messageBodyStatus == ParserStatus.Ready || _messageBodyStatus == ParserStatus.Error)
            {
                return;
            }
            _messageBodyStatus = ParserStatus.Unsatisfied;

            try
            {
                if (IsChunked || _requestBuilder.HasHeader("Transfer-Encoding"))
                {
                    if (_requestBuilder.HasHeader("Content-Length"))
                    {
                        throw new HttpRequestParserException(
                            "A request cannot contain both Content-Length and Transfer-Encoding header-fields."
                        );
                    }

                    if (_transferEncodings == null || _transferEncodings.Count == 0)
                    {
                        ParseTransferEncodings();
                    }
                    _needMoreDataForChunk = false;

                    while (_needMoreDataForChunk == false)
                    {
                        HandleChunkSize();
                        HandleChunkData();
                        HandleTrailerPart();
                    }

                    return;
                }

                if (_messageBodyLength == -1 && _requestBuilder.HasHeader("Content-Length"))
                {
                    var contentLengthStr = _requestBuilder.GetHeader("Content-Length");
                    if (int.TryParse(contentLengthStr, out int contentLengthValue))
                    {
                        if(contentLengthValue < 0)
                        {
                            throw new HttpRequestParserException("Content-Length must be a positive integer.");
                        }
                        if (contentLengthValue > MaxMessageBodyLength)
                        {
                            throw new HttpRequestParserException(
                                $"Content-Length cannot be larger than {MaxMessageBodyLength} bytes."
                            );
                        }

                        _messageBodyLength = contentLengthValue;

                        if (_messageBodyLength == 0)
                        {
                            _messageBodyStatus = ParserStatus.Ready;
                            return;
                        }
                    }
                    else
                    {
                        throw new HttpRequestParserException("Content-Length must be a valid number.");
                    }
                }

                if (_messageBodyLength >= 0)
                {
                    if (_data.Count < _messageBodyLength)
                    {
                        _messageBodyStatus = ParserStatus.Unsatisfied;
                        return;
                    }

                    _requestBuilder.SetBody(_data.GetRange(0, _messageBodyLength));
                    _data.RemoveRange(0, _messageBodyLength);
                }
            }
            catch (HttpRequestParserException)
            {
                _messageBodyStatus = ParserStatus.Error;
                throw;
            }

            _messageBodyStatus = ParserStatus.Ready;
        }

        /// <summary>
        /// This method reads "Transfer-Encoding" header-field and parses its value into
        /// <see cref="_transferEncodings">a list of strings</see>.
        /// </summary>
        private void ParseTransferEncodings()
        {
            var transferEncodings = _requestBuilder.GetHeader("Transfer-Encoding")
                .Split(',')
                .Select(encoding => encoding.Trim().ToLower())
                .ToList();

            if (transferEncodings.Count(encoding => encoding == "chunked") != 1)
            {
                throw new NotImplementedException("Exactly one transfer-encoding chunked is required.");
            }

            if (transferEncodings.Any(string.IsNullOrWhiteSpace))
            {
                throw new HttpRequestParserException("Transfer-Encoding field-value format is invalid.");
            }

            if (transferEncodings.Last() != "chunked")
            {
                throw new HttpRequestParserException(
                    "If transfer-encoding chunked is present in the request, then it must be the last encoding."
                );
            }

            _transferEncodings = transferEncodings;
        }

        /// <summary>
        /// This method reads <see cref="_data" /> and tried to extract a chunk-size.
        /// </summary>
        private void HandleChunkSize()
        {
            if (_currentChunkSize >= 0 || _lastChunkReceived)
            {
                // This means that we still need to process the current chunk first or the last chunk has been received
                // and there will be no more chunk-size.
                return;
            }

            var endOfChunkSize = _data.FindIndex(b => b == 0x0A);  // "\n"
            if (endOfChunkSize == -1)
            {
                if (_data.Count > MaxMessageBodyLength)
                {
                    throw new HttpRequestParserException($"Message body cannot be longer than {MaxMessageBodyLength}.");
                }

                // Need more data.
                _needMoreDataForChunk = true;
                return;
            }

            if (_data[endOfChunkSize - 1] != 0x0D)  // "\r"
            {
                throw new HttpRequestParserException("A chunk-size must end with a single CRLF.");
            }

            var chunkSizeBytes = _data.GetRange(0, endOfChunkSize - 1);  // Do not include "\r"
            _data.RemoveRange(0, endOfChunkSize + 1);  // Remove chunk-size and "\r\n"

            var chunkSizeStr = Encoding.ASCII.GetString(chunkSizeBytes.ToArray());
            if (int.TryParse(chunkSizeStr, NumberStyles.HexNumber, null, out int chunkSize))
            {
                if (chunkSize < 0)
                {
                    throw new HttpRequestParserException("Chunk-size must be a positive integer.");
                }

                if (chunkSize > MaxChunkLength)
                {
                    throw new HttpRequestParserException($"Chunk-size cannot be bigger than {MaxChunkLength}.");
                }

                _currentChunkSize = chunkSize;
            }
            else
            {
                throw new HttpRequestParserException("Chunk-size must be a valid number.");
            }
        }

        /// <summary>
        /// This method reads <see cref="_data" /> and tries to extract a chunk-data.
        /// </summary>
        private void HandleChunkData()
        {
            if (_currentChunkSize == -1)
            {
                // This means that we still needs to process chunk-size first.
                return;
            }

            if (_currentChunkSize == 0)
            {
                // Last chunk does not have any data, so there's no CRLF at the end of the chunk-data.
                _lastChunkReceived = true;
                _currentChunkSize = -1;
                return;
            }

            if (_data.Count < _currentChunkSize + 2)  // chunk-data and CRLF
            {
                // Need more data.
                _needMoreDataForChunk = true;
                return;
            }

            _requestBuilder.AddBodyChunk(_data.GetRange(0, _currentChunkSize));
            _data.RemoveRange(0, _currentChunkSize + 2);

            _currentChunkSize = -1;
        }

        /// <summary>
        /// This method reads <see cref="_data" /> and tries to extract trailer-part of a chunked-body.
        /// </summary>
        private void HandleTrailerPart()
        {
            if (_lastChunkReceived == false)
            {
                // We need to receive the last chunk part before trailer-part.
                return;
            }

            if (HandleHeaders())
            {
                _messageBodyStatus = ParserStatus.Ready;
            }

            _needMoreDataForChunk = true;
        }

        /// <summary>
        /// This field contains the data that will be used to create <see cref="IRequest" /> objects.
        /// </summary>
        private readonly List<byte> _data = new List<byte>();

        /// <summary>
        /// This field contains bytes of request-line.
        /// </summary>
        private List<byte> _requestLineBytes;

        /// <summary>
        /// This field contains bytes of a single HTTP header.
        /// </summary>
        private List<byte> _headerBytes;

        /// <summary>
        /// This field contains the request builder used to build HTTP requests.
        /// </summary>
        private readonly RequestBuilder _requestBuilder;

        /// <summary>
        /// This field contains the status of the request line.
        /// </summary>
        private ParserStatus _requestLineStatus;

        /// <summary>
        /// This field contains the status of the HTTP headers section.
        /// </summary>
        private ParserStatus _headersStatus;

        /// <summary>
        /// This field contains the list of all transfer-encodings that were applied to the payload to form the message
        /// body.
        /// </summary>
        private List<string> _transferEncodings;

        /// <summary>
        /// This field contains the status of the message body.
        /// </summary>
        private ParserStatus _messageBodyStatus;

        /// <summary>
        /// This field contains cached Content-Length used to determin the length of message body.
        /// </summary>
        private int _messageBodyLength;

        /// <summary>
        /// This property returns an indication of whether or not the chunked transfer-encoding has been applied to
        /// the message body.
        /// </summary>
        private bool IsChunked => _transferEncodings != null && _transferEncodings.Last() == "chunked";

        /// <summary>
        /// This field contains the size of the current chunk. If a request does not have chunked transfer-encoding
        /// applied or chunk-size has been processed for the current chunk, then the value of this field is -1.
        /// </summary>
        private int _currentChunkSize;

        /// <summary>
        /// This field contains an indication of whether or not the last-chunk has been received and now we need to
        /// expect trailer-part.
        /// </summary>
        private bool _lastChunkReceived;

        /// <summary>
        /// This field contains an indication of whether or not be can continue processing chunks.
        /// </summary>
        private bool _needMoreDataForChunk;
    }
}
