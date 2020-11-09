#region Copyrights
// This file is a part of the Http.Tests project.
//
// Copyright (c) 2020 Kamil Rusin
// Licensed under the MIT License.
// See LICENSE.txt file in the project root for full license information.
#endregion

using Http.Common.Headers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Http.Tests.Common.Headers
{
    [TestClass]
    public class HttpHeadersTests
    {
        private IHttpHeaders headers;

        [TestInitialize]
        public void SetHeaders()
        {
            headers = new HttpHeaders();
        }

        [TestMethod]
        public void IndexerString_RequestedAHeaderOnEmptyCollection_ThrowsAnException()
        {
            // Assert
            Assert.ThrowsException<UnknownHeaderFieldException>(() => headers["Host"]);
        }

        [TestMethod]
        public void IndexerString_CheckCountOnEmptyCollection_ReturnsZero()
        {
            // Assert
            Assert.AreEqual(0, headers.Count);
        }

        [TestMethod]
        public void IndexerString_AddOneHeader_ReturnsOne()
        {
            // Act
            headers["Host"] = "example.com";

            // Assert
            Assert.AreEqual(1, headers.Count);
        }

        [TestMethod]
        public void IndexerString_RequestedExistingHeader_ReturnsValidFieldValue()
        {
            // Act
            headers["Host"] = "example.com";

            // Assert
            Assert.AreEqual("example.com", headers["Host"]);
        }

        [TestMethod]
        public void IndexerString_RequestedExistingHeaderWithChangedValue_ReturnsValidFieldValue()
        {
            // Arrange
            headers["Host"] = "google.com";

            // Act
            headers["Host"] = "example.com";

            // Assert
            Assert.AreEqual("example.com", headers["Host"]);
        }

        [TestMethod]
        public void IndexerString_GivenNonEmptyCollectionRequestedNonExistingHeader_ThrowsAnException()
        {
            // Arrange
            headers["Host"] = "example.com";

            // Assert
            Assert.ThrowsException<UnknownHeaderFieldException>(() => headers["Keep-Alive"]);
        }

        [TestMethod]
        public void IndexerString_SetAHeaderAndAccessItWithChangedCase_ReturnsValidFieldValue()
        {
            // Arrange
            headers["Host"] = "example.com";

            // Assert
            Assert.AreEqual("example.com", headers["host"]);
        }

        [TestMethod]
        public void IndexerString_SetAHeaderWithDashInNameAndAccessItWithChangedCase_ReturnsValidFieldValue()
        {
            // Arrange
            headers["Keep-Alive"] = "True";

            // Assert
            Assert.AreEqual("True", headers["keep-ALIVE"]);
        }

        [TestMethod]
        public void ToList_GivenEmptyCollection_ReturnsZeroAsTheNumberOfElements()
        {
            // Assert
            Assert.AreEqual(0, headers.ToList().Count);
        }

        [TestMethod]
        public void ToList_GivenCollectionOfTwoElements_ReturnsTwoAsTheNumberOfElements()
        {
            // Act
            headers["Host"] = "example.com";
            headers["Keep-Alive"] = "True";

            // Assert
            Assert.AreEqual(2, headers.ToList().Count);
        }

        [TestMethod]
        public void IndexerString_GivenFieldNameWithWrongCase_ReturnsNormalizedCase()
        {
            // Act
            headers["host"] = "example.com";

            // Assert
            Assert.AreEqual("Host", headers.ToList()[0].Name);
        }

        [TestMethod]
        public void IndexerString_GivenFieldNameWithVeryWrongCase_ReturnsNormalizedCase()
        {
            // Act
            headers["hOST"] = "example.com";

            // Assert
            Assert.AreEqual("Host", headers.ToList()[0].Name);
        }

        [TestMethod]
        public void IndexerString_GivenFieldNameWithDashAndWrongCase_ReturnsNormalizedCase()
        {
            // Act
            headers["keep-alive"] = "True";

            // Assert
            Assert.AreEqual("Keep-Alive", headers.ToList()[0].Name);
        }

        [TestMethod]
        public void IndexerString_GivenFieldNameWithSeveralDashes_ReturnsNormalizedCase()
        {
            // Act
            headers["clear-side-DATA"] = string.Empty;

            // Assert
            Assert.AreEqual("Clear-Side-Data", headers.ToList()[0].Name);
        }

        [DataTestMethod]
        [DataRow("te", "TE")]
        [DataRow("www-Authenticate", "WWW-Authenticate")]
        [DataRow("content-md5", "Content-MD5")]
        [DataRow("p3p", "P3P")]
        [DataRow("http2-settings", "HTTP2-Settings")]
        public void IndexerString_GivenFieldNameThatRequiresCustomCase_ReturnsNormalizedCase(
            string fieldName,
            string normalizedFieldName
        )
        {
            // Act
            headers[fieldName] = string.Empty;

            // Assert
            Assert.AreEqual(normalizedFieldName, headers.ToList()[^1].Name);
        }
    }
}
