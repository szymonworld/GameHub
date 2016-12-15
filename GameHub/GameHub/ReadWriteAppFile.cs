using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Runtime.Serialization.Formatters.Binary;

namespace GameHub
{
    class ReadWriteAppFile
    {
        private string fileName;
        private string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);


        public ReadWriteAppFile(string file_name)
        {
            fileName = file_name;
        }

        public void ChangeFileName(string name)
        {
            fileName = name;
        }

        public void SaveFile(string stringToWrite)
        {
            string filePath = Path.Combine(path, fileName);
            File.WriteAllText(filePath, stringToWrite);
        }

        public string RestoreFile()
        {
            string filePath = Path.Combine(path, fileName);
            string content;

            using (var streamReader = new StreamReader(filePath))
            {
                content = streamReader.ReadToEnd();
                System.Diagnostics.Debug.WriteLine(content);
            }
            return content;
        }

        public void SaveObject(object obj)
        {
            byte[] objInByte = ObjectToByteArray(obj);
            //string s2 = BitConverter.ToString(objInByte);
            //string s1 = Convert.ToBase64String(objInByte);
            //SaveFile(s1);
            string filePath = Path.Combine(path, fileName);
            File.WriteAllBytes(filePath, objInByte);

        }

        public object RestoreObject()
        {
            string filePath = Path.Combine(path, fileName);
            byte[] byteObj = File.ReadAllBytes(filePath);
            object obj = ByteArrayToObject(byteObj);
            return obj;

            //string s2 = RestoreFile();
            //byte[] byteObj = Convert.FromBase64String(s2);
            //object obj = ByteArrayToObject(byteObj);
            //return obj;
        }

        public byte[] ObjectToByteArray(Object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public Object ByteArrayToObject(byte[] arrBytes)
        {
            using (var memStream = new MemoryStream())
            {
                var binForm = new BinaryFormatter();
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                var obj = binForm.Deserialize(memStream);
                return obj;
            }
        }

    }
}