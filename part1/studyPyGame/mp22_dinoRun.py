# dinoRun 
import pygame
import os

pygame.init()

ASSETS = './studyPyGame/Assets/'
SCREEN = pygame.display.set_mode((1100, 600))

# 아이콘
icon = pygame.image.load('./studypyGame/dinoRun2.png')
pygame.display.set_icon(icon)

# 배경이미지 로드
BG = pygame.image.load(os.path.join(f'{ASSETS}Other', 'Track.png'))

# 공룡 이미지 로드
RUNNING = [pygame.image.load(f'./studyPyGame/Assets/Dino/DinoRun1.png'),
           pygame.image.load(f'{ASSETS}Dino/DinoRun2.png')]

DUCKING = [pygame.image.load(f'{ASSETS}Dino/DinoDuck1.png'),
           pygame.image.load(f'{ASSETS}Dino/DinoDuck2.png')]

JUMPING = pygame.image.load(f'{ASSETS}Dino/DinoJump.png')

'''
이미지 로드하는 방법
1. ('./경로적기.png')
2. os.path.join(f'{큰 경로}'나머지, .png)
3. (f'{큰 경로}나머지경로.png')
'''

# 공룡 클래스
class Dino: 
    X_POS = 80
    Y_POS = 310
    Y_POS_DUCK = 340
    JUMP_VEL = 9.0

    def __init__(self) -> None:
        # 공룡 상태
        self.run_img = RUNNING
        self.duck_img = DUCKING
        self.jump_img = JUMPING

        self.dino_run = True
        self.dino_duck = False
        self.dino_jump = False

        self.step_index = 0
        self.jump_vel = self.JUMP_VEL # 점프 초기값 : 9.0
        self.image = self.run_img[0]
        self.dino_rect = self.image.get_rect() # 이미지의 사각형 정보
        self.dino_rect.x = self.X_POS
        self.dino_rect.y = self.Y_POS 

    def update(self, userInput) -> None:
        if self.dino_run:
            self.run()
        elif self.dino_duck:
            self.duck()
        elif self.dino_jump:
            self.jump()

        if self.step_index >= 10:
            self.step_index = 0  # 애니메이션 스텝을 위해 사용

        if userInput[pygame.K_UP] and not self.dino_jump: # 점프
            self.dino_run = False
            self.dino_duck = False
            self.dino_jump = True

        elif userInput[pygame.K_DOWN] and not self.dino_jump : # 숙이기
            self.dino_run = False
            self.dino_duck = True
            self.dino_jump = False
        
        elif not(self.dino_jump or userInput[pygame.K_DOWN]): # 점프, 숙이기 상황 아닌 경우 -> 달리기
            self.dino_run = True
            self.dino_duck = False
            self.dino_jump = False

    def run(self):
        self.image = self.run_img[self.step_index // 5] # run_img / step 이 10까지 있음 0 ~ 4 ->0 /  5 -> 1 / 6 ~ 9 -> 0 / 10 -> 2
        self.dino_rect = self.image.get_rect()
        self.dino_rect.x = self.X_POS
        self.dino_rect.y = self.Y_POS 
        self.step_index += 1

    def duck(self):
        self.image = self.duck_img[self.step_index // 5] # duck_img
        self.dino_rect = self.image.get_rect()
        self.dino_rect.x = self.X_POS
        self.dino_rect.y = self.Y_POS_DUCK  # duck은 이미지 높이가 낮아져야 하기 때문에 Y_POS_DUCK 사용 
        self.step_index += 1

    def jump(self):
        self.image = self.jump_img
        if self.dino_jump:
            self.dino_rect.y -= self.jump_vel * 4
            self.jump_vel -= 0.8
        if self.jump_vel < -self.JUMP_VEL: # -9.0 이 되면 점프 중단
            self.dino_jump = False
            self.jump_vel = self.JUMP_VEL # 이걸 안해줄 경우 공룡이 아래로 내려감

    def draw(self, SCREEN) -> None:
        SCREEN.blit(self.image, (self.dino_rect.x, self.dino_rect.y))

# 메인함수
def main():
    run = True
    clock = pygame.time.Clock()
    dino = Dino()

    while run:
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                run = False

        SCREEN.fill((255, 255, 255)) # 배경 흰색
        userInput = pygame.key.get_pressed()

        dino.draw(SCREEN) # 공룡을 게임스크린에 그리기
        dino.update(userInput)

        clock.tick(30) 
        pygame.display.update() # fps 초당 프레임 수 => 초당 30번 update 수행

if __name__ == '__main__':
    main()
