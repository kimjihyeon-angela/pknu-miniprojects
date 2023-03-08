# NaverApi 클래스 - OpenAPI = 인터넷을 통해 데이터를 전달받는 것
from urllib.request import Request, urlopen
from urllib.parse import quote
import datetime    # 현재시간 사용
import json        # 결과는 json으로 return

class NaverApi:
    # 생성자
    def __init__(self) -> None:
        print(f'[{datetime.datetime.now()}] NaverAPI 요청 성공')

    # Naver API 요청 함수(핵심)
    def get_request_url(self, url):
        req = Request(url)
        # Naver API 개인별 인증
        req.add_header('X-Naver-Client-Id', 'BTKkKrKQkM8oPFPm5uB2')
        req.add_header('X-Naver-Client-Secret', '0AO6Z7uf85')

        try:
            res = urlopen(req)       # 요청 결과가 바로 돌아옴
            if res.getcode() == 200: # response OK
                print(f'[{datetime.datetime.now()}] NaverAPI 요청 성공')
                return res.read().decode('utf-8')
            else:
                print(f'[{datetime.datetime.now()}] NaverAPI 요청 실패')
                return None

        except Exception as e:
            print(f'[{datetime.datetime.now()}] 예외발생 : {e}')
            return None

    # 실제 호출함수(핵심)
    def get_naver_search(self, node, search, start, display):
        base_url = 'https://openapi.naver.com/v1/search'
        node_url = f'/{node}.json'
        params = f'?query={quote(search)}&start={start}&display={display}'

        url = base_url + node_url + params
        retData = self.get_request_url(url)

        if retData == None:
            return None
        else:
            return json.loads(retData)  # json으로 return