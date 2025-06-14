using Fintual_Task.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fintual_Task.Negocio
{
    public class Portafolio
    {
        public List<Accion> Acciones { get; set; } = new List<Accion>();

        public Dictionary<string, decimal> DistribucionObjetivo { get; set; } = new Dictionary<string, decimal>();

        public void AgregarAccion(Accion accion)
        {
            Acciones.Add(accion);
        }

        public Dictionary<string, decimal> Rebalancear()
        {
            var totalActual = Acciones.Sum(a => a.GetValor());
            var rebalanceo = new Dictionary<string, decimal>();

            foreach (var objetivo in DistribucionObjetivo)
            {
                var nombre = objetivo.Key;
                var porcentajeDeseado = objetivo.Value;
                var valorEsperado = totalActual * porcentajeDeseado;

                var accion = Acciones.FirstOrDefault(a => a.Nombre == nombre);
                var valorActual = accion != null ? accion.GetValor() : 0;

                var diferencia = valorEsperado - valorActual;
                rebalanceo[nombre] = diferencia;
            }
            return rebalanceo;
        }
    }
}
