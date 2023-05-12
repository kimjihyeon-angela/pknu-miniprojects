# 미니프로젝트 Part2
기간 : 2023.05.02. ~ 2023.05.16.

## WPF 학습
- SCADA/HMI 시뮬레이션 (SmartHome 시스템) 시작
	- C# WPF 
	- MahApps.Metro (MetroUI 디자인 라이브러리)
	- Bogus (더미데이터 생성 라이브러리)
	- Newtonsoft.json
	- M2Mqtt (통신 라이브러리)
	- DB 데이터바인딩 (MySql)
	- LiveCarts
	- OxyPlot
	
- SmartHome 시스템 문제점
	- 실행 후 로그 텍스트박스 내용 많아 UI가 느려짐
		- > 일정 수의 로그 출력 후 내용 삭제
	- LiveCarts로 실시간 게이지 외 대용량의 내용을 출력할 경우 시간이 오래걸림
		- > 대용량의 내용 출력할 경우 OxyPlot을 사용해야함


	
<img src="https://raw.githubusercontent.com/kimjihyeon-angela/miniprojects/main/images/BogusTestApp.gif" width="700"/>


< BogusTestApp >


<img src="https://raw.githubusercontent.com/kimjihyeon-angela/miniprojects/main/images/FakeIotDeviceApp_ing.gif" width="700"/>

<IoT 센서 출력 해보기>


<img src="https://raw.githubusercontent.com/kimjihyeon-angela/miniprojects/main/images/FakeIotDeviceApp.gif" width="460"/>

<IoT 센서 화면까지 출력>


<img src="https://raw.githubusercontent.com/kimjihyeon-angela/miniprojects/main/images/SmartHomeMonitoringApp_Design.PNG" width="700"/>

<SmartHomeMonitoringApp 디자인 화면>


<img src="https://raw.githubusercontent.com/kimjihyeon-angela/miniprojects/main/images/smarthomemonitoringapp.gif" width="700"/>

<SmartHomeMonitoringApp 실시간 모니터링 화면>


<img src="https://raw.githubusercontent.com/kimjihyeon-angela/miniprojects/main/images/Fake-smarthome.gif" width="700"/>

<FakeIotDeviceApp + SmartHomeMonitoringApp 실시간 모니터링 화면(온습도 더미데이터 시뮬레이터)>


<img src="https://raw.githubusercontent.com/kimjihyeon-angela/miniprojects/main/images/oxyplot.png" width="700"/>

<DB로 차트 그리기>
