using System;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Runtime.InteropServices;
using PdfSharp.Drawing;
using Zebra.Sdk.Comm;
using Zebra.Sdk.Graphics;
using Zebra.Sdk.Printer;
using static System.Net.Mime.MediaTypeNames;

namespace GerarQrCode
{
    [ProgId("GerarQrCode.QrCode")]
    [ComVisible(true), Guid("A5F81242-C212-4624-BCC2-F78A680503A0")]
    [ClassInterface(ClassInterfaceType.None)]
    public class QrCode : IQrCode
    {
        #region Métodos

        public void GerarQrcode(int largura, int altura, string texto, string localDoArquivo, string textodescricao, int tamanhoDaFonte)
        {
            if (string.IsNullOrEmpty(texto))
            {
                throw new Exception("Erro ao montar o QrCode, conteúdo do QrCode não disponível.");
            }

            //Inicializando Zxing
            var bw = new ZXing.BarcodeWriter();

            //Iniciando Opções de Encoding
            var opcaoesDeEncoding = new ZXing.Common.EncodingOptions() { Width = largura, Height = altura, Margin = 0 };

            //Atribuindo as opções de Enconding ao Zxing
            bw.Options = opcaoesDeEncoding;

            //Inicializando o Formato do Código
            bw.Format = ZXing.BarcodeFormat.QR_CODE;

            //Bitmap contendo a Imagem de QrCode
            var bitmapDeQrcode = bw.Write(texto);
            
            Bitmap bitmap = new Bitmap(bitmapDeQrcode);

            var imageHeight = bitmap.Height;
            var imageWidth = bitmap.Width;
            var textHeight = 50;

            // Create a new bitmap with a size of loaded image + rectangle for caption
            Bitmap img = new Bitmap(imageWidth, imageHeight + textHeight, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            Graphics graphics = Graphics.FromImage(img);

            // Draw the loaded image on newly created image
            graphics.DrawImage(bitmap, 0, 0);

            // Draw a rectangle for caption box
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(0, imageHeight, imageWidth, textHeight);
            Brush fillColor = new SolidBrush(Color.White);
            Pen pen = new Pen(Color.White, 2);
            graphics.DrawRectangle(pen, rectangle);
            graphics.FillRectangle(fillColor, rectangle);

            //Especificando Formato do texto
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;

            //cor do texto
            Brush textColor = new SolidBrush(Color.Black);

            //Fonte do Texto
            System.Drawing.Font arial = new System.Drawing.Font("Arial", tamanhoDaFonte, FontStyle.Regular);

            string text = textodescricao;

            // Texto no display
            if (text.Length > 35)
            {
                // Insere um hífen e uma quebra de linha após o 30º caractere.
                text = text.Insert(30, " -\n");
            }

            // Draw text
            graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            graphics.DrawString(text, arial, textColor, rectangle, stringFormat);

            // Save the output
            img.Save(localDoArquivo);
        }

        public void ImprimirImagem(string imagePath, string printerName, short numCopies=1)
        {
            try
            {
                #region Transformar Imagem em pdf

                //var pdfPath = imagePath.Replace(".jpg", ".pdf");

                //// Crie um novo documento PDF
                //PdfSharp.Pdf.PdfDocument document = new PdfSharp.Pdf.PdfDocument();

                //// Adicione uma página ao documento
                //PdfSharp.Pdf.PdfPage page = document.AddPage();

                //// Crie um objeto XGraphics para desenhar na página
                //XGraphics gfx = XGraphics.FromPdfPage(page);

                //// Carregue a imagem
                //XImage image = XImage.FromFile(imagePath);

                //// Desenhe a imagem na página
                //gfx.DrawImage(image, 0, 0, page.Width, page.Height);

                //// Salve o documento como um arquivo PDF
                //document.Save(pdfPath);

                ////Dispensando os documentos para que possam se excluidos.
                //document.Dispose();
                //image.Dispose();

                ////ler o pdf
                //Spire.Pdf.PdfDocument doc = new Spire.Pdf.PdfDocument(pdfPath);

                ////Especificando no nome da impressora.
                //doc.PrintSettings.PrinterName = printerName;
                //doc.PrintSettings.Copies = numCopies;

                ////Print the document
                //doc.Print();

                //if (File.Exists(imagePath))
                //{
                //    File.Delete(imagePath);
                //    if (File.Exists(pdfPath))
                //    {
                //        File.Delete(pdfPath);
                //    }
                //}

                #endregion


                #region Converter para Zpl e enviar para Impressão

                var testeZpl = PDFtoZPL.Conversion.ConvertBitmap(imagePath);

                // Abra a conexão com a impressora
                Connection thePrinterConn = new DriverPrinterConnection(printerName);
                thePrinterConn.Open();

                // Crie um objeto de impressão Zebra
                ZebraPrinter printer = ZebraPrinterFactory.GetInstance(thePrinterConn);

                // Envie o comando ZPL para a impressora
                string zplCommand = testeZpl;

                // Loop para imprimir várias cópias
                for (int i = 0; i < numCopies; i++)
                {
                    printer.SendCommand(zplCommand);

                }

                // Feche a conexão
                thePrinterConn.Close();

                #endregion

                #region Imprimir uma Imagem

                //// Imprimir uma imagem
                //PrintDocument printDocument = new PrintDocument();
                //printDocument.PrinterSettings.PrinterName = printerName;
                //printDocument.PrinterSettings.PrintToFile = true;
                //printDocument.PrinterSettings.Copies = (short)numCopies;

                //printDocument.PrintPage += (sender, e) =>
                //{
                //    Image image = Image.FromFile(imagePath);
                //    Rectangle m = e.MarginBounds;
                //    if ((double)image.Width / (double)image.Height > (double)m.Width / (double)m.Height)
                //    {
                //        m.Height = (int)((double)image.Height / (double)image.Width * (double)m.Width);
                //    }
                //    else
                //    {
                //        m.Width = (int)((double)image.Width / (double)image.Height * (double)m.Height);
                //    }
                //    e.Graphics.DrawImage(image, m);
                //};

                //printDocument.Print();

                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        #endregion
    }
}
