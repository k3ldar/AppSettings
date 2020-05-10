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
 *  File: UriTests.cs
 *
 *  Purpose:  Test Cases
 *
 *  Date        Name                Reason
 *  06/12/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace AppSettings.Tests
{
    [TestClass]
    public class UriTests
    {
        public class UriTest
        {
            [SettingUri(false, UriKind.Absolute)]
            public string Value { get; set; }
        }

        public class UriEndpointTest
        {
            [SettingUri(true, UriKind.Absolute)]
            public string Value { get; set; }
        }

        public class UriRelativeTest
        {
            [SettingUri(false, UriKind.Relative)]
            public string Value { get; set; }
        }

        [TestMethod]
        public void ValidPartialUri()
        {
            UriRelativeTest test = new UriRelativeTest()
            {
                Value = "/Help"
            };

            test = ValidateSettings<UriRelativeTest>.Validate(test);
        }

        [TestMethod]
        public void ValidUriEndpoint()
        {
            UriEndpointTest test = new UriEndpointTest()
            {
                Value = "http://www.google.com"
            };

            test = ValidateSettings<UriEndpointTest>.Validate(test);
        }

        [TestMethod]
        [ExpectedException(typeof(SettingException))]
        public void InvalidUriEndpoint()
        {
            UriEndpointTest test = new UriEndpointTest()
            {
                Value = "http://www.thereshouldbenodomainhere.com"
            };

            test = ValidateSettings<UriEndpointTest>.Validate(test);
        }

        [TestMethod]
        public void ValidUri()
        {
            // can not perform this test as my router intervienes
            UriTest test = new UriTest()
            {
                Value = "http://www.microsoft.com"
            };

            test = ValidateSettings<UriTest>.Validate(test);
        }

        [TestMethod]
        [ExpectedException(typeof(SettingException))]
        public void InvalidUri()
        {
            UriTest test = new UriTest()
            {
                Value = "not a / uri.com"
            };

            test = ValidateSettings<UriTest>.Validate(test);
        }
    }
}
