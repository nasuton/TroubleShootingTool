using System;
using System.IO;

namespace TroubleShootingTool
{
    class WebCacheClear
    {
        public WebCacheClear()
        {

        }

        public void EdgeClear()
        {
            Log.Instance.LogWrite(LogState.INFO, "Edgeアプリのキャッシュ削除を開始します");
            //appDataパスを取得
            string appDataPath = "";
            appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            EdgeChromiumBaseVer(appDataPath);
            EdgeOldVer(appDataPath);
            Log.Instance.LogWrite(LogState.INFO, "Edgeアプリのキャッシュ削除を終了します");
        }

        public void ChromeClear()
        {
            Log.Instance.LogWrite(LogState.INFO, "Chromeアプリのキャッシュ削除を開始します");
            //appDataパスを取得
            string appDataPath = "";
            appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            Chrome(appDataPath);
            Log.Instance.LogWrite(LogState.INFO, "Chromeアプリのキャッシュ削除を終了します");
        }

        //Edge(Chromiumベース)
        private void EdgeChromiumBaseVer(string _appDataPath)
        {
            string edge_ChromiumBase = Path.Combine(_appDataPath, @"Microsoft\Edge\User Data\Default\Cache");
            if (Directory.Exists(edge_ChromiumBase))
            {
                GetFileAndFolder(edge_ChromiumBase);
            }
            else
            {
                Log.Instance.LogWrite(LogState.INFO, "Edge(Chromiumベース) のフォルダが見つかりませんでした");
            }
        }

        //Edge(旧バージョン)
        private void EdgeOldVer(string _appDataPath)
        {

            string edge_oldVer = Path.Combine(_appDataPath, @"Packages\Microsoft.MicrosoftEdge_8wekyb3d8bbwe\AC\MicrosoftEdge\Cache");
            if (Directory.Exists(edge_oldVer))
            {
                GetFileAndFolder(edge_oldVer);
            }

            string edge_oldVer001 = Path.Combine(_appDataPath, @"Packages\Microsoft.MicrosoftEdge_8wekyb3d8bbwe\AC\#!001\MicrosoftEdge\Cache");
            if (Directory.Exists(edge_oldVer001))
            {
                GetFileAndFolder(edge_oldVer001);
            }

            string edge_oldVer002 = Path.Combine(_appDataPath, @"Packages\Microsoft.MicrosoftEdge_8wekyb3d8bbwe\AC\#!002\MicrosoftEdge\Cache");
            if (Directory.Exists(edge_oldVer002))
            {
                GetFileAndFolder(edge_oldVer002);
            }
            
        }

        //Chrome
        private void Chrome(string _appDataPath)
        {
            //デフォルト
            string chromeDefault = Path.Combine(_appDataPath, @"Google\Chrome\User Data\Default\Cache");
            if (Directory.Exists(chromeDefault))
            {
                GetFileAndFolder(chromeDefault);
            }

            //他ユーザー(Profile 5 まで確認)
            for (int i = 1; i < 6; i++)
            {
                string profileNum = @"Google\Chrome\User Data\Profile " + i + @"\Cache";
                string chromeProfile = Path.Combine(_appDataPath, profileNum);
                if (Directory.Exists(chromeProfile))
                {
                    GetFileAndFolder(chromeProfile);
                }
            }
        }

        //対象ファイル内を検索
        private void GetFileAndFolder(string _targetFolder)
        {
            try
            {
                DirectoryInfo currentDirectory = new DirectoryInfo(_targetFolder);
                string message = _targetFolder + " 内を検索します";
                Log.Instance.LogWrite(LogState.DEBUG, message);

                //ディレクトリ直下のすべてのディレクトリ一覧を取得する
                DirectoryInfo[] directoryInfos = currentDirectory.GetDirectories();
                foreach (var di in directoryInfos)
                {
                    //フォルダ内をさらに検索する(再起)
                    GetFileAndFolder(di.FullName);
                    DeleteFolder(di.FullName);
                }

                //ディレクトリ直下のすべてのファイル一覧を取得する
                string[] fileNames = Directory.GetFiles(_targetFolder);
                foreach (var fileName in fileNames)
                {
                    DeleteFile(fileName);
                }
            }
            catch (Exception ex)
            {
                Log.Instance.LogWrite(LogState.ERROR, ex.ToString());
            }
        }

        //フォルダを削除する
        private void DeleteFolder(string _targetFolder)
        {
            try
            {
                Log.Instance.LogWrite(LogState.INFO, _targetFolder + " を削除します");
                Directory.Delete(_targetFolder, true);
            }
            catch (Exception ex)
            {
                Log.Instance.LogWrite(LogState.ERROR, ex.ToString());
            }
        }

        //ファイルを削除する
        private void DeleteFile(string _targetFile)
        {
            try
            {
                Log.Instance.LogWrite(LogState.INFO, _targetFile + " を削除します");
                File.Delete(_targetFile);
            }
            catch (Exception ex)
            {
                Log.Instance.LogWrite(LogState.ERROR, ex.ToString());
            }
        }


    }
}
