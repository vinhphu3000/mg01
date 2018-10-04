
local function main(ms)
    local raw = ms['黑暗深渊常量']
    local ret = {}
    for k, v in pairs(raw) do
	
		if not next(v.list_value) then

			if next(v.num_value) and next(v.str_value) then
				error(string.format('%s both fill num and string value!!!', k))
			end
			if not next(v.num_value) and not next(v.str_value) then
				error(string.format('%s both num and string value are empty!!!', k))
			end

		end
	
        if next(v.num_value) then
            ret[k] = v.num_value[1]
        elseif next(v.str_value) then
            ret[k] = v.str_value[1]
		else
            ret[k] = v.list_value
        end

        --ret[k.."_desc"] = v.desc
    end

	--function ret.get_const(key)
	--	if not ret[key] then
	--		assert(false, 'miss const key: ' .. key)
	--	end
	--	return ret[key]
	--end

    return ret
end

return main
