using System;
using System.Security.Cryptography;
using System.Text;
//using log4net;
//using LineBotApi2.Properties;

namespace Code
{
    public class CheckSignature
    {
        //static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static bool IsMatchSignature(string signature, string reqestBody, string channelSecret)
        {
            ////doSignatureがfalseの場合は署名チェックを行わない
            //if (!Settings.Default.doSignature)
            //{
            //    logger.Info("IsMatchSignature No Check");
            //    return true;
            //}

            if (string.IsNullOrEmpty(signature))
            {
                //logger.Info("ChannelSignature is Null or Empty");
                //署名が未設定
                return false;
            }

            #region 署名による判定
            //秘密キーをハッシュ化
            HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(channelSecret));

            //ハッシュ値を計算
            byte[] bs = hmac.ComputeHash(Encoding.UTF8.GetBytes(reqestBody));
            //リソースを解放する
            hmac.Clear();

            //byte型配列をBASE64エンコードする
            string base64String = Convert.ToBase64String(bs);

            //logger.Info("IsMatchSignature Data\r\n" + "signature[" + signature + "]\r\n" + "base64String[" + base64String + "]\r\n" + "reqestBody[" + reqestBody + "]\r\n");

            #endregion
            return signature.Equals(base64String);
        }
    }
}