using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PedronsaDev.Console.Components
{
	public class LogMessageSetup : MonoBehaviour
	{
		[SerializeField] private ConsolePreferences m_Preferences;
		[SerializeField] private TextMeshProUGUI m_Text;
		[SerializeField] private TextMeshProUGUI m_AltText;
		[SerializeField] private Image m_Background;

		public GameObject AltTextGameObject => m_AltText.gameObject;
		private Color m_TextColor;

		private void Start()
		{
			m_TextColor = m_Text.color;
		}

		public void SetText(string text, string altText, Color textColor, TMP_FontAsset font, float fontSize)
		{
			m_Text.text = $"{text}";
			m_AltText.text = $"{altText}";
			m_Text.color = textColor;
			m_Text.font = font;
			m_Text.fontSize = fontSize;
		}

		public void EnableAltText()
		{
			if(m_AltText.text.Length > 0)
				AltTextGameObject.SetActive(true);
		}
		
		public void DisableAltText()
		{
			if(m_AltText.text.Length > 0)
				AltTextGameObject.SetActive(false);
		}

		public void HighlightText()
		{
			m_Text.color = m_Preferences.HighlightTextColor;
		}
		
		public void UnhighlightText()
		{
			m_Text.color = m_TextColor;
		}
		
		public void SetTextParameter(ParameterInfo[] parameters)
		{
			string colorHex = ColorUtility.ToHtmlStringRGBA(m_Preferences.AutocompleteParameterTextColor);

			for (int i = 0; i < parameters.Length; i++)
			{
				m_Text.text += $"<i><color=#{colorHex}> {parameters[i].Name}</color></i>";

				if (parameters[i].HasDefaultValue)
				{
					m_Text.text += $"<i><color=#{colorHex}> = {parameters[i].DefaultValue.ToString()}</color></i>";
				}
			}
		}

		public void SetBackgroundColor(Color color)
		{
			m_Background.color = color;
		}

		public void DisableSelection()
		{
			LogMessageSelector selector = gameObject.GetComponent<LogMessageSelector>();

			if (selector != null)
			{
				selector.enabled = false;
			}
		}
	}
}
