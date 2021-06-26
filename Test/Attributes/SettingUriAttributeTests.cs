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
 *  File: SettingUriAttributeTests.cs
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
    public sealed class SettingUriAttributeTests
    {
        private const string TestCategoryName = "Attribute";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Counstruct_ValidInstance_EmptyConstructor_Success()
        {
            SettingUriAttribute sut = new SettingUriAttribute();
            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.ValidateEndPoint);
            Assert.AreEqual(UriKind.Absolute, sut.UriKind);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Counstruct_ValidInstance_ValidateEndpointConstructor_Success()
        {
            SettingUriAttribute sut = new SettingUriAttribute(true);
            Assert.IsNotNull(sut);
            Assert.IsTrue(sut.ValidateEndPoint);
            Assert.AreEqual(UriKind.Absolute, sut.UriKind);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Counstruct_ValidInstance_ValidateAndUriKindEndpointConstructor_Success()
        {
            SettingUriAttribute sut = new SettingUriAttribute(true, UriKind.RelativeOrAbsolute);
            Assert.IsNotNull(sut);
            Assert.IsTrue(sut.ValidateEndPoint);
            Assert.AreEqual(UriKind.RelativeOrAbsolute, sut.UriKind);
        }
    }
}
