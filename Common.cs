using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using LineBotApi2.Properties;
using System.Text;
using System.Security.Cryptography;

namespace LineBotApi2.Code
{
    public class Common
    {
        const string saltStr = "__SALT__";


        #region 暗号化/復号化
        /// <summary>
        /// ハッシュ化：指定の文字列を秘密キーでハッシュ化する
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        //public static string EncryptString(string str)
        //{
        //    ////秘密キーをハッシュ化
        //    //HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(Settings.Default.channelSecret));

        //    ////ハッシュ値を計算
        //    //byte[] bs = hmac.ComputeHash(Encoding.UTF8.GetBytes(str));
        //    ////リソースを解放する
        //    //hmac.Clear();

        //    ////byte型配列をBASE64エンコードする
        //    //return Convert.ToBase64String(bs);

        //    return EncryptString(str, Settings.Default.channelSecret);

        //}

        public static string EncryptString(string sourceString, string password)
        {
            //RijndaelManagedオブジェクトを作成
            RijndaelManaged rijndael = new RijndaelManaged();

            //パスワードから共有キーと初期化ベクタを作成
            byte[] key, iv;
            GenerateKeyFromPassword(password, rijndael.KeySize, out key, rijndael.BlockSize, out iv);
            rijndael.Key = key;
            rijndael.IV = iv;

            //文字列をバイト型配列に変換する
            byte[] strBytes = Encoding.UTF8.GetBytes(sourceString);

            //対称暗号化オブジェクトの作成
            ICryptoTransform encryptor = rijndael.CreateEncryptor();
            //バイト型配列を暗号化する
            byte[] encBytes = encryptor.TransformFinalBlock(strBytes, 0, strBytes.Length);
            //閉じる
            encryptor.Dispose();

            //バイト型配列を文字列に変換して返す
            return Convert.ToBase64String(encBytes);
        }

        public static string DecryptString(string sourceString, string password)
        {
            //RijndaelManagedオブジェクトを作成
            RijndaelManaged rijndael = new RijndaelManaged();

            //パスワードから共有キーと初期化ベクタを作成
            byte[] key, iv;
            GenerateKeyFromPassword( password, rijndael.KeySize, out key, rijndael.BlockSize, out iv);
            rijndael.Key = key;
            rijndael.IV = iv;

            //文字列をバイト型配列に戻す
            byte[] strBytes = Convert.FromBase64String(sourceString);

            //対称暗号化オブジェクトの作成
            ICryptoTransform decryptor = rijndael.CreateDecryptor();
            //バイト型配列を復号化する
            //復号化に失敗すると例外CryptographicExceptionが発生
            byte[] decBytes = decryptor.TransformFinalBlock(strBytes, 0, strBytes.Length);
            //閉じる
            decryptor.Dispose();

            //バイト型配列を文字列に戻して返す
            return Encoding.UTF8.GetString(decBytes);
        }

        private static void GenerateKeyFromPassword(string password,int keySize, out byte[] key, int blockSize, out byte[] iv)
        {
            //パスワードから共有キーと初期化ベクタを作成する
            //saltを決める
            byte[] salt = System.Text.Encoding.UTF8.GetBytes(saltStr);
            //Rfc2898DeriveBytesオブジェクトを作成する
            System.Security.Cryptography.Rfc2898DeriveBytes deriveBytes =
                new System.Security.Cryptography.Rfc2898DeriveBytes(password, salt);
            //.NET Framework 1.1以下の時は、PasswordDeriveBytesを使用する
            //System.Security.Cryptography.PasswordDeriveBytes deriveBytes =
            //    new System.Security.Cryptography.PasswordDeriveBytes(password, salt);
            //反復処理回数を指定する デフォルトで1000回
            deriveBytes.IterationCount = 1000;

            //共有キーと初期化ベクタを生成する
            key = deriveBytes.GetBytes(keySize / 8);
            iv = deriveBytes.GetBytes(blockSize / 8);
        }
        #endregion


    }
}