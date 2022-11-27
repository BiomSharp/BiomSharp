// BiomSharp: Copyright (c) Businessware Architects
// Licensed under the MIT License
// See: https://biomsharp.github.io/BiomSharp/LICENSE.txt

using System.Text.RegularExpressions;

namespace BiomSharp.Nist
{
    /// <summary>
    /// Finger code enumeration.
    /// </summary>
    /// <remarks>
    /// Interpol version 06.00.01 - based on ANSI/NIST ITL 2011 Upd 2015
    /// Table A.25.: Table of Generalized Fingerprint Position Codes
    /// Reference:
    /// https://github.com/INTERPOL-Innovation-Centre/ANSI-NIST-XML-ITL-Implementation
    /// </remarks>

    public enum NistFingerCode
    {
        /// <summary>
        /// Not defined, or invalid
        /// </summary>
        NullObject = -1,
        /// <summary>
        /// Finger: Unknown finger (references every finger position from 1 - 10, 16, and 17)
        /// </summary>
        AnyFinger = 0,
        /// <summary>
        /// Finger: right thumb
        /// </summary>
        RightThumb = 1,
        // Finger: right index
        RightIndex = 2,
        /// <summary>
        /// Finger: right middle
        /// </summary>
        RightMiddle = 3,
        /// <summary>
        /// Finger: right ring
        /// </summary>
        RightRing = 4,
        /// <summary>
        /// Finger: right little
        /// </summary>
        RightLittle = 5,
        /// <summary>
        /// Finger: left thumb
        /// </summary>
        LeftThumb = 6,
        /// <summary>
        /// Finger: left index
        /// </summary>
        LeftIndex = 7,
        /// <summary>
        /// Finger: left middle
        /// </summary>
        LeftMiddle = 8,
        /// <summary>
        /// Finger: left ring
        /// </summary>
        LeftRing = 9,
        /// <summary>
        /// Finger: left little
        /// </summary>
        LeftLittle = 10,
        /// <summary>
        /// Finger: plain right thumb
        /// </summary>
        PlainRightThumb = 11,
        /// <summary>
        /// Finger: plain left thumb
        /// </summary>
        PlainLeftThumb = 12,
        /// <summary>
        /// Finger: Plain right four fingers (may include extra digits)
        /// </summary>
        PlainRightFour = 13,
        /// <summary>
        /// Finger: Plain left four fingers (may include extra digits)
        /// </summary>
        PlainLeftFour = 14,
        /// <summary>
        /// Finger: Plain left and right thumbs
        /// </summary>
        PlainLeftRightThumbs = 15,
        /// <summary>
        /// Finger: Right extra digit
        /// </summary>
        RightExtraDigit = 16,
        /// <summary>
        /// Finger: Left extra digit
        /// </summary>
        LeftExtraDigit = 17,
        /// <summary>
        /// Unknown: friction ridge (not known whether the print is from a hand or foot)
        /// </summary>
        UnknownPrint = 18,
        /// <summary>
        /// Finger: EJI or tip (latent image that includes substantive portion of the medial or proximal
        /// segments of a finger, or the extreme tip of a fingerprint)
        /// </summary>
        FingerSegmentTip = 19,
        /// <summary>
        /// Palm: Unknown palm
        /// </summary>
        UnknownPalm = 20,
        // Palm: Right full palm (incl. upper and lower)
        RightFullPalm = 21,
        /// <summary>
        /// Palm: Right writer's palm
        /// </summary>
        RightWriterPalm = 22,
        /// <summary>
        /// Palm: Left full palm (incl. upper and lower)
        /// </summary>
        LeftFullPalm = 23,
        /// <summary>
        /// Palm: Left writer's palm
        /// </summary>
        LeftWriterPalm = 24,
        /// <summary>
        /// Palm: Right lower palm
        /// </summary>
        RightLowerPalm = 25,
        /// <summary>
        /// Palm: Right upper palm
        /// </summary>
        RightUpperPalm = 26,
        /// <summary>
        /// Palm: Left lower palm
        /// </summary>
        LeftLowerPalm = 27,
        /// <summary>
        /// Palm: Left upper palm
        /// </summary>
        LeftUpperPalm = 28,
        /// <summary>
        /// Palm: Right other palm
        /// </summary>
        RightOtherPalm = 29,
        /// <summary>
        /// Palm: Left other palm
        /// </summary>
        LeftOtherPalm = 30,
        /// <summary>
        /// Palm: Right inter-digital
        /// </summary>
        RightInterDigital = 31,
        /// <summary>
        /// Palm: Right thenar
        /// </summary>
        RightThenar = 32,
        /// <summary>
        /// Palm: Right hypo-thenar
        /// </summary>
        RightHypoThenar = 33,
        /// <summary>
        /// Palm: Left inter-digital
        /// </summary>
        LeftInterDigital = 34,
        /// <summary>
        /// Palm: Left thenar
        /// </summary>
        LeftThenar = 35,
        /// <summary>
        /// Palm: Left hypo-thenar
        /// </summary>
        LeftHypoThenar = 36,
        /// <summary>
        /// Palm: Right grasp
        /// </summary>
        RightGrasp = 37,
        /// <summary>
        /// Palm: Left grasp
        /// </summary>
        LeftGrasp = 38,
    };

