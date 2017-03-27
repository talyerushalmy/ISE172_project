using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;

namespace MarketClient.Utils
{
    public static class SimpleCryptoLibrary
    {
        /// <summary>
        /// Returns the authentication token of the given username and private key
        /// </summary>
        /// <param name="username"></param>
        /// <param name="privateKey"></param>
        /// <returns>authentication token</returns>
        public static string CreateToken(string username, string privateKey)
        {
            return RSASignWithSHA256(username, privateKey);
        }


        /// <summary>
        /// This method extract the private ket from PEM format string
        /// </summary>
        /// <param name="privateKey">the private key in a PEM format</param>
        /// <returns>PrivateKey for RSACryptoServiceProvider</returns>
        private static RSAParameters ExtractRSAPrivateKey(string privateKey)
        {
            using (var txtreader = new StringReader(privateKey))
            {
                var keyPair = (AsymmetricCipherKeyPair)new PemReader(txtreader).ReadObject();
                RsaPrivateCrtKeyParameters rsaPrivKey = (RsaPrivateCrtKeyParameters)keyPair.Private;
                RSAParameters rsaKeyInfo = Org.BouncyCastle.Security.DotNetUtilities.ToRSAParameters(rsaPrivKey);
                return rsaKeyInfo;
            }
        }

        /// <summary>
        /// Sign on a message using SHA256 and RSA, the signed value is encoded in base64 format
        /// </summary>
        /// <param name="message">to sign</param>
        /// <param name="privateKey">of RSA in a PEM format</param>
        /// <returns>the signed value encoded in base64 format</returns>
        private static string RSASignWithSHA256(string message, string privateKey)
        {
            RSACryptoServiceProvider rsaAlgo = new RSACryptoServiceProvider();
            rsaAlgo.ImportParameters(ExtractRSAPrivateKey(privateKey));
            return Convert.ToBase64String(rsaAlgo.SignData(Encoding.UTF8.GetBytes(message), "SHA256"));
        }
    }
}
