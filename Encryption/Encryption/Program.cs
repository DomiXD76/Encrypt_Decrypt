namespace Encryption
{
    class Program
    {
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
            if(code >= 0 && code <= 25)
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

        // A titkos és megoldott szöveg kiírása
        static void Main(string[] args)
        {
            try
            {
                // Itt adom meg, a kulcsot és a szöveget
                string szoveg = "finom almapite";
                string kulcs = "abcdefghijklmn"; // A kulcs legalább a szöveg hossza
                // HA kell, a teljes angol ABC
                // string kulcs = "abcdefghijklmnopqrstuvwxyz"; 
                string titkositottSzoveg = Encrypt(szoveg, kulcs);

                // A titkos szöveg kiírása
                Console.WriteLine("Alap szöveg: " + szoveg);
                Console.WriteLine("Kulcs: " + kulcs);
                Console.WriteLine("Titkositott szöveg: " + titkositottSzoveg);

                // Elválasztom a titkos es megoldott részeket
                Console.WriteLine("\n---------------------------------\n");

                // A megoldott szöveg kiírás
                string megoldottSzoveg = Decrypt(titkositottSzoveg, kulcs);
                Console.WriteLine("Megoldott szöveg: " + megoldottSzoveg);
            }
            catch(Exception ex)
            {
                // Ha lenne hiba.
                Console.WriteLine("Hiba: " + ex.Message);
            }

            
            Console.ReadKey();
        }
        
    }
}
