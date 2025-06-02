using UnityEngine;

namespace PedronsaDev.Console.Extra
{
	[CommandGroup("Core")]
	public static class CoreCommands
	{
		[Command("help", "Displays information about the Console")]
		public static void PrintHelp()
		{
			Console.Log("Type 'get_commands' for a list of available commands. \r\nFor any problems with the console message .pedronsa in Discord");
		}

		[Command("version", "Displays the Smart Console version")]
		public static void PrintVersion()
		{
			Console.Log($"{Console.Version}\n");
		}

		[Command("quit", "Quits the application")]
		public static void Quit()
		{
			Application.Quit();
		}
		
		[Command("test_logs", "Tests all kinds of logs available")]
		public static void TestLogs()
		{
			Console.Log("This is a Console Log");
			Console.LogWarning("This is a Console Warning Log");
			Console.LogError("This is a Console Error Log");
			Debug.Log("This is a Unity Engine Log");
			Debug.LogWarning("This is a Unity Engine Warning Log");
			Debug.LogError("This is a Unity Engine Error Log");
		}

		[Command("pause_game", "Pauses the game")]
		public static void PauseGame()
		{
			GameManager.Instance.PauseGame();
			Console.Log("Game Paused");
		}
		
		[Command("resume_game", "Resumes the game")]
		public static void ResumeGame()
		{
			GameManager.Instance.ResumeGame();
			Console.Log("Game Resumed");
		}
	}
}
