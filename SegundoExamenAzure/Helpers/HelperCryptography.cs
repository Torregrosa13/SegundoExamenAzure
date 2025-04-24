using System.Security.Cryptography;
using System.Text;

namespace SegundoExamenAzure.Helpers
{
    public class HelperCryptography
    {
        private static IConfiguration configuration;

        private static string salt;
        private static string key;
        private static string iterate;
        public static void Initialize(string keyVaultSalt, string keyVaultKey, string iterateVault)
        {
            salt = keyVaultSalt;
            key = keyVaultKey;
            iterate = iterateVault;
        }

        public static string EncryptString(String dato)
        {
            byte[] saltpassword = EncriptarPasswordSalt
                (key, salt, int.Parse(iterate));
            String res = EncryptString(saltpassword, dato);
            return res;
        }

        public static string DecryptString(String dato)
        {
            byte[] saltpassword = EncriptarPasswordSalt
                (key, salt, int.Parse(iterate));
            String res = DecryptString(saltpassword, dato);
            return res;
        }

        private static string EncryptString(byte[] key, string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        private static byte[] EncriptarPasswordSalt(string contenido
            , string salt, int numhash)
        {
            string textocompleto = contenido + salt;
            SHA256 objsha = SHA256.Create();
            byte[] bytesalida = null;

            try
            {
                bytesalida =
                    Encoding.UTF8.GetBytes(textocompleto);
                for (int i = 0; i < numhash; i++)
                    bytesalida = objsha.ComputeHash(bytesalida);
            }
            finally
            {
                objsha.Clear();
            }
            return bytesalida;
        }

        private static string DecryptString(byte[] key, string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }

    }
}
