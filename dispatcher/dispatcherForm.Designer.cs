using shareBike;
using Sunny.UI;
using System.Collections;
using System.Windows.Forms;

namespace dispatcher_Form
{
    partial class dispatcherForm
    {
        #region 逻辑代码成员变量
        //本调度员正在处理的id数组
        private ArrayList processing = new ArrayList();//int
        /*检索时先检索这里，且类型符合的；再检索其他未处理的
        处理时不删掉
        显示的时候默认是选中状态。
        */

        //调度员id
        string dispatcher_id;
        #endregion


    }
}