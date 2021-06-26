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
 *  File: SettingDefaultAttribute.cs
 *
 *  Purpose:  Default value for a setting if after loading it is the same as the default
 *            value for the type
 *
 *  Date        Name                Reason
 *  28/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace AppSettings
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class SettingDefaultAttribute : Attribute
    {
        #region Constructors

        public SettingDefaultAttribute(char value)
        {
            DefaultValue = value;
        }

        public SettingDefaultAttribute(string value)
        {
            DefaultValue = value;
        }

        public SettingDefaultAttribute(int value)
        {
            DefaultValue = value;
        }

        public SettingDefaultAttribute(uint value)
        {
            DefaultValue = value;
        }

        public SettingDefaultAttribute(ulong value)
        {
            DefaultValue = value;
        }

        public SettingDefaultAttribute(long value)
        {
            DefaultValue = value;
        }

        public SettingDefaultAttribute(byte value)
        {
            DefaultValue = value;
        }

        public SettingDefaultAttribute(decimal value)
        {
            DefaultValue = value;
        }

        public SettingDefaultAttribute(float value)
        {
            DefaultValue = value;
        }

        public SettingDefaultAttribute(double value)
        {
            DefaultValue = value;
        }

        public SettingDefaultAttribute(bool value)
        {
            DefaultValue = value;
        }

        public SettingDefaultAttribute(short value)
        {
            DefaultValue = value;
        }

        public SettingDefaultAttribute(ushort value)
        {
            DefaultValue = value;
        }

        #endregion Constructors

        #region Public Properties

        public object DefaultValue { get; }

        #endregion Public Properties
    }
}
