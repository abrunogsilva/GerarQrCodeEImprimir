using System;
using System.Runtime.InteropServices;

namespace GerarQrCode
{
    [ComVisible(true), Guid("A92951B3-FCA2-400F-8C28-75F0ED5BB468")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IQrCode
    {
        #region Métodos

        void GerarQrcode(int largura, int altura, string texto, string localDoArquivo, string textodescricao, int tamanhoDaFonte);

        void ImprimirImagem(string imagePath, string printerName, short numCopies);

        #endregion
    }
}
