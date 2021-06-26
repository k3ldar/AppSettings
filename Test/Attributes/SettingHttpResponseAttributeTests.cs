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
 *  File: SettingHttpResponseAttributeTests.cs
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
    public sealed class SettingHttpResponseAttributeTests
    {
        private const string TestCategoryName = "Attribute";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_DefaultConstructor_ResponseTypeAny()
        {
            SettingHttpResponseAttribute sut = new SettingHttpResponseAttribute();
            Assert.IsNotNull(sut);
            Assert.AreEqual(HttpResponseType.Any, sut.ResponseType);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Construct_ResponseTypeConstructor_ResponseTypeCustom_Throws_ArgumentOutOfRangeException()
        {
            new SettingHttpResponseAttribute(HttpResponseType.Custom);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_ResponseTypeConstructor_ResponseTypeSuccess()
        {
            SettingHttpResponseAttribute sut = new SettingHttpResponseAttribute(HttpResponseType.Success);
            Assert.IsNotNull(sut);
            Assert.AreEqual(HttpResponseType.Success, sut.ResponseType);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentException))]
        public void Construct_ValidCodesConstructor_ZeroLengthArray_Throws_ArgumentException()
        {
            new SettingHttpResponseAttribute(new int[] { });
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_ValidCodesConstructor_NullArray_Throws_ArgumentNullException()
        {
            new SettingHttpResponseAttribute(null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_ValidCodesConstructor_ValidArray_Success()
        {
            SettingHttpResponseAttribute sut = new SettingHttpResponseAttribute(new int[] { 21, 87, 180 });
            Assert.IsNotNull(sut);
            Assert.AreEqual(HttpResponseType.Custom, sut.ResponseType);
            Assert.AreEqual(3, sut.ValidCodes.Length);
            Assert.AreEqual(21, sut.ValidCodes[0]);
            Assert.AreEqual(87, sut.ValidCodes[1]);
            Assert.AreEqual(180, sut.ValidCodes[2]);
            Assert.IsTrue(sut.ValidateResponseCode(21));
            Assert.IsTrue(sut.ValidateResponseCode(87));
            Assert.IsTrue(sut.ValidateResponseCode(180));
            Assert.IsFalse(sut.ValidateResponseCode(15));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidateResponseCode_ClientErrors_ResponseTypeClientErrors_Success()
        {
            SettingHttpResponseAttribute sut = new SettingHttpResponseAttribute(HttpResponseType.ClientErrors);
            Assert.IsNotNull(sut);
            Assert.AreEqual(HttpResponseType.ClientErrors, sut.ResponseType);

            for (int i = 400; i < 419; i++)
                Assert.IsTrue(sut.ValidateResponseCode(i));

            Assert.IsTrue(sut.ValidateResponseCode(421));
            Assert.IsTrue(sut.ValidateResponseCode(426));
            Assert.IsTrue(sut.ValidateResponseCode(428));
            Assert.IsTrue(sut.ValidateResponseCode(429));
            Assert.IsTrue(sut.ValidateResponseCode(431));
            Assert.IsTrue(sut.ValidateResponseCode(451));

            Assert.IsFalse(sut.ValidateResponseCode(440));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidateResponseCode_ClientErrors_ResponseTypeClientAny_Success()
        {
            SettingHttpResponseAttribute sut = new SettingHttpResponseAttribute(HttpResponseType.Any);
            Assert.IsNotNull(sut);
            Assert.AreEqual(HttpResponseType.Any, sut.ResponseType);

            for (int i = 400; i < 419; i++)
                Assert.IsTrue(sut.ValidateResponseCode(i));

            Assert.IsTrue(sut.ValidateResponseCode(421));
            Assert.IsTrue(sut.ValidateResponseCode(426));
            Assert.IsTrue(sut.ValidateResponseCode(428));
            Assert.IsTrue(sut.ValidateResponseCode(429));
            Assert.IsTrue(sut.ValidateResponseCode(431));
            Assert.IsTrue(sut.ValidateResponseCode(451));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidateResponseCode_Cloudflare_ResponseTypeClloudflare_Success()
        {
            SettingHttpResponseAttribute sut = new SettingHttpResponseAttribute(HttpResponseType.Cloudflare);
            Assert.IsNotNull(sut);
            Assert.AreEqual(HttpResponseType.Cloudflare, sut.ResponseType);

            for (int i = 520; i < 528; i++)
                Assert.IsTrue(sut.ValidateResponseCode(i));

            Assert.IsTrue(sut.ValidateResponseCode(530));

            Assert.IsFalse(sut.ValidateResponseCode(440));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidateResponseCode_Cloudflare_ResponseTypeClientAny_Success()
        {
            SettingHttpResponseAttribute sut = new SettingHttpResponseAttribute(HttpResponseType.Any);
            Assert.IsNotNull(sut);
            Assert.AreEqual(HttpResponseType.Any, sut.ResponseType);


            for (int i = 520; i < 528; i++)
                Assert.IsTrue(sut.ValidateResponseCode(i));

            Assert.IsTrue(sut.ValidateResponseCode(530));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidateResponseCode_IIS_ResponseTypeIIS_Success()
        {
            SettingHttpResponseAttribute sut = new SettingHttpResponseAttribute(HttpResponseType.IIS);
            Assert.IsNotNull(sut);
            Assert.AreEqual(HttpResponseType.IIS, sut.ResponseType);

            Assert.IsTrue(sut.ValidateResponseCode(440));
            Assert.IsTrue(sut.ValidateResponseCode(449));
            Assert.IsTrue(sut.ValidateResponseCode(451));

            Assert.IsFalse(sut.ValidateResponseCode(404));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidateResponseCode_IIS_ResponseTypeClientAny_Success()
        {
            SettingHttpResponseAttribute sut = new SettingHttpResponseAttribute(HttpResponseType.Any);
            Assert.IsNotNull(sut);
            Assert.AreEqual(HttpResponseType.Any, sut.ResponseType);

            Assert.IsTrue(sut.ValidateResponseCode(440));
            Assert.IsTrue(sut.ValidateResponseCode(449));
            Assert.IsTrue(sut.ValidateResponseCode(451));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidateResponseCode_Information_ResponseTypeInformation_Success()
        {
            SettingHttpResponseAttribute sut = new SettingHttpResponseAttribute(HttpResponseType.Information);
            Assert.IsNotNull(sut);
            Assert.AreEqual(HttpResponseType.Information, sut.ResponseType);

            Assert.IsTrue(sut.ValidateResponseCode(100));
            Assert.IsTrue(sut.ValidateResponseCode(101));
            Assert.IsTrue(sut.ValidateResponseCode(102));
            Assert.IsTrue(sut.ValidateResponseCode(103));

            Assert.IsFalse(sut.ValidateResponseCode(104));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidateResponseCode_Information_ResponseTypeClientAny_Success()
        {
            SettingHttpResponseAttribute sut = new SettingHttpResponseAttribute(HttpResponseType.Any);
            Assert.IsNotNull(sut);
            Assert.AreEqual(HttpResponseType.Any, sut.ResponseType);

            Assert.IsTrue(sut.ValidateResponseCode(100));
            Assert.IsTrue(sut.ValidateResponseCode(101));
            Assert.IsTrue(sut.ValidateResponseCode(102));
            Assert.IsTrue(sut.ValidateResponseCode(103));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidateResponseCode_Nginx_ResponseTypeInformation_Success()
        {
            SettingHttpResponseAttribute sut = new SettingHttpResponseAttribute(HttpResponseType.Nginx);
            Assert.IsNotNull(sut);
            Assert.AreEqual(HttpResponseType.Nginx, sut.ResponseType);

            Assert.IsTrue(sut.ValidateResponseCode(444));
            Assert.IsTrue(sut.ValidateResponseCode(494));
            Assert.IsTrue(sut.ValidateResponseCode(495));
            Assert.IsTrue(sut.ValidateResponseCode(496));
            Assert.IsTrue(sut.ValidateResponseCode(497));
            Assert.IsTrue(sut.ValidateResponseCode(499));

            Assert.IsFalse(sut.ValidateResponseCode(498));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidateResponseCode_Nginx_ResponseTypeClientAny_Success()
        {
            SettingHttpResponseAttribute sut = new SettingHttpResponseAttribute(HttpResponseType.Any);
            Assert.IsNotNull(sut);
            Assert.AreEqual(HttpResponseType.Any, sut.ResponseType);

            Assert.IsTrue(sut.ValidateResponseCode(444));
            Assert.IsTrue(sut.ValidateResponseCode(494));
            Assert.IsTrue(sut.ValidateResponseCode(495));
            Assert.IsTrue(sut.ValidateResponseCode(496));
            Assert.IsTrue(sut.ValidateResponseCode(497));
            Assert.IsTrue(sut.ValidateResponseCode(499));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidateResponseCode_Redirection_ResponseTypeInformation_Success()
        {
            SettingHttpResponseAttribute sut = new SettingHttpResponseAttribute(HttpResponseType.Redirection);
            Assert.IsNotNull(sut);
            Assert.AreEqual(HttpResponseType.Redirection, sut.ResponseType);

            Assert.IsTrue(sut.ValidateResponseCode(300));
            Assert.IsTrue(sut.ValidateResponseCode(301));
            Assert.IsTrue(sut.ValidateResponseCode(302));
            Assert.IsTrue(sut.ValidateResponseCode(303));
            Assert.IsTrue(sut.ValidateResponseCode(304));
            Assert.IsTrue(sut.ValidateResponseCode(305));
            Assert.IsTrue(sut.ValidateResponseCode(306));
            Assert.IsTrue(sut.ValidateResponseCode(307));
            Assert.IsTrue(sut.ValidateResponseCode(308));

            Assert.IsFalse(sut.ValidateResponseCode(201));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidateResponseCode_Redirection_ResponseTypeClientAny_Success()
        {
            SettingHttpResponseAttribute sut = new SettingHttpResponseAttribute(HttpResponseType.Any);
            Assert.IsNotNull(sut);
            Assert.AreEqual(HttpResponseType.Any, sut.ResponseType);

            Assert.IsTrue(sut.ValidateResponseCode(300));
            Assert.IsTrue(sut.ValidateResponseCode(301));
            Assert.IsTrue(sut.ValidateResponseCode(302));
            Assert.IsTrue(sut.ValidateResponseCode(303));
            Assert.IsTrue(sut.ValidateResponseCode(304));
            Assert.IsTrue(sut.ValidateResponseCode(305));
            Assert.IsTrue(sut.ValidateResponseCode(306));
            Assert.IsTrue(sut.ValidateResponseCode(307));
            Assert.IsTrue(sut.ValidateResponseCode(308));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidateResponseCode_ServerErrors_ResponseTypeInformation_Success()
        {
            SettingHttpResponseAttribute sut = new SettingHttpResponseAttribute(HttpResponseType.ServerErrors);
            Assert.IsNotNull(sut);
            Assert.AreEqual(HttpResponseType.ServerErrors, sut.ResponseType);

            Assert.IsTrue(sut.ValidateResponseCode(500));
            Assert.IsTrue(sut.ValidateResponseCode(501));
            Assert.IsTrue(sut.ValidateResponseCode(502));
            Assert.IsTrue(sut.ValidateResponseCode(503));
            Assert.IsTrue(sut.ValidateResponseCode(504));
            Assert.IsTrue(sut.ValidateResponseCode(505));
            Assert.IsTrue(sut.ValidateResponseCode(506));
            Assert.IsTrue(sut.ValidateResponseCode(507));
            Assert.IsTrue(sut.ValidateResponseCode(508));
            Assert.IsTrue(sut.ValidateResponseCode(510));
            Assert.IsTrue(sut.ValidateResponseCode(511));

            Assert.IsFalse(sut.ValidateResponseCode(509));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidateResponseCode_ServerErrors_ResponseTypeClientAny_Success()
        {
            SettingHttpResponseAttribute sut = new SettingHttpResponseAttribute(HttpResponseType.Any);
            Assert.IsNotNull(sut);
            Assert.AreEqual(HttpResponseType.Any, sut.ResponseType);

            Assert.IsTrue(sut.ValidateResponseCode(500));
            Assert.IsTrue(sut.ValidateResponseCode(501));
            Assert.IsTrue(sut.ValidateResponseCode(502));
            Assert.IsTrue(sut.ValidateResponseCode(503));
            Assert.IsTrue(sut.ValidateResponseCode(504));
            Assert.IsTrue(sut.ValidateResponseCode(505));
            Assert.IsTrue(sut.ValidateResponseCode(506));
            Assert.IsTrue(sut.ValidateResponseCode(507));
            Assert.IsTrue(sut.ValidateResponseCode(508));
            Assert.IsTrue(sut.ValidateResponseCode(510));
            Assert.IsTrue(sut.ValidateResponseCode(511));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidateResponseCode_Success_ResponseTypeInformation_Success()
        {
            SettingHttpResponseAttribute sut = new SettingHttpResponseAttribute(HttpResponseType.Success);
            Assert.IsNotNull(sut);
            Assert.AreEqual(HttpResponseType.Success, sut.ResponseType);

            Assert.IsTrue(sut.ValidateResponseCode(200));
            Assert.IsTrue(sut.ValidateResponseCode(201));
            Assert.IsTrue(sut.ValidateResponseCode(202));
            Assert.IsTrue(sut.ValidateResponseCode(203));
            Assert.IsTrue(sut.ValidateResponseCode(204));
            Assert.IsTrue(sut.ValidateResponseCode(205));
            Assert.IsTrue(sut.ValidateResponseCode(206));
            Assert.IsTrue(sut.ValidateResponseCode(207));
            Assert.IsTrue(sut.ValidateResponseCode(208));
            Assert.IsTrue(sut.ValidateResponseCode(226));

            Assert.IsFalse(sut.ValidateResponseCode(209));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidateResponseCode_Success_ResponseTypeClientAny_Success()
        {
            SettingHttpResponseAttribute sut = new SettingHttpResponseAttribute(HttpResponseType.Any);
            Assert.IsNotNull(sut);
            Assert.AreEqual(HttpResponseType.Any, sut.ResponseType);

            Assert.IsTrue(sut.ValidateResponseCode(200));
            Assert.IsTrue(sut.ValidateResponseCode(201));
            Assert.IsTrue(sut.ValidateResponseCode(202));
            Assert.IsTrue(sut.ValidateResponseCode(203));
            Assert.IsTrue(sut.ValidateResponseCode(204));
            Assert.IsTrue(sut.ValidateResponseCode(205));
            Assert.IsTrue(sut.ValidateResponseCode(206));
            Assert.IsTrue(sut.ValidateResponseCode(207));
            Assert.IsTrue(sut.ValidateResponseCode(208));
            Assert.IsTrue(sut.ValidateResponseCode(226));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidateResponseCode_Unnoficial_ResponseTypeInformation_Success()
        {
            SettingHttpResponseAttribute sut = new SettingHttpResponseAttribute(HttpResponseType.Unnoficial);
            Assert.IsNotNull(sut);
            Assert.AreEqual(HttpResponseType.Unnoficial, sut.ResponseType);

            Assert.IsTrue(sut.ValidateResponseCode(103));
            Assert.IsTrue(sut.ValidateResponseCode(218));
            Assert.IsTrue(sut.ValidateResponseCode(419));
            Assert.IsTrue(sut.ValidateResponseCode(420));
            Assert.IsTrue(sut.ValidateResponseCode(450));
            Assert.IsTrue(sut.ValidateResponseCode(498));
            Assert.IsTrue(sut.ValidateResponseCode(499));
            Assert.IsTrue(sut.ValidateResponseCode(509));
            Assert.IsTrue(sut.ValidateResponseCode(526));
            Assert.IsTrue(sut.ValidateResponseCode(530));
            Assert.IsTrue(sut.ValidateResponseCode(598));

            Assert.IsFalse(sut.ValidateResponseCode(209));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ValidateResponseCode_Unnoficial_ResponseTypeClientAny_Success()
        {
            SettingHttpResponseAttribute sut = new SettingHttpResponseAttribute(HttpResponseType.Any);
            Assert.IsNotNull(sut);
            Assert.AreEqual(HttpResponseType.Any, sut.ResponseType);

            Assert.IsTrue(sut.ValidateResponseCode(103));
            Assert.IsTrue(sut.ValidateResponseCode(218));
            Assert.IsTrue(sut.ValidateResponseCode(419));
            Assert.IsTrue(sut.ValidateResponseCode(420));
            Assert.IsTrue(sut.ValidateResponseCode(450));
            Assert.IsTrue(sut.ValidateResponseCode(498));
            Assert.IsTrue(sut.ValidateResponseCode(499));
            Assert.IsTrue(sut.ValidateResponseCode(509));
            Assert.IsTrue(sut.ValidateResponseCode(526));
            Assert.IsTrue(sut.ValidateResponseCode(530));
            Assert.IsTrue(sut.ValidateResponseCode(598));
        }
    }
}
