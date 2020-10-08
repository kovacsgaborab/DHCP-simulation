using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
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


        static void Feladatok()
        {
            foreach (var commmand in commands)
            {
                Feladat(commmand);
            }
        }
        static void Feladat(string parancs)
        {

            /*
             * megnézzük, hogy request-e
             *              *          
             * ki kell szedni a MAC címet a parancsból
             */

            

            if (parancs.Contains("request"))
            {
                string[] a = parancs.Split(';');
                string mac = a[1];

                if (dhcp.ContainsKey(mac))
                {
                    Console.WriteLine($"DHCP {mac} --> {dhcp[mac]}");
                }
                else
                {
                    if (reserved.ContainsKey(mac))
                    {
                        Console.WriteLine($"Res.: {mac} --> {reserved[mac]}");
                        dhcp.Add(mac, reserved[mac]);
                    }
                    else
                    {
                        string indulo = "192.168.10.100";
                        int okt4 = 100;

                        while (okt4 < 200 && (dhcp.ContainsValue(indulo) || reserved.ContainsValue(indulo) || excluded.Contains(indulo)))
                        {
                            okt4++;
                            indulo = CimEggyelNo(indulo);
                        }

                        if (okt4 < 200)
                        {
                            Console.WriteLine($"Kiosztott: {mac} --> {indulo}");
                            dhcp.Add(mac, indulo);
                        }
                        else
                        {
                            Console.WriteLine($"{mac} nincs IP");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Nem oké");
            }
        }


        static void Main(string[] args)
        {
            BeolvasList(excluded, "excluded.csv");
            BeolvasList(commands, "test.csv");
            BeolvasDictionary(dhcp, "dhcp.csv");
            BeolvasDictionary(reserved, "reserved.csv");


            //foreach (var e in commands)
            //{
            //    Console.WriteLine(e);
            //}

            Feladatok();
            

            //Console.WriteLine(CimEggyelNo("192.168.10.255"));

            Console.WriteLine("\n.......................................................................................................................");

            Console.ReadLine();
        }
    }
}
