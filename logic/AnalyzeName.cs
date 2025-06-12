using Malshinon.DALFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon
{
    internal static class AnalyzeName
    {

        public static bool IsValidateName(string word)
        {
            if (word.Length < 2 || word.Length > 20) return false;
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
                if (IsValidateName(splitted[i]) && IsValidateName(splitted[i + 1]))
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
                else if (IsValidateName(splitted[i]) && !IsValidateName(splitted[i + 1]))
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
                lastName.Trim();
                if(firstName.Length > 19) firstName = firstName.Substring(0, 19);
                if(lastName.Length > 19) lastName = lastName.Substring(0, 19);
                fullNamesSeperated.Add(new string[] { firstName, lastName });
            }

            return fullNamesSeperated;

        }
    }
}












       



       