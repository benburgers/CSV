﻿/*
 * © 2022-2024 Ben Burgers and contributors.
 * This work is licensed by GNU General Public License version 3.
 */

namespace BenBurgers.Text.Csv.Exceptions;

/// <summary>
/// An exception that is thrown if an error occurs during the processing of CSV data.
/// </summary>
public class CsvException : Exception
{
    /// <summary>
    /// Initializes a new instance of <see cref="CsvException" />.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="innerException">The inner exception.</param>
    internal CsvException(string message, Exception? innerException = null)
        : base(message, innerException)
    {
    }
}
