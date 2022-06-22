using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;

public class DataSystem 
{
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };

    public static readonly string SAVE_FOLDER = Application.dataPath + "/Data/";

    const string codeTemplate = @"
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class $CLASS
{
$ROW_MEMBER_CODE
}";

    static public List<Dictionary<string, object>> Load(string file)
	{
        List<Dictionary<string, object>> listData = new List<Dictionary<string, object>>();     // 데이터를 저장할 녀석
        TextAsset data = Resources.Load(file) as TextAsset;                                 // 일단 Resources쪽에서 로드하는 식으로
        string[] lines = Regex.Split(data.text, LINE_SPLIT_RE);                             // 자를껀 자르고 생각

        if (lines.Length <= 1) {
            return listData;
        }

        string[] header = Regex.Split(lines[2], SPLIT_RE);          // 두번째 줄 부터 순회를 시작  사유 : 구조
        string[] type = Regex.Split(lines[3], SPLIT_RE);
        

        for (int i = 4; i < lines.Length; ++i) {
            string[] values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length == 0 || values[0] == "" || values[0] == "\0") {
                continue;
            }

            Dictionary<string, object> dict = new Dictionary<string, object>();
            for (int j = 0; j < lines.Length - 1; ++j) {
                string value = values[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                object finalValue = value;
                int iTemp = 0;
                float fTemp = 0.0f;

                if(type[j].Contains("int") == true) {
                    if (int.TryParse(value, out iTemp)) {
                        finalValue = iTemp;
                    }
                }
                else if (type[j].Contains("float") == true) {
                    if (float.TryParse(value, out fTemp))
                    {
                        finalValue = iTemp;
                    }
                }

                dict[header[j]] = finalValue;
            }

            listData.Add(dict);
        }


        return listData;
    }

 //   static public List<T> LoadFileToData<T>(string file)
 //   {
 //       List<Dictionary<string, object>> listData = DataSystem.Load(file);

 //       int size = listData.Count;

 //       T data = new T();
 //       // Fields 변수명을 가져온다.
 //       System.Reflection.FieldInfo[] info = data.GetType().GetFields();

 //       int count = listData.Count;
 //       for (int i = 0; i < count; ++i) {
            
 //       }

 //       return new List<T>();
	//}


    // 파일 제작 기능
    static public string Generate(string csvText, string className)
    {
        if (string.IsNullOrEmpty(csvText)) {
            return null;
        }

        List<Dictionary<string, object>> listData = Load(csvText);

        string rowMemberCode = "";

        foreach(var item in listData[0]) {
            string key = item.Key;
            if(key == "" || key[0] == '\0') {
                continue;
			}
            rowMemberCode += string.Format("\t\tpublic {1} {0};\n", key, item.Value.GetType().ToString());
        }

        string code = codeTemplate;
        code = code.Replace("$CLASS", className);
        code = code.Replace("$ROW_MEMBER_CODE", rowMemberCode);

        return code;
    }

    // 파일 제작 기능
    static public void CreateGenerateCSS(string csvText, string className)
	{
        string str = Generate(csvText, className);
        File.WriteAllText(SAVE_FOLDER + className + ".cs", str);
    }
    
}

