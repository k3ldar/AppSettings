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
 *  File: SettingRangeAttribute.cs
 *
 *  Purpose:  Min/max values for integral and decimal types
 *
 *  Date        Name                Reason
 *  28/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace AppSettings
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class SettingRangeAttribute : Attribute
    {
        #region Constructors

        public SettingRangeAttribute(int minimumValue, int maximumValue)
        {
            MinimumValue = minimumValue;
            MaximumValue = maximumValue;
        }

        public SettingRangeAttribute(uint minimumValue, uint maximumValue)
        {
            MinimumValue = minimumValue;
            MaximumValue = maximumValue;
        }

        public SettingRangeAttribute(float minimumValue, float maximumValue)
        {
            MinimumValue = minimumValue;
            MaximumValue = maximumValue;
        }

        public SettingRangeAttribute(long minimumValue, long maximumValue)
        {
            MinimumValue = minimumValue;
            MaximumValue = maximumValue;
        }

        #endregion Constructors

        #region Properties

        public object MinimumValue { get; }

        public object MaximumValue { get; }

        #endregion Properties
    }
}
