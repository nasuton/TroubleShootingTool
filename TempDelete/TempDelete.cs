using System;
using System.IO;

namespace TroubleShootingTool
{
    /// <summary>
    /// %temp%フォルダ配下のフォルダ/ファイルを削除
    /// </summary>
    class TempDelete
    {
        public TempDelete()
        {

        }

        public void StartTempDelete()
        {
            Log.Instance.LogWrite(LogState.INFO, "Tempフォルダ内削除を開始します");
            string tempPath = Path.GetTempPath();
            GetFileAndFolder(tempPath);
            Log.Instance.LogWrite(LogState.INFO, "Tempフォルダ内削除を終了します");
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
