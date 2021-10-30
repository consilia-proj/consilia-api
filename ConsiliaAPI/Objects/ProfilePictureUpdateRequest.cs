using System;
using System.IO;
using System.Threading.Tasks;
using nQuant;
using System.Drawing;
using System.Drawing.Imaging;
using ImageMagick;

namespace ConsiliaAPI.Objects
{
    public class ProfilePictureUpdateRequest
    {
        public string Picture { get; set; }

        public async Task<string> OptimizedImage()
        {
            if ((Picture.Length > 0))
            {

                await using (var memoryStream = new MemoryStream(Convert.FromBase64String(Picture)))
                {
                    if (Picture.Substring(0, 7).Equals("AAAAJGZ"))
                    {
                        using (var heicImg = new MagickImage(memoryStream))
                        {
                            heicImg.Format = MagickFormat.Jpeg;
                            heicImg.Write(memoryStream);
                        }
                    }

                    using (var img = Image.FromStream(memoryStream))
                    {

                        using (var compressedImage = new WuQuantizer().QuantizeImage(new Bitmap(img)))
                        {
                            byte[] imageBytes = null;
                            await using (var ms = new MemoryStream())
                            {
                                compressedImage.Save(ms, ImageFormat.Jpeg);
                                imageBytes = ms.ToArray();
                            }

                            return Convert.ToBase64String(imageBytes);
                        }
                    }
                }
            }

            return Picture;
        }
    }
}