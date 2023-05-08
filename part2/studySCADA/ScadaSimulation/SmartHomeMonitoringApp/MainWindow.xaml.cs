using MahApps.Metro.Controls;
using SmartHomeMonitoringApp.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SmartHomeMonitoringApp
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // ActiveItem.Content = new Views.DataBaseMonPage();
            // Frame => Page.xaml
            // ContentControl => UserControl.xaml사용해야함

            // ActiveItem.Content = new Views.DataBaseControl();
        }

        #region < 끝내기 버튼 클릭 이벤트 핸들러 >
        private void MnuExitSubscribe_Click(object sender, RoutedEventArgs e)
        {
            //Environment.Exit(0);

            // 위, 아래 둘 중 하나만 사용하면 됨
            // 작업 관리자에서 프로세스 종료와 같은 역할
            Process.GetCurrentProcess().Kill();
        }
        #endregion

        #region < MQTT 시작메뉴 클릭 이벤트 핸들러 >
        private void MnuStartSubscribe_Click(object sender, RoutedEventArgs e)
        {
            var mqttPopWin = new MqttPopupWindow();
            mqttPopWin.Owner = this;
            mqttPopWin.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            var result = mqttPopWin.ShowDialog();
            
            if (result == true)
            {
                ActiveItem.Content = new Views.DataBaseControl();
            }
        }
        #endregion
    }
}
