using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PartialZ.Api.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace PartialZ.Api.Services
{
    public class CryptographyService: ICryptographyService
    {
        private IConfiguration _configuration { get; }
        public CryptographyService(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public string Encrypt(string Text)
        {
            string EncryptionKey = this._configuration.GetValue<string>("Cryptography:Key");
            byte[] clearBytes = Encoding.Unicode.GetBytes(Text);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        try
                        {
                            cs.Write(clearBytes, 0, clearBytes.Length);
                            cs.Close();
                        }
                        catch (Exception)
                        { 
                            throw;
                        }
                    }
                    Text = Convert.ToBase64String(ms.ToArray());
                    Text = Convert.ToBase64String(Encoding.UTF8.GetBytes(Text));
                }
            }
            return Text;
        }
        public string Decrypt(string Text)
        {
            string EncryptionKey = this._configuration.GetValue<string>("Cryptography:Key");
            Text = Encoding.UTF8.GetString(Convert.FromBase64String(Text));
            byte[] cipherBytes = Convert.FromBase64String(Text);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        try
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        catch (Exception)
                        {
                            throw;
                        }

                    }
                    Text = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return Text;
        }
    }
}
