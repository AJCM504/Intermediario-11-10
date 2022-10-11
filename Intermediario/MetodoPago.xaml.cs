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
using MessageBox = System.Windows.Forms.MessageBox;

using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.IO;
using Intermediario.Facturas;

namespace Intermediario
{
    /// <summary>
    /// Interaction logic for MetodoPago.xaml
    /// </summary>
    public partial class MetodoPago : Window
    {
        DatosPersonales datosPersonales = new DatosPersonales();
        ConexionBD bdConexion = new ConexionBD();
        string consulta;
        List<string> nombreProducto = new List<string>();
        List<double> precioProducto = new List<double>();
        double total;
        string idfactura;

        /// <summary>
        /// Metodo Constructor del formulario MetodoPago
        /// </summary>
        /// <param name="total"></param>
        public MetodoPago(double total)
        {
            InitializeComponent();
            this.total = total;
        }


        /// <summary>
        /// Evento click del boton btnContinuarMetodoPago.
        /// Actualiza la base de datos en la tabla de factura en el registro
        /// donde la factura del cliente este pendiente y la actualiza a pagada
        /// y crear una nueva factura pendiente.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnContinuarMetodoPago_Click(object sender, RoutedEventArgs e)
        {


            consulta = "EXECUTE PagarProductos @IDCOMPRADOR";

            SqlCommand cmd = new SqlCommand(consulta, bdConexion.conexion());
            cmd.Parameters.AddWithValue("@IDCOMPRADOR", datosPersonales.Id);
            SqlDataReader reader = cmd.ExecuteReader();

            List<int> idProducto = new List<int>();

            //bdConexion.CerrarConexion();

            while (reader.Read())
            {
                idfactura = Convert.ToString(reader[0]);

                if (idProducto.Contains(Convert.ToInt32(reader[7])) == false)
                {

                    nombreProducto.Add(Convert.ToString(reader["NombreProducto"]));
                    precioProducto.Add(Convert.ToDouble(reader["PrecioProducto"]));

                    int cantidad = 0, estado = 0;

                    ConexionBD conexionBD = new ConexionBD();

                    consulta = "Insert into [Notificacion](IDProducto,IDCuentaVendedor,Descripcion,EstadoNotificacion) values(@IDPRODUCTO,@IDVENDEDOR,@DESCRIPCION,1)";

                    SqlCommand cmd2 = new SqlCommand(consulta, conexionBD.conexion());
                    cmd2.Parameters.AddWithValue("@IDPRODUCTO", Convert.ToInt32(reader[7]));
                    cmd2.Parameters.AddWithValue("@IDVENDEDOR", Convert.ToInt32(reader[17]));
                    cmd2.Parameters.AddWithValue("@DESCRIPCION", "Se vendio el producto.Por favor realice el envio.");
                    
                    cmd2.ExecuteScalar();

                    conexionBD.CerrarConexion();

                    consulta = "UPDATE [DATOS PRODUCTO] SET CantidadProducto = @CANTIDAD,IDEstadoProducto = @ESTADO where IDProducto = @IDPRODUCTO";

                    cantidad = Convert.ToInt32(reader[18]) - 1;

                    if(cantidad == 0) { estado = 3; }
                    else { estado = 1; }

                    MessageBox.Show("Cantidad: " + Convert.ToString(cantidad));
                    MessageBox.Show(Convert.ToString(reader[8]));

                    SqlCommand cmd3 = new SqlCommand(consulta, conexionBD.conexion());
                    cmd3.Parameters.AddWithValue("@CANTIDAD", cantidad);
                    cmd3.Parameters.AddWithValue("@ESTADO", estado);
                    cmd3.Parameters.AddWithValue("@IDPRODUCTO", Convert.ToInt32(reader[7]));
                    cmd3.ExecuteScalar();
                    

                    idProducto.Add(Convert.ToInt32(reader[7]));

                }

            }

            bdConexion.CerrarConexion();

            consulta = "EXECUTE ActualizarFactura @IDCOMPRADOR";

            cmd = new SqlCommand(consulta, bdConexion.conexion());
            cmd.Parameters.AddWithValue("@IDCOMPRADOR", datosPersonales.Id);
            cmd.ExecuteScalar();

            DialogResult res = (DialogResult)System.Windows.MessageBox.Show("¿Desea guardar un recibo?.", "Confirmación", MessageBoxButton.OKCancel);

            if(res == System.Windows.Forms.DialogResult.OK)
            {



                var factura = new ClassFactura(datosPersonales.Nombre, datosPersonales.Correo,total,nombreProducto,precioProducto,idfactura);
                factura.CrearFactura();

            }

            var window = new MarketplaceComprador();
            this.Close();
            window.Show();



        }

        /// <summary>
        /// Evento click del boton btnRegresar.
        /// Cierra el formulario MetodoPago y muestra el formulario CarritoCompras.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRegresar_Click(object sender, RoutedEventArgs e)
        {
            var window = new CarritodeCompras();
            this.Close();
            window.Show();
        }
    }
}
