using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using Microsoft.Speech;
using Microsoft.Speech.Synthesis;
using System.IO;
using System.Threading;
using Microsoft.Speech.Recognition;
using System.Diagnostics;
using System.Globalization;
using Microsoft.CognitiveServices.Speech;
using OpenCvSharp;
using Tesseract;

namespace _1002파파고
{
    public partial class Form1 : Form
    {
        bool selectLanguage;
        Web wb = new Web();
        #region 객체생성
        TranslateTo t = new TranslateTo();
        DictionarySearch d = new DictionarySearch();
        Microsoft.Speech.Synthesis.SpeechSynthesizer ts = new Microsoft.Speech.Synthesis.SpeechSynthesizer();
        List<string> link = new List<string>();
        List<string> thumlink = new List<string>();
        List<string> Desc = new List<string>();

        private object[] diclist = new Dictionary[200];
        public int listSize { get; private set; }
        public string TLink { get; private set; }

        List<string> TransLog = new List<string>();

        delegate void TextSetCallback(string str);
        delegate void ComboSetCallback(string str);

        Microsoft.Speech.Synthesis.SpeechSynthesizer sSyn = new Microsoft.Speech.Synthesis.SpeechSynthesizer();
        CvCapture capture;      // 카메라의 영상을 받아올 CvCapture
        IplImage src;           // 영상을 출력해 줄 IplImage 
        private string Path { get; set; }

