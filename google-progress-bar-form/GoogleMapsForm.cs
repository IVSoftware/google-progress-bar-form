using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace google_progress_bar_form
{
    public partial class GoogleMapsForm : Form
    {
        public GoogleMapsForm() => InitializeComponent();
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            progressBar1.Visible = false;
            chromiumWebBrowser1.LoadingStateChanged += onLoadingStateChanged;
            chromiumWebBrowser1.Load("https://www.google.com");
        }
        private async void onLoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            const int UPDATE_RESOLUTION =25;
            if(e.IsLoading)
            {
                Stopwatch sw = new Stopwatch();
                BeginInvoke(new Action(()=> progressBar1.Visible= true));                
                sw.Start();
                while(sw.Elapsed <= TimeSpan.FromSeconds(4))
                {
                    await Task.Delay(UPDATE_RESOLUTION);
                    if (!IsDisposed)
                    {
                        BeginInvoke(new Action(() => {
                            var progress = Math.Min(100, (int)Math.Round(sw.Elapsed.TotalSeconds / 0.04));
                            progressBar1.Value = progress;
                        }));
                    }
                }
                sw.Stop();
                BeginInvoke(new Action(() => progressBar1.Visible = false));

                Debug.WriteLine($"Progress: {progressBar1.Value} Elapsed {sw.Elapsed.ToString(@"ss\:ff")}");
            }
        }
    }
}
