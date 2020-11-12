#region Copyrights
// This file is a part of the Http.Tests project.
//
// Copyright (c) 2020 Kamil Rusin
// Licensed under the MIT License.
// See LICENSE.txt file in the project root for full license information.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Http.Common.Headers;
using Http.Common.Method;
using Http.Common.Version;
using Http.Http11.Request;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Http.Tests.Http11.Request
{
    [TestClass]
    public class HttpRequestParserTests
    {
        private static readonly IHttpMethodRepository _httpMethodRepository = new HttpMethodRepository();

        private static readonly IHttpVersionRepository _httpVersionRepository = new HttpVersionRepository();

        private RequestBuilder _requestBuilder;

        private IRequestParser _requestParser;

        [TestInitialize]
        public void SetUp()
        {
            _requestBuilder = new RequestBuilder(_httpMethodRepository, _httpVersionRepository, new HttpHeaders());
            _requestParser = new HttpRequestParser(_requestBuilder);
        }

        [TestMethod]
        public void Status_NoDataGiven_ReturnsEmpty()
        {
            Assert.AreEqual(ParserStatus.Empty, _requestParser.Status);
        }

        [TestMethod]
        public void Error_NoDataGiven_ThrowsInvalidOperationException()
        {
            Assert.ThrowsException<InvalidOperationException>(() => _requestParser.Error);
        }

        [TestMethod]
        public void Status_SomeDataGiven_ReturnUnsatisfied()
        {
            // Arrange
            var data = new List<byte> { 0x47, 0x45, 0x54 };  // "GET"

            // Act
            _requestParser.FeedData(data);

            // Assert
            Assert.AreEqual(ParserStatus.Unsatisfied, _requestParser.Status);
        }

        [TestMethod]
        public void Clear_GivenSomeDataAndThenCleared_StatusIsEmpty()
        {
            // Arrange
            var data = new List<byte> { 0x47, 0x45, 0x54 };  // "GET"
            _requestParser.FeedData(data);

            // Act
            _requestParser.Clear();

            // Assert
            Assert.AreEqual(ParserStatus.Empty, _requestParser.Status);
        }

        [TestMethod]
        public void Status_GivenTooLongRequestLine_ReturnsError()
        {
            // Arrange
            var data = Enumerable.Repeat<byte>(0x41, 8001).ToList();

            // Act
            _requestParser.FeedData(data);

            // Assert
            Assert.AreEqual(ParserStatus.Error, _requestParser.Status);
        }

        [TestMethod]
        public void Status_GivenValidRequestLine_ReturnsUnsatisfied()
        {
            // Arrange
            const string strData = "GET /index.html HTTP/1.1\r\n";
            var data = new List<byte>(Encoding.ASCII.GetBytes(strData));

            // Act
            _requestParser.FeedData(data);

            // Assert
            Assert.AreEqual(ParserStatus.Unsatisfied, _requestParser.Status);
        }

        [TestMethod]
        public void Status_GivenLowercaseMethod_ReturnsError()
        {
            // Arrange
            const string strData = "get /index.html HTTP/1.1\r\n";
            var data = new List<byte>(Encoding.ASCII.GetBytes(strData));

            // Act
            _requestParser.FeedData(data);

            // Assert
            Assert.AreEqual(ParserStatus.Error, _requestParser.Status);
        }

        [TestMethod]
        public void Status_GivenRequestLineWithoutCRLF_ReturnsUnsatisfied()
        {
            // Arrange
            const string strData = "GET /index.html HTTP/1.1";
            var data = new List<byte>(Encoding.ASCII.GetBytes(strData));

            // Act
            _requestParser.FeedData(data);

            // Assert
            Assert.AreEqual(ParserStatus.Unsatisfied, _requestParser.Status);
        }

        [TestMethod]
        public void Status_GivenRequestLineWithoutTarget_ReturnsError()
        {
            // Arrange
            const string strData = "GET HTTP/1.1\r\n";
            var data = new List<byte>(Encoding.ASCII.GetBytes(strData));

            // Act
            _requestParser.FeedData(data);

            // Assert
            Assert.AreEqual(ParserStatus.Error, _requestParser.Status);
        }

        [TestMethod]
        public void Status_NoMethod_ReturnsError()
        {
            // Arrange
            const string strData = "/index.html HTTP/1.1\r\n";
            var data = new List<byte>(Encoding.ASCII.GetBytes(strData));

            // Act
            _requestParser.FeedData(data);

            // Assert
            Assert.AreEqual(ParserStatus.Error, _requestParser.Status);
        }

        [TestMethod]
        public void Status_NoHttpVersion_ReturnsError()
        {
            // Arrange
            const string strData = "OPTIONS /index.html\r\n";
            var data = new List<byte>(Encoding.ASCII.GetBytes(strData));

            // Act
            _requestParser.FeedData(data);

            // Assert
            Assert.AreEqual(ParserStatus.Error, _requestParser.Status);
        }

        [TestMethod]
        public void Status_TooLongMethod_ReturnsError()
        {
            // Arrange
            const string strData = "12345678 /index.html HTTP/1.1\r\n";
            var data = new List<byte>(Encoding.ASCII.GetBytes(strData));

            // Act
            _requestParser.FeedData(data);

            // Assert
            Assert.AreEqual(ParserStatus.Error, _requestParser.Status);
        }

        [TestMethod]
        public void Status_UnknownMethod_ReturnsError()
        {
            // Arrange
            const string strData = "ERASE /index.html HTTP/1.1\r\n";
            var data = new List<byte>(Encoding.ASCII.GetBytes(strData));

            // Act
            _requestParser.FeedData(data);

            // Assert
            Assert.AreEqual(ParserStatus.Error, _requestParser.Status);
        }

        [TestMethod]
        public void Status_NoHeadersSection_ReturnReady()
        {
            // Arrange
            const string strData = "GET /index.html HTTP/1.1\r\n\r\n";
            var data = new List<byte>(Encoding.ASCII.GetBytes(strData));

            // Act
            _requestParser.FeedData(data);

            // Assert
            Assert.AreEqual(ParserStatus.Ready, _requestParser.Status);
        }

        [TestMethod]
        public void Status_ValidHeaderSection_ReturnsReady()
        {
            // Arrange
            const string strData =
                "GET /index.html HTTP/1.1\r\n" +
                "Host:example.com\r\n" +
                "\r\n";
            var data = new List<byte>(Encoding.ASCII.GetBytes(strData));

            // Act
            _requestParser.FeedData(data);

            // Assert
            Assert.AreEqual(ParserStatus.Ready, _requestParser.Status);
        }

        [TestMethod]
        public void Status_FieldValueWithLeadingWhiteSpace_ReturnsReady()
        {
            // Arrange
            const string strData =
                "GET /index.html HTTP/1.1\r\n" +
                "Host:                       example.com\r\n" +
                "\r\n";
            var data = new List<byte>(Encoding.ASCII.GetBytes(strData));

            // Act
            _requestParser.FeedData(data);

            // Assert
            Assert.AreEqual(ParserStatus.Ready, _requestParser.Status);
            Assert.IsTrue(_requestBuilder.HasHeader("Host"));
            Assert.AreEqual("example.com", _requestBuilder.GetHeader("Host"));
        }

        [TestMethod]
        public void Status_FieldValueWithTrailingWhiteSpace_ReturnsReady()
        {
            // Arrange
            const string strData =
                "GET /index.html HTTP/1.1\r\n" +
                "Host:example.com                  \r\n" +
                "\r\n";
            var data = new List<byte>(Encoding.ASCII.GetBytes(strData));

            // Act
            _requestParser.FeedData(data);

            // Assert
            Assert.AreEqual(ParserStatus.Ready, _requestParser.Status);
            Assert.IsTrue(_requestBuilder.HasHeader("Host"));
            Assert.AreEqual("example.com", _requestBuilder.GetHeader("Host"));
        }

        [TestMethod]
        public void Status_NoRequestLine_ReturnsError()
        {
            // Arrange
            const string strData =
                "Host:example.com                  \r\n" +
                "\r\n";
            var data = new List<byte>(Encoding.ASCII.GetBytes(strData));

            // Act
            _requestParser.FeedData(data);

            // Assert
            Assert.AreEqual(ParserStatus.Error, _requestParser.Status);
        }

        [TestMethod]
        public void Status_NoColonInHeader_ReturnsError()
        {
            // Arrange
            const string strData =
                "GET /index.html HTTP/1.1\r\n" +
                "Hostexample.com                  \r\n" +
                "\r\n";
            var data = new List<byte>(Encoding.ASCII.GetBytes(strData));

            // Act
            _requestParser.FeedData(data);

            // Assert
            Assert.AreEqual(ParserStatus.Error, _requestParser.Status);
        }

        [TestMethod]
        public void Status_FieldNameIsEmpty_ReturnsError()
        {
            // Arrange
            const string strData =
                "GET /index.html HTTP/1.1\r\n" +
                ":example.com                  \r\n" +
                "\r\n";
            var data = new List<byte>(Encoding.ASCII.GetBytes(strData));

            // Act
            _requestParser.FeedData(data);

            // Assert
            Assert.AreEqual(ParserStatus.Error, _requestParser.Status);
        }

        [TestMethod]
        public void Status_FieldValueIsEmpty_ReturnsReady()
        {
            // Arrange
            const string strData =
                "GET /index.html HTTP/1.1\r\n" +
                "Host:\r\n" +
                "\r\n";
            var data = new List<byte>(Encoding.ASCII.GetBytes(strData));

            // Act
            _requestParser.FeedData(data);

            // Assert
            Assert.AreEqual(ParserStatus.Ready, _requestParser.Status);
        }

        [TestMethod]
        public void Status_FieldValueContainsOnlySpaces_ReturnsReady()
        {
            // Arrange
            const string strData =
                "GET /index.html HTTP/1.1\r\n" +
                "Host:                 \r\n" +
                "\r\n";
            var data = new List<byte>(Encoding.ASCII.GetBytes(strData));

            // Act
            _requestParser.FeedData(data);

            // Assert
            Assert.AreEqual(ParserStatus.Ready, _requestParser.Status);
        }

        [TestMethod]
        public void Status_FieldValueContainsLF_ReturnsError()
        {
            // Arrange
            const string strData =
                "GET /index.html HTTP/1.1\r\n" +
                "Host: example\n.com\r\n" +
                "\r\n";
            var data = new List<byte>(Encoding.ASCII.GetBytes(strData));

            // Act
            _requestParser.FeedData(data);

            // Assert
            Assert.AreEqual(ParserStatus.Error, _requestParser.Status);
        }

        [TestMethod]
        public void FeedData_GivenNull_ThrowsArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _requestParser.FeedData(null));
        }

        [TestMethod]
        public void FeedData_GivenContentLengthZeroAndFullMessage_ReturnsReady()
        {
            // Arrange
            const string strData =
                "GET /index.html HTTP/1.1\r\n" +
                "Host: example.com\r\n" +
                "Content-Length: 0\r\n" +
                "\r\n";
            var data = new List<byte>(Encoding.ASCII.GetBytes(strData));

            // Act
            _requestParser.FeedData(data);

            // Assert
            Assert.AreEqual(ParserStatus.Ready, _requestParser.Status);
        }

        [TestMethod]
        public void FeedData_GivenContentLengthNotZeroAndFullMessage_ReturnsReady()
        {
            // Arrange
            const string strData =
                "GET /index.html HTTP/1.1\r\n" +
                "Host: example.com\r\n" +
                "Content-Length: 12\r\n" +
                "\r\nHello World!";
            var data = new List<byte>(Encoding.ASCII.GetBytes(strData));

            // Act
            _requestParser.FeedData(data);

            // Assert
            Assert.AreEqual(ParserStatus.Ready, _requestParser.Status);
        }

        [TestMethod]
        public void FeedData_GivenContentLengthNotZeroAndNotFullMessage_ReturnsUnsatisfied()
        {
            // Arrange
            const string strData =
                "GET /index.html HTTP/1.1\r\n" +
                "Host: example.com\r\n" +
                "Content-Length: 12\r\n" +
                "\r\nHello";
            var data = new List<byte>(Encoding.ASCII.GetBytes(strData));

            // Act
            _requestParser.FeedData(data);

            // Assert
            Assert.AreEqual(ParserStatus.Unsatisfied, _requestParser.Status);
        }

        [TestMethod]
        public void FeedData_GivenContentLengthNotZeroAndNotFullMessageAndThenTheRemainingData_ReturnsReady()
        {
            // Arrange
            const string strData =
                "GET /index.html HTTP/1.1\r\n" +
                "Host: example.com\r\n" +
                "Content-Length: 12\r\n" +
                "\r\nHello";
            var data = new List<byte>(Encoding.ASCII.GetBytes(strData));
            _requestParser.FeedData(data);

            const string strRestOfData = " World!";
            var restOfData = new List<byte>(Encoding.ASCII.GetBytes(strRestOfData));

            // Act
            _requestParser.FeedData(restOfData);

            // Assert
            Assert.AreEqual(ParserStatus.Ready, _requestParser.Status);
        }

        [TestMethod]
        public void FeedData_GivenContentLengthNotZeroAndNotFullMessageAndSomeData_ReturnsReady()
        {
            // Arrange
            const string strData =
                "GET /index.html HTTP/1.1\r\n" +
                "Host: example.com\r\n" +
                "Content-Length: 12\r\n" +
                "\r\nHello World!AAAAAAAAAAAAA";
            var data = new List<byte>(Encoding.ASCII.GetBytes(strData));

            // Act
            _requestParser.FeedData(data);

            // Assert
            Assert.AreEqual(ParserStatus.Ready, _requestParser.Status);
        }

        [TestMethod]
        public void FeedData_GivenContentLengthNotZeroAndNoData_ReturnsUnsatisfied()
        {
            // Arrange
            const string strData =
                "GET /index.html HTTP/1.1\r\n" +
                "Host: example.com\r\n" +
                "Content-Length: 12\r\n" +
                "\r\n";
            var data = new List<byte>(Encoding.ASCII.GetBytes(strData));

            // Act
            _requestParser.FeedData(data);

            // Assert
            Assert.AreEqual(ParserStatus.Unsatisfied, _requestParser.Status);
        }

        [TestMethod]
        public void FeedData_GivenContentLengthTooBig_ReturnsError()
        {
            // Arrange
            const string strData =
                "GET /index.html HTTP/1.1\r\n" +
                "Host: example.com\r\n" +
                "Content-Length: 4194305\r\n" +  // 4194304 + 1 (4MB + 1)
                "\r\n";
            var data = new List<byte>(Encoding.ASCII.GetBytes(strData));

            // Act
            _requestParser.FeedData(data);

            // Assert
            Assert.AreEqual(ParserStatus.Error, _requestParser.Status);
        }

        [TestMethod]
        public void FeedData_GivenContentLengthAtMaximum_ReturnsUnsatisfied()
        {
            // Arrange
            const string strData =
                "GET /index.html HTTP/1.1\r\n" +
                "Host: example.com\r\n" +
                "Content-Length: 4194304\r\n" +  // 4194304 (4MB)
                "\r\n";
            var data = new List<byte>(Encoding.ASCII.GetBytes(strData));

            // Act
            _requestParser.FeedData(data);

            // Assert
            Assert.AreEqual(ParserStatus.Unsatisfied, _requestParser.Status);
        }

        [TestMethod]
        public void FeedData_GivenContentLengthIsLessThanZero_ReturnsError()
        {
            // Arrange
            const string strData =
                "GET /index.html HTTP/1.1\r\n" +
                "Host: example.com\r\n" +
                "Content-Length: -1337\r\n" +
                "\r\n";
            var data = new List<byte>(Encoding.ASCII.GetBytes(strData));

            // Act
            _requestParser.FeedData(data);

            // Assert
            Assert.AreEqual(ParserStatus.Error, _requestParser.Status);
        }

        [TestMethod]
        public void FeedData_GivenTheSameHeaderTwice_ReturnsError()
        {
            // Arrange
            const string strData =
                "GET /index.html HTTP/1.1\r\n" +
                "Host: example.com\r\n" +
                "Host: www.example.com\r\n" +
                "Content-Length: 12\r\n" +
                "\r\nHello World!";
            var data = new List<byte>(Encoding.ASCII.GetBytes(strData));

            // Act
            _requestParser.FeedData(data);

            // Assert
            Assert.AreEqual(ParserStatus.Error, _requestParser.Status);
        }

        [TestMethod]
        public void FeedData_GivenHeaderFieldWithMaximumLength_ReturnsReady()
        {
            // Arrange
            const string strBeninningOfData =
                "GET /index.html HTTP/1.1\r\n" +
                "Host: ";
            var data = new List<byte>(Encoding.ASCII.GetBytes(strBeninningOfData));
            _requestParser.FeedData(data);

            data = Enumerable.Repeat<byte>(0x41, 8000 - "Host: ".Length - "\r\n".Length).ToList();
            data.AddRange(new List<byte> { 0x0D, 0x0A, 0x0D, 0x0A });

            // Act
            _requestParser.FeedData(data);

            // Assert
            Assert.AreEqual(ParserStatus.Ready, _requestParser.Status);
        }

        [TestMethod]
        public void FeedData_GivenHeaderFieldTooLong_ReturnsError()
        {
            // Arrange
            const string strBeninningOfData =
                "GET /index.html HTTP/1.1\r\n" +
                "Host: ";
            var data = new List<byte>(Encoding.ASCII.GetBytes(strBeninningOfData));
            _requestParser.FeedData(data);

            data = Enumerable.Repeat<byte>(0x41, 8001 - "Host: ".Length - "\r\n".Length).ToList();
            data.AddRange(new List<byte> { 0x0D, 0x0A, 0x0D, 0x0A });

            // Act
            _requestParser.FeedData(data);

            // Assert
            Assert.AreEqual(ParserStatus.Error, _requestParser.Status);
        }

        [TestMethod]
        public void FeedData_GivenMaximumAmountOfHeaderFields_ReturnsReady()
        {
            // Arrange
            const string strRequestLine = "GET / HTTP/1.1\r\n";
            var data = new List<byte>(Encoding.ASCII.GetBytes(strRequestLine));

            for (var i = 0; i < 500; i++)
            {
                data.AddRange(Encoding.ASCII.GetBytes($"Name-{i}: Value-{i}\r\n"));
            }

            data.Add(0x0D);
            data.Add(0x0A);

            // Act
            _requestParser.FeedData(data);

            // Assert
            Assert.AreEqual(ParserStatus.Ready, _requestParser.Status);
        }

        [TestMethod]
        public void FeedData_GivenTooManyHeaderFields_ReturnsError()
        {
            // Arrange
            const string strRequestLine = "GET / HTTP/1.1\r\n";
            var data = new List<byte>(Encoding.ASCII.GetBytes(strRequestLine));

            for (var i = 0; i < 501; i++)
            {
                data.AddRange(Encoding.ASCII.GetBytes($"Name-{i}: Value-{i}\r\n"));
            }

            data.Add(0x0D);
            data.Add(0x0A);

            // Act
            _requestParser.FeedData(data);

            // Assert
            Assert.AreEqual(ParserStatus.Error, _requestParser.Status);
        }

        [TestMethod]
        public void Status_GivenContentLengthAndTransferEncoding_ReturnsError()
        {
            // Arrange
            const string strRequestLine = "GET / HTTP/1.1\r\n" +
                "Host: example.com\r\n" +
                "Content-Length: 12\r\n" +
                "Transfer-Encoding: chunked\r\n" +
                "\r\n";
            var data = new List<byte>(Encoding.ASCII.GetBytes(strRequestLine));

            // Act
            _requestParser.FeedData(data);

            // Assert
            Assert.AreEqual(ParserStatus.Error, _requestParser.Status);
        }

        [TestMethod]
        public void Status_GivenChunkedAsNotLastTransferEncoding_ReturnsError()
        {
            // Arrange
            const string strRequestLine = "GET / HTTP/1.1\r\n" +
                "Host: example.com\r\n" +
                "Content-Length: 12\r\n" +
                "Transfer-Encoding: chunked, gzip\r\n" +
                "\r\n";
            var data = new List<byte>(Encoding.ASCII.GetBytes(strRequestLine));

            // Act
            _requestParser.FeedData(data);

            // Assert
            Assert.AreEqual(ParserStatus.Error, _requestParser.Status);
        }

        [TestMethod]
        public void FeedData_GivenChunkedAsLastEncoding_ReturnsUnsatisfied()
        {
            // Arrange
            const string strRequestLine = "GET / HTTP/1.1\r\n" +
                "Host: example.com\r\n" +
                "Transfer-Encoding: gzip, chunked\r\n" +
                "\r\n";
            var data = new List<byte>(Encoding.ASCII.GetBytes(strRequestLine));

            // Act
            _requestParser.FeedData(data);

            // Assert
            Assert.AreEqual(ParserStatus.Unsatisfied, _requestParser.Status);
        }

        [TestMethod]
        public void FeedData_SetTransferEncodingValueToComaAndChunked_ReturnsError()
        {
            // Arrange
            const string strRequestLine = "GET / HTTP/1.1\r\n" +
                "Host: example.com\r\n" +
                "Transfer-Encoding: , chunked\r\n" +
                "\r\n";
            var data = new List<byte>(Encoding.ASCII.GetBytes(strRequestLine));

            // Act
            _requestParser.FeedData(data);

            // Assert
            Assert.AreEqual(ParserStatus.Error, _requestParser.Status);
        }

        [TestMethod]
        public void FeedData_GivenTransferEncodingValueWithDoubleComa_ReturnsError()
        {
            // Arrange
            const string strRequestLine = "GET / HTTP/1.1\r\n" +
                "Host: example.com\r\n" +
                "Transfer-Encoding: gzip,, chunked\r\n" +
                "\r\n";
            var data = new List<byte>(Encoding.ASCII.GetBytes(strRequestLine));

            // Act
            _requestParser.FeedData(data);

            // Assert
            Assert.AreEqual(ParserStatus.Error, _requestParser.Status);
        }

        [TestMethod]
        public void FeedData_GivenTransferEncodingWithMessageBodylengthZero_ReturnsReady()
        {
            // Arrange
            const string strRequestLine = "GET / HTTP/1.1\r\n" +
                "Host: example.com\r\n" +
                "Transfer-Encoding: chunked\r\n" +
                "\r\n" +
                "0\r\n" +
                "\r\n";
            var data = new List<byte>(Encoding.ASCII.GetBytes(strRequestLine));

            // Act
            _requestParser.FeedData(data);

            // Assert
            Assert.AreEqual(ParserStatus.Ready, _requestParser.Status);
        }

        [TestMethod]
        public void FeedData_GivenTransferEncodingWithMessageBodylengthMultipleZeros_ReturnsReady()
        {
            // Arrange
            const string strRequestLine = "GET / HTTP/1.1\r\n" +
                "Host: example.com\r\n" +
                "Transfer-Encoding: chunked\r\n" +
                "\r\n" +
                "000000000000000\r\n" +
                "\r\n";
            var data = new List<byte>(Encoding.ASCII.GetBytes(strRequestLine));

            // Act
            _requestParser.FeedData(data);

            // Assert
            Assert.AreEqual(ParserStatus.Ready, _requestParser.Status);
        }

        [TestMethod]
        public void FeedData_GivenTransferEncodingMessageWithMissingEndingCRLF_ReturnsUnsatisfied()
        {
            // Arrange
            const string strRequestLine = "GET / HTTP/1.1\r\n" +
                "Host: example.com\r\n" +
                "Transfer-Encoding: chunked\r\n" +
                "\r\n" +
                "0\r\n" +
                "";
            var data = new List<byte>(Encoding.ASCII.GetBytes(strRequestLine));

            // Act
            _requestParser.FeedData(data);

            // Assert
            Assert.AreEqual(ParserStatus.Unsatisfied, _requestParser.Status);
        }

        [TestMethod]
        public void FeedData_GivenTransferEncodingMessageWithMissingEndingCRLFNextFeedCRLF_ReturnsReady()
        {
            // Arrange
            const string strRequestLine = "GET / HTTP/1.1\r\n" +
                "Host: example.com\r\n" +
                "Transfer-Encoding: chunked\r\n" +
                "\r\n" +
                "0\r\n" +
                "";
            var data = new List<byte>(Encoding.ASCII.GetBytes(strRequestLine));
            _requestParser.FeedData(data);

            // Act
            _requestParser.FeedData(new List<byte> { 0x0D, 0x0A });

            // Assert
            Assert.AreEqual(ParserStatus.Ready, _requestParser.Status);
        }

        [TestMethod]
        public void FeedData_FirstFeedCompleteHeadersSecondFeedCompleteBody_ReturnsReady()
        {
            // Arrange
            const string strHeaders = "GET / HTTP/1.1\r\n" +
                "Host: example.com\r\n" +
                "Transfer-Encoding: chunked\r\n" +
                "\r\n";
            var data = new List<byte>(Encoding.ASCII.GetBytes(strHeaders));
            _requestParser.FeedData(data);

            const string strMessageBody = "C\r\n" +
                "Hello World!\r\n" +
                "0\r\n" +
                "\r\n";
            data = new List<byte>(Encoding.ASCII.GetBytes(strMessageBody));

            // Act
            _requestParser.FeedData(data);

            // Assert
            Assert.AreEqual(ParserStatus.Ready, _requestParser.Status);
        }

        [TestMethod]
        public void FeedData_ThreeChunksAtTheSameTime_ReturnsReady()
        {
            // Arrange
            const string strData = "GET / HTTP/1.1\r\n" +
                "Host: example.com\r\n" +
                "Transfer-Encoding: chunked\r\n" +
                "\r\n" +
                "8\r\n" +
                "Hello \r\n\r\n" +
                "6\r\n" +
                "World!\r\n" +
                "0\r\n" +
                "\r\n";
            var data = new List<byte>(Encoding.ASCII.GetBytes(strData));

            // Act
            _requestParser.FeedData(data);

            // Assert
            Assert.AreEqual(ParserStatus.Ready, _requestParser.Status);
        }

        [TestMethod]
        public void FeedData_GivenNegativeChunkSize_ReturnsError()
        {
            // Arrange
            const string strData = "GET / HTTP/1.1\r\n" +
                "Host: example.com\r\n" +
                "Transfer-Encoding: chunked\r\n" +
                "\r\n" +
                "-2\r\n" +
                "\r\n" +
                "0\r\n" +
                "\r\n";
            var data = new List<byte>(Encoding.ASCII.GetBytes(strData));

            // Act
            _requestParser.FeedData(data);

            // Assert
            Assert.AreEqual(ParserStatus.Error, _requestParser.Status);
        }

        [TestMethod]
        public void FeedData_GivenChunkSizeWithByteLengthLongerThanMaximumBodySize_ReturnsError()
        {
            // Arrange
            const string strData = "GET / HTTP/1.1\r\n" +
                "Host: example.com\r\n" +
                "Transfer-Encoding: chunked\r\n" +
                "\r\n";
            var data = new List<byte>(Encoding.ASCII.GetBytes(strData));
            data.AddRange(Enumerable.Repeat<byte>(0x31, 4194305));  // "1", 4MB + 1

            // Act
            _requestParser.FeedData(data);

            // Assert
            Assert.AreEqual(ParserStatus.Error, _requestParser.Status);
        }

        [TestMethod]
        public void FeedData_GivenChunkSizeWithValueLargerThanInt32_ReturnsError()
        {
            // Arrange
            const string strData = "GET / HTTP/1.1\r\n" +
                "Host: example.com\r\n" +
                "Transfer-Encoding: chunked\r\n" +
                "\r\n";
            var data = new List<byte>(Encoding.ASCII.GetBytes(strData));
            data.AddRange(Enumerable.Repeat<byte>(0x31, 4194304));  // "1", 4MB
            data.AddRange(new List<byte> { 0x0D, 0x0A });

            // Act
            _requestParser.FeedData(data);

            // Assert
            Assert.AreEqual(ParserStatus.Error, _requestParser.Status);
        }

        [TestMethod]
        public void FeedData_GivenChunkSizeWithValueLargerMaximumChunkSizeLength_ReturnsError()
        {
            // Arrange
            const string strData = "GET / HTTP/1.1\r\n" +
                "Host: example.com\r\n" +
                "Transfer-Encoding: chunked\r\n" +
                "\r\n";
            var data = new List<byte>(Encoding.ASCII.GetBytes(strData));
            data.AddRange(Enumerable.Repeat<byte>(0x31, 9));  // "1"
            data.AddRange(new List<byte> { 0x0D, 0x0A });

            // Act
            _requestParser.FeedData(data);

            // Assert
            Assert.AreEqual(ParserStatus.Error, _requestParser.Status);
        }

        [TestMethod]
        public void FeedData_GivenEmptyChunkSize_ReturnsError()
        {
            // Arrange
            const string strData = "GET / HTTP/1.1\r\n" +
                "Host: example.com\r\n" +
                "Transfer-Encoding: chunked\r\n" +
                "\r\n" +
                "\r\n";
            var data = new List<byte>(Encoding.ASCII.GetBytes(strData));

            // Act
            _requestParser.FeedData(data);

            // Assert
            Assert.AreEqual(ParserStatus.Error, _requestParser.Status);
        }

        [TestMethod]
        public void FeedData_GivenChunkSizeInHexDigits_ReturnsReady()
        {
            // Arrange
            const string strData = "GET / HTTP/1.1\r\n" +
                "Host: example.com\r\n" +
                "Transfer-Encoding: chunked\r\n" +
                "\r\n" +
                "C\r\n" +
                "Hello World!\r\n" +
                "0\r\n" +
                "\r\n";
            var data = new List<byte>(Encoding.ASCII.GetBytes(strData));

            // Act
            _requestParser.FeedData(data);

            // Assert
            Assert.AreEqual(ParserStatus.Ready, _requestParser.Status);
        }

        [TestMethod]
        public void FeedData_GivenChunkSizeInLowerCaseHexDigits_ReturnsReady()
        {
            // Arrange
            const string strData = "GET / HTTP/1.1\r\n" +
                "Host: example.com\r\n" +
                "Transfer-Encoding: chunked\r\n" +
                "\r\n" +
                "c\r\n" +
                "Hello World!\r\n" +
                "0\r\n" +
                "\r\n";
            var data = new List<byte>(Encoding.ASCII.GetBytes(strData));

            // Act
            _requestParser.FeedData(data);

            // Assert
            Assert.AreEqual(ParserStatus.Ready, _requestParser.Status);
        }

        [TestMethod]
        public void FeedData_GivenHeadersInTrailerPart_ReturnsReady()
        {
            // Arrange
            const string strData = "GET / HTTP/1.1\r\n" +
                "Host: example.com\r\n" +
                "Transfer-Encoding: chunked\r\n" +
                "\r\n" +
                "c\r\n" +
                "Hello World!\r\n" +
                "0\r\n" +
                "User-Agent: Fingers v1.0\r\n" +
                "\r\n";
            var data = new List<byte>(Encoding.ASCII.GetBytes(strData));

            // Act
            _requestParser.FeedData(data);

            // Assert
            Assert.AreEqual(ParserStatus.Ready, _requestParser.Status);
            Assert.IsTrue(_requestBuilder.HasHeader("User-Agent"));
        }

        [TestMethod]
        public void FeedData_GivenHeadersInTrailerPartInTwoCommits_ReturnsReady()
        {
            // Arrange
            const string strFirstData = "GET / HTTP/1.1\r\n" +
                "Host: example.com\r\n" +
                "Transfer-Encoding: chunked\r\n" +
                "\r\n" +
                "c\r\n" +
                "Hello World!\r\n" +
                "0\r\n" +
                "User-Agent: Fingers v1.0\r\n" +
                "\r\n" +
                "User-Agent: ";
            var data = new List<byte>(Encoding.ASCII.GetBytes(strFirstData));
            _requestParser.FeedData(data);

            const string strSecondData = "Fingers v1.0\r\n" +
                "\r\n";
            data = new List<byte>(Encoding.ASCII.GetBytes(strSecondData));

            // Act
            _requestParser.FeedData(data);

            // Assert
            Assert.AreEqual(ParserStatus.Ready, _requestParser.Status);
            Assert.IsTrue(_requestBuilder.HasHeader("User-Agent"));
        }
    }
}
