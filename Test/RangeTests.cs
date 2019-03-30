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
 *  Product:  AppSettings.Tests
 *  
 *  File: RangeTests.cs
 *
 *  Purpose:  Test Cases
 *
 *  Date        Name                Reason
 *  06/12/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AppSettings.Tests
{
    [TestClass]
    public class RangeTests
    {
        public class RangeInt
        {
            [SettingRange(1, 40)]
            public int Value { get; set; }
        }

        public class RangeUInt
        {
            [SettingRange(1u, 40u)]
            public uint Value { get; set; }
        }

        public class RangeLong
        {
            [SettingRange(1L, 40L)]
            public long Value { get; set; }
        }

        public class RangeFloat
        {
            [SettingRange(1.1f, 40f)]
            public float Value { get; set; }
        }

        [TestMethod]
        public void RangeIntValid1()
        {
            RangeInt range = new RangeInt()
            {
                Value = 1
            };

            range = ValidateSettings<RangeInt>.Validate(range);
        }

        [TestMethod]
        public void RangeIntValid2()
        {
            RangeInt range = new RangeInt()
            {
                Value = 40
            };

            range = ValidateSettings<RangeInt>.Validate(range);
        }

        [TestMethod]
        [ExpectedException(typeof(SettingException))]
        public void RangeIntInvalid()
        {
            RangeInt range = new RangeInt()
            {
                Value = 0
            };

            range = ValidateSettings<RangeInt>.Validate(range);
        }

        [TestMethod]
        public void RangeLongValid1()
        {
            RangeLong range = new RangeLong()
            {
                Value = 1L
            };

            range = ValidateSettings<RangeLong>.Validate(range);
        }

        [TestMethod]
        public void RangeLongValid2()
        {
            RangeLong range = new RangeLong()
            {
                Value = 40L
            };

            range = ValidateSettings<RangeLong>.Validate(range);
        }

        [TestMethod]
        [ExpectedException(typeof(SettingException))]
        public void RangeLongInvalid()
        {
            RangeLong range = new RangeLong()
            {
                Value = 0L
            };

            range = ValidateSettings<RangeLong>.Validate(range);
        }

        [TestMethod]
        public void RangeFloatValid1()
        {
            RangeFloat range = new RangeFloat()
            {
                Value = 1.1f
            };

            range = ValidateSettings<RangeFloat>.Validate(range);
        }

        [TestMethod]
        public void RangeFloatValid2()
        {
            RangeFloat range = new RangeFloat()
            {
                Value = 40f
            };

            range = ValidateSettings<RangeFloat>.Validate(range);
        }

        [TestMethod]
        [ExpectedException(typeof(SettingException))]
        public void RangeFloatInvalid()
        {
            RangeFloat range = new RangeFloat()
            {
                Value = 0.9f
            };

            range = ValidateSettings<RangeFloat>.Validate(range);
        }

        [TestMethod]
        public void RangeUIntValid1()
        {
            RangeUInt range = new RangeUInt()
            {
                Value = 1u
            };

            range = ValidateSettings<RangeUInt>.Validate(range);
        }

        [TestMethod]
        public void RangeUIntValid2()
        {
            RangeUInt range = new RangeUInt()
            {
                Value = 40u
            };

            range = ValidateSettings<RangeUInt>.Validate(range);
        }

        [TestMethod]
        [ExpectedException(typeof(SettingException))]
        public void RangeUIntInvalid()
        {
            RangeUInt range = new RangeUInt()
            {
                Value = 0u
            };

            range = ValidateSettings<RangeUInt>.Validate(range);
        }
    }
}
