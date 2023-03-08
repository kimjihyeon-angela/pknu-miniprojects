# Qt Designer 디자인 사용
import sys 
from PyQt5 import uic
from PyQt5.QtWidgets import *
from NaverApi import *
from PyQt5.QtGui import *
import webbrowser # 웹브라우저 모듈

class qtApp(QWidget):
    def __init__(self):
        super().__init__()
        uic.loadUi('./studyPyQt/naverApiSearch.ui', self)
        self.setWindowIcon(QIcon('./studyPyQt/newpaper.png'))

        # 검색 버튼 클릭시크널에 대한 슬롯함수
        self.btnSearch.clicked.connect(self.btnSearchClicked)
        # 텍스트박스 검색어 입력 후 엔터 치면 처리하는 부분
        self.txtSearch.returnPressed.connect(self.txtSearchReturned)
        # 테이블위젯 더블클릭했을 때 링크 연결되도록 처리하는 부분
        self.tblResult.doubleClicked.connect(self.tblResultDoubleClicked)
    
    def tblResultDoubleClicked(self):
        # row = self.tblResult.currentIndex().row()
        # column = self.tblResult.currentIndex().column()
        # print(row, column)
        selected = self.tblResult.currentRow()
        url = self.tblResult.item(selected, 1).text()
        webbrowser.open(url) # 뉴스기사 웹 사이트 오픈

    def txtSearchReturned(self):
        self.btnSearchClicked()
    
    def btnSearchClicked(self):
        search = self.txtSearch.text()

        if search == '':
            QMessageBox.warning(self, '경고', '검색어를 입력하세요.')
            return
        else:
            api = NaverApi() # NaverApi 클래스 객체 생성
            node = 'news'    # movie로 변경하면 영화검색 가능
            outputs = []     # 결과 담을 리스트 변수
            display = 100    # 몇 개 출력할 것인가
            
            result = api.get_naver_search(node, search, 1, display)
            # print(result)
            # 테이블위젯에 출력하는 기능
            items = result['items'] # 전체 json 결과 중 items 아래 배열만 추출
            # print(len(items))
            self.makeTable(items)   

    # 테이블 위젯에 데이터들을 할당하는 함수
    def makeTable(self, items) -> None:
        self.tblResult.setSelectionMode(QAbstractItemView.SingleSelection) # 단일선택만 할 수 있도록 하는 옵션
        self.tblResult.setColumnCount(2)
        self.tblResult.setRowCount(len(items)) # 현재 100개의 행 생성
        self.tblResult.setHorizontalHeaderLabels(['기사 제목', '뉴스 링크'])
        self.tblResult.setColumnWidth(0,310)
        self.tblResult.setColumnWidth(1,260)
        # 컬럼 데이터를 수정할 수 없도록 하는 옵션
        self.tblResult.setEditTriggers(QAbstractItemView.NoEditTriggers)

        # 테이블 위젯에 데이터 넣기
        for i, post in enumerate(items): # 0, 뉴스...
            # 뉴스번호
            # num = i + 1                # i가 0부터 시작하기 때문에 뉴스번호 +1 시켜줌

            # 뉴스 제목
            title = self.replaceHtmlTag(post['title']) # HTML 특수문자 변환
            # 뉴스 링크
            originallink = post['originallink']
            
            # setItem (행, 열, 넣을 데이터)
            # self.tblResult.setItem(i, 0, QTableWidgetItem(str(num)))
            self.tblResult.setItem(i, 0, QTableWidgetItem(title))
            self.tblResult.setItem(i, 1, QTableWidgetItem(originallink))
    
    # HTML 특수문자 변환하는 함수
    def replaceHtmlTag(self, sentence) -> str:
        result = sentence.replace('&lt;', '<')   # lesser then
        result = result.replace('&gt;', '>')     # greater then
        result = result.replace('<b>', '')       # bold (진하게)
        result = result.replace('</b>', '')      # bold (진하게)
        result = result.replace('&apos;', "'")   # apostropy (홑따옴표)
        result = result.replace('&quot;', '"')   # quotation Mark (쌍따옴표)
        # 아래 한줄로 적은것과 같은 결과가 나옴

        # result = sentence.replace('&lt;', '<') .replace('&gt;', '>').replace('<b>', '').replace('</b>', '').replace('&apos;', "'").replace('&quot;', '"')
        # 변환 안된 특수문자 생길때마다 추가해야함

        return result


if __name__ == '__main__':
    app = QApplication(sys.argv)
    ex = qtApp()
    ex.show()
    sys.exit(app.exec_())