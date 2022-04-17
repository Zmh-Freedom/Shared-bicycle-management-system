using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using shareDemo2;

namespace DBmanager
{
    public class DBmanager
    {
        
        //创建智能调度时段-需求视图
        //由于参数设置为getdate（），会自动更新
        /*参数说明（地理范围（x,y,宽，高）；时间间隔（类型，长度)；时间长度）
        *   
        *   时间间隔类型interval_type：minute，hour，day，month，year
        *   时间长度time_length = n：即n个interval_type
        */
        /*使用时机：
         *      创建视图由围栏增、开的button调用；
         *      关、删时drop视图
         * 视图名："view"_fenceid_时间间隔类型_长度
        */

        public static void create_period_need_view(int x, int y, int width, int height, string interval_type, int interval_length, int time_length)
        {
            try
            {
                string conString = "Data Source=localhost;Initial Catalog=shareBike;Integrated Security=True";

                using (DBDataContext dc = new DBDataContext(conString))
                {
                    string command = "";
                    //生成sql命令

                    string time0;
                    time0 = "DATEADD(" + interval_type + ",-" + time_length.ToString() + ",GETDATE())";

                    string view_name = "";
                    //查fence_id
                    var fence_id = from p in dc.fence
                                   where p.origin_x == x && p.origin_y == y && p.width == width && p.height == height
                                   select p.id;
                    view_name = "view_" + fence_id.First().ToString() + "_" + interval_type + "_" + interval_length.ToString();

                    string startORend = "start";

                    string cycle;
                    if (interval_type == "year")
                        cycle = "10000000";
                    else if (interval_type == "month")
                        cycle = "12";
                    else if (interval_type == "day")
                        cycle = "30";
                    else if (interval_type == "hour")
                        cycle = "24";
                    else if (interval_type == "minute")
                        cycle = "60";
                    else
                        cycle = "60";

                    Console.WriteLine(cycle);

                    string period = "(DATEPART(" + interval_type + "," + startORend + "_time)-DATEPART(" + interval_type + "," + time0 + ")+" + cycle + ")%" + cycle + "/" + interval_length.ToString();

                    command = "create view " + view_name
                        + "(period,need)as select  " + period + ",count(*) from shareBike.dbo.orderform where "
                        + startORend + "_x between " + x.ToString() + " and " + (x + width).ToString() + " and "
                        + startORend + "_y between " + y.ToString() + " and " + (y + height).ToString() + " and "
                        + startORend + "_time between " + time0 + " and GETDATE()"
                        + " group by " + period;
                    //若存在，先删除
                    dc.ExecuteCommand("drop view if exists " + view_name);
                    //创建视图
                    dc.ExecuteCommand(command);
                    // Console.WriteLine(command);
                }

            }
            catch (Exception ex)
            {

            }
        }
        //删除智能调度视图 interval_type小写英文
        public static void drop_period_need_view(int fence_id, string interval_type, int interval_length)
        {
            string conString = "Data Source=localhost;Initial Catalog=shareBike;Integrated Security=True";

            using (DBDataContext dc = new DBDataContext(conString))
            {
                string command = "drop view if exists view_" + fence_id.ToString() + "_" + interval_type + "_" + interval_length.ToString();
                dc.ExecuteCommand(command);
            }
        }
        //注册：将新顾客入库(成功返回true，失败返回false)//0顾客，1管理员，2调度员
        public static bool register(int user_type, string t_id, string t_name, string t_password)
        {
            try
            {
                DBDataContext dc = new DBDataContext();//创建数据库对象
                if (user_type == 0)
                {
                    //查询id是否存在
                    int idCount = (from p in dc.customer
                                   where t_id == p.id
                                   select p).Count();
                    if (idCount > 0) return false;
                    //生成新记录
                    customer new_customer = new customer() { id = t_id.ToString().PadLeft(5, '0'), password = t_password, nickname = t_name };
                    dc.customer.InsertOnSubmit(new_customer);
                    dc.SubmitChanges();
                }
                else if (user_type == 1)
                {

                    //查询id是否存在
                    int idCount = (from p in dc.manager
                                   where t_id == p.id
                                   select p).Count();
                    if (idCount > 0) return false;
                    //生成新记录
                    manager new_manager = new manager() { id = t_id.ToString().PadLeft(5, '0'), password = t_password, nickname = t_name };
                    dc.manager.InsertOnSubmit(new_manager);
                    dc.SubmitChanges();
                }
                else if (user_type == 2)
                {

                    //查询id是否存在
                    int idCount = (from p in dc.dispatcher
                                   where t_id == p.id
                                   select p).Count();
                    if (idCount > 0) return false;
                    //生成新记录
                    Dispatcher new_dispatcher = new Dispatcher() { id = t_id.ToString().PadLeft(5, '0'), password = t_password, nickname = t_name };
                    dc.dispatcher.InsertOnSubmit(new_dispatcher);
                    dc.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return true;
        }

        //删除用户数据//0顾客，1管理员，2调度员
        public static void delete_user(int user_type, ArrayList user_id)
        {
            try
            {
                DBDataContext dc = new DBDataContext();//创建数据库对象
                foreach (string theID in user_id)
                {
                    if (user_type == 0)
                    {
                        var users = (from p in dc.customer
                                     where theID == (p.id.ToString())
                                     select p);
                        //dc.customer.DeleteAllOnSubmit(users);
                        dc.customer.DeleteOnSubmit(users.First());
                        dc.SubmitChanges();
                    }
                    else if (user_type == 1)
                    {
                        var users = (from p in dc.manager
                                     where theID == (p.id.ToString())
                                     select p);
                        dc.manager.DeleteAllOnSubmit(users);
                        dc.SubmitChanges();
                    }
                    else if (user_type == 2)
                    {
                        var users = (from p in dc.dispatcher
                                     where theID == (p.id.ToString())
                                     select p);
                        dc.dispatcher.DeleteAllOnSubmit(users);
                        dc.SubmitChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        //查询用户数据//0顾客，1管理员，2调度员
        public static ArrayList search_user(int user_type, string user_id, string user_password, string user_name)
        {
            ArrayList result = new ArrayList();
            try
            {
                DBDataContext dc = new DBDataContext();//创建数据库对象
                if (user_type == 0)//顾客
                {
                    if (user_id.Length == 0 && user_password.Length == 0 && user_name.Length == 0)
                    {
                        var t_user = (from p in dc.customer
                                      select p);
                        if (t_user != null)
                        {
                            foreach (var t in t_user)
                            {
                                result.Add(t.id);
                                result.Add(t.password);
                                result.Add(t.nickname);
                            }
                        }
                    }
                    if (user_id.Length > 0)
                    {
                        var t_user = (from p in dc.customer
                                      where p.id == user_id
                                      select p);
                        if (t_user != null)
                        {
                            foreach (var t in t_user)
                            {
                                result.Add(t.id);
                                result.Add(t.password);
                                result.Add(t.nickname);
                            }
                        }

                    }
                    else if (user_password.Length > 0)
                    {
                        var t_user = (from p in dc.customer
                                      where p.password == user_password
                                      select p);
                        foreach (var t in t_user)
                        {
                            result.Add(t.id);
                            result.Add(t.password);
                            result.Add(t.nickname);
                        }
                    }

                    else if (user_name.Length > 0)
                    {
                        var t_user = (from p in dc.customer
                                      where p.nickname == user_name
                                      select p);
                        foreach (var t in t_user)
                        {
                            result.Add(t.id);
                            result.Add(t.password);
                            result.Add(t.nickname);
                        }
                    }

                }
                else if (user_type == 1)//管理员
                {
                    if (user_id.Length == 0 && user_password.Length == 0 && user_name.Length == 0)
                    {
                        var t_user = (from p in dc.manager
                                      select p);
                        foreach (var t in t_user)
                        {
                            result.Add(t.id);
                            result.Add(t.password);
                            result.Add(t.nickname);
                        }
                    }
                    if (user_id.Length > 0)
                    {
                        var t_user = (from p in dc.manager
                                      where p.id == user_id
                                      select p);
                        foreach (var t in t_user)
                        {
                            result.Add(t.id);
                            result.Add(t.password);
                            result.Add(t.nickname);
                        }
                    }

                    else if (user_password.Length > 0)
                    {
                        var t_user = (from p in dc.manager
                                      where p.password == user_password
                                      select p);
                        foreach (var t in t_user)
                        {
                            result.Add(t.id);
                            result.Add(t.password);
                            result.Add(t.nickname);
                        }
                    }

                    else if (user_name.Length > 0)
                    {
                        var t_user = (from p in dc.manager
                                      where p.nickname == user_name
                                      select p);
                        foreach (var t in t_user)
                        {
                            result.Add(t.id);
                            result.Add(t.password);
                            result.Add(t.nickname);
                        }
                    }

                }
                else if (user_type == 2)//调度员
                {
                    if (user_id.Length == 0 && user_password.Length == 0 && user_name.Length == 0)
                    {
                        var t_user = (from p in dc.dispatcher
                                      select p);
                        foreach (var t in t_user)
                        {
                            result.Add(t.id);
                            result.Add(t.password);
                            result.Add(t.nickname);
                        }
                    }

                    if (user_id.Length > 0)
                    {
                        var t_user = (from p in dc.dispatcher
                                      where p.id == user_id
                                      select p);
                        foreach (var t in t_user)
                        {
                            result.Add(t.id);
                            result.Add(t.password);
                            result.Add(t.nickname);
                        }
                    }

                    else if (user_password.Length > 0)
                    {
                        var t_user = (from p in dc.dispatcher
                                      where p.password == user_password
                                      select p);
                        foreach (var t in t_user)
                        {
                            result.Add(t.id);
                            result.Add(t.password);
                            result.Add(t.nickname);
                        }
                    }

                    else if (user_name.Length > 0)
                    {
                        var t_user = (from p in dc.dispatcher
                                      where p.nickname == user_name
                                      select p);
                        foreach (var t in t_user)
                        {
                            result.Add(t.id);
                            result.Add(t.password);
                            result.Add(t.nickname);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }


            return result;
        }

        //调度员操作
        /* 未处理的taskid和bike.id在参数中的序号  (待处理的..，         检查条件，   bike.flag改成，.time.，坐标， task.flag, 结束时间, 调度员)
        * 负数表示不用改，坐标0改(0,0)，1改task.loc
        * Noflag==4,要检查任务类型
        * */
        public static ArrayList dispatcher_Work(ArrayList tasks_bike_id,  int bFlagTo, int bTimeTo, int bLocTo, int tFlagTo, DateTime tEndtimeTo, string tHandlerTo)
        {
            int Noflag = 3;
            ArrayList result = new ArrayList();
            try
            {
                DBDataContext dc = new DBDataContext();//创建数据库对象
                //对每条任务
                for (int i = 0; i < tasks_bike_id.Count; i++)
                {
                    //检查Noflag
                    var t_bike = (from p in dc.bike
                                  where p.id == ((int[])(tasks_bike_id[i]))[1]
                                  select p);
                    if (t_bike.First().flag == Noflag)
                    {
                        result.Add(i);
                        continue;
                    }
                    var t_task = (from p in dc.task
                                  where p.id == ((int[])(tasks_bike_id[i]))[0]
                                  select p);
                    if (tFlagTo == 1 && t_task.First().flag == 2)//调度任务开始处理, 已经完成的不再开始
                    {
                        result.Add(i);
                        continue;
                    }
                    //开始处理
                    //bike
                    if (t_task.First().flag == 3)
                        t_bike.First().flag = 4;
                    else
                        t_bike.First().flag = (byte?)bFlagTo;//bike.flag
                    if(bTimeTo>=0) t_bike.First().total_time = bTimeTo;//bike.total_time
                    if (bLocTo == 0) t_bike.First().current_x = t_bike.First().current_y= 0;//bike位置
                    else if(bLocTo == 1)
                    {
                        t_bike.First().current_x = t_task.First().end_x;
                        t_bike.First().current_y = t_task.First().end_y;
                    }
                    //公共部分
                    //task
                    t_task.First().flag = (byte?)tFlagTo;
                    if(tFlagTo != 1) t_task.First().end_time = tEndtimeTo;//1是处理中
                    t_task.First().handler = tHandlerTo;
                    //提交
                    dc.SubmitChanges();
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            
            return result;
        }

        public static ArrayList dispatch_End(ArrayList tasks_bike_id, DateTime tEndtimeTo, string tHandlerTo)
        {
            ArrayList result = new ArrayList();
            try
            {
                DBDataContext dc = new DBDataContext();//创建数据库对象
                //对每条任务
                for (int i = 0; i < tasks_bike_id.Count; i++)
                {
                    var t_bike = (from p in dc.bike
                                  where p.id == ((int[])(tasks_bike_id[i]))[1]
                                  select p);
                    var t_task = (from p in dc.task//任务必须是处理中且处理人是我
                                  where p.id == ((int[])(tasks_bike_id[i]))[0]
                                  select p);
                    if (t_task.First().flag != 1 || t_task.First().handler != tHandlerTo)
                    {
                        result.Add(i);
                        continue;
                    }
                    //bike
                    if (t_task.First().tag == 2 || t_task.First().tag == 3)//调度、投放
                    {
                        t_bike.First().flag = 0;
                        t_bike.First().current_x = t_task.First().end_x;
                        t_bike.First().current_y = t_task.First().end_y;
                    }
                    else if (t_task.First().tag == 4)//回收
                    {
                        t_bike.First().flag = 5;
                        t_bike.First().total_time = 0;
                        t_bike.First().current_x = t_bike.First().current_y = 0;
                    }
                    //公共部分
                    //task
                    t_task.First().flag = 2;
                    t_task.First().end_time = tEndtimeTo;
                    //提交
                    dc.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            return result;
        }

        //根据task_id查询对应的状态异常的bike_id
        public static ArrayList getAbnormalBike(ArrayList task_id)
        {
            ArrayList bike_id = new ArrayList();
            try
            {
                DBDataContext dc = new DBDataContext();//创建数据库对象
                foreach(int task in task_id)
                {
                    var t_bike = (from p in dc.task
                               where p.id == task
                               select p.bid);
                    if(t_bike != null)
                    {
                        IQueryable<bike> tt_bike = (from p in dc.bike
                                   where p.id == t_bike.First()
                                   select p);
                        //如果状态异常
                        if(tt_bike.First().flag !=0 && tt_bike.First().flag != 3 || tt_bike.First().total_time> 6000)
                        {
                            bike_id.Add(tt_bike.First().id);
                            //Console.WriteLine(tt_bike.First().id);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            return bike_id;
        }

        //查询密码，找不到返回空串
        public static string getPassword(int user_type, string user_id)//0顾客，1管理员，2调度员
        {
            try
            {
                DBDataContext dc = new DBDataContext();//创建数据库对象
                switch (user_type)
                {
                    case 0://顾客
                        {
                            IQueryable<string> query = from p in dc.customer
                                                       where p.id == user_id
                                                       select p.password;
                            return query.First();
                        }
                    case 1://管理员
                        {
                            IQueryable<string> query = from p in dc.manager
                                                       where p.id == user_id
                                                       select p.password;
                            return query.First();
                        }
                    case 2://调度员
                        {
                            IQueryable<string> query = from p in dc.dispatcher
                                                       where p.id == user_id
                                                       select p.password;
                            return query.First();
                        }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "";
        }

        //注册：将新顾客入库(成功返回true，失败返回false)
        public static bool register(string t_id, string t_name, string t_password)
        {
            try
            {
                DBDataContext dc = new DBDataContext();//创建数据库对象
                //查询id是否存在
                int idCount = (from p in dc.customer
                              where t_id == p.id
                              select p).Count();
                if (idCount > 0) return false;
                //生成新记录
                customer new_customer = new customer() { id = t_id.ToString().PadLeft(5, '0'), password = t_password, nickname = t_name };
                dc.customer.InsertOnSubmit(new_customer);
                dc.SubmitChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return true;
        }

        //查询任务
        public static ArrayList getTask(ArrayList processing, int condition)
        {
            //任务类型(需要的==tag， 不需要的==0)
            int is_fix, is_dispatch, is_putin,is_recycle;
            if(condition == 0) return null;
            is_fix = (condition / 1000)*1;
            is_dispatch = ((condition % 1000) / 100)*2;
            is_putin = ((condition % 100) / 10)*3;
            is_recycle = (condition % 10)*4 ;

            ArrayList tasks=new ArrayList();
            string task;
            try
            {
                DBDataContext dc = new DBDataContext();
                //搜索当前正在处理且符合条件的任务
                for(int i = 0; i < processing.Count; i++)
                {
                    var query = from p in dc.task
                                            where p.id == (int)processing[i]
                                            select p;
                    if (query.First().tag == is_fix || query.First().tag == is_dispatch ||
                        query.First().tag == is_putin || query.First().tag == is_recycle)
                    {
                        task = task_to_string(query.First());
                        tasks.Add(task);
                    }
                }
                //搜索其他符合条件的,未处理任务
                int[] t_flag=new int[4];
                t_flag[0]= is_fix; t_flag[1] = is_dispatch; t_flag[2] = is_putin; t_flag[3] = is_recycle;
                for (int i = 0; i < 4; i++)
                {
                    var q1 = from p in dc.task
                             where p.flag < 1 && (p.tag == t_flag[i])
                             select p;
                    foreach (var q in q1)
                    {
                        task = task_to_string(q);
                        tasks.Add(task);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            return tasks;
        }
        private static string task_to_string(task t)
        {
            string format_id = "{0,8:D}";//int id
            string format_point = "  ({0,5:D},{1,5:D})";
            //id
            string r = String.Format(format_id,t.id);
            //类型
            if (t.tag == 1) r += "    检修  ";
            else if (t.tag == 2) r += "    调度  ";
            else if (t.tag == 3) r += "    投放  ";
            else if (t.tag == 4) r += "    回收  ";
            //状态  
            if (t.flag == 0) r += "    未处理  ";
            else if (t.flag == 1) r += "  我在处理  ";
            else if (t.flag == 2) r += "  处理完成  ";
            //来源
            if (t.source == 1) r += "    用户  ";
            else if (t.source == 2) r += "  管理员  ";
            else if (t.source == 3) r += "    系统  ";
            //单车
            r+= String.Format(format_id, t.bid);
            //终点
            r += String.Format(format_point, t.end_x, t.end_y);
            //起始时间
            if (t.start_time.HasValue)
                r += " "+t.start_time.Value.ToString();
            else
                r += "                    ";
            //结束时间
            if (t.end_time.HasValue)
                r += " " + t.end_time.Value.ToString();
            else
                r += "                    ";
            return r;
        }
    }
}
