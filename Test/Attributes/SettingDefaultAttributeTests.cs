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
 *  File: SettingDefaultAttributeTests.cs
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
    public sealed class SettingDefaultAttributeTests
    {
        private const string TestCategoryName = "Attribute";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Counstruct_ValidInstance_DefaultValue_Char_Success()
        {
            SettingDefaultAttribute sut = new SettingDefaultAttribute('\t');
            Assert.IsNotNull(sut);
            Assert.AreEqual('\t', sut.DefaultValue);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Counstruct_ValidInstance_DefaultValue_String_Success()
        {
            SettingDefaultAttribute sut = new SettingDefaultAttribute("test");
            Assert.IsNotNull(sut);
            Assert.AreEqual("test", sut.DefaultValue);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Counstruct_ValidInstance_DefaultValue_Int_Success()
        {
            SettingDefaultAttribute sut = new SettingDefaultAttribute(30);
            Assert.IsNotNull(sut);
            Assert.AreEqual(30, sut.DefaultValue);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Counstruct_ValidInstance_DefaultValue_UInt_Success()
        {
            SettingDefaultAttribute sut = new SettingDefaultAttribute(5u);
            Assert.IsNotNull(sut);
            Assert.AreEqual(5u, sut.DefaultValue);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Counstruct_ValidInstance_DefaultValue_ULong_Success()
        {
            SettingDefaultAttribute sut = new SettingDefaultAttribute((ulong)15);
            Assert.IsNotNull(sut);
            Assert.AreEqual((ulong)15, sut.DefaultValue);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Counstruct_ValidInstance_DefaultValue_Long_Success()
        {
            SettingDefaultAttribute sut = new SettingDefaultAttribute(8L);
            Assert.IsNotNull(sut);
            Assert.AreEqual(8L, sut.DefaultValue);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Counstruct_ValidInstance_DefaultValue_Byte_Success()
        {
            SettingDefaultAttribute sut = new SettingDefaultAttribute((byte)34);
            Assert.IsNotNull(sut);
            Assert.AreEqual((byte)34, sut.DefaultValue);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Counstruct_ValidInstance_DefaultValue_Decimal_Success()
        {
            SettingDefaultAttribute sut = new SettingDefaultAttribute((decimal)21);
            Assert.IsNotNull(sut);
            Assert.AreEqual((decimal)21, sut.DefaultValue);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Counstruct_ValidInstance_DefaultValue_Float_Success()
        {
            SettingDefaultAttribute sut = new SettingDefaultAttribute(9f);
            Assert.IsNotNull(sut);
            Assert.AreEqual(9f, sut.DefaultValue);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Counstruct_ValidInstance_DefaultValue_Double_Success()
        {
            SettingDefaultAttribute sut = new SettingDefaultAttribute(3d);
            Assert.IsNotNull(sut);
            Assert.AreEqual(3d, sut.DefaultValue);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Counstruct_ValidInstance_DefaultValue_Bool_Success()
        {
            SettingDefaultAttribute sut = new SettingDefaultAttribute(true);
            Assert.IsNotNull(sut);
            Assert.AreEqual(true, sut.DefaultValue);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Counstruct_ValidInstance_DefaultValue_Short_Success()
        {
            SettingDefaultAttribute sut = new SettingDefaultAttribute((short)4);
            Assert.IsNotNull(sut);
            Assert.AreEqual((short)4, sut.DefaultValue);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Counstruct_ValidInstance_DefaultValue_UShort_Success()
        {
            SettingDefaultAttribute sut = new SettingDefaultAttribute((ushort)6);
            Assert.IsNotNull(sut);
            Assert.AreEqual((ushort)6, sut.DefaultValue);
        }
    }
}
