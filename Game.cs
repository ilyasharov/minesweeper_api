namespace MineSweeper{
    public class Game{
    
        public Guid GameId { get; }
        public int Width { get; }
        public int Height { get; }
        public int MinesCount { get; }

        public char[][] Field { get; }

        public bool Completed { get; set; } = false;

        private bool[,] mineField; // Массив для хранения информации о минах
        private int[,] numberField; // Массив для хранения информации о номерах в клетках

    public Game(int width, int height, int mines_count){
        
        GameId = Guid.NewGuid();

        this.Width = width;
        this.Height = height;
        this.MinesCount = mines_count;
        
        Field = new char[height][];
        
        for (int i = 0; i < height; i++)
        {
            Field[i] = new char[width];
            for (int j = 0; j < width; j++)
            {
                Field[i][j] = ' ';
            }
        }
        
    }

    // Проверяет, является ли указанная клетка миной
    public bool IsMine(int row, int col)
    {
        return mineField[row, col];
    }

    // Устанавливает указанную клетку как мину
    public void SetMine(int row, int col)
    {
        mineField[row, col] = true;
    }

    // Устанавливает номер в указанной клетке
    public void SetCellNumber(int row, int col, int number)
    {
        numberField[row, col] = number;
    }

    // Открывает все мины на поле
    public void OpenAllMines(){
        
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                if (IsMine(i, j))
                {
                    Field[i][j] = 'X'; // Открываем мину
                }
            }
        }
    }

    // Открывает указанную клетку и, если она пуста, распространяет открытие на соседние пустые клетки
    public void OpenCell(int row, int col){
        
        if (row < 0 || row >= Height || col < 0 || col >= Width || IsCellOpened(row, col))
        {
            return; // Проверка на корректные координаты и на уже открытую клетку
        }

        Field[row][col] = numberField[row, col].ToString()[0]; // Открываем клетку

        if (numberField[row, col] == 0)
        {
            // Если клетка пуста, рекурсивно открываем соседние пустые клетки
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (row + x >= 0 && row + x < Height && col + y >= 0 && col + y < Width)
                    {
                        OpenCell(row + x, col + y);
                    }
                }
            }
        }}

    public bool IsCellOpened(int row, int col)
    {
        return Field[row][col] != ' ';
    }

    // Проверяет, выиграл ли игрок
    public bool IsWin(){
        
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                if (!IsMine(i, j) && !IsCellOpened(i, j))
                {
                    return false; // Не все не мины были открыты, игра продолжается
                }
            }
        }
        return true; // Все не мины были открыты, игрок победил
    }

    // Помечает все мины на поле
    public void MarkAllMines(){
        
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                if (IsMine(i, j))
                {
                    Field[i][j] = 'M'; // Помечаем мину
                }
            }
        }
    }
}
}