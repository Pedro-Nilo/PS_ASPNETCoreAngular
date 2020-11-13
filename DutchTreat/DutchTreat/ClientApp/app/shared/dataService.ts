﻿import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Order, OrderItem } from './order';
import { Product } from './product';

@Injectable()
export class DataService {
    constructor(private http: HttpClient) { }

    public order: Order = new Order();

    public products: Product[] = [];

    loadProducts(): Observable<boolean> {
        return this.http.get('/api/products')
            .pipe(
               map((data: any[]) => {
                   this.products = data;
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