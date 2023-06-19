using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Ejercicios
{
    public class Controller
    {

        public async Task LlamaApi(string url, string parametros)
        {
            string responseData = string.Empty;
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Clear();

                url += parametros;
                Console.WriteLine("\n\nEjercicio 1");
                Console.WriteLine("Relizando get");
                using (HttpResponseMessage response = await client.GetAsync(url))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        responseData = await response.Content.ReadAsStringAsync();
                        var JSONModel = JsonConvert.DeserializeObject<Factura>(responseData);
                        decimal SUMATotalPartidas = 0;
                        Console.WriteLine("\n\nEjercicio 2");
                        Console.WriteLine("IDS");
                        foreach (var partida in JSONModel.partidas)
                        {
                            Console.WriteLine("-->" + partida.id);
                            SUMATotalPartidas += partida.Precio == null ? 0 : Convert.ToDecimal(partida.Precio);
                        }

                        Console.WriteLine("\n\nEjercicio 3");
                        Console.WriteLine("TOTAL DE IMPORTE DE PARTIDAS: $" + Math.Round(SUMATotalPartidas, 2));

                        Console.WriteLine("\n\nEjercicio 4");
                        Console.WriteLine("Total factura: " + Math.Round(Convert.ToDecimal(JSONModel.total), 2));
                        Console.WriteLine("Total partidas: " + Math.Round(SUMATotalPartidas, 2));
                        decimal diferencia = Convert.ToDecimal(JSONModel.total) - SUMATotalPartidas;
                        decimal limite = Convert.ToDecimal(0.10);
                        if (diferencia < limite)
                        {
                            Console.WriteLine("\n\nEjercicio 5");
                            Console.WriteLine("Discrepancia permitida");
                            Console.WriteLine("Realizando put");
                            
                            ModelPOST modelPOST = new ModelPOST();
                            modelPOST.userID = "oscarskapee@gmail.com";
                            modelPOST.companyID = "123456789";
                            modelPOST.portalID = "oaXh7EU0ExaygAvvZM3y";
                            modelPOST.facturaID = "L90107";
                            modelPOST.notification = "La factura fue adendada correctamente";

                            string JSONPost = JsonConvert.SerializeObject(modelPOST);
                            StringContent content = new StringContent(
                            JSONPost, Encoding.UTF8, "application/json");

                            await LlamaApiPut("https://us-central1-b2b-hub-82515.cloudfunctions.net/app/api/Ej1", content);
                        }
                        else
                        {
                            Console.WriteLine("Discrepancia no permitida");
                            ModelPOST modelPOST = new ModelPOST();
                            modelPOST.userID = "oscarskapee@gmail.com";
                            modelPOST.companyID = "123456789";
                            modelPOST.portalID = "oaXh7EU0ExaygAvvZM3y";
                            modelPOST.facturaID = "L90100";
                            modelPOST.notification = "La factura no pudo ser procesada ya que presenta una discrepancia de :" + diferencia;

                            string JSONPost = JsonConvert.SerializeObject(modelPOST);
                            StringContent content = new StringContent(
                            JSONPost, Encoding.UTF8, "application/json");

                            await LlamaApiPut("https://us-central1-b2b-hub-82515.cloudfunctions.net/app/api/Ej1", content);
                        }
                    }
                    else
                    {
                        Console.WriteLine("___________________________________");
                        Console.WriteLine("Response invalid");
                        Console.WriteLine("___________________________________");
                        Console.WriteLine(response.ToString());
                        Console.WriteLine("___________________________________");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("___________________________________");
                Console.WriteLine("Ha ocurrido un error en la peticion");
                Console.WriteLine("___________________________________");
                Console.WriteLine(e.ToString());
                Console.WriteLine("___________________________________");
            }
        }

        public async Task LlamaApiPut(string url, StringContent content)
        {
            string responseData = string.Empty;
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Clear();


                using (HttpResponseMessage response = await client.PutAsync(url, content))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("\n\nEjercicio 6");
                        Console.WriteLine("La factura fue adendada correctamente");
                    }
                    else
                    {
                        Console.WriteLine("___________________________________");
                        Console.WriteLine("Response invalid");
                        Console.WriteLine("___________________________________");
                        Console.WriteLine(response.ToString());
                        Console.WriteLine("___________________________________");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("___________________________________");
                Console.WriteLine("Ha ocurrido un error en la peticion");
                Console.WriteLine("___________________________________");
                Console.WriteLine(e.ToString());
                Console.WriteLine("___________________________________");
            }
        }

        public async Task LlamaApi2(string url, string parametros)
        {
            string responseData = string.Empty;
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Clear();

                url += parametros;             
                Console.WriteLine("Relizando get");

                int totalFacturas = 0;
                int totalFacturasError = 0;
                int totalFacturasComp = 0;
                Decimal totalMonetario  = Decimal.Zero;
                using (HttpResponseMessage response = await client.GetAsync(url))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        responseData = await response.Content.ReadAsStringAsync();
                        var JSONModel = JsonConvert.DeserializeObject<List<FacturaPrueba2>>(responseData);
                        List<string> lisErrores = new List<string>();
                        List<ERRORESFAC> lisErroresV = new List<ERRORESFAC>();

                        foreach (var factura in JSONModel)
                        {
                            if (factura.error != null)
                            {
                                totalFacturasError++;
                            }

                            if (factura.comprobante != null)
                            {
                                totalFacturasComp++;
                            }

                            if (factura.error != null || factura.comprobante  != null)
                            {
                                totalFacturas++;
                                totalMonetario += (Decimal)factura.total;
                            }

                            if (factura.error != null)
                            {
                                if (!lisErrores.Contains(factura.error.errorMSJ))
                                {
                                    lisErrores.Add((string)factura.error.errorMSJ);
                                }
                            }

                        }

                        Console.WriteLine("No. total, de facturas procesadas y el total monetario que representa");
                        Console.WriteLine("No. total: " + totalFacturas);
                        Console.WriteLine("No. total monetario: " + Math.Round(totalMonetario, 2));
                        Console.WriteLine("\n\n No. total, de facturas exitosas y el total monetario que representa");
                        Console.WriteLine("Total con comprobantes: " + totalFacturasComp);
                         Console.WriteLine("Total con error: " + totalFacturasError);
                        decimal totalMon = decimal.Zero;
                        
                        for (int i = 0; i < lisErrores.Count; i++)
                        {
                            decimal totalError = decimal.Zero;
                            int cantidad = 0;
                            foreach (var item in JSONModel)
                            {
                                if (item.error != null)
                                {
                                    if (item.error.errorMSJ.Equals(lisErrores[i])) {
                                        totalError += Math.Round((decimal)item.total, 2);
                                        totalMon += Math.Round((decimal)item.total, 2);
                                        cantidad++;
                                    }   
                                }
                            }

                            lisErroresV.Add(new ERRORESFAC
                            {
                                errorMSJ = lisErrores[i],
                                Cantidad = cantidad,
                                Total = totalError.ToString()
                            }); ;
                        }

                        Console.WriteLine("\n\nNo. total, de facturas con algún tipo de error y el total monetario que representa");
                        Console.WriteLine("No. total: " + totalFacturasError);
                        Console.WriteLine("No. total monetario: " + totalMon);

                        foreach (var item in lisErroresV)
                        {
                            Console.WriteLine("\nTipo de Error " + item.errorMSJ);
                            Console.WriteLine("\nNumero de facturas con ese error " + item.Cantidad);
                            Console.WriteLine("Total del monetario del error: " + Math.Round(Convert.ToDecimal(item.Total),2));
                        }
                        
                    }
                    else
                    {
                        Console.WriteLine("___________________________________");
                        Console.WriteLine("Response invalid");
                        Console.WriteLine("___________________________________");
                        Console.WriteLine(response.ToString());
                        Console.WriteLine("___________________________________");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("___________________________________");
                Console.WriteLine("Ha ocurrido un error en la peticion");
                Console.WriteLine("___________________________________");
                Console.WriteLine(e.ToString());
                Console.WriteLine("___________________________________");
            }
        }

    }
}
