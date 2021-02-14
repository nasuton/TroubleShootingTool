using System;
using System.Diagnostics;

namespace TroubleShootingTool
{
    class SkypeClear
    {
        public SkypeClear()
        {
            
        }

        private void ProcessKill()
        {
            Process[] ps = Process.GetProcesses();

            foreach(var p in ps)
            {
                if(p.ProcessName == "lync")
                {
                    p.Kill();
                }
                else if(p.ProcessName == "UcMapi")
                {
                    p.Kill();
                }
            }
        }

        private void CredentialDelete()
        {
            var credentials = Credential.Enumerate();
            credentials.ForEach(cred =>
            {
                string targetName = cred.TargetName;
                string targetAlias = cred.TargetAlias;
                string userName = cred.UserName;
                string message = "資格情報を削除しました。TargetName : " + targetName + ", TargetAlias : " + targetAlias + ", UserName : " + userName;
                Log.Instance.LogWrite(LogState.INFO, message);
                Credential.Delete(cred);
            });
        }

        private void DNSCasheClear()
        {
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
                p.StartInfo.Arguments = @"/c ipconfig /flushdns";

                //起動
                p.Start();

                //プロセス終了まで待機する
                //WaitForExitはReadToEndの後である必要がある(親プロセス、子プロセスでブロック防止のため)
                p.WaitForExit();
                p.Close();
            }
            catch (Exception ex)
            {
                Log.Instance.LogWrite(LogState.ERROR, ex.ToString());
            }
        }

        public void SkypeClearExe()
        {
            ProcessKill();
            CredentialDelete();
            DNSCasheClear();
        }
    }
}
