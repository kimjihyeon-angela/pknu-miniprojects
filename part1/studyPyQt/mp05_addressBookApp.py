# 주소록 GUI 프로그램 - MySQL 연동
import sys 
from PyQt5 import uic
from PyQt5.QtWidgets import *
from PyQt5.QtGui import *
import pymysql

class qtApp(QMainWindow):
    conn = None
    curIdx = 0          # 현재 데이터 PK

    def __init__(self):
        super().__init__()
        uic.loadUi('./studyPyQt/addressbook.ui', self)
        self.setWindowIcon(QIcon('./studyPyQt/addressbook.png'))
        self.setWindowTitle('주소록 v0.5')

        self.initDB() # DB 초기화

        # 버튼 시그널/슬롯 함수 지정
        self.btnNew.clicked.connect(self.btnNewClicked)
        self.btnSave.clicked.connect(self.btnSaveClicked)
        self.tblAddress.doubleClicked.connect(self.tblAddressDoubleClicked)
        self.btnDel.clicked.connect(self.btnDelClicked)

    def btnDelClicked(self):
        if self.curIdx == 0:
            QMessageBox.warning(self, '경고', '삭제할 데이터를 선택하세요.')
            return # return : 함수를 빠져나감
        else:
            reply = QMessageBox.question(self, '확인', '정말로 삭제하시겠습니까?', QMessageBox.Yes | QMessageBox.No, 
                                        QMessageBox.Yes)
            if reply == QMessageBox.No:
                return
            
            self.conn = pymysql.connect(host='localhost', user='root', password='12345',
                                        db='miniproject', charset='utf8')
            query = 'DELETE FROM addressbook WHERE Idx = %s'
            cur = self.conn.cursor()
            cur.execute(query, (self.curIdx))

            self.conn.commit()
            self.conn.close()

            QMessageBox.about(self, '성공', '데이터를 삭제 완료했습니다.')  
          
            self.initDB()
            self.btnNewClicked()

    
    def btnNewClicked(self): # 신규버튼 클릭시 할 일 정하는 함수
        # 라인에디트 내용 삭제 후 이름에 포커스
        self.txtName.setText('')
        self.txtPhone.setText('')
        self.txtEmail.setText('')
        self.txtAddress.setText('')
        self.txtName.setFocus()
        self.curIdx = 0 # 0 => 신규
        # print(self.curIdx)

    
    def tblAddressDoubleClicked(self): 
        rowIndex = self.tblAddress.currentRow()
        self.txtName.setText(self.tblAddress.item(rowIndex, 1).text())
        self.txtPhone.setText(self.tblAddress.item(rowIndex, 2).text())
        self.txtEmail.setText(self.tblAddress.item(rowIndex, 3).text())
        self.txtAddress.setText(self.tblAddress.item(rowIndex, 4).text())
        self.curIdx = int(self.tblAddress.item(rowIndex,0).text())
        # print(self.curIdx)
    
    def btnSaveClicked(self): # 저장버튼 클릭 시 할 일 정하는 함수
        fullName = self.txtName.text()
        phoneNum = self.txtPhone.text()
        email = self.txtEmail.text()
        address = self.txtAddress.text()

        # print(fullName, phoneNum, email, address)
        # 이름, 전화번호 미입력시 알람 필요함
        if fullName == '' or phoneNum == '':
            QMessageBox.warning(self, '주의', '이름과 핸드폰 번호를 입력하세요!')
            return # 종료, 진행 불가
        else:
            self.conn = pymysql.connect(host='localhost', user='root', password='12345',
                                        db='miniproject', charset='utf8')
            # 신규저장 or 수정을 나눠줘야함
            # curidx -> 0일 경우 신규 저장 / 아닐 경우 -> 수정

            if self.curIdx == 0 : # 네개의 변수 값 받아서 INSERT 쿼리문 만들기
                query = '''INSERT INTO addressbook (FullName, PhoneNum, Email, Address)
                                VALUES (%s, %s, %s, %s)'''
            else: # update 쿼리문
                query = '''UPDATE addressbook
                              SET FullName = %s
                              , PhoneNum = %s
                              , Email = %s
                              , Address = %s
                          WHERE Idx = %s;''' 
            
            cur = self.conn.cursor()
            if self.curIdx == 0:
                cur.execute(query, (fullName, phoneNum, email, address))
            else :
                cur.execute(query,(fullName, phoneNum, email, address, self.curIdx))
            
            self.conn.commit()
            self.conn.close()

            # 저장완료 메시지
            if self.curIdx == 0:
                QMessageBox.about(self, '성공', '저장이 완료되었습니다.')
            else:
                QMessageBox.about(self, '성공', '수정이 완료되었습니다.')

            # QTableWidget 새 데이터가 조회될 수 있도록 만들기
            self.initDB()

            # 라인에디트 내용 지워져야함
            self.btnNewClicked()
            

    def initDB(self):
        self.conn = pymysql.connect(host='localhost', user='root', password='12345',
                                    db='miniproject', charset='utf8')
        cur = self.conn.cursor()
        query = '''SELECT Idx
	                    , FullName
                        , PhoneNum
                        , Email
                        , Address
                     FROM addressbook''' # ''' 멀티라인 문자열 '''
        cur.execute(query)
        rows = cur.fetchall()

        # print(rows)
        self.makeTable(rows)
        self.conn.close() # 프로그램 종료할 때

    def makeTable(self, rows) -> None:
        self.tblAddress.setColumnCount(5)  # setColumnCount -> 대소문자 구분함,,
        self.tblAddress.setRowCount(len(rows))
        self.tblAddress.setSelectionMode(QAbstractItemView.SingleSelection)
        self.tblAddress.setHorizontalHeaderLabels(['번호', '이름', '핸드폰', '이메일', '주소'])
        self.tblAddress.setColumnWidth(0,0)   # 번호 -> 숨김
        self.tblAddress.setColumnWidth(1,70)  # 이름 열 사이즈 : 70
        self.tblAddress.setColumnWidth(2,105) # 핸드폰 열 사이즈 : 105
        self.tblAddress.setColumnWidth(3,175) # 이메일 열 사이즈 : 175
        self.tblAddress.setColumnWidth(4,200) # 주소 열 사이즈 : 200
        self.tblAddress.setEditTriggers(QAbstractItemView.NoEditTriggers) # 컬럼 수정 금지


        for i, row in enumerate(rows):
            # row[0] ~ row[4] 까지 쓸 수 있음
            idx = row[0]
            fullName = row[1]
            phoneNum = row[2]
            email = row[3]
            address = row[4]
            self.tblAddress.setItem(i, 0 , QTableWidgetItem(str(idx)))
            self.tblAddress.setItem(i, 1 , QTableWidgetItem(fullName))
            self.tblAddress.setItem(i, 2 , QTableWidgetItem(phoneNum))
            self.tblAddress.setItem(i, 3 , QTableWidgetItem(email))
            self.tblAddress.setItem(i, 4 , QTableWidgetItem(address))

        self.stbCurrent.showMessage(f'전체 주소록 : {len(rows)} 개')



if __name__ == '__main__':
    app = QApplication(sys.argv)
    ex = qtApp()
    ex.show()
    sys.exit(app.exec_())