        SpeechRecognitionEngine sre;
        SpeechRecognitionEngine sre2;
        #endregion
        #region 마마고
        private void RecogStop()
        {
            sre2.RecognizeAsyncStop();
        }
        public void initRS()
        {
            try
            {
                sre = new SpeechRecognitionEngine(new CultureInfo("ko-KR"));

                Choices c = new Choices();
                c.Add(new string[] { "마마고" });
                Microsoft.Speech.Recognition.Grammar g = new Microsoft.Speech.Recognition.Grammar(new GrammarBuilder(c));
                sre.LoadGrammar(g);

                //sre.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(sre_SpeechRecognized);
                sre.RequestRecognizerUpdate();
                sre.SpeechRecognized += sre_SpeechRecognized;
                sre.SetInputToDefaultAudioDevice();
                sre.RecognizeAsync(RecognizeMode.Multiple);
            }
            catch (Exception e)
            {
                MessageBox.Show("init RS Error : " + e.ToString());
            }
        }
        private void sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            Microsoft.Speech.Synthesis.SpeechSynthesizer ms = new Microsoft.Speech.Synthesis.SpeechSynthesizer();
            ms.SetOutputToDefaultAudioDevice();
            ms.SelectVoice("Microsoft Server Speech Text to Speech Voice (ko-KR, Heami)");
            switch (e.Result.Text)
            {
                case "마마고":
                    {
                        MaMaState(true);
                        ms.Speak("네 말씀하세요");

                        sre2 = new SpeechRecognitionEngine(new CultureInfo("ko-KR"));

                        Choices c = new Choices();
                        c.Add(new string[] { "입력해줘", "아니야", "영어로번역해줘", "한국어로번역해줘", "일본어로번역해줘",
                            "독일어로번역해줘", "스페인어로번역해줘", "프랑스어로번역해줘",
                            "러시아어로번역해줘", "이탈리아어로번역해줘", "중국어번체로번역해줘", "중국어간체로번역해줘" });
                        Microsoft.Speech.Recognition.Grammar g = new Microsoft.Speech.Recognition.Grammar(new GrammarBuilder(c));
                        sre2.LoadGrammar(g);

                        sre2.RequestRecognizerUpdate();
                        sre2.SpeechRecognized += doWhat;
                        sre2.SetInputToDefaultAudioDevice();
                        sre2.RecognizeAsync(RecognizeMode.Multiple);
                        break;
                    }
            }
            ms.Dispose();
            return;
        }
        private void doWhat(object sender, SpeechRecognizedEventArgs e2)
        {
            Microsoft.Speech.Synthesis.SpeechSynthesizer tts = new Microsoft.Speech.Synthesis.SpeechSynthesizer();
            tts.SetOutputToDefaultAudioDevice();
            tts.SelectVoice("Microsoft Server Speech Text to Speech Voice (ko-KR, Heami)");
            switch (e2.Result.Text)
            {
                case "입력해줘":
                    {
                        tts.Speak("네 입력할게요");
                        RecognizeSpeechAsync().Wait();
                        MaMaState(false);
                        RecogStop();
                        break;
                    }
                case "아니야":
                    {
                        tts.Speak("알겠습니다");
                        MaMaState(false);
                        RecogStop();
                        break;
                    }
                case "한국어로번역해줘":
                    {
                        if (textBox1.Text.Equals(string.Empty))
                        {
                            tts.Speak("번역할 문장을 먼저 입력해주세요");
                            break;
                        }
                        ts.Speak("한국어로 번역해드릴게요");
                        ComboSet("한국어");
                        TranslateAtForm1();
                        MaMaState(false);
                        RecogStop();
                        break;
                    }
                case "영어로번역해줘":
                    {
                        if (textBox1.Text.Equals(string.Empty))
                        {
                            tts.Speak("번역할 문장을 먼저 입력해주세요");
                            break;
                        }
                        tts.Speak("영어로 번역해드릴게요");
                        ComboSet("영어");
                        TranslateAtForm1();
                        MaMaState(false);
                        RecogStop();
                        break;
                    }
                case "일본어로번역해줘":
                    {
                        if (textBox1.Text.Equals(string.Empty))
                        {
                            tts.Speak("번역할 문장을 먼저 입력해주세요");
                            break;
                        }
                        tts.Speak("일본어로 번역해드릴게요");
                        ComboSet("일본어");
                        TranslateAtForm1();
                        MaMaState(false);
                        RecogStop();
                        break;
                    }
                case "독일어로번역해줘":
                    {
                        if (textBox1.Text.Equals(string.Empty))
                        {
                            tts.Speak("번역할 문장을 먼저 입력해주세요");
                            break;
                        }
                        tts.Speak("독일어로 번역해드릴게요");
                        ComboSet("독일어");
                        TranslateAtForm1();
                        MaMaState(false);
                        RecogStop();
                        break;
                    }
                case "스페인어로번역해줘":
                    {
                        if (textBox1.Text.Equals(string.Empty))
                        {
                            tts.Speak("번역할 문장을 먼저 입력해주세요");
                            break;
                        }
                        tts.Speak("스페인어로 번역해드릴게요");
                        ComboSet("스페인어");
                        TranslateAtForm1();
                        MaMaState(false);
                        RecogStop();
                        break;
                    }
                case "프랑스어로번역해줘":
                    {
                        if (textBox1.Text.Equals(string.Empty))
                        {
                            tts.Speak("번역할 문장을 먼저 입력해주세요");
                            break;
                        }
                        tts.Speak("프랑스어로 번역해드릴게요");
                        ComboSet("프랑스어");
                        TranslateAtForm1();
                        MaMaState(false);
                        RecogStop();
                        break;
                    }
                case "러시아어로번역해줘":
                    {
                        if (textBox1.Text.Equals(string.Empty))
                        {
                            tts.Speak("번역할 문장을 먼저 입력해주세요");
                            break;
                        }
                        tts.Speak("러시아어로 번역해드릴게요");
                        ComboSet("러시아어");
                        TranslateAtForm1();
                        MaMaState(false);
                        RecogStop();
                        break;
                    }
                case "이탈리아어로번역해줘":
                    {
                        if (textBox1.Text.Equals(string.Empty))
                        {
                            tts.Speak("번역할 문장을 먼저 입력해주세요");
                            break;
                        }
                        tts.Speak("이탈리아어로 번역해드릴게요");
                        ComboSet("이탈리아어");
                        TranslateAtForm1();
                        MaMaState(false);
                        RecogStop();
                        break;
                    }
                case "중국어간체로번역해줘":
                    {
                        if (textBox1.Text.Equals(string.Empty))
                        {
                            tts.Speak("번역할 문장을 먼저 입력해주세요");
                            break;
                        }
                        tts.Speak("중국어간체로 번역해드릴게요");
                        ComboSet("중국어간체");
                        TranslateAtForm1();
                        MaMaState(false);
                        RecogStop();
                        break;
                    }
                case "중국어번체로번역해줘":
                    {
                        if (textBox1.Text.Equals(string.Empty))
                        {
                            tts.Speak("번역할 문장을 먼저 입력해주세요");
                            break;
                        }
                        tts.Speak("중국어번체로 번역해드릴게요");
                        ComboSet("중국어번체");
                        TranslateAtForm1();
                        MaMaState(false);
                        RecogStop();
                        break;
                    }
            }
            tts.Dispose();
            return;
        }
        public async Task RecognizeSpeechAsync()
        {
            var config = SpeechConfig.FromSubscription("aade2640ad4f4650a31c1335d4704c4e", "westus");
            config.SpeechRecognitionLanguage = "ko-KR";

            using (var recognizer = new SpeechRecognizer(config))
            {
                var result = await recognizer.RecognizeOnceAsync();

                if (result.Reason == ResultReason.RecognizedSpeech)
                {
                    TextSet(result.Text);
                }
                else if (result.Reason == ResultReason.NoMatch)
                {
                    MessageBox.Show("NoMatch");
                    MaMaState(false);
                }
                else if (result.Reason == ResultReason.Canceled)
                {
                    var cancellation = CancellationDetails.FromResult(result);
                    MessageBox.Show(cancellation.Reason.ToString());

                    if (cancellation.Reason == CancellationReason.Error)
                    {
                        MessageBox.Show("Error");
                        MaMaState(false);
                    }
                }
            }
            return;
        }
        private void TextSet(string str)
        {
            if(this.InvokeRequired)
            {
                TextSetCallback d = new TextSetCallback(TextSet);
                this.Invoke(d, new object[] { str });
            }
            else
            {
                textBox1.Text = str;
            }
        }
        private void ComboSet(string str)
        {
            if (this.InvokeRequired)
            {
                TextSetCallback d = new TextSetCallback(ComboSet);
                this.Invoke(d, new object[] { str });
            }
            else
            {
                comboBox1.SelectedItem = str;
            }
        }
        private void MaMaState(bool b)
        {
            if(b==true)
            {
                //pictureBox1.Image = Image.FromFile(@"C:\Users\권도훈\Desktop\고급30기\openAPI준비\복원중\C#파파고인식기수정\파파고(TTS) (2)\파파고(TTS)\1002파파고\bin\Debug\on.png");
                pictureBox1.Load("on1.png");
            }
            if(b==false)
            {
                //pictureBox1.Image = Image.FromFile(@"C:\Users\권도훈\Desktop\고급30기\openAPI준비\복원중\C#파파고인식기수정\파파고(TTS) (2)\파파고(TTS)\1002파파고\bin\Debug\off.png");
                pictureBox1.Load("off1.png");
            }
        }
        #endregion
        #region 폼
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            ListLoad();
            LogLoad();

