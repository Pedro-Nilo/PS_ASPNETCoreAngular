class StoreCustomer {
    constructor(firstName, lastName) {
        this.firstName = firstName;
        this.lastName = lastName;
        this.visits = 0;
    }
    showName() {
        alert(this.firstName + ' ' + this.lastName);
    }
    set name(value) {
        this.ourName = value;
    }
    get name() {
        return this.ourName;
    }
}
//# sourceMappingURL=storecustomer.js.map