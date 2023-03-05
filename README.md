Your post says that you want to increment 100 values in 4 seconds. Consider 'not' using a `Timer` for this. Here's one alternative way to go about it:

- Set how frequently you want the progress bar to update (let's say ~25 ms).
- Use a `System.Diagnostics.Stopwatch` to keep track of elapsed time.
- Loop, updating at `UPDATE_RESOLUTION` intervals.
- To **calculate progress bar** set `Value` to the fraction of `StopWatch.Elapsed` / 0.04 where `progressBar1.Maximum` is 100
- Exit the loop when `StopWatch.Elapsed` exceeds 4 seconds.

***

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

[![screenshot][1]][1]


  [1]: https://i.stack.imgur.com/5S4EB.png