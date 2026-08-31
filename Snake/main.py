from tkinter import *
import random

GAME_WIDTH = 700
GAME_HEIGHT = 700
SPEED = 200
SPACE_SIZE = 50
BODY_PARTS = 3
FOOD_COLOR = "red"
SNAKE_COLOR =  "green"
BACKGROUND_COLOR = "black"
direction = "down"
score = 0

class Snake:
    def __init__(self):
        self.body_size = BODY_PARTS
        self.coordenadas = []
        self.squares = []

        for i in range(0, BODY_PARTS):
            self.coordenadas.append([0,0])

        for x, y in self.coordenadas:
            square = canvas.create_rectangle(x, y, x + SPACE_SIZE, y + SPACE_SIZE, fill = SNAKE_COLOR)
            self.squares.append(square)

class Food:

    def __init__(self):
        x = random.randint(0, GAME_WIDTH // SPACE_SIZE - 1) * SPACE_SIZE
        y = random.randint(0, GAME_HEIGHT // SPACE_SIZE - 1) * SPACE_SIZE
        self.coordenadas = [x, y]
        self.circulo = canvas.create_oval(x, y, x + SPACE_SIZE, y + SPACE_SIZE, fill = FOOD_COLOR, tag = "food")

def next_turn(snake, food, score_label):
    global score
    x, y = snake.coordenadas[0]

    if direction == "up":
        y -= SPACE_SIZE
    elif direction == "down":
        y += SPACE_SIZE
    elif direction == "right":
        x += SPACE_SIZE
    elif direction == "left":
        x -= SPACE_SIZE

    snake.coordenadas.insert(0, [x, y])
    square = canvas.create_rectangle(x, y ,x + SPACE_SIZE, y + SPACE_SIZE, fill = SNAKE_COLOR)
    snake.squares.insert(0, square)

    if snake.coordenadas[0] != food.coordenadas:
        del snake.coordenadas[-1]
        canvas.delete(snake.squares[-1])
        del snake.squares[-1]
    else:
        canvas.delete(food.circulo)
        score += 1
        score_label.config(text = f"Score = {score}")
        food = Food()
    if check_colissions(snake):
        game_over()



    ventana.after(SPEED, next_turn, snake, food, score_label)


def change_direction(new_direction):
    global direction
    key = new_direction.keysym

    if key == "Up" and direction != "down":
        direction = "up"
        return
    elif key == "Down" and direction != "up":
        direction = "down"
        return
    elif key == "Right" and direction != "left":
        direction = "right"
        return
    elif key == "Left" and direction != "right":
        direction = "left"
        return
    else:
        pass

def check_colissions(snake):

   x, y = snake.coordenadas[0]

   if x < 0 or x >= GAME_WIDTH:
       return True

   elif y < 0 or y >= GAME_HEIGHT:
       return True

   for body_part in snake.coordenadas[1:]:
       if x == body_part[0] and y == body_part[1]:
           return True
   return False

def game_over():

    canvas.delete("all")
    canvas.create_text(GAME_WIDTH/2, GAME_HEIGHT/2, text= "GAME OVER", fill = "red" )






ventana = Tk()
ventana.title("Snake Game")


score_label = Label(ventana, text = f"Score = {score}", font=("Arial", 14))

score_label.pack()
canvas = Canvas(ventana, bg = BACKGROUND_COLOR, height = GAME_HEIGHT, width = GAME_WIDTH)
canvas.pack()

ventana.bind("<Up>", change_direction)
ventana.bind("<Down>", change_direction)
ventana.bind("<Left>", change_direction)
ventana.bind("<Right>", change_direction)

snake = Snake()
food = Food()
next_turn(snake, food, score_label)


ventana.mainloop()