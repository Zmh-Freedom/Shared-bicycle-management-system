using shareDemo2;
using System.Drawing;
using System.Windows.Forms;

namespace shareDemo3
{
    partial class managerForm
    {
        
        #region 数据库成员变量
        DBDataContext dc;
        #endregion

        #region 绘图变量
        Graphics g = null;
        SolidBrush blueBrush = null;
        SolidBrush shadowBrush = null;
        SolidBrush goldBrush = null;
        SolidBrush silverBrush = null;
        SolidBrush greenBrush = null;
        #endregion

        #region 逻辑代码成员变量
        
        string manager_nickname = "xuhao";
        int numUsing = 0;
        int numAvailable = 0;
        int numToOverhaul = 0;
        int numOutServiceArea = 0;

        //框选区域的参数
        int origin_SelectX = 0;
        int origin_SelectY = 0;
        int selectWidth = 0;
        int selectHeight = 0;
        
        bool isMouseDown = false;
        Rectangle myRect;
        
        #endregion

    }
}

