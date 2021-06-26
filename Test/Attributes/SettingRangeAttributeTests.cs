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
 *  File: SettingRangeAttributeTests.cs
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
    public sealed class SettingRangeAttributeTests
    {
        private const string TestCategoryName = "Attribute";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Counstruct_IntegerValues_Success()
        {
            SettingRangeAttribute sut = new SettingRangeAttribute(0, 15);
            Assert.IsNotNull(sut);
            Assert.AreEqual(0, sut.MinimumValue);
            Assert.AreEqual(15, sut.MaximumValue);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Counstruct_UnsignedIntegerValues_Success()
        {
            SettingRangeAttribute sut = new SettingRangeAttribute(12u, 365u);
            Assert.IsNotNull(sut);
            Assert.AreEqual(12u, sut.MinimumValue);
            Assert.AreEqual(365u, sut.MaximumValue);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Counstruct_FloatValues_Success()
        {
            SettingRangeAttribute sut = new SettingRangeAttribute(19f, 875f);
            Assert.IsNotNull(sut);
            Assert.AreEqual(19f, sut.MinimumValue);
            Assert.AreEqual(875f, sut.MaximumValue);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Counstruct_LongValues_Success()
        {
            SettingRangeAttribute sut = new SettingRangeAttribute(45L, 78L);
            Assert.IsNotNull(sut);
            Assert.AreEqual(45L, sut.MinimumValue);
            Assert.AreEqual(78L, sut.MaximumValue);
        }
    }
}
