using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WFS.business.SessionSettings
{
    public class Crypting
    {
        public static class En_De_crypt
        {
            #region Definitions
            public const String strPermutation = "ouivybkxaqtd";
            public const Int32 bytePermutation1 = 0x19;
            public const Int32 bytePermutation2 = 0x59;
            public const Int32 bytePermutation3 = 0x17;
            public const Int32 bytePermutation4 = 0x41;
            #endregion

            #region Functions
            //Login ekranında ve rol kontrolünde token oluşturulmak için kullanılan şifreleme
            public static string _Encrypt(string strData)
            {
                return Convert.ToBase64String(_Encrypt(Encoding.UTF8.GetBytes(strData)));
            }
            //Login ekranında ve rol kontrolünde token oluşturulmak için kullanılan şifrelenen veriyi çözme
            public static string _Decrypt(string strData)
            {
                return Encoding.UTF8.GetString(_Decrypt(Convert.FromBase64String(strData)));
            }

            //Method kullanılmadı Byte veritüründeki değerleri şifrelemek için yazıldı
            public static byte[] _Encrypt(byte[] strData)
            {
                PasswordDeriveBytes passbytes =
                new PasswordDeriveBytes(strPermutation,
                new byte[] {bytePermutation1,
                        bytePermutation2,
                        bytePermutation3,
                     bytePermutation4
                });

                MemoryStream memstream = new MemoryStream();
                Aes aes = new AesManaged();
                aes.Key = passbytes.GetBytes(aes.KeySize / 8);
                aes.IV = passbytes.GetBytes(aes.BlockSize / 8);

                CryptoStream cryptostream = new CryptoStream(memstream,
                aes.CreateEncryptor(), CryptoStreamMode.Write);
                cryptostream.Write(strData, 0, strData.Length);
                cryptostream.Close();
                return memstream.ToArray();
            }
            //Method kullanılmadı Byte veritüründe şifrelenen değerleri çözmek için yazıldı
            public static byte[] _Decrypt(byte[] strData)
            {
                PasswordDeriveBytes passbytes =
                new PasswordDeriveBytes(strPermutation,
                new byte[] {bytePermutation1,
                       bytePermutation2,
                       bytePermutation3,
                       bytePermutation4
                });

                MemoryStream memstream = new MemoryStream();
                Aes aes = new AesManaged();
                aes.Key = passbytes.GetBytes(aes.KeySize / 8);
                aes.IV = passbytes.GetBytes(aes.BlockSize / 8);

                CryptoStream cryptostream = new CryptoStream(memstream,
                aes.CreateDecryptor(), CryptoStreamMode.Write);
                cryptostream.Write(strData, 0, strData.Length);
                cryptostream.Close();
                return memstream.ToArray();
            }
            #endregion
        }

    }
}
