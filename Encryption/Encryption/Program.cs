using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Encryption
{
    class Program
    {
        // 1. feladat kezdete

        // A beírt szöveget a titkos kóddá alakítja
        static int CharToCode(char c)
        {
            if (c >= 'a' && c <= 'z')
                return c - 'a';
            else if (c == ' ')
                return 26;
            else
                throw new ArgumentException("Csak szóköz vagy kisbetű lehet a szövegben!");
        }

        // A titkos szöveget a titkos kóddá alakítja 
        static char CodeToChar(int code)
        {
            if (code >= 0 && code <= 25)
                return (char)('a' + code);
            else if (code == 26)
                return ' ';
            else throw new ArgumentException("A kódnak 0 es 26 karakter között kell lennie!");
        }

        // Titkosítás 
        static string Encrypt(string szoveg, string kulcs)
        {
            if (kulcs.Length < szoveg.Length)
                throw new ArgumentException("A kulcsnak legalább  olyan hosszúnak kell lenni, mint az üzenetnek");

            char[] titkositott = new char[szoveg.Length];

            for (int i = 0; i < szoveg.Length; i++)
            {
                int szovegKod = CharToCode(szoveg[i]);
                int kulcsKod = CharToCode(kulcs[i]);
                int Titkosit = (szovegKod + kulcsKod + 27) % 27;
                titkositott[i] = CodeToChar(Titkosit);
            }

            return new string(titkositott);
        }

        // Megoldja a titkosítás 
        static string Decrypt(string titkosSzoveg, string kulcs)
        {
            if (kulcs.Length < titkosSzoveg.Length)
                throw new ArgumentException("A kulcsnak legalább  olyan hosszúnak kell lenni, mint a titkosított üzenetnek!");

            char[] megoldas = new char[titkosSzoveg.Length];

            for (int i = 0; i < titkosSzoveg.Length; i++)
            {
                int titkosKod = CharToCode(titkosSzoveg[i]);
                int kulcsKod = CharToCode(kulcs[i]);
                int szovegKod = (titkosKod - kulcsKod + 27) % 27;
                megoldas[i] = CodeToChar(szovegKod);
            }
            return new string(megoldas);
        }

        // 2. feladat kezdete

        // Szótár betöltése (a Program.cs mellett kell lennie)
        static List<string> LoadDictionary(string filePath)
        {
            return File.ReadAllLines(filePath)
                .Select(w => w.Trim())
                .Where(w => !string.IsNullOrEmpty(w))
                .ToList();
        }

        // Kulcsrészletet ismert text alapján
        static string GetKeyFragment(string ciphertext, string knownPlaintext)
        {
            char[] keyFrag = new char[knownPlaintext.Length];
            for (int i = 0; i < knownPlaintext.Length; i++)
            {
                int ct = CharToCode(ciphertext[i]);
                int pt = CharToCode(knownPlaintext[i]);
                int k = (ct - pt + 27) % 27;
                keyFrag[i] = CodeToChar(k);
            }
            return new string(keyFrag);
        }

        // Megkeresi azokat a kulcsrészleteket, amelyek megfelelnek a két titkosított üzenet alapján ismert kulcsrészletnek
        static List<string> FindKeyCandidates(string titkos1, string titkos2, string ismertPrefix)
        {
            // kulcsrészlet
            string kulcsDarab = GetKeyFragment(titkos2, ismertPrefix);

            // Alkalmazzuk a kulcsrészletet az elejére, így részben meglesz az eleje
            string reszbenMegoldott = Decrypt(titkos1.Substring(0, ismertPrefix.Length), kulcsDarab);

            // Szótár betöltése ("words.txt")
            List<string> wordList = LoadDictionary("words.txt");

            // "reszbenMegoldott" szöveggel kezdődő szavak keresése
            List<string> candidates = wordList.Where(w => w.StartsWith(reszbenMegoldott, StringComparison.InvariantCultureIgnoreCase)).ToList();
            List<string> kulcsCandidates = new List<string>();

            foreach (var candidate in candidates)
            {
                // Csak azok, amelyek hossza legalább akkora, mint a kulcsrészlet
                if (candidate.Length < kulcsDarab.Length)
                    continue;

                char[] candidateKulcs = new char[candidate.Length];
                bool valid = true;
                for (int i = 0; i < candidate.Length; i++)
                {
                    int ct = CharToCode(titkos1[i]);
                    int pt = CharToCode(candidate[i]);
                    int k = (ct - pt + 27) % 27;
                    char kulcsChar = CodeToChar(k);
                    // Az ismert kulcsrészlettel megegyezik-e
                    if (i < kulcsDarab.Length && kulcsDarab[i] != kulcsChar)
                    {
                        valid = false;
                        break;
                    }
                    candidateKulcs[i] = kulcsChar;
                }
                if (valid)
                    kulcsCandidates.Add(new string(candidateKulcs));
            }
            return kulcsCandidates;
        }


        // A kiírások 
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("1. feladat\n");
                // Itt adom meg, a kulcsot és a szöveget
                string szoveg = "finom almapite";
                string kulcs = "abcdefghijklmn"; // A kulcs legalább a szöveg hossza
                // string kulcs = "abcdefghijklmnopqrstuvwxyz"; HA kell, a teljes angol ABC
                string titkositottSzoveg = Encrypt(szoveg, kulcs);
                Console.WriteLine("Alap szöveg: " + szoveg);
                Console.WriteLine("Kulcs: " + kulcs);
                Console.WriteLine("Titkositott szöveg: " + titkositottSzoveg);


                // A megoldott szöveg
                string megoldottSzoveg = Decrypt(titkositottSzoveg, kulcs);
                Console.WriteLine("\nMegoldott szöveg: " + megoldottSzoveg);

                // Elválasztom az feladatokat
                Console.WriteLine("\n---------------------------------\n");

                Console.WriteLine("2. feladat\n");
                // Két üzenet, azonos kulccsal titkosítva, ismert prefix ("early ")
                // Eredeti üzenetek:
                //   szoveg1: "curiosity killed the cat"
                //   szoveg2: "early bird catches the worm"
                // Előre definiált a kulcs, hosszabb mint az üzenetek
                string szoveg1 = "curiosity killed the cat";
                string szoveg2 = "early bird catches the worm";
                string foKulcs = "nagyontitkoskulcsnagyontitkoskulcs";

                // Titkosítás
                string titkos1 = Encrypt(szoveg1, foKulcs);
                string titkos2 = Encrypt(szoveg2, foKulcs);

                Console.WriteLine("Eredeti üzenet 1: " + szoveg1);
                Console.WriteLine("Titkosított üzenet 1: " + titkos1);
                Console.WriteLine();
                Console.WriteLine("Eredeti üzenet 2: " + szoveg2);
                Console.WriteLine("Titkosított üzenet 2: " + titkos2);

                // Szoveg2 "early " prefixével ismert
                string ismertPrefix = "early ";
                List<string> lehetoKulcsok = FindKeyCandidates(titkos1, titkos2, ismertPrefix);

                Console.Write("\nLehetséges kulcsrészlet(ek) (ismert prefix alapján): ");
                foreach (var candidate in lehetoKulcsok)
                {
                    Console.WriteLine(candidate);
                }
            }
            catch (Exception ex)
            {
                // Ha lenne hiba
                Console.WriteLine("Hiba: " + ex.Message);
            }

            Console.ReadKey();
        }

    }
}
