#region Copyrights
// This file is a part of the Http.Tests project.
//
// Copyright (c) 2020 Kamil Rusin
// Licensed under the MIT License.
// See LICENSE.txt file in the project root for full license information.
#endregion

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Http;

namespace Http.Tests
{
    [TestClass]
    public class MethodTests
    {
        [DataTestMethod]
        [DataRow("GET", "GET", Method.Type.Get)]
        [DataRow("HEAD", "HEAD", Method.Type.Head)]
        [DataRow("POST", "POST", Method.Type.Post)]
        [DataRow("PUT", "PUT", Method.Type.Put)]
        [DataRow("DELETE", "DELETE", Method.Type.Delete)]
        [DataRow("TRACE", "TRACE", Method.Type.Trace)]
        [DataRow("CONNECT", "CONNECT", Method.Type.Connect)]
        [DataRow("OPTIONS", "OPTIONS", Method.Type.Options)]
        public void FromString_ValidMethod_ReturnValidMethodObject(string encodedMethod,
            string expectedEncodedMethod,
            Method.Type expectedHttpMethodType)
        {
            // Act
            var method = Method.FromString(encodedMethod);

            // Assert
            Assert.AreEqual(expectedEncodedMethod, method.EncodedMethod);
            Assert.AreEqual(expectedHttpMethodType, method.MethodType);
        }

        [TestMethod]
        public void FromString_MixedCase_ReturnValidMethodObject()
        {
            // Act
            var getMethod = Method.FromString("gEt");
            var deleteMethod = Method.FromString("deLETE");

            // Assert
            Assert.AreEqual("GET", getMethod.EncodedMethod);
            Assert.AreEqual(Method.Type.Delete, deleteMethod.MethodType);
        }

        [TestMethod]
        public void FromString_InvalidEncodedMethod_ReturnsNull()
        {
            // Act
            var method = Method.FromString("unknown");

            // Assert
            Assert.IsNull(method);
        }

        [TestMethod]
        public void FromString_WithUntrimmedWhitespaces_ReturnsValidMethodObject()
        {
            // Act
            var method = Method.FromString("  GET         ");

            // Assert
            Assert.AreEqual(Method.Type.Get, method.MethodType);
        }

        [TestMethod]
        public void FromString_ParameterIsNull_ReturnsNull()
        {
            // Act
            var method = Method.FromString(null);

            // Assert
            Assert.IsNull(method);
        }

        [TestMethod]
        public void FromString_ParameterIsEmpty_ReturnsNull()
        {
            // Act
            var method = Method.FromString(string.Empty);

            // Assert
            Assert.IsNull(method);
        }

        [TestMethod]
        public void FromString_ParameterIsOnlyWhiteSpace_ReturnsNull()
        {
            // Act
            var method = Method.FromString("      \t     \n    \r    ");

            // Assert
            Assert.IsNull(method);
        }
    }
}
