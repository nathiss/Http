#region Copyrights
// This file is a part of the Http project.
//
// Copyright (c) 2020 Kamil Rusin
// Licensed under the MIT License.
// See LICENSE.txt file in the project root for full license information.
#endregion

using Http.Common.Version;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Http.Tests.Common.Version
{
    [TestClass]
    public class HttpVersionRepositoryTests
    {
        [DataTestMethod]
        [DataRow(HttpVersionType.Http1_0)]
        [DataRow(HttpVersionType.Http1_1)]
        [DataRow(HttpVersionType.Http2_0)]
        public void GetHttpVersion_GivenValidHttpVersionType_ReturnsHttpVersionObjectContainingTheSameValue(
            HttpVersionType type)
        {
            // Arrange
            var repository = new HttpVersionRepository();

            // Act
            var version = repository.GetHttpVersion(type);

            // Assert
            Assert.AreEqual(type, version.Version);
        }
    }
}
