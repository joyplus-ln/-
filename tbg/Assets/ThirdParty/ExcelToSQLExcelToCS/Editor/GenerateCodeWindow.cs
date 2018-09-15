using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
//using Rotorz.ReorderableList;
using System.Text;
using System;
using System.IO;
using System.Reflection;
using Mono.Data.Sqlite;

namespace Quick.Code
{
    public class GenerateCodeWindow : EditorWindow
    {

        [MenuItem("WordExcelEditor/Excel生成脚本&&SQL")]
        public static void OpenWindow()
        {
            if (codeWindow == null)
                codeWindow = EditorWindow.GetWindow(typeof(GenerateCodeWindow)) as GenerateCodeWindow;

            codeWindow.titleContent = new GUIContent("Excel生成代码&&SQL");
            codeWindow.Show();

        }

        private static GenerateCodeWindow codeWindow = null;
        private SerializedObject serializedObj;

        //选择的根游戏体
        private UnityEngine.Object root;
        private UnityEngine.Object cspath;
        //视图宽度一半
        private float halfViewWidth;
        //视图高度一半
        private float halfViewHeight;

        private Vector2 scrollWidgetPos;
        private Vector2 scrollObjectPos;
        private Vector2 scrollTextPos;
        private Vector2 scrollSQLTextPos;

        private int selectedBar = 0;
        private bool isMono = true;

        #region 代码生成
        private StringBuilder codeStateText;
        private StringBuilder codeAllText;
        private StringBuilder sqlStateText;


        private string className;

        #endregion



        void OnEnable()
        {
            serializedObj = new SerializedObject(this);
        }

        void OnGUI()
        {
            serializedObj.Update();

            if (codeWindow == null)
            {
                codeWindow = GetWindow<GenerateCodeWindow>();
            }
            halfViewWidth = EditorGUIUtility.currentViewWidth / 2f;
            halfViewHeight = codeWindow.position.height / 2f;

            using (new EditorGUILayout.HorizontalScope())
            {
                //左半部分
                using (EditorGUILayout.VerticalScope vScope = new EditorGUILayout.VerticalScope(GUILayout.Width(EditorGUIUtility.currentViewWidth * 0.5f)))
                {
                    GUI.backgroundColor = Color.white;
                    Rect rect = vScope.rect;
                    rect.height = codeWindow.position.height;
                    GUI.Box(rect, "");

                    DrawSelectUI();
                    DrawFindWidget();

                }
                //右半部分
                using (new EditorGUILayout.VerticalScope(GUILayout.Width(EditorGUIUtility.currentViewWidth * 0.5f)))
                {
                    DrawCodeGenTitle();
                    DrawCodeGenToolBar();
                }
            }

            serializedObj.ApplyModifiedProperties();
        }

