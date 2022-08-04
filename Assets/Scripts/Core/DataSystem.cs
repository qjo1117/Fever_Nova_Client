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

$DEFINE_ENUM

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
    public List<$CLASS> list$CLASS = new List<$CLASS>();

    public void DataParsing()
    {
        var data = DataSystem.Load(""$FILENAME"");

        foreach(var item in data) {
            $CLASS info = new $CLASS();
$ROW_MEMBER_CODE
        
            list$CLASS.Add(info);
        }
	}

    public $CLASS At(int _index)
	{
        if((0 <= _index && _index < list$CLASS.Count) == false) {
            return null;
		}

        return list$CLASS[_index];
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
            if(Regex.Split(lines[index], SPLIT_RE)[1].Contains("index")) {
                break;
			}
        }

        if(index == lines.Length - 1){
            return null;
		}

        string[] header = Regex.Split(lines[index], SPLIT_RE);          // 두번째 줄 부터 순회를 시작  사유 : 구조
        string[] type = Regex.Split(lines[index + 1], SPLIT_RE);

        int l_rowSize = header.Length;

        int lenLine = lines.Length;
        for (int i = index + 2; i < lenLine; ++i) {
            string[] values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length < l_rowSize || values[1] == "" || values[1] == "\0") {
                continue;
            }

            Dictionary<string, object> dict = new Dictionary<string, object>();
            int length = type.Length;
            for (int j = 0; j < length; ++j) {
                string key = header[j];

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
                else if (type[j].Contains("enum") == true) {
                    key = type[j] + '|' + header[j];
                }

                dict[key] = finalValue;
            }

            listData.Add(dict);
        }


        return listData;
    }

    // 파일 제작 기능
    static public string GenerateDataToString(string _csvText, string _className, out List<Dictionary<string, object>> _listData)
    {

        _listData = Load(_csvText);

        string rowMemberCode = "";
        // Enum에 대한 데이터를 맵핑해둔다.
        Dictionary<string, List<string>> l_dicEnumData = new Dictionary<string, List<string>>();

        foreach(var item in _listData[0]) {
            // Enum일 경우 두번째 있는 타입이 Enum의 명칭이 된다.
            if(item.Key.Contains("enum") == true) {
				l_dicEnumData.Add(item.Key, new List<string>());
            }

            string key = item.Key;
            if (key == "" || key[0] == '\0') {
                continue;
            }
            if (key.Contains("enum") == true) {
                string[] splitEnum = key.Split('|');
                rowMemberCode += string.Format("\t\tpublic {1} {0};\n", splitEnum[2], splitEnum[1]);
            }
            else {
                rowMemberCode += string.Format("\t\tpublic {1} {0};\n", key, item.Value.GetType().ToString());
            }
        }

        int l_size = _listData.Count;
        for (int i = 0; i < l_size; ++i) {
            // 해당하는 키의 데이터를 가져온다.
            foreach(var item in l_dicEnumData) {
				if(_listData[i].ContainsKey(item.Key) == true) {
                    // 겹치는 키가 있는지 확인한다.
                    bool l_check = false;

                    int l_keyCount = l_dicEnumData[item.Key].Count;
                    for (int j = 0; j < l_keyCount; ++j) {
						if (l_dicEnumData[item.Key][j].Contains(_listData[i][item.Key].ToString()) == true) {
                            l_check = true;
                        }
                    }

                    // 겹치는 값이 없으면 추가한다.
                    if(l_check == false) {
                        l_dicEnumData[item.Key].Add(_listData[i][item.Key].ToString());
                    }
				}
            }
        }

        string code = codeTemplate;
        code = code.Replace("$CLASS", _className);
        code = code.Replace("$ROW_MEMBER_CODE", rowMemberCode);

        string resultDefineEnum = "";

        // 만약 Enum이 있을 경우
        if (l_dicEnumData.Count > 0) {
            string enumCode = "";
            foreach (var item in l_dicEnumData) {
                string defineEnum = "public enum $ENUM { \n $DATA };";
                defineEnum = defineEnum.Replace("$ENUM", item.Key.Split('|')[1]);
                int size = item.Value.Count;
                for (int i = 0; i < size; ++i) {
                    enumCode += string.Format("\t{0},\n", item.Value[i]);
                }
                defineEnum = defineEnum.Replace("$DATA", enumCode);

                resultDefineEnum += defineEnum;
            }
        }

        code = code.Replace("$DEFINE_ENUM", resultDefineEnum);

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
            if (key.Contains("enum") == true) {
                string[] splitKey = key.Split('|');
                //info.rangeType = (range)Enum.Parse(typeof(range), (string)item["enum|range|rangeType"]);
                rowMemberCode += string.Format("\t\t\tinfo.{0} = ({1})Enum.Parse(typeof({1}), (string)item[\"{2}\"]);\n", splitKey[2], splitKey[1], key);
            }
            else {
                rowMemberCode += string.Format("\t\t\tinfo.{0} = ({1})item[\"{0}\"];\n", key, data.Value.GetType().ToString());
            }
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

