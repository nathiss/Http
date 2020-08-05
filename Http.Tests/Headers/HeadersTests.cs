#region Copyrights
// This file is a part of the Http project.
//
// Copyright (c) 2020 Kamil Rusin
// Licensed under the MIT License.
// See LICENSE.txt file in the project root for full license information.
#endregion

using Http.Headers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Http.Tests.Headers
{
    [TestClass]
    public class HeadersTests
    {
        private IHeaders headers;

        [TestInitialize]
        public void SetHeaders()
        {
            headers = new Http.Headers.Headers();
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
    }
}
