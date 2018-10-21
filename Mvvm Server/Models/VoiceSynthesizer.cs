using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;

namespace Mvvm_Server.Models
{
	class VoiceSynthesizer
	{
		SpeechSynthesizer mySpeechSynth = new SpeechSynthesizer();
		Choices lst = new Choices();
		SpeechRecognitionEngine myEngine;

		public VoiceSynthesizer()
		{
			SetupSynthesizer();
		}

		public void MServer_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			string propertyName = e.PropertyName;
			SpeakAsync(propertyName);
		}

		private void SpeakAsync(string state)
		{
			mySpeechSynth.SpeakAsync(String.Format("{0} State", state));
		}

		private void SetupSynthesizer()
		{
			myEngine = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-us"));
			string[] arr = new string[] { "Hi", "hello" };
			//myGrammar = new Grammar(new GrammarBuilder(lst//, SubsetMatchingMode.SubsequenceContentRequired);//(new GrammarBuilder(lst));
			lst.Add(arr);
			foreach (string item in arr)
			{
				GrammarBuilder gb = new GrammarBuilder(item, SubsetMatchingMode.SubsequenceContentRequired);
				Grammar myGrammar = new Grammar(gb);
				myGrammar.Enabled = true;
				myEngine.LoadGrammarAsync(myGrammar);
			}

			mySpeechSynth.SelectVoice("Microsoft David Desktop");

			//mySpeechSynth.SpeakAsync("Welcome,back, Ishay.");
		}
	}
}
