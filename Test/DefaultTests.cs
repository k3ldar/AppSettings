﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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
 *  File: DefaultTests.cs
 *
 *  Purpose:  Test Cases
 *
 *  Date        Name                Reason
 *  06/12/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace AppSettings.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class DefaultTests
    {
        public class DefaultTestString
        {
            [SettingDefault("hello")]
            public string Value { get; set; }
        }

        public class DefaultTestStringEnvVar
        {
            [SettingDefault("%System%")]
            public string Value { get; set; }
        }

        public class DefaultTestStringAppPath
        {
            [SettingDefault("%AppPath%")]
            public string Value { get; set; }
        }

        public class DefaultTestStringSpecialFolder
        {
            [SettingDefault("%CommonMusic%")]
            public string Value { get; set; }
        }

        public class DefaultIntZeroWithinRange
        {
            [SettingDefault(1)]
            [SettingRange(0, 10)]
            public int Value { get; set; }
        }

        [TestMethod]
        public void ValidateDefaultIntWithRange()
        {
            DefaultIntZeroWithinRange test = new DefaultIntZeroWithinRange();
            test = ValidateSettings<DefaultIntZeroWithinRange>.Validate(test);
            Assert.AreEqual(0, test.Value);
        }

        [TestMethod]
        public void ValidDefault()
        {
            DefaultTestString test = new DefaultTestString();

            test = ValidateSettings<DefaultTestString>.Validate(test);

            Assert.AreEqual(test.Value, "hello");
        }

        [TestMethod]
        public void ValidDefaultEnvVar()
        {
            //System.Environment.SetEnvironmentVariable("GeoIpKey", "123456789");
            //System.Environment.SetEnvironmentVariable("connstandard", "the connection string");
            DefaultTestStringEnvVar test = new DefaultTestStringEnvVar();

            test = ValidateSettings<DefaultTestStringEnvVar>.Validate(test);

            Assert.AreEqual(test.Value, "C:\\WINDOWS\\system32", true);
        }

        [TestMethod]
        public void ValidDefaultAppPath()
        {
            DefaultTestStringAppPath test = new DefaultTestStringAppPath();

            test = ValidateSettings<DefaultTestStringAppPath>.Validate(test);

            Assert.IsTrue(test.Value.EndsWith("AppSettings\\Test\\bin\\Debug\\net5.0"));
        }

        [TestMethod]
        public void ValidDefaultSpecialFolder()
        {
            DefaultTestStringSpecialFolder test = new DefaultTestStringSpecialFolder();

            test = ValidateSettings<DefaultTestStringSpecialFolder>.Validate(test);

            Assert.AreEqual(test.Value, "C:\\Users\\Public\\Music");
        }

    }
}
