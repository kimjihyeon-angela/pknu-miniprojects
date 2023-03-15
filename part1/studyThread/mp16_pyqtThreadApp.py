# 스레드 사용 앱
import sys 
from PyQt5 import uic
from PyQt5.QtWidgets import *
from PyQt5.QtGui import *
from PyQt5.QtCore import *

import time

Max = 1000

# 스레드를 위한 class
class BackgrounWorker(QThread):   # PyQt5 스레드를 위한 클래스 존재
    procChanged = pyqtSignal(int) # 커스텀 시그널 (마우스 클릭같은 시그널을 사용자가 따로 만드는 것)

    def __init__(self, count=0, parent=None) -> None:
        super().__init__()
        self.main = parent
        self.working = False # 스레드 동작여부
        self.count = count
        

    def run(self):   # thread.start() -> run() 대신 실행
        # self.parent.pgbTask.setRange(0,100)
        # for i in range(0, 101):
        #     print(f'스레드 출력 > {i}')
        #     self.parent.pgbTask.setValue(i)
        #     self.parent.txbLog.append(f'스레드 출력 > {i}')
        while self.working:
            if self.count <= Max:
                self.procChanged.emit(self.count) # 시그널을 내보냄
                self.count += 1                   # 값 증가만 처리함, 업무프로세스 동작하는 위치
                time.sleep(0.001)                 # 1ms, 응답 없음을 없애기 위함, 너무 세밀하게 줄 경우 GUI 처리를 제대로 못함
                # 간단히 count만 하는 것이기 때문에 time.sleep()이 필요함(굳이 time.sleep()을 사용할 필요는 없음)
                # if time.sleep()이 필요한 경우 적당한 시간을 찾아야 할 필요가 있음
                # 너무 느리지도 빠르지도 않도록,,
            else:
                self.working = False              # 멈춤

# GUI 앱을 실행하는 class
class qtApp(QWidget):
    def __init__(self):
        super().__init__()
        uic.loadUi('./studyThread/threadApp.ui', self)
        self.setWindowTitle('스레드 사용 앱 v0.4')
        self.pgbTask.setValue(0)

        # 내장된 시그널
        self.btnStart.clicked.connect(self.btnStartClicked)
        # 스레드 생성
        self.worker = BackgrounWorker(parent=self, count=0)
        # 백그라운드 워커에 있는 시그널 접근, 처리하기 위한 슬롯 함수
        self.worker.procChanged.connect(self.procUpdated)
        self.pgbTask.setRange(0, Max)

    # @pyqtSlot(int) # 데코레이션 -> 없어도 동작함, 정확하게 커스텀 시그널을 사용하는 슬롯임을 알려주기 위함
    def procUpdated(self, count):
        self.txbLog.append(f'스레드 출력 > {count}')
        self.pgbTask.setValue(count)
        print(f'스레드 출력 > {count}')

    # @pyqtSlot()
    def btnStartClicked(self):
        self.worker.start() # 스레드 클래스 안에 있는 run() 함수 실행됨
        self.worker.working = True
        self.worker.count = 0


if __name__ == '__main__':
    app = QApplication(sys.argv)
    ex = qtApp()
    ex.show()
    sys.exit(app.exec_())