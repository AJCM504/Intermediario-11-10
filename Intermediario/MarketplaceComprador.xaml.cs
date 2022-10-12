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
    /// Lógica de interacción para MarketplaceComprador.xaml
    /// </summary>
    public partial class MarketplaceComprador : Window
    {
        /// <summary>
        /// Metodo constructor del Formulario MarketplaceComprador.
        /// Este obtiene los datos de los productos de la base de datos y los muestra
        /// dentro de botones dinamicos.
        /// </summary>
        public MarketplaceComprador()
        {
            InitializeComponent();

            DatosPersonales datosPersonales = new DatosPersonales();
            var classImagenes = new Imagenes();

            ConexionBD bdconexion = new ConexionBD();
            string consulta = "execute MostrarProductosCompradores";
            SqlCommand cmd = new SqlCommand(consulta, bdconexion.conexion());
            cmd.Parameters.AddWithValue("@id", 1);

            SqlDataReader reader = cmd.ExecuteReader();

            int contador = 0;
            int control = -0;

            //Muestra de productos
            while (reader.Read())
            {
                Button boton = new Button();
                Grid grid = new Grid();
                Label labelnombre = new Label();
                Label labelPrecio = new Label();
                Label labelID = new Label();
                Image imagen = new Image();
                Label lblIDProducto = new Label();

                if (control != Convert.ToInt32(reader["IDProducto"]))
                {


                    boton.Name = "btn" + Convert.ToString(reader["IDProducto"]);
                    boton.Height = 227;
                    boton.Width = 371;
                    boton.Margin = new Thickness(25, 25, 0, 0);
                    boton.Content = grid;
                    boton.Click += new RoutedEventHandler(Boton_Click);

                    grid.Height = 227;
                    grid.Width = 371;
                    

                    imagen.Height = 150;
                    imagen.Margin = new Thickness(0, -30, 0, 0);
                    imagen.VerticalAlignment = VerticalAlignment.Center;
                    imagen.Source = classImagenes.imageFromByte((byte[])reader["FotoProducto"]);

                    labelnombre.Name = "lblNombre" + Convert.ToInt32(contador);
                    labelnombre.Content = "Nombre: " + reader["NombreProducto"];
                    labelnombre.VerticalAlignment = VerticalAlignment.Bottom;
                    labelnombre.Margin = new Thickness(0, 20, 0, 30);
                    labelnombre.HorizontalAlignment = HorizontalAlignment.Left;

                    labelPrecio.Name = "lblPrecio" + Convert.ToString(contador);
                    labelPrecio.Content = "Precio: " + Convert.ToString(reader["PrecioProducto"]);
                    labelPrecio.VerticalAlignment = VerticalAlignment.Bottom;
                    labelPrecio.Margin = new Thickness(0, 20, 0, 15);

                    lblIDProducto.Name = "lblIDProducto" + Convert.ToString(contador);
                    lblIDProducto.Content = reader["IDProducto"];
                    lblIDProducto.HorizontalAlignment = HorizontalAlignment.Left;

                    grid.Children.Add(labelnombre);
                    grid.Children.Add(labelPrecio);
                    grid.Children.Add(imagen);
                    grid.Children.Add(lblIDProducto);

                    wrpProductos.Children.Add(boton);

                    control = Convert.ToInt32(reader["IDProducto"]);
                }

                List<Button> botones =  wrpProductos.Children.OfType<Button>().ToList();

                foreach (Button btn in botones)
                {
                    wrpProductos.Children.Remove(btn);
                }

                contador++;
            }
        }

        /// <summary>
        /// Evento click del boton btnPerfil.
        /// Cierra el formulario de MarketplaceComprador y muestra el formulario de Perfil.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPerfil_Click(object sender, RoutedEventArgs e)
        {
            var window = new Perfil();
            this.Close();
            window.Show();
        }

        /// <summary>
        /// Evento click del boton btnCerrarSesion.
        /// Cierra el formulario MarketplaceComprador y despliega el formulario MainWindow.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            var window = new MainWindow();
            this.Close();
            window.Show();


        }

        private void TBMostrar(object sender, RoutedEventArgs e)
        {

        }

        private void TBOcultar(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Evento Click del Boton btnInicio.
        /// Cierra el formulario MarketplaceComprador y despliega el formulario MainWindow.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInicio_Click(object sender, RoutedEventArgs e)
        {
            var window = new MainWindow();
            this.Close();
            window.Show();
        }

        /// <summary>
        /// Evento Click del Boton btnPerfil.
        /// Cierra el formulario MarketplaceComprador y despliega el formulario Perfil.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPerfil_Click_1(object sender, RoutedEventArgs e)
        {
            var window = new Perfil();
            this.Close();
            window.Show();
        }

        /// <summary>
        /// Evento Click del Boton btnCarrito.
        /// Cierra el formulario MarketplaceComprador y despliega el formulario CarritoCompras.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCarrito_Click(object sender, RoutedEventArgs e)
        {
            var window = new CarritodeCompras();
            this.Close();
            window.Show();
        }

        /// <summary>
        /// Evento Click del Boton btnCerrarSesion.
        /// Cierra el formulario MarketplaceComprador y despliega el formulario MainWindow.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCerrarSesion_Click_1(object sender, RoutedEventArgs e)
        {
            var window = new MainWindow();
            this.Close();
            window.Show();
        }

        /// <summary>
        /// Evento click de los botones dinamicos creados por el constructor.
        /// Obtiene el ID del producto que esta guardado en el nombre del boton.
        /// Posteriormente cierra el formulario MarketplaceComprador y muestra
        /// el formulario AgregarProductosComprador pasandole el id del producto como parametro.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Boton_Click(object sender, RoutedEventArgs e)
        {

            string id = "0";

            var fuente = e.OriginalSource as FrameworkElement;
            if (fuente == null)
                return;
            //MessageBox.Show(fuente.Name);

            id = fuente.Name;

            var windows = new AgregarProductosVendedor(id);
            this.Close();
            windows.Show();
            this.InitializeComponent();
        }

        /// <summary>
        /// Evento Click del Boton Button.
        /// Ejecuta la busqueda de producto parecidos al texto del TextBox txtBuscador
        /// en la base de datos
        /// segun el nombre del producto y los muestra en el WrapPanel wrpProductos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            wrpProductos.Children.Clear();
            var classImagenes = new Imagenes();

            ConexionBD bdconexion = new ConexionBD();
            string consulta = "SELECT * FROM [DATOS PRODUCTO] JOIN [FOTO PRODUCTO] ON [Datos Producto].IDProducto=[Foto Producto].IDProducto WHERE NombreProducto  like @Nombre and IDEstadoProducto = 1";
            SqlCommand cmd = new SqlCommand(consulta, bdconexion.conexion());
            cmd.Parameters.AddWithValue("@Nombre",txtBuscador.Text+"%");

            SqlDataReader reader = cmd.ExecuteReader();

            int contador = 0;
            int control = -0;

            //Muestra de productos
            while (reader.Read())
            {
                Button boton = new Button();
                Grid grid = new Grid();
                Label labelnombre = new Label();
                Label labelPrecio = new Label();
                Label labelID = new Label();
                Image imagen = new Image();
                Label lblIDProducto = new Label();

                if (control != Convert.ToInt32(reader["IDProducto"]))
                {


                    boton.Name = "btn" + Convert.ToString(reader["IDProducto"]);
                    boton.Height = 227;
                    boton.Width = 371;
                    boton.Margin = new Thickness(25, 25, 0, 0);
                    boton.Content = grid;
                    boton.Click += new RoutedEventHandler(Boton_Click);

                    grid.Height = 227;
                    grid.Width = 371;

                    imagen.Height = 150;
                    imagen.Margin = new Thickness(0, -30, 0, 0);
                    imagen.VerticalAlignment = VerticalAlignment.Center;
                    imagen.Source = classImagenes.imageFromByte((byte[])reader["FotoProducto"]);

                    labelnombre.Name = "lblNombre" + Convert.ToInt32(contador);
                    labelnombre.Content = "Nombre: " + reader["NombreProducto"];
                    labelnombre.VerticalAlignment = VerticalAlignment.Bottom;
                    labelnombre.Margin = new Thickness(0, 20, 0, 30);
                    labelnombre.HorizontalAlignment = HorizontalAlignment.Left;

                    labelPrecio.Name = "lblPrecio" + Convert.ToString(contador);
                    labelPrecio.Content = "Precio: " + Convert.ToString(reader["PrecioProducto"]);
                    labelPrecio.VerticalAlignment = VerticalAlignment.Bottom;
                    labelPrecio.Margin = new Thickness(0, 20, 0, 15);

                    lblIDProducto.Name = "lblIDProducto" + Convert.ToString(contador);
                    lblIDProducto.Content = reader["IDProducto"];
                    lblIDProducto.HorizontalAlignment = HorizontalAlignment.Left;

                    grid.Children.Add(labelnombre);
                    grid.Children.Add(labelPrecio);
                    grid.Children.Add(imagen);
                    grid.Children.Add(lblIDProducto);

                    wrpProductos.Children.Add(boton);

                    control = Convert.ToInt32(reader["IDProducto"]);
                }

                contador++;
            }

        }
    }
}