    public static partial class NistFingerCodeEx
    {
        private static readonly string[] Descriptors = new string[]
        {
            /* 0*/ "Flat any finger",
            /* 1*/ "Flat right thumb",
            /* 2*/ "Flat right index finger",
            /* 3*/ "Flat right middle finger",
            /* 4*/ "Flat right ring finger",
            /* 5*/ "Flat right little finger",
            /* 6*/ "Flat left thumb",
            /* 7*/ "Flat left index finger",
            /* 8*/ "Flat left middle finger",
            /* 9*/ "Flat left ring finger",
            /*10*/ "Flat left little finger",
            /*11*/ "Plain right thumb",
            /*12*/ "Plain left thumb",
            /*13*/ "Plain right four fingers (may include extra digits)",
            /*14*/ "Plain left four fingers (may include extra digits)",
            /*15*/ "Plain left and right thumbs",
            /*16*/ "Right extra digit",
            /*17*/ "Left extra digit",
            /*18*/ "Unknown friction ridge (print may be from hand or foot)",
            /*19*/ "EJI or tip (latent)",
            /*20*/ "Unknown palm (references every listed palm print position)",
            /*21*/ "Right full palm (includes upper and lower)",
            /*22*/ "Right writer's palm",
            /*23*/ "Left full palm (includes upper and lower)",
            /*24*/ "Left writer's palm",
            /*25*/ "Right lower palm",
            /*26*/ "Right upper palm",
            /*27*/ "Left lower palm",
            /*28*/ "Left upper palm",
            /*29*/ "Right other palm",
            /*30*/ "Left other palm",
            /*31*/ "Right inter-digital",
            /*32*/ "Right thenar",
            /*33*/ "Right hypo-thenar",
            /*34*/ "Left inter-digital",
            /*35*/ "Left thenar",
            /*36*/ "Left hypo-thenar",
            /*37*/ "Right grasp",
            /*38*/ "Left grasp",
        };

        private const string SplitCamelCaseRegex = @"(?<=[a-z])([A-Z])|(?<=[A-Z])([A-Z][a-z])";

        public static string Name(this NistFingerCode fingerCode, bool allUpperCase = false)
        {
            Regex regex = new(SplitCamelCaseRegex, RegexOptions.None);
            string name = regex.Replace(fingerCode.ToString(), @" $1$2");
            return allUpperCase ? name.ToUpper() : name;
        }

        public static string MinimumName(this NistFingerCode fingerCode, bool allUpperCase = false)
        {
            Regex regex = new(SplitCamelCaseRegex, RegexOptions.None);
            string name = fingerCode.ToString();
            name = regex.Replace(name, @" $1$2");
            return allUpperCase ? name.ToUpper() : name;
        }

        public static string Descriptor(this NistFingerCode fingerCode, bool allUpperCase = false)
        {
            int index = (int)fingerCode;
            if (index < 0)
            {
                return "Invalid hand object";
            }

            string descriptor = Descriptors[index];
            return allUpperCase ? descriptor.ToUpper() : descriptor;
        }

        public static string[] Names(this NistFingerCode[] fingerCodes, bool allUpperCase = false)
            => (from fingerCode in fingerCodes select fingerCode.Name(allUpperCase)).ToArray();

        public static IDictionary<string, NistFingerCode> FingerCodesDictionary(
            this NistFingerCode[] fingerCodes, bool allUpperCase = false)
        {
            var dictionary = new Dictionary<string, NistFingerCode>();
            foreach (NistFingerCode fingerCode in fingerCodes)
            {
                dictionary.Add(fingerCode.Name(allUpperCase), fingerCode);
            }
            return dictionary;
        }

        public static IDictionary<string, NistFingerCode> FingerCodesMinDictionary(
            this NistFingerCode[] fingerCodes, bool allUpperCase = false)
        {
            var dictionary = new Dictionary<string, NistFingerCode>();
            foreach (NistFingerCode fingerCode in fingerCodes)
            {
                dictionary.Add(fingerCode.MinimumName(allUpperCase), fingerCode);
            }
            return dictionary;
        }

