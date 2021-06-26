/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *  AppSettings is distributed under the GNU General Public License version 3 and  
 *  is also available under alternative licenses negotiated directly with Simon Carter.  
 *  If you obtained Service Manager under the GPL, then the GPL applies to all loadable 
 *  Service Manager modules used on your system as well. The GPL (version 3) is 
 *  available at https://opensource.org/licenses/GPL-3.0
 *
 *  This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
 *  without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 *  See the GNU General Public License for more details.
 *
 *  The Original Code was created by Simon Carter (s1cart3r@gmail.com)
 *
 *  Copyright (c) 2018 - 2019 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AppSettings
 *  
 *  File: SettingHttpResultAttribute.cs
 *
 *  Purpose:  Ensures the result is a valid Http Response
 *
 *  Date        Name                Reason
 *  01/12/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace AppSettings
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class SettingHttpResponseAttribute : Attribute
    {
        #region Private Constants

        // https://en.wikipedia.org/wiki/List_of_HTTP_status_codes

        private static readonly int[] Information = { 100, 101, 102, 103 };
        private static readonly int[] Success = { 200, 201, 202, 203, 204, 205, 206, 207, 208, 226 };
        private static readonly int[] Redirect = { 300, 301, 302, 303, 304, 305, 306, 307, 308 };
        private static readonly int[] ClientErrors = { 400, 401, 402, 403, 404, 405, 406, 407, 408, 409,
            410, 411, 412, 413, 414, 415, 416, 417, 418, 421, 422, 423, 424, 426, 428, 429, 431, 451 };
        private static readonly int[] ServerErrors = { 500, 501, 502, 503, 504, 505, 506, 507, 508, 510, 511 };
        private static readonly int[] Unofficial = { 103, 218, 419, 420, 450, 498, 499, 509, 526, 530, 598 };
        private static readonly int[] IIS = { 440, 449, 451 };
        private static readonly int[] Nginx = { 444, 494, 495, 496, 497, 499 };
        private static readonly int[] Cloudflare = { 520, 521, 522, 523, 524, 525, 526, 527, 530 };



        #endregion Private Constants

        #region Constructors

        public SettingHttpResponseAttribute()
            : this (HttpResponseType.Any)
        {

        }

        public SettingHttpResponseAttribute(HttpResponseType responseType)
        {
            if (responseType == HttpResponseType.Custom)
                throw new ArgumentOutOfRangeException(nameof(responseType), "Invalid constructor for custom range");

            ResponseType = responseType;
        }

        public SettingHttpResponseAttribute(params int[] validCodes)
        {
            ResponseType = HttpResponseType.Custom;
            ValidCodes = validCodes ?? throw new ArgumentNullException(nameof(ValidCodes));

            if (validCodes.Length == 0)
                throw new ArgumentException(nameof(validCodes), "You must specify at least 1 valid code");
        }

        #endregion Constructors

        #region Properties

        public HttpResponseType ResponseType { get; }

        public int[] ValidCodes { get; }

        #endregion Properties

        #region Internal Methods

        internal bool ValidateResponseCode(in int value)
        {
            if (ResponseType == HttpResponseType.Custom)
                return ArrayContains(value, ValidCodes);

            bool Result = false;

            if (ResponseType == HttpResponseType.Any || ResponseType == HttpResponseType.ClientErrors)
                Result = ArrayContains(value, ClientErrors);

            if (!Result && (ResponseType == HttpResponseType.Any || ResponseType == HttpResponseType.Cloudflare))
                Result = ArrayContains(value, Cloudflare);

            if (!Result && (ResponseType == HttpResponseType.Any || ResponseType == HttpResponseType.IIS))
                Result = ArrayContains(value, IIS);

            if (!Result && (ResponseType == HttpResponseType.Any || ResponseType == HttpResponseType.Information))
                Result = ArrayContains(value, Information);

            if (!Result && (ResponseType == HttpResponseType.Any || ResponseType == HttpResponseType.Nginx))
                Result = ArrayContains(value, Nginx);

            if (!Result && (ResponseType == HttpResponseType.Any || ResponseType == HttpResponseType.Redirection))
                Result = ArrayContains(value, Redirect);

            if (!Result && (ResponseType == HttpResponseType.Any || ResponseType == HttpResponseType.ServerErrors))
                Result = ArrayContains(value, ServerErrors);

            if (!Result && (ResponseType == HttpResponseType.Any || ResponseType == HttpResponseType.Success))
                Result = ArrayContains(value, Success);

            if (!Result && (ResponseType == HttpResponseType.Any || ResponseType == HttpResponseType.Unnoficial))
                Result = ArrayContains(value, Unofficial);


            return (Result);
        }

        #endregion Internal Methods

        #region Private Methods

        private bool ArrayContains(in int value, in int[] array)
        {
            foreach (int val in array)
                if (val == value)
                    return true;

            return false;
        }

        #endregion Private Methods
    }

    public enum HttpResponseType
    {
        Any = 0,

        Information,

        Success,

        Redirection,

        ClientErrors,

        ServerErrors,

        Unnoficial,

        IIS,

        Nginx,

        Cloudflare,

        Custom
    }
}
