using System;
using System.Diagnostics;
using System.IO;

namespace TroubleShootingTool
{
    class OneDriveCashClear
    {
        public OneDriveCashClear()
        {

        }

        private void GetAttribute(string _targetFilePath)
        {
            string result = CommandCheck("attrib", _targetFilePath);

            if (result.Contains("パラメーターの書式が違います -"))
            {
                //パスが長すぎるなど何らかの要因で取得できなかった場合
                Log.Instance.LogWrite(LogState.INFO, "処理できませんでした");
            }
            else
            {
                //attribで取得できる属性の文字数
                result = result.Substring(0, 21);
                //O属性(オフライン)、S属性(システム)、P属性(固定)を持たないファイルがキャッシュクリア対象
                if ((result.IndexOf('O') == -1) && (result.IndexOf('P') == -1) && (result.IndexOf('S') == -1))
                {
                    string attribResult = CommandCheck("attrib -P +U", _targetFilePath);
                    attribResult = CommandCheck("attrib", _targetFilePath);
                    //U属性の有無で成否判断
                    string attributes_r = attribResult.Substring(0, 21);
                    if (attributes_r.IndexOf('U') >= -1)
                    {
                        Log.Instance.LogWrite(LogState.INFO, "キャッシュクリア成功しました");
                    }
                    else
                    {
                        Log.Instance.LogWrite(LogState.INFO, "キャッシュクリア失敗しました");
                    }
                }
            }
        }

        private string CommandCheck(string _command, string _targetFilePath)
        {
            string result = "";

            Process p = new Process();

            string cmdPath = Environment.GetEnvironmentVariable("COMSPEC");
            //実行環境が64bitだった場合は、コマンドプロンプトの実行ファイルを変える
            if (Environment.Is64BitOperatingSystem)
            {
                cmdPath = Environment.GetEnvironmentVariable("SystemRoot") + @"\Sysnative\cmd.exe";
            }

            try
            {
                p.StartInfo.FileName = cmdPath;
                //出力を読み取れるようにする
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardInput = false;
                //ウィンドウを表示しないようにする
                p.StartInfo.CreateNoWindow = true;
                //コマンドラインを指定("/c" は実行後閉じるため必要)
                p.StartInfo.Arguments = @"/c " + _command + " " + _targetFilePath;

                //起動
                p.Start();

                //出力を読み取る
                result = p.StandardOutput.ReadToEnd();

                //プロセス終了まで待機する
                //WaitForExitはReadToEndの後である必要がある(親プロセス、子プロセスでブロック防止のため)
                p.WaitForExit();
                p.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                result = "";
            }

            return result;
        }

        public void GetFileAndFolder(string _targetFolder)
        {
            if (Directory.Exists(_targetFolder))
            {
                DirectoryInfo currentDirectory = new DirectoryInfo(_targetFolder);

                //ディレクトリ直下のすべてのディレクトリ一覧を取得する
                DirectoryInfo[] directoryInfos = currentDirectory.GetDirectories();
                foreach (var di in directoryInfos)
                {
                    GetFileAndFolder(di.FullName);
                }

                //ディレクトリ直下のすべてのファイル一覧を取得する
                string[] fileNames = Directory.GetFiles(_targetFolder);
                foreach (var fileName in fileNames)
                {
                    string message = fileName + " に対して処理を実施します";
                    Log.Instance.LogWrite(LogState.INFO, message);
                    GetAttribute(fileName);
                }
            }
            else
            {
                string message = _targetFolder + " が見つかりませんでした";
                Log.Instance.LogWrite(LogState.INFO, message);
            }
        }
    }
}
