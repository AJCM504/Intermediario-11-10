using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using Microsoft.VisualBasic;

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
        
        /// <summary>
        /// Metodo Constructo del formulario Perfil
        /// Obtiene los datos personales del comprador de la clase DatosPersonales
        /// y los muestra en los TextBox respectivos
        /// </summary>
        public Perfil()
        {
            InitializeComponent();

            //MessageBox.Show(datosPersonales.Nombre);
            txtNombrePerfil.Text = datosPersonales.Nombre;
            txtApellidoPerfil.Text = datosPersonales.Apellido;
           
            txtCorreoPerfil.Text = datosPersonales.Correo;
            txtTelefonoPerfil.Text = datosPersonales.Telefono;
            txtDireccionPerfil.Text = datosPersonales.Direccion;

            if(datosPersonales.TipoCuenta == 1) { txtTipoCuenta.Text = "Comprador"; }
            if(datosPersonales.TipoCuenta == 2) { txtTipoCuenta.Text = "Vendedor"; }

            txtCorreoPerfil.IsEnabled = false;

        }

        /// <summary>
        /// Evento click del boton btnEditarPerfil.
        /// Verifica que todos los datos esten llenado correctamente.
        /// Posteriormente actualiza los datos en la base de datos y la clase DatosPersonales.
        /// Luego cierra el formulario Perfil y muestra el formulario MarketplaceComprador o MarketplaceVendedor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditarPerfil_Click(object sender, RoutedEventArgs e)
        {

            RegistroCampos registroCampos = new RegistroCampos(txtNombrePerfil.Text,txtApellidoPerfil.Text, datosPersonales.Contraseña, "true", txtCorreoPerfil.Text, txtTelefonoPerfil.Text, txtDireccionPerfil.Text);


            if (registroCampos.CamposLlenos() != 7)
            {
                System.Windows.MessageBox.Show("Rellene todos los campos");
            }
            

            else if (txtTelefonoPerfil.Text.Length != 8)
            {
                System.Windows.MessageBox.Show("Numero de Telefono incorrecto");
            }
            else if (!registroCampos.VerificarNumeroMovil(txtTelefonoPerfil.Text))
            {
                System.Windows.MessageBox.Show("Numero de Telefono incorrecto");
            }
            else
            {

                if (datosPersonales.TipoCuenta == 1)
                {
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
                cmd.Parameters.AddWithValue("@contrasena", datosPersonales.Contraseña);
                cmd.Parameters.AddWithValue("telefono", txtTelefonoPerfil.Text);
                cmd.Parameters.AddWithValue("direccion", txtCorreoPerfil.Text);

                datosPersonales.Nombre = txtNombrePerfil.Text;
                datosPersonales.Apellido = txtApellidoPerfil.Text;
               
                datosPersonales.Telefono = txtTelefonoPerfil.Text;
                datosPersonales.Direccion = txtDireccionPerfil.Text;


                cmd.ExecuteScalar();

                if (datosPersonales.TipoCuenta == 1)
                {
                    var window = new MarketplaceComprador();
                    this.Close();
                    window.Show();
                }

                if (datosPersonales.TipoCuenta == 2)
                {
                    var window = new MarketplaceVendedor();
                    this.Close();
                    window.Show();
                }

            }
        }

        /// <summary>
        /// Evento click del boton btnRegresarPerfil.
        /// Cierra el formulario Perfil y muestra el formulario MarketplaceComprador o MarketplaceVendedor.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Evento click del boton btnAceptarPerfil.
        /// Muestra un messageBox para confirmar la accion del usuario.
        /// Posteriormente si se confirma la accion se desactiva la cuenta desde la base de datos
        /// y se cierra el formulario Perfil y se muestra el formulario MainWindow.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        private void txtNombrePerfil_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^A-Z | a-z]+").IsMatch(e.Text);
        }

        private void txtTelefonoPerfil_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        private void txtDireccionPerfil_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9 | . | _ | A-Z | a-z]+").IsMatch(e.Text);
        }

        private void txtDireccionPerfil_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {

        }

        private void txtNombrePerfil_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            txtNombrePerfil.Text = txtNombrePerfil.Text.Replace(" ", "");

            txtNombrePerfil.Select(txtNombrePerfil.Text.Length, 0);
        }

        private void txtApellidoPerfil_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtApellidoPerfil_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            txtApellidoPerfil.Text = txtApellidoPerfil.Text.Replace(" ", "");

            txtApellidoPerfil.Select(txtApellidoPerfil.Text.Length, 0);
        }

        private void txtPasswordRegistro_Copy_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^A-Z | a-z | @ | _ | - | . | 0-9]+").IsMatch(e.Text);
        }

        private void txtCorreoPerfil_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^A-Z | a-z | @ | _ | - | . | 0-9]+").IsMatch(e.Text);
        }

        private void txtCorreoPerfil_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            txtCorreoPerfil.Text = txtCorreoPerfil.Text.Replace(" ", "");

            txtCorreoPerfil.Select(txtCorreoPerfil.Text.Length, 0);
        }

        private void txtTelefonoPerfil_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            txtTelefonoPerfil.Text = txtTelefonoPerfil.Text.Replace(" ", "");

            txtTelefonoPerfil.Select(txtTelefonoPerfil.Text.Length, 0);
        }

        private void btnMostrarPass_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            
        }

        private void btnMostrarPass_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            
        }

        private void btnCambiarPass_Click(object sender, RoutedEventArgs e)
        {

            var window = new VerContraseña();
            this.Close();
            window.Show();
        }
    }
}
