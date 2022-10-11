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
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace Intermediario
{
    /// <summary>
    /// Lógica de interacción para registro.xaml
    /// 
    /// </summary>
    public partial class registro : Window
    {

        DatosPersonales datosPersonales = new DatosPersonales();
        /// <summary>
        /// Metodo constructor para el formulario de registro
        /// </summary>
        public registro()
        {
            
            InitializeComponent();

            
        }
        /// <summary>
        /// Evento click del boton Registro.Obtiene todos los campos del formulario de registro
        /// y verifica que el correo con el que intenta registrarse no existe en la base de datos
        /// si no existe manda toda la informacion del cliente a la base de datos y carga los datos
        /// a la clase de datos personales
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIngresarRegistro_Click_1(object sender, RoutedEventArgs e)
        {
            

           
            string nombre = txtNombreRegistro.Text;
            string apellido = txtApellidoRegistro.Text;
            string contraseña = txtPasswordRegistro.Password;
            string cuenta = cmbCuentaRegistro.Text;
            string correo = txtCorreoRegistro.Text;
            string telefono = txtTelefonoRegistro.Text;
            string direccion = txtDireccionRegistro.Text;
            string consulta = "";

            try
            {

                //MessageBox.Show(nombre.Substring(0,2));

                RegistroCampos registroCampos = new RegistroCampos(nombre, apellido, contraseña, cuenta, correo, telefono, direccion);

                
                if (registroCampos.CamposLlenos() != 7)
                {
                    MessageBox.Show("Llene todos los campos.");
                }
                else if (txtPasswordRegistro.Password != txtConfirmarPass.Password)
                {
                    MessageBox.Show("Verifique las contraseñas");
                }
                else if (!registroCampos.ContrasenaSegura(contraseña))
                {
                    MessageBox.Show("La contraseña no es segura");
                }
                else if (!correo.Contains("@gmail.com"))
                {
                    MessageBox.Show("Correo no valido");
                }
                else if (telefono.Length != 8)
                {
                    MessageBox.Show("Numero de Telefono incorrecto");
                }
                else if (!registroCampos.VerificarNumeroMovil(telefono))
                {
                    MessageBox.Show("Numero de Telefono incorrecto");
                }
                else
                {
                    //MessageBox.Show(cmbCuentaRegistro.Text);
                    if (cmbCuentaRegistro.Text == "Comprador")
                    {
                        consulta = "EXECUTE REGISTRO @CORREO,@PRIMERNOMBRE,@APELLIDO,@CONTRASEÑA,@TELEFONO,@DIRECCION,1";

                    }
                    else if(cmbCuentaRegistro.Text == "Vendedor")
                    {
                        consulta = "EXECUTE REGISTRO @CORREO,@PRIMERNOMBRE,@APELLIDO,@CONTRASEÑA,@TELEFONO,@DIRECCION,2";
                    }

                    ConexionBD conexionBD = new ConexionBD();
                    SqlCommand cmd = new SqlCommand(consulta, conexionBD.conexion());
                    cmd.Parameters.AddWithValue("@CORREO", txtCorreoRegistro.Text);
                    cmd.Parameters.AddWithValue("@PRIMERNOMBRE", txtNombreRegistro.Text);
                    cmd.Parameters.AddWithValue("@APELLIDO", txtApellidoRegistro.Text);
                    cmd.Parameters.AddWithValue("@CONTRASEÑA", txtPasswordRegistro.Password);
                    cmd.Parameters.AddWithValue("@TELEFONO", txtTelefonoRegistro.Text);
                    cmd.Parameters.AddWithValue("@DIRECCION", txtDireccionRegistro.Text);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {


                    if (Convert.ToString(reader[0]) == "1")
                    {
                        MessageBox.Show("La cuenta ya existe");
                    }
                    else if (Convert.ToString(reader[0]) == "2")
                    {
                        Guardar_Datos_Personales();
                        datosPersonales.Id = Convert.ToString(reader[1]);
                        datosPersonales.TipoCuenta = 1;
                        MessageBox.Show("Registro exitoso.");
                        var window = new MarketplaceComprador();
                        this.Close();
                        window.Show();
                    }
                    else if (Convert.ToString(reader[0]) == "3")
                    {
                        Guardar_Datos_Personales();
                        datosPersonales.Id = Convert.ToString(reader[1]);
                        datosPersonales.TipoCuenta = 2;
                        MessageBox.Show("Registro exitoso.");
                        var window = new MarketplaceVendedor();
                        this.Close();
                        window.Show();
                    }
                }
                }
            }
            catch
            {
                MessageBox.Show("Error A-001. Fallo de Conexión.");
            }
        }
        /// <summary>
        /// Evento click del boton Cancelar del formulario de registro. Cierra el formulario
        /// de registro y regresa al formulario de Login
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelarRegistro_Click(object sender, RoutedEventArgs e)
        {
            var window = new MainWindow();
            this.Close();
            window.Show();  
        }
        /// <summary>
        /// Metodo que carga la informacion del cliente a la clase de DatosPersonales obtenida de los TextBox
        /// </summary>
        private void Guardar_Datos_Personales()
        {
            datosPersonales.Nombre = txtNombreRegistro.Text;
            datosPersonales.Apellido = txtApellidoRegistro.Text;
            datosPersonales.Correo = txtCorreoRegistro.Text;
            datosPersonales.Contraseña = txtPasswordRegistro.Password;
            datosPersonales.Telefono = txtTelefonoRegistro.Text;
            datosPersonales.Direccion = txtDireccionRegistro.Text;
        }
        /// <summary>
        /// Verifica que el usuario ingrese uno de los valores admitidos
        /// (Alfabeto)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNombreRegistro_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^A-Z | a-z]+").IsMatch(e.Text);
        }

        /// <summary>
        /// Evento KeyUp del textBox txtNombreRegistro.Cuando el usuario suelta el espacio
        /// este evento borra el espacio ingresado por el usuario.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNombreRegistro_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {

                txtNombreRegistro.Text = txtNombreRegistro.Text.Replace(" ", "");

                txtNombreRegistro.Select(txtNombreRegistro.Text.Length, 0);
            }
        }
        /// <summary>
        /// Evento KeyUp del textBox txtApellidoRegistro.Cuando el usuario suelta el espacio
        /// este evento borra el espacio ingresado por el usuario.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtApellidoRegistro_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {

                txtApellidoRegistro.Text = txtApellidoRegistro.Text.Replace(" ", "");

                txtApellidoRegistro.Select(txtApellidoRegistro.Text.Length, 0);
            }
        }
        /// <summary>
        /// Evento KeyUp del textBox txtCorreoRegistro.Cuando el usuario suelta el espacio
        /// este evento borra el espacio ingresado por el usuario.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCorreoRegistro_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {

                txtCorreoRegistro.Text = txtCorreoRegistro.Text.Replace(" ", "");

                txtCorreoRegistro.Select(txtCorreoRegistro.Text.Length, 0);
            }
        }

        /// <summary>
        /// Evento KeyUp del textBox txtTelefonoRegistro.Cuando el usuario suelta el espacio
        /// este evento borra el espacio ingresado por el usuario.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTelefonoRegistro_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {

                txtTelefonoRegistro.Text = txtTelefonoRegistro.Text.Replace(" ", "");

                txtTelefonoRegistro.Select(txtTelefonoRegistro.Text.Length, 0);
            }
        }

        /// <summary>
        /// Evento KeyUp del textBox txtDireccionRegistro.Cuando el usuario suelta el espacio
        /// este evento borra el espacio ingresado por el usuario.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtDireccionRegistro_KeyUp(object sender, KeyEventArgs e)
        {
            
        }

        /// <summary>
        /// Evento PreviewTextInput del textBox txtCorreoRegistro.
        /// Verifica que el usuario haya ingresado uno de los caracteres admitidos
        /// para insertarlo en el TextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCorreoRegistro_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^A-Z | a-z | @ | _ | - | . | 0-9]+").IsMatch(e.Text);
        }

        /// <summary>
        /// Evento PreviewTextInput del textBox txtPasswordRegistro.
        /// Verifica que el usuario haya ingresado uno de los caracteres admitidos
        /// para insertarlo en el TextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPasswordRegistro_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^A-Z | a-z | @ | _ | . | 0-9 | !\"#\\$%&'()+,-./:;=?@*\\[\\]^_{|}~]+").IsMatch(e.Text);
        }

        /// <summary>
        /// Evento MouseEnter del Boton btnMostrarPass.
        /// Cuando se activa este evento se muestra la contraseña ingresada por el usuario
        /// el TextBox txtMostrarPassword.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMostrarPass_MouseEnter(object sender, MouseEventArgs e)
        {
            txtMostrarPass.Visibility = Visibility.Visible;
            txtPasswordRegistro.Visibility = Visibility.Hidden;
            txtMostrarPass.Text = txtPasswordRegistro.Password;
        }

        private void txtMostrarPass_MouseLeave(object sender, MouseEventArgs e)
        {
            
        }

        /// <summary>
        /// Evento MouseLeave del Boton btnMostrarPass.
        /// Cuando se activa este evento se deja de mostrar la contraseña ingresada por el usuario en
        /// el TextBox txtMostrarPassword.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMostrarPass_MouseLeave(object sender, MouseEventArgs e)
        {
            txtMostrarPass.Visibility = Visibility.Hidden;
            txtPasswordRegistro.Visibility = Visibility.Visible;
            txtMostrarPass.Text = String.Empty;
        }

        private void txtTelefonoRegistro_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^[0-9]").IsMatch(e.Text);
        }

        private void btnMostrarConfirmarPass_MouseEnter(object sender, MouseEventArgs e)
        {
            txtMostrarPass_Copy.Visibility = Visibility.Visible;
            txtConfirmarPass.Visibility = Visibility.Hidden;
            txtMostrarPass_Copy.Text = txtConfirmarPass.Password;
        }
        private void btnMostrarConfirmarPass_MouseLeave(object sender, MouseEventArgs e)
        {
            txtMostrarPass_Copy.Visibility = Visibility.Hidden;
            txtConfirmarPass.Visibility = Visibility.Visible;
            txtMostrarPass_Copy.Text = String.Empty;
        }
    }
}
