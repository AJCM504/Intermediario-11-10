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

using Intermediario;


namespace Intermediario
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //Boton para registrarse
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var window = new registro();
            window.Show();
            this.Close();
        }

        //Boton de Ingresar
        private void btnIngresarLogin_Click(object sender, RoutedEventArgs e)
        {
            String validacion;
            String correo = txtUsuarioLogin.Text;
            String contraseña = txtPasswordLogin.Password;

            //Ejecucion de la validación.
            //try
            //{

                ConexionBD dbConexion = new ConexionBD();
                string consulta = "execute LoginValidation @correo,@password";
                SqlCommand cmd = new SqlCommand(consulta, dbConexion.conexion());
                cmd.Parameters.AddWithValue("@password", contraseña);
                cmd.Parameters.AddWithValue("@correo", correo);


                var codigo = Convert.ToString(cmd.ExecuteScalar());

                validacion = codigo;
                //Comprador
                if (validacion == "1")
                {

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

                        MessageBox.Show(Convert.ToString(reader[1]));
                    }

                    var window = new MarketplaceComprador();
                    this.Close();
                    window.Show();

                }
                //Vendedor
                else if(validacion == "2")
                {
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

                    var window = new MarketplaceVendedor();
                    this.Close();
                    window.Show();
                }
                //Contrasña/Correo Incorrecto
                else
                {
                    MessageBox.Show("Correo/Contraseña incorrectos.Intentelo de nuevo");
                }
            /*}
            catch
            {
                MessageBox.Show("Error A-001. Fallo de Conexión.");
            }*/

           
        }

        //Boton para salir
        private void btnCancelarLogin_Click(object sender, RoutedEventArgs e)
        {
            //Application.Current.Shutdown();

            var carga = new LoadingPage();
            this.Close();
            carga.Show();
        }

        //Boton para registrarse
        private void btnCrearCuentaLogin_Click(object sender, RoutedEventArgs e)
        {
            var window = new registro();
            this.Close();
            window.Show();
        }


        //Boton de recuperar contraseña
        private void BtnAyudaLogin_Click(object sender, RoutedEventArgs e)
        {
            /*var window = new RecuperarContraseña();
            this.Close();
            window.Show();*/
            string id = "0";

            var window = new AgregarProductosVendedor(id);
            this.Close();
            window.Show();
        }
    }
}
