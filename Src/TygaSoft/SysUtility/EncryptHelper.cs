using System;
using System.Security.Cryptography;
using System.Text;
using TygaSoft.Model;
using TygaSoft.SysException;

namespace TygaSoft.SysUtility
{
    public class EncryptHelper
    {
        public const PasswordFormatOptions DefaultPasswordFormat = PasswordFormatOptions.Clear;

        public static string EncodePassword(string pass, PasswordFormatOptions passwordFormat, string salt)
        {
            if (passwordFormat == PasswordFormatOptions.Clear) return pass;

            if (passwordFormat == PasswordFormatOptions.Aes)
            {
                var aes = new AESEncrypt();
                return aes.Encrypt(pass, salt);
            }

            var hm = GetHashAlgorithm(passwordFormat);
            //Hashed：不可逆，不能解密
            if (passwordFormat == PasswordFormatOptions.Hashed)
            {
                byte[] bIn = Encoding.Unicode.GetBytes(pass);
                byte[] bSalt = Convert.FromBase64String(salt);
                byte[] bRet = null;
                if (hm is KeyedHashAlgorithm)
                {
                    KeyedHashAlgorithm kha = (KeyedHashAlgorithm)hm;
                    if (kha.Key.Length == bSalt.Length)
                    {
                        kha.Key = bSalt;
                    }
                    else if (kha.Key.Length < bSalt.Length)
                    {
                        byte[] bKey = new byte[kha.Key.Length];
                        Buffer.BlockCopy(bSalt, 0, bKey, 0, bKey.Length);
                        kha.Key = bKey;
                    }
                    else
                    {
                        byte[] bKey = new byte[kha.Key.Length];
                        for (int iter = 0; iter < bKey.Length;)
                        {
                            int len = Math.Min(bSalt.Length, bKey.Length - iter);
                            Buffer.BlockCopy(bSalt, 0, bKey, iter, len);
                            iter += len;
                        }
                        kha.Key = bKey;
                    }
                    bRet = kha.ComputeHash(bIn);
                }
                else
                {
                    byte[] bAll = new byte[bSalt.Length + bIn.Length];
                    Buffer.BlockCopy(bSalt, 0, bAll, 0, bSalt.Length);
                    Buffer.BlockCopy(bIn, 0, bAll, bSalt.Length, bIn.Length);
                    bRet = hm.ComputeHash(bAll);
                }

                return Convert.ToBase64String(bRet);
            }
            else
            {
                return Convert.ToBase64String(hm.ComputeHash(Encoding.UTF8.GetBytes(pass)));
            }
        }

        public static string UnEncodePassword(string pass, PasswordFormatOptions passwordFormat, string salt)
        {
            switch (passwordFormat)
            {
                case PasswordFormatOptions.Clear: // Clear:
                    return pass;
                case PasswordFormatOptions.Hashed: //Hashed:
                    throw new CustomException(SR.M_CanNotDecodeHashedPassword);
                default:
                    var aes = new AESEncrypt();
                    return aes.Decrypt(pass, salt);
            }
        }

        /// <summary>
        /// 生成随机字节值的强密码序列。
        /// </summary>
        /// <returns></returns>
        public static string GenerateSalt()
        {
            byte[] buf = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(buf);
            }
            return Convert.ToBase64String(buf);
        }

        /// <summary>
        /// MD5加密，并转换为16进制字符串
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public string Md5Encode(string inputString)
        {
            var hm = GetHashAlgorithm(PasswordFormatOptions.Md5);
            return ConvertByteToString(hm.ComputeHash(Encoding.UTF8.GetBytes(inputString)));
        }

        /// <summary>
        /// 将字节数组转换为16进制字符串
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static string ConvertByteToString(byte[] inputData)
        {
            var sb = new StringBuilder(inputData.Length * 2);
            foreach (byte b in inputData)
            {
                sb.AppendFormat("{0:X2}", b);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 将16进制字符串转换为字节数组
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static byte[] ConvertStringToByte(string inputString)
        {
            if (inputString == null || inputString.Length < 2) return null;

            int l = inputString.Length / 2;
            byte[] result = new byte[l];
            for (int i = 0; i < l; ++i)
            {
                result[i] = Convert.ToByte(inputString.Substring(2 * i, 2), 16);
            }

            return result;
        }

        /// <summary>
        /// 获取加密哈希算法实现
        /// </summary>
        /// <param name="passwordFormat"></param>
        /// <returns></returns>
        private static HashAlgorithm GetHashAlgorithm(PasswordFormatOptions passwordFormat)
        {
            HashAlgorithm hashAlgo = null;
            switch (passwordFormat)
            {
                case PasswordFormatOptions.Sha1:
                    hashAlgo = SHA1.Create();
                    break;
                case PasswordFormatOptions.Sha256:
                    hashAlgo = SHA256.Create();
                    break;
                case PasswordFormatOptions.Md5:
                    hashAlgo = MD5.Create();
                    break;
                default:
                    hashAlgo = SHA256.Create();
                    break;
            }

            return hashAlgo;
        }
    }
}
