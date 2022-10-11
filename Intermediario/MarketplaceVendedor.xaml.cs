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
    /// Lógica de interacción para MarketplaceVendedor.xaml
    /// </summary>
    public partial class MarketplaceVendedor : Window
    {
        DatosPersonales datosPersonales = new DatosPersonales();

        public MarketplaceVendedor()
        {
            InitializeComponent();

            var classImagenes = new Imagenes();

            Button botonAgregar = new Button();
            botonAgregar.Width = 371;
            botonAgregar.Height = 227;
            botonAgregar.Margin = new Thickness(25, 25, 0, 0);
            botonAgregar.Click += new RoutedEventHandler(Agregar_Producto);
            botonAgregar.Name="btn0";

            Grid gridAgregar = new Grid();

            gridAgregar.Height = 227;
            gridAgregar.Width = 371;

            Label lblAgregar = new Label();
            lblAgregar.Content = "Agregar Producto";
            lblAgregar.VerticalAlignment = VerticalAlignment.Bottom;
            lblAgregar.Margin = new Thickness(0, 20, 0, 30);
            lblAgregar.HorizontalAlignment = HorizontalAlignment.Left;
            lblAgregar.FontSize = 20;

            Image imagenAgregarProducto = new Image();
            BitmapImage imagenBit = new BitmapImage();
            imagenBit.BeginInit();
            imagenBit.UriSource = new Uri("rsAgregarProducto.png",UriKind.Relative);

            gridAgregar.Children.Add(lblAgregar);
            gridAgregar.Children.Add(imagenAgregarProducto);

            botonAgregar.Content = gridAgregar;

            wrpProductos.Children.Add(botonAgregar);



            ConexionBD bdconexion = new ConexionBD();
            string consulta = "Select * from [Datos Producto] join [Foto Producto] on [Datos Producto].IDProducto =[Foto Producto].IDProducto where [Datos Producto].IDEstadoProducto=1 and IDCuentaVendedor = @IDVendedor";
            SqlCommand cmd = new SqlCommand(consulta,bdconexion.conexion());
            cmd.Parameters.AddWithValue("@id", 1);
            cmd.Parameters.AddWithValue("@IDVendedor", Convert.ToString(datosPersonales.Id));

            SqlDataReader reader = cmd.ExecuteReader();

            int contador = 0;
            int control = -0;
              
            
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


                    boton.Name ="btn"+ Convert.ToString(reader["IDProducto"]);
                    boton.Height = 227;
                    boton.Width = 371;
                    boton.Margin = new Thickness(25, 25, 0, 0);
                    boton.Content = grid;
                    boton.Click += new RoutedEventHandler(Boton_Click);

                    grid.Height = 227;
                    grid.Width = 371;

                    imagen.Height = 150;
                    imagen.Margin= new Thickness(0, -30, 0, 0);
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

        private void btnCerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            var window = new MainWindow();
            this.Close();
            window.Show();
        }

        private void btnPerfil_Click(object sender, RoutedEventArgs e)
        {
            var window = new Perfil();
            this.Close();
            window.Show();
        }

        private void TBMostrar(object sender, RoutedEventArgs e)
        {
        }

        private void TBOcultar(object sender, RoutedEventArgs e)
        {

        }

        private void btnInicio_Click(object sender, RoutedEventArgs e)
        {
            var window = new MainWindow();
            this.Close();
            window.Show();
        }

        private void btnPerfil_Click_1(object sender, RoutedEventArgs e)
        {
            var window = new Perfil();
            this.Close();
            window.Show();
        }

        private void btnCarrito_Click(object sender, RoutedEventArgs e)
        {
            var window = new MisPedidos();
            this.Close();
            window.Show();
        }

        private void btnCerrarSesion_Click_1(object sender, RoutedEventArgs e)
        {
            var window = new MainWindow();
            this.Close();
            window.Show();
        }

        private void Agregar_Producto(object sender, RoutedEventArgs e)
        {
            string id= "0";
            var window = new AgregarProductosVendedor(id);
            this.Close();
            window.Show();
        }

    }
}
