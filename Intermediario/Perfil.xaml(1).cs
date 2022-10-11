using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Intermediario
{
    /// <summary>
    /// Interaction logic for Perfil.xaml
    /// </summary>
    public partial class Perfil : Window
    {

        ConexionBD conexionBD=new ConexionBD();
        String consulta;

        DatosPersonales datosPersonales = new DatosPersonales();
        

        public Perfil()
        {
            InitializeComponent();

            //MessageBox.Show(datosPersonales.Nombre);
            txtNombrePerfil.Text = datosPersonales.Nombre;
            txtApellidoPerfil.Text = datosPersonales.Apellido;
            txtPasswordRegistro_Copy.Password = datosPersonales.Contraseña;
            txtCorreoPerfil.Text = datosPersonales.Correo;
            txtTelefonoPerfil.Text = datosPersonales.Telefono;
            txtDireccionPerfil.Text = datosPersonales.Direccion;

        }

        private void btnEditarPerfil_Click(object sender, RoutedEventArgs e)
        {
            if (datosPersonales.TipoCuenta == 1) {
                consulta = "UPDATE [Datos Comprador] set NombreComprador=@nombre, ApellidoComprador=@apellido,CorreoComprador=@corre,ContrasenaComprador=@contrasena,TelefonoComprador=@telefono,DireccionComprador=@direccion where IDCuentaComprador=@ID";
            }
            else if (datosPersonales.TipoCuenta == 2)
            {
                consulta = "UPDATE [Datos Vendedor] set NombreVendedor=@nombre, ApellidoVendedor=@apellido,CorreoVendedor=@corre,ContrasenaVendedor=@contrasena,TelefonoVendedor=@telefono,DireccionVendedor=@direccion where IDCuentaVendedor=@ID";
            }

            SqlCommand cmd = new SqlCommand(consulta, conexionBD.conexion());
            cmd.Parameters.AddWithValue("@ID", datosPersonales.Id);
            cmd.Parameters.AddWithValue("@nombre", txtNombrePerfil.Text);
            cmd.Parameters.AddWithValue("@apellido", txtApellidoPerfil.Text);
            cmd.Parameters.AddWithValue("@corre", txtCorreoPerfil.Text);
            cmd.Parameters.AddWithValue("@contrasena", txtPasswordRegistro_Copy.Password);
            cmd.Parameters.AddWithValue("telefono", txtTelefonoPerfil.Text);
            cmd.Parameters.AddWithValue("direccion", txtCorreoPerfil.Text);


            cmd.ExecuteScalar();

            if(datosPersonales.TipoCuenta == 1)
            {
                var window = new MarketplaceComprador();
                this.Close();
                window.Show();
            }

            if(datosPersonales.TipoCuenta == 2)
            {
                var window = new MarketplaceVendedor();
                this.Close();
                window.Show();
            }

        }

        private void btnRegresarPerfil_Click(object sender, RoutedEventArgs e)
        {

            if(datosPersonales.TipoCuenta ==1)
            {
                var window = new MarketplaceComprador();
                window.Show();
                this.Close();
            }
            if (datosPersonales.TipoCuenta == 2)
            {
                var window = new MarketplaceVendedor();
                window.Show();
                this.Close();
            }
            
        }

        private void btnAceptarPerfil_Click(object sender, RoutedEventArgs e)
        {


            DialogResult res = (DialogResult)System.Windows.MessageBox.Show("¿Seguro que desea borrar su cuenta? Se perderá permanentemente.","Confirmación", MessageBoxButton.OKCancel);

            if(res == System.Windows.Forms.DialogResult.OK)
            {
                if (datosPersonales.TipoCuenta == 1)
                {
                    consulta = "update [Datos Comprador] set IDEstadoCuenta=@estadoCuenta where IDCuentaComprador=@id";
                }
                else
                {
                    consulta = "update [Datos Vendedor] set IDEstadoCuenta=@estadoCuenta where IDCuentaVendedor=@id";
                }
               

                ConexionBD conexionBD = new ConexionBD();

                SqlCommand cmd = new SqlCommand(consulta, conexionBD.conexion());
                cmd.Parameters.AddWithValue("@estadoCuenta", 2);
                cmd.Parameters.AddWithValue("@id", datosPersonales.Id);

                cmd.ExecuteScalar();
                System.Windows.MessageBox.Show("Borrando...");

                var window = new MainWindow();
                this.Close();
                window.Show();
            }

        }
    }
}
