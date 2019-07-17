using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Snake
{
    class Program
    {
        static int x = 80;
        static int y = 20;
        static Walls walls;
        static Snake snake;
        static Food food;
        static Timer time;

        static void fun (object st){
            if (walls.Collider(snake.snake.Last()) || snake.Collider(snake.snake.Last())){
                snake.Clear();
                snake = new Snake(x / 2, y / 2, 5);
                walls = new Walls(x, y, '+');
            } else if (snake.Eat(food.food)){
                food.Create();
            } else {
                snake.Move ();
            }
        }
        static void Main(string[] args)
        {
            Console.Clear();
            Console.CursorVisible = false;

            walls = new Walls(x, y, '+');
            food = new Food(x, y, '@');
            food.Create();
            snake = new Snake(x / 2, y / 2, 5);

            time = new Timer (fun, null, 0, 80);

            while (true){
                if (Console.KeyAvailable){
                    ConsoleKeyInfo key = Console.ReadKey();
                    snake.Rotation(key.Key);
                }
            }
        }
    }

    struct Point{
        public int x;
        public int y;
        public char ch;

        public static bool operator == (Point a, Point b) => (a.x == b.x && a.y == b.y);
        public static bool operator != (Point a, Point b) => !(a.x == b.x && a.y == b.y);

        public static implicit operator Point((int, int, char) value) => new Point {x = value.Item1, y = value.Item2, ch = value.Item3};

        public void Put(){
            Console.SetCursorPosition(x, y);
            Console.Write(ch);
        }
        public void Clear(){
            Console.SetCursorPosition(x, y);
            Console.Write(' ');
        }
    }


    class Walls{
        char ch;
        List<Point> wall = new List<Point>();

        public Walls(int x, int y, char ch){
            for (int i = 0; i <= x; i++){
                Point p = (i, 0, ch);
                p.Put();
                wall.Add(p);
                p.y = y;
                p.Put();
                wall.Add(p);
            }

            for (int i = 0; i <= y; i++) {
                Point p = (0, i, ch);
                p.Put();
                wall.Add(p);
                p.x = x;
                p.Put();
                wall.Add(p);
            }
        }

        public bool Collider (Point p) {
            for (int i=0; i < wall.Count; i++){
                if (p == wall[i]){
                    return true;
                }

            }
            return false;
        }

    }

    class Food{
        int x;
        int y;
        char ch;
        public Point food;

        Random random = new Random();

        public Food(int x, int y, char ch){
            this.x = x;
            this.y = y;
            this.ch = ch;
        }

        public void Create(){
            food = (random.Next(2, x - 1), random.Next(2, y - 1), ch);
            food.Put();
        }
    }

    class Snake{
        String direction;
        Point tail;
        public List<Point> snake;
        public Point head;
        public Snake(int x, int y, int length){
            direction = "right";
            snake = new List<Point>();
            for (int i = x - length; i < x; i++){
                Point p = (i, y, '*');
                snake.Add(p);
                p.Put();
            }
        }
       
        public void Move(){
            head = NextPoint();
            snake.Add(head);
            tail = snake.First();
            snake.Remove(tail);
            tail.Clear();
            head.Put();
        }
        public Point NextPoint(){
            Point p = snake.Last();
            switch (direction){
                case "left":
                    p.x -= 1;
                    break;
                case "right":
                    p.x += 1;
                    break;
                case "up":
                    p.y -= 1;
                    break;
                case "down":
                    p.y += 1;
                    break;
            }
            return p;
        }
        public void Rotation(ConsoleKey key){
            switch (direction){
                case "left":
                case "right":
                    if (key == ConsoleKey.DownArrow)
                        direction = "down";
                    else if (key == ConsoleKey.UpArrow)
                        direction = "up";
                    break;
                case "up":
                case "down":
                    if (key == ConsoleKey.LeftArrow)
                        direction = "left";
                    else if (key == ConsoleKey.RightArrow)
                        direction = "right";
                    break;
            }
        }

        public bool Collider (Point p){
            for (int i = snake.Count - 2; i > 0; i--){
                if (snake[i] == p){
                    return true;
                }
            }
            return false;
        }

        public bool Eat (Point p){
            head = NextPoint ();
            if (head == p){
                snake.Add(head);
                head.Put();
                return true;
            }
            return false;
        }

        public void Clear(){
            for (int i = 0; i < snake.Count; i++){
                Console.SetCursorPosition(snake[i].x, snake[i].y);
                Console.Write(' ');
            }
        }
    }
}
