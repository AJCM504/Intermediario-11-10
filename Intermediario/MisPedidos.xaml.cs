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
    /// Lógica de interacción para MisPedidos.xaml
    /// </summary>
    public partial class MisPedidos : Window
    {

        DatosPersonales datosPersonales = new DatosPersonales();
        Imagenes imagenes = new Imagenes();
        string idProducto;

        /// <summary>
        /// Metodo Constructor del formulario MisPedidos.
        /// Obtiene de la base de datos de la tabla notificaciones los registros que aun ni han sido entregados
        /// y los muestra con controles dinamicos.
        /// </summary>
        public MisPedidos()
        {
            InitializeComponent();

            


            ConexionBD bdConexion = new ConexionBD();

            string consulta = "EXECUTE CargarNotificaciones @IDVENDEDOR";

            SqlCommand cmd = new SqlCommand(consulta, bdConexion.conexion());
            cmd.Parameters.AddWithValue("@IDVENDEDOR", datosPersonales.Id);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Grid grid = new Grid();
                Label lblNombre = new Label();
                Label lblDescripcio = new Label();
                Button btnEnviado = new Button();
                Image imgProducto = new Image();

                var bc = new BrushConverter();

                grid.Height = 130;
                grid.Margin = new Thickness(30, 35, 30, 0);
                grid.Background = (Brush)bc.ConvertFrom("#42526f");

                stkProductoVendidos.Children.Add(grid);

                lblNombre.Content = "Nombre:" + Convert.ToString(reader[1]);
                lblNombre.HorizontalAlignment = HorizontalAlignment.Left;
                lblNombre.Margin = new Thickness(21, 10, 0, 0);
                lblNombre.VerticalAlignment = VerticalAlignment.Top;
                lblNombre.Width = 200;
                lblNombre.Foreground = Brushes.White;

                lblDescripcio.Content = Convert.ToString(reader[2]);
                lblDescripcio.HorizontalAlignment = HorizontalAlignment.Left;
                lblDescripcio.Margin = new Thickness(21, 25, 0, 0);
                lblDescripcio.VerticalAlignment = VerticalAlignment.Top;
                lblDescripcio.Width = 500;
                lblDescripcio.Foreground = Brushes.White;

                btnEnviado.Content = "Enviado";
                btnEnviado.Name = "btn" + Convert.ToString(reader[0]);
                btnEnviado.HorizontalAlignment = HorizontalAlignment.Center;
                btnEnviado.VerticalAlignment = VerticalAlignment.Top;
                btnEnviado.Width = 113;
                btnEnviado.Margin = new Thickness(0, 88, 0, 0);
                btnEnviado.Click += new RoutedEventHandler(Boton_Enviado);

                imgProducto.Source = imagenes.imageFromByte((byte[])reader[3]);
                imgProducto.Height = 100;
                imgProducto.Width = 100;
                imgProducto.HorizontalAlignment = HorizontalAlignment.Right;
                imgProducto.Stretch = Stretch.Fill;
                imgProducto.Margin = new Thickness(0, 0, 50, 0);

                grid.Children.Add(lblNombre);
                grid.Children.Add(lblDescripcio);
                grid.Children.Add(btnEnviado);
                grid.Children.Add(imgProducto);

                stkProductoVendidos.Height += 135;

            }
        }

        /// <summary>
        /// Evento click del boton Boton_Enviado.
        /// Obtiene el id de la notificacion que esta guardado en el nombre del boton.
        /// Luego actualiza esa notificacion en la base de datos segun el id y lo pone como actualizado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Boton_Enviado(object sender, RoutedEventArgs e)
        {

            string idNotificacion = "0";

            var fuente = e.OriginalSource as FrameworkElement;
            if (fuente == null)
                return;
            //MessageBox.Show(fuente.Name);

            idNotificacion = fuente.Name;

            for (int i = 3; i < idNotificacion.Length; i++)
            {
                idProducto += idNotificacion[i];
            }


            ConexionBD bdConexion = new ConexionBD();
            string consulta = "UPDATE Notificacion SET EstadoNotificacion=2 where IdNotficacion=@IdNotificacion";
        
            SqlCommand cmd = new SqlCommand(consulta, bdConexion.conexion());
            cmd.Parameters.AddWithValue("@IdNotificacion", idProducto);
            cmd.ExecuteScalar();

            var window = new MisPedidos();
            this.Close();
            window.Show();
            
        }

        /// <summary>
        /// Evento click del boton btnAceptar.
        /// Cierra el formulario MisPedidos y muestra el formulario MarketplaceVendedor.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            var window = new MarketplaceVendedor();
            this.Close();
            window.Show();
        }
    }
}
