using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models
{
    public class Cliente
    {
        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string phone_number { get; set; }
        public bool active { get; set; }
        public bool is_deleted { get; set; }
        public User User { get; set; }
        public ICollection<Pedido> Pedidos { get; set; }
    }
}
