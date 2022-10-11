using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
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
    /// Lógica de interacción para AgregarProductosVendedor.xaml
    /// </summary>
    public partial class AgregarProductosVendedor : Window
    {

        Imagenes imag = new Imagenes();

        List<byte[]> imagen1 = new List<byte[]>();
        BitmapImage imagenBitMap = null;
        int contador, contadorAux, imageCount;
        int imagenesCapt;
        string idProducto;

        DatosPersonales datosPersonales = new DatosPersonales();


        public AgregarProductosVendedor(string id)
        {



            InitializeComponent();

            for (int i = 3; i < id.Length; i++)
            {
                idProducto += id[i];
            }

            MessageBox.Show(idProducto);

            if (id != "0")
            {
                
                

                ConexionBD bdconexion = new ConexionBD();

                string consulta = "SELECT * FROM [Datos Producto] JOIN [Foto Producto] ON [Datos Producto].IDProducto=[Foto Producto].IDProducto where [Foto Producto].IDProducto=@ID";

                SqlCommand cmd = new SqlCommand(consulta, bdconexion.conexion());
                cmd.Parameters.AddWithValue("@ID", Convert.ToInt32(idProducto));
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {


                    txtNombreProducto.Text = Convert.ToString(reader[0]);
                    txtCategoria.Text = Convert.ToString(reader["IDCategoria"]);
                    txtPrecio.Text = Convert.ToString(reader["PrecioProducto"]);
                    txtMarca.Text = Convert.ToString(reader["Marca"]);
                    txtModelo.Text = Convert.ToString(reader["ModeloProducto"]);
                    txtCantidad.Text = Convert.ToString(reader["CantidadProducto"]);
                    txtDescripcionn.Text = Convert.ToString(reader["DescripcionProducto"]);

                    MessageBox.Show(Convert.ToString(imagen1.Count));

                    do
                    {
                        imagen1.Add((byte[])reader["FotoProducto"]);
                        imagenesCapt++;
                        MessageBox.Show(Convert.ToString(imagen1.Count));
                    } while (reader.Read());

                    contadorAux = imagenesCapt - 1;
                    contador = contadorAux;

                    imagenBitMap = imag.imageFromByte(imagen1[0]);
                    imgImagenesProductos.Source = imagenBitMap;
                }

                if (datosPersonales.TipoCuenta == 1)
                {
                    txtNombreProducto.IsEnabled = false;
                    txtCategoria.IsEnabled = false;
                    txtPrecio.IsEnabled = false;
                    txtMarca.IsEnabled = false;
                    txtModelo.IsEnabled = false;
                    txtCantidad.IsEnabled = false;
                    txtDescripcionn.IsEnabled = false;

                    btnGuardarProducto.Click += new RoutedEventHandler(Comprar);
                    btnGuardarProducto.Content = "Comprar";
                }
                else
                {
                    Button btnEliminar = new Button();

                    btnEliminar.Content = "Eliminar";
                    btnEliminar.HorizontalAlignment = HorizontalAlignment.Left;
                    btnEliminar.VerticalAlignment = VerticalAlignment.Bottom;
                    btnEliminar.Margin = new Thickness(220, 0, 0, 25);
                    btnEliminar.Height = 30;
                    btnEliminar.Width = 100;
                    btnEliminar.Click += new RoutedEventHandler(Eliminar);

                    btnGuardarProducto.Margin = new Thickness(65, 0, 0, 0);
                    btnGuardarProducto.Content = "Guardar";
                    btnGuardarProducto.Click += new RoutedEventHandler(Cambios);



                    grdBase.Children.Add(btnEliminar);
                }
                

            }
            else
            {
                btnGuardarProducto.Click += new RoutedEventHandler(btnGuardarProducto_Click);
            }

            if (imagen1.Count == 0)
            {
                btnFotoSiguiente.IsEnabled = false;
                btnFotoAnterior.IsEnabled = false;
            }

            

        }

        private void btnGuardarProducto_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                ConexionBD bDConexion = new ConexionBD();
                string consulta = "execute CargarDatosProducto @NOMBRE,@IDCATEGORIA,@DESCRIPCION,@PRECIO,@IDESTADO,@MARCA,@IDVENDEDOR,@MODELO,@CANTIDAD";
                SqlCommand cmd = new SqlCommand(consulta, bDConexion.conexion());
                cmd.Parameters.AddWithValue("@NOMBRE", txtNombreProducto.Text);
                cmd.Parameters.AddWithValue("@IDCATEGORIA", Convert.ToInt32(txtCategoria.Text));
                cmd.Parameters.AddWithValue("@DESCRIPCION", txtDescripcionn.Text);
                cmd.Parameters.AddWithValue("@PRECIO", Convert.ToInt32(txtPrecio.Text));
                cmd.Parameters.AddWithValue("@IDESTADO", 1);
                cmd.Parameters.AddWithValue("@MARCA", txtMarca.Text);
                cmd.Parameters.AddWithValue("@IDVENDEDOR", 1);
                cmd.Parameters.AddWithValue("@MODELO", txtModelo.Text);
                cmd.Parameters.AddWithValue("@CANTIDAD", Convert.ToInt32(txtCantidad.Text));
                cmd.ExecuteNonQuery();

                bDConexion.CerrarConexion();

                consulta = "SELECT TOP(1) IDProducto FROM [Datos Producto] order by IDProducto desc";
                SqlCommand cmd3 = new SqlCommand(consulta, bDConexion.conexion());
                int idProducto = Convert.ToInt32(cmd3.ExecuteScalar());

                bDConexion.CerrarConexion();

                consulta = "INSERT INTO [Foto Producto](IDProducto,FotoProducto) values(@idproducto,@foto)";

                MessageBox.Show(Convert.ToString(idProducto));

                for (int i = 0; i < imagen1.Count; i++)
                {
                    var cmd2 = new SqlCommand(consulta, bDConexion.conexion());
                    cmd2.Parameters.AddWithValue("@idproducto", idProducto);
                    cmd2.Parameters.AddWithValue("@foto", imagen1[i]);
                    cmd2.ExecuteNonQuery();

                    bDConexion.CerrarConexion();
                }
            }
            catch { }
            finally
            {
                var window = new MarketplaceVendedor();
                this.Close();
                window.Show();
            }
        } //Publicar

        private void btnFotoSiguiente_Click(object sender, RoutedEventArgs e)
        {

            if (imagen1.Count != 1)
            {

                imageCount = imagen1.Count;
                if (contador >= (imagen1.Count - 1))
                {
                    contador = 0;
                    imagenBitMap = imag.imageFromByte(imagen1[0]);
                    imgImagenesProductos.Source = imagenBitMap;
                }
                else
                {
                    contador++;
                    imagenBitMap = imag.imageFromByte(imagen1[contador]);
                    imgImagenesProductos.Source = imagenBitMap;
                }
            }
            else
            {
                btnFotoSiguiente.IsEnabled = false;
            }
        }

        private void btnFotoAnterior_Click(object sender, RoutedEventArgs e)
        {

            if (imagen1.Count != 1)
            {

                if (contador == 0)
                {
                    contador = imagen1.Count - 1;
                    MessageBox.Show(Convert.ToString(contador));
                    imagenBitMap = imag.imageFromByte(imagen1[contador]);
                    imgImagenesProductos.Source = imagenBitMap;

                }
                else
                {
                    contador--;
                    imagenBitMap = imag.imageFromByte(imagen1[contador]);
                    imgImagenesProductos.Source = imagenBitMap;
                }
            }
            else
            {
                btnFotoAnterior.IsEnabled = false;
            }
        }

        private void btnCancelarProducto_Click(object sender, RoutedEventArgs e)
        {
            if (datosPersonales.TipoCuenta == 1)
            {
                var windows = new MarketplaceComprador();
                this.Close();
                windows.Show();
            }
            else
            {
                var windows = new MarketplaceVendedor();
                this.Close();
                windows.Show();
            }
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)//Agregar Foto
        {
            OpenFileDialog openFielDialog = new OpenFileDialog();
            openFielDialog.Filter = "Arvhivos PNG (*.PNG)|*.PNG| Archivos JPG (*.JPG)|*.JPG";

            if (openFielDialog.ShowDialog() == true)
            {
                Uri fileuri = new Uri(openFielDialog.FileName);
                imgImagenesProductos.Source = new BitmapImage(fileuri);
                //byte[] imagen = ConvertirImg(openFielDialog.FileName);
                imagen1.Add(File.ReadAllBytes(openFielDialog.FileName));
                contadorAux++;
                contador = contadorAux;
            }

            if(imagen1.Count == 2)
            {
                btnFotoSiguiente.IsEnabled = true;
                btnFotoAnterior.IsEnabled = true;
            }
        }

        private void Cambios(object sender, RoutedEventArgs e)
        {
            ConexionBD bdConexion = new ConexionBD();

            string consulta = "EXECUTE ModificarProducto @ID, @NOMBRE, @IDCATEGORIA, @DESCRIPCIONPRODUCTO, @PRECIOPRODUCTO, @MARCA, @MODELOPRODUCTO";

            SqlCommand cmd = new SqlCommand(consulta, bdConexion.conexion());
            cmd.Parameters.AddWithValue("@ID", idProducto);
            cmd.Parameters.AddWithValue("@nombre", txtNombreProducto.Text);
            cmd.Parameters.AddWithValue("@IDCATEGORIA", Convert.ToInt32(txtCategoria.Text));
            cmd.Parameters.AddWithValue("@DESCRIPCIONPRODUCTO", txtDescripcionn.Text);
            cmd.Parameters.AddWithValue("@PRECIOPRODUCTO", Convert.ToInt32(txtPrecio.Text));
            cmd.Parameters.AddWithValue("@MARCA", txtMarca.Text);
            cmd.Parameters.AddWithValue("@MODELOPRODUCTO", txtModelo.Text);

            if (imagen1.Count < imagenesCapt)
            {
                for (int i = imagenesCapt + 1; i < imagen1.Count; i++)
                {

                    consulta = "INSERT INTO [Foto de Producto](IDProducto,FotoProducto) values(@ID,@FOTO)";
                    SqlCommand cmd2 = new SqlCommand(consulta, bdConexion.conexion());
                    cmd2.Parameters.AddWithValue("ID", idProducto);
                    cmd2.Parameters.AddWithValue("@FOTO", imagen1[i]);

                }

                if (imagen1.Count > 1)
                {
                    btnFotoAnterior.IsEnabled = true;
                    btnFotoAnterior.IsEnabled = true;
                }

            }

        }

        private void Eliminar(object sender, RoutedEventArgs e)
        {
            string consulta = "UPDATE [Datos Producto] SET IDEstadoProducto=2 WHERE IDProducto = @ID";

            ConexionBD bdConexion = new ConexionBD();

            SqlCommand cmd = new SqlCommand(consulta, bdConexion.conexion());
            cmd.Parameters.AddWithValue("@ID", idProducto);
            cmd.ExecuteScalar();

            MessageBox.Show("El producto a sido eliminado.");

            var window = new MarketplaceVendedor();
            this.Close();
            window.Show();
        }

        private void Comprar(object sender, RoutedEventArgs e)
        {
            ConexionBD bdConexion = new ConexionBD();

            MessageBox.Show("Se añadio el producto al carrito");
            string consulta = "EXECUTE AgregarCarrito @IDPRODUCTO,@IDCOMPRADOR,@CANTIDAD";

            SqlCommand cmd = new SqlCommand(consulta, bdConexion.conexion());
            cmd.Parameters.AddWithValue("@IDPRODUCTO", idProducto);
            cmd.Parameters.AddWithValue("@IDCOMPRADOR", datosPersonales.Id);
            cmd.Parameters.AddWithValue("@CANTIDAD", 1);
            cmd.ExecuteScalar();



        }
    }
}
