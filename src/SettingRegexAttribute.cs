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
 *  File: SettingRegexAttribute.cs
 *
 *  Purpose:  Validates that the string against user supplied regex
 *
 *  Date        Name                Reason
 *  01/12/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Linq;

namespace AppSettings
{
    /// <summary>
    /// Validates that the setting against a regex
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class SettingRegexAttribute : Attribute
    {
        #region Constructors

        public SettingRegexAttribute(string regex)
        {
            if (String.IsNullOrEmpty(regex))
                throw new ArgumentNullException(nameof(regex), "You must specify a regex value");

            Regex = regex;
        }

        #endregion Constructors

        #region Public Properties

        public string Regex { get; private set; }

        #endregion Public Properties
    }
}
