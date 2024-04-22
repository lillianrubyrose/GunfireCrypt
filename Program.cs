using GunfireCrypt;

#pragma warning disable SYSLIB0022


var saveData = File.ReadAllBytes("C:\\Users\\lily\\Documents\\user_1.dat");
Console.WriteLine($"Save file has {saveData.Length} bytes");
var header = saveData[..4];
if (!header.SequenceEqual("EF2S"u8.ToArray()))
{
    Console.WriteLine("Bad header. Expected EF2S");
    return;
}

Console.WriteLine("Good header");

var crc16Hash = BitConverter.ToUInt16(saveData, header.Length + 1);
Console.WriteLine($"Hash = {crc16Hash}");

var dataToDecrypt = saveData[(header.Length + 3)..];
using var crypt = new Crypt();

var decryptedBytes = crypt.decrypt(dataToDecrypt);
var encryptedBytes = crypt.encrypt(decryptedBytes);
""
Console.WriteLine($"{dataToDecrypt.SequenceEqual(encryptedBytes)}");

var hash = Crc16.ComputeChecksum(encryptedBytes);
Console.WriteLine($"New hash = {hash}");

using var outputStream = new MemoryStream();
outputStream.Write("EF2S\x1"u8.ToArray());
outputStream.Write(BitConverter.GetBytes(hash));
outputStream.Write(encryptedBytes);

File.WriteAllBytes("C:\\Users\\lily\\Documents\\user_3.dat", outputStream.ToArray());


Console.WriteLine("Hello, World!");