        public static KeyValuePair<string, NistFingerCode> FingerCodeDictionaryValue(
            this NistFingerCode fingerCode, bool allUpperCase = false) => new(
                fingerCode.Name(allUpperCase), fingerCode);

        public static NistFingerCode FingerCodeValue(
            this KeyValuePair<string, NistFingerCode> dictionaryValue) => dictionaryValue.Value;

        public static int SegmentCount(this NistFingerCode fingerCode) => fingerCode switch
        {
            NistFingerCode.NullObject => 0,
            NistFingerCode.PlainLeftFour or NistFingerCode.PlainRightFour => 4,
            NistFingerCode.PlainLeftRightThumbs => 2,
            NistFingerCode.AnyFinger => 1,
            NistFingerCode.RightThumb => 1,
            NistFingerCode.RightIndex => 1,
            NistFingerCode.RightMiddle => 1,
            NistFingerCode.RightRing => 1,
            NistFingerCode.RightLittle => 1,
            NistFingerCode.LeftThumb => 1,
            NistFingerCode.LeftIndex => 1,
            NistFingerCode.LeftMiddle => 1,
            NistFingerCode.LeftRing => 1,
            NistFingerCode.LeftLittle => 1,
            NistFingerCode.PlainRightThumb => 1,
            NistFingerCode.PlainLeftThumb => 1,
            NistFingerCode.RightExtraDigit => 1,
            NistFingerCode.LeftExtraDigit => 1,
            NistFingerCode.UnknownPrint => 1,
            NistFingerCode.FingerSegmentTip => 1,
            NistFingerCode.UnknownPalm => 1,
            NistFingerCode.RightFullPalm => 1,
            NistFingerCode.RightWriterPalm => 1,
            NistFingerCode.LeftFullPalm => 1,
            NistFingerCode.LeftWriterPalm => 1,
            NistFingerCode.RightLowerPalm => 1,
            NistFingerCode.RightUpperPalm => 1,
            NistFingerCode.LeftLowerPalm => 1,
            NistFingerCode.LeftUpperPalm => 1,
            NistFingerCode.RightOtherPalm => 1,
            NistFingerCode.LeftOtherPalm => 1,
            NistFingerCode.RightInterDigital => 1,
            NistFingerCode.RightThenar => 1,
            NistFingerCode.RightHypoThenar => 1,
            NistFingerCode.LeftInterDigital => 1,
            NistFingerCode.LeftThenar => 1,
            NistFingerCode.LeftHypoThenar => 1,
            NistFingerCode.RightGrasp => 1,
            NistFingerCode.LeftGrasp => 1,
            _ => throw new NotImplementedException(),
        };

        public static NistFingerCode[] SequenceValues(NistFingerCode startCode, NistFingerCode endCode)
        {
            if (startCode >= NistFingerCode.RightThumb &&
                endCode <= NistFingerCode.LeftLittle &&
                startCode <= endCode)
            {
                var codes = new NistFingerCode[endCode - startCode + 1];
                for (int i = (int)startCode; i <= (int)endCode; i++)
                {
                    codes[i - 1] = (NistFingerCode)i;
                }
                return codes;
            }
            throw new InvalidOperationException("FingerCode array bounds invalid.");
        }

        public static int[] FingersToEnumCodes(
            this IEnumerable<NistFingerCode> fingerCodes) => (
                from
                    fingerCode in fingerCodes
                where
                    fingerCode is >= NistFingerCode.RightThumb and
                    <= NistFingerCode.LeftLittle
                select
                    (int)fingerCode
            ).ToArray();

        public static NistFingerCode? EnumNameToFinger(string fingerName)
            => Enum.TryParse(fingerName, out NistFingerCode fingerCode)
                ?
                new NistFingerCode?(fingerCode) : null;

        public static NistFingerCode[] EnumNamesToFingers(
            this IEnumerable<string> fingerNames)
        {
            List<NistFingerCode> fingerCodes = new();
            foreach (string fingerName in fingerNames)
            {
                NistFingerCode? fingerCode = EnumNameToFinger(fingerName);
                if (fingerCode != null)
                {
                    fingerCodes.Add(fingerCode.Value);
                }
            }
            return fingerCodes.ToArray();
        }

        public static string[] FingersToEnumNames(
            this IEnumerable<NistFingerCode> fingerCodes)
        {
            List<string> names = new();
            foreach (NistFingerCode fingerCode in fingerCodes)
            {
                names.Add(fingerCode.ToString());
            }
            return names.ToArray();
        }

