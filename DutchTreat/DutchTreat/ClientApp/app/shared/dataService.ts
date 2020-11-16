import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Order, OrderItem } from './order';
import { Product } from './product';

@Injectable()
export class DataService {
    constructor(private http: HttpClient) { }

    private token: string = '';
    private tokenExpiration: Date;

    public order: Order = new Order();

    public products: Product[] = [];

    loadProducts(): Observable<boolean> {
        return this.http
            .get('/api/products')
            .pipe(map((data: any[]) => {
               this.products = data;
               return true;
            }));
    }

    public get loginRequired(): boolean {
        return this.token.length == 0 || this.tokenExpiration > new Date();
    }

    login(credentials): Observable<boolean> {
        return this.http
            .post("/account/createtoken", credentials)
            .pipe(map((data: any) => {
               this.token = data.token;
               this.tokenExpiration = data.tokenExpiration;
               return true;
            }));
    }

    public checkout() {
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

    public AddToOrder(product: Product) {
        var item: OrderItem = this.order.items.find(item => item.productId == product.id);

        if (item) {
            item.quantity++;
        } else {
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
}