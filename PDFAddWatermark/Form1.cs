using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using System.Drawing;

namespace PDFAddWatermark
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string source = @"E:\MyProject\PDFAddWatermark\res\83073fb4-5f17-4d98-8f7c-499ac489cb00_1011036090.pdf"; //模板路径
            string output = @"E:\MyProject\PDFAddWatermark\res\watermark.pdf"; //导出水印背景后的PDF
            string watermark = @"E:\MyProject\PDFAddWatermark\res\QQ图片20161229171219.png";   // 水印图片

            //bool isSurrcess = PDFSetWatermark.setWatermark(source, output, "网站查询版本，不得用于其他用途");  
            bool isSurrcess = PDFSetWatermark.setWatermark(source, output, "网站查询版本，不得用于其他用途");  
            //bool isSurrcess = PDFWatermark(source, output, watermark, 100, 200);
            MessageBox.Show(isSurrcess.ToString());
        }


        public bool PDFWatermark(string inputfilepath, string outputfilepath, string ModelPicName, float top, float left)
        {
            //throw new NotImplementedException();
            PdfReader pdfReader = null;
            PdfStamper pdfStamper = null;
            try
            {
                pdfReader = new PdfReader(inputfilepath);

                int numberOfPages = pdfReader.NumberOfPages;////获取PDF的总页数

                iTextSharp.text.Rectangle psize = pdfReader.GetPageSize(1);//获取第一页

                float width = psize.Width;//PDF页面的宽度，用于计算水印倾斜

                float height = psize.Height;

                pdfStamper = new PdfStamper(pdfReader, new FileStream(outputfilepath, FileMode.Create));

                PdfContentByte waterMarkContent;

                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(ModelPicName);

                image.GrayFill = 200;//透明度，灰色填充
                image.Rotation = 30;//旋转
                image.RotationDegrees = 150;//旋转角度
                //水印的位置 
                if (left < 0)
                {
                    left = width - image.Width + left;
                }

                image.SetAbsolutePosition(left, (height - image.Height) - top);


                //每一页加水印,也可以设置某一页加水印 
                for (int i = 1; i <= numberOfPages; i++)
                {
                    waterMarkContent = pdfStamper.GetUnderContent(i);

                    waterMarkContent.AddImage(image);
                }
                //strMsg = "success";
                return true;
            }
            catch (Exception ex)
            {
                ex.Message.Trim();
                return false;
            }
            finally
            {

                if (pdfStamper != null)
                    pdfStamper.Close();

                if (pdfReader != null)
                    pdfReader.Close();
            }
        }





    }
}
