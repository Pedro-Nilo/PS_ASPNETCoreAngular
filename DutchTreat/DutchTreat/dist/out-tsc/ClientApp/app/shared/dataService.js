import { __decorate } from "tslib";
import { HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { Order, OrderItem } from './order';
let DataService = class DataService {
    constructor(http) {
        this.http = http;
        this.token = '';
        this.order = new Order();
        this.products = [];
    }
    loadProducts() {
        return this.http
            .get('/api/products')
            .pipe(map((data) => {
            this.products = data;
            return true;
        }));
    }
    get loginRequired() {
        return this.token.length == 0 || this.tokenExpiration > new Date();
    }
    login(credentials) {
        return this.http
            .post("/account/createtoken", credentials)
            .pipe(map((data) => {
            this.token = data.token;
            this.tokenExpiration = data.tokenExpiration;
            return true;
        }));
    }
    checkout() {
        if (!this.order.orderNumber) {
            this.order.orderNumber = this.order.orderDate.getFullYear().toString() + this.order.orderDate.getTime().toString();
        }
        return this.http
            .post("/api/orders", this.order, {
            headers: new HttpHeaders().set("Authorization", "Bearer " + this.token)
        })
            .pipe(map(response => {
            this.order = new Order();
            return true;
        }));
    }
    AddToOrder(product) {
        var item = this.order.items.find(item => item.productId == product.id);
        if (item) {
            item.quantity++;
        }
        else {
            item = new OrderItem();
            item.productId = product.id;
            item.productArtist = product.artist;
            item.productArtId = product.artId;
            item.productCategory = product.category;
            item.productSize = product.size;
            item.productTitle = product.title;
            item.unitPrice = product.price;
            item.quantity = 1;
            this.order.items.push(item);
        }
    }
};
DataService = __decorate([
    Injectable()
], DataService);
export { DataService };
//# sourceMappingURL=dataService.js.map