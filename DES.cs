using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DES_File_Protector {
    class DESalgorithm {
        public static string GenerateRandomKey(int key_size) {
            byte[] key = new byte[key_size];
            using (var rng = RandomNumberGenerator.Create()) {
                rng.GetBytes(key);
            }
            return Convert.ToBase64String(key);
        }
        // خوارزمية التشفير
        static public void EncryptFile(string filePath, string outputFile, string key) {
            if(key.Length != 16) 
                throw new Exception("Please select a 16 character password");
            byte[] keyBytes = new byte[8];
            for (int i = 0; i < 16; i+=2) 
                keyBytes[i / 2] = (byte)(((int)key[i] + (int)key[i + 1]) / 2);
            //byte[] keyBytes = Encoding.UTF8.GetBytes(key);

            // Create a random IV of 8 bytes
            byte[] ivBytes = new byte[8];
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            rng.GetBytes(ivBytes);

            // Create a DES instance and set the key and the IV
            DES des = DES.Create();
            des.Key = keyBytes;
            des.IV = ivBytes;

            // Create a file stream to read the file
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            // Create a memory stream to store the encrypted file
            MemoryStream memoryStream = new MemoryStream();

            // Write the IV to the memory stream
            memoryStream.Write(ivBytes, 0, ivBytes.Length);

            // Create a crypto stream to perform the encryption
            CryptoStream cryptoStream = new CryptoStream(memoryStream, des.CreateEncryptor(), CryptoStreamMode.Write);

            // Read the file and write the encrypted data to the memory stream
            byte[] buffer = new byte[4096];
            int bytesRead;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0) {
                cryptoStream.Write(buffer, 0, bytesRead);
            }

            // Close the streams
            cryptoStream.Close();
            fileStream.Close();

            // Get the encrypted file bytes from the memory stream
            byte[] encryptedFileBytes = memoryStream.ToArray();

            // Close the memory stream
            memoryStream.Close();

            // Write the encrypted file bytes to the new file
            File.WriteAllBytes(outputFile, encryptedFileBytes);

        }
        // خوارزمية فك التشفير
        public static void DecryptFile(string filePath, string outputFile, string key) {
            if (key.Length != 16)
                throw new Exception("Please select a 16 character password");
            byte[] keyBytes = new byte[8];
            for (int i = 0; i < 16; i += 2) 
                keyBytes[i / 2] = (byte)(((int)key[i] + (int)key[i + 1]) / 2);


            //byte[] keyBytes = Encoding.UTF8.GetBytes(key);

            // Create a file stream to read the encrypted file
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            // Read the IV from the first 8 bytes of the file
            byte[] ivBytes = new byte[8];
            fileStream.Read(ivBytes, 0, ivBytes.Length);

            // Create a DES instance and set the key and the IV
            DES des = DES.Create();
            des.Key = keyBytes;
            des.IV = ivBytes;

            // Create a crypto stream to perform the decryption
            CryptoStream cryptoStream = new CryptoStream(fileStream, des.CreateDecryptor(), CryptoStreamMode.Read);

            // Create a memory stream to store the decrypted file
            MemoryStream memoryStream = new MemoryStream();

            // Read the encrypted file and write the decrypted data to the memory stream
            byte[] buffer = new byte[4096];
            int bytesRead;
            while ((bytesRead = cryptoStream.Read(buffer, 0, buffer.Length)) > 0) {
                memoryStream.Write(buffer, 0, bytesRead);
            }

            // Close the streams
            cryptoStream.Close();
            fileStream.Close();

            // Get the decrypted file bytes from the memory stream
            byte[] decryptedFileBytes = memoryStream.ToArray();

            // Close the memory stream
            memoryStream.Close();

            // Write the decrypted file bytes to the new file
            File.WriteAllBytes(outputFile, decryptedFileBytes);

        }
    }
}
