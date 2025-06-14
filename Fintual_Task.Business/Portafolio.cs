using Fintual_Task.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Fintual_Task.Negocio
{
    public class Portafolio
    {
        public List<Accion> Acciones { get; set; } = new List<Accion>();

        public Dictionary<string, decimal> DistribucionObjetivo { get; set; } = new Dictionary<string, decimal>();

        public bool AgregarAccion(Accion accion)
        {
            try
            {
                var existente = Acciones.FirstOrDefault(a => a.Nombre == accion.Nombre);
                if (existente != null)
                {
                    var totalCantidad = existente.Cantidad + accion.Cantidad;
                    var precioPromedio = ((existente.PrecioActual * existente.Cantidad) + (accion.PrecioActual * accion.Cantidad)) / totalCantidad;

                    existente.Cantidad = totalCantidad;
                    existente.PrecioActual = precioPromedio;
                }
                else
                {
                    Acciones.Add(accion);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<Accion> GetAcciones()
        {
            return Acciones;
        }

        public Dictionary<string, decimal> Rebalancear()
        {
            var totalActual = Acciones.Sum(a => a.PrecioActual * a.Cantidad);
            var rebalanceo = new Dictionary<string, decimal>();
            decimal umbral = 100m; 
            foreach (var objetivo in DistribucionObjetivo)
            {
                var nombre = objetivo.Key;
                var porcentajeDeseado = objetivo.Value;
                var valorEsperado = totalActual * porcentajeDeseado;

                var accion = Acciones.FirstOrDefault(a => a.Nombre == nombre);
                var valorActual = accion != null ? accion.PrecioActual * accion.Cantidad : 0;

                var diferencia = valorEsperado - valorActual;

                if (Math.Abs(diferencia) >= umbral)
                    rebalanceo[nombre] = diferencia;
                else
                    rebalanceo[nombre] = 0m; 
            }
            return rebalanceo;
        }
    }
}
