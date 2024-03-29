﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace UserApi.Commons
{
    /// <summary>
    /// Crypto class
    /// </summary>
    public class Hasher
    {
        /// <summary>
        /// Returns security hash of a string
        /// </summary>
        public static string GetHash(string input)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                return sBuilder.ToString();
            }
        }

        /// <summary>
        /// Verifies the input string with the given hash
        /// </summary>
        public static bool VerifyHash(string input, string hash)
        {
            string hashOfInput = GetHash(input);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
