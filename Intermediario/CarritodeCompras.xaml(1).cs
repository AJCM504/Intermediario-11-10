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
    /// Interaction logic for CarritodeCompras.xaml
    /// </summary>
    public partial class CarritodeCompras : Window
    {
        ConexionBD bdConexion = new ConexionBD();
        DatosPersonales datosPersonales = new DatosPersonales();
        Imagenes classImagenes = new Imagenes();
        string consulta;
        List<int> idUsados = new List<int>();
        double total,impuestos,envio;


        int control = 0;

        public CarritodeCompras()
        {
            InitializeComponent();

            consulta = "EXECUTE CARGARCARRITO @IDCOMPRADOR";

            SqlCommand cmd = new SqlCommand(consulta, bdConexion.conexion());
            cmd.Parameters.AddWithValue("@IDCOMPRADOR", datosPersonales.Id);

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Button boton = new Button();
                Button eliminar = new Button();
                Label lblPrecio = new Label();
                Label lblNombre = new Label();
                Label lblMarca = new Label();
                Label lblModelo = new Label();
                Image imgProducto = new Image();
                Image imgEliminar = new Image();
                Grid grid = new Grid();
                Grid gdEliminar = new Grid();



                if (idUsados.Contains(Convert.ToInt32(reader[8])) == false)
                {

                    idUsados.Add(Convert.ToInt32(reader[8]));

                    stckProductos.Height += 120;

                    boton.Name = "btn" + Convert.ToString(reader[8]);
                    boton.Height = 120;
                    boton.Margin = new Thickness(0, 0, 0, 10);
                    boton.Content = grid;
                    boton.Click += new RoutedEventHandler(Boton_Click);

                    grid.Height = 120;
                    grid.Width = 466;

                    imgProducto.Height = 80;
                    imgProducto.Width = 80;
                    imgProducto.Margin = new Thickness(10, 0, 0, 0);
                    imgProducto.HorizontalAlignment = HorizontalAlignment.Left;
                    imgProducto.Source = classImagenes.imageFromByte((byte[])reader["FotoProducto"]);

                    lblNombre.Content = Convert.ToString(reader[11]);
                    lblNombre.Height = 25;
                    lblNombre.Width = 150;
                    lblNombre.VerticalAlignment = VerticalAlignment.Top;
                    lblNombre.Margin = new Thickness(106, 12, 27, 0);
                    lblNombre.Foreground = Brushes.White;

                    lblPrecio.Content = "Precio: " + Convert.ToString(reader["PrecioProducto"]);
                    lblPrecio.Height = 25;
                    lblPrecio.Width = 150;
                    lblPrecio.HorizontalAlignment = HorizontalAlignment.Left;
                    lblPrecio.Margin = new Thickness(106, 22, 0, 33);
                    lblPrecio.Foreground = Brushes.White;

                    lblMarca.Content = "Marca: " + Convert.ToString(reader["Marca"]);
                    lblMarca.Height = 25;
                    lblMarca.Width = 150;
                    lblMarca.HorizontalAlignment = HorizontalAlignment.Left;
                    lblMarca.Margin = new Thickness(106, 47, 0, 8);
                    lblMarca.Foreground = Brushes.White;

                    lblModelo.Content = "Modelo: " + Convert.ToString(reader["ModeloProducto"]);
                    lblModelo.Height = 25;
                    lblModelo.Width = 150;
                    lblModelo.HorizontalAlignment = HorizontalAlignment.Left;
                    lblModelo.Margin = new Thickness(106, 72, 0, -17);
                    lblModelo.Foreground = Brushes.White;

                    eliminar.Width = 20;
                    eliminar.Height = 20;
                    eliminar.HorizontalAlignment = HorizontalAlignment.Right;
                    eliminar.VerticalAlignment = VerticalAlignment.Top;
                    eliminar.Margin = new Thickness(0, 5, 35, 0);
                    eliminar.Content = gdEliminar;
                    eliminar.Click += new RoutedEventHandler(EliminarProducto);
                    eliminar.Name = "b" + Convert.ToString(reader[8]);

                    BitmapImage imagenBit = new BitmapImage();
                    imagenBit.BeginInit();
                    imagenBit.UriSource = new Uri(@"C:\Users\erick\Desktop\Universidad\Desarrollo de Software\Segundo Parcial\Intermediario-master_4-7-22\Intermediario\Imagenes\basurero.png", UriKind.Relative);
                    imagenBit.EndInit();

                    gdEliminar.Height = 30;
                    gdEliminar.Width = 30;

                    gdEliminar.Children.Add(imgEliminar);

                    imgEliminar.Stretch = Stretch.Fill;
                    imgEliminar.Source = imagenBit;

                    stckProductos.Children.Add(boton);
                    grid.Children.Add(imgProducto);
                    grid.Children.Add(lblNombre);
                    grid.Children.Add(lblPrecio);
                    grid.Children.Add(lblMarca);
                    grid.Children.Add(lblModelo);
                    grid.Children.Add(eliminar);

                    total += Convert.ToInt32(reader["PrecioProducto"]);
                    impuestos = Convert.ToInt32(reader[1]);
                    envio += Convert.ToInt32(reader[2]);

                    MessageBox.Show(Convert.ToString(total) + "," + Convert.ToString(impuestos));
                }
            }

            lblDetalleTotalCarrito.Content = Convert.ToString((impuestos * 0.01) * total);

            lblImpuestos.Content = Convert.ToString(total);

            lblCargosCarrito.Content = Convert.ToString(envio);

            lblTotalCarrito.Content = Convert.ToString(((impuestos * 0.01) * total) + total + envio);

        }

        private void Boton_Click(object sender, RoutedEventArgs e)
        {

            if (control == 0)
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
            else { control = 0; }
        }

        private void EliminarProducto(object sender, RoutedEventArgs e)
        {
            control = 1;

            string idProducto = "0";

            var fuente = e.OriginalSource as FrameworkElement;
            if (fuente == null)
                return;
            //MessageBox.Show(fuente.Name);

            string id = fuente.Name;

            for (int i = 1; i < id.Length; i++)
            {
                idProducto += id[i];
            }

            bdConexion.CerrarConexion();

            consulta = "EXECUTE EliminarProductoCarrito @IDCOMPRADOR, @IDPRODUCTO";

            SqlCommand cmd = new SqlCommand(consulta, bdConexion.conexion());
            cmd.Parameters.AddWithValue("@IDCOMPRADOR", datosPersonales.Id);
            cmd.Parameters.AddWithValue("@IDPRODUCTO", idProducto);
            cmd.ExecuteScalar();

            MessageBox.Show("Se elimino el producto");

            var window = new CarritodeCompras();
            this.Close();
            window.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var window = new MarketplaceComprador();
            this.Close();
            window.Show();
        }
    }
}
