using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsap.Domain.Events
{
    // ==========================================
    // DOMAIN EVENT BASE
    // ==========================================
    public abstract class DomainEvent
    {
        public DateTime OcurridoEn { get; protected set; }

        protected DomainEvent()
        {
            OcurridoEn = DateTime.Now;
        }
    }

}
