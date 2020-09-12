using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace TroubleShootingTool
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Log.Instance.LogFileCreate();
            TempDelete tempDelete = new TempDelete();
            tempDelete.StartTempDelete();
            WebCacheClear webClear = new WebCacheClear();
            webClear.StartWebCacheClear();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Log.Instance.LogFileClose();
        }
    }
}
