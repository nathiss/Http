#region Copyrights
// This file is a part of the Http project.
//
// Copyright (c) 2020 Kamil Rusin
// Licensed under the MIT License.
// See LICENSE.txt file in the project root for full license information.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using Http.Common.Extentions;

namespace Http.Common.Headers
{
    /// <summary>
    /// This class provides the default functionality to normalize the case of field-names.
    /// </summary>
    public class DefaultFieldNameNormalizer : IFieldNameNormalizer
    {
        /// <inheritdoc />
        public string Normalize(string fieldName)
        {
            if (fieldName is null)
            {
                throw new ArgumentNullException(nameof(fieldName));
            }

            var customCaseListIndex = fieldNamesWithCustomCase.FindIndex(
                customCaseFieldName => customCaseFieldName.Equals(fieldName, StringComparison.OrdinalIgnoreCase)
            );
            if (customCaseListIndex != -1)
            {
                return fieldNamesWithCustomCase[customCaseListIndex];
            }

            return string.Join(
                '-',
                fieldName.Split('-').Select(segment => segment.FirstCharToUpper())
            );
        }

        /// <summary>
        /// This collection contains all custom-cased field-names. When normalization is requested via
        /// <see cref="Normalize(string)" /> call, then the name first gets check inside this collection.
        /// </summary>
        private static readonly List<string> fieldNamesWithCustomCase = new List<string>
        {
            "TE",
            "WWW-Authenticate",
            "Accept-CH",
            "Accept-CH-Lifetime",
            "Content-DPR",
            "DPR",
            "ETag",
            "DNT",
            "Tk",
            "Expect-CT",
            "X-XSS-Protection",
            "X-XSS-Protection",
            "Last-Event-ID",
            "NET",
            "X-DNS-Prefetch-Control",
            "Content-MD5",
            "HTTP2-Settings",
            "X-ATT-DeviceId",
            "X-UIDH",
            "P3P",
            "X-WebKit-CSP",
            "X-Request-ID",
            "X-Correlation-ID",
            "X-UA-Compatible",
            "X-XSS-Protection",
        };
    }
}
