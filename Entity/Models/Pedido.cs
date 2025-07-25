using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models
{
    public class Pedido
    {
        [Key]
        public int IdPedido { get; set; } // ✅ Clave primaria

        public int IdCliente { get; set; }

        public DateTime FechaPedido { get; set; } = DateTime.Now;

        public string Estado { get; set; } = "Pendiente";

        public int IdUsuario { get; set; }

        // Relaciones (opcional si estás navegando con EF)
        [ForeignKey("IdCliente")]
        public Cliente Cliente { get; set; }

        [ForeignKey("IdUsuario")]
        public User Usuario { get; set; }

        public ICollection<DetallePedido> Detalles { get; set; }
    }
}
