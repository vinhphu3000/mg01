/* ==============================================================================
 * ISubject
 * @author jr.zeng
 * 2016/8/23 16:15:53
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mg.org
{
    public interface ISubject
    {
        void Attach(string type_, CALLBACK_1 callback_, object refer_);
        void Detach(string type_, CALLBACK_1 callback_);
        void DetachByType(string type_);
        void DetachAll();

        bool Notify(string type_, object data_);
        bool NotifyEvent(SubjectEvent evt_);
        bool NotifyWithEvent(string type_, object data_);

    }
}
