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
 *  File: OptionalTests.cs
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
    public class OptionalTests
    {
        public class ValidEmail
        {
            [SettingEmail]
            [SettingOptional]
            public string Email { get; set; }
        }

        [TestMethod]
        public void TestValidOptional1()
        {
            ValidEmail emailSettings = new ValidEmail();
            emailSettings = ValidateSettings<ValidEmail>.Validate(emailSettings);

            Assert.IsTrue(String.IsNullOrEmpty(emailSettings.Email));
        }

        [TestMethod]
        public void TestValidOptional2()
        {
            ValidEmail emailSettings = new ValidEmail()
            {
                Email = "me@here.com"
            };

            emailSettings = ValidateSettings<ValidEmail>.Validate(emailSettings);

            Assert.IsTrue(emailSettings.Email == "me@here.com");
        }

        [TestMethod]
        [ExpectedException(typeof(SettingException))]
        public void TestInvalidOptional1()
        {
            ValidEmail emailSettings = new ValidEmail()
            {
                Email = "mehere.com"
            };

            emailSettings = ValidateSettings<ValidEmail>.Validate(emailSettings);
        }
    }
}
