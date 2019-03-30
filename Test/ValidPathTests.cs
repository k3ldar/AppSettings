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
 *  File: ValidPathTests.cs
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
    public class ValidPathTests
    {
        public class ValidPathTest
        {
            [SettingValidPath]
            public string Value { get; set; }
        }

        [TestMethod]
        public void ValidPathTest1()
        {
            ValidPathTest test = new ValidPathTest()
            {
                Value = "C:\\"
            };

            test = ValidateSettings<ValidPathTest>.Validate(test);
        }

        [TestMethod]
        public void ValidPathTest2()
        {
            ValidPathTest test = new ValidPathTest()
            {
                Value = "..\\..\\test"
            };

            test = ValidateSettings<ValidPathTest>.Validate(test);
        }

        [TestMethod]
        public void ValidPathTest3()
        {
            ValidPathTest test = new ValidPathTest()
            {
                Value = "test"
            };

            test = ValidateSettings<ValidPathTest>.Validate(test);
        }

        [TestMethod]
        [ExpectedException(typeof(SettingException))]
        public void InvalidPathTest()
        {
            ValidPathTest test = new ValidPathTest()
            {
                Value = "..\\..\\test\\" + new string(System.IO.Path.GetInvalidPathChars())
            };

            test = ValidateSettings<ValidPathTest>.Validate(test);
        }
    }
}
