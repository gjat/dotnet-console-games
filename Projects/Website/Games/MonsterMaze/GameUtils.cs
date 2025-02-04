using System;
using System.Threading.Tasks;

namespace Website.Games.MonsterMaze;

public static class GameUtils
{
    public static char[,] ConvertToCharMaze(bool[,] maze, char wallCharacter = '#')
    {
        var result = new char[maze.GetLength(0), maze.GetLength(1)];
        for (int i = 0; i < maze.GetLength(0); i++)
        {
            for (int j = 0; j < maze.GetLength(1); j++)
            {
                result[i, j] = maze[i, j] ? ' ' : wallCharacter;
            }
        }
        return result;
    }

    public static async Task<bool> WaitForEscapeOrSpace(BlazorConsole console)
    {
        var key = await console.ReadKey(true);
        while(key.Key != ConsoleKey.Spacebar && key.Key != ConsoleKey.Escape) 
        {
            key = await console.ReadKey(true);
        }
        return key.Key == ConsoleKey.Escape;
    }
}

