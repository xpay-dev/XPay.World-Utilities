using System;

namespace CryptoManagement.ConsoleTest {
    class Program {
        static void Main(string[] args) {
            var key = "!PAYMENTKEYPASS00";
            var iv = "!P@Ym3nTK3y!@#$%";
            Console.WriteLine("\nText : ");
            var text = Console.ReadLine();
            var crypto = new CryptoProvider(key, iv);
            var encode = crypto.Encrypt(text);
            Console.WriteLine("\nEncode : " + encode);
            var decode = crypto.Encrypt(encode);
            Console.WriteLine("\nDecode : " + decode);
            Console.ReadKey();
        }
    }
}
