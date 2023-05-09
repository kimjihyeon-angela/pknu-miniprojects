using Aspose.BarCode.Generation;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZXing;
using ZXing.Common;
using ZXing.QrCode.Internal;
using ZXing.Rendering;

namespace StudentManagement
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnExitProgram_Click(object sender, RoutedEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            BtnExitProgram_Click(sender, e);
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {

            //BarcodeGenerator gen = new BarcodeGenerator(EncodeTypes.QR, "Aspose");
            //gen.Parameters.Barcode.XDimension.Pixels = 4;
            //gen.Parameters.Barcode.QR.QrVersion = QRVersion.Auto;
            //gen.Parameters.Barcode.QR.QrEncodeType = QREncodeType.ForceMicroQR;
            //gen.Save(dataDir + "QREncodeTypeForceQR.png", BarCodeImageFormat.Png);
            ZXing.BarcodeWriter barcodeWriter = new ZXing.BarcodeWriter();
            barcodeWriter.Format = ZXing.BarcodeFormat.QR_CODE;

            barcodeWriter.Options.Width = 400;
            barcodeWriter.Options.Height = 400;

            string strQRCode = "https://www.naver.com"; // 한글은 안되더라 ㅠ

            var qrCodeImage = barcodeWriter.Write(strQRCode);

            byte[] byteArray = ConvertToByteArray(qrCodeImage);
            return File(byteArray, "image/jpeg");

            //this.pictureBox1.Image = barcodeWriter.Write(strQRCode);
            //string deskPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            //barcodeWriter.Write(strQRCode).Save(deskPath + @"\test.jpg", ImageFormat.Jpeg);

            //ZXing.BarcodeReader barcodeReader = new ZXing.BarcodeReader();
            //string deskPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

            //var barcodeBitmap = (Bitmap)Image.FromFile(deskPath + @"\test.jpg");
            //var result = barcodeReader.Decode(barcodeBitmap);

            //this.textBox1.Text = result.Text;
        }

        private byte[] ConvertToByteArray(Image img)
        {
            using (var stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
    }
}
