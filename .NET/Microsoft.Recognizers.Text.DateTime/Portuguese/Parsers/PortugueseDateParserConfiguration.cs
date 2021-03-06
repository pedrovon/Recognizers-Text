﻿using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

using Microsoft.Recognizers.Definitions.Portuguese;
using Microsoft.Recognizers.Text.DateTime.Utilities;
using Microsoft.Recognizers.Text.Number;

namespace Microsoft.Recognizers.Text.DateTime.Portuguese
{
    public class PortugueseDateParserConfiguration : BaseOptionsConfiguration, IDateParserConfiguration
    {
        public string DateTokenPrefix { get; }

        public IExtractor IntegerExtractor { get; }

        public IExtractor OrdinalExtractor { get; }

        public IExtractor CardinalExtractor { get; }

        public IParser NumberParser { get; }

        public IDateTimeExtractor DurationExtractor { get; }

        public IDateExtractor DateExtractor { get; }

        public IDateTimeParser DurationParser { get; }

        public IImmutableDictionary<string, string> UnitMap { get; }

        public IEnumerable<Regex> DateRegexes { get; }

        public Regex OnRegex { get; }

        public Regex SpecialDayRegex { get; }

        public Regex SpecialDayWithNumRegex { get; }

        public Regex NextRegex { get; }

        public Regex ThisRegex { get; }

        public Regex LastRegex { get; }

        public Regex UnitRegex { get; }

        public Regex WeekDayRegex { get; }

        public Regex MonthRegex { get; }

        public Regex WeekDayOfMonthRegex { get; }

        public Regex ForTheRegex { get; }

        public Regex WeekDayAndDayOfMothRegex { get; }

        public Regex RelativeMonthRegex { get; }

        public Regex YearSuffix { get; }

        public Regex RelativeWeekDayRegex { get; }

        //TODO: implement the relative day regex if needed. If yes, they should be abstracted
        public static readonly Regex RelativeDayRegex = new Regex("", RegexOptions.Singleline);

        public static readonly Regex NextPrefixRegex = new Regex(DateTimeDefinitions.NextPrefixRegex, RegexOptions.Singleline);

        public static readonly Regex PastPrefixRegex = new Regex(DateTimeDefinitions.PastPrefixRegex, RegexOptions.Singleline);

        public IImmutableDictionary<string, int> DayOfMonth { get; }

        public IImmutableDictionary<string, int> DayOfWeek { get; }

        public IImmutableDictionary<string, int> MonthOfYear { get; }

        public IImmutableDictionary<string, int> CardinalMap { get; }

        public IDateTimeUtilityConfiguration UtilityConfiguration { get; }

        public PortugueseDateParserConfiguration(ICommonDateTimeParserConfiguration config) : base(config)
        {
            DateTokenPrefix = DateTimeDefinitions.DateTokenPrefix;
            DateRegexes = new PortugueseDateExtractorConfiguration(this).DateRegexList;
            OnRegex = PortugueseDateExtractorConfiguration.OnRegex;
            SpecialDayRegex = PortugueseDateExtractorConfiguration.SpecialDayRegex;
            SpecialDayWithNumRegex = PortugueseDateExtractorConfiguration.SpecialDayWithNumRegex;
            NextRegex = PortugueseDateExtractorConfiguration.NextDateRegex;
            ThisRegex = PortugueseDateExtractorConfiguration.ThisRegex;
            LastRegex = PortugueseDateExtractorConfiguration.LastDateRegex;
            UnitRegex = PortugueseDateExtractorConfiguration.DateUnitRegex;
            WeekDayRegex = PortugueseDateExtractorConfiguration.WeekDayRegex;
            MonthRegex = PortugueseDateExtractorConfiguration.MonthRegex;
            WeekDayOfMonthRegex = PortugueseDateExtractorConfiguration.WeekDayOfMonthRegex;
            ForTheRegex = PortugueseDateExtractorConfiguration.ForTheRegex;
            WeekDayAndDayOfMothRegex = PortugueseDateExtractorConfiguration.WeekDayAndDayOfMothRegex;
            RelativeMonthRegex = PortugueseDateExtractorConfiguration.RelativeMonthRegex;
            YearSuffix = PortugueseDateExtractorConfiguration.YearSuffix;
            RelativeWeekDayRegex = PortugueseDateExtractorConfiguration.RelativeWeekDayRegex;
            DayOfMonth = config.DayOfMonth;
            DayOfWeek = config.DayOfWeek;
            MonthOfYear = config.MonthOfYear;
            CardinalMap = config.CardinalMap;
            IntegerExtractor = config.IntegerExtractor;
            OrdinalExtractor = config.OrdinalExtractor;
            CardinalExtractor = config.CardinalExtractor;
            NumberParser = config.NumberParser;
            DurationExtractor = config.DurationExtractor;
            DateExtractor = config.DateExtractor;
            DurationParser = config.DurationParser;
            UnitMap = config.UnitMap;
            UtilityConfiguration = config.UtilityConfiguration;
        }

        public int GetSwiftDay(string text)
        {
            var trimmedText = text.Trim().ToLowerInvariant().Normalized();
            var swift = 0;

            //TODO: add the relative day logic if needed. If yes, the whole method should be abstracted.
            if (trimmedText.Equals("hoje") || trimmedText.Equals("este dia") || trimmedText.Equals("esse dia") || trimmedText.Equals("o dia"))
            {
                swift = 0;
            }
            else if (trimmedText.Equals("amanha") ||
                     trimmedText.Equals("de amanha") ||
                     trimmedText.EndsWith("dia seguinte") ||
                     trimmedText.EndsWith("o dia de amanha") ||
                     trimmedText.EndsWith("proximo dia"))
            {
                swift = 1;
            }
            else if (trimmedText.Equals("ontem"))
            {
                swift = -1;
            }
            else if (trimmedText.EndsWith("depois de amanha") ||
                     trimmedText.EndsWith("dia depois de amanha"))
            {
                swift = 2;
            }
            else if (trimmedText.EndsWith("anteontem") || 
                     trimmedText.EndsWith("dia antes de ontem"))
            {
                swift = -2;
            }
            else if (trimmedText.EndsWith("ultimo dia"))
            {
                swift = -1;
            }

            return swift;
        }

        public int GetSwiftMonth(string text)
        {
            var trimmedText = text.Trim().ToLowerInvariant();
            var swift = 0;

            if (NextPrefixRegex.IsMatch(trimmedText))
            {
                swift = 1;
            }

            if (PastPrefixRegex.IsMatch(trimmedText))
            {
                swift = -1;
            }

            return swift;
        }

        public bool IsCardinalLast(string text)
        {
            var trimmedText = text.Trim().ToLowerInvariant();
            return PastPrefixRegex.IsMatch(trimmedText);
        }

    }

    public static class StringExtension
    {
        public static string Normalized(this string text)
        {
            return text
                .Replace('á', 'a')
                .Replace('é', 'e')
                .Replace('í', 'i')
                .Replace('ó', 'o')
                .Replace('ú', 'u')
                .Replace("ê", "e")
                .Replace("ô", "o")
                .Replace("ü", "u")
                .Replace("ã", "a")
                .Replace("õ", "o")
                .Replace("ç", "c");
        }
    }
}
