using System.Reflection;
using PedronsaDev.Console.Components;

namespace PedronsaDev.Console
{
	public class LogMessage
	{
		public string Text;
		public string AltText;
		public LogMessageType Type;
		public ParameterInfo[] Parameters;
		public LogMessageSetup LogMessageSetup { get; set; }
		public LogMessageSelector LogMessageSelector { get; set; }

		public LogMessage(string text,string altText, LogMessageType type)
		{
			Text = text;
			AltText = altText;
			Type = type;
			Parameters = null;
		}

		public LogMessage(string text,string altText, LogMessageType type, ParameterInfo[] parameters)
		{
			Text = text;
			AltText = altText;
			Type = type;
			Parameters = parameters;
		}

		public static implicit operator string(LogMessage logMessage)
		{
			return $"{logMessage.Type}:{logMessage.Text}";
		}
	}
}
