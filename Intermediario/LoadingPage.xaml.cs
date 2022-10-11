using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Intermediario
{
    /// <summary>
    /// Lógica de interacción para LoadingPage.xaml
    /// </summary>
    public partial class LoadingPage : Window
    {
        public LoadingPage()
        {
            InitializeComponent();

            meLoading.Source = new Uri(@"C:\Users\erick\Desktop\Intermediario-master_17-6-22\Intermediario\Imagenes\Loading\load.gif");
            meLoading.LoadedBehavior = MediaState.Play;


        }
    }
}
