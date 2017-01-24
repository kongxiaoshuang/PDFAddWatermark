using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PDFAddWatermark
{
   public class PDFSetWatermark
    {
        /// <summary>  
        /// 加图片水印  
        /// </summary>  
        /// <param name="inputfilepath"></param>  
        /// <param name="outputfilepath"></param>  
        /// <param name="ModelPicName"></param>  
        /// <param name="top"></param>  
        /// <param name="left"></param>  
        /// <returns></returns>  
        public static bool PDFWatermark(string inputfilepath, string outputfilepath, string ModelPicName, float top, float left)
        {
            //throw new NotImplementedException();  
            PdfReader pdfReader = null;
            PdfStamper pdfStamper = null;
            try
            {
                pdfReader = new PdfReader(inputfilepath);

                int numberOfPages = pdfReader.NumberOfPages;

                iTextSharp.text.Rectangle psize = pdfReader.GetPageSize(1);

                float width = psize.Width;

                float height = psize.Height;

                pdfStamper = new PdfStamper(pdfReader, new FileStream(outputfilepath, FileMode.Create));

                PdfContentByte waterMarkContent;

                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(ModelPicName);

                image.GrayFill = 20;//透明度，灰色填充  
                //image.Rotation//旋转  
                //image.RotationDegrees//旋转角度  
                //水印的位置   
                if (left < 0)
                {
                    left = width / 2 - image.Width + left;
                }

                //image.SetAbsolutePosition(left, (height - image.Height) - top);  
                image.SetAbsolutePosition(left, (height / 2 - image.Height) - top);


                //每一页加水印,也可以设置某一页加水印   
                for (int i = 1; i <= numberOfPages; i++)
                {
                    //waterMarkContent = pdfStamper.GetUnderContent(i);//内容下层加水印  
                    waterMarkContent = pdfStamper.GetOverContent(i);//内容上层加水印  

                    waterMarkContent.AddImage(image);
                }
                //strMsg = "success";  
                return true;
            }
            catch (Exception ex)
            {
                throw ex;

            }
            finally
            {

                if (pdfStamper != null)
                    pdfStamper.Close();

                if (pdfReader != null)
                    pdfReader.Close();
            }
        }
        /// <summary>  
        /// 添加普通偏转角度文字水印  
        /// </summary>  
        /// <param name="inputfilepath"></param>  
        /// <param name="outputfilepath"></param>  
        /// <param name="waterMarkName"></param>  
        /// <param name="permission"></param>  
        public static bool setWatermark(string inputfilepath, string outputfilepath, string waterMarkName)
        {
            PdfReader pdfReader = null;
            PdfStamper pdfStamper = null;
            try
            {
                pdfReader = new PdfReader(inputfilepath);
                pdfStamper = new PdfStamper(pdfReader, new FileStream(outputfilepath, FileMode.Create));
                int total = pdfReader.NumberOfPages + 1;
                iTextSharp.text.Rectangle psize = pdfReader.GetPageSize(1);
                float width = psize.Width;
                float height = psize.Height;
                PdfContentByte content;
                BaseFont font = BaseFont.CreateFont(@"C:\Windows\Fonts\msyh.TTF", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);//微软雅黑字体
                PdfGState gs = new PdfGState();
                for (int i = 1; i < total; i++)
                {
                    content = pdfStamper.GetOverContent(i);//在内容上方加水印  
                    //content = pdfStamper.GetUnderContent(i);//在内容下方加水印  
                    //透明度  
                    gs.FillOpacity = 0.3f;
                    content.SetGState(gs);
                    //content.SetGrayFill(0.3f);  
                    //开始写入文本  
                    content.BeginText();
                    content.SetColorFill(BaseColor.RED);
                    content.SetFontAndSize(font, 18);//字体18号

                    float xpos = 0;//X坐标
                    float ypos = 0;//Y坐标
                    //循环打印文字水印
                    for (float p = -width/2; p < width * 5; p += 600)//调整宽度
                    {
                        xpos = p;
                        for (float q = -height / 2; q < height * 5; q += 650)//调整高度
                        {
                            ypos = q;
                            content.SetTextMatrix(xpos, ypos);//字体起始位置
                            content.ShowTextAligned(Element.ALIGN_CENTER, waterMarkName, xpos / 2 - 300, ypos / 2 - 150, 30);
                        }
                    }
                   
                    //content.SetColorFill(BaseColor.BLACK);  
                    //content.SetFontAndSize(font, 8);  
                    //content.ShowTextAligned(Element.ALIGN_CENTER, waterMarkName, 0, 0, 0);  
                    content.EndText();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

                if (pdfStamper != null)
                    pdfStamper.Close();

                if (pdfReader != null)
                    pdfReader.Close();
            }
        }
        /// <summary>  
        /// 添加倾斜水印  
        /// </summary>  
        /// <param name="inputfilepath"></param>  
        /// <param name="outputfilepath"></param>  
        /// <param name="waterMarkName"></param>  
        /// <param name="userPassWord"></param>  
        /// <param name="ownerPassWord"></param>  
        /// <param name="permission"></param>  
        public static bool setWatermark(string inputfilepath, string outputfilepath, string waterMarkName, string userPassWord, string ownerPassWord, int permission)
        {
            PdfReader pdfReader = null;
            PdfStamper pdfStamper = null;
            try
            {
                pdfReader = new PdfReader(inputfilepath);
                pdfStamper = new PdfStamper(pdfReader, new FileStream(outputfilepath, FileMode.Create));
                // 设置密码     
                //pdfStamper.SetEncryption(false,userPassWord, ownerPassWord, permission);   

                int total = pdfReader.NumberOfPages + 1;//获取页码
                iTextSharp.text.Rectangle psize = pdfReader.GetPageSize(1);
                float width = psize.Width;//PDF页面的宽度，用于计算水印倾斜
                float height = psize.Height;//获取页面的高度
                PdfContentByte content;
                BaseFont font = BaseFont.CreateFont(@"C:\WINDOWS\Fonts\SIMFANG.TTF", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                PdfGState gs = new PdfGState();

                gs.FillOpacity = 0.5f;//透明度
                int j = waterMarkName.Length;
                char c;
                int rise = 0;
                for (int i = 1; i < total; i++)
                {
                    rise = 800;
                    content = pdfStamper.GetOverContent(i);//在内容上方加水印
                    //content = pdfStamper.GetUnderContent(i);//在内容下方加水印 

                    content.BeginText();
                    content.SetColorFill(BaseColor.RED);//设置字体颜色
                    content.SetFontAndSize(font, 20);//设置字体大小
                    float xpos = 0;//X坐标
                    float ypos = 0;//Y坐标

                    // 设置水印文字字体倾斜 开始
                    //判断文字字符是否大于15个字符
                    if (j >= 15)
                    {
                        //循环打印文字水印
                        for (float p = -width/2; p < width*1.5; p+=200)
                        {
                            xpos = p;
                            for (float q = -height/2; q < height*1.5; q+=120)
                            {
                                ypos = q;
                                content.SetTextMatrix(200, 120);
                                for (int k = 0; k < j; k++)
                                {
                                    content.SetTextRise(rise);
                                    c = waterMarkName[k];
                                    content.ShowText(c + "");
                                    rise -= 10;
                                }
                            }
                        }
                    }
                    else
                    {
                        content.SetTextMatrix(180, 100);
                        for (int k = 0; k < j; k++)
                        {
                            content.SetTextRise(rise);
                            c = waterMarkName[k];
                            content.ShowText(c + "");
                            rise -= 18;
                        }
                    }
                    // 字体设置结束   
                    content.EndText();
                    // 画一个圆   
                    //content.Ellipse(250, 450, 350, 550);  
                    //content.SetLineWidth(1f);  
                    //content.Stroke();   
                }

                return true;

            }
            catch (Exception ex)
            {
                throw ex;
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
