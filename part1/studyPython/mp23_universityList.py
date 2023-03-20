# 전국 대학교 리스트
# pandas -> 빅데이터 분석할 때 사용하는 모듈 중 하나
# pip install pandas
import pandas as pd

filePath = './studyPython/university_list_2020.xlsx'
df_excel = pd.read_excel(filePath, engine='openpyxl')
df_excel.columns = df_excel.loc[4].tolist()
df_excel = df_excel.drop(index=list(range(0, 5))) # 실제 데이터 이외의 행을 삭제한 것

print(df_excel.head())           # 상위 5개 출력

print(df_excel['학교명'].values) # 학교명만 출력됨
print(df_excel['주소'].values)   # 주소만 출력됨
