using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Intermediario
{
    public class Imagenes
    {


        public BitmapImage imageFromByte(byte[] value)
        {
            var bytes = value;
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            if (bytes == null)
            {
                MessageBox.Show("Error");// Processing null case
            }
            else
            {

                using (var ms = new MemoryStream(bytes))
                {
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = ms;

                    image.EndInit();
                }
            }
            return image;
        }
    }
}
