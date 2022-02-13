using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Core.ToolBox
{
    // Bu class listeleri referanslamadan kopyalamaya yarıyor. Ancak şu anda kullanımda değil.
    // Belki daha sonra lazım olur diye tutuyorum.
    public static class DeepCopy
    {
        public static T DeepCopyList<T>(T item)
        {
            var formatter = new BinaryFormatter();
            var stream = new MemoryStream();
            formatter.Serialize(stream, item);
            stream.Seek(0, SeekOrigin.Begin);
            var result = (T) formatter.Deserialize(stream);
            stream.Close();
            return result;
        }
    }
}