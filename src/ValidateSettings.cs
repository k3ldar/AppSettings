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
 *  File: ValidateSettings.cs
 *
 *  Purpose:  Validates classes based on attributes
 *
 *  Date        Name                Reason
 *  28/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;

namespace AppSettings
{
    public static class ValidateSettings<T>
    {
        #region Public Static Methods

        public static T Validate(T settings, ISettingOverride settingOverride, 
            ISettingError settingError, IApplicationOverride applicationOverride)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            ValidateAllSettings(settings.GetType(), settings, settingOverride, settingError, applicationOverride);

            return settings;
        }

        public static T Validate(T settings, ISettingOverride settingOverride)
        {
            return Validate(settings, settingOverride, null);
        }

        public static T Validate(T settings, ISettingOverride settingOverride,
            ISettingError settingError)
        {
            return Validate(settings, settingOverride, settingError, null);
        }

        public static T Validate(T settings, ISettingError settingError)
        {
            return Validate(settings, null, settingError);
        }

        public static T Validate(T settings, IApplicationOverride applicationOverride)
        {
            return Validate(settings, null, null, applicationOverride);
        }

        public static T Validate (T settings)
        {
            return Validate(settings, null, null);
        }
        /// <summary>
        /// Finds all validate-able settings in a class/nested class
        /// </summary>
        /// <param name="path"></param>
        /// <param name="classType"></param>
        public static void ValidateAllSettings(in Type classType, in object settings, 
            ISettingOverride settingOverride, ISettingError settingError, IApplicationOverride applicationOverride)
        {
            if (classType == null)
                throw new ArgumentNullException(nameof(classType));

            foreach (PropertyInfo propertyInfo in classType.GetProperties(
                BindingFlags.Public | BindingFlags.Static))
            {
                bool isDefault = (propertyInfo.PropertyType.FullName == "System.String" && 
                    String.IsNullOrEmpty((string)propertyInfo.GetValue(propertyInfo))) ||
                    propertyInfo.GetValue(propertyInfo) == null ||
                    propertyInfo.GetValue(propertyInfo).Equals(GetDefault(propertyInfo.PropertyType));

                ValidateSetting(propertyInfo, isDefault, null, settingOverride, settingError, applicationOverride);
            }

            foreach (PropertyInfo propertyInfo in classType.GetProperties(
                BindingFlags.Public | BindingFlags.Instance))
            {
                bool isDefault = (propertyInfo.PropertyType.FullName == "System.String" &&
                    String.IsNullOrEmpty((string)propertyInfo.GetValue(settings))) || 
                    propertyInfo.GetValue(settings) == null ||
                    propertyInfo.GetValue(settings).Equals(GetDefault(propertyInfo.PropertyType));

                ValidateSetting(propertyInfo, isDefault, settings, settingOverride, settingError, applicationOverride);
            }

            foreach (Type subClass in classType.GetNestedTypes())
            {
                ValidateAllSettings(subClass, settings, settingOverride, settingError, applicationOverride);
            }


            // If the class has a public static/non static method called ValidateSettings, 
            // call it, the method should self validate itself
            MethodInfo validateSettingsMethod = classType.GetMethod("ValidateSettings", 
                BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);

            if (validateSettingsMethod != null)
                validateSettingsMethod.Invoke(settings, null);
        }

        #endregion Public Static Methods

        #region Private Static Methods


