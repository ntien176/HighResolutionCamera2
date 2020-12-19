using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;

namespace HighResolutionCamera
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //Initialize camera library
        FilterInfoCollection filterInfoCollection;
        VideoCaptureDevice videoCaptureDevice;
        private void Form1_Load_1(object sender, EventArgs e)
        {
            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo filterInfo in filterInfoCollection)
                cboCamera.Items.Add(filterInfo.Name);
            cboCamera.SelectedIndex = 1;
            videoCaptureDevice = new VideoCaptureDevice();
        }
        
        //c# webcam capture picture
        private void FinalFrame_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            pic.Image = (Bitmap)eventArgs.Frame.Clone();
        }

        //Choose camera and open it
        private void start_btn_Click(object sender, EventArgs e)
        {
            videoCaptureDevice = new VideoCaptureDevice(filterInfoCollection[cboCamera.SelectedIndex].MonikerString);
            videoCaptureDevice.VideoResolution = selectResolution(videoCaptureDevice);
            videoCaptureDevice.NewFrame += FinalFrame_NewFrame;
            videoCaptureDevice.Start();
        }
        //Close the camera when closing the form
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (videoCaptureDevice.IsRunning == true)
                videoCaptureDevice.Stop();
        }
        
        //Take a photo
        private void Take_btn_Click(object sender, EventArgs e)
        {
            videoCaptureDevice.Stop();
            pic.Image = (Bitmap)pic.Image;
        }

        //Set resolution 1920 x 1080
        private static VideoCapabilities selectResolution(VideoCaptureDevice device)
        {
            foreach (var cap in device.VideoCapabilities)
            {
                if (cap.FrameSize.Height == 1080)
                    return cap;
                if (cap.FrameSize.Width == 1920)
                    return cap;
            }
            return device.VideoCapabilities.Last();
        }

        //Save the image
        private void save_btn_Click(object sender, EventArgs e)
        {
            if (pic.Image != null)
            {
                Bitmap varBmp = new Bitmap(pic.Image);
                Bitmap newBitmap = new Bitmap(varBmp);
                varBmp.Save(@"C:\Users\TN\Pictures\Camera Roll\savemode.jpeg", ImageFormat.Jpeg);
                varBmp.Dispose();
                varBmp = null;
            }
            else
            { MessageBox.Show("null exception"); }
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
