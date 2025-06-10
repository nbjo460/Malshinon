using Malshinon.DALFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon
{
    internal static class AnalyzeReport
    {
        public static string[] GetName(string report)
        {
            report = report.Trim();
            string[] fullName = new string[2];
            string[] spliited = report.Split(' ');
            for (int i=0; i< spliited.Count() -1; i++)
            {
                if (CanBeName(spliited[i]) && CanBeName(spliited[i + 1]))
                {
                    fullName[0] = spliited[i];
                    fullName[1] = spliited[i + 1];
                    return fullName;
                }
            }
            return null;
        }

        public static bool CanBeName(string word)
        {
            if (word.Length < 2) return false;
            if (Char.IsLower(word[0])) return false;

            for (int i = 1; i <word.Length;i++)
            {
                if (((word[i] == '-') || (word[i] == ' ')) && i < word.Length -1) continue;
                if (!Char.IsLetter(word[i])) return false;
            }
            return true;
        }

        private static List<string> GetNames(string report)
        {
            string[] splitted = report.Split(' ');
            List<string> Names = new List<string>();

            // case: first is 1 and last is 1.
            // case: first is more than 1, and last is 1.

            // case: few names. each first is more than 1

            //case: 

            int numNames = 0;
            bool continuingName = false;
            bool newName = true;

            int splittedLength = splitted.Count();
            for (int i = 0; i < (splittedLength-1); i++)
            {
                if (CanBeName(splitted[i]) && CanBeName(splitted[i + 1]))
                {

                    if (newName)
                    {
                        //Console.WriteLine("@@@@@@@@@@@" + splitted[i]);
                        Names.Add(splitted[i] + " ");
                        numNames += 1;
                        newName = false;
                    }
                    else
                    {                        
                        //Console.WriteLine("##########" + splitted[i]);
                        Names[numNames - 1] += splitted[i] + " ";
                    }
                    if (splittedLength - 2 == i) Names[numNames - 1] += splitted[i+1];

                    continuingName = true;
                }
                else if (CanBeName(splitted[i]) && !CanBeName(splitted[i + 1]))
                {
                    if (continuingName)
                    {
                        string name = splitted[i];
                        //Console.WriteLine("$$$$$$$$$" + splitted[i]+Names.Count());
                        Names[numNames - 1] += name;
                        continuingName = false;
                        newName = true;
                        
                    }
                }
            }
            
            return Names;
        }

        public static List<string[]> GetFullNames(string report)
        {
            report = report.Trim();
            List<string> Names = GetNames(report);
            List<string[]> fullNamesSeperated= new List<string[]> ();

            foreach (string fullName in Names)
            {
                string[] seperatedName = fullName.Split(' ');
                string lastName = seperatedName[seperatedName.Count() - 1];
                string firstName = "";

                for (int i = 0; i < seperatedName.Count() - 1; i++)
                {
                    firstName += seperatedName[i] + " ";
                }
                firstName.Trim();

                fullNamesSeperated.Add(new string[] { firstName, lastName });
            }

            return fullNamesSeperated;

        }














        private static List<string[]> FindNames(string report)
        {
            string[] splitted = report.Split(' ');
            List<List<string>> fullNames = new List<List<string>>() { };

            int countName = 0;

            for (int i = 0; i < (splitted.Length - 1); i++)
            {
                Console.WriteLine(splitted[i] + i);


                if (Char.IsUpper(splitted[i][0]) && Char.IsUpper(splitted[i + 1][0]))
                {
                    if (fullNames.Count - 1 < countName)
                    {
                        //Console.WriteLine(fullNames.Count+"IF");
                        fullNames.Add(new List<string> { splitted[i] });
                    }
                    else
                    {
                        //Console.WriteLine(fullNames.Count+"ELSE");

                        fullNames[countName].Add(splitted[i]);
                        //fullNames.Add(new List<string> { splitted[i] });

                    }
                }
                else if (Char.IsUpper(splitted[i][0]) && !Char.IsUpper(splitted[i + 1][0]) && countName < fullNames.Count - 1)
                {

                    fullNames[countName].Add(splitted[i]);
                    countName += 1;
                }
            }
            return GetFullNamesByJoin(fullNames);

        }

        //private static List<string[]> FindNames(string report)
        //{
        //    string[] splitted = report.Split(' ');
        //    List<string> fullNames = new List<string>();

        //    int countName = 0;

        //    for (int i = 0; i < splitted.Length - 1; i++)
        //    {

        //        if (Char.IsUpper(splitted[i][0]) && Char.IsUpper(splitted[i + 1][0]))
        //        {
        //            //if (fullNames.Count - 1 == countName)
        //            //{
        //            //    fullNames.Add(new List<string> { splitted[i] });

        //            //}
        //            //else
        //            //{
        //            //    fullNames[countName].Add(splitted[i]);
        //            //    //fullNames.Add(new List<string> { splitted[i] });

        //            //}

        //            if (countName == fullNames.Count - 2)
        //            {
        //                fullNames.Add(" " + splitted[i]);
        //            }
        //            else if()
        //        }
        //        else if (countName == fullNames.Count - 1 && Char.IsUpper(splitted[i][0]) && !Char.IsUpper(splitted[i + 1][0]))
        //        {
        //            fullNames[countName].Add(splitted[i]);
        //            countName += 1;
        //        }
        //    }
        //    return GetFullNamesByJoin(fullNames);

        //}


        private static List<string[]> GetFullNamesByJoin(List<List<string>> _fullNames)
        {
            Console.WriteLine(_fullNames.Count);

            List<string[]> fullNames = new List<string[]>();
            foreach (List<string> fullNameSeperated in _fullNames)
            {
                string[] fullName = new string[2] { "", "" };
                for (int i = 0; i < fullNameSeperated.Count; i++)
                {
                    if (i == fullNameSeperated.Count - 1)
                    {
                        fullName[1] = " " + fullNameSeperated[i];
                    }
                    else
                    {
                        fullName[0] += " " + fullNameSeperated[i];
                    }
                }
            }
            return fullNames;
        }
    }
}
