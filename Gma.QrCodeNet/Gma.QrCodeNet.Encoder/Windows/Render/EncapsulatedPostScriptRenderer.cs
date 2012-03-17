using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Globalization;

namespace Gma.QrCodeNet.Encoding.Windows.Render
{
	public class EncapsulatedPostScriptRenderer
	{
		private Color m_DarkColor;

		private Color m_LightColor;

		private QuietZoneModules m_QuietZoneModules;

		/// <summary>
		/// Initializes a Encapsulated PostScript renderer.
		/// </summary>
		/// <param name="darkColor">DarkColor used to draw Dark modules of the QrCode</param>
		/// <param name="lightColor">LightColor used to draw Light modules and QuietZone of the QrCode.
		/// Setting to Color.Transparent allows transparent light modules so the QR Code blends in the existing background.
		/// In that case the existing background should remain light and rather uniform, and higher error correction levels are recommended.</param>
		/// <param name="quietZoneModules"></param>
		public EncapsulatedPostScriptRenderer(Color darkColor, Color lightColor, QuietZoneModules quietZoneModules)
		{
			m_DarkColor = darkColor;
			m_LightColor = lightColor;
			m_QuietZoneModules = quietZoneModules;
		}

		/// <summary>
		/// Renders the matrix in an Encapsuled PostScript format.
		/// </summary>
		/// <param name="matrix">The matrix to be rendered</param>
		/// <param name="moduleSize">Size in points (1 inch contains 72 point in PostScript) of a module</param>
		/// <param name="stream">Output text stream</param>
		public void WriteToStream(BitMatrix matrix, double moduleSize, StreamWriter stream)
		{
			string strHeader = @"%!PS-Adobe-3.0 EPSF-3.0
%%Creator: Gma.QrCodeNet
%%Title: QR Code
%%CreationDate: {0:yyyyMMdd}
%%Pages: 1
%%BoundingBox: 0 0 {1} {2}
%%Document-Fonts: Times-Roman
%%LanguageLevel: 1
%%EndComments
%%BeginProlog
/w {{ {3} }} def
/h {{ {4} }} def
/q {{ {5} }} def
/s {{ {6} }} def
/W {{ w q q add add }} def
/H {{ h q q add add }} def
% Define the box function taking X and Y coordinates of the top left corner and filling a 1 point large square
/b {{ newpath moveto 1 0 rlineto 0 1 rlineto -1 0 rlineto closepath fill }} def
%%EndProlog
%%Page: 1 1

% Save the current state
save

% Invert the Y axis
0 W s mul translate
s s neg scale";

			string strBackground = @"
% Create the background
{0} 255 div {1} 255 div {2} 255 div setrgbcolor
newpath 0 0 moveto W 0 rlineto 0 H rlineto W neg 0 rlineto closepath fill";

			string strSquaresHeader = @"
% Draw squares
{0} 255 div {1} 255 div {2} 255 div setrgbcolor
q q translate";

			string strFooter = @"
% Restore the initial state
restore showpage
%
% End of page
%
%%Trailer
%%EOF";

			stream.WriteLine(string.Format(strHeader,
				DateTime.UtcNow,
				// Use invariant culture to ensure that the dot is used as the decimal separator
				(moduleSize * (matrix.Width + (int)m_QuietZoneModules * 2)).ToString(CultureInfo.InvariantCulture.NumberFormat),
				(moduleSize * (matrix.Height + (int)m_QuietZoneModules * 2)).ToString(CultureInfo.InvariantCulture.NumberFormat),
				matrix.Width,
				matrix.Height,
				(int)m_QuietZoneModules,
				moduleSize.ToString(CultureInfo.InvariantCulture.NumberFormat)));

			if (LightColor != Color.Transparent)
				stream.WriteLine(string.Format(strBackground,
					LightColor.R,
					LightColor.G,
					LightColor.B));

			#region Draw squares for each dark module
			stream.WriteLine(string.Format(strSquaresHeader,
				DarkColor.R,
				DarkColor.G,
				DarkColor.B));

			for (int y = 0; y < matrix.Height; ++y)
				for (int x = 0; x < matrix.Width; ++x)
					if (matrix[x, y])
						// Output the coordinates of the upper left corner and call to the box function
						stream.WriteLine(string.Format("{0} {1} b", x, y));
			#endregion

			stream.Write(strFooter);
		}

		/// <summary>
		/// DarkColor used to draw Dark modules of the QrCode
		/// </summary>
		public Color DarkColor
		{
			set
			{
				m_DarkColor = value;
			}
			get
			{
				return m_DarkColor;
			}
		}

		/// <summary>
		/// LightColor used to draw Light modules and QuietZone of the QrCode.
		/// Setting to Color.Transparent allows transparent light modules so the QR Code blends in the existing background.
		/// In that case the existing background should remain light and rather uniform, and higher error correction levels are recommended.
		/// </summary>
		public Color LightColor
		{
			set
			{
				m_LightColor = value;
			}
			get
			{
				return m_LightColor;
			}
		}

		/// <summary>
		/// Number of surroungind modules forming a quiet zone intended to improve detection by reducing noise.
		/// </summary>
		public QuietZoneModules QuietZoneModules
		{
			get
			{
				return m_QuietZoneModules;
			}
			set
			{
				m_QuietZoneModules = value;
			}
		}
	}
}