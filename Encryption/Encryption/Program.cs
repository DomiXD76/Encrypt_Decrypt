namespace Encryption
{
    class Program
    {
        // A beírt szöveget a titkos kóddá alakítja
        static int CharToCode(char c)
        {
            if (c >= 'a' && c <= 'z')
                return c- 'a';
            else if (c == ' ')
                return 26;
            else throw new ArgumentException("Csak szóköz vagy kisbetű lehet a szövegben!");
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
        static string Encrypt(string uzenet, string kulcs)
        {
            if (kulcs.Length != uzenet.Length)
                throw new ArgumentException("A kulcsnak olyan hosszúnak kell lenni, mint az üzenetnek");

            char[] titkositott = new char[uzenet.Length];

            for (int i = 0; i < uzenet.Length; i++)
            {
                int uzenetKod = CharToCode(uzenet[i]);
                int kulcsKod = CharToCode(kulcs[i]);
                int Titkosit = (uzenetKod + kulcsKod + 27) % 27;
                titkositott[i] = CodeToChar(Titkosit);
            }

            return new string(titkositott);
        }
        
        // Megoldja a titkosítás 
        static string Decrypt(string titkosUzenet, string kulcs)
        {
            if (kulcs.Length != titkosUzenet.Length)
                throw new ArgumentException("A kulcsnak olyan hosszúnak kell lenni, mint a titkosított üzenetnek!");

            char[] megoldas = new char[titkosUzenet.Length];

            for (int i=0; i < titkosUzenet.Length; i++)
            {
                int titkosKod = CharToCode(titkosUzenet[i]);
                int kulcsKod = CharToCode(kulcs[i]);
                int uzenetKod = (titkosKod - kulcsKod + 27) % 27;
                megoldas[i] = CodeToChar(uzenetKod);
            }
            return new string(megoldas);
        }

        
        static void Main(string[] args)
        {
            
        }
        
        
    }
}
