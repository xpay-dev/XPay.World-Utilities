using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CryptoManagement {
     [Serializable]
    public class CryptoProvider {
        private static readonly string DEFAULT_HASH_ALGORITHM = "SHA512";
        private static readonly int DEFAULT_KEY_SIZE = 256;
        private static readonly int MAX_ALLOWED_SALT_LEN = byte.MaxValue;
        private static readonly int MIN_ALLOWED_SALT_LEN = 4;
        private static readonly int DEFAULT_MIN_SALT_LEN = MIN_ALLOWED_SALT_LEN;
        private static readonly int DEFAULT_MAX_SALT_LEN = 8;
        private readonly int minSaltLen = -1;
        private readonly int maxSaltLen = -1;
        private readonly ICryptoTransform encryptor = null;
        private readonly ICryptoTransform decryptor = null;
        public CryptoProvider(string passPhrase) : this(passPhrase, null) {
        }
        public CryptoProvider(string passPhrase, string initVector) : this(passPhrase, initVector, -1) {
        }
        public CryptoProvider(string passPhrase, string initVector, int minSaltLen) : this(passPhrase, initVector, minSaltLen, -1) {
        }
        public CryptoProvider(string passPhrase, string initVector, int minSaltLen, int maxSaltLen) : this(passPhrase, initVector, minSaltLen, maxSaltLen, -1) {
        }
        public CryptoProvider(string passPhrase, string initVector, int minSaltLen, int maxSaltLen, int keySize) : this(passPhrase, initVector, minSaltLen, maxSaltLen, keySize, (string)null) {
        }
        public CryptoProvider(string passPhrase, string initVector, int minSaltLen, int maxSaltLen, int keySize, string hashAlgorithm) : this(passPhrase, initVector, minSaltLen, maxSaltLen, keySize, hashAlgorithm, (string)null) {
        }
        public CryptoProvider(string passPhrase, string initVector, int minSaltLen, int maxSaltLen, int keySize, string hashAlgorithm, string saltValue) : this(passPhrase, initVector, minSaltLen, maxSaltLen, keySize, hashAlgorithm, saltValue, 3) {
        }
        public CryptoProvider(string passPhrase, string initVector, int minSaltLen, int maxSaltLen, int keySize, string hashAlgorithm, string saltValue, int passwordIterations) {
            this.minSaltLen = minSaltLen >= MIN_ALLOWED_SALT_LEN ? minSaltLen : DEFAULT_MIN_SALT_LEN;
            this.maxSaltLen = maxSaltLen >= 0 && maxSaltLen <= MAX_ALLOWED_SALT_LEN ? maxSaltLen : DEFAULT_MAX_SALT_LEN;
            if (keySize <= 0) { keySize = DEFAULT_KEY_SIZE; }
            hashAlgorithm = hashAlgorithm != null ? hashAlgorithm.ToUpper().Replace("-", "") : DEFAULT_HASH_ALGORITHM;
            byte[] rgbIV = initVector != null ? Encoding.ASCII.GetBytes(initVector) : new byte[0];
            byte[] rgbSalt = saltValue != null ? Encoding.ASCII.GetBytes(saltValue) : new byte[0];
            byte[] bytes = new PasswordDeriveBytes(passPhrase, rgbSalt, hashAlgorithm, passwordIterations).GetBytes(keySize / 8);
            RijndaelManaged rijndaelManaged = new RijndaelManaged();
            if (rgbIV.Length == 0) { rijndaelManaged.Mode = CipherMode.ECB;
            } else { rijndaelManaged.Mode = CipherMode.CBC; }
            encryptor = rijndaelManaged.CreateEncryptor(bytes, rgbIV);
            decryptor = rijndaelManaged.CreateDecryptor(bytes, rgbIV);
        }
        public string Encrypt(string plainText) { return Encrypt(Encoding.UTF8.GetBytes(plainText)); }
        internal string Encrypt(byte[] plainTextBytes) { return Convert.ToBase64String(EncryptToBytes(plainTextBytes)); }
        internal byte[] EncryptToBytes(string plainText) { return EncryptToBytes(Encoding.UTF8.GetBytes(plainText)); }
        internal byte[] EncryptToBytes(byte[] plainTextBytes) {
            byte[] buffer = AddSalt(plainTextBytes);
            MemoryStream memoryStream = new MemoryStream();
            lock (this) {
                CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
                cryptoStream.Write(buffer, 0, buffer.Length);
                cryptoStream.FlushFinalBlock();
                byte[] array = memoryStream.ToArray();
                memoryStream.Close();
                cryptoStream.Close();
                return array;
            }
        }
        public string Decrypt(string cipherText) { return Decrypt(Convert.FromBase64String(cipherText)); }
        internal string Decrypt(byte[] cipherTextBytes) { return Encoding.UTF8.GetString(DecryptToBytes(cipherTextBytes)); }
        internal byte[] DecryptToBytes(string cipherText) { return DecryptToBytes(Convert.FromBase64String(cipherText)); }
        internal byte[] DecryptToBytes(byte[] cipherTextBytes) {
            int num = 0;
            int sourceIndex = 0;
            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
            byte[] buffer = new byte[cipherTextBytes.Length];
            lock (this) {
                CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
                num = cryptoStream.Read(buffer, 0, buffer.Length);
                memoryStream.Close();
                cryptoStream.Close();
            }
            if (maxSaltLen > 0 && maxSaltLen >= minSaltLen) { sourceIndex = buffer[0] & 3 | buffer[1] & 12 | buffer[2] & 48 | buffer[3] & 192; }
            byte[] numArray = new byte[num - sourceIndex];
            Array.Copy(buffer, sourceIndex, numArray, 0, num - sourceIndex);
            return numArray;
        }
        private byte[] AddSalt(byte[] plainTextBytes) {
            if (maxSaltLen == 0 || maxSaltLen < minSaltLen) { return plainTextBytes; }
            byte[] salt = GenerateSalt();
            byte[] numArray = new byte[plainTextBytes.Length + salt.Length];
            Array.Copy(salt, numArray, salt.Length);
            Array.Copy(plainTextBytes, 0, numArray, salt.Length, plainTextBytes.Length);
            return numArray;
        }
        private byte[] GenerateSalt() {
            int length = minSaltLen != maxSaltLen ? GenerateRandomNumber(minSaltLen, maxSaltLen) : minSaltLen;
            byte[] data = new byte[length];
            new RNGCryptoServiceProvider().GetNonZeroBytes(data);
            data[0] = (byte)(data[0] & 252 | length & 3);
            data[1] = (byte)(data[1] & 243 | length & 12);
            data[2] = (byte)(data[2] & 207 | length & 48);
            data[3] = (byte)(data[3] & 63 | length & 192);
            return data;
        }
        private int GenerateRandomNumber(int minValue, int maxValue) {
            byte[] data = new byte[4];
            new RNGCryptoServiceProvider().GetBytes(data);
            return new Random((data[0] & sbyte.MaxValue) << 24 | data[1] << 16 | data[2] << 8 | data[3]).Next(minValue, maxValue + 1);
        }
    }
}
