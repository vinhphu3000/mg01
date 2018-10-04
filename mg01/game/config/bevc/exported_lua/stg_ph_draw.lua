-- EXPORTED BY TOOL, DON'T MODIFY IT!
-- Source File: stg_ph_draw.xml
return {
    behavior = {
        name = "stg_ph_draw.xml",
        agentType = "stage_mgr",
        version = "5",
        node = {
            {
                class = "_seq_",
                id = 0,
                node = {
                    {
                        class = "_wait_",
                        id = 1,
                        time = 1,
                        timeType = 0,
                    },
                    {
                        class = "_agent_action_",
                        id = 2,
                        CustomClass = "_act_each_player_",
                        proc_id = "player_in_ph_draw",
                        proc_idType = 0,
                        resultOpt = 2,
                    },
                },
            },
        },
    },
}
