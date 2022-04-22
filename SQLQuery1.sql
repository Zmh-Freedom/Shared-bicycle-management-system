use shareBike
go
--drop procedure smart_dispatch--删除存储过程
--smart_dispatch--调用存储过程
--select * from task where id>222--查新生成的任务
--delete task from task where id>222--删除新生成的任务
--update bike set flag=5 where id<20 --添加可用单车（前19辆置“5”）
--select * from orderform where id>4590--查看新加入的订单
--delete orderform from orderform where id>4595--删除新添加的订单
--insert orderform(cid,bid,flag,start_x,start_y,end_x,end_y,start_time,end_time,cost)values--插入订单
