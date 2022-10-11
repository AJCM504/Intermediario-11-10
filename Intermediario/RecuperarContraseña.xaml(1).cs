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
    /// Lógica de interacción para RecuperarContraseña.xaml
    /// </summary>
    public partial class RecuperarContraseña : Window
    {
        public RecuperarContraseña()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string cuenta="";
            if (Convert.ToBoolean(Comprador.IsChecked))
            {
                cuenta = "Comprador";
            }
            if (Convert.ToBoolean(Vendedor.IsChecked))
            {
                cuenta = "Vendedor";
            }

            ConexionBD dconexion = new ConexionBD();
            MessageBox.Show(dconexion.recuperarContraseñaComprador(txtCorreo.Text,cuenta));
        }
    }
}
