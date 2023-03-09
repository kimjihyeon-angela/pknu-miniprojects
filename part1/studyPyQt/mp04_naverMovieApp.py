# Qt Designer 디자인 사용
import sys 
from PyQt5 import uic
from PyQt5.QtWidgets import *
from NaverApi import *
from PyQt5.QtGui import *  # QIcon
from urllib.request import urlopen
import webbrowser # 웹브라우저 모듈

class qtApp(QWidget):
    def __init__(self):
        super().__init__()
        uic.loadUi('./studyPyQt/naverApiMovie.ui', self)
        self.setWindowIcon(QIcon('./studyPyQt/movie.png'))

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
        url = self.tblResult.item(selected, 5).text()
        webbrowser.open(url) # 네이버영화 웹 사이트 오픈

    def txtSearchReturned(self):
        self.btnSearchClicked()
    
    def btnSearchClicked(self):
        search = self.txtSearch.text()

        if search == '':
            QMessageBox.warning(self, '경고', '영화명을 입력하세요.')
            return
        else:
            api = NaverApi() # NaverApi 클래스 객체 생성
            node = 'movie'    
            display = 100    # 몇 개 출력할 것인가
            
            result = api.get_naver_search(node, search, 1, display)
            # print(result)
            # 테이블위젯에 출력하는 기능
            items = result['items'] # 전체 json 결과 중 items 아래 배열만 추출
            # # print(len(items))
            self.makeTable(items)   

    # 테이블 위젯에 데이터들을 할당하는 함수 --> 네이버 영화 결과에 맞게 변경해야함
    def makeTable(self, items) -> None:
        self.tblResult.setSelectionMode(QAbstractItemView.SingleSelection) # 단일선택만 할 수 있도록 하는 옵션
        self.tblResult.setColumnCount(7) # 컬럼수
        self.tblResult.setRowCount(len(items)) 
        self.tblResult.setHorizontalHeaderLabels(['영화 제목', '개봉년도', '감독', '배우진', '평점', '영화 링크', '포스터'])
        self.tblResult.setColumnWidth(0,150)
        self.tblResult.setColumnWidth(1, 70)  # 개봉년도
        self.tblResult.setColumnWidth(4, 50)  # 평점
        # 컬럼 데이터를 수정할 수 없도록 하는 옵션
        self.tblResult.setEditTriggers(QAbstractItemView.NoEditTriggers)

        # 테이블 위젯에 데이터 넣기
        for i, post in enumerate(items): # 0, 영화...
            # 영화 제목
            title = self.replaceHtmlTag(post['title']) # HTML 특수문자 변환 / 영어제목 가져오기 추가해야함
            subtitle = post['subtitle']
            title = f'{title}\n({subtitle})'
            # 개봉년도
            pubDate = post['pubDate']
            # 감독
            director = post['director']
            director = director.replace('|', ',')[:-1]
            # 배우진
            actor = post['actor'].replace('|', ',')[:-1] # [:-1] 파이썬에서만 가능
            # 평점
            userRating = post['userRating']
            # 영화 링크
            link = post['link']
            # 포스터
            img_url = post['image']
            # print(img_url == '')
            # 이미지 추가
            if img_url != '': # 빈값이면 포스터 없음  Json의 경우 빈값은 None이 아닌 ''임
                data = urlopen(img_url).read()  # 네이버 영화에 있는 이미지를 다운받고, 2진데이터(텍스트형태 데이터)로 만들어줌
                image = QImage()                # 이미지를 담을 수 있는 객체
                image.loadFromData(data)
                # QTableWidget 이미지를 바로 넣을 수 없기 때문에 QLabel()에 넣은 뒤 QLabel을 QTableWidget에 할당해야함
                imgLabel = QLabel()
                imgLabel.setPixmap(QPixmap(image))

                # # data 이미지로 저장하기
                # f = open(f'./studyPyQt/temp/image_{i+1}.png', mode='wb') # 파일쓰기
                # f.write(data)
                # f.close()

            # setItem (행, 열, 넣을 데이터)
            # self.tblResult.setItem(i, 0, QTableWidgetItem(str(num)))
            self.tblResult.setItem(i, 0, QTableWidgetItem(title))
            self.tblResult.setItem(i, 1, QTableWidgetItem(pubDate))
            self.tblResult.setItem(i, 2, QTableWidgetItem(director))
            self.tblResult.setItem(i, 3, QTableWidgetItem(actor))
            self.tblResult.setItem(i, 4, QTableWidgetItem(userRating))
            self.tblResult.setItem(i, 5, QTableWidgetItem(link))
            if img_url != '':
                self.tblResult.setCellWidget(i, 6, imgLabel)
                self.tblResult.setRowHeight(i, 110)  # 포스터가 있을 때 셀의 높이 조절
            else:
                self.tblResult.setItem(i, 6, QTableWidgetItem('No Poster!'))
    
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