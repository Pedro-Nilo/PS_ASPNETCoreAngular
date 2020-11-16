import { __decorate } from "tslib";
import { Component } from '@angular/core';
let Login = class Login {
    constructor(data, router) {
        this.data = data;
        this.router = router;
        this.errorMessage = "";
        this.credentials = {
            username: "",
            password: ""
        };
    }
    onLogin() {
        this.data.login(this.credentials)
            .subscribe(success => {
            if (success) {
                if (this.data.order.items.length == 0) {
                    this.router.navigate(['']);
                }
                else {
                    this.router.navigate(['checkout']);
                }
            }
        }, error => this.errorMessage = 'Failed to login');
    }
};
Login = __decorate([
    Component({
        selector: 'login',
        templateUrl: 'login.component.html'
    })
], Login);
export { Login };
//# sourceMappingURL=login.component.js.map