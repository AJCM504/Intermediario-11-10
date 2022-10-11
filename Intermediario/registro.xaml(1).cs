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

namespace Intermediario
{
    /// <summary>
    /// Lógica de interacción para registro.xaml
    /// </summary>
    public partial class registro : Window
    {

        DatosPersonales datosPersonales = new DatosPersonales();

        public registro()
        {
            InitializeComponent();
        }

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

            //try
            //{

                RegistroCampos registroCampos = new RegistroCampos(nombre, apellido, contraseña, cuenta, correo, telefono, direccion);
                if (registroCampos.CamposLlenos() != 7)
                {
                    MessageBox.Show("Llene todos los campos.");
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
                        MessageBox.Show("Registor exitoso.");
                        var window = new MarketplaceComprador();
                        this.Close();
                        window.Show();
                    }
                    else if (Convert.ToString(reader[0]) == "3")
                    {
                        Guardar_Datos_Personales();
                        datosPersonales.Id = Convert.ToString(reader[1]);
                        datosPersonales.TipoCuenta = 2;
                        MessageBox.Show("Registor exitoso.");
                        var window = new MarketplaceVendedor();
                        this.Close();
                        window.Show();
                    }
                }
                }
            /*}
            catch
            {
                MessageBox.Show("Error A-001. Fallo de Conexión.");
            }*/
        }

        private void btnCancelarRegistro_Click(object sender, RoutedEventArgs e)
        {
            var window = new MainWindow();
            this.Close();
            window.Show();  
        }

        private void Guardar_Datos_Personales()
        {
            datosPersonales.Nombre = txtNombreRegistro.Text;
            datosPersonales.Apellido = txtApellidoRegistro.Text;
            datosPersonales.Correo = txtCorreoRegistro.Text;
            datosPersonales.Contraseña = txtPasswordRegistro.Password;
            datosPersonales.Telefono = txtTelefonoRegistro.Text;
            datosPersonales.Direccion = txtDireccionRegistro.Text;
        }
    }
}
