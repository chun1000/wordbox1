using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;
using System.Globalization;

namespace WordBox
{
    //TTS 기능을 수행하는 객체입니다.
    public class Speech
    {
        private static SpeechSynthesizer tts = null;

        //초기화 하면서 TTS 객체의 기본 설정을 가져옵니다.
        public Speech()
        {
            if (tts == null)
            {
                tts = new SpeechSynthesizer();
                tts.SetOutputToDefaultAudioDevice();

                tts.SelectVoiceByHints(VoiceGender.Neutral, VoiceAge.NotSet, 0, CultureInfo.GetCultureInfo("en-us"));
            }
        }

        //입력된 스트링을 읽는 TTS를 출력합니다.
        public void UseTTS(String str)
        {
            if(tts != null) tts.Speak(str);
        }
    }
}
