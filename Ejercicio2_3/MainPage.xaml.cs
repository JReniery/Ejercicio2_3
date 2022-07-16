using Ejercicio2_3.Model;
using Plugin.AudioRecorder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Ejercicio2_3
{
    public partial class MainPage : ContentPage
    {
        private readonly AudioRecorderService recorder = new AudioRecorderService();
        private readonly AudioPlayer audioPlayer = new AudioPlayer();

        private static string DEFAULTPATH = "/storage/emulated/0/Android/data/com.companyname.ejercicio2_3/files";
        //private string defaultAudioPath;
        private string audioName;
        private string audioPath;
        private string tempAudioPath;
        private int cantAudiosGallery;

        private DateTime DateAndTime;
        private string Date;

        public MainPage()
        {
            InitializeComponent();            
        }

        private async void btnRec_Clicked(object sender, EventArgs e)
        {
            /*var statusMic = await Permissions.RequestAsync<Permissions.Microphone>();
            if (statusMic != PermissionStatus.Granted)
            {
                return;
            }*/
            //string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            //defaultAudioPath = Path.Combine(DEFAULTPATH, "Audios");            

            audioName = txbName.Text + ".wav";
            audioPath = Path.Combine(DEFAULTPATH, audioName);
            DateAndTime = DateTime.Now;
            Date = DateAndTime.ToString("dd-MM-yyyy hh:mm:ss");

            if (File.Exists(audioPath))
                await DisplayAlert("Aviso", "Archivo ya existe", "Ok");
            else
            {
                await RecordAudio();
            }
        }


        private async Task RecordAudio()
        {
            try
            {
                if (!recorder.IsRecording)
                {
                    await recorder.StartRecording();
                    btnRec.Source = "detener.png";
                }
                else
                {
                    btnRec.Source = "voiceLogo";
                    await recorder.StopRecording();
                    await guardarAudio();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /*private async void PlayAudio()
        {
            try
            {
                tempAudioPath = recorder.GetAudioFilePath();

                if (tempAudioPath != null)
                {
                    audioPlayer.Play(tempAudioPath);
                    File.Move(tempAudioPath, audioPath);
                    await guardarAudio();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }*/

        private async void btnAudioList_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Views.AudiosGallery());
        }

        /*private void saveAudio()
        {
            if (!File.Exists(audioPath))
            {
                File.Move(tempAudioPath, audioPath);
            }
        }*/

        private async Task guardarAudio()
        {
            try
            {
                tempAudioPath = recorder.GetAudioFilePath();

                if (tempAudioPath != null)
                {                   
                    File.Move(tempAudioPath, audioPath);

                    var datosAudio = new Audio
                    {
                        id = 0,
                        name = audioName,
                        path = audioPath,
                        date = Date
                    };
                    await App.DBase.guardarAudio(datosAudio);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        /*private async void cantAudios()
        {
            var audioList = await App.DBase.obtenerAudios();
            int cant = audioList.Count() + 1;
            cantAudiosGallery = cant;
            txbName.Text = "grabacion" + cantAudiosGallery.ToString();            
        }*/

    }
}