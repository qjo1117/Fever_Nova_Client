using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using UnityEditor;

public class DataSystem 
{
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };

    public static readonly string SAVE_FOLDER = Application.dataPath + "/Data/";

    const string codeTemplate = @"
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class $CLASS
{
$ROW_MEMBER_CODE
}
";
    const string codeListTemplate = @"
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Data$CLASS
{
    public List<$CLASS> list$CLASS;

    public void DataParsing()
    {
        var data = DataSystem.Load(""$FILENAME"");

        foreach(var item in data) {
            $CLASS info = new $CLASS();
$ROW_MEMBER_CODE
        
            list$CLASS.Add(info);
        }
	}
}
";

    static public List<Dictionary<string, object>> Load(string file)
	{
        List<Dictionary<string, object>> listData = new List<Dictionary<string, object>>();     // 데이터를 저장할 녀석
        string data = File.ReadAllText(SAVE_FOLDER + file + ".csv");                                 // 일단 Resources쪽에서 로드하는 식으로
        string[] lines = Regex.Split(data, LINE_SPLIT_RE);                             // 자를껀 자르고 생각

        if (lines.Length <= 1) {
            return listData;
        }

        int index = 0;
        for (; index < lines.Length; ++index) {
            if(Regex.Split(lines[index], SPLIT_RE)[0].Contains("Index")) {
                break;
			}
        }

        if(index == lines.Length - 1){
            return null;
		}

        string[] header = Regex.Split(lines[index], SPLIT_RE);          // 두번째 줄 부터 순회를 시작  사유 : 구조
        string[] type = Regex.Split(lines[index + 1], SPLIT_RE);


        int lenLine = lines.Length;
        for (int i = index + 2; i < lenLine; ++i) {
            string[] values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length == 0 || values[0] == "" || values[0] == "\0") {
                continue;
            }

            Dictionary<string, object> dict = new Dictionary<string, object>();
            int length = type.Length;
            for (int j = 0; j < length; ++j) {
                string value = values[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                object finalValue = value;
                int iTemp = 0;
                float fTemp = 0.0f;
                bool bTemp = false;
                short sTemp = 0;

                if(type[j].Contains("int") == true) {
                    if (int.TryParse(value, out iTemp)) {
                        finalValue = iTemp;
                    }
                }
                else if (type[j].Contains("float") == true) {
                    if (float.TryParse(value, out fTemp)) {
                        finalValue = iTemp;
                    }
                }
                else if (type[j].Contains("bool") == true) {
                    if(bool.TryParse(value, out bTemp)) {
                        finalValue = bTemp;
                    }
				}
                else if (type[j].Contains("short") == true) {
                    if (short.TryParse(value, out sTemp)) {
                        finalValue = sTemp;
                    }
                }

                dict[header[j]] = finalValue;
            }

            listData.Add(dict);
        }


        return listData;
    }

    // 파일 제작 기능
    static public string GenerateDataToString(string csvText, string className, out List<Dictionary<string, object>> listData)
    {

        listData = Load(csvText);

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

    static public string GenerateDataToFile(string csvText, string className, List<Dictionary<string, object>> listData)
	{
        if (string.IsNullOrEmpty(className)) {
            return null;
        }

        string code = codeListTemplate;        
        string rowMemberCode = "";
        foreach(var data in listData[0]) {
            string key = data.Key;
            if (key == "" || key[0] == '\0') {
                continue;
            }
            rowMemberCode += string.Format("\t\t\tinfo.{0} = ({1})item[\"{0}\"];\n", key, data.Value.GetType().ToString());
        }
        

        code = code.Replace("$CLASS", className);
        code = code.Replace("$FILENAME", csvText);
        code = code.Replace("$ROW_MEMBER_CODE", rowMemberCode);

        return code;
    }

    // 파일 제작 기능
    static public void CreateGenerateScript(string csvText, string className)
    {
        List<Dictionary<string, object>> listData;

        string str = GenerateDataToString(csvText, className, out listData);
        File.WriteAllText(SAVE_FOLDER + className + ".cs", str);
        
        
        str = GenerateDataToFile(csvText, className, listData);
        File.WriteAllText(SAVE_FOLDER + "Data" + className + ".cs", str);
        
    }

    
}

