using shareDemo2;
using System.Windows.Forms;

namespace login_register_Form1
{
    partial class registerForm
    {
        #region 数据库成员变量
        public DBDataContext dc;
        #endregion

        #region 成员变量
        private bool tb_id_hasText = false;
        private bool tb_name_hasText = false;
        private bool tb_password_hasText = false;
        #endregion
    }
}