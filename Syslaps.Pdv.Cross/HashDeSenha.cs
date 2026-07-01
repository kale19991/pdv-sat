using System;
using System.Security.Cryptography;
using System.Text;

namespace Syslaps.Pdv.Cross
{
    /// <summary>
    /// Gera e valida hashes de senha com PBKDF2-HMACSHA256. O hash é gravado em formato
    /// auto-descritivo (algoritmo$iterações$salt$hash), permitindo elevar o custo no
    /// futuro sem invalidar os hashes já gravados no banco.
    /// </summary>
    public static class HashDeSenha
    {
        private const string Algoritmo = "PBKDF2-SHA256";
        private const int Iteracoes = 210000;
        private const int TamanhoSaltEmBytes = 16;
        private const int TamanhoHashEmBytes = 32;

        public static string GerarHash(string senha)
        {
            var salt = new byte[TamanhoSaltEmBytes];
            using (var gerador = RandomNumberGenerator.Create())
            {
                gerador.GetBytes(salt);
            }

            var hash = DerivarHash(senha, salt, Iteracoes);
            return $"{Algoritmo}${Iteracoes}${Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";
        }

        public static bool Validar(string senha, string hashArmazenado)
        {
            if (string.IsNullOrEmpty(senha) || string.IsNullOrEmpty(hashArmazenado)) return false;

            var partes = hashArmazenado.Split('$');
            if (partes.Length != 4 || partes[0] != Algoritmo) return false;
            if (!int.TryParse(partes[1], out var iteracoes)) return false;

            byte[] salt, hashEsperado;
            try
            {
                salt = Convert.FromBase64String(partes[2]);
                hashEsperado = Convert.FromBase64String(partes[3]);
            }
            catch (FormatException)
            {
                return false;
            }

            var hashCalculado = DerivarHash(senha, salt, iteracoes);
            return CompararEmTempoConstante(hashCalculado, hashEsperado);
        }

        private static byte[] DerivarHash(string senha, byte[] salt, int iteracoes)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(senha)))
            {
                var u = hmac.ComputeHash(Concatenar(salt, new byte[] { 0, 0, 0, 1 }));
                var t = (byte[])u.Clone();

                for (var i = 1; i < iteracoes; i++)
                {
                    u = hmac.ComputeHash(u);
                    for (var j = 0; j < t.Length; j++)
                    {
                        t[j] ^= u[j];
                    }
                }

                return t;
            }
        }

        private static byte[] Concatenar(byte[] a, byte[] b)
        {
            var resultado = new byte[a.Length + b.Length];
            Buffer.BlockCopy(a, 0, resultado, 0, a.Length);
            Buffer.BlockCopy(b, 0, resultado, a.Length, b.Length);
            return resultado;
        }

        private static bool CompararEmTempoConstante(byte[] a, byte[] b)
        {
            if (a.Length != b.Length) return false;

            var diferenca = 0;
            for (var i = 0; i < a.Length; i++)
            {
                diferenca |= a[i] ^ b[i];
            }

            return diferenca == 0;
        }
    }
}
