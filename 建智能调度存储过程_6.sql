-- ================================================
-- ��ΪÿСʱ���һ�Σ���ֹ��ǰ�ҳ�
-- ����ͼ�ϲ����ӿ��е���
-- �޸��˵����α�
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- ��ʾ��ע��
-- ��datepart(weekday,getdate())=6 �����ܸ߷����㵫ʱ��δ����Ҳ����ִ���ո߷�
-- ע�ⶩ���߷壨����ͼ��������ʱ�䡢��������ɾ����Χ���ڵ�������Ҫ�١���Χ��ֿ��г�
-- ע�� max/a - total > 5 ?����ͼ������>6*a(��:(6+total)*36>=216; ��:(6+total)*7>42)
-- =============================================
use shareBike
go
CREATE PROCEDURE smart_dispatch

	
	
AS 
	 declare fence_cur cursor for 
		select id,origin_x,origin_y,width,height from fence where tag = 2 and score>0;--�����ܵ����Ҵ򿪹��ܵ�Χ��
BEGIN
	--����������begin֮ǰ�Ǵ���Ĳ�����
	declare
	@total int,--����Χ���е��ܵ�������
	@need int,--ʵ������
	@need_max int,
	@fence_id int,
	@fence_x int,
	@fence_y int,
	@fence_width int,
	@fence_height int,
	@delta int	,	--�仯��
	@bike_id int--��ѯ���ĵ���



	-- �������е����������λ��
	open fence_cur;--���α�
	fetch next from fence_cur into @fence_id,@fence_x,@fence_y,@fence_width,@fence_height;--�ƶ��α�ȡֵ

	while @@FETCH_STATUS = 0 --�α���ֵ������ÿһ��֧�����ܵ��ȵ�Χ����
	begin
		--ȡ���е�����
		set @total = (select count(*) --Χ���ڵĵ�������
					from bike 
					where current_x>@fence_x and current_x<@fence_x+@fence_width 
							and current_y>@fence_y and current_y<@fence_y+@fence_height)
		set @total += (select count(*) --�������δ��ɵ�end_loc��Χ���е���������
					from task
					where flag<2 and (tag=2 or tag=3) and
						end_x>@fence_x and end_x<@fence_x+@fence_width 
						and end_y>@fence_y and end_y<@fence_y+@fence_height)

						
		
		
		if datepart(weekday,getdate())=6 --ÿִ���ܸ߷����
		begin
			--��ѯ����ͼ
			declare @view_name nvarchar(20) = 'view_'+ cast(@fence_id as varchar)+'_weekday_1'
			exec('sp_refreshview '+@view_name)--ˢ����ͼ
			declare @s1 nvarchar(200)
			set @s1 = 'select @need_max = max(need) from '+@view_name
			EXEC sp_executeSql @s1, N'@need_max int OUTPUT',@need_max=@need_max  OUTPUT --max(need)

			set @s1 = 'select @need = sum(need) from '+@view_name
			EXEC sp_executeSql @s1, N'@need int OUTPUT',@need=@need OUTPUT	--sum(need)

			--����
			if @need_max > @need/3 --����߷����� > ������*1/3��ִ���ܸ߷����
			begin
				set @delta = @need_max/(12) - @total--��ͼ����3*7�����ݣ�ÿ��24Сʱ
				--ȷ��ʱ�䣨ƫ������
				set @s1 = 'select @need = period from '+@view_name+' where need='+cast(@need_max as varchar)
				EXEC sp_executeSql @s1, N'@need int OUTPUT',@need=@need OUTPUT	--����@need�Ǹ߷����ڵ�ƫ����

				if @delta>5 and @need = 1 -- ����(�ȴ�ͼ����)
				begin
					-- ����ͼ�ϵ����α꣨���������׼����
					declare bike_cur1 cursor for 
						select id from bike --��Χ������200m����id
						where current_x>@fence_x-200 and current_x<@fence_x+@fence_width+200 
							and current_y>@fence_y-200 and current_y<@fence_y+@fence_height+200
							and (current_x<@fence_x or current_x>@fence_x+@fence_width)
							and (current_y<@fence_y or current_y>@fence_y+@fence_height)
							and flag = 0;
					open bike_cur1;--���α�
					fetch next from bike_cur1 into @bike_id;--�ƶ��α�ȡֵ
					while @@FETCH_STATUS = 0 and @delta>0
					begin
						--��������
						insert into task(tag,source , flag,end_x,end_y,start_time ,bid)
							values(3,3,0,(@fence_x+@fence_x+@fence_width)/2, (@fence_y+@fence_y+@fence_height)/2, dateadd(day,(@need+6)%7,GETDATE()),@bike_id)--��������ǰ
						--����������
						update  bike set flag=1 where id=@bike_id
						set @delta -= 1;
						fetch next from bike_cur1 into @bike_id;--�ƶ��α�ȡֵ
					end--ͼ���ҳ�����
					close   bike_cur1  --�ر��ڲ��α�
					deallocate   bike_cur1 --ɾ���ڲ��α�
					if @delta>0  -- �����ӿ����ҳ�
					begin
						-- �������ڵ����α�(ͼ�ϲ�����ӳ������)
						declare bike_cur2 cursor for
							select id from bike
							where flag=5;--�ҿ��ڵ���
						open bike_cur2;
						fetch next from bike_cur2 into @bike_id;--�ƶ��α�ȡֵ
						-- ��ʼ�ҳ�
						while @@FETCH_STATUS = 0 and @delta>0
						begin
							--��������
							insert into task(tag,source , flag,end_x,end_y,start_time ,bid)
								values(3,3,0,(@fence_x+@fence_width)/2, (@fence_y+@fence_height)/2, dateadd(day,(@need+6)%7,GETDATE()),@bike_id)--��������ǰ
							--����������
							update  bike set flag=4 where id=@bike_id
							set @delta -= 1;
							fetch next from bike_cur2 into @bike_id;--�ƶ��α�ȡֵ
						end
						-- ��ɾ�α�
						close   bike_cur2  --�ر��ڲ��α�
						deallocate   bike_cur2 --ɾ���ڲ��α�
					end--���ڵ�������
				end--��������
			end--�߷��������
		end--�ܸ߷���Խ���
		else 
		begin
			--��ѯ����ͼ
			set @view_name  = 'view_'+ cast(@fence_id as varchar)+'_hour_1'
			exec('sp_refreshview '+@view_name)--ˢ����ͼ
			set @s1 = 'select @need_max = max(need) from '+@view_name
			EXEC sp_executeSql @s1, N'@need_max int OUTPUT',@need_max=@need_max  OUTPUT --max(need)
			set @s1 = 'select @need = sum(need) from '+@view_name
			EXEC sp_executeSql @s1, N'@need int OUTPUT',@need=@need OUTPUT	--sum(need)

			if @need_max > @need/3 --���ĳʱ�� > ��*1/3��
			begin
				set @delta = (@need_max/7-@total)--��ͼ����7������
				--ȷ��ʱ�䣨ƫ������
				set @s1 = 'select @need = period from '+@view_name+' where need='+cast(@need_max as varchar)
				EXEC sp_executeSql @s1, N'@need int OUTPUT',@need=@need OUTPUT	
				if @delta>5 and @need = 2	--�Ӹ�������
				begin
					-- ����ͼ�ϵ����α꣨���������׼����
					declare bike_cur1 cursor for 
						select id from bike --��Χ������200m����id
						where current_x>@fence_x-200 and current_x<@fence_x+@fence_width+200 
							and current_y>@fence_y-200 and current_y<@fence_y+@fence_height+200
							and (current_x<@fence_x or current_x>@fence_x+@fence_width)
							and (current_y<@fence_y or current_y>@fence_y+@fence_height)
							and flag = 0;
					open bike_cur1;--���α�
					fetch next from bike_cur1 into @bike_id;--�ƶ��α�ȡֵ
					while @@FETCH_STATUS = 0 and @delta>0
					begin
						--��������
						insert into task(tag,source , flag,end_x,end_y,start_time ,bid)
							values(3,3,0,(@fence_x+@fence_width)/2, (@fence_y+@fence_height)/2,dateadd(hour,(@need+22)%24,GETDATE()),@bike_id)--�ڶ���ǰ2С ʱ����
						--����������
						update  bike set flag=1 where id=@bike_id
						set @delta -= 1;
						fetch next from bike_cur1 into @bike_id;--�ƶ��α�ȡֵ
					end--ͼ�ϵ�������
					close   bike_cur1  --�ر��ڲ��α�
					deallocate   bike_cur1 --ɾ���ڲ��α�
					if @delta>0-- �����ӿ����ҳ�
					begin
						-- �������ڵ����α�(ͼ�ϲ�����ӳ������)
						declare bike_cur2 cursor for
							select id from bike
							where flag=5;--�ҿ��ڵ���
						open bike_cur2;
						fetch next from bike_cur2 into @bike_id;--�ƶ��α�ȡֵ
						-- ��ʼ�ҳ�
						while @@FETCH_STATUS = 0 and @delta>0
						begin
							--��������
							insert into task(tag,source , flag,end_x,end_y,start_time ,bid)
								values(3,3,0,(@fence_x+@fence_width)/2, (@fence_y+@fence_height)/2,dateadd(hour,(@need+22)%24,GETDATE()),@bike_id)--�ڶ���ǰ2С ʱ����
							--����������
							update  bike set flag=4 where id=@bike_id
							set @delta -= 1;
							fetch next from bike_cur2 into @bike_id;--�ƶ��α�ȡֵ
						end -- �����������
						-- ��ɾ�α�
						close   bike_cur2  --�ر��ڲ��α�
						deallocate   bike_cur2 --ɾ���ڲ��α�
					end--���ڵ�������
				end--��������
			end--�߷��������
		end--��߷���Խ���

		

	
				
		fetch next from fence_cur into @fence_id,@fence_x,@fence_y,@fence_width,@fence_height;--�ƶ��α�ȡΧ������
	end
	
	--�ر�Χ���α�
	close   fence_cur  
	deallocate   fence_cur

END
GO