        /// <summary>
        /// Retrieves the value for a property, and performs validation
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <param name="keyName"></param>
        /// <param name="propertyValue"></param>
        /// <returns></returns>
        private static void ValidateSetting (in PropertyInfo propertyInfo, bool isDefault, object instance, 
            ISettingOverride settingOverride, ISettingError settingError, IApplicationOverride applicationOverride)
        {
            if (propertyInfo == null)
                throw new ArgumentNullException(nameof(propertyInfo));

            object propertyValue = null;

            // There is an opportunity to override the settings, if the settingOverride param has been set
            if (settingOverride != null)
            {
                if (settingOverride.OverrideSettingValue(propertyInfo.Name, ref propertyValue))
                {
                    propertyInfo.SetValue(instance ?? propertyInfo, propertyValue);
                    ValidateSettingValues(propertyInfo, propertyValue, false, settingError);

                    return;
                }
            }
            
            // is the property the default value and can the property have a default value assigned??????
            if (isDefault)
            {
                foreach (CustomAttributeData attribute in propertyInfo.CustomAttributes)
                {
                    if (attribute.AttributeType == typeof(SettingDefaultAttribute))
                    {
                        object currentValue = attribute.ConstructorArguments[0].Value;

                        if (applicationOverride != null)
                        {
                            object appValue = null;

                            string exp = currentValue.ToString();
                            bool expandable = exp[0] == '%' && exp[exp.Length - 1] == '%';

                            if (expandable && applicationOverride.ExpandApplicationVariable(exp.Substring(1, exp.Length -2), ref appValue))
                            {
                                propertyInfo.SetValue(instance ?? propertyInfo, appValue);
                                return;
                            }
                        }

                        if (attribute.ConstructorArguments[0].Value.GetType() == propertyInfo.PropertyType)
                        {

                            if (propertyInfo.PropertyType.FullName == "System.String")
                            {
                                currentValue = ExpandVariables(propertyInfo.Name, (string)currentValue);
                            }

                            ValidateSettingValues(propertyInfo, currentValue, true, settingError);

                            bool? isWithinRange = ValueIsWithinRange(propertyInfo, propertyInfo.GetValue(instance ?? propertyInfo));

                            if (isWithinRange == null || !isWithinRange.Value)
                            {
                                propertyInfo.SetValue(instance ?? propertyInfo, currentValue);
                            }

                            return;
                        }
                        else
                        {
                            ReportError(propertyInfo.Name, "Invalid Default Property Type", settingError);
                        }
                    }
                }
            }

            // does the property have an optional attribute set
            SettingOptionalAttribute optionalSetting = (SettingOptionalAttribute)propertyInfo.GetCustomAttribute(
                typeof(SettingOptionalAttribute));
            
            if (optionalSetting != null)
            {
                ReportError(propertyInfo.Name, "Value is optional", settingError);

                if (!isDefault)
                    ValidateSettingValues(propertyInfo, 
                        propertyInfo.GetValue(instance ?? propertyInfo, null), false, settingError);

                return;
            }

            // is the property a string, is it empty, is it allowed to be empty?
            if (isDefault && propertyInfo.PropertyType.FullName == "System.String")
            {
                SettingStringAttribute stringSetting = (SettingStringAttribute)propertyInfo.GetCustomAttribute(
                    typeof(SettingStringAttribute));

                if (stringSetting == null || !stringSetting.AllowNullOrEmpty)
                {
                    ReportError(propertyInfo.Name, "Can not be null or empty", settingError);
                }
            }

            ValidateSettingValues(propertyInfo, 
                propertyInfo.GetValue(instance ?? propertyInfo, null), false, settingError);
        }

        private static void ReportError(in string propertyName, string error, ISettingError settingError)
        {
            if (settingError != null)
                settingError.SettingError(propertyName, $"{propertyName}: {error}");
            else if (!error.Equals("Value is optional"))
                throw new SettingException(propertyName, $"{propertyName}: {error}");
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
            in bool isDefault, ISettingError settingError)
        {
            ValidateEmail(propInfo, propValue, isDefault, settingError);
            ValidatePathExists(propInfo, propValue, isDefault, settingError);
            ValidatePathIsValid(propInfo, propValue, isDefault, settingError);
            ValidateUri(propInfo, propValue, isDefault, settingError);
            ValidateValueString(propInfo, propValue, isDefault, settingError);
            ValidateRange(propInfo, propValue, isDefault, settingError);
            ValidateDelimited(propInfo, propValue, isDefault, settingError);
            ValidateNVPair(propInfo, propValue, isDefault, settingError);
            ValidateHttpResponse(propInfo, propValue, isDefault, settingError);
            ValidateRegex(propInfo, propValue, isDefault, settingError);
            ValidateGuid(propInfo, propValue, isDefault, settingError);
        }