        /// <summary>
        /// 绘制 选择要分析的UI
        /// </summary>
        private void DrawSelectUI()
        {
            EditorGUILayout.Space();
            using (EditorGUILayout.HorizontalScope hScope = new EditorGUILayout.HorizontalScope())
            {
                GUI.backgroundColor = Color.white;
                Rect rect = hScope.rect;
                rect.height = EditorGUIUtility.singleLineHeight;
                GUI.Box(rect, "");

                EditorGUILayout.LabelField("1.选择待处理Excel*:", GUILayout.Width(halfViewWidth / 4f));
                root = EditorGUILayout.ObjectField(root, typeof(UnityEngine.Object), true);
            }
            EditorGUILayout.Space();
            using (EditorGUILayout.HorizontalScope hScope = new EditorGUILayout.HorizontalScope())
            {
                GUI.backgroundColor = Color.white;
                Rect rect = hScope.rect;
                rect.height = EditorGUIUtility.singleLineHeight;
                GUI.Box(rect, "");
                EditorGUILayout.LabelField("2.选择脚本生成路径*:", GUILayout.Width(halfViewWidth / 4f));
                cspath = EditorGUILayout.ObjectField(cspath, typeof(UnityEngine.Object), true);
            }
        }
        Dictionary<int, Dictionary<int, string>> map = null;
        string text = "";
        string textright = "";
        Vector2 scroll;
        int[] sizes = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        string[] names = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        int selectedSize = 0;
        int selectedclazzSize = 0;
        int startSqlFrom = 0;
        private int currentTableIndex = 0;
        /// <summary>
        /// 绘制 查找UI控件
        /// </summary>
        private void DrawFindWidget()
        {

            EditorGUILayout.Space();
            using (EditorGUILayout.HorizontalScope hScope = new EditorGUILayout.HorizontalScope())
            {
                GUI.backgroundColor = Color.white;
                Rect rect = hScope.rect;
                rect.height = EditorGUIUtility.singleLineHeight + 80;
                GUI.Box(rect, "");
                EditorGUILayout.BeginVertical();
                selectedclazzSize = EditorGUILayout.IntPopup("5.选择类型行: " + selectedclazzSize.ToString(), selectedclazzSize, names, sizes);
                EditorGUILayout.Space();
                selectedSize = EditorGUILayout.IntPopup("6.选择行: " + selectedSize.ToString(), selectedSize, names, sizes);
                EditorGUILayout.Space();
                className = EditorGUILayout.TextField("3.填写待生成的类名(同表明)*:", className);
                EditorGUILayout.EndVertical();

                if (GUILayout.Button("4.解析EXCEL(先解析再生成)", GUILayout.Width(halfViewWidth / 2f)))
                {
                    if (root == null)
                    {
                        Debug.LogWarning("请先选择一个Excel!");
                        return;
                    }
                    EditorGUILayout.Space();

                    //        pathRoot = Application.dataPath;
                    //注意这里需要对路径进行处理
                    //目的是去除Assets这部分字符以获取项目目录
                    //我表示Windows的/符号一直没有搞懂
                    //pathRoot = pathRoot.Substring(0, pathRoot.LastIndexOf("/"));
                    string objPath = AssetDatabase.GetAssetPath(root);
                    map = RecursiveExcelUI(objPath);


                }

            }
            using (EditorGUILayout.HorizontalScope hScope2 = new EditorGUILayout.HorizontalScope())
            {

                if (tableDictionary != null && tableDictionary.Count > 0)
                {
                    foreach (var key in tableDictionary.Keys)
                    {
                        if (GUILayout.Button(key + tableDictionary[key] + (currentTableIndex == key ? "(当前)" : "")))
                        {
                            map = excelutils.ConvertToMap(key);
                            currentTableIndex = key;
                        }
                    }
                }
            }
            EditorGUILayout.Space();        // 空一行
            if (map == null) return;

            text = "";
            textright = "";
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            using (new EditorGUILayout.HorizontalScope())
            {
                //右半部分
                using (new EditorGUILayout.VerticalScope(GUILayout.Width(200)))
                {
                    GUILayout.Label("类型");
                    foreach (var tkey in map[selectedclazzSize].Keys)
                    {
                        textright = map[selectedclazzSize][tkey];
                        GUILayout.Label(textright);
                    }
                }
                //左半部分
                using (EditorGUILayout.VerticalScope vScope = new EditorGUILayout.VerticalScope(GUILayout.Width(100)))
                {
                    GUILayout.Label("变量名");
                    foreach (var tkey in map[selectedSize].Keys)
                    {
                        text = map[selectedSize][tkey];
                        GUILayout.Label(text);
                    }
                }


            }
        }



        private void DrawCodeGenTitle()
        {
            EditorGUILayout.Space();
            using (var hScope = new EditorGUILayout.HorizontalScope(GUILayout.Height(EditorGUIUtility.singleLineHeight)))
            {
                GUI.backgroundColor = Color.white;
                Rect rect = hScope.rect;
                GUI.Box(rect, "");

                EditorGUILayout.LabelField("代码生成:");
            }
        }

        private void DrawCodeGenToolBar()
        {
            EditorGUILayout.Space();

            selectedBar = GUILayout.Toolbar(selectedBar, new string[] { "C#", "生成SQL" });

            switch (selectedBar)
            {
                case 0:
                    DrawCsPage();
                    break;
                case 1:
                    DrawSQLPage();
                    break;

                default:
                    break;
            }
        }

