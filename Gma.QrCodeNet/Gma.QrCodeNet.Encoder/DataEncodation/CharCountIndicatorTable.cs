namespace Gma.QrCodeNet.Encoding.DataEncodation
{
	public static class CharCountIndicatorTable
	{
		/// <remarks>ISO/IEC 18004:2000 Table 3 Page 18</remarks>
		public static int[] GetCharCountIndicator(Mode mode)
		{
			switch(mode)
			{
				case Mode.Numeric:
					return new int[]{10, 12, 14};
				case Mode.Alphanumeric:
					return new int[]{9, 11, 13};
				case Mode.EightBitByte:
					return new int[]{8, 16, 16};
				case Mode.Kanji:
					return new int[]{8, 10, 12};
				default:
					throw new System.InvalidOperationException(string.Format("Unexpected Mode: {0}", mode.ToString()));
			}
			
		} //
	}
}
