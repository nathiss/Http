#region Copyrights
// This file is a part of the Http.Tests project.
//
// Copyright (c) 2020 Kamil Rusin
// Licensed under the MIT License.
// See LICENSE.txt file in the project root for full license information.
#endregion

using Http.Common.StatusCode;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Http.Tests.Common.StatusCode
{
    [TestClass]
    public class HttpStatusCodeRepositoryTests
    {
        [DataTestMethod]
        [DataRow(100)]
        [DataRow(101)]
        [DataRow(200)]
        [DataRow(201)]
        [DataRow(202)]
        [DataRow(203)]
        [DataRow(204)]
        [DataRow(205)]
        [DataRow(206)]
        [DataRow(300)]
        [DataRow(301)]
        [DataRow(302)]
        [DataRow(303)]
        [DataRow(304)]
        [DataRow(305)]
        [DataRow(306)]
        [DataRow(307)]
        [DataRow(400)]
        [DataRow(401)]
        [DataRow(402)]
        [DataRow(403)]
        [DataRow(404)]
        [DataRow(405)]
        [DataRow(406)]
        [DataRow(407)]
        [DataRow(408)]
        [DataRow(409)]
        [DataRow(410)]
        [DataRow(411)]
        [DataRow(412)]
        [DataRow(413)]
        [DataRow(414)]
        [DataRow(415)]
        [DataRow(416)]
        [DataRow(417)]
        [DataRow(426)]
        [DataRow(500)]
        [DataRow(501)]
        [DataRow(502)]
        [DataRow(503)]
        [DataRow(504)]
        [DataRow(505)]
        public void GetStatusCode_GivenValidStatusCodeNumber_ReturnsObjectConstaingTheSameNumber(int statusCodeNumber)
        {
            // Arrange
            var repository = new HttpStatusCodeRepository();

            // Act
            var statusCode = repository.GetStatusCode(statusCodeNumber);

            // Assert
            Assert.AreEqual(statusCodeNumber, statusCode.Value);
        }

        [DataTestMethod]
        [DataRow(102)]
        [DataRow(-8)]
        [DataRow(0)]
        [DataRow(100000000)]
        [DataRow(506)]
        [DataRow(308)]
        public void GetStatusCode_GivenInvalidStatusCodeNumber_ThrownsAnExceptionOfTypeUnknownStatusCodeException(
            int statusCodeNumber)
        {
            // Arrange
            var repository = new HttpStatusCodeRepository();

            // Assert
            Assert.ThrowsException<UnknownHttpStatusCodeException>(() => repository.GetStatusCode(statusCodeNumber));
        }
    }
}
