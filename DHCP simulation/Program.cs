using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DHCP_simulation
{
    class Program
    {
        static List<string> excluded = new List<string>();

        static Dictionary<string, string> dhcp = new Dictionary<string, string>();

        static Dictionary<string, string> reserved = new Dictionary<string, string>();

        static List<string> commands = new List<string>();


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

        static void BeolvasDictionary(Dictionary<string, string> d, string filenev)
        {
            try
            {
                StreamReader file = new StreamReader(filenev);

                while (!file.EndOfStream)
                {
                    string[] adat = file.ReadLine().Split(';');

                    d.Add(adat[0], adat[1]);
                }

                file.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        static void BeolvasList(List<string> l, string filename)
        {
            try
            {
                StreamReader file = new StreamReader(filename);

                try                                                      //megpróbálja végigvinni a filet
                {
                    while (!file.EndOfStream)
                    {
                        l.Add(file.ReadLine());
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
            BeolvasList(excluded, "excluded.csv");
            BeolvasList(commands, "test.csv");
            BeolvasDictionary(dhcp, "dhcp.csv");
            BeolvasDictionary(reserved, "reserved.csv");


            foreach (var e in commands)
            {
                Console.WriteLine(e);
            }

            //Console.WriteLine(CimEggyelNo("192.168.10.255"));

            Console.WriteLine("\n.......................................................................................................................");

            Console.ReadLine();
        }
    }
}
