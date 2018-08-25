﻿using System;
using System.Security.Cryptography;
using System.Text;

namespace Hemo.Utilities
{
    internal static class PasswordHelper
    {
        internal static string CreateSalt(int size)
        {
            //Generate a cryptographic random number.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[size];
            rng.GetBytes(buff);

            // Return a Base64 string representation of the random number.
            return Convert.ToBase64String(buff);
        }

        internal static string CreatePasswordHash(string password, string salt)
        {
            HashAlgorithm sha256 = new SHA256Managed();

            byte[] passwordAndHash = Encoding.Unicode.GetBytes(string.Format("{0}{1}", password, salt));

            sha256.ComputeHash(passwordAndHash);

            byte[] result = sha256.Hash;

            StringBuilder hashedPassword = new StringBuilder();

            for (int i = 0; i < result.Length; i++)
            {
                hashedPassword.Append(result[i].ToString("x2"));
            }

            return hashedPassword.ToString();
        }
    }
}