            //ts.SetOutputToDefaultAudioDevice();
            //ts.SelectVoice("Microsoft Server Speech Text to Speech Voice (ko-KR, Heami)");
            MaMaState(false);
            Thread t = new Thread(new ThreadStart(initRS));
            t.IsBackground = true;
            t.Start();

            CameraSet();
            Path = string.Empty;

            HwpPath();

            한국어ToolStripMenuItem.Checked = true;
            selectLanguage = false;
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            ListSave();
            LogSave();

            Cv.ReleaseImage(src);           //ReleaseImage()는 이미지의 메모리 할당을 해제
            if (src != null) src.Dispose(); //Dispose()는 클래스등의 메모리 할당을 해제
        }
        private void 종료ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion
        #region 번역메서드
        //번역기능
        private void Button1_Click(object sender, EventArgs e)
        {
            TranslateAtForm1();

        }

        private void TranslateAtForm1()
        {
            try
            {
                string str = textBox1.Text;
                str = str.Replace(Environment.NewLine, "");  //개행문자 제거
                t.TranslateLanguage(str);
                textBox2.Text = t.TransText;

                string[] strs = new string[] { textBox1.Text, textBox2.Text };

                string tran = string.Format("{0}@{1}#", textBox1.Text, textBox2.Text);
                TransLog.Add(tran);

                ListViewItem lvi = new ListViewItem(strs);
                listView1.Items.Add(lvi);
            }
            catch
            {

            }
        }

        //콤보박스
        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str = comboBox1.SelectedItem.ToString();
            t.ComboLanguage(str);
        }
        //TTS음성출력
        private void Button3_Click(object sender, EventArgs e)
        {
            try
            {
                Thread t = new Thread(new ThreadStart(VoiceSpeak));
                t.IsBackground = true;
                t.Start();
            }
            catch
            {

            }
        }

        private void VoiceSpeak()
        {
            Microsoft.Speech.Synthesis.SpeechSynthesizer tts = new Microsoft.Speech.Synthesis.SpeechSynthesizer();
            tts.SetOutputToDefaultAudioDevice();
            string str = textBox2.Text;

            button3.Enabled = false;
            if (selectLanguage == true)
            {
                button3.Text = "Speaking";
            }
            else if (selectLanguage == false)
            {
                button3.Text = "말하는중";
            }
            

            if (t.Lang.Equals("ko") == true)
            {
                tts.SelectVoice("Microsoft Server Speech Text to Speech Voice (ko-KR, Heami)");
                tts.Speak(str);
            }
            else if (t.Lang.Equals("ja") == true)
            {
                tts.SelectVoice("Microsoft Server Speech Text to Speech Voice (ja-JP, Haruka)");
                tts.Speak(str);
            }
            else if (t.Lang.Equals("en") == true)
            {
                tts.SelectVoice("Microsoft Server Speech Text to Speech Voice (en-US, Helen)");
                tts.Speak(str);
            }
            else if (t.Lang.Equals("de") == true)
            {
                tts.SelectVoice("Microsoft Server Speech Text to Speech Voice (de-De, Hedda)");
                tts.Speak(str);
            }
            else if (t.Lang.Equals("es") == true)
            {
                tts.SelectVoice("Microsoft Server Speech Text to Speech Voice (es-ES, Helena)");
                tts.Speak(str);
            }
            else if (t.Lang.Equals("fr") == true)
            {
                tts.SelectVoice("Microsoft Server Speech Text to Speech Voice (fr-FR, Hortense)");
                tts.Speak(str);
            }
            //else if (t.Lang.Equals("zh-TW") == true)
            //{
            //    ts.SelectVoice("Microsoft Server Speech Text to Speech Voice (zh-TW, HanHan)");
            //    ts.Speak(str);
            //}
            //else if (t.Lang.Equals("zh-CN") == true)
            //{
            //   ts.SelectVoice("Microsoft Server Speech Text to Speech Voice (zh-CN, HuiHui)");
            //    ts.Speak(str);
            //}
            else if (t.Lang.Equals("ru") == true)
            {
                tts.SelectVoice("Microsoft Server Speech Text to Speech Voice (ru-RU, Elena)");
                tts.Speak(str);
            }
            else if (t.Lang.Equals("it") == true)
            {
                tts.SelectVoice("Microsoft Server Speech Text to Speech Voice (it-IT, Lucia)");
                tts.Speak(str);
            }
            else
            {
                tts.SelectVoice("Microsoft Server Speech Text to Speech Voice (en-US, Helen)");
                tts.Speak(str);
            }
            tts.Dispose();
            button3.Enabled = true;

            if (selectLanguage == true)
            {
                button3.Text = "Voice";
            }
            else if (selectLanguage == false)
            {
                button3.Text = "음성";
            }
        }

        //번역로그 저장(List<string>) 바이트방식IO
        private void LogSave()
        {
            string str = string.Empty;
            for (int i = 0; i < TransLog.Count; i++)
            {
                str += TransLog[i];
            }
            Stream ws = new FileStream("log.txt", FileMode.Create);
            byte[] wBytes = Encoding.UTF8.GetBytes(str);

            ws.Write(wBytes, 0, wBytes.Length);
            ws.Dispose();
        }
        //번역로그 로드
        private void LogLoad()
        {
            try
            {
                string str = File.ReadAllText("log.txt");
                string[] result = str.Split('#');
                for (int i = 0; i < result.Length - 1; i++)
                {
                    string[] finalresult = result[i].Split('@');
                    if (finalresult.Length >= 1)
                    {

                        string tran = string.Format("{0}@{1}#", finalresult[0], finalresult[1]);
                        TransLog.Add(tran);

                        ListViewItem lvi = new ListViewItem(finalresult);
                        listView1.Items.Add(lvi);
                    }
                }
            }
            catch
            {

            }
        }
        //번역로그 및 리스트뷰 아이템 삭제
        private void Button6_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            TransLog.Clear();
        }
        //번역로그 선택
        private void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idx = listView1.FocusedItem.Index;
            textBox1.Text = listView1.Items[idx].SubItems[0].Text;
            textBox2.Text = listView1.Items[idx].SubItems[1].Text;
        }
        #endregion
        #region 단어검색메서드
        //검색기능
        private void Button2_Click(object sender, EventArgs e)
        {
            string str = textBox3.Text;
            d.SearchLanguage(str);
            //textBox4.Text = d.XmlString;
            TitlePrint();
        }
        //리스트박스추가
        private void TitlePrint()
        {
            listBox1.Items.Clear();

            foreach (Dictionary dc in d.list)
            {
                //==========================================보기싫은글자 제거
                string text = dc.Title.Replace("<b>", "");
                string title = text.Replace("</b>", "");
                //============================================================
                listBox1.Items.Add(title);
            }
        }
        
        //리스트박스 선택
        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idx = listBox1.SelectedIndex;
            //==========================================보기싫은글자 제거
            string title = d.list[idx].Title.Replace("<b>", "");
            string text = title.Replace("</b>", "");
            //============================================================
            //최근 검색한 목록 추가..!
            listBox2.Items.Add(text);
            link.Add(d.list[idx].Link);
            thumlink.Add(d.list[idx].Thumbnail);
            Desc.Add(d.list[idx].Description);
            //Searchlist.Add(d.list[idx]);
            //============================================================
            TLink = d.list[idx].Thumbnail;
            //===============================================================웹창
            if (wb.IsDisposed)
            {
                wb = new Web();
            }

            wb.LinkSet(d.list[idx].Link);
            foreach (Form form in Application.OpenForms)
            {
                if (form.Name == "Web")
                {
                    if (form.WindowState == FormWindowState.Minimized)
                    {
                        form.WindowState = FormWindowState.Normal;
                    }
                    wb.LinkLoad();
                    form.Activate();
                    return;
                }
            }
            wb.Show();
        }
        //최근 검색한 단어 선택
        private void ListBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idx = listBox2.SelectedIndex;
            //============================================================
            TLink = thumlink[idx];
            //============================================================웹
            if(wb.IsDisposed)
            {
                wb = new Web();
            }

            wb.LinkSet(link[idx]);
            foreach (Form form in Application.OpenForms)
            {
                if (form.Name == "Web")
                {
                    if (form.WindowState == FormWindowState.Minimized)
                    {
                        form.WindowState = FormWindowState.Normal;
                    }
                    wb.LinkLoad();
                    form.Activate();
                    return;
                }
            }
            wb.Show();
        }
        //최근 검색한 단어장 비우기
        private void Button4_Click(object sender, EventArgs e)
        {
            //리스트 저장한것들 비우기...
            link.Clear();
            thumlink.Clear();
            //리스트박스 비우기
            listBox2.Items.Clear();
        }
        //검색단어장 저장(List<Dictionary>) 직렬화방식
        private void ListSave()
        {
            for (int i = 0; i < listBox2.Items.Count; i++)
            {
                Dictionary d = new Dictionary(listBox2.Items[i].ToString(), link[i], Desc[i], thumlink[i]);
                dicinsert(d);
            }
            wbList.filesersave(diclist, listSize);
        }
        //검색단어장 저장 배열늘리기
        public void dicinsert(object dic)
        {
            if (listSize >= 200)
            {
                if (selectLanguage == true)
                {
                    throw new Exception("Insufficient storage space");
                }
                else if (selectLanguage == false)
                {
                    throw new Exception("저장 공간 부족");
                }
            }
            diclist[listSize] = dic;
            listSize++;
        }
        //검색단어장 로드
        private void ListLoad()
        {
            try
            {
                int max;
                Dictionary[] arr1 = wbList.fileserload(out max);
                //arr = new wbArr(max);
                for (int i = 0; i < arr1.Length; i++)
                {
                    listBox2.Items.Add(arr1[i].Title);
                    link.Add(arr1[i].Link);
                    thumlink.Add(arr1[i].Thumbnail);
                    Desc.Add(arr1[i].Description);
                }
            }
            catch (Exception)
            {

            }
        }
        #endregion
        #region 이미지스캔
        public void imgscan(Image file)
        {
            try
            {
                Bitmap img = new Bitmap(file);
                if (radioButton1.Checked == true)//한국어 선택한 경우..
                {
                    var ocr = new TesseractEngine("./tessdata", "kor", EngineMode.Default);
                    var texts = ocr.Process(img);
                    textBox4.Text = texts.GetText();
                }
                else if (radioButton2.Checked == true)//영어 선택한 경우..
                {
                    var ocr = new TesseractEngine("./tessdata", "eng", EngineMode.TesseractAndCube);
                    var texts = ocr.Process(img);
                    textBox4.Text = texts.GetText();
                }
                else
                {
                    if(selectLanguage==true)
                    {
                        MessageBox.Show("Please select a language to translate");
                    }
                    else if(selectLanguage==false)
                    {
                        MessageBox.Show("번역할 언어를 선택해주세요");
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void CameraSet()
        {
            try
            {
                capture = CvCapture.FromCamera(CaptureDevice.DShow, 0);
                // CvCapture.FromCamera(CaptureDevice.DShow, 0);에서 0은 카메라의 장치 번호
                // 만약 카메라가 내부,외부 카메라 두개가 있을경우 1로 변경하면 외부 카메라를 이용
                capture.SetCaptureProperty(CaptureProperty.FrameWidth, 501);
                capture.SetCaptureProperty(CaptureProperty.FrameHeight, 495);

                //capture.SetCaptureProperty는 영상의 너비와 높이
            }
            catch
            {
                timer1.Enabled = false;
            }
        }
        private void TextSpeak()
        {
            if (radioButton1.Checked == false && radioButton2.Checked == false)
            {
                if (selectLanguage == true)
                {
                    MessageBox.Show("Please select a language");
                }
                else if (selectLanguage == false)
                {
                    MessageBox.Show("언어를 선택해주세요");
                }
                return;
            }
            ButtonSet(false);
            button5.Text = "말하는중";
            sSyn.Speak(textBox4.Text);
            ButtonSet(true);
            button5.Text = "음성";
        }
        private void ButtonSet(bool b)
        {
            button8.Enabled = b;
            button5.Enabled = b;
            button7.Enabled = b;
        }
        private void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            sSyn.SelectVoice("Microsoft Server Speech Text to Speech Voice (ko-KR, Heami)");
            sSyn.SetOutputToDefaultAudioDevice();
        }

        private void RadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            sSyn.SelectVoice("Microsoft Server Speech Text to Speech Voice (en-US, Helen)");
            sSyn.SetOutputToDefaultAudioDevice();
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            try
            {
                //디렉토리 설정
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.ShowDialog();
                string file = dlg.FileName;
                //이미지 저장
                Image img = Image.FromFile(file);
                pictureBox2.Image = img;
                imgscan(img);
            }
            catch
            {

            }
        }
        private void Button5_Click(object sender, EventArgs e)
        {
            Thread th = new Thread(new ThreadStart(TextSpeak));
            th.IsBackground = true;
            th.Start();
        }
        private void Button7_Click(object sender, EventArgs e)
        {
            string save_name = DateTime.Now.ToString("yyyy-MM-dd-hh시mm분ss초");
            Cv.SaveImage(save_name + ".jpg", src);
            string name = save_name + ".jpg";

            pictureBox2.Load(name);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;

            Bitmap img = new Bitmap(pictureBox2.Image);

            if (radioButton1.Checked == true)
            {
                var ocr = new TesseractEngine("./tessdata", "kor", EngineMode.Default);
                var texts = ocr.Process(img);
                textBox4.Text = texts.GetText();
            }
            else if (radioButton2.Checked == true)
            {
                var ocr = new TesseractEngine("./tessdata", "eng", EngineMode.TesseractAndCube);
                var texts = ocr.Process(img);
                textBox4.Text = texts.GetText();
            }
            else
            {
                if (selectLanguage == true)
                {
                    MessageBox.Show("Please select a language to translate");
                }
                else if (selectLanguage == false)
                {
                    MessageBox.Show("번역할 언어를 선택해주세요");
                }
            }
        }
        private void Timer1_Tick(object sender, EventArgs e)
        {
            src = capture.QueryFrame();
            pictureBoxIpl1.ImageIpl = src;  //영상을 초마다 보여줌
        }
        #endregion
        #region 한글문서
        private void TextBox1_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                axHwpCtrl1.Open(files[0]); // hwp_path 한글파일 경로
                axHwpCtrl1.ReadOnlyMode = true; //수정불가능 설정
                string hwp_text = axHwpCtrl1.GetTextFile("TEXT", "").ToString();
                //html에서 텍스트 부분을 불러온다. 불러온 텍스트를 hwp_text(string) 저장
                textBox1.Text = hwp_text; // 텍스트박스 출력
            }
            catch
            {

            }

        }
        private void TextBox1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;//끌어온 아이콘 변경
        }
        private void HwpPath()
        {
            string HNCRoot = @"HKEY_Current_User\Software\HNC\HwpCtrl\Modules";

            try
            {
                // 보안모듈 레지스트리에 등록되어 있는지 확인
                if (Microsoft.Win32.Registry.GetValue(HNCRoot, "FilePathCheckerModuleExample", "Not Exist").Equals("Not Exist"))
                {
                    // 등록되어 있지 않을경우 레지스트리에 등록
                    Microsoft.Win32.Registry.SetValue(HNCRoot, "FilePathCheckerModuleExample", Environment.CurrentDirectory + "\\FilePathCheckerModuleExample.dll");
                }
                axHwpCtrl1.RegisterModule("FilePathCheckDLL", "FilePathCheckerModuleExample");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion
        #region 언어선택
        private void 영어ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            한국어ToolStripMenuItem.Checked = false;
            영어ToolStripMenuItem.Checked = true;
            selectLanguage = true;
            this.Text = "MaMaGo";
            button1.Text = "Translation";
            button3.Text = "Voice";
            tabControl1.TabPages[0].Text = "Translator";
            tabControl1.TabPages[1].Text = "ImageScan";
            listView1.Columns[0].Text = "Before Translation";
            listView1.Columns[1].Text = "After Translation";
            button6.Text = "Delete Translation Record";
            label5.Text = "Search Word";
            label7.Text = "Recently Searched Word";
            button2.Text = "Search";
            button4.Text = "Delete Search Record";
            프로그램ToolStripMenuItem.Text = "Program";
            종료ToolStripMenuItem.Text = "Exit";
            언어ToolStripMenuItem.Text = "Language";
            영어ToolStripMenuItem.Text = "English";
            한국어ToolStripMenuItem.Text = "Korean";
            radioButton1.Text = "Korean";
            radioButton2.Text = "English";
            button8.Text = "Load Image";
            button5.Text = "Voice";
            button7.Text = "Capture";

            comboBox1.Items.Clear();
            comboBox1.Items.Add("Korean");
            comboBox1.Items.Add("English");
            comboBox1.Items.Add("Japanese");
            comboBox1.Items.Add("German");
            comboBox1.Items.Add("Chinese");
            comboBox1.Items.Add("Spanish");
            comboBox1.Items.Add("French");
            comboBox1.Items.Add("Russian");
            comboBox1.Items.Add("Italian");
        }
        private void 한국어ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            한국어ToolStripMenuItem.Checked = true;
            영어ToolStripMenuItem.Checked = false;
            selectLanguage = false;
            this.Text = "마마고";
            button1.Text = "번역";
            button3.Text = "음성";
            tabControl1.TabPages[0].Text = "번역기";
            tabControl1.TabPages[1].Text = "이미지번역";
            listView1.Columns[0].Text = "번역 전";
            listView1.Columns[1].Text = "번역 결과";
            button6.Text = "번역기록삭제";
            label5.Text = "검색할 단어";
            label7.Text = "최근 검색한 단어";
            button2.Text = "단어검색";
            button4.Text = "단어장비우기";
            프로그램ToolStripMenuItem.Text = "프로그램";
            종료ToolStripMenuItem.Text = "종료";
            언어ToolStripMenuItem.Text = "언어";
            영어ToolStripMenuItem.Text = "영어";
            한국어ToolStripMenuItem.Text = "한국어";
            radioButton1.Text = "한국어";
            radioButton2.Text = "영어";
            button8.Text = "이미지불러오기";
            button5.Text = "음성";
            button7.Text = "캡처";

            comboBox1.Items.Clear();
            comboBox1.Items.Add("한국어");
            comboBox1.Items.Add("영어");
            comboBox1.Items.Add("일본어");
            comboBox1.Items.Add("독일어");
            comboBox1.Items.Add("중국어간체");
            comboBox1.Items.Add("중국어번체");
            comboBox1.Items.Add("스페인어");
            comboBox1.Items.Add("프랑스어");
            comboBox1.Items.Add("러시아어");
            comboBox1.Items.Add("이탈리아어");
        }
        #endregion
    }
}