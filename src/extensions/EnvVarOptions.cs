// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.CommandLine;

namespace Obsidian.CLI.extensions;

/// <summary>
/// Add environment variable values to System.CommandLine Options
/// </summary>
public static class EnvVarOptions
{
    // capture parse errors from env vars
    private static readonly List<string> EnvVarErrors = new ();

    /// <summary>
    /// Add an option that uses env var for default value
    /// </summary>
    /// <typeparam name="T">type of option</typeparam>
    /// <param name="names">list of option names / aliases</param>
    /// <param name="description">option description</param>
    /// <param name="defaultValue">option default value</param>
    /// <returns>Option</returns>
    public static Option AddOption<T>(string[] names, string description, T defaultValue)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentNullException(nameof(description));
        }

        // this will throw on bad names
        string env = GetValueFromEnvironment(names, out string key);

        T value = defaultValue;

        // set default to environment value if set
        if (!string.IsNullOrWhiteSpace(env))
        {
            if (defaultValue.GetType().IsEnum)
            {
                if (Enum.TryParse(defaultValue.GetType(), env, true, out object result))
                {
                    value = (T)result;
                }
                else
                {
                    EnvVarErrors.Add($"Environment variable {key} is invalid");
                }
            }
            else
            {
                try
                {
                    value = (T)Convert.ChangeType(env, typeof(T));
                }
                catch
                {
                    EnvVarErrors.Add($"Environment variable {key} is invalid");
                }
            }
        }

        return new Option<T>(names, () => value, description);
    }

    /// <summary>
    /// Add an option that uses env var for default value
    /// Must be a numeric value type
    /// Optional min and max value validation
    /// </summary>
    /// <typeparam name="T">type of option</typeparam>
    /// <param name="names">list of option names / aliases</param>
    /// <param name="description">option description</param>
    /// <param name="defaultValue">option default value</param>
    /// <param name="minValue">min value</param>
    /// <param name="maxValue">max value</param>
    /// <returns>Option</returns>
    public static Option<int> AddOption(string[] names, string description, int defaultValue, int minValue, int? maxValue = null)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentNullException(nameof(description));
        }

        // this will throw on bad names
        string env = GetValueFromEnvironment(names, out string key);

        int value = defaultValue;

        // set default to environment value if set
        if (!string.IsNullOrWhiteSpace(env))
        {
            if (!int.TryParse(env, out value))
            {
                EnvVarErrors.Add($"Environment variable {key} is invalid");
            }
        }

        Option<int> opt = new (names, () => value, description);

        opt.AddValidator((res) =>
        {
            string s = string.Empty;
            int val;

            try
            {
                val = (int)res.GetValueOrDefault();

                if (val < minValue)
                {
                    s = $"{names[0]} must be >= {minValue}";
                }
            }
            catch
            {
            }

            res.ErrorMessage = s;
        });

        if (maxValue != null)
        {
            opt.AddValidator((res) =>
            {
                string s = string.Empty;
                int val;

                try
                {
                    val = (int)res.GetValueOrDefault();

                    if (val > maxValue)
                    {
                        s = $"{names[0]} must be <= {maxValue}";
                    }
                }
                catch
                {
                }

                res.ErrorMessage = s;
            });
        }

        return opt;
    }

    // check for environment variable value
    private static string GetValueFromEnvironment(string[] names, out string key)
    {
        if (names == null || names.Length < 1)
        {
            throw new ArgumentNullException(nameof(names));
        }

        key = names[0];
        string val;

        foreach (string name in names)
        {
            if (string.IsNullOrWhiteSpace(name) || name[0] != '-')
            {
                throw new ArgumentException($"Invalid command line parameter {name.Trim()}", nameof(names));
            }
            else if (name[1] == '-')
            {
                key = name[2..].Trim().ToUpperInvariant().Replace('-', '_');
                val = Environment.GetEnvironmentVariable(key);

                if (!string.IsNullOrWhiteSpace(val))
                {
                    return val.Trim();
                }
            }
        }

        return string.Empty;
    }
}
