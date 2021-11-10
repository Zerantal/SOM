using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics.Contracts;
using System.Drawing.Imaging;


namespace Util
{
    static public class MiscMethods
    {

        public static Bitmap GrabScreenshot(string caption)
        {
            // Contract.Requires(caption != null);

            IntPtr winHandle;
            Bitmap winImage = null;
            Rectangle srcRect;

            try
            {
                winHandle = WinApiWrapper.FindWindow(caption);

                WinApiWrapper.SetForegroundWindow(winHandle);

                srcRect = WinApiWrapper.GetWindowRectangle(winHandle);

                int width = srcRect.Right - srcRect.Left;
                int height = srcRect.Bottom - srcRect.Top;

                if (width == 0 || height == 0)
                    throw new InvalidOperationException("Specified window has zero width or height.");

                winImage = new Bitmap(width, height, PixelFormat.Format32bppArgb);
                // Contract.Assume((winImage.PixelFormat & PixelFormat.Indexed) == 0); // don't know why this can't be proved :(
                Graphics screenG = Graphics.FromImage(winImage);

                screenG.CopyFromScreen(srcRect.Left, srcRect.Top,
                            0, 0, new Size(width, height),
                            CopyPixelOperation.SourceCopy);

                screenG.Dispose();
            }
            catch (WinApiException)
            {
                throw;
            }
            finally
            {
                if (winImage != null)
                    winImage.Dispose();
            }

            return winImage;
        }

        
        public static bool ImagesEqual(Bitmap lhs,Bitmap rhs, int allowedDifference)
        {
            // Contract.Requires(allowedDifference >= 0);
            // Contract.Requires(Is32bppImage(lhs) && Is32bppImage(rhs));
            // Contract.Requires(lhs != null && rhs != null);
            // Contract.Requires(lhs.PixelFormat == rhs.PixelFormat);

            int pixelDiff = 0;      // measured difference between images in pixels

            if (lhs == null || rhs == null)
                return false;
            if (lhs.Width != rhs.Width || lhs.Height != lhs.Height)
                return false;


            BitmapData bmd1 = lhs.LockBits(new Rectangle(0, 0, 10, 10), System.Drawing.Imaging.ImageLockMode.ReadOnly, lhs.PixelFormat);
            BitmapData bmd2 = rhs.LockBits(new Rectangle(0, 0, 10, 10), System.Drawing.Imaging.ImageLockMode.ReadOnly, rhs.PixelFormat);

            unsafe
            {
                for (int y = 0; y < bmd1.Height; y++)
                {
                    int* row1 = (int*)bmd1.Scan0 + (y * bmd1.Stride);
                    int* row2 = (int*)bmd2.Scan0 + (y * bmd2.Stride);
                    for (int x = 0; x < bmd1.Width; x++)
                    {
                        if (row1[x] != row2[x])
                            pixelDiff++;                        
                    }
                }
            }

            lhs.UnlockBits(bmd1);
            rhs.UnlockBits(bmd2);

            if (pixelDiff > allowedDifference)
                return false;
            else
                return true;
        }

        [Pure]
        public static bool Is32bppImage(Image im)
        {
            // Contract.Requires(im != null);
            
            if (im.PixelFormat == PixelFormat.Format32bppArgb ||
                im.PixelFormat == PixelFormat.Format32bppPArgb ||
                im.PixelFormat == PixelFormat.Format32bppRgb)
                return true;
            else
                return false;            
        }
    }
}
