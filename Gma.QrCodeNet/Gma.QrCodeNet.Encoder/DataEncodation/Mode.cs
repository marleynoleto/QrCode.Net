namespace Gma.QrCodeNet.Encoding.DataEncodation
{
    internal enum Mode
    {
        Numeric = 0001,
        Alphanumeric = 0001 << 1,
        EightbitByte = 0001 << 2,
        Kanji = 0001 << 3
    }
}
