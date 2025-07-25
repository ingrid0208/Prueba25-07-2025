using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models
{
    public class Pizza
    {
        [Key] // Esto es esencial
        public int IdPizza { get; set; }

        [Required]
        [StringLength(100)]
        public string NombreProducto { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Precio { get; set; }

        // Relación con DetallePedido (opcional pero recomendado)
        public ICollection<DetallePedido>? Detalles { get; set; }
    }
}
