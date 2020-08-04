#region Copyrights
// This file is a part of the Http project.
//
// Copyright (c) 2020 Kamil Rusin
// Licensed under the MIT License.
// See LICENSE.txt file in the project root for full license information.
#endregion

using System.Collections.Generic;

namespace Http.StatusCode
{
    /// <summary>
    /// This class implements the functionality used to access <see cref="StatusCode" /> objects.
    /// </summary>
    public class StatusCodeRepository : IStatusCodeRepository
    {
        /// <inheritdoc />
        public StatusCode Get(int statusCodeValue)
        {
            try
            {
                return KnownStatusCodes[statusCodeValue];
            }
            catch (KeyNotFoundException)
            {
                throw new UnknownStatusCodeException($"Unknown HTTP status-code: {statusCodeValue}.");
            }
        }

        /// <summary>
        /// This collection contains all knows HTTP status-codes known in the system.
        /// </summary>

        private static readonly IDictionary<int, StatusCode> KnownStatusCodes = new Dictionary<int, StatusCode>
        {
            // Informational
            /// <summary>
            /// The 100 (Continue) status code indicates that the initial part of a request has been received and has
            /// not yet been rejected by the server.  The server intends to send a final response after the request has
            /// been fully received and acted upon.
            /// </summary>
            { 100, new StatusCode(100) },

            /// <summary>
            /// The 101 (Switching Protocols) status code indicates that the server understands and is willing to comply
            ///  with the client's request, via the Upgrade header field (Section 6.7 of [RFC7230]), for a change in the
            ///  application protocol being used on this connection.
            /// </summary>
            { 101, new StatusCode(101) },

            // Successful
            /// <summary>
            /// The 200 (OK) status code indicates that the request has succeeded.
            /// </summary>
            { 200, new StatusCode(200) },

            /// <summary>
            /// The 201 (Created) status code indicates that the request has been fulfilled and has resulted in one or
            /// more new resources being created.
            /// </summary>
            { 201, new StatusCode(201) },

            /// <summary>
            ///The 202 (Accepted) status code indicates that the request has been accepted for processing, but the
            /// processing has not been completed.
            /// </summary>
            { 202, new StatusCode(202) },

            /// <summary>
            /// The 203 (Non-Authoritative Information) status code indicates that the request was successful but the
            /// enclosed payload has been modified from that of the origin server's 200 (OK) response by a transforming
            /// proxy.
            /// </summary>
            { 203, new StatusCode(203) },

            /// <summary>
            /// The 204 (No Content) status code indicates that the server has successfully fulfilled the request and
            /// that there is no additional content to send in the response payload body.
            /// </summary>
            { 204, new StatusCode(204) },

            /// <summary>
            /// The 205 (Reset Content) status code indicates that the server has fulfilled the request and desires that
            /// the user agent reset the "document view", which caused the request to be sent, to its original state as
            /// received from the origin server.
            /// </summary>
            { 205, new StatusCode(205) },

            /// <summary>
            /// The 206 (Partial Content) status code indicates that the server is successfully fulfilling a range
            /// request for the target resource by transferring one or more parts of the selected representation that
            /// correspond to the satisfiable ranges found in the request's Range header field.
            /// </summary>
            { 206, new StatusCode(206) },

            // Redirects
            /// <summary>
            /// The 300 (Multiple Choices) status code indicates that the target resource has more than one
            /// representation, each with its own more specific identifier, and information about the alternatives is
            /// being provided so that the user (or user agent) can select a preferred representation by redirecting its
            ///  request to one or more of those identifiers.
            /// </summary>
            { 300, new StatusCode(300) },

            /// <summary>
            /// The 301 (Moved Permanently) status code indicates that the target resource has been assigned a new
            /// permanent URI and any future references to this resource ought to use one of the enclosed URIs.
            /// </summary>
            { 301, new StatusCode(301) },

            /// <summary>
            /// The 302 (Found) status code indicates that the target resource resides temporarily under a different
            /// URI. Since the redirection might be altered on occasion, the client ought to continue to use the
            /// effective request URI for future requests.
            /// </summary>
            { 302, new StatusCode(302) },

            /// <summary>
            /// The 303 (See Other) status code indicates that the server is redirecting the user agent to a different
            /// resource, as indicated by a URI in the Location header field, which is intended to provide an indirect
            /// response to the original request.
            /// </summary>
            { 303, new StatusCode(303) },

            /// <summary>
            /// The 304 (Not Modified) status code indicates that a conditional GET or HEAD request has been received
            /// and would have resulted in a 200 (OK) response if it were not for the fact that the condition
            /// evaluated to false.  In other words, there is no need for the server to transfer a representation of the
            /// target resource because the request indicates that the client, which made the request conditional,
            /// already has a valid representation; the server is therefore redirecting the client to make use of that
            /// stored representation as if it were the payload of a 200 (OK) response.
            /// </summary>
            { 304, new StatusCode(304) },

            /// <summary>
            /// The 305 (Use Proxy) status code was defined in a previous version of HTTP specification and is now
            /// deprecated.
            /// </summary>
            { 305, new StatusCode(305) },

            /// <summary>
            /// The 306 status code was defined in a previous version of this specification, is no longer used, and the
            /// code is reserved.
            /// </summary>
            { 306, new StatusCode(306) },

            /// <summary>
            /// The 307 (Temporary Redirect) status code indicates that the target resource resides temporarily under a
            /// different URI and the user agent MUST NOT change the request method if it performs an automatic
            /// redirection to that URI.
            /// </summary>
            { 307, new StatusCode(307) },

            // Client Error
            /// <summary>
            /// The 400 (Bad Request) status code indicates that the server cannot or will not process the request due
            /// to something that is perceived to be a client error.
            /// </summary>
            {400, new StatusCode(400) },

            /// <summary>
            /// The 401 (Unauthorized) status code indicates that the request has not been applied because it lacks
            /// valid authentication credentials for the target resource.  The server generating a 401 response MUST
            /// send a WWW-Authenticate header field containing at least one challenge applicable to the target
            /// resource.
            /// </summary>
            {401, new StatusCode(401) },

            /// <summary>
            /// The 402 (Payment Required) status code is reserved for future use.
            /// </summary>
            {402, new StatusCode(402) },

            /// <summary>
            /// The 403 (Forbidden) status code indicates that the server understood the request but refuses to
            /// authorize it.  A server that wishes to make public why the request has been forbidden can describe that
            /// reason in the response payload (if any).
            /// </summary>
            {403, new StatusCode(403) },

            /// <summary>
            /// The 404 (Not Found) status code indicates that the origin server did not find a current representation
            /// for the target resource or is not willing to disclose that one exists. A 404 status code does not
            /// indicate whether this lack of representation is temporary or permanent; the 410 (Gone) status code is
            /// preferred over 404 if the origin server knows, presumably through some configurable means, that the
            /// condition is likely to be permanent.
            /// </summary>
            {404, new StatusCode(404) },

            /// <summary>
            /// The 405 (Method Not Allowed) status code indicates that the method received in the request-line is
            /// known by the origin server but not supported by the target resource.  The origin server MUST generate an
            /// Allow header field in a 405 response containing a list of the target resource's currently supported
            /// methods.
            /// </summary>
            {405, new StatusCode(405) },

            /// <summary>
            /// The 406 (Not Acceptable) status code indicates that the target resource does not have a current
            /// representation that would be acceptable to the user agent, according to the proactive negotiation header
            /// fields received in the request, and the server is unwilling to supply a default representation.
            /// </summary>
            /// <seealso href="https://tools.ietf.org/html/rfc7231#section-5.3">
            /// RFC 7231 (Section 5.3 - Content Negotiation)
            /// </seealso>
            {406, new StatusCode(406) },

            /// <summary>
            /// The 407 (Proxy Authentication Required) status code is similar to 401 (Unauthorized), but it indicates
            /// that the client needs to authenticate itself in order to use a proxy.  The proxy MUST send a
            /// Proxy-Authenticate header field containing a challenge applicable to that proxy for the target resource.
            /// </summary>
            {407, new StatusCode(407) },

            /// <summary>
            ///  The 408 (Request Timeout) status code indicates that the server did not receive a complete request
            /// message within the time that it was prepared to wait.  A server SHOULD send the "close" connection
            /// option in the response, since 408 implies that the server has decided to close the connection rather
            /// than continue waiting.  If the client has an outstanding request in transit, the client MAY repeat that
            /// request on a new connection.
            /// </summary>
            {408, new StatusCode(408) },

            /// <summary>
            /// The 409 (Conflict) status code indicates that the request could not be completed due to a conflict with
            /// the current state of the target resource.  This code is used in situations where the user might be able
            /// to resolve the conflict and resubmit the request.  The server SHOULD generate a payload that includes
            /// enough information for a user to recognize the source of the conflict.
            /// </summary>
            {409, new StatusCode(409) },

            /// <summary>
            /// The 410 (Gone) status code indicates that access to the target resource is no longer available at the
            /// origin server and that this condition is likely to be permanent.
            /// </summary>
            {410, new StatusCode(410) },

            /// <summary>
            /// The 411 (Length Required) status code indicates that the server refuses to accept the request without a
            /// defined Content-Length.
            /// </summary>
            {411, new StatusCode(411) },

            /// <summary>
            /// The 412 (Precondition Failed) status code indicates that one or more conditions given in the request
            /// header fields evaluated to false when tested on the server.
            /// </summary>
            {412, new StatusCode(412) },

            /// <summary>
            /// The 413 (Payload Too Large) status code indicates that the server is refusing to process a request
            /// because the request payload is larger than the server is willing or able to process.  The server MAY
            /// close the connection to prevent the client from continuing the request.
            /// </summary>
            {413, new StatusCode(413) },

            /// <summary>
            /// The 414 (URI Too Long) status code indicates that the server is refusing to service the request because
            /// the request-target is longer than the server is willing to interpret.
            /// </summary>
            /// <returns></returns>
            {414, new StatusCode(414) },

            /// <summary>
            /// The 415 (Unsupported Media Type) status code indicates that the origin server is refusing to service the
            /// request because the payload is in a format not supported by this method on the target resource. The
            /// format problem might be due to the request's indicated Content-Type or Content-Encoding, or as a result
            /// of inspecting the data directly.
            /// </summary>
            {415, new StatusCode(415) },

            /// <summary>
            /// The 416 (Range Not Satisfiable) status code indicates that none of the ranges in the request's Range
            /// header field (Section 3.1) overlap the current extent of the selected resource or that the set of ranges
            /// requested has been rejected due to invalid ranges or an excessive request of small or overlapping
            /// ranges.
            /// </summary>
            {416, new StatusCode(416) },

            /// <summary>
            /// The 417 (Expectation Failed) status code indicates that the expectation given in the request's Expect
            /// header field could not be met by at least one of the inbound servers.
            /// </summary>
            {417, new StatusCode(417) },

            /// <summary>
            /// The 426 (Upgrade Required) status code indicates that the server refuses to perform the request using
            /// the current protocol but might be willing to do so after the client upgrades to a different protocol.
            /// The server MUST send an Upgrade header field in a 426 response to indicate the required protocol(s).
            /// </summary>
            {426, new StatusCode(426) },

            // Server Error
            /// <summary>
            /// The 500 (Internal Server Error) status code indicates that the server encountered an unexpected
            /// condition that prevented it from fulfilling the request.
            /// </summary>
            { 500, new StatusCode(500) },

            /// <summary>
            /// The 501 (Not Implemented) status code indicates that the server does not support the functionality
            /// required to fulfill the request.  This is the appropriate response when the server does not recognize
            /// the request method and is not capable of supporting it for any resource.
            /// </summary>
            { 501, new StatusCode(501) },

            /// <summary>
            /// The 502 (Bad Gateway) status code indicates that the server, while acting as a gateway or proxy,
            /// received an invalid response from an inbound server it accessed while attempting to fulfill the request.
            /// </summary>
            { 502, new StatusCode(502) },

            /// <summary>
            /// The 503 (Service Unavailable) status code indicates that the server is currently unable to handle the
            /// request due to a temporary overload or scheduled maintenance, which will likely be alleviated after some
            /// delay.
            /// </summary>
            { 503, new StatusCode(503) },

            /// <summary>
            /// The 504 (Gateway Timeout) status code indicates that the server, while acting as a gateway or proxy, did
            /// not receive a timely response from an upstream server it needed to access in order to complete the
            /// request.
            /// </summary>
            { 504, new StatusCode(504) },

            /// <summary>
            /// The 505 (HTTP Version Not Supported) status code indicates that the server does not support, or refuses
            /// to support, the major version of HTTP that was used in the request message.
            /// </summary>
            { 505, new StatusCode(505) },
        };
    }
}
