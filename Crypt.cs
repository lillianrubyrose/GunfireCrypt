using System.Security.Cryptography;
using System.Text;

namespace GunfireCrypt;

public class Crypt : IDisposable
{
    private readonly Rijndael crypt = Rijndael.Create();
    private readonly ICryptoTransform decryptor;
    private readonly ICryptoTransform encryptor;
    private readonly byte[] key = Encoding.UTF8.GetBytes("jwnnAS92nfe9Sas92nrSDdsn83Sd2w8".PadRight(32));
    private readonly byte[] vector = "ai9d9vjnSNSd8f3w"u8.ToArray();

    public Crypt()
    {
        encryptor = crypt.CreateEncryptor(key, vector);
        decryptor = crypt.CreateDecryptor(key, vector);
    }

    public void Dispose()
    {
        crypt.Dispose();
    }

    public byte[] encrypt(byte[] data)
    {
        using var stream = new MemoryStream(data);
        using var cryptoStream = new CryptoStream(stream, encryptor, CryptoStreamMode.Read);
        using var outStream = new MemoryStream();
        cryptoStream.CopyTo(outStream);
        return outStream.ToArray();
    }

    public byte[] decrypt(byte[] data)
    {
        using var stream = new MemoryStream(data);
        using var cryptoStream = new CryptoStream(stream, decryptor, CryptoStreamMode.Read);
        using var outStream = new MemoryStream();
        cryptoStream.CopyTo(outStream);
        return outStream.ToArray();
    }
}