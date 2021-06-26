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
 *  File: SettingNameValuePairAttributeTests.cs
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
    public sealed class SettingNameValuePairAttributeTests
    {
        private const string TestCategoryName = "Attribute";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Counstruct_ValidInstance_SeperatorOnlyConstructor_Success()
        {
            SettingNameValuePairAttribute sut = new SettingNameValuePairAttribute('\0');
            Assert.IsNotNull(sut);
            Assert.AreEqual(0u, sut.MinimumItems);
            Assert.AreEqual(100u, sut.MaximumItems);
            Assert.AreEqual('\0', sut.Delimiter);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Counstruct_ValidInstance_SeperatorAndMinConstructor_Success()
        {
            SettingNameValuePairAttribute sut = new SettingNameValuePairAttribute('\0', 5);
            Assert.IsNotNull(sut);
            Assert.AreEqual(5u, sut.MinimumItems);
            Assert.AreEqual(100u, sut.MaximumItems);
            Assert.AreEqual('\0', sut.Delimiter);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Counstruct_ValidInstance_MinMaxConstructor_Success()
        {
            SettingNameValuePairAttribute sut = new SettingNameValuePairAttribute(1, 15);
            Assert.IsNotNull(sut);
            Assert.AreEqual(1u, sut.MinimumItems);
            Assert.AreEqual(15u, sut.MaximumItems);
            Assert.AreEqual(';', sut.Delimiter);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Counstruct_ValidInstance_DelimiterMinMaxConstructor_Success()
        {
            SettingNameValuePairAttribute sut = new SettingNameValuePairAttribute('\t', 0, 8);
            Assert.IsNotNull(sut);
            Assert.AreEqual(0u, sut.MinimumItems);
            Assert.AreEqual(8u, sut.MaximumItems);
            Assert.AreEqual('\t', sut.Delimiter);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Counstruct_ValidInstance_MaxLessThanMin_Throws_ArgumentOutOfRangeException()
        {
            new SettingNameValuePairAttribute('\t', 8, 5);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Counstruct_ValidInstance_MaxEqualsMin_Throws_ArgumentOutOfRangeException()
        {
            new SettingNameValuePairAttribute('\t', 6, 6);
        }
    }
}
