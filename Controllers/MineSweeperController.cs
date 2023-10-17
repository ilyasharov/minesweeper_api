using Microsoft.AspNetCore.Mvc;
using MineSweeper;
using System;
using System.Collections.Generic;

[ApiController]
[Route("api/[controller]")]
public class MinesweeperController : ControllerBase
{
    private Dictionary<Guid, Game> games = new Dictionary<Guid, Game>();
    
    [HttpPost("/new")]
    public ActionResult<Game> CreateNewGame([FromBody] Game newGame){

        int width = newGame.Width;
        int height = newGame.Height;
        int mines_count = newGame.MinesCount;

        try{
            
            if (width <= 0 || height <= 0 || width > 30 || height > 30 ||
                mines_count <= 0 || mines_count >= ((width * height) - 1))
        {
            return BadRequest(new { error = "Invalid parameters." });
        }

        var gameId = Guid.NewGuid(); 
        var game = new Game(width, height, mines_count);
        games[gameId] = game;
         
        var random = new Random();
        for (int i = 0; i < mines_count; i++)
        {
            int row, col;
            do
            {
                row = random.Next(height);
                col = random.Next(width);
            } while (game.IsMine(row, col));

            game.SetMine(row, col);
        }

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (!game.IsMine(i, j))
                {
                    int minesNearby = 0;
                    for (int x = -1; x <= 1; x++)
                    {
                        for (int y = -1; y <= 1; y++)
                        {
                            int newRow = i + x;
                            int newCol = j + y;
                            if (newRow >= 0 && newRow < height && newCol >= 0 && newCol < width && game.IsMine(newRow, newCol))
                            {
                                minesNearby++;
                            }
                        }
                    }
                    game.SetCellNumber(i, j, minesNearby);
                }
            }
        }
            return Ok(game);

        } catch(Exception ex){
            return BadRequest(new { error = "Some error." });
        }
    }

    [HttpPost("/turn")]
    public ActionResult<Game> PlayGame([FromBody] Turn turn){
        
        Guid gameId = turn.gameId;
        int row = turn.row;
        int col = turn.col;
        
        try{

            if (!games.ContainsKey(gameId) || games[gameId].Completed)
        {
            return BadRequest(new { error = "Invalid game ID or game has already completed." });
        }

        var game = games[gameId];
        if (row < 0 || row >= game.Height || col < 0 || col >= game.Width || game.IsCellOpened(row, col))
        {
            return BadRequest(new { error = "Invalid cell or cell already opened." });
        }

        if (game.IsMine(row, col))
        {
            game.Completed = true;
            game.OpenAllMines();
        }
        else
        {
            game.OpenCell(row, col);
            if (game.IsWin())
            {
                game.Completed = true;
                game.MarkAllMines();
            }
        }

        return Ok(game);

        } catch(Exception ex){

            return BadRequest(new { error = "Some error." });
        }
    }
}
