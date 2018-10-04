
local function main(ms)

	local ret = {}

	for id, item in pairs(ms['魂卡秘境']) do

		local cp = ret[item.chapter]
		if not cp then
			cp = {chapter = item.chapter,
				name = item.chapter_name,
				sections = {},  }
			ret[item.chapter] = cp
		end

		cp.sections[item.section] = item
	end

	assert(false, '为什么啊')

	return ret
end

return main