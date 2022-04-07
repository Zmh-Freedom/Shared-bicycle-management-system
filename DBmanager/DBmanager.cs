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
            //起点
            r += String.Format(format_point, t.start_x, t.start_y);
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