        private void DrawCsPage()
        {
            EditorGUILayout.Space();
            isMono = GUILayout.Toggle(isMono, "继承MonoBehaviour");
            EditorGUILayout.Space();
            if (GUILayout.Button("变量声明", GUILayout.Width(halfViewWidth / 3f)))
            {
                BuildStatementCode();
            }

            EditorGUILayout.Space();
            using (EditorGUILayout.HorizontalScope hScope = new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("预览代码"))
                {
                    BuildStatementCode();
                }
                if (GUILayout.Button("复制代码"))
                {
                    TextEditor p = new TextEditor();
                    codeAllText = new StringBuilder(codeStateText.ToString());
                    p.text = codeAllText.ToString();
                    p.OnFocus();
                    p.Copy();

                    EditorUtility.DisplayDialog("提示", "代码复制成功", "OK");
                }
                if (GUILayout.Button("生成脚本"))
                {
                    CreateCsUIScript();
                }
            }

            EditorGUILayout.Space();
            DrawPreviewText();
        }

        private void DrawSQLPage()
        {
            EditorGUILayout.Space();

            using (EditorGUILayout.HorizontalScope hScope = new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("预览表"))
                {
                    BuildSimpleTable();
                }

                if (GUILayout.Button("生成数据库"))
                {
                    CreatSQLLite3();
                }
            }
            EditorGUILayout.Space();
            startSqlFrom = EditorGUILayout.IntPopup("选择从哪一行倒库: " + startSqlFrom.ToString(), startSqlFrom, names, sizes);
            EditorGUILayout.Space();

