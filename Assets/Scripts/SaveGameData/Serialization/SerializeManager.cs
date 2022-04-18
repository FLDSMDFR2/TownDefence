using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SerializeManager
{
    private static BinaryFormatter formatter = new BinaryFormatter();
    private static string mainDirectory = Application.persistentDataPath + "/SaveData";
    private static string fileExtention = ".bam";

    public static void Save(SerializableObject obj)
    {
        string filePath = mainDirectory + obj.Path + fileExtention;
        System.IO.FileInfo file = new System.IO.FileInfo(filePath);

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        else
        {
            file.Directory.Create();
        }

        FileStream fStream = new FileStream(mainDirectory + obj.Path + fileExtention, FileMode.Create);

        try
        {
            formatter.Serialize(fStream, obj);

            fStream.Close();
    }
        catch
        {
            fStream.Close();
        }
    }

    public static T Load<T>(SerializableObject obj)
    {
        TraceManager.WriteTrace(TraceChannel.Main, TraceType.info, mainDirectory + obj.Path + fileExtention);
        if (File.Exists(mainDirectory + obj.Path + fileExtention))
        {
            FileStream fStream = new FileStream(mainDirectory + obj.Path + fileExtention, FileMode.Open);
            try
            {
                T retval = (T)formatter.Deserialize(fStream);

                fStream.Close();

                return retval;
            }
            catch
            {
                fStream.Close();
                return default(T);

            }
        }
        else
        {
            TraceManager.WriteTrace(TraceChannel.Main, TraceType.info, "No Data to Load");
            return default(T);
        }

    }

}
