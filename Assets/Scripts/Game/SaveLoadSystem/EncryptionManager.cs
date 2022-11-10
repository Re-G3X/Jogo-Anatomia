﻿using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Game.SaveLoadSystem
{
    public class EncryptionManager
    {
        public static string EncryptString(string key, string plainText)  
        {  
            var iv = new byte[16];  
            byte[] array;  
  
            using (var aes = Aes.Create())  
            {  
                aes.Key = Encoding.UTF8.GetBytes(key);  
                aes.IV = iv;  
  
                var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);  
  
                using (var memoryStream = new MemoryStream())  
                {  
                    using (var cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))  
                    {  
                        using (var streamWriter = new StreamWriter((Stream)cryptoStream))  
                        {  
                            streamWriter.Write(plainText);  
                        }  
  
                        array = memoryStream.ToArray();  
                    }  
                }  
            }  
  
            return Convert.ToBase64String(array);  
        }  
  
        public static string DecryptString(string key, string cipherText)  
        {  
            var iv = new byte[16];  
            var buffer = Convert.FromBase64String(cipherText);  
  
            using (var aes = Aes.Create())  
            {  
                aes.Key = Encoding.UTF8.GetBytes(key);  
                aes.IV = iv;  
                var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);  
  
                using (var memoryStream = new MemoryStream(buffer))  
                {  
                    using (var cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))  
                    {  
                        using (var streamReader = new StreamReader((Stream)cryptoStream))  
                        {  
                            return streamReader.ReadToEnd();  
                        }  
                    }  
                }  
            }  
        }  
        
        public static string EncryptDecrypt(string text, int key)  
        {  
            var szInputStringBuild = new StringBuilder(text);  
            var szOutStringBuild = new StringBuilder(text.Length);  
            char Textch;  
            for (var iCount = 0; iCount < text.Length; iCount++)  
            {  
                Textch = szInputStringBuild[iCount];  
                Textch = (char)(Textch ^ key);  
                szOutStringBuild.Append(Textch);  
            }  
            return szOutStringBuild.ToString();  
        }  
    }
}