using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHCP_simulation
{
    class Program
    {
        static List<string> excluded = new List<string>();

        static string CimEggyelNo(string cim)
        {
            /* cim = "192.168.10.100"
             * return 192.168.10.101"
             * 
             * szétvágni a pötty mentén
             * az utolsót intté konvertálni
             * egyet hozzáadni
             * 255-öt ne lépjük túl
             * összefűzni stringgé
             */
            
            string[] adat = cim.Split('.');
            int okt4 = Convert.ToInt32(adat[3]);

            if (okt4 < 255)
            {
                okt4++;
            }

            return adat[0] + '.' + adat[1] + '.' + adat[2] + '.' + Convert.ToString(okt4);
        }

        static void BeolvasReserved()
        {

        }
        static void BeolvasExcluded()
        {
            try
            {
                StreamReader file = new StreamReader("excluded.csv");

                try                                                      //megpróbálja végigvinni a filet
                {
                    while (!file.EndOfStream)
                    {
                        excluded.Add(file.ReadLine());
                    }
                }
                catch (Exception expection)                              //ha valami hiba van, azt kiírja
                {
                    Console.WriteLine(expection.Message);                    
                }
                finally                                                  //végül bezárja a streamreadert
                {
                    file.Close();
                }

                file.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void Main(string[] args)
        {
            BeolvasExcluded();

            //foreach (var e in excluded)
            //{
            //    Console.WriteLine(e);
            //}

            //Console.WriteLine(CimEggyelNo("192.168.10.255"));

            Console.WriteLine("\nVége...");

            Console.ReadLine();
        }
    }
}
