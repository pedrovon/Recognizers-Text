﻿using System.Text.RegularExpressions;
using System.Collections.Generic;

using Microsoft.Recognizers.Definitions.German;

namespace Microsoft.Recognizers.Text.DateTime.German
{
    class GermanDateTimeAltExtractorConfiguration : BaseOptionsConfiguration, IDateTimeAltExtractorConfiguration
    {
        public GermanDateTimeAltExtractorConfiguration(IOptionsConfiguration config) : base(config)
        {
            DateExtractor = new BaseDateExtractor(new GermanDateExtractorConfiguration(this));
            DatePeriodExtractor = new BaseDatePeriodExtractor(new GermanDatePeriodExtractorConfiguration(this));
        }

        public IDateExtractor DateExtractor { get; }

        public IDateTimeExtractor DatePeriodExtractor { get; }

        private static readonly Regex OrRegex =
            new Regex(DateTimeDefinitions.OrRegex, RegexOptions.Singleline);

        private static readonly Regex DayRegex =
            new Regex(DateTimeDefinitions.DayRegex, RegexOptions.Singleline);

        public static readonly Regex ThisPrefixRegex =
            new Regex(DateTimeDefinitions.ThisPrefixRegex, RegexOptions.Singleline);

        public static readonly Regex PastPrefixRegex =
            new Regex(DateTimeDefinitions.PastPrefixRegex, RegexOptions.Singleline);

        public static readonly Regex NextPrefixRegex =
            new Regex(DateTimeDefinitions.NextPrefixRegex, RegexOptions.Singleline);

        public static readonly Regex AmRegex =
            new Regex(DateTimeDefinitions.AmRegex, RegexOptions.Singleline);

        public static readonly Regex PmRegex =
            new Regex(DateTimeDefinitions.PmRegex, RegexOptions.Singleline);

        public static readonly Regex[] RelativePrefixList =
        {
            ThisPrefixRegex, PastPrefixRegex, NextPrefixRegex,
        };

        public static readonly Regex[] AmPmRegexList =
        {
            AmRegex, PmRegex,
        };

        IEnumerable<Regex> IDateTimeAltExtractorConfiguration.RelativePrefixList => RelativePrefixList;

        IEnumerable<Regex> IDateTimeAltExtractorConfiguration.AmPmRegexList => AmPmRegexList;

        Regex IDateTimeAltExtractorConfiguration.OrRegex => OrRegex;

        Regex IDateTimeAltExtractorConfiguration.DayRegex => DayRegex;
    }
}
