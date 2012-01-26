using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Runtime.InteropServices;

namespace Gma.QrCodeNet.Encoding.Windows.WPF
{
    internal static class WriteableBitmapExtension
    {
        private const int s_SizeOfInt32 = 4;

        /// <summary>
        /// Clear whole bitmap with specific color
        /// </summary>
        /// <param name="wBitmap">WriteableBitmap, PixelFormats must be Pbgra32</param>
        /// <param name="color"></param>
        internal static void Clear(this WriteableBitmap wBitmap, Color color)
        {
            //WriteableBitmap's Pixels array uses Premultiplied ARGB32. 
            //We have to specify an alpha value other than 255(fully opaque)
            //3 other value(red, green and blue) need to be scaled based on the alpha value. 
            int a = color.A + 1;
            int col = ((0xFF & color.A) << 24)
                | ((0xFF & ((color.R * a) >> 8)) << 16)
                | ((0xFF & ((color.G * a) >> 8)) << 8)
                | (0xFF & ((color.B * a) >> 8));


            int pixelW = wBitmap.PixelWidth;
            int pixelH = wBitmap.PixelHeight;
            int totalPixels = pixelW * pixelH;


            wBitmap.Lock();
            unsafe
            {
                int* pixels = (int*)wBitmap.BackBuffer;
                *pixels = col;

                int pixelIndex = 1;
                int blockPixels = 1;
                while (pixelIndex < totalPixels)
                {
                    CopyUnmanagedMemory((IntPtr)pixels, 0, (IntPtr)pixels, pixelIndex * s_SizeOfInt32, blockPixels * s_SizeOfInt32);

                    pixelIndex += blockPixels;
                    blockPixels = Math.Min(2 * blockPixels, totalPixels - pixelIndex);
                }
            }
            wBitmap.AddDirtyRect(new Int32Rect(0, 0, wBitmap.PixelWidth, wBitmap.PixelHeight));
            wBitmap.Unlock();

        }

        internal static void FillRectangle(this WriteableBitmap wBitmap, Int32Rect rectangle, Color color)
        {
            //WriteableBitmap's Pixels array uses Premultiplied ARGB32. 
            //We have to specify an alpha value other than 255(fully opaque)
            //3 other value(red, green and blue) need to be scaled based on the alpha value. 
            int a = color.A + 1;
            int col = ((0xFF & color.A) << 24)
                | ((0xFF & ((color.R * a) >> 8)) << 16)
                | ((0xFF & ((color.G * a) >> 8)) << 8)
                | (0xFF & ((color.B * a) >> 8));

            int pixelW = wBitmap.PixelWidth;
            int pixelH = wBitmap.PixelHeight;

            if (rectangle.X >= pixelW
                || rectangle.Y >= pixelH
                || rectangle.X + rectangle.Width - 1 < 0
                || rectangle.Y + rectangle.Height - 1 < 0)
                return;

            if (rectangle.X < 0) rectangle.X = 0;
            if (rectangle.Y < 0) rectangle.Y = 0;
            if (rectangle.X + rectangle.Width - 1 >= pixelW) rectangle.Width = pixelW - rectangle.X;
            if (rectangle.Y + rectangle.Height - 1 >= pixelH) rectangle.Height = pixelH - rectangle.Y;

            wBitmap.Lock();
            unsafe
            {
                int* pixels = (int*)wBitmap.BackBuffer;

                //Fill first line
                int startPoint = rectangle.Y * pixelW + rectangle.X;
                int endBoundry = startPoint + rectangle.Width;

                pixels[startPoint] = col;

                int pixelIndex = startPoint + 1;
                int blockPixels = 1;

                int srcOffsetBytes = startPoint * s_SizeOfInt32;
                while (pixelIndex < endBoundry)
                {
                    CopyUnmanagedMemory((IntPtr)pixels, srcOffsetBytes, (IntPtr)pixels, pixelIndex * s_SizeOfInt32, blockPixels * s_SizeOfInt32);

                    pixelIndex += blockPixels;
                    blockPixels = Math.Min(2 * blockPixels, endBoundry - pixelIndex);
                }

                int bottomLeft = (rectangle.Y + rectangle.Height - 1) * pixelW + rectangle.X;

                for (pixelIndex = startPoint + pixelW; pixelIndex <= bottomLeft; pixelIndex += pixelW)
                {
                    CopyUnmanagedMemory((IntPtr)pixels, srcOffsetBytes, (IntPtr)pixels, pixelIndex * s_SizeOfInt32, rectangle.Width * s_SizeOfInt32);
                }

            }
            wBitmap.AddDirtyRect(rectangle);
            wBitmap.Unlock();

        }



        private static unsafe void CopyUnmanagedMemory(IntPtr src, int srcOffset, IntPtr dst, int dstOffset, int count)
        {
            byte* srcPtr = (byte*)src.ToPointer();
            srcPtr += srcOffset;
            byte* dstPtr = (byte*)dst.ToPointer();
            dstPtr += dstOffset;

            memcpy(dstPtr, srcPtr, count);
        }


        /// <summary>
        /// Copies characters between buffers
        /// </summary>
        /// <param name="dst">New buffer</param>
        /// <param name="src">Buffer to copy from</param>
        /// <param name="count">Number of character to copy</param>
        /// <returns></returns>
        [DllImport("msvcrt.dll", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
        private static extern unsafe byte* memcpy(
            byte* dst,
            byte* src,
            int count);
    }
}
