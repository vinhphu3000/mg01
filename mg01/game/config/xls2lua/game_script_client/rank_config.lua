
local function main(ms)
    local data = ms['排行榜']
    local title = ms['栏目类型']
    local update_ty = ms['更新类型']
    local srv_info = ms['排行榜榜单']
    local rank_title = ms['排行榜主榜单']

    local update_type = update_ty

    local rank_client_name2title = {}
    local rank_ty2name = {}
    local rank_client_name2ty = {}
    local rank_titleid2showname = {}
    local rank_titleid2showtype = {}
    
    -- 转换排行榜信息
    local rank_srv_info = {}
    if srv_info then
        for i, v in pairs(srv_info) do
            rank_srv_info[v.id] = {
                server_sproto_name = v.server_sproto_name,
                const_name = v.const_name,
            }
            if #v.server_sproto_params > 0 then
                rank_srv_info[v.id].server_sproto_params = v.server_sproto_params
            end
        end
    end

    for _, v in pairs(rank_title) do
        rank_ty2name[v.id] = v.name
        rank_client_name2ty[v.client_name] = v.id
    end

    for _, v in pairs(title) do
        rank_client_name2title[v.client_name] = v.id
        rank_titleid2showtype[v.id] = v.show_type
        rank_titleid2showname[v.id] = v.show_name
    end
    local res = {
	    update_type = update_type,
	    rank_info = data,
        srv_info = rank_srv_info,
        rank_client_name2ty = rank_client_name2ty,
        rank_ty2name = rank_ty2name,
        rank_titleid2showtype = rank_titleid2showtype,
        rank_titleid2showname = rank_titleid2showname,
        rank_client_name2title = rank_client_name2title,
	}
    return res
end

return main
