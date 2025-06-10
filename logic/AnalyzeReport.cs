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
            for (int i = 0; i < spliited.Count() - 1; i++)
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

            for (int i = 1; i < word.Length; i++)
            {
                if (((word[i] == '-') || (word[i] == ' ')) && i < word.Length - 1) continue;
                if (!Char.IsLetter(word[i])) return false;
            }
            return true;
        }

        private static List<string> GetNames(string report)
        {
            string[] splitted = report.Split(' ');
            List<string> Names = new List<string>();

            int numNames = 0;
            bool continuingName = false;
            bool newName = true;

            int splittedLength = splitted.Count();
            for (int i = 0; i < (splittedLength - 1); i++)
            {
                if (CanBeName(splitted[i]) && CanBeName(splitted[i + 1]))
                {

                    if (newName)
                    {
                        Names.Add(splitted[i] + " ");
                        numNames += 1;
                        newName = false;
                    }
                    else
                    {
                        Names[numNames - 1] += splitted[i] + " ";
                    }
                    if (splittedLength - 2 == i) Names[numNames - 1] += splitted[i + 1];

                    continuingName = true;
                }
                else if (CanBeName(splitted[i]) && !CanBeName(splitted[i + 1]))
                {
                    if (continuingName)
                    {
                        string name = splitted[i];
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
            List<string[]> fullNamesSeperated = new List<string[]>();

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
    }
}












       



       