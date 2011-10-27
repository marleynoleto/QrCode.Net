using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gma.QrCodeNet.Encoding.DataEncodation;

namespace Gma.QrCodeNet.Encoding.EncodingRegion
{
    /// <summary>
    /// 6.9 Format information
    /// The Format Information is a 15 bit sequence containing 5 data bits, with 10 error correction bits calculated using the (15, 5) BCH code.
    /// </summary>
    internal class FormatInformation
    {
        internal FormatInformation(ErrorCorrectionLevel errorLevel)
        {
            BitArray formatInformation = new BitArray(15);
            //formatInformation
        }


        //According Table 25 — Error correction level indicators
        //Using this bits as enum values would destroy thir order which currently correspond to error correction strength.
        internal static bool[] GetErrorCorrectionIndicatorBits(ErrorCorrectionLevel errorLevel)
        {
            //L 01
            //M 00
            //Q 11
            //H 10
            switch (errorLevel)
            {
                case ErrorCorrectionLevel.H:
                    return new[] {true, false};

                case ErrorCorrectionLevel.L:
                    return new[] { false, true };

                case ErrorCorrectionLevel.M:
                    return new[] { false, false };

                case ErrorCorrectionLevel.Q:
                    return new[] { true, true };
            }
            throw new ArgumentException(string.Format("Unsupported error correction level [{0}]", errorLevel), "errorLevel");
        }

        internal static bool[] GetMaskPatternIndicatorBits(Mode maskPattern)
        {
            //6.8.1 Data mask patterns
            bool[] threeBits = new bool[3];
            for (int i = 0; i < threeBits.Length; i++)
            {
               // (maskPattern)
            }
            throw new NotImplementedException();
        }

    }
}
