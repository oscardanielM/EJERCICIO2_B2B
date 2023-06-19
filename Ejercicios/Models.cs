using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejercicios
{
    public class Models
    {
        
    }

    public class Factura
    {
        public Decimal? total { get; set; }
        public string ordenDeCompra { get; set; }
        public string id { get; set; }
        public string type { get; set; }
        public List<Partidas> partidas { get; set; }
    }

    public class Partidas
    {
        public string ordenDeCompra { get; set;}
        public string ReceiverTicket { get; set;}
        public string Producto { get; set;}
        public int? Cantidad { get; set;}
        public string id { get; set;}
        public decimal? Precio { get; set;}
    }

    public class ModelPOST
    {
        public string userID { get; set; }
        public string companyID { get; set; }
        public string portalID { get; set; }
        public string facturaID { get; set; }
        public string notification { get; set; }
    }

    public class FacturaPrueba2
    {
        public Decimal? total { get; set; }
        public Comprobante comprobante { get; set; }
        public Error error { get; set; }

    }

    public class Comprobante
    {
        public string url { get; set; }
    }


    public class Error
    {
        public string errorMSJ { get; set; }
    }

    public class ERRORESFAC
    {
        public string errorMSJ { get; set; }
        public string Total { get; set; }
        public int Cantidad { get; set; }
    }
}
