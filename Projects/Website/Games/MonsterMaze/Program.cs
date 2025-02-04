using System;
using System.Threading.Tasks;

namespace Website.Games.MonsterMaze;

public class Program
{
	public readonly BlazorConsole Console = new();

    // Save console colours, to restore state after the game ends.
    private ConsoleColor originalBackgroundColor;
    private ConsoleColor originalForegroundColor;
    
    public async Task Run()
    {    
        Console.CursorVisible = false;
        //Console.CancelKeyPress += new ConsoleCancelEventHandler(CleanupHandler);
        
        originalBackgroundColor = Console.BackgroundColor;
        originalForegroundColor = Console.ForegroundColor;

        var maxWidth = Console.WindowWidth > 50 ? 50 : Console.WindowWidth-1;
        var maxHeight = Console.WindowHeight > 24 ? 24: Console.WindowHeight-2;

        var game = new MonsterMazeGame(maxWidth, maxHeight, Console);

        bool quitGame = false;
        while(!quitGame)
        {
            await ShowTitleScreen();
            
            if(await GameUtils.WaitForEscapeOrSpace(Console) != true)
            {
                bool gameOver = false;
                for(int levelNumber = 1; levelNumber <= MonsterMazeGame.MaxLevel && !gameOver; levelNumber++)
                {
                    gameOver = await game.PlayLevel(levelNumber);
                }
            }
            else
            {
                // Player wants to quit the game
                quitGame = true;
            }
        }
        await CleanupHandler(null, null);
    }

    protected async Task ShowTitleScreen()
    {
        await Console.Clear();
        
        await Console.SetCursorPosition(Console.WindowWidth/2-20, 5);
        Console.ForegroundColor = ConsoleColor.Red;
        await Console.Write("### ");
        Console.ForegroundColor = ConsoleColor.Yellow;
        await Console.Write("Monster Maze");
        Console.ForegroundColor = ConsoleColor.Red;
        await Console.Write(" ###");

        await Console.SetCursorPosition(0, 10);
        Console.ForegroundColor = ConsoleColor.White;
        await Console.WriteLine("You are trapped in a maze with monsters. Your goal is to escape.");
        await Console.WriteLine("Use the arrow keys to move, avoid the monsters.");
        await Console.WriteLine();
        await Console.WriteLine("Press space to start, or escape to quit.");
    }

    // If "escape" or "control-c" is pressed, try to get the console window back into a clean state.
    protected async Task CleanupHandler(object? sender, ConsoleCancelEventArgs? args)
    {
        Console.ForegroundColor = originalForegroundColor;
        Console.BackgroundColor = originalBackgroundColor;
        await Console.Clear();
    }
}
