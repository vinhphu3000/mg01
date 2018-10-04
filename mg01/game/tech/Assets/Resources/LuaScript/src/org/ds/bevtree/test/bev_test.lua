--bevtest_config
--@author jr.zeng
--2017年4月18日 下午2:33:10
local modname = "bev_test"
--==================global reference=======================

--===================namespace========================
local ns = "org.bevtree.test"
local using = { "org.bevtree"}
local _ENV = namespace(ns, using, modname)
--===================module property========================
--===================module content========================

test_1 = function()

	local cnt = 1
	local function print(a)
		log.debug(modname, cnt, a)
		cnt = cnt + 1
	end

	local root = loop_({loop = 1},
		seq_(nil,
			callback_({callback=print, param=cnt, wait = 2}),
			goto_id_("test_2"),
			callback_({ callback=print, param=cnt, wait = 2}),
			callback_({ callback=print, param=cnt , wait = 2}),
			callback_({ callback=print, param=cnt, wait = 2 })
		)
	)

	--local root = sel_seq({
	--    childs = {
	--        wait({time = 2}),
	--        _if( __chance({ chance = 0.5 }),
	--            callback({callback=print, params="a"}) ),
	--        wait({time = 2}),
	--
	--        _if_else( __chance({ chance = 0.2 }),
	--            callback({callback=print, params="b"}),
	--
	--            _if_else( __chance({ chance =  0.2 }),
	--                callback({callback=print, params="c"}),
	--                callback({callback=print, params="b"})
	--            )
	--        ),
	--
	--        wait({time = 2}),
	--        callback({callback=print, params = cnt}),
	--        wait({time = 2}),
	--        }
	--})


	return root
	end


	test_2 = function()

		local cnt = 1
		local function print(a)
			log.debug(modname, "id2 " .. cnt)
			cnt = cnt + 1
		end

		local function reset()
			cnt = 1
		end

		local root = seq_( {
			childs = {
				callback_({callback=reset}),
				callback_({callback=print, param=cnt, wait = 1}),
				callback_({ callback=print, param=cnt, wait = 1}),
				callback_({ callback=print, param=cnt , wait = 1}),
				callback_({ callback=print, param=cnt, wait = 1 }),
			}
		})

		return root
	end

return _ENV