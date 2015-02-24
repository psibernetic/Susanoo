﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Susanoo
{
    /// <summary>
    /// Comparison options
    /// </summary>
    public static class Comparison
    {
        /// <summary>
        /// Remove the property from the comparison.
        /// </summary>
        public static CompareMethod Ignore
        {
            get { return CompareMethod.Ignore; }
        }

        /// <summary>
        /// Values must equal.
        /// </summary>
        public static CompareMethod Equal
        {
            get { return CompareMethod.Equal; }
        }

        /// <summary>
        /// Column value must be less than parameter value.
        /// </summary>
        public static CompareMethod LessThan
        {
            get { return CompareMethod.LessThan; }
        }

        /// <summary>
        /// Column value must be less than or equal parameter value.
        /// </summary>
        public static CompareMethod LessThanOrEqual
        {
            get { return CompareMethod.LessThanOrEqual; }
        }

        /// <summary>
        /// Column value must be greater than parameter value.
        /// </summary>
        public static CompareMethod GreaterThan
        {
            get { return CompareMethod.GreaterThan; }
        }

        /// <summary>
        /// Column value must be greater than or equal parameter value.
        /// </summary>
        public static CompareMethod GreaterThanOrEqual
        {
            get { return CompareMethod.GreaterThanOrEqual; }
        }

        /// <summary>
        /// Values must NOT equal.
        /// </summary>
        public static CompareMethod NotEqual
        {
            get { return CompareMethod.NotEqual; }
        }

        /// <summary>
        /// Column value must start with parameter value.
        /// </summary>
        public static CompareMethod StartsWith
        {
            get { return CompareMethod.StartsWith; }
        }

        /// <summary>
        /// Column value must end with parameter value.
        /// </summary>
        public static CompareMethod EndsWith
        {
            get { return CompareMethod.EndsWith; }
        }

        /// <summary>
        /// Column value must contain parameter value.
        /// </summary>
        public static CompareMethod Contains
        {
            get { return CompareMethod.Contains; }
        }

        /// <summary>
        /// Overrides the comparison with a provided comparison string.
        /// </summary>
        /// <param name="overrideText">The override text.</param>
        public static ComparisonOverride Override(string overrideText)
        {
            return new ComparisonOverride(overrideText);
        }

        /// <summary>
        /// Gets the comparison format string.
        /// </summary>
        /// <param name="compare">The compare.</param>
        /// <returns>System.String.</returns>
        public static string GetComparisonFormat(CompareMethod compare)
        {
            var comparisonString = "";

            switch (compare)
            {
                case CompareMethod.Equal:
                    comparisonString = " AND (@{0} IS NULL OR {0} = @{0})";
                    break;
                case CompareMethod.NotEqual:
                    comparisonString = " AND (@{0} IS NULL OR {0} <> @{0})";
                    break;
                case CompareMethod.LessThan:
                    comparisonString = " AND (@{0} IS NULL OR {0} < @{0})";
                    break;
                case CompareMethod.LessThanOrEqual:
                    comparisonString = " AND (@{0} IS NULL OR {0} <= @{0})";
                    break;
                case CompareMethod.GreaterThan:
                    comparisonString = " AND (@{0} IS NULL OR {0} > @{0})";
                    break;
                case CompareMethod.GreaterThanOrEqual:
                    comparisonString = " AND (@{0} IS NULL OR {0} >= @{0})";
                    break;
                case CompareMethod.StartsWith:
                    comparisonString = " AND (@{0} IS NULL OR {0} LIKE  @{0} + '%')";
                    break;
                case CompareMethod.EndsWith:
                    comparisonString = " AND (@{0} IS NULL OR {0} LIKE  '%' + @{0})";
                    break;
                case CompareMethod.Contains:
                    comparisonString = " AND (@{0} IS NULL OR {0} LIKE  '%' + @{0} + '%')";
                    break;
            }

            return comparisonString;
        }

        /// <summary>
        /// Replacement comparison for override
        /// </summary>
        public class ComparisonOverride
        {
            /// <summary>
            /// Gets the override text.
            /// </summary>
            /// <value>The override text.</value>
            public string OverrideText { get; private set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="ComparisonOverride"/> class.
            /// </summary>
            /// <param name="overrideText">The override text.</param>
            public ComparisonOverride(string overrideText)
            {
                OverrideText = overrideText;
            }

            public override string ToString()
            {
                return OverrideText;
            }
        }
    }
}