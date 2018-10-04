/* ==============================================================================
 * LoaderFactory
 * @author jr.zeng
 * 2017/5/18 19:59:01
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using UnityEngine;
using Object = UnityEngine.Object;

namespace mg.org
{

    public class LoaderFactory
    {
        static ClassPools m_pool = new ClassPools();

        static LoaderFactory()
        {

        }

        static public AbstractLoader GetLoader(LoadReqType type_)
        {
            AbstractLoader loader = null;
            switch (type_)
            {
                case LoadReqType.QUEUE:
                    loader = m_pool.Pop<QueueLoader>();
                    break;
                case LoadReqType.BATCH:
                    loader = m_pool.Pop<BatchLoader>();
                    break;
                case LoadReqType.LEVEL:
                    loader = m_pool.Pop<LevelLoader>();
                    break;
                case LoadReqType.DELAY:
                    loader = m_pool.Pop<DelayLoader>();
                    break;
                case LoadReqType.PROGRESS:
                    loader = m_pool.Pop<ProgLoader>();
                    break;
                default:
                    loader = m_pool.Pop<AssetLoader>();
                    break;
            }
            return loader;
        }


        static public void CloseLoader(AbstractLoader loader_)
        {
            if (loader_ == null)
                return;
            loader_.Close();
            m_pool.Push(loader_);
        }


    }
}