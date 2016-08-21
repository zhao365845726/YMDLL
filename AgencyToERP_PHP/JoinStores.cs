using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMDLL.Class;
using YMDLL.Common;

namespace AgencyToERP_PHP
{
    public class JoinStores : Base
    {
        /// <summary>
        /// 加盟店类的构造函数
        /// </summary>
        public JoinStores()
        {
            //sTableName = "Position";
            //sColumns = "PositionName,FlagDeleted,PositionID";
            //sOrder = "PositionID";
            dTableName = "erp_join_stores";
            dColumns = "fstores_name,fresponsible_name,Fif_deleted,fcreate_time,fupdate_time,Fcompany_id,fsubordinate_dept_id,FSTATUS,Fsecond_hand_proportion,Fhand_room_proportion";
        }

        public void importJoinStores()
        {
            string m_SQL = "INSERT INTO " + dTableName +  " (" + dColumns + ") " + 
                "SELECT dept_name,manager_user_name," + dDeleteMark + ",create_time,update_time," + dCompanyId + ",pid,'有效',4,7 FROM erp_department WHERE dept_type = '店'; ";
            _mysql.ExecuteSQL(m_SQL);
        }
    }
}
