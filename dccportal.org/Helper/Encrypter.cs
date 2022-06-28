using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace dccportal.org.Helper
{
    public class Encrypter
    {
        // This constant is used to determine the keysize of the encryption algorithm in bits.
        // We divide this by 8 within the code below to get the equivalent number of bytes.
        //private const int Keysize = 256;
        // // private const int Keysize = 128;

        // // // This constant determines the number of iterations for the password bytes generation function.
        // // private const int DerivationIterations = 1000;

        // // public static string Encrypt(string plainText, string passPhrase)
        // // {
        // //     // Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
        // //     // so that the same Salt and IV values can be used when decrypting.  
        // //     //var saltStringBytes = Generate256BitsOfRandomEntropy();
        // //     //var ivStringBytes = Generate256BitsOfRandomEntropy();
        // //     var saltStringBytes = Generate128BitsOfRandomEntropy();
        // //     var ivStringBytes = Generate128BitsOfRandomEntropy();
        // //     var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
        // //     using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
        // //     {
        // //         var keyBytes = password.GetBytes(Keysize / 8);
        // //         using (var symmetricKey = new RijndaelManaged())
        // //         {
        // //             symmetricKey.BlockSize = 128;
        // //             symmetricKey.Mode = CipherMode.CBC;
        // //             symmetricKey.Padding = PaddingMode.PKCS7;
        // //             using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
        // //             {
        // //                 using (var memoryStream = new MemoryStream())
        // //                 {
        // //                     using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
        // //                     {
        // //                         cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
        // //                         cryptoStream.FlushFinalBlock();
        // //                         // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
        // //                         var cipherTextBytes = saltStringBytes;
        // //                         cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
        // //                         cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
        // //                         memoryStream.Close();
        // //                         cryptoStream.Close();
        // //                         return Convert.ToBase64String(cipherTextBytes);
        // //                     }
        // //                 }
        // //             }
        // //         }
        // //     }
        // // }

        // // public static string Decrypt(string cipherText, string passPhrase)
        // // {
        // //     try
        // //     {
        // //         var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
        // //         // Get the saltbytes by extracting the first 32 bytes from the supplied cipherText bytes.
        // //         var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(Keysize / 8).ToArray();
        // //         // Get the IV bytes by extracting the next 32 bytes from the supplied cipherText bytes.
        // //         var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(Keysize / 8).Take(Keysize / 8).ToArray();
        // //         // Get the actual cipher text bytes by removing the first 64 bytes from the cipherText string.
        // //         var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((Keysize / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((Keysize / 8) * 2)).ToArray();

        // //         using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
        // //         {
        // //             var keyBytes = password.GetBytes(Keysize / 8);
        // //             using (var symmetricKey = new RijndaelManaged())
        // //             {
        // //                 symmetricKey.BlockSize = 128;
        // //                 symmetricKey.Mode = CipherMode.CBC;
        // //                 symmetricKey.Padding = PaddingMode.PKCS7;
        // //                 using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
        // //                 {
        // //                     using (var memoryStream = new MemoryStream(cipherTextBytes))
        // //                     {
        // //                         using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
        // //                         {
        // //                             var plainTextBytes = new byte[cipherTextBytes.Length];
        // //                             var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
        // //                             memoryStream.Close();
        // //                             cryptoStream.Close();
        // //                             return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        // //                         }
        // //                     }
        // //                 }
        // //             }
        // //         }
        // //     }
        // //     catch (Exception)
        // //     {
        // //         throw;
        // //     }
        // //     // Get the complete stream of bytes that represent:
        // //     // [32 bytes of Salt] + [32 bytes of IV] + [n bytes of CipherText]
            
        // // }

        // // private static byte[] Generate256BitsOfRandomEntropy()
        // // {
        // //     var randomBytes = new byte[32]; // 32 Bytes will give us 256 bits.
        // //     using (var rngCsp = new RNGCryptoServiceProvider())
        // //     {
        // //         // Fill the array with cryptographically secure random bytes.
        // //         rngCsp.GetBytes(randomBytes);
        // //     }
        // //     return randomBytes;
        // // }

        // // private static byte[] Generate128BitsOfRandomEntropy()
        // // {
        // //     var randomBytes = new byte[16]; // 32 Bytes will give us 256 bits.
        // //     using (var rngCsp = new RNGCryptoServiceProvider())
        // //     {
        // //         // Fill the array with cryptographically secure random bytes.
        // //         rngCsp.GetBytes(randomBytes);
        // //     }
        // //     return randomBytes;
        // // }

        // // public static string RandomizeStringData(string data, string acctNumber)
        // // {
        // //     data = data + acctNumber;
        // //     Random num = new Random();
        // //     string rand = new string(data.ToCharArray().
        // //     OrderBy(s => (num.Next(2) % 2) == 0).ToArray());
        // //     return rand;
        // // }

        // // public static string GenerateString(int length, string acctNo)
        // {
        //     var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz0123456789";
        //     var stringChars = new char[length];
        //     var random = new Random();

        //     for (int i = 0; i < stringChars.Length; i++)
        //     {
        //         stringChars[i] = chars[random.Next(chars.Length)];
        //     }

        //     return RandomizeStringData(new String(stringChars), acctNo);
        // }
    
        #region aes

        public static string Encrypt(string text, string keyString)
        {
            var key = Encoding.UTF8.GetBytes(keyString);

            using (var aesAlg = Aes.Create())
            {
                using (var encryptor = aesAlg.CreateEncryptor(key, aesAlg.IV))
                {
                    using (var msEncrypt = new MemoryStream())
                    {
                        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(text);
                        }

                        var iv = aesAlg.IV;

                        var decryptedContent = msEncrypt.ToArray();

                        var result = new byte[iv.Length + decryptedContent.Length];

                        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                        Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);

                        return Convert.ToBase64String(result);
                    }
                }
            }
        }

        public static string Decrypt(string cipherText, string keyString)
        {
            string converted = cipherText.Replace(' ', '+');
            converted = converted.Replace('_', '/');
            var fullCipher = Convert.FromBase64String(converted);

            var iv = new byte[16];
            var cipher = new byte[16];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, iv.Length);
            var key = Encoding.UTF8.GetBytes(keyString);

            using (var aesAlg = Aes.Create())
            {
                using (var decryptor = aesAlg.CreateDecryptor(key, iv))
                {
                    string result;
                    using (var msDecrypt = new MemoryStream(cipher))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                result = srDecrypt.ReadToEnd();
                            }
                        }
                    }

                    return result;
                }
            }
        }
        #endregion  
    }
}