        // inSegmentOrder = true: order from left-to-right as scanned
        // inSegmentOrder = false: order in FingerCode order
        public static NistFingerCode[] GetSingleFingerCodes(
            this NistFingerCode scanObject,
            bool inSegmentOrder)
        {
            switch (scanObject)
            {
                case NistFingerCode.PlainRightFour:
                    return new NistFingerCode[]
                    {
                        NistFingerCode.RightIndex,
                        NistFingerCode.RightMiddle,
                        NistFingerCode.RightRing,
                        NistFingerCode.RightLittle
                    };
                case NistFingerCode.PlainLeftFour:
                    if (inSegmentOrder)
                    {
                        return new NistFingerCode[]
                        {
                            NistFingerCode.LeftLittle,
                            NistFingerCode.LeftRing,
                            NistFingerCode.LeftMiddle,
                            NistFingerCode.LeftIndex
                        };
                    }
                    else
                    {
                        return new NistFingerCode[]
                        {
                            NistFingerCode.LeftIndex,
                            NistFingerCode.LeftMiddle,
                            NistFingerCode.LeftRing,
                            NistFingerCode.LeftLittle
                        };
                    }
                case NistFingerCode.PlainLeftRightThumbs:
                    if (inSegmentOrder)
                    {
                        return new NistFingerCode[]
                        {
                            NistFingerCode.LeftThumb,
                            NistFingerCode.RightThumb
                        };
                    }
                    else
                    {
                        return new NistFingerCode[]
                        {
                            NistFingerCode.RightThumb,
                            NistFingerCode.LeftThumb
                        };
                    }
                case NistFingerCode.NullObject:
                case NistFingerCode.AnyFinger:
                case NistFingerCode.RightThumb:
                case NistFingerCode.RightIndex:
                case NistFingerCode.RightMiddle:
                case NistFingerCode.RightRing:
                case NistFingerCode.RightLittle:
                case NistFingerCode.LeftThumb:
                case NistFingerCode.LeftIndex:
                case NistFingerCode.LeftMiddle:
                case NistFingerCode.LeftRing:
                case NistFingerCode.LeftLittle:
                case NistFingerCode.PlainRightThumb:
                case NistFingerCode.PlainLeftThumb:
                case NistFingerCode.RightExtraDigit:
                case NistFingerCode.LeftExtraDigit:
                case NistFingerCode.UnknownPrint:
                case NistFingerCode.FingerSegmentTip:
                case NistFingerCode.UnknownPalm:
                case NistFingerCode.RightFullPalm:
                case NistFingerCode.RightWriterPalm:
                case NistFingerCode.LeftFullPalm:
                case NistFingerCode.LeftWriterPalm:
                case NistFingerCode.RightLowerPalm:
                case NistFingerCode.RightUpperPalm:
                case NistFingerCode.LeftLowerPalm:
                case NistFingerCode.LeftUpperPalm:
                case NistFingerCode.RightOtherPalm:
                case NistFingerCode.LeftOtherPalm:
                case NistFingerCode.RightInterDigital:
                case NistFingerCode.RightThenar:
                case NistFingerCode.RightHypoThenar:
                case NistFingerCode.LeftInterDigital:
                case NistFingerCode.LeftThenar:
                case NistFingerCode.LeftHypoThenar:
                case NistFingerCode.RightGrasp:
                case NistFingerCode.LeftGrasp:
                default:
                    return
                        scanObject is >= NistFingerCode.RightThumb
                        and
                        <= NistFingerCode.LeftLittle
                        ?
                        new NistFingerCode[] { scanObject }
                        :
                        Array.Empty<NistFingerCode>();
            }
        }

        public static int[] GetSegmentOrderIndexes(
            this NistFingerCode scanObject,
            IEnumerable<NistFingerCode> useCodes,
            bool reverseIndexOrder)
        {
            List<int> indexes = new();
            NistFingerCode[] scanCodes = scanObject.GetSingleFingerCodes(true);
            for (int i = 0; i < scanCodes.Length; i++)
            {
                if (useCodes.Contains(scanCodes[i]))
                {
                    indexes.Add(i);
                }
            }
            if (reverseIndexOrder)
            {
                indexes.Reverse();
            }
            return indexes.ToArray();
        }

        public static IEnumerable<NistFingerCode> GetSegmentOrderCodes(
            this NistFingerCode scanObject,
            IEnumerable<int> segmentIndexes)
        {
            if (segmentIndexes.Any())
            {
                NistFingerCode[] segmentCodes = scanObject.GetSingleFingerCodes(true);
                return from segmentIndex in segmentIndexes
                       select segmentCodes[segmentIndex];
            }
            return Array.Empty<NistFingerCode>();
        }

        public static bool IsSingleFinger(this NistFingerCode scanObject)
            => scanObject is >= NistFingerCode.AnyFinger
                and
                <= NistFingerCode.LeftLittle;
    }
}
