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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Intermediario
{
    /// <summary>
    /// Lógica de interacción para VerContraseña.xaml
    /// </summary>
    public partial class VerContraseña : Window
    {
        Boolean cambioContraseña = false;

        ConexionBD conexionBd = new ConexionBD();

        DatosPersonales datosPersonales = new DatosPersonales();

        RegistroCampos registroCampos = new RegistroCampos("a","b","c","d","e","1","d","e");

        public VerContraseña()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (cambioContraseña == false)
            {
                var window = new Perfil();
                window.Show();
            }
        }

        private void btnEditarPerfil_Click(object sender, RoutedEventArgs e)
        {
            if(txtPasswordActual.Password == "")
            {
                MessageBox.Show("La contraseña actual no puede estar vacio");
            }
            else if (txtPasswordNueva.Password =="") {
                MessageBox.Show("La nueva contraseña no puede estar vacio");
            }
            else if(txtPasswordActual.Password != datosPersonales.Contraseña)
            {
                MessageBox.Show("La contraseña actual no es correcta");
            }
            else if(txtPassNuevaConf.Password == "")
            {
                MessageBox.Show("La confirmacion de contraseña no puede estar vacio");
            }
            else if (txtPasswordNueva.Password != txtPassNuevaConf.Password) {
                MessageBox.Show("La nueva contraseña y la confirmacion deben ser iguales");
            }
            else if (!registroCampos.ContrasenaSegura(txtPasswordNueva.Password))
            {
                MessageBox.Show("La contraseña no es segura");
            }
            else
            {
                string consulta = "EXECUTE ActualizarPass @CONTRASEÑA,@CORREO,@TIPOCUENTA";

                ConexionBD conexionBD = new ConexionBD();

                SqlCommand cmd = new SqlCommand(consulta, conexionBD.conexion());
                cmd.Parameters.AddWithValue("@CONTRASEÑA", txtPasswordNueva.Password);
                cmd.Parameters.AddWithValue("@CORREO", datosPersonales.Correo);


                if (datosPersonales.TipoCuenta == 1)
                {
                    cmd.Parameters.AddWithValue("@TIPOCUENTA", 1);
                }
                if(datosPersonales.TipoCuenta == 2)
                {
                    cmd.Parameters.AddWithValue("@TIPOCUENTA", 2);
                }
                cmd.ExecuteScalar();

                cambioContraseña = true;

                MessageBox.Show("Se realizo el cambio exitosamente.");

                var window = new MainWindow();
                this.Close();
                window.Show();

            }
            

            
           
        }

        private void btnRegresarPerfil_Click(object sender, RoutedEventArgs e)
        {
            cambioContraseña = true;
            var window = new Perfil();
            this.Close();
            window.Show();
        }

        /// <summary>
        /// Muestra la contraseña actual ingresada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMostrarPass_MouseEnter(object sender, MouseEventArgs e)
        {
            txtMostrarPassActual.Visibility = Visibility.Visible;
            txtPasswordActual.Visibility = Visibility.Hidden;
            txtMostrarPassActual.Text = txtPasswordActual.Password;
        }

        /// <summary>
        /// Oculta la contraseña actual ingresada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMostrarPass_MouseLeave(object sender, MouseEventArgs e)
        {
            txtMostrarPassActual.Visibility = Visibility.Hidden;
            txtPasswordActual.Visibility = Visibility.Visible;
            txtMostrarPassActual.Text = String.Empty;
            txtPasswordActual.Focus();


        }

        /// <summary>
        /// Muestra la nueva contraseña ingresada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMostrarPassNueva_MouseEnter(object sender, MouseEventArgs e)
        {
            txtMostrarPassNueva.Visibility = Visibility.Visible;
            txtPasswordNueva.Visibility = Visibility.Hidden;
            txtMostrarPassNueva.Text = txtPasswordNueva.Password;
        }

        /// <summary>
        /// Oculta la nueva contraseña ingresada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMostrarPassNueva_MouseLeave(object sender, MouseEventArgs e)
        {
            txtMostrarPassNueva.Visibility = Visibility.Hidden;
            txtPasswordNueva.Visibility = Visibility.Visible;
            txtMostrarPassNueva.Text = String.Empty;
            txtPasswordNueva.Focus();
        }

        /// <summary>
        /// Muestra la confirmacion de la nueva contraseña ingresada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMostrarPassNuevaConf_MouseEnter(object sender, MouseEventArgs e)
        {
            txtMostrarPassNuevaConf.Visibility = Visibility.Visible;
            txtPassNuevaConf.Visibility = Visibility.Hidden;
            txtMostrarPassNuevaConf.Text = txtPassNuevaConf.Password;
        }

        private void btnMostrarPassNuevaConf_MouseLeave(object sender, MouseEventArgs e)
        {
            txtMostrarPassNuevaConf.Visibility = Visibility.Hidden;
            txtPassNuevaConf.Visibility = Visibility.Visible;
            txtMostrarPassNuevaConf.Text = String.Empty;
            txtPassNuevaConf.Focus();
        }
    }
}
