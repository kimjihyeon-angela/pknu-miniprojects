using Bogus;
using FakeIotDeviceApp.Models;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
using uPLibrary.Networking.M2Mqtt;

namespace FakeIotDeviceApp
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        Faker<SensorInfo> FakeHomeSensor { get; set; } = null; // 가짜 스마트홈 센서 값 저장할 변수
        MqttClient Client { get; set; }
        Thread MqttThread { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            InitFakeData();
        }

        private void InitFakeData()
        {
            var Rooms = new[] { "Bed", "Bath", "Living", "Dining" };

            FakeHomeSensor = new Faker<SensorInfo>()
                .RuleFor(s => s.Home_Id, "D501H409") // 임의로 픽스된 홈아이디
                .RuleFor(s => s.Room_Name, f => f.PickRandom(Rooms)) // 실행할 때마다 방 이름이 계속 바뀜
                .RuleFor(s => s.Sensing_DateTime, f => f.Date.Past(0)) // 현재 시각이 생성됨
                .RuleFor(s => s.Temp, f => f.Random.Float(20.0f, 30.0f)) // 20도에서 30도 사이의 실수값 랜덤 생성
                .RuleFor(s => s.Humid, f => f.Random.Float(40.0f, 64.0f)); // 40~64% 사이의 습도값

        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            TxtMqttBrokerIP.Focus();
        }

        private async void BtnConnect_Click(object sender, RoutedEventArgs e)
        {
            // 브로커 IP 미입력 시
            if (string.IsNullOrEmpty(TxtMqttBrokerIP.Text))
            {
                await this.ShowMessageAsync("오류", "브로커 IP를 입력하세요");
                return;
            }

            // 브로커 IP로 접속
            ConnectMqttBroker();

            // 하위의 로직을 무한 반복 
            StartPublish();
            
        }

        // 핵심 처리 센싱된 데이터값을 MQTT 브로커로 전송
        private void StartPublish()
        {
            MqttThread = new Thread(() =>
            {
                while (true)
                {
                    // 가짜 스마트홈 센서값 생성
                    SensorInfo info = FakeHomeSensor.Generate();
                    // Debug -> 배포 시 주석 처리 or 삭제(주석처리가 더 좋음)
                    Debug.WriteLine($"{info.Home_Id} / {info.Room_Name} / {info.Sensing_DateTime} / {info.Temp}");

                    // 객체 직렬화 => 객체 데이터를 xml이나 json 등의 문자열로 변환
                    // Indented -> 들여쓰기
                    var jsonValue = JsonConvert.SerializeObject(info, Formatting.Indented);

                    // 센서값 MQTT 브로커에 전송 (publish)
                    Client.Publish("SmartHome/IoTData/", Encoding.Default.GetBytes(jsonValue));

                    // 스레드와 UI 스레드간 충돌이 안나도록 변경
                    this.Invoke(new Action(() =>
                    {
                        //FlowDocument flowDoc = new FlowDocument();
                        //Paragraph paragraph = new Paragraph();
                        //paragraph.Inlines.Add(new Run(jsonValue));
                        //flowDoc.Blocks.Add(paragraph);
                        //RtbLog.Document = flowDoc;
                        RtbLog.AppendText($"{jsonValue}\n");
                        RtbLog.ScrollToEnd(); // 스크롤 제일 밑으로 보내기
                    }));

                    // RtbLog에 출력 -> 이대로 출력시 스레드와 UI 스레드간 충돌이 남
                    // FlowDocument flowDoc = new FlowDocument();
                    // flowDoc.Blocks.Add(new Paragraph(new Run("테스트")));
                    // RtbLog.Document = flowDoc;

                    // 1초동안 대기
                    Thread.Sleep(1000);
                }
            });
            MqttThread.Start();

        }

        private void ConnectMqttBroker()
        {
            Client = new MqttClient(TxtMqttBrokerIP.Text);
            Client.Connect("SmartHomeDev"); // publish Client ID를 지정
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Client != null && Client.IsConnected == true)
            {
                Client.Disconnect(); // 접속을 끊어줘야 함
            }

            if (MqttThread != null)
            {
                MqttThread.Abort(); 
                // 이 부분이 없으면 프로그램 종료 후에도 메모리에 계속 남아있음
            }
        }
    }
}
