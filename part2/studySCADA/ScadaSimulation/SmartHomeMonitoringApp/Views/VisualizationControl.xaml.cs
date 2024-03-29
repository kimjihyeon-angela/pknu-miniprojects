﻿using OxyPlot;
using OxyPlot.Series;
using MySql.Data.MySqlClient;
using SmartHomeMonitoringApp.Logics;
using SmartHomeMonitoringApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
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
using OxyPlot.Legends;

namespace SmartHomeMonitoringApp.Views
{
    /// <summary>
    /// VisualizationControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class VisualizationControl : UserControl
    {
        List<string> Divisions = null;

        string FirstSensingDate = string.Empty;
         
        int TotalDataCount = 0; // 검색된 데이터 개수

        DataSet ds = new DataSet(); // 실제 DB상에 있던 센싱 데이터 담는 데이터셋

        public VisualizationControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // 룸 선택 콤보박스 초기화
            Divisions = new List<string> { "SELECT", "LIVING", "DINING", "BED", "BATH" };
            CboRoomName.ItemsSource = Divisions;
            CboRoomName.SelectedIndex = 0; // SELECT가 선택되어 있도록 설정

            // 검색 시작일 날짜 - DB에서 제일 오래된 날짜 받아와 할당하기
            using (MySqlConnection conn = new MySqlConnection(Commons.MYSQL_CONNSTRING))
            {
                conn.Open();
                var dtQuery = @"SELECT F.sensing_date 
                                  FROM (
		                                SELECT DATE_FORMAT(Sensing_DateTime, '%Y-%m-%d') AS Sensing_Date
		                                  FROM smarthomesensor 
		                                ) AS F
                                 GROUP BY F.Sensing_Date
                                 ORDER BY F.Sensing_Date ASC Limit 1";
                MySqlCommand cmd = new MySqlCommand(dtQuery, conn);
                var result = cmd.ExecuteScalar();
                Debug.WriteLine(result.ToString());
                FirstSensingDate = DtpStart.Text = result.ToString();

                // 검색 종료일 = 현재일자 할당
                DtpEnd.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
            
        }

        // 검색 버튼 클릭 이벤트 핸들러
        private async void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = true;
            string errorMsg = string.Empty; 

            // 검색, 저장, 수정, 삭제 전 반드시 검증(Validation)필요, 필수
            if (CboRoomName.SelectedValue.ToString() == "SELECT")
            {
                isValid = false;
                errorMsg += "방구분을 선택하세요. \n";
                //await Commons.ShowCustomMessageAsync("검색", "방구분을 선택하세요");
                //return;
            }

            // 시스템이 시작된 날짜보다 오래된 날짜 검색 불가하도록 만들기
            if (DateTime.Parse(DtpStart.Text) < DateTime.Parse(FirstSensingDate))
            {
                isValid = false;
                errorMsg += $"검색 시작일은 {FirstSensingDate}부터 가능합니다. \n";
                //await Commons.ShowCustomMessageAsync("검색", $"검색 시작일은 {FirstSensingDate}부터 가능합니다.");
                //return;
            }

            // 오늘날짜 이후의 날짜로 검색 불가하도록 만들기
            if (DateTime.Parse(DtpEnd.Text) > DateTime.Now)
            {
                isValid = false;
                errorMsg += "검색 종료일은 오늘까지 가능합니다. \n";
                //await Commons.ShowCustomMessageAsync("검색", $"검색 종료일은 오늘까지 가능합니다.");
                //return;
            }

            // 검색 시작일이 검색종료일보다 이후일 경우 검색 불가하도록 만들기
            if (DateTime.Parse(DtpStart.Text) > DateTime.Parse(DtpEnd.Text))
            {
                isValid = false;
                errorMsg += "검색 시작일이 검색 종료일보다 이후로 검색이 불가합니다. \n";
                //await Commons.ShowCustomMessageAsync("검색", $"검색 시작일이 검색 종료일보다 이후로 검색이 불가합니다.");
                //return;
            }

            if (isValid == false)
            {
                await Commons.ShowCustomMessageAsync("검색", errorMsg);
                return;
            }


            TotalDataCount = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Commons.MYSQL_CONNSTRING))
                {
                    conn.Open();
                    var searchQuery = @"SELECT id
                                             , Home_Id
                                             , Room_Name
                                             , Sensing_DateTime
                                             , Temp
                                             , Humid
                                          FROM smarthomesensor
                                         WHERE UPPER(Room_Name) = @Room_Name
                                           AND DATE_FORMAT(Sensing_DateTime, '%Y-%m-%d') 
                                       BETWEEN @StartDate AND @EndDate";
                    MySqlCommand cmd = new MySqlCommand(searchQuery, conn);
                    cmd.Parameters.AddWithValue("@Room_Name", CboRoomName.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@StartDate", DtpStart.Text);
                    cmd.Parameters.AddWithValue("@EndDate", DtpEnd.Text);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    
                    adapter.Fill(ds, "smarthomesensor");

                    // MessageBox.Show(ds.Tables["smarthomesensor"].Rows.Count.ToString(), "TotalData"); // 데이터 개수 확인
                }

            }

            catch (Exception ex)
            {
                await Commons.ShowCustomMessageAsync("DB 검색", $"DB 검색 오류 {ex.Message}");
            }

            var tmp = new PlotModel { Title = $"{CboRoomName.SelectedValue} ROOM" };
            var legend = new Legend
            {
                LegendBorder = OxyColors.DarkGray,
                LegendBackground = OxyColor.FromArgb(150, 255, 255, 255),
                LegendPosition = LegendPosition.TopRight,
                LegendPlacement = LegendPlacement.Outside
            };
            tmp.Legends.Add(legend);  // 범례 추가

            var tempSeries = new LineSeries
            {
                Title = "Temperature(℃)",
                MarkerType = MarkerType.Circle,
                Color = OxyColors.DarkOrange,  // 라인색상 온도는 주황색
            };
            var humidSeries = new LineSeries
            {
                Title = "Humidity(%)",
                MarkerType = MarkerType.Square,
                Color = OxyColors.Aqua,  // 습도는 물색
            };


            // DB에서 가져온 데이터 차트에 뿌릴 수 있도록 처리
            if (ds.Tables[0].Rows.Count > 0)
            {
                #region < LiveCharts로 데이터 출력하기 >
                //LvcSmartHome.Series.Clear();

                //LineSeries tempSeries = new LineSeries
                //{
                //    Title = "Temp",
                //    Stroke = new SolidColorBrush(Colors.OrangeRed),
                //};

                //LineSeries humidSeries = new LineSeries
                //{
                //    Title = "Humid",
                //    Stroke = new SolidColorBrush(Colors.Aqua)
                //};

                //IChartValues tempValues = new ChartValues<double>();
                //IChartValues humidValues = new ChartValues<double>();

                ////MessageBox.Show("데이터 출력 가능");
                //foreach (DataRow row in ds.Tables[0].Rows)
                //{
                //    tempValues.Add(Convert.ToDouble(row["Temp"]));
                //}

                //tempSeries.Values = tempValues;

                //LvcSmartHome.Series.Add(tempSeries);
                #endregion
                TotalDataCount = ds.Tables[0].Rows.Count;

                var count = 0;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    tempSeries.Points.Add(new DataPoint(count++, Convert.ToDouble(row["Temp"])));
                    humidSeries.Points.Add(new DataPoint(count++, Convert.ToDouble(row["Humid"])));
                }

            }
            
            tmp.Series.Add(tempSeries);
            tmp.Series.Add(humidSeries);

            OpvSmartHome.Model = tmp;
            LblTotalCount.Content = $"검색 데이터 {TotalDataCount} 개";
            
        }
    }
}
