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
 *  File: EmailTest.cs
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
    public class EmailTest
    {
        public class ValidEmail
        {
            [SettingEmail]
            [SettingDefault("me@test.com")]
            public string Email { get; set; }
        }

        public class InvalidEmail
        {
            [SettingEmail]
            [SettingDefault("asdfasdf@asdf")]
            public string Email { get; set; }
        }

        public class ValidEmailMultiple
        {
            [SettingEmail(';')]
            [SettingDefault("me@test.com;me1@test.com")]
            public string Email { get; set; }
        }

        public class InvalidEmailMultiple
        {
            [SettingEmail(';')]
            [SettingDefault("me@test.com;asdfasdf@asdf")]
            public string Email { get; set; }
        }

        [TestMethod]
        public void TestValidEmailSingle ()
        {
            ValidEmail emailSettings = new ValidEmail();
            ValidEmail validEmail = ValidateSettings<ValidEmail>.Validate(emailSettings);

            Assert.IsTrue(validEmail.Email == "me@test.com");
        }

        [TestMethod]
        [ExpectedException(typeof(SettingException))]
        public void TestInvalidEmailSingle ()
        {
            InvalidEmail emailSettings = new InvalidEmail();
            InvalidEmail validEmail = ValidateSettings<InvalidEmail>.Validate(emailSettings);
        }

        [TestMethod]
        public void TestValidEmailMultiple ()
        {
            ValidEmailMultiple emailSettings = new ValidEmailMultiple();
            ValidEmailMultiple validEmail = ValidateSettings<ValidEmailMultiple>.Validate(emailSettings);

            Assert.IsTrue(validEmail.Email == "me@test.com;me1@test.com");
        }

        [TestMethod]
        [ExpectedException(typeof(SettingException))]
        public void TestInvalidEmailMultile ()
        {
            InvalidEmailMultiple emailSettings = new InvalidEmailMultiple();
            InvalidEmailMultiple validEmail = ValidateSettings<InvalidEmailMultiple>.Validate(emailSettings);
        }
    }
}
