using ControlzEx.Theming;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using SmartHomeMonitoringApp.Logics;
using SmartHomeMonitoringApp.Views;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace SmartHomeMonitoringApp
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        string DefaultTheme { get; set; } = "Light";
        string DefaultAccent { get; set; } = "Yellow";

        public MainWindow()
        {
            InitializeComponent();
            ThemeManager.Current.ThemeSyncMode = ThemeSyncMode.SyncWithAppMode;
            ThemeManager.Current.SyncTheme();
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
                var userControl = new Views.DataBaseControl();
                ActiveItem.Content = userControl;
                //StsSelScreen.Content = typeof(Views.DataBaseControl);
                StsSelScreen.Content = "DataBase Monitoring";
            }
        }
        #endregion

        private async void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // e.Cancel을 true 하고 시작
            e.Cancel = true;

            var mySettings = new MetroDialogSettings
                                 {
                                     AffirmativeButtonText = "종료",
                                     NegativeButtonText = "취소",
                                     AnimateShow = true,
                                     AnimateHide = true
                                 };

            var result = await this.ShowMessageAsync("프로그램을 종료","프로그램을 종료하시겠습니까?",
                                                     MessageDialogStyle.AffirmativeAndNegative, mySettings);

            if (result == MessageDialogResult.Negative)
            {
                e.Cancel = true;
            }
            else if (result == MessageDialogResult.Affirmative)
            {
                if (Commons.MQTT_CLIENT.IsConnected)
                {
                    Commons.MQTT_CLIENT.Disconnect();
                }
                Process.GetCurrentProcess().Kill(); // 가장 확실한 끝내기 방법
            }
        }

        #region < 메뉴버튼 클릭 시 창 이동 이벤트 >
        private void BtnExitProgram_Click(object sender, RoutedEventArgs e)
        {
            // 메트로윈도우 화면 닫을 때 이벤트 핸들러 호출
            MetroWindow_Closing(sender, new System.ComponentModel.CancelEventArgs());
        }

        private void MnuDataBaseMon_Click(object sender, RoutedEventArgs e)
        {
            var userControl = new Views.DataBaseControl();
            ActiveItem.Content = userControl;
            StsSelScreen.Content = "DataBase Monitoring";
        }

        private void MnuRealTimeMon_Click(object sender, RoutedEventArgs e)
        {
            ActiveItem.Content = new Views.RealTimeControl();
            StsSelScreen.Content = "RealTime Monitoring";
        }

        private void MnuVisualizationMon_Click(object sender, RoutedEventArgs e)
        {
            ActiveItem.Content = new Views.VisualizationControl();
            StsSelScreen.Content = "Visualization View";
        }

        private void MnuAbout_Click(object sender, RoutedEventArgs e)
        {
            var about = new About();
            about.Owner = this;
            about.Show();
        }


        #region < 테마, 액센트 전부 처리할 이벤트 핸들러 >
        private void MnuThemeAccent_Clck(object sender, RoutedEventArgs e)
        {
            // 클릭되는 테마가 라이트인지 다크인지 판단
            // -> 라이트 클릭하면 다크 체크 해제, 다크 클릭하면 라이트 체크 해제
            Debug.WriteLine((sender as MenuItem).Header);

            switch ((sender as MenuItem).Header)
            {
                case "Light":
                    MnuLightTheme.IsChecked = true; 
                    MnuDarkTheme.IsChecked = false;
                    DefaultTheme = "Light";
                    break;

                case "Dark":
                    MnuLightTheme.IsChecked = false;
                    MnuDarkTheme.IsChecked = true;
                    DefaultTheme = "Dark";
                    break;

                case "Amber":
                    MnuAccentAmber.IsChecked = true;
                    MnuAccentEmerald.IsChecked = false;
                    MnuAccentBlue.IsChecked = false;
                    MnuAccentBrown.IsChecked = false;
                    MnuAccentCobalt.IsChecked = false;
                    MnuAccentCrimson.IsChecked = false;
                    MnuAccentCyan.IsChecked = false;
                    MnuAccentGreen.IsChecked = false;
                    MnuAccentIndigo.IsChecked = false;
                    MnuAccentLime.IsChecked = false;
                    MnuAccentMagenta.IsChecked = false;
                    MnuAccentMauve.IsChecked = false;
                    MnuAccentOlive.IsChecked = false;
                    MnuAccentOrange.IsChecked = false;
                    MnuAccentPink.IsChecked = false;
                    MnuAccentPurple.IsChecked = false;
                    MnuAccentRed.IsChecked = false;

                    MnuAccentYellow.IsChecked = false;
                    DefaultAccent = "Amber";
                    break;

                case "Emerald":
                    MnuAccentAmber.IsChecked = false;
                    MnuAccentEmerald.IsChecked = true;
                    MnuAccentBlue.IsChecked = false;
                    MnuAccentBrown.IsChecked = false;
                    MnuAccentCobalt.IsChecked = false;
                    MnuAccentCrimson.IsChecked = false;
                    MnuAccentCyan.IsChecked = false;
                    MnuAccentGreen.IsChecked = false;
                    MnuAccentIndigo.IsChecked = false;
                    MnuAccentLime.IsChecked = false;
                    MnuAccentMagenta.IsChecked = false;
                    MnuAccentMauve.IsChecked = false;
                    MnuAccentOlive.IsChecked = false;
                    MnuAccentOrange.IsChecked = false;
                    MnuAccentPink.IsChecked = false;
                    MnuAccentPurple.IsChecked = false;
                    MnuAccentRed.IsChecked = false;

                    MnuAccentYellow.IsChecked = false;
                    DefaultAccent = "Emerald";
                    break;

                case "Blue":
                    MnuAccentAmber.IsChecked = false;
                    MnuAccentEmerald.IsChecked = false;
                    MnuAccentBlue.IsChecked = true;
                    MnuAccentBrown.IsChecked = false;
                    MnuAccentCobalt.IsChecked = false;
                    MnuAccentCrimson.IsChecked = false;
                    MnuAccentCyan.IsChecked = false;
                    MnuAccentGreen.IsChecked = false;
                    MnuAccentIndigo.IsChecked = false;
                    MnuAccentLime.IsChecked = false;
                    MnuAccentMagenta.IsChecked = false;
                    MnuAccentMauve.IsChecked = false;
                    MnuAccentOlive.IsChecked = false;
                    MnuAccentOrange.IsChecked = false;
                    MnuAccentPink.IsChecked = false;
                    MnuAccentPurple.IsChecked = false;
                    MnuAccentRed.IsChecked = false;

                    MnuAccentYellow.IsChecked = false;
                    DefaultAccent = "Blue";
                    break;

                case "Brown":
                    MnuAccentAmber.IsChecked = false;
                    MnuAccentEmerald.IsChecked = false;
                    MnuAccentBlue.IsChecked = false;
                    MnuAccentBrown.IsChecked = true;
                    MnuAccentCobalt.IsChecked = false;
                    MnuAccentCrimson.IsChecked = false;
                    MnuAccentCyan.IsChecked = false;
                    MnuAccentGreen.IsChecked = false;
                    MnuAccentIndigo.IsChecked = false;
                    MnuAccentLime.IsChecked = false;
                    MnuAccentMagenta.IsChecked = false;
                    MnuAccentMauve.IsChecked = false;
                    MnuAccentOlive.IsChecked = false;
                    MnuAccentOrange.IsChecked = false;
                    MnuAccentPink.IsChecked = false;
                    MnuAccentPurple.IsChecked = false;
                    MnuAccentRed.IsChecked = false;

                    MnuAccentYellow.IsChecked = false;
                    DefaultAccent = "Brown";
                    break;

                case "Cobalt":
                    MnuAccentAmber.IsChecked = false;
                    MnuAccentEmerald.IsChecked = false;
                    MnuAccentBlue.IsChecked = false;
                    MnuAccentBrown.IsChecked = false;
                    MnuAccentCobalt.IsChecked = true;
                    MnuAccentCrimson.IsChecked = false;
                    MnuAccentCyan.IsChecked = false;
                    MnuAccentGreen.IsChecked = false;
                    MnuAccentIndigo.IsChecked = false;
                    MnuAccentLime.IsChecked = false;
                    MnuAccentMagenta.IsChecked = false;
                    MnuAccentMauve.IsChecked = false;
                    MnuAccentOlive.IsChecked = false;
                    MnuAccentOrange.IsChecked = false;
                    MnuAccentPink.IsChecked = false;
                    MnuAccentPurple.IsChecked = false;
                    MnuAccentRed.IsChecked = false;

                    MnuAccentYellow.IsChecked = false;
                    DefaultAccent = "Cobalt";
                    break;

                case "Crimson":
                    MnuAccentAmber.IsChecked = false;
                    MnuAccentEmerald.IsChecked = false;
                    MnuAccentBlue.IsChecked = false;
                    MnuAccentBrown.IsChecked = false;
                    MnuAccentCobalt.IsChecked = false;
                    MnuAccentCrimson.IsChecked = true;
                    MnuAccentCyan.IsChecked = false;
                    MnuAccentGreen.IsChecked = false;
                    MnuAccentIndigo.IsChecked = false;
                    MnuAccentLime.IsChecked = false;
                    MnuAccentMagenta.IsChecked = false;
                    MnuAccentMauve.IsChecked = false;
                    MnuAccentOlive.IsChecked = false;
                    MnuAccentOrange.IsChecked = false;
                    MnuAccentPink.IsChecked = false;
                    MnuAccentPurple.IsChecked = false;
                    MnuAccentRed.IsChecked = false;

                    MnuAccentYellow.IsChecked = false;
                    DefaultAccent = "Crimson";
                    break;

                case "Cyan":
                    MnuAccentAmber.IsChecked = false;
                    MnuAccentEmerald.IsChecked = false;
                    MnuAccentBlue.IsChecked = false;
                    MnuAccentBrown.IsChecked = false;
                    MnuAccentCobalt.IsChecked = false;
                    MnuAccentCrimson.IsChecked = false;
                    MnuAccentCyan.IsChecked = true;
                    MnuAccentGreen.IsChecked = false;
                    MnuAccentIndigo.IsChecked = false;
                    MnuAccentLime.IsChecked = false;
                    MnuAccentMagenta.IsChecked = false;
                    MnuAccentMauve.IsChecked = false;
                    MnuAccentOlive.IsChecked = false;
                    MnuAccentOrange.IsChecked = false;
                    MnuAccentPink.IsChecked = false;
                    MnuAccentPurple.IsChecked = false;
                    MnuAccentRed.IsChecked = false;

                    MnuAccentYellow.IsChecked = false;
                    DefaultAccent = "Cyan";
                    break;

                case "Green":
                    MnuAccentAmber.IsChecked = false;
                    MnuAccentEmerald.IsChecked = false;
                    MnuAccentBlue.IsChecked = false;
                    MnuAccentBrown.IsChecked = false;
                    MnuAccentCobalt.IsChecked = false;
                    MnuAccentCrimson.IsChecked = false;
                    MnuAccentCyan.IsChecked = false;
                    MnuAccentGreen.IsChecked = true;
                    MnuAccentIndigo.IsChecked = false;
                    MnuAccentLime.IsChecked = false;
                    MnuAccentMagenta.IsChecked = false;
                    MnuAccentMauve.IsChecked = false;
                    MnuAccentOlive.IsChecked = false;
                    MnuAccentOrange.IsChecked = false;
                    MnuAccentPink.IsChecked = false;
                    MnuAccentPurple.IsChecked = false;
                    MnuAccentRed.IsChecked = false;

                    MnuAccentYellow.IsChecked = false;
                    DefaultAccent = "Green";
                    break;

                case "Indigo":
                    MnuAccentAmber.IsChecked = false;
                    MnuAccentEmerald.IsChecked = false;
                    MnuAccentBlue.IsChecked = false;
                    MnuAccentBrown.IsChecked = false;
                    MnuAccentCobalt.IsChecked = false;
                    MnuAccentCrimson.IsChecked = false;
                    MnuAccentCyan.IsChecked = false;
                    MnuAccentGreen.IsChecked = false;
                    MnuAccentIndigo.IsChecked = true;
                    MnuAccentLime.IsChecked = false;
                    MnuAccentMagenta.IsChecked = false;
                    MnuAccentMauve.IsChecked = false;
                    MnuAccentOlive.IsChecked = false;
                    MnuAccentOrange.IsChecked = false;
                    MnuAccentPink.IsChecked = false;
                    MnuAccentPurple.IsChecked = false;
                    MnuAccentRed.IsChecked = false;

                    MnuAccentYellow.IsChecked = false;
                    DefaultAccent = "Indigo";
                    break;

                case "Lime":
                    MnuAccentAmber.IsChecked = false;
                    MnuAccentEmerald.IsChecked = false;
                    MnuAccentBlue.IsChecked = false;
                    MnuAccentBrown.IsChecked = false;
                    MnuAccentCobalt.IsChecked = false;
                    MnuAccentCrimson.IsChecked = false;
                    MnuAccentCyan.IsChecked = false;
                    MnuAccentGreen.IsChecked = false;
                    MnuAccentIndigo.IsChecked = false;
                    MnuAccentLime.IsChecked = true;
                    MnuAccentMagenta.IsChecked = false;
                    MnuAccentMauve.IsChecked = false;
                    MnuAccentOlive.IsChecked = false;
                    MnuAccentOrange.IsChecked = false;
                    MnuAccentPink.IsChecked = false;
                    MnuAccentPurple.IsChecked = false;
                    MnuAccentRed.IsChecked = false;

                    MnuAccentYellow.IsChecked = false;
                    DefaultAccent = "Lime";
                    break;

                case "Magenta":
                    MnuAccentAmber.IsChecked = false;
                    MnuAccentEmerald.IsChecked = false;
                    MnuAccentBlue.IsChecked = false;
                    MnuAccentBrown.IsChecked = false;
                    MnuAccentCobalt.IsChecked = false;
                    MnuAccentCrimson.IsChecked = false;
                    MnuAccentCyan.IsChecked = false;
                    MnuAccentGreen.IsChecked = false;
                    MnuAccentIndigo.IsChecked = false;
                    MnuAccentLime.IsChecked = false;
                    MnuAccentMagenta.IsChecked = true;
                    MnuAccentMauve.IsChecked = false;
                    MnuAccentOlive.IsChecked = false;
                    MnuAccentOrange.IsChecked = false;
                    MnuAccentPink.IsChecked = false;
                    MnuAccentPurple.IsChecked = false;
                    MnuAccentRed.IsChecked = false;

                    MnuAccentYellow.IsChecked = false;
                    DefaultAccent = "Magenta";
                    break;

                case "Mauve":
                    MnuAccentAmber.IsChecked = false;
                    MnuAccentEmerald.IsChecked = false;
                    MnuAccentBlue.IsChecked = false;
                    MnuAccentBrown.IsChecked = false;
                    MnuAccentCobalt.IsChecked = false;
                    MnuAccentCrimson.IsChecked = false;
                    MnuAccentCyan.IsChecked = false;
                    MnuAccentGreen.IsChecked = false;
                    MnuAccentIndigo.IsChecked = false;
                    MnuAccentLime.IsChecked = false;
                    MnuAccentMagenta.IsChecked = false;
                    MnuAccentMauve.IsChecked = true;
                    MnuAccentOlive.IsChecked = false;
                    MnuAccentOrange.IsChecked = false;
                    MnuAccentPink.IsChecked = false;
                    MnuAccentPurple.IsChecked = false;
                    MnuAccentRed.IsChecked = false;

                    MnuAccentYellow.IsChecked = false;
                    DefaultAccent = "Mauve";
                    break;

                case "Olive":
                    MnuAccentAmber.IsChecked = false;
                    MnuAccentEmerald.IsChecked = false;
                    MnuAccentBlue.IsChecked = false;
                    MnuAccentBrown.IsChecked = false;
                    MnuAccentCobalt.IsChecked = false;
                    MnuAccentCrimson.IsChecked = false;
                    MnuAccentCyan.IsChecked = false;
                    MnuAccentGreen.IsChecked = false;
                    MnuAccentIndigo.IsChecked = false;
                    MnuAccentLime.IsChecked = false;
                    MnuAccentMagenta.IsChecked = false;
                    MnuAccentMauve.IsChecked = false;
                    MnuAccentOlive.IsChecked = true;
                    MnuAccentOrange.IsChecked = false;
                    MnuAccentPink.IsChecked = false;
                    MnuAccentPurple.IsChecked = false;
                    MnuAccentRed.IsChecked = false;

                    MnuAccentYellow.IsChecked = false;
                    DefaultAccent = "Olive";
                    break;

                case "Orange":
                    MnuAccentAmber.IsChecked = false;
                    MnuAccentEmerald.IsChecked = false;
                    MnuAccentBlue.IsChecked = false;
                    MnuAccentBrown.IsChecked = false;
                    MnuAccentCobalt.IsChecked = false;
                    MnuAccentCrimson.IsChecked = false;
                    MnuAccentCyan.IsChecked = false;
                    MnuAccentGreen.IsChecked = false;
                    MnuAccentIndigo.IsChecked = false;
                    MnuAccentLime.IsChecked = false;
                    MnuAccentMagenta.IsChecked = false;
                    MnuAccentMauve.IsChecked = false;
                    MnuAccentOlive.IsChecked = false;
                    MnuAccentOrange.IsChecked = true;
                    MnuAccentPink.IsChecked = false;
                    MnuAccentPurple.IsChecked = false;
                    MnuAccentRed.IsChecked = false;

                    MnuAccentYellow.IsChecked = false;
                    DefaultAccent = "Orange";
                    break;

                case "Pink":
                    MnuAccentAmber.IsChecked = false;
                    MnuAccentEmerald.IsChecked = false;
                    MnuAccentBlue.IsChecked = false;
                    MnuAccentBrown.IsChecked = false;
                    MnuAccentCobalt.IsChecked = false;
                    MnuAccentCrimson.IsChecked = false;
                    MnuAccentCyan.IsChecked = false;
                    MnuAccentGreen.IsChecked = false;
                    MnuAccentIndigo.IsChecked = false;
                    MnuAccentLime.IsChecked = false;
                    MnuAccentMagenta.IsChecked = false;
                    MnuAccentMauve.IsChecked = false;
                    MnuAccentOlive.IsChecked = false;
                    MnuAccentOrange.IsChecked = false;
                    MnuAccentPink.IsChecked = true;
                    MnuAccentPurple.IsChecked = false;
                    MnuAccentRed.IsChecked = false;

                    MnuAccentYellow.IsChecked = false;
                    DefaultAccent = "Pink";
                    break;

                case "Purple":
                    MnuAccentAmber.IsChecked = false;
                    MnuAccentEmerald.IsChecked = false;
                    MnuAccentBlue.IsChecked = false;
                    MnuAccentBrown.IsChecked = false;
                    MnuAccentCobalt.IsChecked = false;
                    MnuAccentCrimson.IsChecked = false;
                    MnuAccentCyan.IsChecked = false;
                    MnuAccentGreen.IsChecked = false;
                    MnuAccentIndigo.IsChecked = false;
                    MnuAccentLime.IsChecked = false;
                    MnuAccentMagenta.IsChecked = false;
                    MnuAccentMauve.IsChecked = false;
                    MnuAccentOlive.IsChecked = false;
                    MnuAccentOrange.IsChecked = false;
                    MnuAccentPink.IsChecked = false;
                    MnuAccentPurple.IsChecked = true;
                    MnuAccentRed.IsChecked = false;

                    MnuAccentYellow.IsChecked = false;
                    DefaultAccent = "Purple";
                    break;

                case "Red":
                    MnuAccentAmber.IsChecked = false;
                    MnuAccentEmerald.IsChecked = false;
                    MnuAccentBlue.IsChecked = false;
                    MnuAccentBrown.IsChecked = false;
                    MnuAccentCobalt.IsChecked = false;
                    MnuAccentCrimson.IsChecked = false;
                    MnuAccentCyan.IsChecked = false;
                    MnuAccentGreen.IsChecked = false;
                    MnuAccentIndigo.IsChecked = false;
                    MnuAccentLime.IsChecked = false;
                    MnuAccentMagenta.IsChecked = false;
                    MnuAccentMauve.IsChecked = false;
                    MnuAccentOlive.IsChecked = false;
                    MnuAccentOrange.IsChecked = false;
                    MnuAccentPink.IsChecked = false;
                    MnuAccentPurple.IsChecked = false;
                    MnuAccentRed.IsChecked = true;

                    MnuAccentYellow.IsChecked = false;
                    DefaultAccent = "Red";
                    break;

                case "Yellow":
                    MnuAccentAmber.IsChecked = false;
                    MnuAccentEmerald.IsChecked = false;
                    MnuAccentBlue.IsChecked = false;
                    MnuAccentBrown.IsChecked = false;
                    MnuAccentCobalt.IsChecked = false;
                    MnuAccentCrimson.IsChecked = false;
                    MnuAccentCyan.IsChecked = false;
                    MnuAccentGreen.IsChecked = false;
                    MnuAccentIndigo.IsChecked = false;
                    MnuAccentLime.IsChecked = false;
                    MnuAccentMagenta.IsChecked = false;
                    MnuAccentMauve.IsChecked = false;
                    MnuAccentOlive.IsChecked = false;
                    MnuAccentOrange.IsChecked = false;
                    MnuAccentPurple.IsChecked = false;
                    MnuAccentPink.IsChecked = false;
                    MnuAccentRed.IsChecked = false;

                    MnuAccentYellow.IsChecked = true;
                    DefaultAccent = "Yellow";
                    break;
            }

            // Accent도 체크 하는 값을 받아와 나머지 액센트 체크 해제


            ThemeManager.Current.ChangeTheme(this, $"{DefaultTheme}.{DefaultAccent}");
        }
        #endregion
        #endregion
    }
}
