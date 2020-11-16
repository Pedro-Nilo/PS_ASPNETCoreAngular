import * as _ from 'lodash-es';
export class Order {
    constructor() {
        this.orderDate = new Date();
        this.items = new Array();
    }
    get subtotal() {
        return _.sum(_.map(this.items, item => item.unitPrice * item.quantity));
    }
    ;
}
export class OrderItem {
}
//# sourceMappingURL=order.js.map