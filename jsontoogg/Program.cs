using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jsontoogg
{
    class Program
    {
        public static readonly char Symbol = '&';
        static void Main(string[] args)
        {
            WriteLineColor("Program for converting new minecraft sounds into human-readable ogg files. Made by lukacat10.");
            WriteLineColor("Please enter the location of the json file that contains all the sound hashes. In the location please include the file it-self.");
            WriteLineColor("Example:" + "&2" + @"'C:\Users\[name]\AppData\Roaming\.minecraft\assets\indexes\1.8.json'");
            
            string jsonlocation = "";
            bool foundjson = false;
            while (foundjson == false)
            {
                jsonlocation = @Console.ReadLine();
                if (jsonlocation == "stop")
                    return;
                if (!(foundjson = System.IO.File.Exists(jsonlocation)))
                {
                    WriteLineColor("&4FILE NOT FOUND! YOU MUST PROVIDE A CORRECT FILE PATH!");
                }
                
            }
            WriteLineColor("File was found. Now you must provide the objects folder which contains folders with their hashes.");
            string objectfolderlocation = "";
            bool foundobjectfolder = false;
            while (foundobjectfolder == false)
            {
                objectfolderlocation = @Console.ReadLine();
                if (objectfolderlocation == "stop")
                    return;
                if (!(foundobjectfolder = System.IO.Directory.Exists(objectfolderlocation)))
                {
                    WriteLineColor("&eDIRECTORY NOT FOUND! YOU MUST PROVIDE A CORRECT DIRECTORY PATH!");
                }

            }
            WriteLineColor("Looks good! Now please enter the " + "&4" + "FOLDER "+ "&f" + "location for the generated .ogg files:");
            WriteLineColor("Example:" + "&2" + @"'C:\Users\[name]\Documents\minecrafted\sounds'");
            string folderlocation = "";
            bool foundfolder = false;
            while (foundfolder == false)
            {
                folderlocation = @Console.ReadLine();
                if (folderlocation == "stop")
                    return;
                if (!(foundfolder = System.IO.Directory.Exists(folderlocation)))
                {
                    WriteLineColor("&4FOLDER NOT FOUND! YOU MUST PROVIDE A CORRECT FOLDER PATH!");
                }

            }
            WriteLineColor("&aProccessing data from json file...");
            JObject jsonFileObject = JObject.Parse(System.IO.File.ReadAllText(jsonlocation));
            List<JToken> ljt = jsonFileObject["objects"].Children().ToList();
            foreach(JToken jt in ljt)
            {
                
                WriteLineColor("----------------------------------");
                string pathofoggfile = jt.Path.Split("'".ToCharArray()[0])[1];//name of path to ogg
                string filenameofogg = pathofoggfile.Split('/').Last();//getting name of ogg file itself
                string extension = pathofoggfile.Split('/').Last().Split('.').Last();//file extension
                if (extension == "ogg")
                {
                    string hashfile = jt.Values("hash").ToArray()[0].ToString();//new hash name
                    string folderhash = hashfile.Substring(0, 2);//folder of hash
                    string finalpath = System.IO.Path.Combine(objectfolderlocation, folderhash);
                    finalpath = System.IO.Path.Combine(finalpath, hashfile);
                    WriteLineColor(finalpath);
                    if(!System.IO.File.Exists(System.IO.Path.Combine(folderlocation, filenameofogg)))
                        System.IO.File.Copy(finalpath, System.IO.Path.Combine(folderlocation, filenameofogg));
                }
            }
        }
        public static bool IsLegalColor(char c)
        {
            bool result = false;
            if (Char.IsNumber(c)) {
                int toBeChecked = int.Parse(c.ToString());
                if(toBeChecked >= 0 && toBeChecked <= 9)
                {
                    result = true;
                }
            }
            else
            {
                if(c >= 97 && c <= 102)
                {
                    result = true;
                }
            }
            return result;
        }//checks if the character after the 'Symbol' sign is allocated to a ConsoleColor
        public static ConsoleColor GetConsoleColor(char c)
        {
            ConsoleColor color;
            switch (c)
            {
                case '0':
                    color = ConsoleColor.Black;
                    break;
                case '1':
                    color = ConsoleColor.DarkBlue;
                    break;
                case '2':
                    color = ConsoleColor.DarkGreen;
                    break;
                case '3':
                    color = ConsoleColor.DarkCyan;
                    break;
                case '4':
                    color = ConsoleColor.DarkRed;
                    break;
                case '5':
                    color = ConsoleColor.DarkMagenta;
                    break;
                case '6':
                    color = ConsoleColor.DarkYellow;
                    break;
                case '7':
                    color = ConsoleColor.Gray;
                    break;
                case '8':
                    color = ConsoleColor.DarkGray;
                    break;
                case '9':
                    color = ConsoleColor.Blue;
                    break;
                case 'a':
                    color = ConsoleColor.Green;
                    break;
                case 'b':
                    color = ConsoleColor.Cyan;
                    break;
                case 'c':
                    color = ConsoleColor.Red;
                    break;
                case 'd':
                    color = ConsoleColor.Magenta;
                    break;
                case 'e':
                    color = ConsoleColor.Yellow;
                    break;
                case 'f':
                    color = ConsoleColor.White;
                    break;
                default:
                    color = ConsoleColor.White;
                    break;
            }
            return color;
        }//returns the ConsoleColor of the specified character (the character is supposed to be after the 'Symbol' sign). Returns ConsoleColor.White if not allocated to any color.
        public static void WriteLineColor(string toBeSent)
        {
            string returned = toBeSent;
            Dictionary<string, ConsoleColor> dic = new Dictionary<string, ConsoleColor>();
            List<KeyValuePair<string, ConsoleColor>> lkvpcc = new List<KeyValuePair<string, ConsoleColor>>();
            //any part before the color codes:
            List<int> specialslocation = new List<int>();//index of Symbol character
            List<char> numberafterspecial = new List<char>();//the number after the Symbol. synced with the int array
            char[] chararray = returned.ToCharArray();//a char array of the string 'toBeSent'
            for (int i = 0; i<(chararray.Length - 1); i++)
            {
                if(chararray[i] == Symbol && IsLegalColor(chararray[i+1]))
                {
                    specialslocation.Add(i);
                    numberafterspecial.Add(chararray[i + 1]);
                    //specialslocation[specialslocation.ToArray().Length] = i;
                    //numberafterspecial[numberafterspecial.ToArray().Length] = chararray[i + 1];
                }
            }
            if (specialslocation.ToArray().Length > 0)
            {
                if(!(specialslocation[0] - 1 < 0))
                    lkvpcc.Add(new KeyValuePair<string, ConsoleColor>(returned.Substring(0, specialslocation[0]), ConsoleColor.White));

            }
            else
            {
                lkvpcc.Add(new KeyValuePair<string, ConsoleColor>(returned.Substring(0, chararray.Length/* - 1*/), ConsoleColor.White));
            }
            ConsoleColor color;
            for(int i = 0; i < specialslocation.ToArray().Length; i++)
            {
                color = GetConsoleColor(chararray[specialslocation[i] + 1]);//getting ConsoleColor for the char in the chararray right after the 'Symbol' sign.
                if(i == specialslocation.ToArray().Length - 1)//checks if the special color is the last one, meaning that to the right of this color code there will be no others
                {
                    lkvpcc.Add(new KeyValuePair<string, ConsoleColor>(returned.Substring(specialslocation.ToArray()[i] + 2, (chararray.Length/* - 1*/) - (specialslocation.ToArray()[i] + 2)), color));//from the character after the 'Symbol' and its allocated color, to the end of the string. 
                    break;
                }
                else
                {
                    lkvpcc.Add(new KeyValuePair<string, ConsoleColor>(returned.Substring(specialslocation[i] + 2, specialslocation[i + 1]/* - 1*/ -(specialslocation[i] + 2)), color));//from the character after the 'Symbol' and its allocated color, to one character before the next 'Symbol'.
                }
            }
            //ConsoleColor cc;
            foreach(KeyValuePair<string, ConsoleColor> item in lkvpcc)
            {
                
                    Console.ForegroundColor = item.Value;
                    Console.Write(item.Key);
                
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("");
        }
    }
}
