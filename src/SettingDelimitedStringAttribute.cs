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
 *  File: SettingDelimitedStringAttribute.cs
 *
 *  Purpose:  Validates that the string is a delimited string
 *
 *  Date        Name                Reason
 *  30/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace AppSettings
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class SettingDelimitedStringAttribute : Attribute
    {
        #region Constructors

        public SettingDelimitedStringAttribute(char delimiter)
            : this (delimiter, 0)
        {
            Delimiter = delimiter;
            MinimumItems = uint.MinValue;
        }

        public SettingDelimitedStringAttribute(char delimiter, uint minimumItems)
            : this (delimiter, minimumItems, 100)
        {

        }

        public SettingDelimitedStringAttribute(char delimiter, uint minimumItems, uint maximumItems)
        {
            if (minimumItems >= maximumItems)
                throw new ArgumentOutOfRangeException(nameof(maximumItems));

            Delimiter = delimiter;
            MinimumItems = minimumItems;
            MaximumItems = maximumItems;
        }

        #endregion Constructors

        #region Properties

        public char Delimiter { get; private set; }

        public uint MinimumItems { get; private set; }

        public uint MaximumItems { get; private set; }

        #endregion Properties
    }
}
