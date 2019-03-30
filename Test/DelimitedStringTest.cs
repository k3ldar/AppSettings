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
 *  File: DelimitedStringTest.cs
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
    public class DelimitedStringTest
    {
        public class Delimited
        {
            [SettingDelimitedString('#', 2, 5)]
            public string Test { get; set; }
        }

        public class DelimitedInvalid
        {
            [SettingDelimitedString('#', 2)]
            [SettingDefault("test1;test2")]
            public string Test { get; set; }
        }

        [TestMethod]
        public void ValidDelimitedString()
        {
            Delimited delimited = new Delimited()
            {
                Test = "one#two#three"
            };

            delimited = ValidateSettings<Delimited>.Validate(delimited);
        }

        [TestMethod]
        [ExpectedException(typeof(SettingException))]
        public void ValidDelimitedStringTooFew()
        {
            Delimited delimited = new Delimited()
            {
                Test = "one"
            };

            delimited = ValidateSettings<Delimited>.Validate(delimited);
        }

        [TestMethod]
        [ExpectedException(typeof(SettingException))]
        public void ValidDelimitedStringTooMany()
        {
            Delimited delimited = new Delimited()
            {
                Test = "one#two#three#four#five#six#seven"
            };

            delimited = ValidateSettings<Delimited>.Validate(delimited);
        }

        [TestMethod]
        [ExpectedException(typeof(SettingException))]
        public void InvalidDelimitedString()
        {
            DelimitedInvalid delimited = new DelimitedInvalid()
            {
                Test = "one;two;three;four;five;six;seven"
            };

            delimited = ValidateSettings<DelimitedInvalid>.Validate(delimited);
        }
    }
}
