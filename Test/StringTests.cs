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
 *  File: StringTests.cs
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
    public class StringTests
    {
        public class StringTestNull
        {
            [SettingString(true)]
            public string Value { get; set; }
        }

        public class StringTestNotNull
        {
            [SettingString(false)]
            public string Value { get; set; }
        }

        public class StringTestNotNullLength
        {
            [SettingString(5, 10)]
            public string Value { get; set; }
        }

        public class StringEnvironmentVariable
        {
            [SettingDefault("%GeoIpKey%")]
            public string Value { get; set; }

            [SettingDefault("%connstandard%")]
            public string Standard { get; set; }
        }

        [TestMethod]
        public void NullStringValid()
        {
            StringTestNull test = new StringTestNull()
            {
                Value = null
            };

            test = ValidateSettings<StringTestNull>.Validate(test);
        }

        [TestMethod]
        [ExpectedException(typeof(SettingException))]
        public void NullStringInvalid()
        {
            StringTestNotNull test = new StringTestNotNull()
            {
                Value = null
            };

            test = ValidateSettings<StringTestNotNull>.Validate(test);
        }

        [TestMethod]
        public void ValidStringLength()
        {
            StringTestNotNullLength test = new StringTestNotNullLength()
            {
                Value = "testing"
            };

            test = ValidateSettings<StringTestNotNullLength>.Validate(test);
        }

        [TestMethod]
        [ExpectedException(typeof(SettingException))]
        public void InvalidStringLength1()
        {
            StringTestNotNullLength test = new StringTestNotNullLength()
            {
                Value = "test"
            };

            test = ValidateSettings<StringTestNotNullLength>.Validate(test);
        }

        [TestMethod]
        [ExpectedException(typeof(SettingException))]
        public void InvalidStringLength2()
        {
            StringTestNotNullLength test = new StringTestNotNullLength()
            {
                Value = "test string too long"
            };

            test = ValidateSettings<StringTestNotNullLength>.Validate(test);
        }

        [TestMethod]
        public void RetrieveEnvironmentalVariable()
        {
            StringEnvironmentVariable environmentVariable = new StringEnvironmentVariable();

            environmentVariable = ValidateSettings<StringEnvironmentVariable>.Validate(environmentVariable);

            Assert.AreEqual(environmentVariable.Value, "123456789");
        }
    }
}
