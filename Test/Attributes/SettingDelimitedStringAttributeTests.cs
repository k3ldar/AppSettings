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
 *  File: SettingDelimitedStringAttributeTests.cs
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
    public sealed class SettingDelimitedStringAttributeTests
    {
        private const string TestCategoryName = "Attribute";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Counstruct_ValidInstance_SeperatorOnlyConstructor_Success()
        {
            SettingDelimitedStringAttribute sut = new SettingDelimitedStringAttribute('\0');
            Assert.IsNotNull(sut);
            Assert.AreEqual(0u, sut.MinimumItems);
            Assert.AreEqual(100u, sut.MaximumItems);
            Assert.AreEqual('\0', sut.Delimiter);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Counstruct_ValidInstance_SeperatorMinConstructor_Success()
        {
            SettingDelimitedStringAttribute sut = new SettingDelimitedStringAttribute('#', 5);
            Assert.IsNotNull(sut);
            Assert.AreEqual(5u, sut.MinimumItems);
            Assert.AreEqual(100u, sut.MaximumItems);
            Assert.AreEqual('#', sut.Delimiter);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Counstruct_ValidInstance_SeperatorMinMaxConstructor_Success()
        {
            SettingDelimitedStringAttribute sut = new SettingDelimitedStringAttribute('|', 2, 4);
            Assert.IsNotNull(sut);
            Assert.AreEqual(2u, sut.MinimumItems);
            Assert.AreEqual(4u, sut.MaximumItems);
            Assert.AreEqual('|', sut.Delimiter);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Counstruct_MinEqualsMax_Throws_ArgumentOutOfRangeException()
        {
            new SettingDelimitedStringAttribute('|', 2, 2);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Counstruct_MinLessThanMax_Throws_ArgumentOutOfRangeException()
        {
            new SettingDelimitedStringAttribute('|', 2, 0);
        }
    }
}
