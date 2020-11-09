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
using Http.Common.MessageBody;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Http.Tests.Common.MessageBody
{
    [TestClass]
    public class HttpMessageBodyTests
    {
        [TestMethod]
        public void Count_SetContentToNull_Returns0()
        {
            // Arrange
            var messageBody = new HttpMessageBody();

            // Act
            messageBody.SetContent((List<byte>)null);

            // Assert
            Assert.AreEqual(0, messageBody.Count);
        }

        [TestMethod]
        public void Count_SetContentToListOfZeroElements_Returns0()
        {
            // Arrange
            var messageBody = new HttpMessageBody();

            // Act
            messageBody.SetContent(new List<byte>());

            // Assert
            Assert.AreEqual(0, messageBody.Count);
        }

        [TestMethod]
        public void Count_SetContentToListOfBytes_ReturnsTheSizeOfTheList()
        {
            // Arrange
            var messageBody = new HttpMessageBody();
            var bytes = new List<byte>
            {
                45,
                67,
                82,
                16,
                0,
                255
            };

            // Act
            messageBody.SetContent(bytes);

            // Assert
            Assert.AreEqual(bytes.Count, messageBody.Count);
        }

        [TestMethod]
        public void GetContent_SetContentToASequenceOfBytes_ReturnsTheSameList()
        {
            // Arrange
            var messageBody = new HttpMessageBody();
            var bytes = new List<byte>
            {
                45,
                67,
                82,
                16,
                0,
                255
            };

            // Act
            messageBody.SetContent(bytes);

            // Assert
            CollectionAssert.AreEqual(bytes, messageBody.GetContent());
        }

        [TestMethod]
        public void GetContentString_SetContentToString_ReturnsTheSameString()
        {
            // Arrange
            var messageBody = new HttpMessageBody();
            const string content = "467y4higifgerhtthąśććąę123123";

            // Act
            messageBody.SetContent(content);

            // Assert
            Assert.AreEqual(content, messageBody.GetContentString());
        }

        [TestMethod]
        public void GetContentStringEncoding_SetContentToStringEncoding_ReturnsTheSameString()
        {
            // Arrange
            var messageBody = new HttpMessageBody();
            const string content = "467y4higifgerhtthąśććąę123123";

            // Act
            messageBody.SetContent(content, Encoding.UTF8);

            // Assert
            Assert.AreEqual(content, messageBody.GetContentString(Encoding.UTF8));
        }

        [TestMethod]
        public void SetContentString_GivenNull_ThrowsArgumentNullException()
        {
            // Arrage
            var messageBody = new HttpMessageBody();

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => messageBody.SetContent((string)null));
        }

        [TestMethod]
        public void SetContentStringEncoding_GivenContentNull_ThrowsArgumentNullException()
        {
            // Arrage
            var messageBody = new HttpMessageBody();

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => messageBody.SetContent(null, Encoding.Default));
        }

        [TestMethod]
        public void SetContentStringEncoding_GivenEncodingNull_ThrowsArgumentNullException()
        {
            // Arrage
            var messageBody = new HttpMessageBody();

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => messageBody.SetContent(string.Empty, null));
        }

        [TestMethod]
        public void GetContentStringEncoding_GivenNull_ThrowsArgumentNullException()
        {
            // Arrage
            var messageBody = new HttpMessageBody();

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => messageBody.GetContentString(null));
        }
    }
}
