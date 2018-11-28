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
 *  Copyright (c) 2018 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AppSettings
 *  
 *  File: SettingStringAttribute.cs
 *
 *  Purpose:  Options for string properties
 *
 *  Date        Name                Reason
 *  28/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace AppSettings
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class SettingStringAttribute : Attribute
    {
        #region Constructors

        public SettingStringAttribute(uint minimumLength, uint maximumLength)
            : this (false, minimumLength, maximumLength)
        {
        }

        public SettingStringAttribute(bool allowNullOrEmpty)
            : this (allowNullOrEmpty, 1, 500)
        {

        }

        public SettingStringAttribute(bool allowNullOrEmpty, uint minimumLength, uint maximumLength)
        {
            if (maximumLength < minimumLength)
                throw new ArgumentOutOfRangeException(nameof(maximumLength), 
                    $"{nameof(maximumLength)} can not be less than {nameof(minimumLength)}");

            AllowNullOrEmpty = allowNullOrEmpty;
            MinLength = minimumLength;
            MaxLength = maximumLength;
        }

        #endregion Constructors

        #region Properties

        public uint MinLength { get; private set; }

        public uint MaxLength { get; private set; }

        public bool AllowNullOrEmpty { get; private set; }

        #endregion Properties
    }
}
