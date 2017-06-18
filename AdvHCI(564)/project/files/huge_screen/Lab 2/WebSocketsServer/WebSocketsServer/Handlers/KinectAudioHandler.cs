using Microsoft.Kinect;
using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Recognition;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeMiniServer.Beans;
using WebSocketsServer.Interfaces;

namespace OfficeMiniServer.Handlers
{

    /// <summary>
    /// Kinect Audio Holder Singleton class.
    /// Used code from: http://kin-educate.blogspot.gr/2012/06/speech-recognition-for-kinect-easy-way.html
    /// </summary>
    public class KinectAudioHandler : MessageObserver
    {
        //Create an instance of your kinect sensor
        public KinectSensor               CurrentSensor    { get; private set; }

        public List<string>               WordsToRecognize { get; private set; }
        //and the speech recognition engine (SRE)
        private SpeechRecognitionEngine   _speechRecognizer;

        private static KinectAudioHandler _kinectAudioHandler;

        private KinectAudioHandler(KinectSensor currentSensor)
        {
            CurrentSensor = currentSensor;
        }

        public static KinectAudioHandler Initialize(KinectSensor sensor, List<string> wordsToRecognize)
        {
            _kinectAudioHandler                      = new KinectAudioHandler(sensor);
            _kinectAudioHandler._speechRecognizer    = _kinectAudioHandler.CreateSpeechRecognizer(
                _kinectAudioHandler.WordsToRecognize = wordsToRecognize
            );

            //Start the sensor
            _kinectAudioHandler.CurrentSensor.Start();
            //Then run the start method to start streaming audio
            _kinectAudioHandler.Start();
            return _kinectAudioHandler;
        }

        public KinectAudioHandler Instance()
        {
            if (_kinectAudioHandler == null)
            {
                throw new InvalidOperationException("Handler not initialized");
            }
            return _kinectAudioHandler;
        }

        //Get the speech recognizer (SR)
        private static RecognizerInfo GetKinectRecognizer()
        {
            Func<RecognizerInfo, bool> matchingFunc = r =>
            {
                string value;
                r.AdditionalInfo.TryGetValue("Kinect", out value);
                return "True".Equals(value, StringComparison.InvariantCultureIgnoreCase) &&
                    "en-US".Equals(r.Culture.Name, StringComparison.InvariantCultureIgnoreCase);
            };
            return SpeechRecognitionEngine.InstalledRecognizers().Where(matchingFunc).FirstOrDefault();
        }


        //Start streaming audio
        private void Start()
        {
            //set sensor audio source to variable
            var audioSource           = CurrentSensor.AudioSource;
            //Set the beam angle mode - the direction the audio beam is pointing
            //we want it to be set to adaptive
            audioSource.BeamAngleMode = BeamAngleMode.Adaptive;
            //start the audiosource 
            var kinectStream          = audioSource.Start();
            //configure incoming audio stream
            _speechRecognizer.SetInputToAudioStream(
                kinectStream, new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
            //make sure the recognizer does not stop after completing     
            _speechRecognizer.RecognizeAsync(RecognizeMode.Multiple);
            //reduce background and ambient noise for better accuracy
            CurrentSensor.AudioSource.EchoCancellationMode = EchoCancellationMode.None;
            CurrentSensor.AudioSource.AutomaticGainControlEnabled = false;
        }

        //here is the fun part: create the speech recognizer
        private SpeechRecognitionEngine CreateSpeechRecognizer(List<string> wordsToRecongnize)
        {
            //set recognizer info
            RecognizerInfo ri = GetKinectRecognizer();
            //create instance of SRE
            SpeechRecognitionEngine sre;
            sre = new SpeechRecognitionEngine(ri.Id);

            //Now we need to add the words we want our program to recognise
            var grammar = new Choices();

            foreach(string word in wordsToRecongnize)
            {
                grammar.Add(word);
            }
            

            //set culture - language, country/region
            var gb = new GrammarBuilder { Culture = ri.Culture };
            gb.Append(grammar);

            //set up the grammar builder
            var g = new Grammar(gb);
            sre.LoadGrammar(g);

            //Set events for recognizing, hypothesising and rejecting speech
            sre.SpeechRecognized          += SreSpeechRecognized;
            sre.SpeechHypothesized        += SreSpeechHypothesized;
            sre.SpeechRecognitionRejected += SreSpeechRecognitionRejected;
            return sre;
        }

        //Speech is recognised
        private void SreSpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            //Very important! - change this value to adjust accuracy - the higher the value
            //the more accurate it will have to be, lower it if it is not recognizing you
            if (e.Result.Confidence < 0.8)
            {
                RejectSpeech(e.Result);
            }
            else
            {
                //and finally, here we set what we want to happen when 
                //the SRE recognizes a word
                WebSocketsHandler.Instance().SendToClients(new ServerMessage()
                {
                    SpeechText = e.Result.Text.ToUpperInvariant()
                }.ToJSON());
            }
        }


        //if speech is rejected
        private void RejectSpeech(RecognitionResult result)
        {
        }

        //if sre speech recognition is rejected
        private void SreSpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {
        }

        //hypothesized result
        private void SreSpeechHypothesized(object sender, SpeechHypothesizedEventArgs e)
        {
        }

        /// <summary>
        /// Notification callback for getting messages from the clients.
        /// </summary>
        /// <param name="message"></param>
        public void Notify(string message)
        {

            ClientMessage settings = JsonConvert.DeserializeObject<ClientMessage>(message);
            KinectAudioHandler.Initialize(KinectHandler.Instance().SensorChooser.Kinect, settings.Contents);

            //TODO: You can add more code here for notifying more components using client-side messages properly.
        }

    }

}
