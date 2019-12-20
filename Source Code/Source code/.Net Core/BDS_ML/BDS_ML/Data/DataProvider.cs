using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BDS_ML.Models.ModelDB;

namespace BDS_ML.Data
{
    public class DataProvider
    {
        private static DataProvider _ins;

        public static DataProvider Ins
        {
            get
            {
                if (_ins == null)
                    _ins = new DataProvider();
                return _ins;
            }
            set
            {
                _ins = value;
            }
        }

        public BDT_MLDBContext  db { get; set; }

        private DataProvider()
        {
            db = new BDT_MLDBContext();
        }
    }
}
