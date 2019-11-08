using UnityEngine;
using System.IO;
using System;

namespace JSON
{
    public class JsonUtil
    {
        [Serializable]
        private class Wrapper<T>
        {
            public T[] Items;
        }

        public static string ToJson(object obj)
        {
            return JsonUtility.ToJson(obj);
        }

        public static string ToJson<T>(T[] arr)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = arr;
            return ToJson(wrapper);
            //배열 형식의 JSON ToJson
        }

        public static T FromJson<T>(string jsonData)
        {
            return JsonUtility.FromJson<T>(jsonData);
        }

        public static T[] FromArrJson<T>(string FileName)
        {
            Wrapper<T> wrapper = FromJson<Wrapper<T>>(FileName);
            return wrapper.Items;
            //배열 형식의 JSON FromJson
        }
        public static void CreateJson(string FileName, string jsonData)
        {
            FileStream fileStream = new FileStream(FileRoute(FileName), FileMode.Create);
            byte[] data = System.Text.Encoding.UTF8.GetBytes(jsonData);
            fileStream.Write(data, 0, data.Length);
            fileStream.Close();
            //크리에이터
        }

        public static bool FileCheck(string FileName)
        {
            if (File.Exists(FileRoute(FileName)))
            {
                return true;
            }
            else
                return false;
            //파일 경로에 해당 JSON 파일이 존재하는가?
        }

        public static string FileRoute(string FileName)
        {
            string Path = Application.dataPath + "/Resources/Json";
            return string.Format("{0}/{1}.json", Path, FileName);
            //json 기본 경로
        }

        public static T LoadJson<T>(string FileName)
        {
            FileStream fileStream = new FileStream(FileRoute(FileName), FileMode.Open);
            byte[] data = new byte[fileStream.Length];

            fileStream.Read(data, 0, data.Length);
            fileStream.Close();
            string Json = System.Text.Encoding.UTF8.GetString(data);
            return FromJson<T>(Json);
        }

        public static T[] LoadArrJson<T>(string FileName)
        {
            FileStream fileStream = new FileStream(FileRoute(FileName), FileMode.Open);
            byte[] data = new byte[fileStream.Length];

            fileStream.Read(data, 0, data.Length);
            fileStream.Close();
            string Json = System.Text.Encoding.UTF8.GetString(data);
            return FromArrJson<T>(Json);
            //배열 제이슨반환
        }
    }
}