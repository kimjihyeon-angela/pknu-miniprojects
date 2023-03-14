# 암호해제 앱 (무차별대입공격)
# itertools
import itertools
import time
import zipfile

passwd_String = '0123456789' 

# 패스워드에 영문자도 들어있을 경우
# passwd_String = '0123456789abcdefg.........xyzABCD....XYZ' 

file = zipfile.ZipFile('./studyPython/passwordZip.zip')

isFind = False # 암호를 찾았는지 확인

for i in range(4, 5):
    attempts = itertools.product(passwd_String, repeat=i)
    for attempt in attempts:
        try_pass=''.join(attempt)
        print(try_pass)
        # time.sleep(0.3)
        try:
            file.extractall(pwd=try_pass.encode(encoding='utf-8'))
            print(f'암호는 {try_pass} 입니다.')
            isFind = True
            break
        except:
            pass

    if isFind == True:
        break