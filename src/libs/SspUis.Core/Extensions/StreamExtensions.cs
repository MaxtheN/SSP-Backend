using System.IO;

namespace SspUis.Core;

public static class StreamExtensions
{
    /// <summary>
    /// 100 MB default
    /// </summary>
    public const int DEFAULT_BUFFER_SIZE = 102_400;// 100 MB
    public static byte[] ReadAsBytes(this Stream input)
    {
        byte[] array = new byte[DEFAULT_BUFFER_SIZE];
        using MemoryStream memoryStream = new MemoryStream();
        int count;
        while ((count = input.Read(array, 0, array.Length)) > 0)
        {
            memoryStream.Write(array, 0, count);
        }

        return memoryStream.ToArray();
    }
}
