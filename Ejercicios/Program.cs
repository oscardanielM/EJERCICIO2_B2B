using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejercicios
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Controller controller = new Controller();
            Task.Run(async () =>
            {
                //await controller.LlamaApi("https://us-central1-b2b-hub-82515.cloudfunctions.net/app/api/Ej1", "?userID=oscarskapee@gmail.com&companyID=123456789&portalID=oaXh7EU0ExaygAvvZM3y&facturaID=L90107");
                //await controller.LlamaApi("https://us-central1-b2b-hub-82515.cloudfunctions.net/app/api/Ej1", "?userID=oscarskapee@gmail.com&companyID=123456789&portalID=oaXh7EU0ExaygAvvZM3y&facturaID=L90100");                //await controller.LlamaApi("https://us-central1-b2b-hub-82515.cloudfunctions.net/app/api/Ej1", "?userID=oscarskapee@gmail.com&companyID=123456789&portalID=oaXh7EU0ExaygAvvZM3y&facturaID=L90100");
                await controller.LlamaApi2("https://us-central1-b2b-hub-82515.cloudfunctions.net/app/api/facturas", "?userID=oscarskapee@gmail.com&companyID=123456789&portalID=oaXh7EU0ExaygAvvZM3y");                //await controller.LlamaApi("https://us-central1-b2b-hub-82515.cloudfunctions.net/app/api/Ej1", "?userID=oscarskapee@gmail.com&companyID=123456789&portalID=oaXh7EU0ExaygAvvZM3y&facturaID=L90100");
            }).GetAwaiter().GetResult();

            Console.ReadLine();
        }
    }
}
