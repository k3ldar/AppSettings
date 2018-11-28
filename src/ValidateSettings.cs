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
 *  Copyright (c) 2018 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AppSettings
 *  
 *  File: ValidateSettings.cs
 *
 *  Purpose:  Validates classes based on attributes
 *
 *  Date        Name                Reason
 *  28/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;

namespace AppSettings
{
    public static class ValidateSettings<T>
    {
        #region Public Static Properties

        public static ISettingError SettingError { get; set; }

        public static ISettingOverride SettingOverride { get; set; }

        #endregion Public Static Properties

        #region Public Static Methods

        public static T Validate(T settings, ISettingOverride settingOverride, ISettingError settingError)
        {
            SettingOverride = settingOverride ?? throw new ArgumentNullException(nameof(settingOverride));
            SettingError = settingError ?? throw new ArgumentNullException(nameof(settingError));
                
            return (Validate(settings));
        }

        public static T Validate(T settings, ISettingOverride settingOverride)
        {
            SettingOverride = settingOverride ?? throw new ArgumentNullException(nameof(settingOverride));

            return (Validate(settings));
        }

        public static T Validate(T settings, ISettingError settingError)
        {
            SettingError = settingError ?? throw new ArgumentNullException(nameof(settingError));

            return (Validate(settings));
        }

        public static T Validate (T settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            ValidateAllSettings(settings.GetType());

            return (settings);
        }

        #endregion Public Static Methods

        #region Private Static Methods

        /// <summary>
        /// Finds all validate-able settings in a class/nested class
        /// </summary>
        /// <param name="path"></param>
        /// <param name="classType"></param>
        private static void ValidateAllSettings(in Type classType)
        {
            if (classType == null)
                throw new ArgumentNullException(nameof(classType));

            foreach (PropertyInfo propertyInfo in classType.GetProperties(BindingFlags.NonPublic | 
                BindingFlags.Public | BindingFlags.Static))
            {
                ValidateSetting(propertyInfo);
            }

            foreach (Type subClass in classType.GetNestedTypes())
            {
                ValidateAllSettings(subClass);
            }


            // If the class has a public static/non static method called ValidateSettings, 
            // call it, the method should self validate itself
            MethodInfo validateSettingsMethod = classType.GetMethod("ValidateSettings", 
                BindingFlags.NonPublic | BindingFlags.Static);

            if (validateSettingsMethod != null)
                validateSettingsMethod.Invoke(classType, null);
        }

        /// <summary>
        /// Retrieves the value for a property, and performs validation
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <param name="keyName"></param>
        /// <param name="propertyValue"></param>
        /// <returns></returns>
        private static void ValidateSetting (in PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
                throw new ArgumentNullException(nameof(propertyInfo));

            object propertyValue = null;

            // There is an opportunity to override the settings, if the SettingOverride property
            // has been set
            if (SettingOverride != null)
            {
                if (SettingOverride.OverrideSettingValue(propertyInfo.Name, ref propertyValue))
                {
                    propertyInfo.SetValue(propertyInfo, propertyValue);
                    ValidateSettingValues(propertyInfo, propertyValue, false);

                    return;
                }
            }
            
            // is the property the default value and can the property have a default value assigned??????
            if (propertyInfo.GetValue(propertyInfo) == null ||
                propertyInfo.GetValue(propertyInfo).Equals(GetDefault(propertyInfo.PropertyType)))
            {
                foreach (CustomAttributeData attribute in propertyInfo.CustomAttributes)
                {
                    if (attribute.AttributeType == typeof(SettingDefaultAttribute))
                    {
                        if (attribute.ConstructorArguments[0].Value.GetType() == propertyInfo.PropertyType)
                        {
                            ValidateSettingValues(propertyInfo, attribute.ConstructorArguments[0].Value, true);
                            return;
                        }
                        else
                        {
                            ReportError(propertyInfo.Name, "Invalid Default Property Type");
                        }
                    }
                }
            }

            // does the property have an optional attribute set
            SettingOptionalAttribute optionalSetting = (SettingOptionalAttribute)propertyInfo.GetCustomAttribute(
                typeof(SettingOptionalAttribute));
            
            if (optionalSetting != null)
            {
                ReportError(propertyInfo.Name, "Value is optional");
                return;
            }

            // is the property a string, is it empty, is it allowed to be empty?
            if (propertyInfo.PropertyType.FullName == "System.String")
            {
                SettingStringAttribute stringSetting = (SettingStringAttribute)propertyInfo.GetCustomAttribute(
                    typeof(SettingStringAttribute));

                if (stringSetting == null || !stringSetting.AllowNullOrEmpty)
                {
                    ReportError(propertyInfo.Name, "Can not be null or empty");
                }
            }

            ValidateSettingValues(propertyInfo, propertyInfo.GetValue(propertyInfo), false);

            // if we don't have a value, and the value doesn't have a setting default attribute, or optional attribute, 
            // report the error
            ReportError(propertyInfo.Name, $"Does not have a value");

            return;
        }

