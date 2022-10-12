using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        ConexionBD conexionBD = new ConexionBD();

        string nombreProducto, precio, marca, modelo, cantidad, descripcion;

        Imagenes imag = new Imagenes();

        List<byte[]> imagen1 = new List<byte[]>();
        List<int> idFotos = new List<int>();
        BitmapImage imagenBitMap = null;
        int contador, contadorAux, imageCount;
        int imagenesCapt;
        string idProducto;

        DatosPersonales datosPersonales = new DatosPersonales();

        /// <summary>
        /// Metodo constructor del formulario AgregarProductosComprador.
        /// Si el id recibido es cero entonces el formulario se usara para añadir productos
        /// a la base de datos. Si el id recibido es diferente a cero y positivo va a cargar la informacion
        /// de la base de datos segun el id y va a mostrar la informacion en los TtextBox respectivos
        /// </summary>
        /// <param name="id">ID del producto</param>
        public AgregarProductosVendedor(string id)
        {
            


            InitializeComponent();

            try
            {


                
                string consul = "SELECT NombreCategoria FROM Categoria ORDER BY IDCategoria ASC";

                SqlCommand cmd1 = new SqlCommand(consul, conexionBD.conexion());
                SqlDataReader reader1 = cmd1.ExecuteReader();

                while (reader1.Read())
                {
                    txtCategoria.Items.Add(Convert.ToString(reader1[0]));

                }

                for (int i = 3; i < id.Length; i++)
                {
                    idProducto += id[i];
                }

                

                if (id != "0")
                {



                    ConexionBD bdconexion = new ConexionBD();

                    string consulta = "SELECT * FROM [Datos Producto] JOIN [Foto Producto] ON [Datos Producto].IDProducto=[Foto Producto].IDProducto where [Foto Producto].IDProducto=@ID";

                    SqlCommand cmd = new SqlCommand(consulta, bdconexion.conexion());
                    cmd.Parameters.AddWithValue("@ID", Convert.ToInt32(idProducto));
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {


                        txtNombreProducto.Text = Convert.ToString(reader[1]);
                        txtCategoria.SelectedIndex = Convert.ToInt32(reader["IDCategoria"]) - 1;
                        //txtCategoria.Text = Convert.ToString(reader["IDCategoria"]);
                        txtPrecio.Text = Convert.ToString(reader["PrecioProducto"]);
                        txtMarca.Text = Convert.ToString(reader["Marca"]);
                        txtModelo.Text = Convert.ToString(reader["ModeloProducto"]);
                        txtCantidad.Text = Convert.ToString(reader["CantidadProducto"]);
                        txtDescripcionn.Text = Convert.ToString(reader["DescripcionProducto"]);

                        nombreProducto = txtNombreProducto.Text;
                        precio = txtPrecio.Text;
                        marca = txtMarca.Text;
                        modelo = txtModelo.Text;
                        cantidad = txtCantidad.Text;
                        descripcion = txtDescripcionn.Text;

                        //MessageBox.Show(Convert.ToString(imagen1.Count));

                        do
                        {
                            imagen1.Add((byte[])reader["FotoProducto"]);
                            idFotos.Add((int)reader["IDFoto"] * -1);
                            imagenesCapt++;
                            //MessageBox.Show(Convert.ToString(imagen1.Count));
                        } while (reader.Read());

                        contadorAux = imagenesCapt - 1;
                        contador = contadorAux;

                        imagenBitMap = imag.imageFromByte(imagen1[0]);
                        imgImagenesProductos.Source = imagenBitMap;




                    } //Carga de la informacion del producto

                    if (datosPersonales.TipoCuenta == 1)
                    {
                        txtNombreProducto.IsEnabled = false;
                        txtCategoria.IsEnabled = false;
                        txtPrecio.IsEnabled = false;
                        txtMarca.IsEnabled = false;
                        txtModelo.IsEnabled = false;
                        txtCantidad.IsEnabled = false;
                        txtDescripcionn.IsEnabled = false;

                        btnEliminar.Visibility = Visibility.Hidden;
                        btnEliminar.IsEnabled = false;

                        btnAgregarFoto.IsEnabled = false;
                        btnAgregarFoto.Opacity = 0;
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

                if (imagen1.Count == 0 || imagen1.Count == 1)
                {
                    btnFotoSiguiente.IsEnabled = false;
                    btnFotoAnterior.IsEnabled = false;
                }


            }
            catch
            {
                MessageBox.Show("No fue posible conectarse a la base de datos. Intentelo de nuevo mas tarde.");
            }

        }

        /// <summary>
        /// Evento click del boton btnGuardarProducto.
        /// Guarda la informacion ingresada por el usuario del producto en la base de datos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGuardarProducto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                

                RegistroCampos verCampos = new RegistroCampos(txtNombreProducto.Text,txtCategoria.Text, txtPrecio.Text, txtMarca.Text, txtModelo.Text, txtCantidad.Text,txtDescripcionn.Text, Convert.ToString(imagen1.Count));

                if (verCampos.CamposLlenos() != 8)
                {
                    MessageBox.Show("Llene todos los campos");
                }
                else
                {

                    ConexionBD bDConexion = new ConexionBD();
                    string consulta = "execute CargarDatosProducto @NOMBRE,@IDCATEGORIA,@DESCRIPCION,@PRECIO,@IDESTADO,@MARCA,@IDVENDEDOR,@MODELO,@CANTIDAD";
                    SqlCommand cmd = new SqlCommand(consulta, bDConexion.conexion());
                    cmd.Parameters.AddWithValue("@NOMBRE", txtNombreProducto.Text);
                    cmd.Parameters.AddWithValue("@IDCATEGORIA", txtCategoria.SelectedIndex + 1);
                    cmd.Parameters.AddWithValue("@DESCRIPCION", txtDescripcionn.Text);
                    cmd.Parameters.AddWithValue("@PRECIO", Convert.ToDouble(txtPrecio.Text));
                    cmd.Parameters.AddWithValue("@IDESTADO", 1);
                    cmd.Parameters.AddWithValue("@MARCA", txtMarca.Text);
                    cmd.Parameters.AddWithValue("@IDVENDEDOR", datosPersonales.Id);
                    cmd.Parameters.AddWithValue("@MODELO", txtModelo.Text);
                    cmd.Parameters.AddWithValue("@CANTIDAD", Convert.ToInt32(txtCantidad.Text));
                    cmd.ExecuteNonQuery();

                    bDConexion.CerrarConexion();

                    consulta = "SELECT TOP(1) IDProducto FROM [Datos Producto] order by IDProducto desc";
                    SqlCommand cmd3 = new SqlCommand(consulta, bDConexion.conexion());
                    int idProducto = Convert.ToInt32(cmd3.ExecuteScalar());

                    bDConexion.CerrarConexion();

                    consulta = "INSERT INTO [Foto Producto](IDProducto,FotoProducto) values(@idproducto,@foto)";

                    //MessageBox.Show(Convert.ToString(idProducto));

                    for (int i = 0; i < imagen1.Count; i++)
                    {
                        var cmd2 = new SqlCommand(consulta, bDConexion.conexion());
                        cmd2.Parameters.AddWithValue("@idproducto", idProducto);
                        cmd2.Parameters.AddWithValue("@foto", imagen1[i]);
                        cmd2.ExecuteNonQuery();

                        bDConexion.CerrarConexion();

                        var window = new MarketplaceVendedor();
                        this.Close();
                        window.Show();
                    }

                }
            }
            catch
            {
                MessageBox.Show("No fue posible conectarse a la base de datos. Intentelo de nuevo mas tarde.");
            }
        } //Publicar

        /// <summary>
        /// Evento click del boton btnFotoSiguiente.
        /// Muestra la siguiente foto guardada en la lista imagen1.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFotoSiguiente_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                /*if (imagen1.Count == 1)
                {*/

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
                /*}
                else
                {
                    btnFotoSiguiente.IsEnabled = false;
                }*/
            }
            catch{
                MessageBox.Show("Algo salio mal :(");
            }
        }

        /// <summary>
        /// Evento click del boton btnFotoAnterior
        /// Muestra la foto anterior guardada en la lista imagen1.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFotoAnterior_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (imagen1.Count != 1)
                {

                    if (contador == 0)
                    {
                        contador = imagen1.Count - 1;
                        //MessageBox.Show(Convert.ToString(contador));
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
            catch { }
        }

        /// <summary>
        /// Evento click del boton btnCancelarProducto.
        /// Cierra el formulario AgregarProductoComprador y muestra el formulario MarketplaceComprador.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Evento click del boton Button.
        /// Obtiene la ruta de la imagen seleccionada por el usuario y la convierte en un arreglo de bytes
        /// para agregarla a la lista imagen1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                idFotos.Add(idFotos.Count);
                contadorAux++;
                contador = contadorAux;

                //MessageBox.Show(Convert.ToString(fileuri));
            }

            if(imagen1.Count == 2)
            {
                btnFotoSiguiente.IsEnabled = true;
                btnFotoAnterior.IsEnabled = true;
            }
        }

        /// <summary>
        /// Evento click del boton btnGuardarProducto.
        /// Obtiene la informacion del textBox y el id obtenida como paramentro del formulario
        /// y actualiza la informacion en la base de datos segun el id.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cambios(object sender, RoutedEventArgs e)
        {
            /*try
            {*/
                if (!VerificarCambios(nombreProducto, precio, marca, modelo, cantidad, descripcion, imagen1))
                {
                    ConexionBD bdConexion = new ConexionBD();

                    string consulta = "EXECUTE ModificarProducto @ID, @NOMBRE, @IDCATEGORIA, @DESCRIPCIONPRODUCTO, @PRECIOPRODUCTO, @MARCA, @MODELOPRODUCTO";

                    bdConexion.CerrarConexion();
                    SqlCommand cmd = new SqlCommand(consulta, bdConexion.conexion());
                    cmd.Parameters.AddWithValue("@ID", idProducto);
                    cmd.Parameters.AddWithValue("@nombre", txtNombreProducto.Text);
                    cmd.Parameters.AddWithValue("@IDCATEGORIA", txtCategoria.SelectedIndex + 1);
                    cmd.Parameters.AddWithValue("@DESCRIPCIONPRODUCTO", txtDescripcionn.Text);
                    cmd.Parameters.AddWithValue("@PRECIOPRODUCTO", Convert.ToDouble(txtPrecio.Text));
                    cmd.Parameters.AddWithValue("@MARCA", txtMarca.Text);
                    cmd.Parameters.AddWithValue("@MODELOPRODUCTO", txtModelo.Text);
                    cmd.ExecuteScalar();

                //MessageBox.Show(Convert.ToString(imagen1.Count) + ":Cantidad de imagenes y " + 
                    //Convert.ToString(imagenesCapt));

                    if (imagen1.Count > imagenesCapt)
                    {
                        for (int i = imagenesCapt; i < imagen1.Count; i++)
                        {
                        bdConexion.CerrarConexion();
                            consulta = "INSERT INTO [Foto Producto](IDProducto,FotoProducto) values(@ID,@FOTO)";
                            SqlCommand cmd2 = new SqlCommand(consulta, bdConexion.conexion());
                            cmd2.Parameters.AddWithValue("ID", idProducto);
                            cmd2.Parameters.AddWithValue("@FOTO", imagen1[i]);
                            cmd2.ExecuteScalar();

                        MessageBox.Show("Se guardo la foto");
                        }

                        

                        if (imagen1.Count > 1)
                        {
                            btnFotoAnterior.IsEnabled = true;
                            btnFotoAnterior.IsEnabled = true;
                        }

                    }
                }
                MessageBox.Show("Se han realizado los cambios.");

            var window = new MarketplaceVendedor();
            this.Close();
            window.Show();
            /*}
            catch
            {
                MessageBox.Show("No fue posible conectarse a la base de datos. Intentelo de nuevo mas tarde.");
            }*/
        }

        /// <summary>
        /// No hace nada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnActualizar_Click_1(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(Convert.ToString(txtCategoria.SelectedIndex));

            txtCategoria.SelectedIndex = 1;
        }

        /// <summary>
        /// Evento PreviewTextInput del TextBox txtPrecio
        /// Verifica que el usuario ingrese uno de los caracteres admitidos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPrecio_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9 | .]+").IsMatch(e.Text);
        }

        private void txtNombreProducto_KeyUp(object sender, KeyEventArgs e)
        { 
        }

        private void txtPrecio_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void btnEliminarFoto_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(Convert.ToString(idFotos[contador]));

            if (idFotos[contador] < 0)
            {
                string consulta = "DELETE FROM [Foto Producto] where IDfoto=@IDFOTO";

                conexionBD.CerrarConexion();
                SqlCommand cmd = new SqlCommand(consulta, conexionBD.conexion());
                cmd.Parameters.AddWithValue("@IDFOTO", idFotos[contador] * -1);
                cmd.ExecuteScalar();

                idFotos.RemoveAt(contador);
                imagen1.RemoveAt(contador);
                contador--;
                btnFotoSiguiente.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

                
            }
            else
            {
                imagen1.RemoveAt(contador);
                idFotos.RemoveAt(contador);
                contador--;
                btnFotoSiguiente.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }

            if (imagen1.Count == 0 || imagen1.Count == 1)
            {
                btnFotoSiguiente.IsEnabled = false;
                btnFotoAnterior.IsEnabled = false;
            }

        }

        private void BtnEliminar_MouseEnter(object sender, MouseEventArgs e)
        {
            //this.Background = new SolidColorBrush(Colors.Blue);

        }

        private void btnEliminar_MouseLeave(object sender, MouseEventArgs e)
        {
            //this.Background = new SolidColorBrush(Colors.Red);
        }
        /// <summary>
        /// Evento click del boton eliminar del formulario AgregarProductoVendedor.
        /// Obtiene el id del producto a eliminar y cambia la columna IDEstadoProducto de la tabla [Datos Comprador]
        /// a 2. En caso de tener un id=0 solo limpia los campos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {

            try
            {

                System.Windows.Forms.DialogResult res = (System.Windows.Forms.DialogResult)System.Windows.MessageBox.Show("¿Desea eliminar este producto?", "Confirmación", MessageBoxButton.OKCancel);



                if (res == System.Windows.Forms.DialogResult.OK && idProducto != "0")
                {
                    MessageBox.Show(idProducto);
                    conexionBD.CerrarConexion();
                    string consulta = "UPDATE [Datos Producto] SET IDEstadoProducto = 2 where IDProducto=@IDPRODUCTO";
                    SqlCommand cmd1 = new SqlCommand(consulta, conexionBD.conexion());
                    cmd1.Parameters.AddWithValue("@IDPRODUCTO", idProducto);
                    cmd1.ExecuteScalar();
                    conexionBD.CerrarConexion();

                    MessageBox.Show("Se elimino el producto");

                    var window = new MarketplaceVendedor();
                    this.Close();
                    window.Show();

                }
                if (res == System.Windows.Forms.DialogResult.OK && idProducto == "0")
                {
                    

                    txtNombreProducto.Text = String.Empty;
                    txtPrecio.Text = String.Empty;
                    txtMarca.Text = String.Empty;
                    txtModelo.Text = String.Empty;
                    txtCantidad.Text = String.Empty;
                    txtDescripcionn.Text = String.Empty;
                    txtCategoria.SelectedIndex = -1;

                    var window = new MarketplaceVendedor();
                    this.Close();
                    window.Show();
                }
            }
            catch
            {
                MessageBox.Show("No fue posible eliminar el producto, Intentelo mas tarde");
            }
        }

        /// <summary>
        /// Evento KeyUp del TextBox txtCantidad
        /// Elimina el espacio que haya ingresado el usuario en el TextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCantidad_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                txtCantidad.Text= txtCantidad.Text.Replace(" ", "");

                txtCantidad.Select(txtCantidad.Text.Length, 0);
            }
        }

        /// <summary>
        /// Evento KeyUp del TextBox txtPrecio
        /// Elimina el espacio que haya ingresado el usuario en el TextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPrecio_KeyUp(object sender, KeyEventArgs e)
        {
            string precio = txtPrecio.Text;
            //char letra;
            int contador=0;
            if (e.Key == Key.Space)
            {
                txtPrecio.Text= txtPrecio.Text.Replace(" ", "");
                txtPrecio.Select(txtPrecio.Text.Length,0);
            }
            if(e.Key == Key.Decimal)
            {
                //MessageBox.Show("Presiono un punto");
                for(int i=0;i< precio.Length; i++)
                {
                    if (precio[i] == '.')
                    {
                        if (contador >= 1)
                        {
                            StringBuilder sb = new StringBuilder(precio);
                            sb[i] = ' ';

                            precio = sb.ToString();

                            precio = precio.Replace(" ", "");
                        }
                        else
                        {
                            contador++;
                        }
                    }
                }
                txtPrecio.Text = precio;

                txtPrecio.Select(txtPrecio.Text.Length, 0);
            }
        }

        /// <summary>
        /// Evento click de boton dinamico.
        /// Obtiene el id del producto y lo agrega a la base de datos en la tabla de FacturaDetalle
        /// segun el id del usuario y la factura que tenga pendiente.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Comprar(object sender, RoutedEventArgs e)
        {
            ConexionBD bdConexion = new ConexionBD();

            try
            {

                string consulta = "EXECUTE AgregarCarrito @IDPRODUCTO,@IDCOMPRADOR,@CANTIDAD";

                SqlCommand cmd = new SqlCommand(consulta, bdConexion.conexion());
                cmd.Parameters.AddWithValue("@IDPRODUCTO", idProducto);
                cmd.Parameters.AddWithValue("@IDCOMPRADOR", datosPersonales.Id);
                cmd.Parameters.AddWithValue("@CANTIDAD", 1);
                cmd.ExecuteScalar();

                MessageBox.Show("Se añadio el producto al carrito");
            }
            catch
            {
                MessageBox.Show("No fue posible conectarse a la base de datos. Intentelo de nuevo mas tarde.");
            }

        }

        
        private Boolean VerificarCambios(string nombre, string precio, string marca, string modelo, string cantidad, string descripcion, List<byte[]> imagenesBD)
        {

            if(nombre == txtNombreProducto.Text)
            {
                return false;
            }
            else if (precio == txtPrecio.Text)
            {
                return false;
            }
            else if (marca == txtMarca.Text)
            {
                return false;
            }
            else if (modelo == txtMarca.Text)
            {
                return false;
            }
            else if (cantidad == txtCantidad.Text)
            {
                return false;
            }
            else if (descripcion == txtDescripcionn.Text)
            {
                return false;
            }
            if(imagenesBD == imagen1)
            {
                return false;
            }
            return true;
        }
    }
}
