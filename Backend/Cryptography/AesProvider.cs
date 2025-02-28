using System.Security.Cryptography;

namespace Backend.Cryptography {
    #region Класс AesProvider
    /** Класс AesProvider
     * <summary>
     *  Класс предоставляет интерфейс для криптографических операций над данными.<br/>
     *  <br/>
     *  Включает две функции:<br/>
     *  Функция шифрования -- <see cref="Encrypt(byte[], byte[])"/><br/>
     *  Функция дешифрования -- <see cref="Decrypt(byte[], byte[])"/><br/>
     *  <br/>
     *  Используется алгоритм AES с длиной ключа 128 бит.
     * </summary>
     */
    public class AesProvider {
        #region Функционал обратного шифрования
        /** Функция Decrypt
         * <summary>
         *  Функция осуществляет операцию обратного шифрования.<br/>
         *  См. <see cref="AesProvider"/>.
         * </summary>
         * <param name="bytes">
         *  Входной поток данных
         * </param>
         * <param name="key">
         *  Ключ симметричного алгоритма
         * </param>
         * <returns>
         *  Возвращает дешифрованные данные.
         * </returns>
         */
        public static byte[] Decrypt(byte[] bytes, byte[] key) {
            byte[] tmpRes = new byte[bytes.Length];
            byte[] result;
            using (var aes = Aes.Create()) {
                aes.BlockSize = 128;
                aes.KeySize   = 128;
                aes.GenerateIV();
                aes.Key = key;
                int bytesWritten;
                aes.TryDecryptEcb(
                    bytes, tmpRes, PaddingMode.PKCS7, out bytesWritten
                );
                result = new byte[bytesWritten];
                for (int i = 0; i < bytesWritten; i++) {
                    result[i] = tmpRes[i];
                }
            }
            return result;
        }
        #endregion
        #region Функционал прямого шифрования
        /** Функция Encrypt
         * <summary>
         *  Функция осуществляет операцию прямого шифрования.<br/>
         *  См. <see cref="AesProvider"/>.
         * </summary>
         * <param name="bytes">
         *  Входной поток данных
         * </param>
         * <param name="key">
         *  Ключ симметричного алгоритма
         * </param>
         * <returns>
         *  Возвращает зашифрованные данные.
         * </returns>
         */
        public static byte[] Encrypt(byte[] bytes, byte[] key) {
            byte[] tmpRes = new byte[bytes.Length+16];
            byte[] result;
            using (var aes = Aes.Create()) {
                aes.BlockSize = 128;
                aes.KeySize   = 128;
                aes.Key = key;
                aes.GenerateIV();

                int bytesWritten;
                aes.TryEncryptEcb(
                    bytes, tmpRes, PaddingMode.PKCS7, out bytesWritten
                );
                result = new byte[bytesWritten];
                for(int i = 0; i < bytesWritten; i++) {
                    result[i] = tmpRes[i];
                }
            }
            return result;
        }
        #endregion
    }
    #endregion
}