        #region Attribute Validation Methods

        /// <summary>
        /// Validates that an email address is valid
        /// </summary>
        /// <param name="propInfo"></param>
        /// <param name="propValue"></param>
        /// <param name="isDefault"></param>
        private static void ValidateEmail(in PropertyInfo propInfo, in object propValue, 
            in bool isDefault, ISettingError settingError)
        {
            SettingEmailAttribute emailSetting = (SettingEmailAttribute)propInfo.GetCustomAttribute(
                    typeof(SettingEmailAttribute));

            if (emailSetting != null)
            {
                // check it's a valid email address
                string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                                @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                                @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

                Regex exp = new Regex(strRegex);
                Match m;

                if (emailSetting.AllowMultiple)
                {
                    string[] emailAddresses = propValue.ToString().Split(new char[] { emailSetting.SeperatorChar },
                        StringSplitOptions.RemoveEmptyEntries);

                    foreach (string email in emailAddresses)
                    {
                        m = exp.Match(email, 0);

                        if (!m.Success)
                            ReportError(propInfo.Name, $"Contains an invalid email address: {email}", settingError);
                    }
                }
                else
                {
                    m = exp.Match(propValue.ToString(), 0);

                    if (!m.Success)
                        ReportError(propInfo.Name, $"Not a valid email address: {propValue.ToString()}", settingError);
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
            in bool isDefault, ISettingError settingError)
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
                        ReportError(propInfo.Name, $"Path does not exist or does not have permissions: '{path}'", settingError);
                }
                catch (Exception err)
                {
                    ReportError(propInfo.Name, propValue.ToString(), settingError);
                    ReportError(propInfo.Name, err.Message, settingError);
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
            in bool isDefault, ISettingError settingError)
        {
            bool attrExists = propInfo.CustomAttributes.Where(
                attr => attr.AttributeType == typeof(SettingValidPathAttribute)).FirstOrDefault() != null;

            if (attrExists)
            {
                try
                {
                    if (propValue.ToString().IndexOfAny(Path.GetInvalidPathChars()) > -1)
                        ReportError(propInfo.Name, "Contains invalid characters", settingError);

                    // last check on valid path
                    string pathValid = Path.GetFullPath(propValue.ToString());
                }
                catch
                {
                    // check the path exists
                    ReportError(propInfo.Name, "Not a valid path", settingError);
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
            in bool isDefault, ISettingError settingError)
        {
            SettingRangeAttribute attrRange = (SettingRangeAttribute)propInfo.GetCustomAttribute(
                    typeof(SettingRangeAttribute));

            if (attrRange != null)
            {
                // the value is within range
                if (attrRange.MinimumValue.GetType().Name == propValue.GetType().Name &&
                    attrRange.MaximumValue.GetType().Name == propValue.GetType().Name)
                {
                    bool success = false;

                    switch (attrRange.MinimumValue.GetType().Name)
                    {
                        case "Single":
                            if (float.TryParse(propValue.ToString(), out float floatValue))
                                success = floatValue >= Convert.ToSingle(attrRange.MinimumValue) &&
                                    floatValue <= Convert.ToSingle(attrRange.MaximumValue);
                            break;
                        case "Int32":
                            if (int.TryParse(propValue.ToString(), out int intValue))
                                success = intValue >= Convert.ToInt32(attrRange.MinimumValue) &&
                                    intValue <= Convert.ToInt32(attrRange.MaximumValue);
                            break;
                        case "UInt32":
                            if (uint.TryParse(propValue.ToString(), out uint uintValue))
                                success = uintValue >= Convert.ToUInt32(attrRange.MinimumValue) &&
                                    uintValue <= Convert.ToUInt32(attrRange.MaximumValue);
                            break;
                        case "Int64":
                            if (Int64.TryParse(propValue.ToString(), out long longValue))
                                success = longValue >= Convert.ToInt32(attrRange.MinimumValue) &&
                                    longValue <= Convert.ToInt32(attrRange.MaximumValue);
                            break;
                        default:
                            ReportError(propInfo.Name, "Must be long, int, uint or float", settingError);
                            break;

                    }

                    if (!success)
                        ReportError(propInfo.Name, $"Value ({propValue.ToString()}) is outside of the valid range " +
                            $"and must be between {attrRange.MinimumValue} and {attrRange.MaximumValue}",
                            settingError);
                }
                else
                {
                    ReportError(propInfo.Name, "Must be decimal, int, uint or float", settingError);
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
            in bool isDefault, ISettingError settingError)
        {
            SettingUriAttribute uriSetting = (SettingUriAttribute)propInfo.GetCustomAttribute(
                    typeof(SettingUriAttribute));

            if (uriSetting != null)
            {
                if (propValue == null || String.IsNullOrEmpty(propValue.ToString()))
                {
                    ReportError(propInfo.Name, "Not a valid Uri", settingError);
                    return;
                }

                // check it's a valid Uri
                if (!Uri.TryCreate(propValue.ToString(), uriSetting.UriKind, out Uri uriResult))
                {
                    ReportError(propInfo.Name, $"Value {propValue.ToString()}, is not a valid Uri", settingError);
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
                        ReportError(propInfo.Name, "Contains a valid Uri, however the end point can not be reached", settingError);
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
            in bool isDefault, ISettingError settingError)
        {

            if (propInfo.PropertyType.FullName == "System.String")
            {
                SettingStringAttribute stringSetting = (SettingStringAttribute)propInfo.GetCustomAttribute(
                    typeof(SettingStringAttribute));

                if (stringSetting != null)
                {
                    string propVal = propValue == null ? null : propValue.ToString();

                    if (stringSetting.AllowNullOrEmpty && String.IsNullOrEmpty(propVal))
                        return;

                    if (!stringSetting.AllowNullOrEmpty && String.IsNullOrEmpty(propVal))
                        ReportError(propInfo.Name, "Not allowed to be null or empty", settingError);

                    if (propVal.Length < stringSetting.MinLength)
                       ReportError(propInfo.Name, "Minimum length should be at " +
                            $"least {stringSetting.MinLength} characters long, is currently {propVal.Length} characters", settingError);

                    if (propVal.Length > stringSetting.MaxLength)
                        ReportError(propInfo.Name, "Maximum length can not be longer than " +
                            $"{stringSetting.MaxLength} characters long, is currently {propVal.Length} characters", settingError);
                }
            }
        }

        /// <summary>
        /// Validates a string is delimited
        /// </summary>
        /// <param name="propInfo"></param>
        /// <param name="propValue"></param>
        /// <param name="isDefault"></param>
        private static void ValidateDelimited(in PropertyInfo propInfo, in object propValue,
            in bool isDefault, ISettingError settingError)
        {
            if (propInfo.PropertyType.FullName == "System.String")
            {
                SettingDelimitedStringAttribute delimitedSetting = (SettingDelimitedStringAttribute)propInfo.GetCustomAttribute(
                    typeof(SettingDelimitedStringAttribute));

                if (delimitedSetting != null)
                {
                    string propVal = propValue.ToString();

                    string[] items = propVal.Split(new char[] { delimitedSetting.Delimiter }, StringSplitOptions.RemoveEmptyEntries);

                    if (items.Length < delimitedSetting.MinimumItems)
                        ReportError(propInfo.Name, $"Delimited string must contain at least {delimitedSetting.MinimumItems} items", settingError);

                    if (items.Length > delimitedSetting.MaximumItems)
                        ReportError(propInfo.Name, $"Delimited string can only contain {delimitedSetting.MaximumItems} items", settingError);
                }
            }
        }

        /// <summary>
        /// Validates a string is a name value pair
        /// </summary>
        /// <param name="propInfo"></param>
        /// <param name="propValue"></param>
        /// <param name="isDefault"></param>
        private static void ValidateNVPair(in PropertyInfo propInfo, in object propValue,
            in bool isDefault, ISettingError settingError)
        {
            if (propInfo.PropertyType.FullName == "System.String")
            {
                SettingNameValuePairAttribute nvpSetting = (SettingNameValuePairAttribute)propInfo.GetCustomAttribute(
                    typeof(SettingNameValuePairAttribute));

                if (nvpSetting != null)
                {
                    NameValueCollection nameValueCollection = new NameValueCollection();

                    string[] items = propValue.ToString().Split(new char[] { nvpSetting.Delimiter }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string s in items)
                    {
                        string[] parts = s.Split('=');

                        if (parts.Length != 2)
                            ReportError(propInfo.Name, $"'{s}' is not valid for a name value part", settingError);

                        nameValueCollection.Add(parts[0], parts[1]);
                    }

                    if (nameValueCollection.Count < nvpSetting.MinimumItems)
                        ReportError(propInfo.Name, $"Name value pair string must contain at least {nvpSetting.MinimumItems} items", settingError);

                    if (nameValueCollection.Count > nvpSetting.MaximumItems)
                        ReportError(propInfo.Name, $"Name value pair string can only contain {nvpSetting.MaximumItems} items", settingError);
                }
            }
        }

        /// <summary>
        /// Validates a http response
        /// </summary>
        /// <param name="propInfo"></param>
        /// <param name="propValue"></param>
        /// <param name="isDefault"></param>
        private static void ValidateHttpResponse(in PropertyInfo propInfo, in object propValue,
            in bool isDefault, ISettingError settingError)
        {
            if (propInfo.PropertyType.FullName == "System.Int32")
            {
                SettingHttpResponseAttribute httpResponseSetting = (SettingHttpResponseAttribute)propInfo.GetCustomAttribute(
                    typeof(SettingHttpResponseAttribute));

                if (httpResponseSetting != null)
                {
                    if (!httpResponseSetting.ValidateResponseCode(Convert.ToInt32(propValue)))
                        ReportError(propInfo.Name, $"{propValue.ToString()} is not valid for {propInfo.Name} " +
                            $"expecting type {httpResponseSetting.ResponseType.ToString()}", settingError);
                }
            }
        }

        /// <summary>
        /// Validates a http response
        /// </summary>
        /// <param name="propInfo"></param>
        /// <param name="propValue"></param>
        /// <param name="isDefault"></param>
        private static void ValidateRegex(in PropertyInfo propInfo, in object propValue,
            in bool isDefault, ISettingError settingError)
        {
            if (propInfo.PropertyType.FullName == "System.String")
            {
                SettingRegexAttribute regexSetting = (SettingRegexAttribute)propInfo.GetCustomAttribute(
                    typeof(SettingRegexAttribute));

                if (regexSetting != null)
                {
                    Regex regex = new Regex(regexSetting.Regex);
                    Match match = regex.Match(propValue.ToString());
                    if (!match.Success)
                    {
                        ReportError(propInfo.Name, $"{propValue.ToString()} is not valid for {propInfo.Name} " +
                            $"expecting regex compatible to {regexSetting.Regex}", settingError);
                    }
                }
            }
        }

        /// <summary>
        /// Validates a string is a valid guid
        /// </summary>
        /// <param name="propInfo"></param>
        /// <param name="propValue"></param>
        /// <param name="isDefault"></param>
        private static void ValidateGuid(in PropertyInfo propInfo, in object propValue,
            in bool isDefault, ISettingError settingError)
        {
            if (propInfo.PropertyType.FullName == "System.String")
            {
                SettingGuidAttribute guidSetting = (SettingGuidAttribute)propInfo.GetCustomAttribute(
                    typeof(SettingGuidAttribute));

                if (guidSetting != null)
                {
                    if (!Guid.TryParse(propValue.ToString(), out Guid guid))
                    {
                        ReportError(propInfo.Name, $"{propValue.ToString()} is not valid for {propInfo.Name} " +
                            "expecting Guid value", settingError);
                    }
                }
            }
        }

        /// <summary>
        /// Validates range attribute settings
        /// </summary>
        /// <param name="propInfo"></param>
        /// <param name="propValue"></param>
        /// <param name="isDefault"></param>
        private static bool? ValueIsWithinRange(in PropertyInfo propInfo, in object propValue)
        {
            SettingRangeAttribute attrRange = (SettingRangeAttribute)propInfo.GetCustomAttribute(
                    typeof(SettingRangeAttribute));

            if (attrRange != null)
            {
                // the value is within range
                if (attrRange.MinimumValue.GetType().Name == propValue.GetType().Name &&
                    attrRange.MaximumValue.GetType().Name == propValue.GetType().Name)
                {
                    bool success = false;

                    switch (attrRange.MinimumValue.GetType().Name)
                    {
                        case "Single":
                            if (float.TryParse(propValue.ToString(), out float floatValue))
                                success = floatValue >= Convert.ToSingle(attrRange.MinimumValue) &&
                                    floatValue <= Convert.ToSingle(attrRange.MaximumValue);
                            break;
                        case "Int32":
                            if (int.TryParse(propValue.ToString(), out int intValue))
                                success = intValue >= Convert.ToInt32(attrRange.MinimumValue) &&
                                    intValue <= Convert.ToInt32(attrRange.MaximumValue);
                            break;
                        case "UInt32":
                            if (uint.TryParse(propValue.ToString(), out uint uintValue))
                                success = uintValue >= Convert.ToUInt32(attrRange.MinimumValue) &&
                                    uintValue <= Convert.ToUInt32(attrRange.MaximumValue);
                            break;
                        case "Int64":
                            if (Int64.TryParse(propValue.ToString(), out long longValue))
                                success = longValue >= Convert.ToInt32(attrRange.MinimumValue) &&
                                    longValue <= Convert.ToInt32(attrRange.MaximumValue);
                            break;
                        default:
                            return null;

                    }

                    return success;
                }
            }

            return null;
        }

        #endregion Attribute Validation Methods

        #region Expand Variables

        private static object ExpandVariables(in string propertyName, string s)
        {
            return ReplaceSpecialWords(Environment.ExpandEnvironmentVariables(s), '%', '%');
        }

        private static string GetCustomReplacement(in ReplacableWord replacableWord)
        {
            switch (replacableWord)
            {
                case ReplacableWord.AppPath:
                    return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                default:
                    throw new InvalidOperationException("Invalid ReplacableWord");
            }
        }

        private static string ReplaceSpecialWords(in string s, in char startChar, in char endChar)
        {
            StringBuilder Result = new StringBuilder(s.Length);

            bool startFound = false;
            string currentWord = String.Empty;

            for (int i = 0; i < s.Length; i++)
            {
                char currChar = s[i];

                if (!startFound && currChar == startChar)
                {
                    startFound = true;
                }
                else if (startFound && currChar == endChar)
                {
                    if (!String.IsNullOrEmpty(currentWord))
                    {
                        if (Enum.TryParse(currentWord, out Environment.SpecialFolder specialFolder))
                        {
                            Result.Append(Environment.GetFolderPath(specialFolder));
                        }
                        else if (Enum.TryParse(currentWord, out ReplacableWord customReplace))
                        {
                            Result.Append(GetCustomReplacement(customReplace));
                        }
                    }

                    currentWord = String.Empty;
                    startFound = false;
                } 
                else if (!startFound)
                {
                    Result.Append(currChar);
                }
                else if (startFound)
                {
                    currentWord += currChar;
                }
            }

            return Result.ToString();
        }

        #endregion Expand Variables

        #endregion Private Static Methods
    }
}
