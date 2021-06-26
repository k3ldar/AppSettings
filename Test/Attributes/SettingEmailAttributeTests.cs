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
 *  File: SettingEmailAttributeTests.cs
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
    public sealed class SettingEmailAttributeTests
    {
        private const string TestCategoryName = "Attribute";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Counstruct_ValidInstance_DefaultConstructor_Success()
        {
            SettingEmailAttribute sut = new SettingEmailAttribute();
            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.AllowMultiple);
            Assert.AreEqual('\0', sut.SeperatorChar);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Counstruct_InvalidInstance_CharSepConstructor_InvalidChar_Throws_ArgumentOutOfRangeException()
        {
            new SettingEmailAttribute(':');
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Counstruct_ValidInstance_CharSepConstructor_Semicolon_Success()
        {
            SettingEmailAttribute sut = new SettingEmailAttribute(';');
            Assert.IsNotNull(sut);
            Assert.IsTrue(sut.AllowMultiple);
            Assert.AreEqual(';', sut.SeperatorChar);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Counstruct_ValidInstance_CharSepConstructor_Hash_Success()
        {
            SettingEmailAttribute sut = new SettingEmailAttribute('#');
            Assert.IsNotNull(sut);
            Assert.IsTrue(sut.AllowMultiple);
            Assert.AreEqual('#', sut.SeperatorChar);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Counstruct_ValidInstance_CharSepConstructor_Tab_Success()
        {
            SettingEmailAttribute sut = new SettingEmailAttribute('\t');
            Assert.IsNotNull(sut);
            Assert.IsTrue(sut.AllowMultiple);
            Assert.AreEqual('\t', sut.SeperatorChar);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Counstruct_ValidInstance_CharSepConstructor_NewLine_Success()
        {
            SettingEmailAttribute sut = new SettingEmailAttribute('\n');
            Assert.IsNotNull(sut);
            Assert.IsTrue(sut.AllowMultiple);
            Assert.AreEqual('\n', sut.SeperatorChar);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Counstruct_ValidInstance_CharSepConstructor_CarriageReturn_Success()
        {
            SettingEmailAttribute sut = new SettingEmailAttribute('\r');
            Assert.IsNotNull(sut);
            Assert.IsTrue(sut.AllowMultiple);
            Assert.AreEqual('\r', sut.SeperatorChar);
        }
    }
}