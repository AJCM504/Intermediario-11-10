using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Intermediario
{
    /// <summary>
    /// Convertidor de imagenes
    /// </summary>
    public class Imagenes
    {

        /// <summary>
        /// convierte un arreglo de byte a una imagen.
        /// </summary>
        /// <param name="value">Arreglo de byte para convertir en imagen</param>
        /// <returns></returns>
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
