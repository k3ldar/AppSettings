﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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
 *  File: SettingEmailAttribute.cs
 *
 *  Purpose:  Validates that the string is a valid email address
 *
 *  Date        Name                Reason
 *  28/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Linq;

namespace AppSettings
{
    /// <summary>
    /// Validates that the setting is a valid email address
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class SettingEmailAttribute : Attribute
    {
        #region Constants

        private static readonly char[] validSeperators = { ';', '#', '\t', '\n', '\r' };

        #endregion Constants

        #region Constructors

        public SettingEmailAttribute()
        {
            AllowMultiple = false;
        }

        public SettingEmailAttribute(char seperatorCharacter)
        {
            if (!validSeperators.Contains(seperatorCharacter))
                throw new ArgumentOutOfRangeException(nameof(seperatorCharacter), "invalid separator character");

            AllowMultiple = true;
            SeperatorChar = seperatorCharacter;
        }

        #endregion Constructors

        #region Public Properties

        public bool AllowMultiple { get; }

        public char SeperatorChar { get; }

        #endregion Public Properties
    }
}
