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
 *  Product:  AppSettings
 *  
 *  File: ValidateSettingsTests.cs
 *
 *  Purpose:  Validate settings tests
 *
 *  Date        Name                Reason
 *  25/06/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AppSettings.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ValidateSettingsTests
    {
        private const string TestCategoryName = "Validation Settings";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Validate_NullSettings_Throws_ArgumentNullException()
        {
            ValidateSettings<string>.Validate(null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Validate_OverrideSettings_Success()
        {
            TestSettingOverride tso = new TestSettingOverride("MyValue", "A string", true);
            TestSettingValues sut = ValidateSettings<TestSettingValues>.Validate(new TestSettingValues(), tso);
            Assert.IsNotNull(sut);

            Assert.IsTrue(tso.OverrideCalled);
            
            Assert.AreEqual("A string", sut.MyValue);
            Assert.IsNull(sut.UnsetValue);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Validate_SettingError_Success()
        {
            TestSettingError tse = new TestSettingError();
            TestSettingErrorValues sut = ValidateSettings<TestSettingErrorValues>.Validate(new TestSettingErrorValues(), tse);
            Assert.IsNotNull(sut);
            Assert.IsTrue(tse.ErrorsRaised.ContainsKey("PathNotExists"));
            Assert.IsTrue(tse.ErrorsRaised.ContainsKey("PathInvalidChars"));
            Assert.IsTrue(sut.ValidateSettingsCalled);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Validate_ApplicationOverride_Success()
        {
            TestSettingError tse = new TestSettingError();
            TestAppOverride tao = new TestAppOverride();
            tao.AppOverrides.Add("MyIntValue", 123);
            tao.AppOverrides.Add("DefaultUser", "Joe Bloggs");
            TestAppOverrideValues sut = ValidateSettings<TestAppOverrideValues>.Validate(new TestAppOverrideValues(), null, tse, tao);
            Assert.IsNotNull(sut);
            Assert.AreEqual(0, sut.IntValueNotFound);
            Assert.AreEqual(123, sut.IntValueFound);
            Assert.AreEqual("Joe Bloggs", sut.DefaultUser);
            Assert.AreEqual("", sut.Password);
            Assert.IsTrue(tse.ErrorsRaised.ContainsKey("IntValueNotFound"));
        }
    }

    [ExcludeFromCodeCoverage]
    internal class TestAppOverrideValues
    {
        [SettingDefault("%MyIntValue%")]
        public int IntValueFound { get; set; }

        [SettingDefault("%MyIntValueNotFound%")]
        public int IntValueNotFound { get; set; }

        [SettingDefault("%DefaultUser%")]
        public string DefaultUser { get; set; }

        [SettingDefault("%Password%")]
        public string Password { get; set; }
    }

    [ExcludeFromCodeCoverage]
    internal class TestAppOverride : IApplicationOverride
    {
        public TestAppOverride()
        {
            AppOverrides = new Dictionary<string, object>();
        }

        public Dictionary<string, object> AppOverrides { get; }

        public bool ExpandApplicationVariable(string variableName, ref object value)
        {
            if (AppOverrides.ContainsKey(variableName))
            {
                value = AppOverrides[variableName];
                return true;
            }

            return false;
        }
    }

    [ExcludeFromCodeCoverage]
    internal class TestSettingValues
    { 
        public string MyValue { get; set; }

        [SettingString(true)]
        public string UnsetValue { get; set; }
    }

    [ExcludeFromCodeCoverage]
    internal class TestSettingErrorValues
    {
        public bool ValidateSettingsCalled { get; private set; }

        [SettingDefault("z:\\non existent path")]
        [SettingPathExists]
        public string PathNotExists { get; set; }

        [SettingDefault("z:\\<non existent path|")]
        [SettingPathExists]
        public string PathInvalidChars { get; set; }


        public void ValidateSettings()
        {
            ValidateSettingsCalled = true;
        }
    }

    [ExcludeFromCodeCoverage]
    internal class TestSettingError : ISettingError
    {
        public TestSettingError()
        {
            ErrorsRaised = new Dictionary<string, string>();
        }
        public Dictionary<string, string> ErrorsRaised { get; }

        public void SettingError(in string propertyName, in string message)
        {
            ErrorsRaised.Add(propertyName, message);
        }
    }

    [ExcludeFromCodeCoverage]
    internal class TestSettingOverride : ISettingOverride
    {
        private readonly string _propertyNameToOverride;
        private readonly object _propertyValue;
        private readonly bool _hasOverride;

        public TestSettingOverride(string settingName, object propertyValue, bool hasOverridden)
        {
            _propertyNameToOverride = settingName;
            _propertyValue = propertyValue;
            _hasOverride = hasOverridden;
        }

        public bool OverrideSettingValue(in string settingName, ref object propertyValue)
        {
            OverrideCalled = true;
            OverrideSettingName = settingName;
            if (settingName.Equals(_propertyNameToOverride))
            {
                propertyValue = _propertyValue;
                return true;
            }

            return false;
        }

        public bool OverrideCalled { get; private set; }

        public string OverrideSettingName { get; private set; }
    }
}