        private static void ReportError(in string propertyName, string error)
        {
            if (SettingError != null)
                SettingError.SettingError(propertyName, error);
            else
                throw new SettingException(propertyName, error);
        }

        private static object GetDefault(in Type type)
        {
            if (type == null || !type.IsValueType)
                return null;

            return Activator.CreateInstance(type);
        }

        /// <summary>
        /// Perform validation checks depending on whether setting attributes are present or not
        /// </summary>
        /// <param name="propInfo"></param>
        /// <param name="propValue"></param>
        /// <param name="isDefault"></param>
        private static void ValidateSettingValues (in PropertyInfo propInfo, in object propValue, 
            in bool isDefault)
        {
            ValidateEmail(propInfo, propValue, isDefault);
            ValidatePathExists(propInfo, propValue, isDefault);
            ValidatePathIsValid(propInfo, propValue, isDefault);
            ValidateUri(propInfo, propValue, isDefault);
            ValidateValueString(propInfo, propValue, isDefault);
            ValidateRange(propInfo, propValue, isDefault);
        }

        #region Attribute Validation Methods

        /// <summary>
        /// Validates that an email address is valid
        /// </summary>
        /// <param name="propInfo"></param>
        /// <param name="propValue"></param>
        /// <param name="isDefault"></param>
        private static void ValidateEmail(in PropertyInfo propInfo, in object propValue, 
            in bool isDefault)
        {
            SettingEmailAttribute emailSetting = (SettingEmailAttribute)propInfo.GetCustomAttribute(
                    typeof(SettingEmailAttribute));

            if (emailSetting != null)
            {
                // check it's a valid email address
                string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                                @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                                @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

                System.Text.RegularExpressions.Regex exp = new System.Text.RegularExpressions.Regex(strRegex);
                System.Text.RegularExpressions.Match m;

                if (emailSetting.AllowMultiple)
                {
                    string[] emailAddresses = propValue.ToString().Split(new char[] { emailSetting.SeperatorChar },
                        StringSplitOptions.RemoveEmptyEntries);

                    foreach (string email in emailAddresses)
                    {
                        m = exp.Match(email, 0);

                        if (!m.Success)
                            ReportError(propInfo.Name, $"Contains an invalid email address: {email}");
                    }
                }
                else
                {
                    m = exp.Match(propValue.ToString(), 0);

                    if (!m.Success)
                        ReportError(propInfo.Name, $"Not a valid email address: {propValue.ToString()}");
                }
            }
        }

        /// <summary>
        /// Validate a path exists
        /// </summary>
        /// <param name="propInfo"></param>
        /// <param name="propValue"></param>
        /// <param name="isDefault"></param>
        private static void ValidatePathExists(in PropertyInfo propInfo, in object propValue, 
            in bool isDefault)
        {
            bool attrExists = propInfo.CustomAttributes.Where(
                attr => attr.AttributeType == typeof(SettingPathExistsAttribute)).FirstOrDefault() != null;

            if (attrExists)
            {
                try
                {
                    string path = Path.GetDirectoryName(Path.Combine(propValue.ToString(), " ").TrimEnd()) ?? 
                        propValue.ToString();

                    // check the path exists
                    if (!Directory.Exists(path))
                        ReportError(propInfo.Name, $"Path does not exist or does not have permissions: '{path}'");
                }
                catch (Exception err)
                {
                    ReportError(propInfo.Name, propValue.ToString());
                    ReportError(propInfo.Name, err.Message);
                }
            }
        }

