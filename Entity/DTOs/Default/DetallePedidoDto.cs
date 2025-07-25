using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Default
{
    public class DetallePedidoDto
    {
        public int IdDetalle { get; set; }
        public int IdPizza { get; set; }
        public string NombrePizza { get; set; }
        public int Cantidad { get; set; }
        public decimal Subtotal { get; set; }
    }
}
