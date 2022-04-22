-- ================================================
-- 改为每小时检查一次，防止提前找车
-- 增加图上不够从库中调车
-- 修改了单车游标
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- 演示备注：
-- 改datepart(weekday,getdate())=6 ，且周高峰满足但时间未到，也不会执行日高峰
-- 注意订单高峰（看视图）、任务时间、已有任务删除、围栏内单车数量要少、周围或仓库有车
-- 注意 max/a - total > 5 ?周视图订单数>6*a(周:(6+total)*36>=216; 日:(6+total)*7>42)
-- =============================================
use shareBike
go
CREATE PROCEDURE smart_dispatch

	
	
AS 
	 declare fence_cur cursor for 
		select id,origin_x,origin_y,width,height from fence where tag = 2 and score>0;--找智能调度且打开功能的围栏
BEGIN
	--变量声明（begin之前是传入的参数）
	declare
	@total int,--现在围栏中的总单车数量
	@need int,--实际需求
	@need_max int,
	@fence_id int,
	@fence_x int,
	@fence_y int,
	@fence_width int,
	@fence_height int,
	@delta int	,	--变化量
	@bike_id int--查询到的单车



	-- 检索所有的智能区获得位置
	open fence_cur;--打开游标
	fetch next from fence_cur into @fence_id,@fence_x,@fence_y,@fence_width,@fence_height;--移动游标取值

	while @@FETCH_STATUS = 0 --游标有值（对于每一个支持智能调度的围栏）
	begin
		--取现有单车数
		set @total = (select count(*) --围栏内的单车总量
					from bike 
					where current_x>@fence_x and current_x<@fence_x+@fence_width 
							and current_y>@fence_y and current_y<@fence_y+@fence_height)
		set @total += (select count(*) --任务表中未完成的end_loc在围栏中的任务数量
					from task
					where flag<2 and (tag=2 or tag=3) and
						end_x>@fence_x and end_x<@fence_x+@fence_width 
						and end_y>@fence_y and end_y<@fence_y+@fence_height)

						
		
		
		if datepart(weekday,getdate())=6 --每执行周高峰策略
		begin
			--查询周视图
			declare @view_name nvarchar(20) = 'view_'+ cast(@fence_id as varchar)+'_weekday_1'
			exec('sp_refreshview '+@view_name)--刷新视图
			declare @s1 nvarchar(200)
			set @s1 = 'select @need_max = max(need) from '+@view_name
			EXEC sp_executeSql @s1, N'@need_max int OUTPUT',@need_max=@need_max  OUTPUT --max(need)

			set @s1 = 'select @need = sum(need) from '+@view_name
			EXEC sp_executeSql @s1, N'@need int OUTPUT',@need=@need OUTPUT	--sum(need)

			--分析
			if @need_max > @need/3 --如果高峰用量 > 总用量*1/3，执行周高峰策略
			begin
				set @delta = @need_max/(12) - @total--视图共有3*7天数据，每天24小时
				--确定时间（偏移量）
				set @s1 = 'select @need = period from '+@view_name+' where need='+cast(@need_max as varchar)
				EXEC sp_executeSql @s1, N'@need int OUTPUT',@need=@need OUTPUT	--这里@need是高峰日期的偏移量

				if @delta>5 and @need = 1 -- 明天(先从图上找)
				begin
					-- 声明图上单车游标（生成任务的准备）
					declare bike_cur1 cursor for 
						select id from bike --找围栏附近200m车的id
						where current_x>@fence_x-200 and current_x<@fence_x+@fence_width+200 
							and current_y>@fence_y-200 and current_y<@fence_y+@fence_height+200
							and (current_x<@fence_x or current_x>@fence_x+@fence_width)
							and (current_y<@fence_y or current_y>@fence_y+@fence_height)
							and flag = 0;
					open bike_cur1;--打开游标
					fetch next from bike_cur1 into @bike_id;--移动游标取值
					while @@FETCH_STATUS = 0 and @delta>0
					begin
						--生成任务
						insert into task(tag,source , flag,end_x,end_y,start_time ,bid)
							values(3,3,0,(@fence_x+@fence_x+@fence_width)/2, (@fence_y+@fence_y+@fence_height)/2, dateadd(day,(@need+6)%7,GETDATE()),@bike_id)--下周这天前
						--单车待调度
						update  bike set flag=1 where id=@bike_id
						set @delta -= 1;
						fetch next from bike_cur1 into @bike_id;--移动游标取值
					end--图上找车结束
					close   bike_cur1  --关闭内层游标
					deallocate   bike_cur1 --删除内层游标
					if @delta>0  -- 不够从库里找车
					begin
						-- 声明库内单车游标(图上不够则从车库调出)
						declare bike_cur2 cursor for
							select id from bike
							where flag=5;--找库内单车
						open bike_cur2;
						fetch next from bike_cur2 into @bike_id;--移动游标取值
						-- 开始找车
						while @@FETCH_STATUS = 0 and @delta>0
						begin
							--生成任务
							insert into task(tag,source , flag,end_x,end_y,start_time ,bid)
								values(3,3,0,(@fence_x+@fence_width)/2, (@fence_y+@fence_height)/2, dateadd(day,(@need+6)%7,GETDATE()),@bike_id)--下周这天前
							--单车调度中
							update  bike set flag=4 where id=@bike_id
							set @delta -= 1;
							fetch next from bike_cur2 into @bike_id;--移动游标取值
						end
						-- 关删游标
						close   bike_cur2  --关闭内层游标
						deallocate   bike_cur2 --删除内层游标
					end--库内调车结束
				end--调车结束
			end--高峰分析结束
		end--周高峰策略结束
		else 
		begin
			--查询天视图
			set @view_name  = 'view_'+ cast(@fence_id as varchar)+'_hour_1'
			exec('sp_refreshview '+@view_name)--刷新视图
			set @s1 = 'select @need_max = max(need) from '+@view_name
			EXEC sp_executeSql @s1, N'@need_max int OUTPUT',@need_max=@need_max  OUTPUT --max(need)
			set @s1 = 'select @need = sum(need) from '+@view_name
			EXEC sp_executeSql @s1, N'@need int OUTPUT',@need=@need OUTPUT	--sum(need)

			if @need_max > @need/3 --如果某时段 > 总*1/3，
			begin
				set @delta = (@need_max/7-@total)--视图共有7天数据
				--确定时间（偏移量）
				set @s1 = 'select @need = period from '+@view_name+' where need='+cast(@need_max as varchar)
				EXEC sp_executeSql @s1, N'@need int OUTPUT',@need=@need OUTPUT	
				if @delta>5 and @need = 2	--从附近调度
				begin
					-- 声明图上单车游标（生成任务的准备）
					declare bike_cur1 cursor for 
						select id from bike --找围栏附近200m车的id
						where current_x>@fence_x-200 and current_x<@fence_x+@fence_width+200 
							and current_y>@fence_y-200 and current_y<@fence_y+@fence_height+200
							and (current_x<@fence_x or current_x>@fence_x+@fence_width)
							and (current_y<@fence_y or current_y>@fence_y+@fence_height)
							and flag = 0;
					open bike_cur1;--打开游标
					fetch next from bike_cur1 into @bike_id;--移动游标取值
					while @@FETCH_STATUS = 0 and @delta>0
					begin
						--生成任务
						insert into task(tag,source , flag,end_x,end_y,start_time ,bid)
							values(3,3,0,(@fence_x+@fence_width)/2, (@fence_y+@fence_height)/2,dateadd(hour,(@need+22)%24,GETDATE()),@bike_id)--第二天前2小 时调车
						--单车待调度
						update  bike set flag=1 where id=@bike_id
						set @delta -= 1;
						fetch next from bike_cur1 into @bike_id;--移动游标取值
					end--图上调车结束
					close   bike_cur1  --关闭内层游标
					deallocate   bike_cur1 --删除内层游标
					if @delta>0-- 不够从库里找车
					begin
						-- 声明库内单车游标(图上不够则从车库调出)
						declare bike_cur2 cursor for
							select id from bike
							where flag=5;--找库内单车
						open bike_cur2;
						fetch next from bike_cur2 into @bike_id;--移动游标取值
						-- 开始找车
						while @@FETCH_STATUS = 0 and @delta>0
						begin
							--生成任务
							insert into task(tag,source , flag,end_x,end_y,start_time ,bid)
								values(3,3,0,(@fence_x+@fence_width)/2, (@fence_y+@fence_height)/2,dateadd(hour,(@need+22)%24,GETDATE()),@bike_id)--第二天前2小 时调车
							--单车调度中
							update  bike set flag=4 where id=@bike_id
							set @delta -= 1;
							fetch next from bike_cur2 into @bike_id;--移动游标取值
						end -- 生成任务结束
						-- 关删游标
						close   bike_cur2  --关闭内层游标
						deallocate   bike_cur2 --删除内层游标
					end--库内调车结束
				end--调车结束
			end--高峰分析结束
		end--天高峰策略结束

		

	
				
		fetch next from fence_cur into @fence_id,@fence_x,@fence_y,@fence_width,@fence_height;--移动游标取围栏数据
	end
	
	--关闭围栏游标
	close   fence_cur  
	deallocate   fence_cur

END
GO
