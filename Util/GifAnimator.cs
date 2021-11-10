using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Diagnostics.Contracts;

namespace Util
{
    internal struct FrameInfo
    {
        private Image _frame;
        private int _timeDelay;

        internal FrameInfo(Image frame, int delay)
        {
            // Contract.Requires(frame != null);
            // Contract.Requires(delay >= 0);

            _frame = frame;
            _timeDelay = delay;
        }

        internal Image Frame
        {
            get
            {
                return _frame;
            }
        }

        internal int Delay
        {
            get
            {
                return _timeDelay;
            }
        }
    }

    public class GifAnimator
    {
        private Int16 maxWidth = 1, maxHeight = 1; // width and height of the biggest frame

        private Byte[] Header = { 0x47, 0x49, 0x46, 0x38, 0x39, 0x61 };  // GIF89a
        private Byte[] lsr = new Byte[7];       // logical screen descriptor

        private List<FrameInfo> frameData;

        public GifAnimator()
        {
            frameData = new List<FrameInfo>();
            lsr[4] = 0x77;     // no global colour table     
            lsr[5] = 0x00;
            lsr[6] = 0x00;
        }

        public void Clear()
        {
            frameData.Clear();
        }
        
        public void AddFrame(Image frame, int frameTime)
        {
            // Contract.Requires(frame != null);
            // Contract.Requires(frameTime >= 0);

            FrameInfo frameDetails = new FrameInfo((Image)frame.Clone(), frameTime);

            frameData.Add(frameDetails);

            if (frame.Width > maxWidth)
                maxWidth = (Int16)frame.Width;
            if (frame.Height > maxHeight)
                maxHeight = (Int16)frame.Height;
        }

        public void SaveFile(string fileName)
        {
            // Contract.Requires(fileName != null);                       
            FileStream outFile = null;
            Byte[] word = new Byte[2];

            try
            {
                outFile = new FileStream(fileName, FileMode.Create);

                using (BinaryWriter writer = new BinaryWriter(outFile))
                {
                    outFile = null;
                    writer.Write(Header);

                    // create logical screen descriptor
                    Array.Copy(BitConverter.GetBytes(maxWidth), word, word.Length);
                    lsr[0] = word[0];       //width
                    lsr[1] = word[1];
                    Array.Copy(BitConverter.GetBytes(maxHeight), word, word.Length);
                    lsr[2] = word[0];       // height
                    lsr[3] = word[1];
                    writer.Write(lsr);

                    // Process frames into data blocks
                    Byte[] animData = GenerateFrameData();

                    writer.Write(animData);

                    // write trailer
                    writer.Write(0x3B);
                }
            }
            catch (IOException ex)
            {
                throw new IOException("Unable to write gif animation", ex);
            }
            catch (ArgumentException ex)
            {
                    throw new ArgumentException(ex.Message, ex);
            }
            finally
            {                
                if (outFile != null)
                    outFile.Dispose();
            }           
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope"), ToDo("Need to alter this so that when a frame is converted to a gif, no halftoning happens")]
        private Byte[] GenerateFrameData()
        {
            // Contract.Ensures(// Contract.Result<Byte[]>() != null);

            MemoryStream data = null;            
            MemoryStream gifImage = null;
            Byte[] graphicControlExtension = new Byte[8];
            Byte[] imageDescriptor = new Byte[10];
            Byte[] globalColourTable = new Byte[768];
            Byte[] imageData;
            Byte[] outputData;
            int imageDataLength;

            data = new MemoryStream();
            for (int i = 0; i < frameData.Count; i++)
            {
                gifImage = new MemoryStream();               

                frameData[i].Frame.Save(gifImage, ImageFormat.Gif);
                gifImage.Seek(0, SeekOrigin.Begin);
    
                // extract relevant data from gif image
                gifImage.Seek(13, SeekOrigin.Current);
                gifImage.Read(globalColourTable, 0, 768);
                gifImage.Read(graphicControlExtension, 0, 8);
                gifImage.Read(imageDescriptor, 0, 10);
                imageDataLength = (int)(gifImage.Length - gifImage.Position - 1);
                if (imageDataLength >= 0)
                {
                    imageData = new byte[imageDataLength];
                    gifImage.Read(imageData, 0, imageDataLength);

                    // write out data               
                    Array.Copy(BitConverter.GetBytes(frameData[i].Delay), 0, graphicControlExtension, 4, 2); // set delay time for frame
                    data.Write(graphicControlExtension, 0, 8);

                    imageDescriptor[9] = 0x87;      // make decoder aware of local colour table
                    data.Write(imageDescriptor, 0, 10);

                    data.Write(globalColourTable, 0, 768);
                    data.Write(imageData, 0, imageDataLength);
                }
                if (gifImage != null)
                {
                    gifImage.Dispose();
                    gifImage = null;
                }
            }

            if (data != null)
            {
                data.Close();
            }

            outputData = data.ToArray();

            if (outputData == null)
                return new Byte[0];

            return outputData; 
        }
    }
}
