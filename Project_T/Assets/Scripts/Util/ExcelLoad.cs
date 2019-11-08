using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace EXCEL
{
    public class ExcelLoad
    {
        static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))"; //해당 문자가 들어간것을 나눈다.
        static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";//행을 구분하기 위해서 사용
        static char[] TRIM_CHARS = { '\"' }; //공백제거용

        public static List<Dictionary<string, object>> Read(string file)
        {
            var list = new List<Dictionary<string, object>>();
            TextAsset data = Resources.Load<TextAsset>(file);

            var lines = Regex.Split(data.text, LINE_SPLIT_RE);

            if (lines.Length <= 1)
                return list;

            var header = Regex.Split(lines[0], SPLIT_RE);
            for (var i = 1; i < lines.Length; i++)//맨 윗줄은 제외
            {
                var value = Regex.Split(lines[i], SPLIT_RE);//맨 윗줄을 잘라와서 그것을 키 값으로 삼는다.
                if (value.Length == 0 || value[0] == "")    //라인을 한 줄씩 잘라온다.
                    continue;//없으면 컨티뉴

                var entry = new Dictionary<string, object>();

                //해당 줄에 존재하는 문자들을 읽는다.
                for (var j = 0; j < header.Length && j < value.Length; j++)
                {
                    string cellValue = value[j];
                    cellValue = cellValue.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", ""); //해당 문자열을 포함하는 문자를 제거
                    object objectValue = cellValue;
                    int n;
                    float f;
                    if (int.TryParse(cellValue, out n))
                    {
                        objectValue = n;
                    }
                    else if (float.TryParse(cellValue, out f))
                    {
                        objectValue = f;
                    }
                    entry[header[j]] = objectValue;
                }
                list.Add(entry);
            }
            return list;
        }
    }
}
