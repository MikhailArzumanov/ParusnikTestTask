using Backend.Cryptography;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Runtime.CompilerServices;
using System.Text;

namespace Backend.Tests.Cryptography {
    #region Класс CryptographyTests
    /** Класс CryptographyTests
     * <summary>
     *  Класс предоставляет возможность тестирования модуля криптографии.
     * </summary>
     */
    public class CryptographyTests {
        #region Поля
        /** Поле rng
         * <summary>
         *  Поле предоставляет возможность генерации случайных значений.
         * </summary>
         */
        Random rng = new Random();
        #endregion
        #region Конструктор
        public CryptographyTests() { }
        #endregion
        #region Функция ThrowException
        /** Функция ThrowException
         * <summary>
         *  Осуществляет проброс исключения проваленного тестирования.
         * </summary>
         */
        private void ThrowException() {
            throw new Exception("Cryptography test have failed.");
        }
        #endregion
        #region Общий функционал
        /** Функция AreEqual
         * <summary>
         *  Функция осуществляет сравнение двух массивов байт.
         * </summary>
         * <param name="x">Первый массив байт</param>
         * <param name="y">Второй массив байт</param>
         * <returns>Возвращает результат сравнения</returns>
         */
        private bool AreEqual(byte[] x, byte[] y) {
            if (x.Length != y.Length) return false;
            for (int i = 0; i < x.Length; i++) {
                if (x[i] != y[i]) { 
                    return false; 
                }
            }
            return true;
        }
        #endregion
        #region Тесты модуля AesProvider
        /** Функция SingleAesTest
         * <summary>
         *  Функция осуществляет выполнение единичного теста модуля AesProvider.
         * </summary>
         * <param name="key">Ключ симметричного шифрования</param>
         * <param name="dataSize">Число байт в шифруемых данных</param>
         */
        private void SingleAesTest(byte[] key, int dataSize) {
            var data = new byte[dataSize];
            rng.NextBytes(data);
            var encrypted = AesProvider.Encrypt(data, key);
            var decrypted = AesProvider.Decrypt(encrypted, key);
            if (!AreEqual(data, decrypted)) {
                ThrowException();
            }
        }

        /** Функция TestAesByKey
         * <summary>
         *  Функция осуществляет ряд тестов модуля AesProvider для одного ключа.
         * </summary>
         * <param name="key">Ключ симметричного шифрования</param>
         */
        private void TestAesByKey(byte[] key) { 
            for(int dataSize = 8; dataSize < 128; dataSize++) {
                for(int _ = 0; _ < 12; _++) {
                    SingleAesTest(key, dataSize);
                }
            }
        }
        /** Функция TestAes
         * <summary>
         *  Функция осуществляет формирование ряда ключей <br/>
         *  и ряд тестов модуля AesProvider для каждого из них.
         * </summary>
         */
        private void TestAes() {
            for (int _ = 0; _ < 12; _++) {
                var key = new byte[16];
                rng.NextBytes(key);
                TestAesByKey(key);
            }
        }
        #endregion
        #region Тесты модуля контрольных сумм
        /** Функция TestHash
         * <summary>
         *  Функция осуществляет тестирование процесса получения контрольной суммы.
         * </summary>
         */
        private void TestHash() {
            var firstHash = Hasher.Hash(String.Empty);
            for(int _ = 0; _ < 100; _++) {
                byte[] bytes = new byte[rng.Next(12, 70)];
                string str = Convert.ToBase64String(bytes);
                var currentHash = Hasher.Hash(str);
                if(firstHash.Length != currentHash.Length) {
                    ThrowException();
                }
            }
        }
        #endregion
        #region Внешний интерфейс
        /** Функция Test
         * <summary>
         *  Функция осуществляет тестирование модуля AesProvider и Hasher.
         * </summary>
         */
        public static void Test() {
            var instance = new CryptographyTests();
            instance.TestAes();
            instance.TestHash();
        }
        #endregion
    }
    #endregion
}
