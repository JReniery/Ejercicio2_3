using Ejercicio2_3.Model;
using Plugin.AudioRecorder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ejercicio2_3.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AudiosGallery : ContentPage
    {
        private readonly AudioRecorderService recorder = new AudioRecorderService();
        private readonly AudioPlayer audioPlayer = new AudioPlayer();
        public AudiosGallery()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            fillCollectionView();
        }

        private async void fillCollectionView()
        {
            try
            {
                clvAudios.ItemsSource = await App.DBase.obtenerAudios();
            }
            catch(Exception ex)
            {
                await DisplayAlert("Vaya!", "Algo salió mal...", "OK");
                //Console.WriteLine(ex.Message);
            }
            
        }

        private async void swpDelete_Invoked(object sender, EventArgs e)
        {
            if (await DisplayAlert("Confirmar", "Desea Eliminar este Audio? \ntambién se eliminará de su dispositivo", "Sí", "No"))
            {
                var audio = (Audio)(sender as SwipeItemView).CommandParameter;
                var archivo = audio.path;
                var result = await App.DBase.borrarAudio(audio);                
                File.Delete(archivo);

                if (result == 1)
                {
                    fillCollectionView();
                    await DisplayAlert("Aviso", "Audio Eliminado", "OK");
                }
                else
                {
                    await DisplayAlert("Aviso", "Ha ocurrido un error", "OK");
                }
            }
        }

        private void swpPlay_Invoked(object sender, EventArgs e)
        {
            try
            {
                var audio = (Audio)(sender as SwipeItemView).CommandParameter;
                audioPlayer.Play(audio.path);
            }
            catch(Exception ex)
            {
                DisplayAlert("Ups!", "No se ha encontrado este audio", "OK");
                //Console.WriteLine(ex.Message);
            }
        }
    }
}