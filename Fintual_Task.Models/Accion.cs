using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fintual_Task.Models
{
    public class Accion
    {
        public string Nombre { get; set; }
        public decimal PrecioActual { get; set; }
        public decimal Cantidad { get; set; }

        public Accion(string nombre, decimal precio, decimal cantidad)
        {
            Nombre = nombre;
            PrecioActual = precio;
            Cantidad = cantidad;
        }

        public decimal GetValor()
        {
            return PrecioActual * Cantidad;
        }
    }

}
