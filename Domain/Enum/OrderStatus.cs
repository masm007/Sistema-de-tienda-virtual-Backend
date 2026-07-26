using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enum {
    public enum OrderStatus {
        Pending,
        Paid,
        ReadyForPickup,
        Shipped,
        Delivered,
        Completed,
        Cancelled
    }
}
