using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Forms;

namespace TroubleShootingTool
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private string oneDrivePath = "";
        private TempDelete tempDelete = null;
        private WebCacheClear webCacheClear = null;
        private OneDriveCashClear oneDriveCash = null;
        private OutlookClear outlookClear = null;
        private SkypeClear skypeClear = null;

        public MainWindow()
        {
            InitializeComponent();
            oneDrivePathDisp.Text = @"C:\";
            oneDrivePathDisp.IsEnabled = false;
            oneDrive_Button.IsEnabled = false;
            tempDelete = new TempDelete();
            webCacheClear = new WebCacheClear();
            oneDriveCash = new OneDriveCashClear();
            outlookClear = new OutlookClear();
            skypeClear = new SkypeClear();
        }

        private void oneDirve_Button_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbDialog = new FolderBrowserDialog();
            //説明文を指定する
            fbDialog.Description = "OneDriveのフォルダを選択";

            //デフォルトのフォルダを指定する
            fbDialog.SelectedPath = @"C:\";

            //「新しいフォルダーの作成する」ボタンを非表示
            fbDialog.ShowNewFolderButton = false;

            DialogResult result = fbDialog.ShowDialog();

            //ダイアログの結果を受け取る
            if(result == System.Windows.Forms.DialogResult.OK)
            {
                
                oneDrivePathDisp.Text = fbDialog.SelectedPath.ToString();
                oneDrivePath = fbDialog.SelectedPath.ToString();
            }
            else
            {
                //「キャンセル」または「×」を押された場合
                oneDrivePathDisp.Text = "";
                oneDrivePath = "";
            }

            //オブジェクトを破棄する
            fbDialog.Dispose();
        }

        private void oneDriveCacheClear_Checked(object sender, RoutedEventArgs e)
        {
            oneDrivePathDisp.IsEnabled = true;
            oneDrive_Button.IsEnabled = true;
        }

        private void oneDriveCacheClear_Unchecked(object sender, RoutedEventArgs e)
        {
            oneDrivePathDisp.IsEnabled = false;
            oneDrive_Button.IsEnabled = false;
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Window mainWindow = System.Windows.Application.Current.MainWindow;
            mainWindow.Close();
        }

        private void runButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = System.Windows.MessageBox.Show("実行は各自責任の下行ってください!!", "確認", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if(result == MessageBoxResult.OK)
            {
                //%temp%配下の削除
                if(tempCacheClear.IsChecked == true)
                {
#if DEBUG
                    Log.Instance.LogWrite(LogState.DEBUG, "Debug実行のため、Temp削除をスキップします");
#else
                    Log.Instance.LogWrite(LogState.INFO, "Tempを対象とします");
                    tempDelete.StartTempDelete();
                    Log.Instance.LogWrite(LogState.INFO, "Tempへの処理を終了します");
#endif
                }

                //Edgeのキャッシュ削除
                if (edgeCacheClear.IsChecked == true)
                {
#if DEBUG
                    Log.Instance.LogWrite(LogState.DEBUG, "Debug実行のため、Edgeのキャッシュ削除をスキップします");
#else
                    Log.Instance.LogWrite(LogState.INFO, "Edgeを対象とします");
                    webCacheClear.EdgeClear();
                    Log.Instance.LogWrite(LogState.INFO, "Edgeへの処理を終了します");
#endif
                }

                //Chromeのキャッシュ削除
                if (chromeCacheClear.IsChecked == true)
                {
#if DEBUG
                    Log.Instance.LogWrite(LogState.DEBUG, "Debug実行のため、Chromeのキャッシュ削除をスキップします");
#else
                    Log.Instance.LogWrite(LogState.INFO, "Chromeを対象とします");
                    webCacheClear.ChromeClear();
                    Log.Instance.LogWrite(LogState.INFO, "Chromeへの処理を終了します");
#endif
                }

                //OneDriveのキャッシュクリア
                if (oneDriveCacheClear.IsChecked == true)
                {
                    if((oneDrivePath != "") && (oneDrivePath != null))
                    {
#if DEBUG
                        Log.Instance.LogWrite(LogState.DEBUG, "Debug実行のため、OneDriveのキャッシュ削除をスキップします");
#else
                        Log.Instance.LogWrite(LogState.INFO, "OneDriveを対象とします");
                        oneDriveCash.GetFileAndFolder(oneDrivePath);
                        Log.Instance.LogWrite(LogState.INFO, "OneDriveへの処理を終了します");
#endif
                    }
                }

                //資格情報の削除
                if(credClear.IsChecked == true)
                {
#if DEBUG
                    Log.Instance.LogWrite(LogState.DEBUG, "Debug実行のため、資格削除をスキップします");
#else
                    Log.Instance.LogWrite(LogState.INFO, "資格情報を対象とします");
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
                    Log.Instance.LogWrite(LogState.INFO, "資格情報への処理を終了します");
#endif
                }


                //outlookのトラブルシューティング
                if(outlookTrouble.IsChecked == true)
                {
#if DEBUG
                    Log.Instance.LogWrite(LogState.DEBUG, "Debug実行のため、outlookトラブルシューティングをスキップします");
#else
                    Log.Instance.LogWrite(LogState.INFO, "outlookを対象とします");
                    outlookClear.OutlookClearExe();
                    Log.Instance.LogWrite(LogState.INFO, "outlookへの処理を終了します");
#endif
                }

                //skypeのトラブルシューティング
                if(skypeTrouble.IsChecked == true)
                {
#if DEBUG
                    Log.Instance.LogWrite(LogState.DEBUG, "Debug実行のため、skypeトラブルシューティングをスキップします");
#else
                    Log.Instance.LogWrite(LogState.INFO, "skypeを対象とします");
                    skypeClear.SkypeClearExe();
                    Log.Instance.LogWrite(LogState.INFO, "skypeへの処理を終了します");
#endif
                }
            }
            else
            {
                //OK以外のボタンを押された場合
            }
        }
    }
}
