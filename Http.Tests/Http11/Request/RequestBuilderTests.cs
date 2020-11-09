#region Copyrights
// This file is a part of the Http project.
//
// Copyright (c) 2020 Kamil Rusin
// Licensed under the MIT License.
// See LICENSE.txt file in the project root for full license information.
#endregion

using System;
using System.Text;
using Http.Common.Headers;
using Http.Common.Method;
using Http.Common.Version;
using Http.Http11.Request;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uri;

namespace Http.Tests.Http11.Request
{
    [TestClass]
    public class RequestBuilderTests
    {
        private static readonly IHttpMethodRepository _httpMethodRepository = new HttpMethodRepository();

        private static readonly IHttpVersionRepository _httpVersionRepository = new HttpVersionRepository();

        private IHttpHeaders _httpHeaders;

        private RequestBuilder _requestBuilder;

        [TestInitialize]
        public void SetUp()
        {
            _httpHeaders = new HttpHeaders();
            _requestBuilder = new RequestBuilder(_httpMethodRepository, _httpVersionRepository, _httpHeaders);
        }

        [TestMethod]
        public void Build_NothingWasSetUp_ThrowsAnException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _requestBuilder.Build());
        }

        [TestMethod]
        public void Build_ValidProperties_ReturnsIRequest()
        {
            // Act
            var request = _requestBuilder
                .SetMethod(HttpMethodType.Get)
                .SetTarget(UniformResourceIdentifier.FromString("/index.html"))
                .SetHttpVersion(HttpVersionType.Http1_1)
                .SetHeader("Host", "example.com")
                .SetBody("Hello World!", Encoding.Default)
                .Build();

            // Assert
            Assert.IsNotNull(request);
        }

        [DataTestMethod]
        [DataRow(HttpMethodType.Get)]
        [DataRow(HttpMethodType.Head)]
        [DataRow(HttpMethodType.Connect)]
        [DataRow(HttpMethodType.Delete)]
        [DataRow(HttpMethodType.Options)]
        [DataRow(HttpMethodType.Post)]
        [DataRow(HttpMethodType.Put)]
        [DataRow(HttpMethodType.Trace)]
        public void SetMethod_GivenMethod_ReturnsTheSameMethod(HttpMethodType httpMethodType)
        {
            // Act
            var request = _requestBuilder
                .SetMethod(httpMethodType)
                .SetTarget(UniformResourceIdentifier.FromString("/index.html"))
                .SetHttpVersion(HttpVersionType.Http1_1)
                .SetHeader("Host", "example.com")
                .SetBody("Hello World!", Encoding.Default)
                .Build();

            // Assert
            Assert.AreEqual(httpMethodType, request.Method);
        }

        [TestMethod]
        public void Target_SetsTarget_ReturnsTheSameTarget()
        {
            // Arrange
            var uri = UniformResourceIdentifier.FromString("/index.html");

            // Act
            var request = _requestBuilder
                .SetMethod(HttpMethodType.Connect)
                .SetTarget(uri)
                .SetHttpVersion(HttpVersionType.Http1_1)
                .SetHeader("Host", "example.com")
                .SetBody("Hello World!", Encoding.Default)
                .Build();

            // Assert
            Assert.AreEqual(uri, request.Target);
        }


        [DataTestMethod]
        [DataRow(HttpVersionType.Http1_0)]
        [DataRow(HttpVersionType.Http1_1)]
        [DataRow(HttpVersionType.Http2_0)]
        public void SetHttpVersion_GivenVersion_ReturnsTheSameVersion(HttpVersionType httpVersionType)
        {
            // Act
            var request = _requestBuilder
                .SetMethod(HttpMethodType.Connect)
                .SetTarget(UniformResourceIdentifier.FromString("/index.html"))
                .SetHttpVersion(httpVersionType)
                .SetHeader("Host", "example.com")
                .SetBody("Hello World!", Encoding.Default)
                .Build();

            // Assert
            Assert.AreEqual(httpVersionType, request.HttpVersion);
        }

        [DataTestMethod]
        [DataRow("Host", "www.example.com")]
        [DataRow("Connection", "Keep-Alive")]
        [DataRow("ABC", null)]
        public void SetHeader_AddValidHeader_ReturnsTheSameHeader(string fieldName, string fieldValue)
        {
            // Act
            var request = _requestBuilder
                .SetMethod(HttpMethodType.Connect)
                .SetTarget(UniformResourceIdentifier.FromString("/index.html"))
                .SetHttpVersion(HttpVersionType.Http1_1)
                .SetHeader(fieldName, fieldValue)
                .SetBody("Hello World!", Encoding.Default)
                .Build();

            // Assert
            Assert.AreEqual(fieldValue, request.GetHeader(fieldName));
        }

        [TestMethod]
        public void GetHeader_GetNonExistingHeader_ReturnsNull()
        {
            // Act
            var request = _requestBuilder
                .SetMethod(HttpMethodType.Connect)
                .SetTarget(UniformResourceIdentifier.FromString("/index.html"))
                .SetHttpVersion(HttpVersionType.Http1_1)
                .SetHeader("Host", "example.com")
                .SetBody("Hello World!", Encoding.Default)
                .Build();

            // Arrange
            Assert.IsNull(request.GetHeader("NonExistingField"));
        }
    }
}
