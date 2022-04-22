using shareBike;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace shareBike
{
    partial class managerForm
    {
        
        #region 数据库成员变量
        public DBDataContext dc;
        #endregion

        #region 绘图变量
        Graphics g = null;
        SolidBrush blueBrush = null;
        SolidBrush blueShadowBrush = null;
        SolidBrush lightOrangeShadowBrush = null;
        SolidBrush silverShadowBrush = null;
        SolidBrush orangeShadowBrush = null;
        SolidBrush goldBrush = null;
        SolidBrush silverBrush = null;
        SolidBrush greenBrush = null;
        SolidBrush redBrush = null;
        Graphics gNewTask = null;
        Graphics Trend = null;

        SolidBrush bike_stratBrush = null;
        SolidBrush bike_endBrush = null;
        ArrayList PointList;
        #endregion

        #region 逻辑代码成员变量
        private string myid;

        string manager_nickname = "xuhao";
        int numUsing = 0;
        int numAvailable = 0;
        int numToOverhaul = 0;
        int numOutServiceArea = 0;

        //用车热图页
        //框选区域的参数
        int origin_SelectX = 0;
        int origin_SelectY = 0;
        int selectWidth = 0;
        int selectHeight = 0;
        bool isMouseDown = false;
        Rectangle myRect;

        //下达任务页
        int recovery_SelectX = 0;
        int recovery_SelectY = 0;
        int recoveryWidth = 0;
        int recoveryHeight = 0;
        bool recMouseDown = false;
        Rectangle recoveryRect;

        int src_SelectX = 0;
        int src_SelectY = 0;
        int srcWidth = 0;
        int srcHeight = 0;
        int dst_SelectX = 0;
        int dst_SelectY = 0;
        int dstWidth = 0;
        int dstHeight = 0;
        Rectangle srcRect;
        Rectangle dstRect;
        bool srcMouseDown = false;
        bool dstMouseDown = false;
        bool srcDone = false;

        //智能调度页
        bool AddAIAreaButtonDown = false;
        bool DrawAIAreaMouseDown = false;
        bool getSelectedArea = false;
        int ai_SelectX = 0;
        int ai_SelectY = 0;
        int aiWidth = 0;
        int aiHeight = 0;
        Rectangle aiRect;
        fence tempAIarea;
        #endregion
    }
}

