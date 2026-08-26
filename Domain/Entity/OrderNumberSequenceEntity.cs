using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity {
    public class OrderNumberSequenceEntity {
        public int Id { get; private set; }
        public int LastNumber { get; private set; }

        public OrderNumberSequenceEntity() { }
        
        public OrderNumberSequenceEntity(int id, int lastNumber) {
            this.Id = id;
            this.LastNumber = lastNumber;
        }

        public void Increment() { LastNumber++; }
    }
}
