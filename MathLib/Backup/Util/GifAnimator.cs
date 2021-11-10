using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Util
{
    [TODO("This class is not very robust")]   
    public class GifAnimator
    {        
        private int maxWidth = 1, maxHeight = 1; // width and height of the biggest frame
        private MemoryStream memoryStream;
        
        Byte[] ApplicationExtensionBlock = { 0x21, 0xFF, 0x0B, 75, 69, 84, 83, 67, 65, 80,
                                             69, 50, 46, 48, 48, 0x03, 0x01, 0x01, 0x00, 0x00};
        Byte[] GlobalColorTable;    // this includes file header as well;

        // Header information to be inserted before each frame
        Byte[] ImageControlHeader = { 33, 249, 4, 9, 28, 0, 0, 0 }; 

        public GifAnimator()
        {
            MemoryStream tmpStream = new MemoryStream();

            // temporary image used to generate header and global color table
            Bitmap tmpImage = new Bitmap(1, 1, PixelFormat.Format8bppIndexed);
            tmpImage.Save(tmpStream, ImageFormat.Gif);
            GlobalColorTable = new Byte[781];
            tmpStream.Seek(0, SeekOrigin.Begin);
            tmpStream.Read(GlobalColorTable, 0, 781);

            memoryStream = new MemoryStream();
            WriteHeaderInfo();

        }

        public void Clear()
        {
            memoryStream = new MemoryStream();
            WriteHeaderInfo();
        }
        
        public void AddFrame(Image frame, UInt16 frameTime)
        {           
            MemoryStream tmpStream = new MemoryStream();
            Byte[]  buf;
            frame.Save(tmpStream, ImageFormat.Gif);
            buf = tmpStream.ToArray();

            // add delay to frame
            ImageControlHeader[4] = Convert.ToByte(frameTime & 0x00FF);
            ImageControlHeader[5] = Convert.ToByte((frameTime & 0xFF00) >> 8);

            if (frame.Width > maxWidth)
                maxWidth = frame.Width;
            if (frame.Height > maxHeight)
                maxHeight = frame.Height;
                                    
            memoryStream.Write(ImageControlHeader, 0, 8);
            memoryStream.Write(buf, 789, buf.Length - 790);
        }

        public void SaveFile(string fileName)
        {
            BinaryWriter writer = null;

            try
            {
                writer = new BinaryWriter(new FileStream(fileName, FileMode.Create));

                Byte[] data = memoryStream.ToArray();

                // resize logical screen descripter to width and height of largest frame
                data[6] = Convert.ToByte(maxWidth & 0x000000FF);
                data[7] = Convert.ToByte((maxWidth & 0x0000FF00) >> 8);
                data[8] = Convert.ToByte(maxHeight & 0x000000FF);
                data[9] = Convert.ToByte((maxHeight & 0x0000FF00) >> 8);

                writer.Write(data, 0, data.Length);
                writer.Write((Byte)0x3B);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }       
        }

        private void WriteHeaderInfo()
        {
            memoryStream.Write(GlobalColorTable, 0, 781); // write header
            memoryStream.Write(ApplicationExtensionBlock, 0, 19);
        }       
    }
}
