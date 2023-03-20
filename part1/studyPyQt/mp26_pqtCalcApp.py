# 계산기 앱 만들기
import sys 
from PyQt5 import uic
from PyQt5.QtWidgets import *
from PyQt5.QtGui import *
import pymysql

class qtApp(QWidget):
    def __init__(self):
        super().__init__()
        uic.loadUi('./studyPyQt/calculator.ui', self)
        self.setWindowTitle('계산기 v0.5')
        self.setWindowIcon(QIcon('./studyPyQt/calculator.png'))

        # 시그널 16개의 슬롯함수 : 1개 -> btnClicked
        self.btn_C.clicked.connect(self.btnClicked)
        self.btn_number0.clicked.connect(self.btnClicked)
        self.btn_number1.clicked.connect(self.btnClicked)
        self.btn_number2.clicked.connect(self.btnClicked)
        self.btn_number3.clicked.connect(self.btnClicked)
        self.btn_number4.clicked.connect(self.btnClicked)
        self.btn_number5.clicked.connect(self.btnClicked)
        self.btn_number6.clicked.connect(self.btnClicked)
        self.btn_number7.clicked.connect(self.btnClicked)
        self.btn_number8.clicked.connect(self.btnClicked)
        self.btn_number9.clicked.connect(self.btnClicked)
        self.btn_result.clicked.connect(self.btnClicked)
        self.btn_add.clicked.connect(self.btnClicked)
        self.btn_divide.clicked.connect(self.btnClicked)
        self.btn_minus.clicked.connect(self.btnClicked)
        self.btn_multipy.clicked.connect(self.btnClicked)

        self.txt_view.setEnabled(False)
        self.text_value = ''

    def btnClicked(self):
        btn_val = self.sender().text()
        if btn_val == 'C':
            print('Clear')
            self.txt_view.setText('0')
            self.text_value = ''
        elif btn_val == '=': # 계산결과
            print('=')
            try:
                result = eval(self.text_value.lstrip('0'))
                print(round(result, 4)) # 10 / 6 할 경우 소수점 4자리까지 나오도록 만들어줌 round
                self.txt_view.setText(str(round(result, 4)))
            except:
                self.txt_view.setText('ERROR')
        else:
            if btn_val == 'X':
                btn_val = '*'
            self.text_value += btn_val
            print(self.text_value) 
            self.txt_view.setText(self.text_value)       


if __name__ == '__main__':
    app = QApplication(sys.argv)
    ex = qtApp()
    ex.show()
    sys.exit(app.exec_())