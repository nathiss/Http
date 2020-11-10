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

        private IRequestParser _requestParser;

        [TestInitialize]
        public void SetUp()
        {
            var requestBuilder = new RequestBuilder(_httpMethodRepository, _httpVersionRepository, new HttpHeaders());
            _requestParser = new HttpRequestParser(requestBuilder);
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
            var strData = "GET /index.html HTTP/1.1\r\n";
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
            var strData = "get /index.html HTTP/1.1\r\n";
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
            var strData = "GET /index.html HTTP/1.1";
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
            var strData = "GET HTTP/1.1\r\n";
            var data = new List<byte>(Encoding.ASCII.GetBytes(strData));

            // Act
            _requestParser.FeedData(data);

            // Assert
            Assert.AreEqual(ParserStatus.Error, _requestParser.Status);
        }
    }
}
