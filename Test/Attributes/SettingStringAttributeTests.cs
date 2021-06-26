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
 *  File: SettingStringAttributeTests.cs
 *
 *  Purpose:  Attribute validation
 *
 *  Date        Name                Reason
 *  24/06/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Diagnostics.CodeAnalysis;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AppSettings.Tests.Attributes
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public sealed class SettingStringAttributeTests
    {
        private const string TestCategoryName = "Attribute";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Counstruct_ValidInstance_MinMaxLengthConstructor_Success()
        {
            SettingStringAttribute sut = new SettingStringAttribute(15, 23);
            Assert.IsNotNull(sut);
            Assert.AreEqual(15u, sut.MinLength);
            Assert.AreEqual(23u, sut.MaxLength);
            Assert.IsFalse(sut.AllowNullOrEmpty);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Counstruct_ValidInstance_AllowNullEmptyConstructor_Success()
        {
            SettingStringAttribute sut = new SettingStringAttribute(true);
            Assert.IsNotNull(sut);
            Assert.AreEqual(1u, sut.MinLength);
            Assert.AreEqual(500u, sut.MaxLength);
            Assert.IsTrue(sut.AllowNullOrEmpty);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Counstruct_ValidInstance_FullConstructor_Success()
        {
            SettingStringAttribute sut = new SettingStringAttribute(true, 19, 87);
            Assert.IsNotNull(sut);
            Assert.AreEqual(19u, sut.MinLength);
            Assert.AreEqual(87u, sut.MaxLength);
            Assert.IsTrue(sut.AllowNullOrEmpty);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void Counstruct_ValidInstance_MaxLengthGreaterThanMinLength_Throws_ArgumentOutOfRangeException()
        {
            new SettingStringAttribute(true, 81, 16);
        }
    }
}
