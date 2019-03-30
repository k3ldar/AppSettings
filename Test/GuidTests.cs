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
 *  Copyright (c) 2018 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AppSettings.Tests
 *  
 *  File: GuidTests.cs
 *
 *  Purpose:  Test Cases
 *
 *  Date        Name                Reason
 *  30/03/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AppSettings.Tests
{
    [TestClass]
    public class GuidTests
    {
        public class ValidGuid
        {
            [SettingGuid]
            [SettingDefault("EF418728-349A-42BB-9614-708DBA5B41AA")]
            public string Valid1 { get; set; }

            [SettingDefault("{CEF0E962-A00B-460C-9BE0-20FE62EA2B0A}")]
            public string Valid2 { get; set; }
        }

        public class InvalidGuid
        {
            [SettingGuid]
            [SettingDefault("CEF0E962-A00B-460C-9BE0-20FE62EA2XYZ")]
            public string Invalid { get; set; }
        }

        [TestMethod]
        public void TestValidGuid()
        {
            ValidGuid valid = new ValidGuid();
            ValidGuid validGuid = ValidateSettings<ValidGuid>.Validate(valid);
        }

        [TestMethod]
        [ExpectedException(typeof(SettingException))]
        public void TestInvalidEmailSingle()
        {
            InvalidGuid valid = new InvalidGuid();
            InvalidGuid validGuid = ValidateSettings<InvalidGuid>.Validate(valid);
        }
    }
}
