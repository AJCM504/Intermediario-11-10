using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intermediario
{
    internal class DatosProducto
    {
        string nombre, categoria, precio, marca, modelo;

        public DatosProducto(string nombre, string categoria, string precio, string marca, string modelo)
        {
            this.nombre = nombre;
            this.categoria = categoria;
            this.precio = precio;
            this.marca = marca;
            this.modelo = modelo;
        }

        public int VerificarProductos()
        {

            int contador=0;

            if(nombre != "" || nombre.Length< 8) { contador++; }
            if (categoria != "") { contador++; }
            if(precio != "") { contador++; }
            if(marca != "") { contador++; }
            if(modelo != "") { contador++; }

            return contador;
        }
    }
}
