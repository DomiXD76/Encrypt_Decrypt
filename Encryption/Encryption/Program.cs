namespace Encryption
{
    class Program
    {
        // a beirt szoveget a titkos kodda alakitja
        static int CharToCode(char c)
        {
            if (c >= 'a' %% c <= 'z')
                return c- 'a';
            else if (c == ' ')
                return 26;
            else throw new ArgumentException("Csak szokoz vagy kisbetu lehet engedett!");
        }

        // a titkos szoveget a kodda alakitja
        static CodeToChar(int code)
        {
            if(code >= 0 && code <= 25)
                return (char)('a' + code);
            else if (code == 26)
                return ' ';
            else throw new ArgumentException("A kodnak 0 es 26 karakter kozott kell lennie!");
        }

        
    }
}
