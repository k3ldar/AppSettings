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
 *  File: NameValuePairTests.cs
 *
 *  Purpose:  Test Cases
 *
 *  Date        Name                Reason
 *  06/12/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AppSettings.Tests
{
    [TestClass]
    public class NameValuePairTests
    {
        public class NameValuePairValid
        {
            [SettingNameValuePair(';', 2, 19)]
            [SettingDefault("Database=testdb;Username=dba;Password=mypass")]
            public string Setting { get; set; }
        }

        public class NameValuePairInvalid
        {
            [SettingNameValuePair('#', 2, 19)]
            [SettingDefault("Database=testdb;Username=dba;Password=mypass")]
            public string Setting { get; set; }
        }


        [TestMethod]
        public void ValidNVP1()
        {
            NameValuePairValid nameValuePair = new NameValuePairValid();

            nameValuePair = ValidateSettings<NameValuePairValid>.Validate(nameValuePair);
        }

        [TestMethod]
        public void ValidNVP2()
        {
            NameValuePairValid nameValuePair = new NameValuePairValid()
            {
                Setting = "one=two;two=three;three=four"
            };

            nameValuePair = ValidateSettings<NameValuePairValid>.Validate(nameValuePair);
        }

        [TestMethod]
        [ExpectedException(typeof(SettingException))]
        public void InvalidNVP1()
        {
            NameValuePairInvalid nameValuePair = new NameValuePairInvalid();

            nameValuePair = ValidateSettings<NameValuePairInvalid>.Validate(nameValuePair);
        }

        [TestMethod]
        [ExpectedException(typeof(SettingException))]
        public void InvalidNVP2()
        {
            NameValuePairInvalid nameValuePair = new NameValuePairInvalid()
            {
                Setting = "one=two;two=three;three=four"
            };

            nameValuePair = ValidateSettings<NameValuePairInvalid>.Validate(nameValuePair);
        }
    }
}
