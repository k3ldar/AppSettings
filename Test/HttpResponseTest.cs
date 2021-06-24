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
 *  Product:  AppSettings.Tests
 *  
 *  File: HttpResponseTest.cs
 *
 *  Purpose:  Test Cases
 *
 *  Date        Name                Reason
 *  06/12/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Diagnostics.CodeAnalysis;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AppSettings.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class HttpResponseTest
    {
        public class ResponseClientError
        {
            [SettingHttpResponse(HttpResponseType.ClientErrors)]
            public int ResponseCode { get; set; }
        }

        public class ResponseServerError
        {
            [SettingHttpResponse(HttpResponseType.ServerErrors)]
            public int ResponseCode { get; set; }
        }

        public class ResponseServerCustom
        {
            [SettingHttpResponse(new int[] { 401, 500, 200, 219, 999 })]
            public int ResponseCode { get; set; }
        }


        [TestMethod]
        public void ValidResponse1()
        {
            ResponseClientError response = new ResponseClientError
            {
                ResponseCode = 400
            };

            response = ValidateSettings<ResponseClientError>.Validate(response);
        }

        [TestMethod]
        public void ValidResponse2()
        {
            ResponseClientError response = new ResponseClientError
            {
                ResponseCode = 429
            };

            response = ValidateSettings<ResponseClientError>.Validate(response);
        }

        [TestMethod]
        public void ValidResponse3()
        {
            ResponseClientError response = new ResponseClientError
            {
                ResponseCode = 404
            };

            response = ValidateSettings<ResponseClientError>.Validate(response);
        }

        [TestMethod]
        public void ValidResponse4()
        {
            ResponseServerError response = new ResponseServerError
            {
                ResponseCode = 500
            };

            response = ValidateSettings<ResponseServerError>.Validate(response);
        }

        [TestMethod]
        public void ValidResponse5()
        {
            ResponseServerError response = new ResponseServerError
            {
                ResponseCode = 501
            };

            response = ValidateSettings<ResponseServerError>.Validate(response);
        }

        [TestMethod]
        public void ValidResponse6()
        {
            ResponseServerError response = new ResponseServerError
            {
                ResponseCode = 502
            };

            response = ValidateSettings<ResponseServerError>.Validate(response);
        }

        [TestMethod]
        [ExpectedException(typeof(SettingException))]
        public void InvalidResponse1()
        {
            ResponseServerError response = new ResponseServerError
            {
                ResponseCode = 404
            };

            response = ValidateSettings<ResponseServerError>.Validate(response);
        }

        [TestMethod]
        [ExpectedException(typeof(SettingException))]
        public void InvalidResponse2()
        {
            ResponseClientError response = new ResponseClientError
            {
                ResponseCode = 200
            };

            response = ValidateSettings<ResponseClientError>.Validate(response);
        }

        [TestMethod]
        [ExpectedException(typeof(SettingException))]
        public void InvalidResponse3()
        {
            ResponseServerCustom response = new ResponseServerCustom
            {
                ResponseCode = 502
            };

            response = ValidateSettings<ResponseServerCustom>.Validate(response);
        }

        [TestMethod]
        public void ValidResponse7()
        {
            ResponseServerCustom response = new ResponseServerCustom
            {
                ResponseCode = 500
            };

            response = ValidateSettings<ResponseServerCustom>.Validate(response);
        }

        [TestMethod]
        public void ValidResponse8()
        {
            ResponseServerCustom response = new ResponseServerCustom
            {
                ResponseCode = 999
            };

            response = ValidateSettings<ResponseServerCustom>.Validate(response);
        }
    }
}
