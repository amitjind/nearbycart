using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;

namespace PrivatesquaresWebApiNew.CommonCls
{
    public static class CovnertJsonToDataTable
    {
        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();

            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, prop.PropertyType);
            }

            object[] values = new object[props.Count];

            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }

        public static T ConfigSetting<T>(string value)
        {
            object objvalue =value;
            return (T)Convert.ChangeType(value, typeof(T));
        }
        #region Comment Code For DataSet To Json 
        //DataTable dt = new DataTable();
        //dt.Clear();
        //dt.Columns.Add("UserId");
        //dt.Columns.Add("InterestId");
        //dt.Columns.Add("InterestCatId");
        //DataRow _ravi;
        //_ravi = dt.NewRow();
        //_ravi["UserId"] = "1";
        //_ravi["InterestId"] = "2";
        //_ravi["InterestCatId"] = "3";
        //dt.Rows.Add(_ravi);
        //_ravi = dt.NewRow();
        //_ravi["UserId"] = "1";
        //_ravi["InterestId"] = "3";
        //_ravi["InterestCatId"] = "4";
        //dt.Rows.Add(_ravi);
        //_ravi = dt.NewRow();
        //_ravi["UserId"] = "1";
        //_ravi["InterestId"] = "4";
        //_ravi["InterestCatId"] = "5";
        //dt.Rows.Add(_ravi);
        //_ravi = dt.NewRow();
        //_ravi["UserId"] = "1";
        //_ravi["InterestId"] = "6";
        //_ravi["InterestCatId"] = "7";
        //dt.Rows.Add(_ravi);

        //string JSONresult;
        //JSONresult = JsonConvert.SerializeObject(dt);
        #endregion
        #region Comment Code For jsonToDataTable
        //List<UserInterestModel> usersList = JsonConvert.DeserializeObject<List<UserInterestModel>>(data);
        //DataTable dt1 = CovnertJsonToDataTable.ToDataTable<UserInterestModel>(usersList);
        //UserInterestModel ObjUserInterestModel = new UserInterestModel();
        //ObjUserInterestModel.DataTableUserInterested = dt1;

        #endregion
    }
}