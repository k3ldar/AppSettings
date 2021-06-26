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
 *  File: SettingRegexAttributeTests.cs
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
    public sealed class SettingRegexAttributeTests
    {
        private const string TestCategoryName = "Attribute";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Counstruct_InvalidParam_Null_Throws_ArgumentNullException()
        {
            new SettingRegexAttribute(null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Counstruct_InvalidParam_EmptyString_Throws_ArgumentNullException()
        {
            new SettingRegexAttribute(null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Counstruct_ValidInstance_Success()
        {
            SettingRegexAttribute sut = new SettingRegexAttribute("regex text");
            Assert.IsNotNull(sut);
            Assert.AreEqual("regex text", sut.Regex);
        }
    }
}
