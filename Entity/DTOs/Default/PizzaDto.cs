using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.Default
{
    public class PizzaDto
    {
        public int IdPizza { get; set; }
        public string NombreProducto { get; set; }
        public decimal Precio { get; set; }
    }
}
