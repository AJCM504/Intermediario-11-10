using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.IO;
using System.Windows.Forms;

namespace Intermediario.Facturas
{
    /// <summary>
    /// Clase para genera las facturas en archivo pdf.
    /// </summary>
    internal class ClassFactura
    {
        string nombre;
        string correo;
        double total;
        string idFactura;
        List<string> nombreProducto = new List<string>();
        List<double> precioProducto = new List<double>();

        /// <summary>
        /// Metodo contructor de la clase ClassFactura.
        /// Obtiene los valores fundamentales para la factura
        /// </summary>
        /// <param name="nombreCliente">Nombre del Usuario</param>
        /// <param name="correoCliente">Correo del Usuario</param>
        /// <param name="total">El total de la factura</param>
        /// <param name="nombreProducto">Lista de los nombres de los productos</param>
        /// <param name="precioProducto">lista de los precios de los productos</param>
        /// <param name="id">id de la factura</param>
        public ClassFactura(string nombreCliente, string correoCliente, double total, List<string> nombreProducto,List<double> precioProducto,string id)
        {
            this.nombre = nombreCliente;
            this.total = total;
            this.correo = correoCliente;
            this.nombreProducto = nombreProducto;
            this.precioProducto = precioProducto;
            this.idFactura = id;
        }

        /// <summary>
        /// Metodo de la clase ClassFactura que crea la factura en un archivo pdf.
        /// </summary>
        public void CrearFactura()
        {
            

            string tabla = Properties.Resources.plantillaFactura.ToString();
            tabla = tabla.Replace("@cliente", nombre);
            tabla = tabla.Replace("@DOCUMENTO", correo);
            tabla = tabla.Replace("@FECHA", DateTime.Now.ToString("dd/MM/yyyy"));
            tabla = tabla.Replace("@TOTAL", Convert.ToString(total));
            tabla = tabla.Replace("@IDFACTURA", idFactura);



            string filas = string.Empty;

            for (int i = 0; i < nombreProducto.Count; i++)
            {
                filas += "<tr>";
                filas += "<td>1</td>";
                filas += "<td>" + nombreProducto[i] + "</td>";
                filas += "<td>" + precioProducto[i] + "</td>";
                filas += "<td>" + Convert.ToString(((precioProducto[i] * 0.15) + precioProducto[i])) + "</td>";
                filas += "</tr>";

            }

            tabla = tabla.Replace("@FILAS", filas);
            


            SaveFileDialog guardar = new SaveFileDialog();
            guardar.FileName = DateTime.Now.ToString("ddMMyyyy") + ".pdf";
            

            if (guardar.ShowDialog() == DialogResult.OK)
            {

                using (FileStream stream = new FileStream(guardar.FileName, FileMode.Create))
                {
                    Document pdfDoc = new Document(PageSize.A4, 25, 25, 25, 25);

                    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, stream);

                    pdfDoc.Open();

                    

                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(Properties.Resources.Logo_Intermediario,System.Drawing.Imaging.ImageFormat.Png);
                    img.ScaleToFit(80,60);
                    img.Alignment = iTextSharp.text.Image.UNDERLYING;
                    img.SetAbsolutePosition(pdfDoc.LeftMargin,pdfDoc.Top -45);
                    pdfDoc.Add(img);


                    using (StringReader sr = new StringReader(tabla))
                    {
                        XMLWorkerHelper.GetInstance().ParseXHtml(pdfWriter, pdfDoc, sr);
                    }

                        pdfDoc.Close();

                    stream.Close();

                }

            }

            

        }
    }
}
