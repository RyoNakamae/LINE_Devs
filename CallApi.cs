using System.Diagnostics;
using System.Net;
using System.Text;

namespace LineBotApi2.Models
{
    public class CallApi
    {
        //static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        const string Token = "";
        const string CallApiUrl = "";


        //POSTでメッセージを送信する
        public static long SendPost(string message)
        {
            Stopwatch sw1 = Stopwatch.StartNew();

            //有効なトークンを取得する
            //string token = CheckAccessToken.CheckToken();

            //logger.Info("message:" + message);

            //バイト型配列に変換
            byte[] postDataBytes = Encoding.UTF8.GetBytes(message);

            //WebRequestの作成
            WebRequest req = WebRequest.Create(CallApiUrl);

            //メソッドにPOSTを指定
            req.Method = "POST";

            //ContentTypeを"application/x-www-form-urlencoded"にする
            req.ContentType = "application/json;charset=UTF-8";
            //POST送信するデータの長さを指定
            req.ContentLength = postDataBytes.Length;
            req.Headers.Add("X-Line-ChannelToken", Token);

            //データをPOST送信するためのStreamを取得
            System.IO.Stream reqStream = req.GetRequestStream();
            //送信するデータを書き込む
            reqStream.Write(postDataBytes, 0, postDataBytes.Length);
            reqStream.Close();

            //サーバーからの応答を受信するためのWebResponseを取得
            System.Net.WebResponse res = req.GetResponse();
            //応答データを受信するためのStreamを取得
            System.IO.Stream resStream = res.GetResponseStream();
            //受信して表示
            System.IO.StreamReader sr = new System.IO.StreamReader(resStream, Encoding.UTF8);

            string responseStr = sr.ReadToEnd();
            sw1.Stop();
            //logger.Info("SendPost response:time[" + sw1.ElapsedMilliseconds + "ms] Str[" + responseStr + "]");

            //閉じる
            sr.Close();

            return sw1.ElapsedMilliseconds;
        }
    }
}