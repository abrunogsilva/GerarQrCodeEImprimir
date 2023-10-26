using GerarQrCode;

namespace ConsoleRegistroTlb
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var patch = @"C:\\Users\\bruno.silva\\AppData\\Local\\Temp\\8025EC13-3BC2-4A3E-8512-D1465467.jpg";
            QrCode ok = new QrCode();
            ok.GerarQrcode(300, 300 , "Teste de Geracao teste de geracao de geração macavi",patch, "Teste de Geracao teste de geracao de geração macavi", 12);
            ok.ImprimirImagem(patch, "Microsoft Print to PDF");
        }
    }
}
