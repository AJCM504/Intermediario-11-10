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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using Intermediario.Facturas;

using Intermediario;
using System.Text.RegularExpressions;

namespace Intermediario
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Muestra el formulario de registro
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Mostramos el formulario de registros
            var window = new registro();
            window.Show();
            this.Close();
        }

        /// <summary>
        /// Verifica el correo y la contraseña
        /// para entrar al programa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIngresarLogin_Click(object sender, RoutedEventArgs e)
        {
           

            String validacion;
            String correo = txtUsuarioLogin.Text;
            String contraseña = txtPasswordLogin.Password;

            //Ejecucion de la validación de correo y contraseña.
            try
            {
                //Ejecutamos el procedimmiento almacenado de login
                //Pasando como parametros el usuario y la contraseña

                ConexionBD dbConexion = new ConexionBD();
                string consulta = "execute LoginValidation @correo,@password";
                SqlCommand cmd = new SqlCommand(consulta, dbConexion.conexion());
                cmd.Parameters.AddWithValue("@password", contraseña);
                cmd.Parameters.AddWithValue("@correo", correo);

                //El resultado del procedimiento almacenado nos retorna
                //Un 1 si el usuario y la contraseña concuerda y pertenece a la tabla de compradores 
                //y nos retorna un 2 si el usuario y la contraseña concuerda y pertenece a la tabla de vendedores
                //y este valor lo asignamos a la variable validacion
                validacion = Convert.ToString(cmd.ExecuteScalar());

                //Comprador
                if (validacion == "1")
                {
                    //Obtenemos los datos personales del comprador de la base de datos.
                    consulta = "Select * from [Datos Comprador] where CorreoComprador = @correo";
                    ConexionBD dbConexion2 = new ConexionBD();

                    SqlCommand cmd2 = new SqlCommand(consulta, dbConexion2.conexion());
                    cmd2.Parameters.AddWithValue("@correo", txtUsuarioLogin.Text);
                    SqlDataReader reader = cmd2.ExecuteReader();

                    DatosPersonales datosPersonales = new DatosPersonales();

                    //Cargar informacion del Usuario a la clase Datos Personales
                    if (reader.Read())
                    {

                        datosPersonales.TipoCuenta = 1;
                        datosPersonales.Nombre = Convert.ToString(reader[1]);
                        datosPersonales.Apellido = Convert.ToString(reader[2]);
                        datosPersonales.Correo = Convert.ToString(reader[3]);
                        datosPersonales.Contraseña = Convert.ToString(reader[4]);
                        datosPersonales.Telefono = Convert.ToString(reader[5]);
                        datosPersonales.Direccion = Convert.ToString(reader[6]);
                        datosPersonales.Id = Convert.ToString(reader[0]);

                        //MessageBox.Show(Convert.ToString(reader[1]));
                    }

                    //Mostramos el formulario de comprador.
                    var window = new MarketplaceComprador();
                    this.Close();
                    window.Show();

                }
                //Vendedor
                else if (validacion == "2")
                {

                    //Obtenemos los datos personales del vendedor de la base de datos.
                    consulta = "Select * from [Datos Vendedor] where CorreoVendedor = @correo";
                    ConexionBD dbConexion2 = new ConexionBD();

                    SqlCommand cmd2 = new SqlCommand(consulta, dbConexion2.conexion());
                    cmd2.Parameters.AddWithValue("@correo", txtUsuarioLogin.Text);
                    SqlDataReader reader = cmd2.ExecuteReader();

                    DatosPersonales datosPersonales = new DatosPersonales();

                    //Cargar informacion del Usuario a la clase Datos Personales
                    if (reader.Read())
                    {

                        datosPersonales.TipoCuenta = 2;
                        datosPersonales.Nombre = Convert.ToString(reader[1]);
                        datosPersonales.Apellido = Convert.ToString(reader[2]);
                        datosPersonales.Correo = Convert.ToString(reader[3]);
                        datosPersonales.Contraseña = Convert.ToString(reader[4]);
                        datosPersonales.Telefono = Convert.ToString(reader[5]);
                        datosPersonales.Direccion = Convert.ToString(reader[6]);
                        datosPersonales.Id = Convert.ToString(reader[0]);

                        MessageBox.Show(Convert.ToString(reader[1]));
                    }


                    //Mostramos el formulario del Marketplace del vendedor.
                    var window = new MarketplaceVendedor();
                    this.Close();
                    window.Show();
                }
                //Contrasña/Correo Incorrecto
                else
                {
                    MessageBox.Show("Correo/Contraseña incorrectos.Intentelo de nuevo");
                }

            }
            catch
            {
                MessageBox.Show("Error A-001. Fallo de Conexión.");
            }

        }

        /// <summary>
        /// Cancelar el login
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelarLogin_Click(object sender, RoutedEventArgs e)
        {

            //Cerramos la aplicacion
            Application.Current.Shutdown();

            /*var carga = new LoadingPage();
            this.Close();
            carga.Show();*/
        }

        /// <summary>
        /// Despliega el formulario de registro
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCrearCuentaLogin_Click(object sender, RoutedEventArgs e)
        {
            //Mostramos el formulario de registro.
            var window = new registro();
            this.Close();
            window.Show();
        }


        /// <summary>
        /// Muestra ayuda para recuperar la contraseña
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAyudaLogin_Click(object sender, RoutedEventArgs e)
        {

            MessageBox.Show("Contactenos al correo IntermediarioSoporte@Intermediario.com");

            /*var window = new RecuperarContraseña();
            this.Close();
            window.Show();*/
            /*string id = "0";

            var window = new AgregarProductosVendedor(id);
            this.Close();
            window.Show();*/

            /*var factura = new ClassFactura();
            factura.CrearFactura();*/
        }

        /// <summary>
        /// Cada que el usuario ingresa un espacio este metodo borra ese espacio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtUsuarioLogin_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {

                txtUsuarioLogin.Text = txtUsuarioLogin.Text.Replace(" ", "");

                txtUsuarioLogin.Select(txtUsuarioLogin.Text.Length, 0);
            }

        }

        private void txtPasswordLogin_KeyUp(object sender, KeyEventArgs e)
        {

        }
        /// <summary>
        /// Este metodo verifica que el usuario ingrese uno de los caracteres validos
        /// (Alfabeto,arroba,guion bajo,punto y numeros)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPasswordLogin_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^A-Z | a-z | @ | _ | . | 0-9 | !\"#\\$%&'()+,-./:;=?@*\\[\\]^_{|}~]+").IsMatch(e.Text);
        }
        /// <summary>
        /// Este metodo verifica que el usuario ingrese uno de los caracteres validos
        /// (Alfabeto,arroba,guion bajo,punto y numeros)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtUsuarioLogin_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^A-Z | a-z | @ | _ | . | 0-9 | !\"#\\$%&'()+,-./:;=?@*\\[\\]^_{|}~]+").IsMatch(e.Text);
        }

        /// <summary>
        /// Cuando el usuario da enter este metodo ejecuta el evento click
        /// del boton de inicio de sesion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPasswordLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                btnIngresarLogin_Click(sender,e);

                
            }
        }

        /// <summary>
        /// Cuando el usuario da enter este metodo ejecuta el evento click
        /// del boton de inicio de sesion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtUsuarioLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnIngresarLogin_Click(sender, e);
            }
        }

        /// <summary>
        /// Cuando el usuario pone el cursor sobre el boton ejecuta este metodo
        /// que muestra la contraseña ingresada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMostrarPass_MouseEnter(object sender, MouseEventArgs e)
        {
            txtMostrarPass.Visibility = Visibility.Visible;
            txtPasswordLogin.Visibility = Visibility.Hidden;
            txtMostrarPass.Text = txtPasswordLogin.Password;
        }


        /// <summary>
        /// Cuando el usuario quita el cursor del boton deja de mostrar la contraseña ingresada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMostrarPass_MouseLeave(object sender, MouseEventArgs e)
        {
            txtMostrarPass.Visibility = Visibility.Hidden;
            txtPasswordLogin.Visibility = Visibility.Visible;
            txtMostrarPass.Text = String.Empty;
            txtPasswordLogin.Focus();
        }

        /// <summary>
        /// Cuando el usuario quita el cursor del boton deja de mostrar la contraseña ingresada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            txtMostrarPass.Visibility = Visibility.Visible;
            txtPasswordLogin.Visibility = Visibility.Hidden;
            txtMostrarPass.Text = txtPasswordLogin.Password;
        }
    }
}
