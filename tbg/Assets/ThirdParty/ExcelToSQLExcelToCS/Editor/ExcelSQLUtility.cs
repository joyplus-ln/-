using System.Collections.Generic;
using System.IO;
using System.Data;
using UnityEditor;
using Excel;
public class ExcelSQLUtility
{

    /// <summary>
    /// 表格数据集合
    /// </summary>
    private DataSet m_ResultSet;



    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="excelFile">WordExcel file.</param>
    public ExcelSQLUtility(string excelFile)
    {

        FileStream mStream = File.Open(excelFile, FileMode.Open, FileAccess.Read);
        IExcelDataReader mExcelReader = ExcelReaderFactory.CreateOpenXmlReader(mStream);
        m_ResultSet = mExcelReader.AsDataSet();
    }

    public Dictionary<int, string> GetTableDictionary()
    {
        //判断Excel文件中是否存在数据表
        if (m_ResultSet.Tables.Count < 1)
            return null;
        Dictionary<int, string> tableDictionary = new Dictionary<int, string>();

        for (int i = 0; i < m_ResultSet.Tables.Count; i++)
        {
            tableDictionary.Add(i, m_ResultSet.Tables[i].TableName);
        }
        return tableDictionary;
    }

    public Dictionary<int, Dictionary<int, string>> ConvertToMap(int tableindex = 0)
    {
        //判断Excel文件中是否存在数据表
        if (m_ResultSet.Tables.Count < 1)
            return null;
        //默认读取第一个数据表
        DataTable mSheet = m_ResultSet.Tables[tableindex];

        //判断数据表内是否存在数据
        if (mSheet.Rows.Count < 1)
            return null;

        //读取数据表行数和列数
        int rowCount = mSheet.Rows.Count;
        int colCount = mSheet.Columns.Count;

        //准备一个列表以保存全部数据
        Dictionary<int, Dictionary<int, string>> list = new Dictionary<int, Dictionary<int, string>>();

        //读取数据
        for (int i = 0; i < rowCount; i++)
        {
            //创建实例
            Dictionary<int, string> rowList = new Dictionary<int, string>();
            for (int j = 0; j < colCount; j++)
            {
                //读取第1行数据作为表头字段
                //string field = mSheet.Rows[1][j].ToString();
                object value = mSheet.Rows[i][j];
                //设置属性值
                if (!rowList.ContainsKey(j))
                {
                    rowList.Add(j, value.ToString());
                }
            }
            list.Add(i, rowList);
            //添加至列表
        }

        return list;
    }


}


