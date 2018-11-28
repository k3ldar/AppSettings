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
 *  File: SettingUriAttribute.cs
 *
 *  Purpose:  Ensures the string is a valid Uri
 *
 *  Date        Name                Reason
 *  28/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace AppSettings
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class SettingUriAttribute : Attribute
    {
        #region Constructors

        public SettingUriAttribute()
            : this (false)
        {

        }

        public SettingUriAttribute(bool validateEndPoint)
            : this (validateEndPoint, UriKind.Absolute)
        {
        }

        public SettingUriAttribute(bool validateEndPoint, UriKind uriKind)
        {
            ValidateEndPoint = validateEndPoint;
            UriKind = uriKind;
        }

        #endregion Constructors

        #region Properties

        public bool ValidateEndPoint { get; private set; }

        public UriKind UriKind { get; private set; }

        #endregion Properties
    }
}
