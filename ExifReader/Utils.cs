using System.Drawing;
using System.IO;

namespace ExifReader
{
	public static class Utils
	{
		public static byte[] imageToArray(Image im)
		{
			MemoryStream ms = new MemoryStream();
			return ms.ToArray();
		}
        public static byte[] GenerateImage(byte[] imgData, int largestSide)
        {
            byte[] ReturnedThumbnail;

            using (MemoryStream StartMemoryStream = new MemoryStream(), NewMemoryStream = new MemoryStream())
            {
                // write the string to the stream  
                StartMemoryStream.Write(imgData, 0, imgData.Length);

                // create the start Bitmap from the MemoryStream that contains the image  
                Bitmap startBitmap = new Bitmap(StartMemoryStream);

                // set thumbnail height and width proportional to the original image.  
                int newHeight;
                int newWidth;
                double HW_ratio;
                if (startBitmap.Height > startBitmap.Width)
                {
                    newHeight = largestSide;
                    HW_ratio = largestSide / (double)startBitmap.Height;
                    newWidth = (int)(HW_ratio * startBitmap.Width);
                }
                else
                {
                    newWidth = largestSide;
                    HW_ratio = largestSide / (double)startBitmap.Width;
                    newHeight = (int)(HW_ratio * startBitmap.Height);
                }

                // create a new Bitmap with dimensions for the thumbnail.  
                Bitmap newBitmap = new Bitmap(newWidth, newHeight);

                // Copy the image from the START Bitmap into the NEW Bitmap.  
                // This will create a thumnail size of the same image.  
                newBitmap = ResizeImage(startBitmap, newWidth, newHeight);

                // Save this image to the specified stream in the specified format.  
                newBitmap.Save(NewMemoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);

                // Fill the byte[] for the thumbnail from the new MemoryStream.  
                ReturnedThumbnail = NewMemoryStream.ToArray();
            }

            // return the resized image as a string of bytes.  
            return ReturnedThumbnail;
        }
        // Resize a Bitmap  
        private static Bitmap ResizeImage(Bitmap image, int width, int height)
        {
            Bitmap resizedImage = new Bitmap(width, height);
            using (Graphics gfx = Graphics.FromImage(resizedImage))
            {
                gfx.DrawImage(image, new Rectangle(0, 0, width, height),
                    new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);
            }
            return resizedImage;
        } 
	}
}