using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WrenBot.Types;

namespace WrenBot
{
    public class Bot : MarshalByRefObject
    {        
        public virtual void Start() { }
        public virtual void Stop() { }
        public virtual void OnSpellBar() { }
        public virtual void OnAnimation() { }
    }
}