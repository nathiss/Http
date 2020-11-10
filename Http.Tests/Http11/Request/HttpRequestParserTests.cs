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
    }
}
