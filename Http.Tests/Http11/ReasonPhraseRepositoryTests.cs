#region Copyrights
// This file is a part of the Http project.
//
// Copyright (c) 2020 Kamil Rusin
// Licensed under the MIT License.
// See LICENSE.txt file in the project root for full license information.
#endregion

using Http.Http11;
using Http.StatusCode;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Http.Tests.Http11
{
    [TestClass]
    public class ReasonPhraseRepositoryTests
    {
        private readonly IHttpStatusCodeRepository statusCodeRepository;

        private readonly IReasonPhraseRepository reasonPhraseRepository;

        public ReasonPhraseRepositoryTests()
        {
            statusCodeRepository = new HttpStatusCodeRepository();
            reasonPhraseRepository = new ReasonPhraseRepository();
        }

        [TestMethod]
        [DataTestMethod]
        [DataRow(100, "Continue")]
        [DataRow(101, "Switching Protocols")]
        [DataRow(200, "OK")]
        [DataRow(201, "Created")]
        [DataRow(202, "Accepted")]
        [DataRow(203, "Non-Authoritative Information")]
        [DataRow(204, "No Content")]
        [DataRow(205, "Reset Content")]
        [DataRow(206, "Partial Content")]
        [DataRow(300, "Multiple Choices")]
        [DataRow(301, "Moved Permanently")]
        [DataRow(302, "Found")]
        [DataRow(303, "See Other")]
        [DataRow(304, "Not Modified")]
        [DataRow(305, "Use Proxy")]
        [DataRow(307, "Temporary Redirect")]
        [DataRow(400, "Bad Request")]
        [DataRow(401, "Unauthorized")]
        [DataRow(402, "Payment Required")]
        [DataRow(403, "Forbidden")]
        [DataRow(404, "Not Found")]
        [DataRow(405, "Method Not Allowed")]
        [DataRow(406, "Not Acceptable")]
        [DataRow(407, "Proxy Authentication Required")]
        [DataRow(408, "Request Timeout")]
        [DataRow(409, "Conflict")]
        [DataRow(410, "Gone")]
        [DataRow(411, "Length Required")]
        [DataRow(412, "Precondition Failed")]
        [DataRow(413, "Payload Too Large")]
        [DataRow(414, "URI Too Long")]
        [DataRow(415, "Unsupported Media Type")]
        [DataRow(416, "Range Not Supported")]
        [DataRow(417, "Expectation Failed")]
        [DataRow(426, "Upgrade Required")]
        [DataRow(500, "Internal Server Error")]
        [DataRow(501, "Not Implemented")]
        [DataRow(502, "Bad Gateway")]
        [DataRow(503, "Service Unavailable")]
        [DataRow(504, "Gateway Timeout")]
        [DataRow(505, "HTTP Version Not Supported")]
        public void GetReasonPhrase_GivenValidStatusCode_ReturnsValidReasonPhrase(
            int statusCode,
            string validReasonPhrase)
        {
            // Arrange
            var sc = statusCodeRepository.GetStatusCode(statusCode);

            // Act
            var reasonPhrase = reasonPhraseRepository.GetReasonPhrase(sc);

            // Assert
            Assert.AreEqual(validReasonPhrase, reasonPhrase);
        }

        [TestMethod]
        [DataTestMethod]
        [DataRow(306)]
        public void GetReasonPhrase_GivenInvalidStatusCode_ThrowsAnException(int statusCode)
        {
            // Arrange
            var sc = statusCodeRepository.GetStatusCode(statusCode);

            // Assert
            Assert.ThrowsException<UnknownHttpStatusCodeException>(() => reasonPhraseRepository.GetReasonPhrase(sc));
        }
    }
}
