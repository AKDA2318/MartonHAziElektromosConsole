using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MartonHAziElektromosConsole {

    class Program {
        static void Main() {
            // Bemeneti adatok beolvasása
            string[] gateInput = Console.ReadLine().Split(' ');
            string gateType = gateInput[0];
            string input1Name = gateInput[1];
            string input2Name = (gateInput.Length > 2) ? gateInput[2] : null;
            string input3Name = (gateInput.Length > 3) ? gateInput[3] : null;
            List<string[]> Ertekek = ErtekBeolvasas();
            Ertekek = ErtekValaszto(ref Ertekek,input1Name, input2Name,input3Name);
            List<string> eredmeny = LogikaiKapuSzamolo(gateType, input1Name, input2Name,input3Name ,Ertekek);
            kiir(eredmeny);
            Console.ReadKey();

        }
        static void kiir(List<string> eredmenyek) {
            for (int i = 0; i < eredmenyek.Count; i++) {
                Console.Write(eredmenyek[i]);
            }
        }
        private static List<string[]> ErtekValaszto(ref List<string[]> ertekek ,string input1Name, string input2Name,string input3Name) {
            for (int i = 0; i < ertekek.Count; i++) {
                if (ertekek[i][0] != input1Name) {
                    if (ertekek[i][0] != input2Name) {
                        if (ertekek[i][0] != input3Name) {
                            ertekek.Remove(ertekek[i]);
                        }
                        
                    }
                }
            }
            return ertekek;
        }

        static string[] ReadLineWithTimeout(TimeSpan timeout) {
            DateTime startTime = DateTime.Now;
            while (true) {
                if (Console.KeyAvailable) {
                    return Console.ReadLine().Split(' ');
                }
                if (DateTime.Now - startTime > timeout) {
                    string[] vissza = { null, null };
                    return vissza;
                }
            }
        }
        static List<string[]> ErtekBeolvasas() {
            List<string[]> beolvas = new List<string[]>();
            bool amig = true;
            while (amig) {
                string[] temp = ReadLineWithTimeout(TimeSpan.FromMilliseconds(300));
                if (string.IsNullOrEmpty(temp[0])) {
                    amig = false;
                }
                else {
                    beolvas.Add(temp);
                }

            }
            return beolvas;
        }
        static List<string> LogikaiKapuSzamolo(string gateType, string input1Name, string input2Name,string input3Name, List<string[]> Ertekek) {
            List<string> First = new List<string>();
            List<string> Second = new List<string>();
            List<string> Last = new List<string>();
            string temp;
            for (int i = 0; i < Ertekek[0][1].Length; i+=2) {
                temp = Atalakito("" + Ertekek[0][1][i] + Ertekek[0][1][i+1]);
                First.Add(temp);
            }
            if (input2Name!=null) {
                for (int i = 0; i < Ertekek[1][1].Length; i += 2) {
                    temp = Atalakito("" + Ertekek[1][1][i] + Ertekek[1][1][i + 1]);
                    Second.Add(temp);
                }
            }
            if (input3Name != null) {
                for (int i = 0; i < Ertekek[2][1].Length; i += 2) {
                    temp = Atalakito("" + Ertekek[2][1][i] + Ertekek[2][1][i + 1]);
                    Last.Add(temp);
                }
            }
            List<string> eredmenyLista = new List<string>();

            for (int i = 0; i < First.Count; i++) {






                if (input2Name != null) {
                    eredmenyLista.Add(EgysegSzamol(gateType, Convert.ToDouble(First[i]), Convert.ToDouble(Second[i])));// minden másik kapu
                }
                else {
                    eredmenyLista.Add(EgysegSzamol(gateType, Convert.ToDouble(First[i]), 0.0));// a not itt megy végbe
                }
                
            }




            return eredmenyLista;
        }
        static string Atalakito(string input) {
            if (input.Length >= 2) {
                char firstDigit = input[0];
                char secondDigit = input[1];

                return $"{firstDigit},{secondDigit}";
            }
            else {
                return "Invalid input";
            }
        }

        static int BinarissaAlakito(double szam) {
            if (szam >= 0.0 && szam <=0.8 ) { return 0; }
            if (szam<=5.0 && szam >=2.7) { return 1; }
            else { return 2; }

        }
        static string EgysegSzamol(string gateType, double input1, double input2) {
            string eredmeny = "";
            
            int first = BinarissaAlakito(input1);
            int last = 0;
            if (input2 != null) {

                 last = BinarissaAlakito(input2);
            }
                
            if (gateType == "AND") {
                if (first == 2 || last == 2) {
                    eredmeny += "E";
                }
                else {
                    if (first == last && first==1) {
                        eredmeny = "1";
                    }
                    else {
                        eredmeny = "0";
                    }
                }
            }
            else if (gateType == "OR") {
                if (first == 2 || last == 2) {
                    eredmeny += "E";
                }
                else {
                    if (first == 1 || 1== last) {
                        eredmeny = "1";
                    }
                    else {
                        eredmeny = "0";
                    }
                }
            }
            else if (gateType == "NOT") {
                if (first == 2 ) {
                    eredmeny += "E";
                }
                else {
                    if (first == 1 ) {
                        eredmeny = "0";
                    }
                    else {
                        eredmeny = "1";
                    }
                }
            }
            else if (gateType == "NAND") {
                if (first == 2 || input2 == 2) {
                    eredmeny += "E";
                }
                else {
                    if (first == 1 && first == 1) {
                        eredmeny = "0";
                    }
                    else {
                        eredmeny = "1";
                    }
                }
            }
            else if (gateType == "NOR") {
                if (first == 2 || last == 2) {
                    eredmeny += "E";
                }
                else {
                    if (first == 0 || 0 == last && last == first) {
                        eredmeny = "1";
                    }
                    else {
                        eredmeny = "0";
                    }
                }
            }
            else if (gateType == "XOR") {
                if (first == 2 || last == 2) {
                    eredmeny += "E";
                }
                else {
                    if (input1 != last) {
                        eredmeny = "1";
                    }
                    else {
                        eredmeny = "0";
                    }
                }
            }
            else if (gateType == "XNOR") {
                if (first == 2 || last == 2) {
                    eredmeny += "E";
                }
                else {
                    if ((first == last) && (first == 1 || first == 0) ) {
                        eredmeny = "1";
                    }
                    else {
                        eredmeny = "0";
                    }
                }
            }
            else {
                Console.WriteLine("error");
            }
            return eredmeny;        
        }

    }
}