            EditorGUILayout.Space();
            DrawSQLPreviewText();
        }


        private Dictionary<int, string> tableDictionary;
        private ExcelSQLUtility excelutils;
        public Dictionary<int, Dictionary<int, string>> RecursiveExcelUI(string path)
        {
            excelutils = new ExcelSQLUtility(path);
            tableDictionary = excelutils.GetTableDictionary();
            Dictionary<int, Dictionary<int, string>> map = excelutils.ConvertToMap();
            return map;
        }

        private string BuildStatementCode()
        {


            codeStateText = null;
            codeStateText = new StringBuilder();

            CreatCode();
            codeStateText.Append(code1);
            return codeStateText.ToString();
        }

        private void BuildSimpleTable()
        {

            sqlStateText = null;
            sqlStateText = new StringBuilder();
            string tables = "";
            //\n
            foreach (var key in map.Keys)
            {
                if (key < startSqlFrom) continue;
                tables += String.Format("第{0}行:\n", key);
                foreach (var tkey in map[key].Keys)
                {
                    tables += String.Format("       第{0}列[{1}]:{2}\n", tkey, map[selectedclazzSize][tkey], map[key][tkey]);
                }
            }
            sqlStateText.Append(tables);
        }
        /// <summary>
        /// 当前操作生成的代码预览
        /// </summary>
        private void DrawPreviewText()
        {
            EditorGUILayout.Space();

            using (var ver = new EditorGUILayout.VerticalScope())
            {
                GUI.backgroundColor = Color.white;
                GUI.Box(ver.rect, "");

                EditorGUILayout.HelpBox("代码预览:", MessageType.None);
                using (var scr = new EditorGUILayout.ScrollViewScope(scrollTextPos))
                {
                    scrollTextPos = scr.scrollPosition;

                    if (codeStateText != null && !string.IsNullOrEmpty(codeStateText.ToString()) && selectedBar == 0)
                    {
                        //GUILayout.TextArea(codeStateText.ToString());
                        GUILayout.Label(codeStateText.ToString());
                    }
                }
            }
        }
        /// <summary>
        /// 当前操作生成的代码预览
        /// </summary>
        private void DrawSQLPreviewText()
        {
            EditorGUILayout.Space();

            using (var ver = new EditorGUILayout.VerticalScope())
            {
                GUI.backgroundColor = Color.white;
                GUI.Box(ver.rect, "");

                EditorGUILayout.HelpBox("Excel表预览:", MessageType.None);
                using (var scr = new EditorGUILayout.ScrollViewScope(scrollSQLTextPos))
                {
                    scrollSQLTextPos = scr.scrollPosition;
                    if (sqlStateText != null && !string.IsNullOrEmpty(sqlStateText.ToString()) && selectedBar == 1)
                    {
                        //GUILayout.TextArea(codeStateText.ToString());
                        GUILayout.Label(sqlStateText.ToString());
                    }
                }
            }
        }

        /// <summary>
        /// 生成C#脚本
        /// </summary>
        private void CreateCsUIScript()
        {
            string pathcs = AssetDatabase.GetAssetPath(cspath);
            if (string.IsNullOrEmpty(pathcs)) return;
            Debug.Log(Application.dataPath + pathcs.Replace("Assets", "") + "/" + className + ".cs");
            WriteClass(Application.dataPath + pathcs.Replace("Assets", "") + "/" + className + ".cs", className, code1);
            AssetDatabase.Refresh();


        }
        private static void WriteClass(string path, string className, string code)
        {
            System.IO.File.WriteAllText(path, code, System.Text.UnicodeEncoding.UTF8);
        }
        private string code1 = "";
        public void CreatCode()
        {
            code1 = "";

            code1 += "using System;\n";
            code1 += "using System.Collections;\n";
            code1 += "using System.Collections.Generic;\n";
            code1 += "using System.IO;\n";
            code1 += "using UnityEngine;\n";

            code1 += "public class " + className + "\n";
            code1 += "{\n";

            foreach (var key in map[selectedSize].Keys)
            {
                string name = map[selectedSize][key];
                string type = map[selectedclazzSize][key];
                code1 += "    public " + type + " " + name + " { get; set; }\n";
            }
            code1 += "\n    public " + className + "()\n";
            code1 += "    {}\n";

            code1 += "}\n";
        }

        private SqliteConnection connection;
        public string dbPath = "WordConfig.sqlite3";
        private string sqlitepath = "";
        public void CreatSQLLite3()
        {
            sqlitepath = Application.dataPath + AssetDatabase.GetAssetPath(cspath).Replace("Assets", "") + "/" + dbPath;
            Debug.Log(sqlitepath);
            if (!File.Exists(sqlitepath))
            {
                SqliteConnection.CreateFile(sqlitepath);
                AssetDatabase.Refresh();
            }
            connection = new SqliteConnection("URI=file:" + sqlitepath);
            string sqlLine = MakeSQLLine();
            Debug.Log(sqlLine);
            ExecuteNonQuery(@sqlLine);
            //ExecuteNonQuery(@"SET NAMES 'gb2312'");
            string insertLine = "";
            foreach (var key in map.Keys)
            {
                if (key < startSqlFrom) continue;
                insertLine = MakeInsertSQLLine(key);
                Debug.Log(insertLine);
                Debug.Log(ExecuteNonQuery(@insertLine));
            }
            Debug.Log("数据库操作完成");
        }

        /// <summary>
        /// 返回行数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        public int ExecuteNonQuery(string sql, params SqliteParameter[] args)
        {
            int id = 0;
            connection.Open();
            using (var cmd = new SqliteCommand(sql, connection))
            {
                foreach (var arg in args)
                {
                    cmd.Parameters.Add(arg);
                }
                id = cmd.ExecuteNonQuery();
            }
            connection.Close();
            return id;
        }

        public string MakeSQLLine()
        {
            StringBuilder line = new StringBuilder();
            line.AppendFormat("CREATE TABLE IF NOT EXISTS {0} ( id INTEGER NOT NULL PRIMARY KEY,", className);
            foreach (var key in map[selectedclazzSize].Keys)
            {
                if (map[selectedSize][key] == "id") continue;
                line.AppendFormat("{0} {1},", map[selectedSize][key], map[selectedclazzSize][key]);
            }
            line.Remove(line.Length - 1, 1);
            line.AppendFormat("{0}", ")");
            return line.ToString();
        }
        public string MakeInsertSQLLine(int id)
        {
            StringBuilder line = new StringBuilder();
            line.AppendFormat("INSERT INTO {0} (", className);
            foreach (var key in map[selectedclazzSize].Keys)
            {
                line.AppendFormat("{0},", map[selectedSize][key]);
            }
            line.Remove(line.Length - 1, 1);
            line.AppendFormat("{0}", ")");
            line.AppendFormat(" VALUES (");
            foreach (var key in map[id].Keys)
            {
                byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(map[id][key]);
                String str = System.Text.Encoding.UTF8.GetString(byteArray);
                line.AppendFormat("'{0}',", str);
            }
            line.Remove(line.Length - 1, 1);
            line.AppendFormat("{0}", ")");
            return line.ToString();
        }
    }
}
