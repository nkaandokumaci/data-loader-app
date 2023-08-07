using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace dataloaders
{
    public partial class Form1 : Form
    {
        private DateTime programStartTime;
        private Timer timer;
        private int progressValue;
        private int downloadPercentage;
        private bool fileSelected = false;
        private bool isDownloading = false;
        private string selectedFilePath = "";


        public int DownloadPercentage
        {
            get { return downloadPercentage; }
            set
            {
                downloadPercentage = value;
                progressBar1.Value = value;
            }
        }
        public Form1()
        {
            InitializeComponent();
            InitializeTimer();
            //timer.Tick += Timer_Tick;
            //timer = new Timer();
            //timer.Interval = 1000;
            //timer.Tick += Timer_Tick;
            
        }


        private async void button1_Click(object sender, EventArgs e)
        {



            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                // Set the filter and other options for the OpenFileDialog
                openFileDialog.Filter = "Binary Files (*.bin)|*.bin";
                openFileDialog.Title = "Select a .bin file";

                // Show the OpenFileDialog and check if the user clicked the OK button
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Get the selected file path and display it in the TextBox
                    selectedFilePath = openFileDialog.FileName;
                    textBox1.Text = selectedFilePath;

                    // Set the fileSelected flag to true
                    fileSelected = true;
                }
            }
            /*string fileUrl = textBox2.Text.Trim();

            if (!string.IsNullOrEmpty(fileUrl))
            {
                try
                {
                  
                    UpdateLoadingStatus();
                    isDownloading = true;

                    using (WebClient webClient = new WebClient())
                    {
                        string fileName = Path.GetFileName(fileUrl);

                        using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                        {
                            saveFileDialog.FileName = fileName;
                            if (saveFileDialog.ShowDialog() == DialogResult.OK)
                            {
                                await webClient.DownloadFileTaskAsync(new Uri(fileUrl), saveFileDialog.FileName);

                                MessageBox.Show("File downloaded successfully!");
                                UpdateLoadingStatus() ;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error occurred while downloading the file: " + ex.Message);
                }
                finally
                {
                    isDownloading = false;
                  
                }
            }*/

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Record the start time of the program
            programStartTime = DateTime.Now;

            // Start the timer to update the elapsed time
            timer.Start();

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void InitializeTimer()
        {
            timer = new Timer();
            timer.Interval = 100; // Set the interval in milliseconds (adjust as needed)
            timer.Tick += Timer_Tick;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Update the ProgressBar value based on the progressValue
            progressBar1.Value = progressValue;

            // Check if the progress reaches 100%
            if (progressValue >= progressBar1.Maximum)
            {
                // Stop the timer
                timer.Stop();

                // Reset the progressValue for future use
                progressValue = 0;
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            // Clear the NumericUpDown control if the entered value is not numeric
            if (!decimal.TryParse(numericUpDown1.Text, out decimal value))
            {
                numericUpDown1.Text = string.Empty;
            }

        }

        private void numericUpDown1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow numeric characters and control keys (e.g., Backspace, Delete)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Mark the event as handled to suppress the key press
            }
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown2_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow numeric characters and control keys (e.g., Backspace, Delete)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Mark the event as handled to suppress the key press
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
        
        private void UpdateLoadingStatus(string status)
        {
            textBox2.Text = status;
        }
        private void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            // Update the ProgressBar based on the download percentage
            progressBar1.Value = e.ProgressPercentage;
        }
        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }



        private void label7_Click(object sender, EventArgs e)
        {

        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            // Calculate the elapsed time since the program started
            TimeSpan elapsedTime = DateTime.Now - programStartTime;

            // Update the Label control with the elapsed time
            label7.Text = string.Format("Elapsed Time: {0:D2}:{1:D2}:{2:D2}",
                elapsedTime.Hours, elapsedTime.Minutes, elapsedTime.Seconds);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private async void Button1_Click_1(object sender, EventArgs e)
        {
            // Check if a file has been selected and no download process is ongoing
            if (!fileSelected || isDownloading)
            {
                MessageBox.Show("Please select a file to download.");
                return;
            }

            // Set isDownloading to true before the download starts
            isDownloading = true;

            try
            {
                UpdateLoadingStatus("Downloading...");
                progressBar1.Value = 0; // Reset the progress bar

                using (WebClient webClient = new WebClient())
                {
                    // Get the file name from the URL
                    string fileName = Path.GetFileName(selectedFilePath);

                    using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                    {
                        saveFileDialog.FileName = fileName;
                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            // Download the file and save it to the destination path
                            webClient.DownloadProgressChanged += WebClient_DownloadProgressChanged;
                            await webClient.DownloadFileTaskAsync(new Uri(selectedFilePath), saveFileDialog.FileName);

                            MessageBox.Show("File downloaded successfully!");
                            UpdateLoadingStatus("Downloaded");
                        }
                        else
                        {
                            // User canceled the save dialog, cancel the download
                            webClient.CancelAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred while downloading the file: " + ex.Message);
            }
            finally
            {
                // Download is complete, update the status
                isDownloading = false;
                fileSelected = false; // Reset the fileSelected flag
                progressBar1.Value = 0; // Reset the progress bar
            }

        }

        /*private void progressBar1_Click(object sender, EventArgs e)
        {
            // Check if the event arguments are of type DownloadProgressChangedEventArgs
            if (e is DownloadProgressChangedEventArgs downloadArgs)
            {
                // Update the ProgressBar based on the download percentage
                progressBar1.Value = downloadArgs.ProgressPercentage;
            }
        }*/

        private void timer2_Tick(object sender, EventArgs e)
        {

        }

        /*private async void button2_Click(object sender, EventArgs e)
        {
            // Check if a file has been previously selected
            if (string.IsNullOrEmpty(selectedFilePath))
            {
                MessageBox.Show("Please select a file using the 'Browse' button.");
                return;
            }

            // Proceed with the download using the previously selected file path
            try
            {
                isDownloading = true; // Set isDownloading to true before the download starts
                UpdateLoadingStatus();
                //progressBar1.Value = 0; // Reset the progress bar

                using (WebClient webClient = new WebClient())
                {
                    string fileName = Path.GetFileName(selectedFilePath);

                    using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                    {
                        saveFileDialog.FileName = fileName;
                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            // Download the file and save it to the destination path
                            webClient.DownloadProgressChanged += WebClient_DownloadProgressChanged;
                            await webClient.DownloadFileTaskAsync(new Uri(selectedFilePath), saveFileDialog.FileName);


                            MessageBox.Show("File downloaded successfully!");
                            //isDownloading = false; // Set isDownloading to false when download is complete
                            //UpdateLoadingStatus();
                            //progressBar1.Value = 0; // Reset the progress bar
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred while downloading the file: " + ex.Message);
            }
            finally
            {
                // Download is complete, update the status
                isDownloading = false;
                UpdateLoadingStatus();
                progressBar1.Value = 0; // Reset the progress bar
            }
        }*/
        //private void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        //{
        // Update the ProgressBar based on the download percentage
        //  progressBar1.Value = e.ProgressPercentage;
        //}
    }
}