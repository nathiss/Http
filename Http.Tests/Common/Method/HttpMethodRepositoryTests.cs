#region Copyrights
// This file is a part of the Http.Tests project.
//
// Copyright (c) 2020 Kamil Rusin
// Licensed under the MIT License.
// See LICENSE.txt file in the project root for full license information.
#endregion

using Http.Common.Method;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Http.Tests.Common.Method
{
    [TestClass]
    public class HttpMethodRepositoryTests
    {
        [DataTestMethod]
        [DataRow(HttpMethodType.Get)]
        [DataRow(HttpMethodType.Head)]
        [DataRow(HttpMethodType.Post)]
        [DataRow(HttpMethodType.Put)]
        [DataRow(HttpMethodType.Put)]
        [DataRow(HttpMethodType.Delete)]
        [DataRow(HttpMethodType.Trace)]
        [DataRow(HttpMethodType.Connect)]
        [DataRow(HttpMethodType.Options)]
        public void GetMethod_GivenValidMethodType_ReturnsObjectContainingTheSameType(HttpMethodType type)
        {
            // Arrange
            var repository = new HttpMethodRepository();

            // Act
            var method = repository.GetMethod(type);

            // Assert
            Assert.AreEqual(type, method.Type);
        }
    }
}
