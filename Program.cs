using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Collections;
using mrdiff;
// Vegard Steffensen SVV 12.19
namespace mrdiff
{
    public class Vehicle
    {
        public Vehicle(string Regnbr, string Land, string Euroclass, string Fuel, string Hybrid, string Allowed_weight, string Vehicle_group, string Colour, string Brand, string Model, string Regyear, string Length)
        {
            regnbr = Regnbr;
            land = Land;
            euroclass = Euroclass;
            fuel = Fuel;
            hybrid = Hybrid;
            allowed_weight = Allowed_weight;
            vehicle_group = Vehicle_group;
            colour = Colour;
            brand = Brand;
            model = Model;
            regyear = Regyear;
            length = Length;
        }
        public string regnbr { get; set; }
        public string land { get; set; }
        public string euroclass { get; set; }
        public string fuel { get; set; }
        public string hybrid { get; set; }
        public string allowed_weight { get; set; }
        public string vehicle_group { get; set; }
        public string colour { get; set; }
        public string brand { get; set; }
        public string model { get; set; }
        public string regyear { get; set; }
        public string length { get; set; }
        public bool Equal(Vehicle v)
        {
            if (this.regnbr == v.regnbr &&
                this.land == v.land &&
                this.land == v.land &&
                this.euroclass == v.euroclass &&
                this.hybrid == v.hybrid &&
                this.allowed_weight == v.allowed_weight &&
                this.vehicle_group == v.vehicle_group &&
                this.colour == v.colour &&
                this.brand == v.brand &&
                this.model == v.model &&
                this.regyear == v.regyear &&
                this.length == v.length) return true;
            else return false;

        }

    }
    class Program
    {
        //Main method
        static int Main(string[] args)
        {
            //Main local varaibles
            string in_path1 = "";
            string in_path2 = "";
            string out_path = @"output.log";
            string missing1 = "";   //Missing vechicles for one input for result file
            string missing2 = "";   //Missing vechicled for second input for result file
            string difference = ""; //Vichle that is present in both files but with different attributes for result file
            int nbr_vehicles1 = 0; //Number of vehicles in file 1
            int nbr_vehicles2 = 0; //Number of vehicles in file 1
            string string_line;
            string[] tmp_str_arr;
            string[] vehicle_data = null;
            string tmp_str="";
            int line_cnt = 1;
            // Vehicle tmp_vehicle = new Vehicle("", "", "", "", "", "", "", "", "", "", "", "");
            SortedDictionary<string, Vehicle> mrlist_1 = new SortedDictionary<string, mrdiff.Vehicle>();
            SortedDictionary<string, Vehicle> mrlist_2 = new SortedDictionary<string, mrdiff.Vehicle>();
            //Local function
            int numberofvehicles(string in_file)
            {
                int max_line = 0;
                // Open the file to read from file number 1
                using (StreamReader sr = File.OpenText(in_file))
                {
                    //First line give number of input
                    if ((string_line = sr.ReadLine()) != null)
                    {
                        tmp_str_arr = string_line.Split(';');
                        tmp_str = "1"; //First line input
                        if (tmp_str_arr[0].Equals(tmp_str) == false)
                        {
                            Console.WriteLine("illigal value in line 1");
                            return 0;
                        }
                        else
                        {
                            if (int.TryParse(tmp_str_arr[1], out max_line))
                                return max_line;
                        }

                    }
                    return 0;
                }
            }
            if (args.Length == 0)
            {
                Console.WriteLine("No input argument, Use /h for help");
                return 0;
            }
            if (args[0] == "/h")
            {
                //Console.Clear();
                Console.WriteLine(" ");
                Console.WriteLine("Name: Miljøregisteret find differences in MR output files");
                Console.WriteLine("Synopsis: mrdiff [OPTION] mrfile1 mrfile2 resultfile<output.log>");
                Console.WriteLine("Description: Mandatory parameters are mrfile1 and mrfile2. Optionalt parameter resultfile. Default output.log");
                Console.WriteLine("Options: /h = help");
                //Console.WriteLine("Options: /l = ignore difference in length"); 
                return 0;
            }
            else
            {
                if (args.Length >= 2)
                {
                    in_path1 = args[0];
                    in_path2 = args[1];
                    if (!File.Exists(in_path1) || (!File.Exists(in_path2)))
                    {
                        Console.WriteLine("No valid input argument, Use /h for help");
                        return 0;
                    }
                    if (args.Length > 2)
                        out_path = args[2];
                }
                else
                {
                    Console.WriteLine("Too few arguments, Use /h for help");
                    return 0;
                }
            }
            //Get number of vehicles in file 1:
            nbr_vehicles1 = numberofvehicles(in_path1);
            if (nbr_vehicles1 < 1)
            {
                Console.WriteLine("Error in " + in_path1 + "Number of vehicles in file:" + nbr_vehicles1);
                return 0;
            }

            using (StreamWriter sw = File.CreateText(out_path))
            {
                sw.WriteLine("************* Verifying MR files **************");
                sw.WriteLine(" MR file 1: " + in_path1 + " MR file 2: " + in_path2);
                sw.WriteLine("***********************************************");
            }
            Console.WriteLine("Buliding db for input file 1: " + in_path1);
            // Open the file to read from file number 1
            line_cnt = 1;
            using (StreamReader sr = File.OpenText(in_path1))
            {
                while ((string_line = sr.ReadLine()) != null)
                {
                    if (line_cnt >= 2)
                    {
                        tmp_str_arr = string_line.Split(';');
                        tmp_str = "2";  //Vehicle line ='2' 
                        if (tmp_str_arr[0].Equals(tmp_str) == false)
                        {
                            Console.WriteLine("illigal value in line " + line_cnt);
                            return 0;
                        }
                        Vehicle tmp_vehicle = new Vehicle(tmp_str_arr[1], tmp_str_arr[2], tmp_str_arr[3], tmp_str_arr[4], tmp_str_arr[5], tmp_str_arr[6], tmp_str_arr[7], tmp_str_arr[8], tmp_str_arr[9], tmp_str_arr[10], tmp_str_arr[11], tmp_str_arr[12]);
                        try
                        {
                            mrlist_1.Add((tmp_str_arr[1] + tmp_str_arr[2]), tmp_vehicle);
                        }
                        catch (ArgumentException)
                        {
                            using (StreamWriter sw = File.AppendText(out_path))
                            {
                                sw.WriteLine("-------------  Error in " + in_path1 + ": An element with Key = " + tmp_str_arr[1] + " already exists." + " In line: " + line_cnt + "   --------------------");
                                sw.WriteLine("2;" + tmp_str_arr[1] +";"+ tmp_str_arr[2] + ";" + tmp_str_arr[3] + ";" + tmp_str_arr[4] + ";" + tmp_str_arr[5] + ";" + tmp_str_arr[6] + ";" + tmp_str_arr[7] + ";" + tmp_str_arr[8] + ";" + tmp_str_arr[9] + ";" + tmp_str_arr[10] + ";" + tmp_str_arr[11] + ";" + tmp_str_arr[12]);
                            }
                        }
                        line_cnt++;
                    }
                    else
                    {
                        // firt line
                        line_cnt++;
                    }
                } //End of loop

                line_cnt = line_cnt - 2;  //To adjeust for first and last line. 
                if (line_cnt != nbr_vehicles1)
                {
                    using (StreamWriter sw = File.AppendText(out_path))
                    {
                        sw.WriteLine("Error: Mismatch in reported and actual rows for file1: Reported: " + nbr_vehicles1 + " actual: " + line_cnt);
                    }
                    //Console.WriteLine("Mismatch in reported and actual rows for file1: Reported: " + nbr_vehicles1 + " actual: " + line_cnt);
                }
            }
            /// Second file:
            //Get number of vehicles in file 2:
            nbr_vehicles2 = numberofvehicles(in_path2);
            if (nbr_vehicles1 < 1)
            {
                Console.WriteLine("Error in " + in_path2 + "Number of vehicles in file:" + nbr_vehicles2);
                return 0;
            }
            // Open the file to read from file number 2
            Console.WriteLine("Buliding db for input file 2: " + in_path2);
            line_cnt = 1;
            using (StreamReader sr = File.OpenText(in_path2))
            {
                while ((string_line = sr.ReadLine()) != null)
                {
                    if (line_cnt >= 2)
                    {
                        tmp_str_arr = string_line.Split(';');
                        tmp_str = "2";  //Vehicle line ='2' 
                        if (tmp_str_arr[0].Equals(tmp_str) == false)
                        {
                            Console.WriteLine("illigal value in line " + line_cnt);
                            return 0;
                        }
                        Vehicle tmp_vehicle = new Vehicle(tmp_str_arr[1], tmp_str_arr[2], tmp_str_arr[3], tmp_str_arr[4], tmp_str_arr[5], tmp_str_arr[6], tmp_str_arr[7], tmp_str_arr[8], tmp_str_arr[9], tmp_str_arr[10], tmp_str_arr[11], tmp_str_arr[12]);
                        try
                        {
                            mrlist_2.Add((tmp_str_arr[1] + tmp_str_arr[2]), tmp_vehicle);
                        }
                        catch (ArgumentException)
                        {
                            using (StreamWriter sw = File.AppendText(out_path))
                            {
                                sw.WriteLine("-------------  Error in " + in_path1 + ": An element with Key = " + tmp_str_arr[1] + " already exists." + " In line: " + line_cnt + "   --------------------");
                                sw.WriteLine("2;" + tmp_str_arr[1] + ";" + tmp_str_arr[2] + ";" + tmp_str_arr[3] + ";" + tmp_str_arr[4] + ";" + tmp_str_arr[5] + ";" + tmp_str_arr[6] + ";" + tmp_str_arr[7] + ";" + tmp_str_arr[8] + ";" + tmp_str_arr[9] + ";" + tmp_str_arr[10] + ";" + tmp_str_arr[11] + ";" + tmp_str_arr[12]);
                            }
                        }
                        line_cnt++;
                    }
                    else
                    {
                        // firt line
                        line_cnt++;
                    }
                } //End of loop

                line_cnt = line_cnt - 2;  //To adjeust for first and last line. 
                if (line_cnt != nbr_vehicles1)
                {
                    using (StreamWriter sw = File.AppendText(out_path))
                    {
                        sw.WriteLine("Error: Mismatch in reported and actual rows for file1: Reported: " + nbr_vehicles2 + " actual: " + line_cnt);
                    }
                    //more deConsole.WriteLine("Mismatch in reported and actual rows for file1: Reported: " + nbr_vehicles2 + " actual: " + line_cnt);
                }

            }


            // Start comaparing data between the files:
            Console.WriteLine("Start comparing data ");
            if (nbr_vehicles1 <= nbr_vehicles2)
            {
                foreach (KeyValuePair<string, Vehicle> kvp in mrlist_1)
                {
                    Vehicle out_vehicle = new Vehicle("", "", "", "", "", "", "", "", "", "", "", "");
                    if (mrlist_2.TryGetValue(kvp.Key, out out_vehicle))
                    { //Vehicle found verifying data

                        if (!(out_vehicle.Equal(kvp.Value)))
                        {

                            difference += "---------------------  Difference in Vehicle attributes for reg.nr: " + out_vehicle.regnbr + "   --------------------" + Environment.NewLine;
                            difference += "Input 1 :" + "2;" + kvp.Value.regnbr + ";" + kvp.Value.land + ";" + kvp.Value.euroclass + ";" + kvp.Value.fuel + ";" + kvp.Value.hybrid + ";" + kvp.Value.allowed_weight + ";" + kvp.Value.vehicle_group + ";" + kvp.Value.colour + ";" + kvp.Value.brand + ";" + kvp.Value.model + ";" + kvp.Value.regyear + ";" + kvp.Value.length + Environment.NewLine;
                            difference += "Input 2 :" + "2;" + out_vehicle.regnbr + ";" + out_vehicle.land + ";" + out_vehicle.euroclass + ";" + out_vehicle.fuel + ";" + out_vehicle.hybrid + ";" + out_vehicle.allowed_weight + ";" + out_vehicle.vehicle_group + ";" + out_vehicle.colour + ";" + out_vehicle.brand + ";" + out_vehicle.model + ";" + out_vehicle.regyear + ";" + out_vehicle.length + Environment.NewLine;
                        }
                    }
                    //Console.WriteLine("Key matched = :" + out_vehicle.regnbr + out_vehicle.land);
                    else
                    {
                        missing1 += "---------------------  Missing in Vehicle reg.nr: " + kvp.Value.regnbr + " In file 2 :" + in_path2 + "   --------------------" + Environment.NewLine;
                        missing1 += "Input 1 :" + "2;" + kvp.Value.regnbr + ";" + kvp.Value.land + ";" + kvp.Value.euroclass + ";" + kvp.Value.fuel + ";" + kvp.Value.hybrid + ";" + kvp.Value.allowed_weight + ";" + kvp.Value.vehicle_group + ";" + kvp.Value.colour + ";" + kvp.Value.brand + ";" + kvp.Value.model + ";" + kvp.Value.regyear + ";" + kvp.Value.length + Environment.NewLine;
                    }
                }//Foreach
                foreach (KeyValuePair<string, Vehicle> kvp2 in mrlist_2)
                {
                    Vehicle out_vehicle = new Vehicle("", "", "", "", "", "", "", "", "", "", "", "");
                    if (!(mrlist_1.TryGetValue(kvp2.Key, out out_vehicle)))
                    { //Vehicle not found                     }
                        missing2 += "---------------------  Missing in Vehicle reg.nr: " + kvp2.Value.regnbr + " In file 2 :" + in_path2 + "   --------------------" + Environment.NewLine;
                        missing2 += "Input 2 :" + "2;" + kvp2.Value.regnbr + ";" + kvp2.Value.land + ";" + kvp2.Value.euroclass + ";" + kvp2.Value.fuel + ";" + kvp2.Value.hybrid + ";" + kvp2.Value.allowed_weight + ";" + kvp2.Value.vehicle_group + ";" + kvp2.Value.colour + kvp2.Value.brand + ";" + kvp2.Value.model + ";" + kvp2.Value.regyear + ";" + kvp2.Value.length + Environment.NewLine;
                    }
                }//Foreach
            }
            else
            {
                foreach (KeyValuePair<string, Vehicle> kvp in mrlist_2)
                {
                    Vehicle out_vehicle = new Vehicle("", "", "", "", "", "", "", "", "", "", "", "");
                    if (mrlist_1.TryGetValue(kvp.Key, out out_vehicle))
                    { //Vehicle found verifying data

                        if (!(out_vehicle.Equal(kvp.Value)))
                        {

                            difference += "---------------------  Difference in Vehicle attributes for reg.nr: " + out_vehicle.regnbr + "   --------------------" + Environment.NewLine;
                            difference += "Input 1 :" + "2;" + out_vehicle.regnbr + ";" + out_vehicle.land + ";" + out_vehicle.euroclass + ";" + out_vehicle.fuel + ";" + out_vehicle.hybrid + ";" + out_vehicle.allowed_weight + ";" + out_vehicle.vehicle_group + ";" + out_vehicle.colour + ";" + out_vehicle.brand + ";" + out_vehicle.model + ";" + out_vehicle.regyear + ";" + out_vehicle.length + Environment.NewLine;
                            difference += "Input 2 :" + "2;" + kvp.Value.regnbr + ";" + kvp.Value.land + ";" + kvp.Value.euroclass + ";" + kvp.Value.fuel + ";" + kvp.Value.hybrid + ";" + kvp.Value.allowed_weight + ";" + kvp.Value.vehicle_group + ";" + kvp.Value.colour + ";" + kvp.Value.brand + ";" + kvp.Value.model + ";" + kvp.Value.regyear + ";" + kvp.Value.length + Environment.NewLine;
                        }
                    }
                    //Console.WriteLine("Key matched = :" + out_vehicle.regnbr + out_vehicle.land);
                    else
                    {
                        missing2 += "---------------------  Missing in Vehicle reg.nr: " + kvp.Value.regnbr + " In file 1 :" + in_path2 + "   --------------------" + Environment.NewLine;
                        missing2 += "Input 2 :" + "2;" + kvp.Value.regnbr + ";" + kvp.Value.land + ";" + kvp.Value.euroclass + ";" + kvp.Value.fuel + ";" + kvp.Value.hybrid + ";" + kvp.Value.allowed_weight + ";" + kvp.Value.vehicle_group + ";" + kvp.Value.colour + ";" + kvp.Value.brand + ";" + kvp.Value.model + ";" + kvp.Value.regyear + ";" + kvp.Value.length + Environment.NewLine;
                    }
                }//Foreach
                foreach (KeyValuePair<string, Vehicle> kvp2 in mrlist_1)
                {
                    Vehicle out_vehicle = new Vehicle("", "", "", "", "", "", "", "", "", "", "", "");
                    if (!(mrlist_2.TryGetValue(kvp2.Key, out out_vehicle)))
                    { //Vehicle not found                     
                        missing1 += "---------------------  Missing in Vehicle reg.nr: " + kvp2.Value.regnbr + " In file 2 :" + in_path2 + "   --------------------" + Environment.NewLine;
                        missing1 += "Input 1 :" + "2;" + kvp2.Value.regnbr + ";" + kvp2.Value.land + ";" + kvp2.Value.euroclass + ";" + kvp2.Value.fuel + ";" + kvp2.Value.hybrid + ";" + kvp2.Value.allowed_weight + ";" + kvp2.Value.vehicle_group + ";" + kvp2.Value.colour + ";" + kvp2.Value.brand + ";" + kvp2.Value.model + ";" + kvp2.Value.regyear + ";" + kvp2.Value.length + Environment.NewLine;
                    }
                }//Foreach
            }
            using (StreamWriter sw = File.AppendText(out_path))
            {
                sw.WriteLine(" ");
                if(missing1.Length>1)
                {
                    sw.WriteLine("************* Missing Vehicles in file MR file 1 ***************");
                    sw.WriteLine(missing1);
                }
                
                if (missing2.Length > 1)
                {
                    sw.WriteLine(" ");
                    sw.WriteLine("************* Missing Vehicles in files MR file 2 **************");
                    sw.WriteLine(missing2);
                }
                if (difference.Length > 1)
                {
                    sw.WriteLine(" ");
                    sw.WriteLine("************** Changes Vehicles files ****************");
                    sw.WriteLine(difference);
                }
                sw.WriteLine("***********************************************");
            }

            Console.WriteLine("Finished comaparing data, see result in: " + out_path);
            return 0;
        }//Main
    }//Program
} //Namespace
