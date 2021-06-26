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
 *  File: SettingExceptionTests.cs
 *
 *  Purpose:  Attribute validation
 *
 *  Date        Name                Reason
 *  24/06/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics.CodeAnalysis;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AppSettings.Tests.Attributes
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public sealed class SettingExceptionTests
    {
        private const string TestCategoryName = "Exceptions";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Counstruct_ValidInstance_DefaultConstructor_Success()
        {
            SettingException sut = new SettingException();
            Assert.IsNotNull(sut);
            Assert.IsNull(sut.PropertyName);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Counstruct_InvalidInstance_PropertyNameNull_Throws_ArgumentNullException()
        {
            new SettingException(null, "message");
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Counstruct_InvalidInstance_PropertyNameEmptyString_Throws_ArgumentNullException()
        {
            new SettingException("", "message");
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Counstruct_ValidInstance_WithPropertyName_Success()
        {
            SettingException sut = new SettingException("My Property", null);
            Assert.IsNotNull(sut);
            Assert.IsNotNull(sut.PropertyName);
            Assert.AreEqual("My Property", sut.PropertyName);
        }
    }
}
