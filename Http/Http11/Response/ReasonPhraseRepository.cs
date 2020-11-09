#region Copyrights
// This file is a part of the Http project.
//
// Copyright (c) 2020 Kamil Rusin
// Licensed under the MIT License.
// See LICENSE.txt file in the project root for full license information.
#endregion

using System.Collections.Generic;
using Http.Common.StatusCode;

namespace Http.Http11.Response
{
    /// <summary>
    /// This class implements the functionality to map <see cref="HttpStatusCode" /> objects into string representing
    /// reason-phrase.
    /// </summary>
    public class ReasonPhraseRepository : IReasonPhraseRepository
    {
        /// <inheritdoc />
        public string GetReasonPhrase(HttpStatusCode statusCode)
        {
            try
            {
                return reasonPhrases[statusCode.Value];
            }
            catch (KeyNotFoundException)
            {
                throw new UnknownHttpStatusCodeException(
                    $"The given {statusCode} status-code does not have corresponding reason-phrase."
                );
            }
        }

        /// <summary>
        /// This dictionary contains all supported status-code values and their reason-phrases.
        /// </summary>
        private static readonly IDictionary<int, string> reasonPhrases = new Dictionary<int, string>
        {
            // Informational
            { 100, "Continue" },
            { 101, "Switching Protocols" },

            // Successful
            { 200, "OK" },
            { 201, "Created" },
            { 202, "Accepted" },
            { 203, "Non-Authoritative Information" },
            { 204, "No Content" },
            { 205, "Reset Content" },
            { 206, "Partial Content" },

            // Redirects
            { 300, "Multiple Choices" },
            { 301, "Moved Permanently" },
            { 302, "Found" },
            { 303, "See Other" },
            { 304, "Not Modified" },
            { 305, "Use Proxy" },
            { 307, "Temporary Redirect" },

            // Client Error
            { 400, "Bad Request" },
            { 401, "Unauthorized" },
            { 402, "Payment Required" },
            { 403, "Forbidden" },
            { 404, "Not Found" },
            { 405, "Method Not Allowed" },
            { 406, "Not Acceptable" },
            { 407, "Proxy Authentication Required" },
            { 408, "Request Timeout" },
            { 409, "Conflict" },
            { 410, "Gone" },
            { 411, "Length Required" },
            { 412, "Precondition Failed" },
            { 413, "Payload Too Large" },
            { 414, "URI Too Long" },
            { 415, "Unsupported Media Type" },
            { 416, "Range Not Supported" },
            { 417, "Expectation Failed" },
            { 426, "Upgrade Required" },

            // Server Error
            { 500, "Internal Server Error" },
            { 501, "Not Implemented" },
            { 502, "Bad Gateway" },
            { 503, "Service Unavailable" },
            { 504, "Gateway Timeout" },
            { 505, "HTTP Version Not Supported" },
        };
    }
}
