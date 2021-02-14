using System;
using System.IO;
using System.Text;

namespace TroubleShootingTool
{
    class Log
    {
        private static Log _instance = null;

        private StreamWriter sw = null;

        public static Log Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new Log();
                }
                return _instance;
            }
        }

        //ログを書き出すファイルの作成
        public void LogFileCreate()
        {
            //実行時パスの取得
            string exePath = Directory.GetCurrentDirectory();
            //ログファイルの名前を設定
            string logFileName = "TroubleShootingTool_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".log";
            string logFilePath = Path.Combine(exePath, logFileName);
            //ログファイルを作成
            sw = new StreamWriter(logFilePath, false, Encoding.GetEncoding("Shift_JIS"));
            //Consoleに書き出すものをログファイルへ書き出すように設定
            Console.SetOut(sw);
            LogWrite(LogState.INFO, "TroubleShootingToolが起動しました");
        }

        //ログを書き出す
        public void LogWrite(LogState _logState, string _message)
        {
            string now = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            switch (_logState)
            {
                case LogState.INFO:
                    Console.WriteLine(now + " [INFO] " + _message);
                    break;

                case LogState.ERROR:
                    Console.WriteLine(now + " [ERROR] " + _message);
                    break;

                //Debug時のみ出力する
                case LogState.DEBUG:
#if DEBUG
                    Console.WriteLine(now + " [DEBUG] " + _message);
#endif
                    break;

                default:
                    break;

            }
        }

        //ログを書き出すファイルを閉じる
        public void LogFileClose()
        {
            LogWrite(LogState.INFO, "TroubleShootingToolが終了しました");
            //ファイルを閉じてオブジェクトを破棄
            if(sw != null)
            {
                sw.Dispose();
            }
        }
    }
}
