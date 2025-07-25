using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Default
{
    public class CrearPedidoDto
    {
        public int IdCliente { get; set; }
        public int IdUsuario { get; set; }
        public List<CrearDetalleDto> Detalles { get; set; }
    }
}
