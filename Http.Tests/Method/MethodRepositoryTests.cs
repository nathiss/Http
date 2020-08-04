#region Copyrights
// This file is a part of the Http project.
//
// Copyright (c) 2020 Kamil Rusin
// Licensed under the MIT License.
// See LICENSE.txt file in the project root for full license information.
#endregion

using Http.Method;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Http.Tests.Method
{
    [TestClass]
    public class MethodRepositoryTests
    {
        [DataTestMethod]
        [DataRow(MethodType.Get)]
        [DataRow(MethodType.Head)]
        [DataRow(MethodType.Post)]
        [DataRow(MethodType.Put)]
        [DataRow(MethodType.Put)]
        [DataRow(MethodType.Delete)]
        [DataRow(MethodType.Trace)]
        [DataRow(MethodType.Connect)]
        [DataRow(MethodType.Options)]
        public void GetMethod_GivenValidMethodType_ReturnsObjectContainingTheSameType(MethodType type)
        {
            // Arrange
            var repository = new MethodRepository();

            // Act
            var method = repository.GetMethod(type);

            // Assert
            Assert.AreEqual(type, method.Type);
        }
    }
}