        /// <summary>
        /// Validate a path setting is valid
        /// </summary>
        /// <param name="propInfo"></param>
        /// <param name="propValue"></param>
        /// <param name="isDefault"></param>
        private static void ValidatePathIsValid(in PropertyInfo propInfo, in object propValue, 
            in bool isDefault)
        {
            bool attrExists = propInfo.CustomAttributes.Where(
                attr => attr.AttributeType == typeof(SettingValidPathAttribute)).FirstOrDefault() != null;

            if (attrExists)
            {
                try
                {
                    if (propValue.ToString().IndexOfAny(Path.GetInvalidPathChars()) > -1)
                        ReportError(propInfo.Name, "Contains invalid characters");

                    // last check on valid path
                    string pathValid = Path.GetFullPath(propValue.ToString());
                }
                catch
                {
                    // check the path exists
                    ReportError(propInfo.Name, "Not a valid path");
                }
            }
        }

        /// <summary>
        /// Validates range attribute settings
        /// </summary>
        /// <param name="propInfo"></param>
        /// <param name="propValue"></param>
        /// <param name="isDefault"></param>
        private static void ValidateRange(in PropertyInfo propInfo, in object propValue, 
            in bool isDefault)
        {
            SettingRangeAttribute attrRange = (SettingRangeAttribute)propInfo.GetCustomAttribute(
                    typeof(SettingRangeAttribute));

            if (attrRange != null)
            {
                // the value is within range
                if (decimal.TryParse(propValue.ToString(), out decimal parsedValue))
                {


                    if (parsedValue < attrRange.MinimumValue || parsedValue > attrRange.MaximumValue)
                        ReportError(propInfo.Name, $"Value ({propValue.ToString()}) is outside of the valid range " +
                            $"and must be between {attrRange.MinimumValue} and {attrRange.MaximumValue}");
                }
                else
                {
                    ReportError(propInfo.Name, "Must be decimal, int, uint or float");
                }
            }
        }

        /// <summary>
        /// Validates a uri is valid, optionally connects to test the end point
        /// </summary>
        /// <param name="propInfo"></param>
        /// <param name="propValue"></param>
        /// <param name="isDefault"></param>
        private static void ValidateUri(in PropertyInfo propInfo, in object propValue, 
            in bool isDefault)
        {
            SettingUriAttribute uriSetting = (SettingUriAttribute)propInfo.GetCustomAttribute(
                    typeof(SettingUriAttribute));

            if (uriSetting != null)
            {
                Uri uriResult;

                if (propValue == null || String.IsNullOrEmpty(propValue.ToString()))
                {
                    ReportError(propInfo.Name, "Not a valid Uri");
                    return;
                }

                // check it's a valid Uri
                if (!Uri.TryCreate(propValue.ToString(), uriSetting.UriKind, out uriResult))
                {
                    ReportError(propInfo.Name, $"Value {propValue.ToString()}, is not a valid Uri");
                    return;
                }

                if (uriResult.IsAbsoluteUri && uriSetting.ValidateEndPoint)
                {
                    HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(uriResult);
                    try
                    {
                        using (HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse())
                        {
                            using (Stream stream = response.GetResponseStream())
                            {
                                using (StreamReader reader = new StreamReader(stream))
                                {

                                }
                            }
                        }
                    }
                    catch
                    {
                        ReportError(propInfo.Name, "Contains a valid Uri, however the end point can not be reached");
                    }
                    finally
                    {
                        webRequest = null;
                    }
                }
            }
        }

        /// <summary>
        /// Validates a string, min/max length, is allowed null/empty
        /// </summary>
        /// <param name="propInfo"></param>
        /// <param name="propValue"></param>
        /// <param name="isDefault"></param>
        private static void ValidateValueString(in PropertyInfo propInfo, in object propValue, 
            in bool isDefault)
        {

            if (propInfo.PropertyType.FullName == "System.String")
            {
                SettingStringAttribute stringSetting = (SettingStringAttribute)propInfo.GetCustomAttribute(
                    typeof(SettingStringAttribute));

                if (stringSetting != null)
                {
                    string propVal = propValue.ToString();

                    if (!stringSetting.AllowNullOrEmpty && String.IsNullOrEmpty(propVal))
                        ReportError(propInfo.Name, "Not allowed to be null or empty");

                    if (propVal.Length < stringSetting.MinLength)
                       ReportError(propInfo.Name, "Minimum length should be at " +
                            $"least {stringSetting.MinLength} characters long, is currently {propVal.Length} characters");

                    if (propVal.Length > stringSetting.MaxLength)
                        ReportError(propInfo.Name, "Maximum length can not be longer than " +
                            $"{stringSetting.MaxLength} characters long, is currently {propVal.Length} characters");
                }
            }
        }

        #endregion Attribute Validation Methods

        #endregion Private Static Methods
    }
}